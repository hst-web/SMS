/*----------------------------------------------------------------

// 文件名：DBHelper.cs
// 功能描述：通用数据库访问类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace HST.Utillity
{
    /// <summary>
    /// 通用数据库访问类。
    /// </summary>
    public sealed class DBHelper
    {
        public string ConnectionString { get; private set; }
        private DbProviderFactory _providerFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="providerType">数据库类型</param>
        public DBHelper(string connectionString, DbProviderType providerType)
        {
            ConnectionString = connectionString;
            _providerFactory = ProviderFactory.GetDbProviderFactory(providerType);
            if (_providerFactory == null)
            {
                throw new ArgumentException("Can't load DbProviderFactory for given value of providerType");
            }
        }

        #region 事务处理
        private DbTransaction _trans;//事务
        private DbConnection _connection;

        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTrans()
        {
            if (_trans == null)
            {
                if (_connection == null)
                {
                    _connection = _providerFactory.CreateConnection();
                }
                _connection.ConnectionString = ConnectionString;
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                _trans = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);

            }
        }

        /// <summary>   
        /// 在事务中,对数据库执行增删改操作，返回受影响的行数(需先开启事务)。   
        /// </summary>   
        /// <param name="commandText">Sql语句或SP名称</param>   
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQueryInTrans(string commandText, IList<DbParameter> parameters)
        {
            int affectedRows = 0;

            try
            {
                DbCommand command = _providerFactory.CreateCommand();
                if (_connection == null)
                {
                    _connection = _providerFactory.CreateConnection();
                }
                command.Connection = _connection;
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;
                command.Transaction = _trans;
                if (!(parameters == null || parameters.Count == 0))
                {
                    foreach (DbParameter parameter in parameters)
                    {
                        if (!command.Parameters.Contains(parameter))
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                }
                affectedRows = command.ExecuteNonQuery();

                if (affectedRows <= 0)
                {
                    affectedRows = 1;
                }

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                Logger.Info(this, ex.Message, ex);
            }


            return affectedRows;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTrans()
        {
            if (_trans != null)
            {
                _trans.Commit();
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                this._trans = null;
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBack()
        {
            if (_trans != null)
            {
                _trans.Rollback();
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                this._trans = null;
            }
        }
        #endregion

        /// <summary>   
        /// 对数据库执行增删改操作，返回受影响的行数。   
        /// </summary>   
        /// <param name="commandText">Sql语句</param>   
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>  
        public int ExecuteNonQuery(string commandText, IList<DbParameter> parameters)
        {
            return ExecuteNonQuery(commandText, parameters, CommandType.Text);
        }

        /// <summary>   
        /// 对数据库执行增删改操作，返回受影响的行数。   
        /// </summary>   
        /// <param name="commandText">Sql语句或SP名称</param>   
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(commandText, parameters, commandType))
            {
                command.Connection.Open();
                int affectedRows = command.ExecuteNonQuery();
                command.Parameters.Clear();
                command.Connection.Close();

                return affectedRows;
            }
        }


        /// <summary>   
        /// 执行一个查询语句，返回一个关联的DataReader实例   
        /// </summary>   
        /// <param name="commandText">Sql语句</param>   
        /// <param name="parameters">命令参数</param>
        /// <returns></returns> 
        public DbDataReader ExecuteReader(string commandText, IList<DbParameter> parameters)
        {
            return ExecuteReader(commandText, parameters, CommandType.Text);
        }

        /// <summary>
        /// 执行一个查询语句，返回一个关联的DataReader实例
        /// </summary>
        /// <param name="commandText">Sql语句或SP名称</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText, IList<DbParameter> parameters, CommandType commandType)
        {
            DbCommand command = CreateDbCommand(commandText, parameters, commandType);
            command.Connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 执行一个查询语句，返回一个包含查询结果的DataTable
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, IList<DbParameter> parameters)
        {
            return ExecuteDataTable(commandText, parameters, CommandType.Text);
        }

        /// <summary>
        /// 执行一个查询语句，返回一个包含查询结果的DataTable
        /// </summary>
        /// <param name="commandText">Sql语句或SP名称</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(commandText, parameters, commandType))
            {
                using (DbDataAdapter adapter = _providerFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    command.Parameters.Clear();
                    return data;
                }
            }
        }

        /// <summary>
        /// 执行一个查询语句，返回查询结果的第一行第一列
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public Object ExecuteScalar(string commandText, IList<DbParameter> parameters)
        {
            return ExecuteScalar(commandText, parameters, CommandType.Text);
        }

        /// <summary>
        /// 执行一个查询语句，返回查询结果的第一行第一列
        /// </summary>
        /// <param name="commandText">Sql语句或SP名称</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public Object ExecuteScalar(string commandText, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(commandText, parameters, commandType))
            {
                command.Connection.Open();
                object result = command.ExecuteScalar();
                command.Parameters.Clear();
                command.Connection.Close();

                return result;
            }
        }

        /// <summary>
        /// 创建一个DbCommand对象
        /// </summary>
        /// <param name="commandText">Sql语句或SP名称</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        private DbCommand CreateDbCommand(string commandText, IList<DbParameter> parameters, CommandType commandType)
        {
            DbConnection connection = _providerFactory.CreateConnection();
            connection.ConnectionString = ConnectionString;

            DbCommand command = _providerFactory.CreateCommand();
            command.Connection = connection;
            command.CommandType = commandType;
            command.CommandText = commandText;
            if (!(parameters == null || parameters.Count == 0))
            {
                foreach (DbParameter parameter in parameters)
                {
                    parameter.Value = parameter.Value == null ? "" : parameter.Value;
                    if (!command.Parameters.Contains(parameter))
                    {
                        command.Parameters.Add(parameter);
                    }
                }
            }

            return command;
        }
    }
}
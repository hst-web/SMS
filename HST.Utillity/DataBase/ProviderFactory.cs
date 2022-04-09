/*----------------------------------------------------------------

// 文件名：ProviderFactory.cs
// 功能描述：数据库提供程序工厂类。
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System.Collections.Generic;
using System.Data.Common;

namespace HST.Utillity
{
    /// <summary>
    /// 数据库提供程序工厂类。
    /// </summary>
    public class ProviderFactory
    {
        private static Dictionary<DbProviderType, string> _providerInvariantNames = new Dictionary<DbProviderType, string>();
        private static Dictionary<DbProviderType, DbProviderFactory> _providerFactoies = new Dictionary<DbProviderType, DbProviderFactory>(20);

        private static object obj = new object();//锁对象使用
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ProviderFactory()
        {
            //加载已知的数据库访问类的程序集
            _providerInvariantNames.Add(DbProviderType.SqlServer, "System.Data.SqlClient");
            _providerInvariantNames.Add(DbProviderType.OleDb, "System.Data.OleDb");
            _providerInvariantNames.Add(DbProviderType.ODBC, "System.Data.ODBC");
            _providerInvariantNames.Add(DbProviderType.Oracle, "Oracle.DataAccess.Client");
            _providerInvariantNames.Add(DbProviderType.MySql, "MySql.Data.MySqlClient");
            _providerInvariantNames.Add(DbProviderType.SQLite, "System.Data.SQLite");
            _providerInvariantNames.Add(DbProviderType.Firebird, "FirebirdSql.Data.Firebird");
            _providerInvariantNames.Add(DbProviderType.PostgreSql, "Npgsql");
            _providerInvariantNames.Add(DbProviderType.DB2, "IBM.Data.DB2.iSeries");
            _providerInvariantNames.Add(DbProviderType.Informix, "IBM.Data.Informix");
            _providerInvariantNames.Add(DbProviderType.SqlServerCe, "System.Data.SqlServerCe");
        }

        /// <summary>
        /// 获取指定数据库类型对应的程序集名称
        /// </summary>
        /// <param name="providerType">数据库类型枚举</param>
        /// <returns></returns>
        public static string GetProviderInvariantName(DbProviderType providerType)
        {
            return _providerInvariantNames[providerType];
        }

        /// <summary>
        /// 获取指定类型的数据库对应的DbProviderFactory
        /// </summary>
        /// <param name="providerType">数据库类型枚举</param>
        /// <returns></returns>
        public static DbProviderFactory GetDbProviderFactory(DbProviderType providerType)
        {
            lock (obj)
            {
                //如果还没有加载，则加载该DbProviderFactory
                if (!_providerFactoies.ContainsKey(providerType))
                {
                    _providerFactoies.Add(providerType, ImportDbProviderFactory(providerType));
                }
            }
            return _providerFactoies[providerType];
        }

        /// <summary>
        /// 加载指定数据库类型的DbProviderFactory
        /// </summary>
        /// <param name="providerType">数据库类型枚举</param>
        /// <returns></returns>
        private static DbProviderFactory ImportDbProviderFactory(DbProviderType providerType)
        {
            string providerName = _providerInvariantNames[providerType];
            DbProviderFactory factory = null;
            try
            {
                //从全局程序集中查找
                factory = DbProviderFactories.GetFactory(providerName);
            }
            catch
            {
                factory = null;
            }
            return factory;
        }
    }

    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DbProviderType : byte
    {
        SqlServer,
        MySql,
        SQLite,
        Oracle,
        ODBC,
        OleDb,
        Firebird,
        PostgreSql,
        DB2,
        Informix,
        SqlServerCe
    }
}

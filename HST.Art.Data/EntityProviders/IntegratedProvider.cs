/*----------------------------------------------------------------
// 文件名：IntegratedProvider.cs
// 功能描述： 综合数据提供者
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using HST.Art.Core;
using HST.Utillity;
using System.Text;
using System.Linq;
using System.Data;
using Newtonsoft.Json;

namespace HST.Art.Data
{
    public class IntegratedProvider : EntityProviderBase
    {
        #region 查询设置
        /// <summary>
        /// 根据ID获取设置信息
        /// </summary>
        /// <param name="id">设置ID</param>
        /// <returns>设置信息</returns>
        public Setting GetSetting(SettingType setType)
        {
            Setting settingInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT Id, Type, Val, CreateDate  from Setting where IsEnabled=1 and Type=@Type ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Type", (int)setType));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    settingInfo = GetSettingFromReader(reader);
                }
            }

            return settingInfo;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Setting GetSettingFromReader(DbDataReader reader)
        {
            Setting settingInfo = new Setting();
            settingInfo.Id = Convert.ToInt32(reader["Id"]);
            settingInfo.Val = reader["Val"].ToString();
            settingInfo.Type = (SettingType)reader["Type"];
            settingInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

            return settingInfo;
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 修改设置
        /// </summary>
        /// <param name="setInfo">设置信息</param>
        /// <returns>修改成功标识</returns>
        public bool UpdateSetting(Setting setInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update Setting
                              Set [Type]=@Type
                                  ,[Val]=@Val
                                  ,[IsEnabled]=@IsEnabled
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", setInfo.Id));
            parametersList.Add(new SqlParameter("@IsEnabled", Convert.ToInt32(setInfo.IsEnabled)));
            parametersList.Add(new SqlParameter("@Val", setInfo.Val));
            parametersList.Add(new SqlParameter("@Type", (int)setInfo.Type));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 修改设置
        /// </summary>
        /// <param name="setInfo">设置信息</param>
        /// <returns>修改成功标识</returns>
        public bool UpdateSettingByType(Setting setInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update Setting Set [Val]=@Val  Where [Type]=@Type";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Val", setInfo.Val));
            parametersList.Add(new SqlParameter("@Type", (int)setInfo.Type));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除设置
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool DeleteSetting(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete Setting where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion

        #region 日志相关
        public SystemLog GetLog(int id)
        {
            SystemLog logInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT [Id]
                              ,[UserId]
                              ,[Type]
                              ,[Source]
                              ,[ControllerName]
                              ,[ActionName]
                              ,[UserAgent]
                              ,[ResultLog]
                              ,ReqParameter
                              ,[ClientIp]
                              ,[CreateDate]  from systemLog where  Id=@Id ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    logInfo = GetLogFromReader(reader);
                }
            }

            return logInfo;

        }

        public List<SystemLog> GetLogPage(LogQuery query, out int totalNum)
        {
            totalNum = 0;
            List<SystemLog> logList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            StringBuilder sb = new StringBuilder();

            #region 查询字符串
            if (!string.IsNullOrEmpty(query.ActionName))
            {
                sb.Append(" and ActionName like +@ActionName+'%'");
            }
            if (!string.IsNullOrEmpty(query.ControllerName))
            {
                sb.Append(" and ControllerName like +@ControllerName+'%'");
            }
            if (query.Source != LogSource.UnKnown)
            {
                sb.Append(" and Source=@Source");
            }
            if (query.Type != LogType.UnKnown)
            {
                sb.Append(" and Type=@Type");
            }
            if (!string.IsNullOrEmpty(query.StartDate))
            {
                sb.Append(" and CreateDate>=@StartDate");
            }
            if (!string.IsNullOrEmpty(query.EndDate))
            {
                sb.Append(" and CreateDate<@EndDate");
            }
            #endregion

            IList<DbParameter> parameList = new List<DbParameter>();
            parameList.Add(new SqlParameter("@pageSize", query.PageSize));
            parameList.Add(new SqlParameter("@pageIndex", query.PageIndex));
            parameList.Add(new SqlParameter("@ActionName", query.ActionName));
            parameList.Add(new SqlParameter("@ControllerName", query.ControllerName));
            parameList.Add(new SqlParameter("@Source", (int)query.Source));
            parameList.Add(new SqlParameter("@Type", (int)query.Type));
            parameList.Add(new SqlParameter("@StartDate", query.StartDate));
            parameList.Add(new SqlParameter("@EndDate", query.EndDate));

            string strSqlQuery = @"select count(ID) from [SystemLog] where 1=1 " + sb.ToString();//查询有多少条记录
            totalNum = Convert.ToInt32(dbHelper.ExecuteScalar(strSqlQuery, parameList));

            string strSql = @"SELECT [ID]
                                     ,[UserId]
                                     ,[Type]
                                     ,[Source]
                                     ,[ControllerName]
                                     ,[ActionName]
                                     ,[ClientIp]
                                     ,[CreateDate]
                            FROM (select top (@pageSize*@pageIndex)  [ID]
                                    ,[UserId]
                                     ,[Type]
                                     ,[Source]
                                     ,[ControllerName]
                                     ,[ActionName]
                                     ,[ClientIp]
                                     ,[CreateDate]
                                    ,ROW_NUMBER() over (order by createDate desc) as num  from [SystemLog] where 1=1 " + sb.ToString() + ") as t where num between (@pageIndex - 1) * @pageSize + 1  and @pageIndex*@pageSize order by createDate desc";
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                logList = new List<SystemLog>();
                SystemLog logInfo = null;
                while (reader.Read())
                {
                    logInfo = GetLogFromReader(reader);
                    logList.Add(logInfo);
                }
            }

            return logList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private SystemLog GetLogFromReader(DbDataReader reader)
        {
            SystemLog sysLog = new SystemLog();
            sysLog.Id = Convert.ToInt32(reader["Id"]);
            sysLog.UserId = Convert.ToInt32(reader["UserId"]);
            sysLog.Type = (LogType)reader["Type"];
            sysLog.Source = (LogSource)reader["Source"];
            sysLog.ClientIp = reader["ClientIp"].ToString();
            sysLog.ControllerName = reader["ControllerName"].ToString();
            sysLog.ActionName = reader["ActionName"].ToString();
            if (ReaderExists(reader, "UserAgent") && DBNull.Value != reader["UserAgent"])
            {
                sysLog.UserAgent = reader["UserAgent"].ToString();
            }
            if (ReaderExists(reader, "ResultLog") && DBNull.Value != reader["ResultLog"])
            {
                sysLog.ResultLog = reader["ResultLog"].ToString();
            }
            if (ReaderExists(reader, "ReqParameter") && DBNull.Value != reader["ReqParameter"])
            {
                sysLog.ReqParameter = reader["ReqParameter"].ToString();
            }
            
            sysLog.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

            return sysLog;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="logInfo">日志信息</param>
        /// <returns>添加成功标识</returns>
        public bool AddLog(SystemLog logInfo)
        {
            try
            {
                DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
                string strSql = @"Insert Into SystemLog (UserId, [Type], [Source], ControllerName, ActionName, ClientIp, UserAgent, ResultLog,ReqParameter) Values (@UserId, @Type, @Source, @ControllerName, @ActionName, @ClientIp, @UserAgent,@ResultLog,@ReqParameter)";

                List<DbParameter> parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@UserId", logInfo.UserId));
                parametersList.Add(new SqlParameter("@Type", (int)logInfo.Type));
                parametersList.Add(new SqlParameter("@Source", (int)logInfo.Source));
                parametersList.Add(new SqlParameter("@ControllerName", logInfo.ControllerName));
                parametersList.Add(new SqlParameter("@ActionName", logInfo.ActionName));
                parametersList.Add(new SqlParameter("@ClientIp", logInfo.ClientIp));
                parametersList.Add(new SqlParameter("@UserAgent", logInfo.UserAgent));
                parametersList.Add(new SqlParameter("@ResultLog", logInfo.ResultLog));
                parametersList.Add(new SqlParameter("@ReqParameter", logInfo.ReqParameter));

                return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
            }
            catch (Exception ex)
            {
                Logger.Error(this, ex + "\r\n===================\r\n" + JsonConvert.SerializeObject(logInfo));
            }

            return false;
        }
        #endregion
    }
}

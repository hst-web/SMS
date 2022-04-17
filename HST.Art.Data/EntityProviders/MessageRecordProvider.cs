using ZT.SMS.Core;
using ZT.Utillity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.SMS.Data
{
    public class MessageRecordProvider : EntityProviderBase
    {
        IntegratedProvider _interProvider = new IntegratedProvider();

        #region 查询消息
        /// <summary>
        /// 根据ID获取消息信息
        /// </summary>
        /// <param name="id">消息ID</param>
        /// <returns>消息信息</returns>
        public MessageRecord Get(int id)
        {
            MessageRecord messageRecordInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT Id, MessageId, ToAddress,SendDate, MsgData, State, OperatorId,CreateDate,IsRighting  from MessageRecord  where id=@Id ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    messageRecordInfo = GetMessageRecordFromReader(reader);
                }
            }

            return messageRecordInfo;
        }

        /// <summary>
        /// 获取所有消息信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>会员集合</returns>
        public List<MessageRecord> GetAll(MessageRecordQuery condition)
        {
            List<MessageRecord> MessageRecordList = new List<MessageRecord>();
            IList<DbParameter> parameList = new List<DbParameter>();
            string whereSort = GetWhereStr(condition, ref parameList);
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT Id, MessageId, ToAddress,SendDate, MsgData, [State], OperatorId,CreateDate,IsRighting from MessageRecord where IsDeleted=0 " + whereSort;

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                MessageRecord MessageRecordInfo = null;
                while (reader.Read())
                {
                    MessageRecordInfo = GetMessageRecordFromReader(reader);
                    MessageRecordList.Add(MessageRecordInfo);
                }
            }

            return MessageRecordList;
        }

        public List<string> GetAllMessageId()
        {
            List<string> messageIdList = new List<string>();
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "SELECT MessageId from  MessageRecord";
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, null))
            {
                while (reader.Read())
                {
                    messageIdList.Add(reader["MessageId"].ToString());
                }
            }

            return messageIdList;
        }

        /// <summary>
        /// 获取消息分页信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <param name="totalNum">总条数</param>
        /// <returns>会员集合</returns>
        public List<MessageRecord> GetPage(MessageRecordQuery condition, out int totalNum)
        {
            totalNum = 0;
            List<MessageRecord> MessageRecordList = new List<MessageRecord>();
            IList<DbParameter> parameList = new List<DbParameter>();
            string where = GetWhereStr(condition, ref parameList);
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSqlQuery = @"select count(ID) from [MessageRecord] where  IsDeleted=0 " + where;//查询有多少条记录
            totalNum = Convert.ToInt32(dbHelper.ExecuteScalar(strSqlQuery, parameList));

            parameList.Add(new SqlParameter("@pageSize", condition.PageSize));
            parameList.Add(new SqlParameter("@pageIndex", condition.PageIndex));

            string strSql = @"SELECT  [Id]
                                      ,[MessageId]
                                      ,[ToAddress]
                                      ,[SendDate]
                                      ,[MsgData]
                                      ,[State]
                                      ,[OperatorId]
                                      ,[CreateDate]
                                      ,IsRighting
                            FROM (select top (@pageSize*@pageIndex)  [ID]
                                       ,[MessageId]
                                      ,[ToAddress]
                                      ,[SendDate]
                                      ,[MsgData]
                                      ,[State]
                                      ,[OperatorId]
                                      ,[CreateDate]
                                      ,IsRighting
                                    ,ROW_NUMBER() over(order by [state],CreateDate ) as num  from [MessageRecord]  where  IsDeleted=0 " + where + ") as t where num between (@pageIndex - 1) * @pageSize + 1  and @pageIndex*@pageSize order by [state],CreateDate";
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                MessageRecordList = new List<MessageRecord>();
                MessageRecord MessageRecordInfo = null;
                while (reader.Read())
                {
                    MessageRecordInfo = GetMessageRecordFromReader(reader);
                    MessageRecordList.Add(MessageRecordInfo);
                }
            }

            return MessageRecordList;
        }

        public List<MessageReport> GetAllReport()
        {
            List<MessageReport> reportList = new List<MessageReport>();
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT Id, MsgNo, Mobile, [State], CreateDate, IsRighting,ResultCode from MessageReport where IsRighting=0 ";

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, null))
            {
                while (reader.Read())
                {
                    MessageReport reportInfo = new MessageReport();
                    reportInfo.Id = Convert.ToInt32(reader["Id"]);
                    reportInfo.MsgNo = reader["MsgNo"].ToString();
                    reportInfo.Mobile = reader["Mobile"].ToString();
                    reportInfo.ResultCode = reader["ResultCode"].ToString();
                    reportInfo.IsRighting = Convert.ToBoolean(reader["IsRighting"]);
                    reportInfo.SendState = (MsgSendState)reader["State"];
                    reportInfo.Createdate = Convert.ToDateTime(reader["CreateDate"]);
                    reportList.Add(reportInfo);
                }
            }

            return reportList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private MessageRecord GetMessageRecordFromReader(DbDataReader reader)
        {
            MessageRecord MessageRecordInfo = new MessageRecord();
            MessageRecordInfo.Id = Convert.ToInt32(reader["Id"]);
            MessageRecordInfo.MessageId = reader["MessageId"].ToString();
            MessageRecordInfo.ToAddress = reader["ToAddress"].ToString();
            MessageRecordInfo.IsRighting = Convert.ToBoolean(reader["IsRighting"]);
            MessageRecordInfo.SendState = (MsgSendState)reader["State"];
            string msgDataStr = reader["MsgData"].ToString();
            MessageRecordInfo.MsgData = string.IsNullOrEmpty(msgDataStr) ? new MsgDataInfo() : JsonUtils.Deserialize<MsgDataInfo>(msgDataStr);
            MessageRecordInfo.OperatorId = Convert.ToInt32(reader["OperatorId"].ToString());
            MessageRecordInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);
            if (ReaderExists(reader, "SendDate") && DBNull.Value != reader["SendDate"])
            {
                DateTime sendDt = Convert.ToDateTime(reader["SendDate"]);
                MessageRecordInfo.SendDate = sendDt.CompareTo(DateTime.Parse("1900-01-01")) > 0 ? Convert.ToDateTime(reader["SendDate"]) : DateTime.MinValue;
            }

            return MessageRecordInfo;
        }

        private string GetWhereStr(MessageRecordQuery condition, ref IList<DbParameter> parList)
        {
            if (condition == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(condition.MessageId))
            {
                sb.Append(" and MessageId=@MessageId");
                parList.Add(new SqlParameter("@MessageId", condition.MessageId));
            }

            if (condition.SendState >= 0)
            {
                sb.Append(" and [State]=@State");
                parList.Add(new SqlParameter("@State", condition.SendState));
            }

            if (!string.IsNullOrEmpty(condition.CreateDate))
            {
                sb.Append(" and CreateDate>@StartDate and CreateDate<@EndDate");
                DateTime sdate = Convert.ToDateTime(condition.CreateDate).Date;
                parList.Add(new SqlParameter("@StartDate", sdate));
                parList.Add(new SqlParameter("@EndDate", sdate.AddDays(1)));
            }

            if (condition.Righting >= 0)
            {
                sb.Append(" and [IsRighting]=@IsRighting");
                parList.Add(new SqlParameter("@IsRighting", condition.Righting));
            }

            return sb.ToString();
        }

        #endregion

        #region 编辑消息

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="MessageRecordInfo">消息信息</param>
        /// <returns>添加成功标识</returns>
        public bool Add(MessageRecord MessageRecordInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into MessageRecord(MessageId, ToAddress, MsgData, [State], OperatorId) Values(@MessageId, @ToAddress, @MsgData, @State, @OperatorId)";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@MessageId", MessageRecordInfo.MessageId));
            parametersList.Add(new SqlParameter("@ToAddress", MessageRecordInfo.ToAddress));
            parametersList.Add(new SqlParameter("@MsgData", GetMsgDataStr(MessageRecordInfo.MsgData)));
            parametersList.Add(new SqlParameter("@State", (int)MessageRecordInfo.SendState));
            parametersList.Add(new SqlParameter("@OperatorId", MessageRecordInfo.OperatorId));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="MessageRecordInfo">消息信息</param>
        /// <returns>添加成功标识</returns>
        public void Add(List<MessageRecord> MessageRecordInfos, out List<MessageRecord> failList)
        {
            failList = new List<MessageRecord>();
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into MessageRecord(MessageId, ToAddress,  MsgData, [State], OperatorId) Values(@MessageId, @ToAddress, @MsgData, @State, @OperatorId)";

            List<DbParameter> parametersList = null;
            foreach (MessageRecord item in MessageRecordInfos)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@MessageId", item.MessageId));
                parametersList.Add(new SqlParameter("@ToAddress", item.ToAddress));
                parametersList.Add(new SqlParameter("@MsgData", GetMsgDataStr(item.MsgData)));
                parametersList.Add(new SqlParameter("@State", (int)item.SendState));
                parametersList.Add(new SqlParameter("@OperatorId", item.OperatorId));
                try
                {
                    bool success = dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
                    if (!success)
                    {
                        item.Remark = item.Remark + "添加数据库失败";
                        failList.Add(item);
                    }
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Message.Contains("AK_UK_MESSAGEID_MESSAGER"))
                    {
                        item.Remark = item.Remark + "该编号已经存在。不可重复添加:" + item.MessageId;
                    }
                    else
                    {
                        item.Remark = item.Remark + "添加数据库失败:" + sqlEx.Message;

                    }
                    failList.Add(item);
                    Logger.Error(this, "批量添加失败,meeageId=" + item.MessageId + "exception:" + sqlEx.Message, sqlEx);
                }
                catch (Exception ex)
                {
                    item.Remark = item.Remark + "添加数据库失败:" + ex.Message;
                    failList.Add(item);
                    Logger.Error(this, "批量添加失败,meeageId=" + item.MessageId + "exception:" + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 修改消息
        /// </summary>
        /// <param name="MessageRecordInfo">消息信息</param>
        /// <returns>修改成功标识</returns>
        public bool Update(MessageRecord MessageRecordInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update MessageRecord
                              Set [MessageId]=@MessageId
                                  ,[ToAddress]=@ToAddress 
                                  ,[MsgData]=@MsgData
                                  ,[State]=@State
                                  ,[OperatorId]=@OperatorId
                                  ,[IsRighting]=@IsRighting
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", MessageRecordInfo.Id));
            parametersList.Add(new SqlParameter("@MessageId", MessageRecordInfo.MessageId));
            parametersList.Add(new SqlParameter("@ToAddress", MessageRecordInfo.ToAddress));
            parametersList.Add(new SqlParameter("@MsgData", GetMsgDataStr(MessageRecordInfo.MsgData)));
            parametersList.Add(new SqlParameter("@State", (int)MessageRecordInfo.SendState));
            parametersList.Add(new SqlParameter("@OperatorId", MessageRecordInfo.OperatorId));
            parametersList.Add(new SqlParameter("@IsRighting", Convert.ToInt32(MessageRecordInfo.IsRighting)));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }


        public bool Send(MessageRecord MessageRecordInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update MessageRecord   Set [SendDate]=getdate() ,[State]=@State,[MsgData]=@MsgData  Where ID=@ID";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@State", (int)MessageRecordInfo.SendState));
            parametersList.Add(new SqlParameter("@ID", MessageRecordInfo.Id));
            parametersList.Add(new SqlParameter("@MsgData", GetMsgDataStr(MessageRecordInfo.MsgData)));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        public void Update(List<MessageRecord> MessageRecordInfos, out List<MessageRecord> failList)
        {
            failList = new List<MessageRecord>();
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update MessageRecord
                              Set [MessageId]=@MessageId
                                  ,[ToAddress]=@ToAddress 
                                  ,[MsgData]=@MsgData
                                  ,[State]=@State
                                  ,[OperatorId]=@OperatorId
                                  ,[IsRighting]=@IsRighting
                                  Where ID=@ID";

            List<DbParameter> parametersList = null;
            foreach (MessageRecord item in MessageRecordInfos)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@ID", item.Id));
                parametersList.Add(new SqlParameter("@MessageId", item.MessageId));
                parametersList.Add(new SqlParameter("@ToAddress", item.ToAddress));
                parametersList.Add(new SqlParameter("@MsgData", GetMsgDataStr(item.MsgData)));
                parametersList.Add(new SqlParameter("@State", (int)item.SendState));
                parametersList.Add(new SqlParameter("@OperatorId", item.OperatorId));
                parametersList.Add(new SqlParameter("@IsRighting", Convert.ToInt32(item.IsRighting)));
                try
                {
                    bool success = dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
                    if (!success)
                    {
                        failList.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    failList.Add(item);
                    Logger.Error(this, "批量更新失败,meeageId=" + item.MessageId + "exception:" + ex.Message, ex);
                }
            }
        }

        public void UpdateReport(List<MessageReport> reportInfos)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update MessageReport  Set [MsgNo]=@MsgNo
                                  ,[Mobile]=@Mobile 
                                  ,[State]=@State
                                  ,[IsRighting]=@IsRighting
                                  Where ID=@ID";

            List<DbParameter> parametersList = null;
            foreach (MessageReport item in reportInfos)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@ID", item.Id));
                parametersList.Add(new SqlParameter("@MsgNo", item.MsgNo));
                parametersList.Add(new SqlParameter("@Mobile", item.Mobile));
                parametersList.Add(new SqlParameter("@State", (int)item.SendState));
                parametersList.Add(new SqlParameter("@IsRighting", Convert.ToInt32(item.IsRighting)));
                dbHelper.ExecuteNonQuery(strSql, parametersList);
            }
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete MessageRecord where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        public void AddReport(List<MessageReport> reportList)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"if exists(select Id from MessageReport where MsgNo=@MsgNo)
                                begin
                                    update MessageReport set [State]=@State,Mobile=@Mobile,ResultCode=@ResultCode,IsRighting=0 where MsgNo=@MsgNo 
                                end
                                else
                                begin
                                Insert Into MessageReport(MsgNo, Mobile, [State],ResultCode) Values(@MsgNo, @Mobile,@State,@ResultCode)
                                end ";

            List<DbParameter> parametersList = null;
            foreach (MessageReport item in reportList)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@MsgNo", item.MsgNo));
                parametersList.Add(new SqlParameter("@Mobile", item.Mobile));
                parametersList.Add(new SqlParameter("@ResultCode", item.ResultCode));
                parametersList.Add(new SqlParameter("@State", (int)item.SendState));

                try
                {
                    bool success = dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
                    if (!success)
                    {
                        AddLog(item, "未知的错误");
                    }
                }
                catch (Exception ex)
                {
                    AddLog(item, ex.ToString());
                }
            }
        }

        private void AddLog(MessageReport msgReport, string message)
        {
            SystemLog sLog = new SystemLog();
            sLog.ActionName = "SMSReport";
            sLog.ControllerName = "MessageRecordProvider.AddReport";
            sLog.ResultLog = message;
            sLog.Source = LogSource.Admin;
            sLog.Type = LogType.Error;
            sLog.UserAgent = msgReport.MsgNo;
            sLog.ReqParameter = JsonUtils.Serialize(msgReport);
            _interProvider.AddLog(sLog);
        }

        public MessageStatisticsInfo Statistics()
        {
            MessageStatisticsInfo staInfo = new MessageStatisticsInfo();
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "select [State],count(1) as StateCount from MessageRecord group by [state]";

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, null))
            {
                while (reader.Read())
                {
                    MsgSendState sedState = (MsgSendState)reader["State"];
                    int stateCount = (int)reader["StateCount"];
                    switch (sedState)
                    {
                        case MsgSendState.Unsent:
                            staInfo.Unsent = stateCount;
                            break;
                        case MsgSendState.SendFailed:
                            staInfo.SendFailed = stateCount;
                            break;
                        case MsgSendState.SendSuccess:
                            staInfo.SendSuccess = stateCount;
                            break;
                        case MsgSendState.ReceiveFailed:
                            staInfo.ReceiveFailed = stateCount;
                            break;
                    }
                }
            }

            return staInfo;
        }

        private object GetSendDate(DateTime dt)
        {
            if (dt == null || dt.Equals(DateTime.MinValue))
            {
                return "";
            }

            return dt;
        }

        private string GetMsgDataStr(MsgDataInfo info)
        {
            return JsonUtils.Serialize(info == null ? new MsgDataInfo() : info);
        }
        #endregion
    }
}

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
            string strSql = @"SELECT Id, MessageId, ToAddress,SendDate, MsgData, State, OperatorId,CreateDate  from MessageRecord  where id=@Id ";

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

            string strSql = @"SELECT Id, MessageId, ToAddress,SendDate, MsgData, [State], OperatorId,CreateDate from MessageRecord where IsDeleted=0 " + whereSort;

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
                            FROM (select top (@pageSize*@pageIndex)  [ID]
                                       ,[MessageId]
                                      ,[ToAddress]
                                      ,[SendDate]
                                      ,[MsgData]
                                      ,[State]
                                      ,[OperatorId]
                                      ,[CreateDate]
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
            string strSql = @"Insert Into MessageRecord(MessageId, ToAddress, SendDate, MsgData, [State], OperatorId) Values(@MessageId, @ToAddress, @SendDate, @MsgData, @State, @OperatorId)";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@MessageId", MessageRecordInfo.MessageId));
            parametersList.Add(new SqlParameter("@ToAddress", MessageRecordInfo.ToAddress));
            parametersList.Add(new SqlParameter("@SendDate", GetSendDate(MessageRecordInfo.SendDate)));
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
        public bool Add(List<MessageRecord> MessageRecordInfos, out List<MessageRecord> failList)
        {
            bool success = false;
            failList = new List<MessageRecord>();
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into MessageRecord(MessageId, ToAddress, SendDate, MsgData, [State], OperatorId) Values(@MessageId, @ToAddress, @SendDate, @MsgData, @State, @OperatorId)";

            List<DbParameter> parametersList = null;
            foreach (MessageRecord item in MessageRecordInfos)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@MessageId", item.MessageId));
                parametersList.Add(new SqlParameter("@ToAddress", item.ToAddress));
                parametersList.Add(new SqlParameter("@SendDate", GetSendDate(item.SendDate)));
                parametersList.Add(new SqlParameter("@MsgData", GetMsgDataStr(item.MsgData)));
                parametersList.Add(new SqlParameter("@State", (int)item.SendState));
                parametersList.Add(new SqlParameter("@OperatorId", item.OperatorId));
                try
                {
                    success = dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
                    if (!success)
                    {
                        item.Remark = item.Remark + "添加数据库失败";
                        failList.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    item.Remark = item.Remark + "添加数据库失败: ex.Message";
                    failList.Add(item);
                    Logger.Error(this, "批量添加失败,meeageId=" + item.MessageId + "exception:" + ex.Message, ex);
                }
            }


            return success && failList.Count == 0;
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
                                ,[SendDate]=@SendDate 
                                  ,[ToAddress]=@ToAddress 
                                  ,[MsgData]=@MsgData
                                  ,[State]=@State
                                  ,[OperatorId]=@OperatorId
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", MessageRecordInfo.Id));
            parametersList.Add(new SqlParameter("@SendDate", GetSendDate(MessageRecordInfo.SendDate)));
            parametersList.Add(new SqlParameter("@MessageId", MessageRecordInfo.MessageId));
            parametersList.Add(new SqlParameter("@ToAddress", MessageRecordInfo.ToAddress));
            parametersList.Add(new SqlParameter("@MsgData", GetMsgDataStr(MessageRecordInfo.MsgData)));
            parametersList.Add(new SqlParameter("@State", (int)MessageRecordInfo.SendState));
            parametersList.Add(new SqlParameter("@OperatorId", MessageRecordInfo.OperatorId));
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

        public bool Update(List<MessageRecord> MessageRecordInfos, out List<MessageRecord> failList)
        {
            bool success = false;
            failList = new List<MessageRecord>();
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update MessageRecord
                              Set [MessageId]=@MessageId
                                ,[SendDate]=@SendDate 
                                  ,[ToAddress]=@ToAddress 
                                  ,[MsgData]=@MsgData
                                  ,[State]=@State
                                  ,[OperatorId]=@OperatorId
                                  Where ID=@ID";

            List<DbParameter> parametersList = null;
            foreach (MessageRecord item in MessageRecordInfos)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@ID", item.Id));
                parametersList.Add(new SqlParameter("@MessageId", item.MessageId));
                parametersList.Add(new SqlParameter("@SendDate", GetSendDate(item.SendDate)));
                parametersList.Add(new SqlParameter("@ToAddress", item.ToAddress));
                parametersList.Add(new SqlParameter("@MsgData", GetMsgDataStr(item.MsgData)));
                parametersList.Add(new SqlParameter("@State", (int)item.SendState));
                parametersList.Add(new SqlParameter("@OperatorId", item.OperatorId));
                try
                {
                    success = dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
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

            return success && failList.Count == 0;
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

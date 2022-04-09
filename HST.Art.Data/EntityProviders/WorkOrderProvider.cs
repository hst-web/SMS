/*----------------------------------------------------------------
// 文件名：WorkOrderProvider.cs
// 功能描述： 工单数据提供者
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

namespace HST.Art.Data
{
    /// <summary>
    /// 工单数据提供者
    /// </summary>
    public class WorkOrderProvider : EntityProviderBase
    {
        #region 工单查询
        /// <summary>
        /// 根据ID获取工单信息
        /// </summary>
        /// <param name="id">工单ID</param>
        /// <returns>工单信息</returns>
        public DataEntity Get(string id)
        {
            DataEntity workOrder = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT w.[ID]
                                  ,w.[Number]
                                  ,w.[ServiceSpaceID],o.[Name] ServiceSpaceName
                                  ,w.[ServiceNetworkID] ,oo.[Name] ServiceNetworkName
                                  ,w.[OriginalNetworkID] ,ooo.[Name] OriginalNetworkName
                                  ,w.[ServiceStaffID] ,s.Name ServiceStaffName
                                  ,w.[CustomerID],c.Name CustomerName,c.Address CustomerAddress,c.Area CustomerArea,c.mobile CustomerPhone
                                  ,w.[ProductID] ,p.Name ProductName,p.Type ProductType,p.CategoryID ProductCategoryID,pc.Name ProductCategoryName
                                  ,w.[ServiceCategoryID],sc.Name ServiceCategoryName
                                  ,w.[ServiceModeID],sm.Name ServiceModeName
                                  ,w.[FaultCategoryID],f.Name FaultCategoryName
                                  ,w.[CurrentHandlerID]
                                  ,w.[NextHandlerID]
                                  ,w.[Caption]
                                  ,w.[Description]
                                  ,w.[ProcessDesc]
                                  ,w.[Source]
                                  ,w.[Channel]
                                  ,w.[State]
                                  ,w.[PendState]
                                  ,w.PrevOperation
                                  ,w.[Priority]
                                  ,w.[GuaranteedState]
                                  ,w.[StatementState]
                                  ,w.[Contact]
                                  ,w.[ContactNumber]
                                  ,w.[CustomFields]
                                  ,w.[CreateTime]
                                  ,w.CallCount
                                  ,w.[IsDeleted]
                                  ,w.[AbnormalMsg]
                                  ,w.[AbnormalTime]
                                  ,w.[NeedStatemented]
                                  ,w.[AcceptTime] 
                                  ,w.[AssignTime]
                                  ,w.[DispatchTime]
                                  ,w.[ReceivedTime]
                                  ,w.[SubscribeTime]
                                  ,w.[SubscribeServiceTime]
                                  ,w.[StartServiceTime]
                                  ,w.[QualityCheckTime]
                                  ,w.[StatementTime]
                                  ,w.[ReturnVisitTime]
                                  ,w.[Signature]
                                  ,w.[ReportTime]
                                  ,w.[FinishTime]
                                  ,w.[CancelTime]
                                  ,w.IsEvaluated
                                  ,w.Remark
                                  ,w.IsServiceEnd
                                  ,w.IsSubmitImg
                                  ,w.IsHasReport
                                  ,w.IsCustomerEvaluate
                            FROM [WorkOrder] w left join ServiceSpace o on w.ServiceSpaceID=o.ID left join ServiceNetwork oo on w.ServiceNetworkID=oo.ID Left join ServiceNetwork ooo on w.OriginalNetworkID=ooo.ID left join Staff s on w.ServiceStaffID=s.ID left join Customer c on w.CustomerID=c.ID left join Product p on w.ProductID=p.ID left join ServiceCategory sc on w.ServiceCategoryID=sc.ID left join ServiceMode sm on w.ServiceModeID=sm.ID left join FaultCategory f on w.FaultCategoryID=f.ID inner join ProductCategory pc on p.CategoryID=pc.id 
                            Where w.[ID]=@ID ";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                workOrder = getWorkOrdersFromReader(reader).FirstOrDefault();
            }

            if (workOrder != null)
            {
                //获取当前处理者名
                string sqlCurrentHandlerName = @"select Name from Staff where Id='" + workOrder["CurrentHandlerID"].Value + "'";
                //if ((WorkOrderState)workOrder["State"].Value == WorkOrderState.Created && (WorkOrderSource)workOrder["Source"].Value == WorkOrderSource.Customer)
                //{
                //    sqlCurrentHandlerName = @"select Name from Customer where Id='" + workOrder["CurrentHandlerID"].Value + "'";
                //}
                object obj = dbHelper.ExecuteScalar(sqlCurrentHandlerName, null);
                workOrder["CurrentHandlerName"].Value = obj != null ? obj.ToString() : "";
            }

            return workOrder;
        }

        /// <summary>
        /// 根据条件获取所有工单信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>工单集合</returns>
        public List<DataEntity> GetAll(Condition condition)
        {
            List<DataEntity> workOrders = null;
            string sort = string.IsNullOrEmpty(condition.Sort) ? "createtime desc" : condition.Sort + " " + condition.Direction;
            string where = GetWhereStr(condition.Where);
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Select w.[ID]
                                  ,w.[Number]
                                  ,w.[ServiceSpaceID]
                                  ,w.[ServiceNetworkID] ,oo.[Name] ServiceNetworkName
                                  ,w.[ServiceStaffID] ,s.Name ServiceStaffName
                                  ,w.[CustomerID],c.Name CustomerName
                                  ,w.[ProductID] ,p.Name ProductName,p.Type ProductType
                                  ,w.[ServiceCategoryID],sc.Name ServiceCategoryName
                                  ,w.[ServiceModeID],sm.Name ServiceModeName
                                  ,w.[FaultCategoryID],f.Name FaultCategoryName
                                  ,w.[CurrentHandlerID]
                                  ,w.[NextHandlerID]
                                  ,w.[Source]
                                  ,w.[Channel]
                                  ,w.[State]
                                  ,w.[PendState]
                                  ,w.PrevOperation
                                  ,w.[Priority]
                                  ,w.[GuaranteedState]
                                  ,w.[StatementState]
                                  ,w.[Contact]
                                  ,w.[ContactNumber]
                                  ,w.[CustomFields]
                                  ,w.[CreateTime]
                                  ,w.CallCount
                                  ,w.[IsDeleted]
                                  ,w.[NeedStatemented]
                                  ,w.[AbnormalTime]
                                  ,w.[AcceptTime]
                                  ,w.[AssignTime] 
                                  ,w.[DispatchTime]
                                  ,w.[ReceivedTime]
                                  ,w.[SubscribeTime]
                                  ,w.[StartServiceTime]
                                  ,w.[QualityCheckTime]
                                  ,w.[StatementTime]
                                  ,w.[ReturnVisitTime]
                                  ,w.[ReportTime] 
                                  ,w.[FinishTime]
                                  ,w.IsEvaluated
                                  ,w.[CancelTime]
                                  ,w.IsServiceEnd
                                  ,w.IsHasReport
                            FROM [WorkOrder] w  left join ServiceNetwork oo on w.ServiceNetworkID=oo.ID left join Staff s on w.ServiceStaffID=s.ID left join Customer c on w.CustomerID=c.ID left join Product p on w.ProductID=p.ID left join ServiceCategory sc on w.ServiceCategoryID=sc.ID left join ServiceMode sm on w.ServiceModeID=sm.ID left join FaultCategory f on w.FaultCategoryID=f.ID
                            where 1=1 " + where + " order by " + sort;
            IList<DbParameter> parList = new List<DbParameter>();
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                workOrders = getWorkOrdersFromReader(reader);
            }

            return workOrders;
        }

        /// <summary>
        /// 获取处理工单（主要用于手机端首页通过统计数据调整到工单列表中）
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns></returns>
        public List<DataEntity> GetProWork(Condition condition)
        {
            List<DataEntity> workOrders = null;
            string where = GetProWhereStr(condition.Where);
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Select distinct w.[ID]
                                  ,w.[Number]
                                  ,w.[ServiceSpaceID]
                                  ,w.[ServiceNetworkID] ,oo.[Name] ServiceNetworkName
                                  ,w.[ServiceStaffID] ,s.Name ServiceStaffName
                                  ,w.[CustomerID],c.Name CustomerName
                                  ,w.[ProductID] ,p.Name ProductName,p.Type ProductType
                                  ,w.[ServiceCategoryID],sc.Name ServiceCategoryName
                                  ,w.[ServiceModeID],sm.Name ServiceModeName
                                  ,w.[FaultCategoryID],f.Name FaultCategoryName
                                  ,w.[CurrentHandlerID]
                                  ,w.[NextHandlerID]
                                  ,w.[Source]
                                  ,w.[Channel]
                                  ,w.[State]
                                  ,w.[PendState]
                                  ,w.PrevOperation
                                  ,w.[Priority]
                                  ,w.[GuaranteedState]
                                  ,w.[StatementState]
                                  ,w.[Contact]
                                  ,w.[ContactNumber]
                                  ,w.[CustomFields]
                                  ,w.[CreateTime]
                                  ,w.CallCount
                                  ,w.[IsDeleted]
                                  ,w.[NeedStatemented]
                                  ,w.[AbnormalTime]
                                  ,w.[AcceptTime]
                                  ,w.[AssignTime] 
                                  ,w.[DispatchTime]
                                  ,w.[ReceivedTime]
                                  ,w.[SubscribeTime]
                                  ,w.[StartServiceTime]
                                  ,w.[QualityCheckTime]
                                  ,w.[StatementTime]
                                  ,w.[ReturnVisitTime]
                                  ,w.[ReportTime] 
                                  ,w.[FinishTime]
                                  ,w.IsEvaluated
                                  ,w.[CancelTime]
                                  ,w.IsServiceEnd
                                  ,w.IsHasReport
                            FROM [WorkOrder] w  left join ServiceNetwork oo on w.ServiceNetworkID=oo.ID left join Staff s on w.ServiceStaffID=s.ID left join Customer c on w.CustomerID=c.ID left join Product p on w.ProductID=p.ID left join ServiceCategory sc on w.ServiceCategoryID=sc.ID left join ServiceMode sm on w.ServiceModeID=sm.ID left join FaultCategory f on w.FaultCategoryID=f.ID  inner join WorkOrderProcess wp on w.ID= wp.WorkOrderID 
                            where 1=1 " + where;
            IList<DbParameter> parList = new List<DbParameter>();
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                workOrders = getWorkOrdersFromReader(reader);
            }

            return workOrders;
        }

        /// <summary>
        /// 获取所有工单信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <param name="totalNum">总记录数</param>
        /// <returns>工单集合</returns>
        public List<DataEntity> GetPage(Condition condition, out int totalNum)
        {
            totalNum = 0;
            string sort = string.IsNullOrEmpty(condition.Sort) ? "createtime desc" : condition.Sort + " " + condition.Direction;
            string where = GetWhereStr(condition.Where);

            List<DataEntity> workOrders = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select count(w.ID) from WorkOrder w left join ServiceSpace o on w.ServiceSpaceID=o.ID left join ServiceNetwork oo on w.ServiceNetworkID=oo.ID left join Staff s on w.ServiceStaffID=s.ID left join Customer c on w.CustomerID=c.ID left join Product p on w.ProductID=p.ID left join ServiceCategory sc on w.ServiceCategoryID=sc.ID left join ServiceMode sm on w.ServiceModeID=sm.ID left join FaultCategory f on w.FaultCategoryID=f.ID where 1=1" + where;//查询有多少条记录
            totalNum = Convert.ToInt32(dbHelper.ExecuteScalar(strSqlQuery, null));

            string strSql = @"SELECT  [ID]
                                  ,[Number]
                                  ,[ServiceSpaceID]
                                  ,[ServiceNetworkID] , ServiceNetworkName
                                  ,[ServiceStaffID] ,ServiceStaffName
                                  ,[CustomerID], CustomerName,CustomerAddress,CustomerArea
                                  ,[ProductID] , ProductName, ProductType
                                  ,[ServiceCategoryID], ServiceCategoryName
                                  ,[ServiceModeID],ServiceModeName
                                  ,[FaultCategoryID], FaultCategoryName
                                  ,[CurrentHandlerID]
                                  ,[NextHandlerID]
                                  ,[Source]
                                  ,[Channel]
                                  ,[State]
                                  ,[PendState]
                                  ,PrevOperation
                                  ,[Priority]
                                  ,[GuaranteedState]
                                  ,[StatementState]
                                  ,[Contact]
                                  ,[ContactNumber]
                                  ,[CustomFields]
                                  ,[CreateTime]
                                  ,[CallCount]
                                  ,[IsDeleted]
                                  ,[NeedStatemented]
                                  ,[AbnormalTime]
                                  ,[AcceptTime] ,[AssignTime] ,[DispatchTime] ,[ReceivedTime],[SubscribeTime],[StartServiceTime],[QualityCheckTime],[StatementTime],[ReturnVisitTime],[ReportTime] ,[FinishTime],IsEvaluated,[CancelTime],IsServiceEnd,IsHasReport
                            FROM (select top (@pageSize*@pageIndex) w.[ID]
                                  ,w.[Number]
                                  ,w.[ServiceSpaceID]
                                  ,w.[ServiceNetworkID] ,oo.[Name] ServiceNetworkName
                                  ,w.[ServiceStaffID] ,s.Name ServiceStaffName
                                  ,w.[CustomerID],c.Name CustomerName,c.Address CustomerAddress,c.Area CustomerArea
                                  ,w.[ProductID] ,p.Name ProductName,p.Type ProductType
                                  ,w.[ServiceCategoryID],sc.Name ServiceCategoryName
                                  ,w.[ServiceModeID],sm.Name ServiceModeName
                                  ,w.[FaultCategoryID],f.Name FaultCategoryName
                                  ,w.[CurrentHandlerID]
                                  ,w.[NextHandlerID]
                                  ,w.[Source]
                                  ,w.[Channel]
                                  ,w.[State]
                                  ,w.[PendState]
                                  ,w.PrevOperation
                                  ,w.[Priority]
                                  ,w.[GuaranteedState]
                                  ,w.[StatementState]
                                  ,w.[Contact]
                                  ,w.[ContactNumber]
                                  ,w.[CustomFields]
                                  ,w.[CreateTime]
                                  ,w.[CallCount]
                                  ,w.[IsDeleted]
                                  ,w.[NeedStatemented]
                                  ,w.[AbnormalTime]
                                  ,w.[AcceptTime] ,w.[AssignTime] ,w.[DispatchTime] ,w.[ReceivedTime],w.[SubscribeTime],w.[StartServiceTime],w.[QualityCheckTime],w.[StatementTime],w.[ReturnVisitTime],w.[ReportTime] ,w.[FinishTime],w.IsEvaluated,w.[CancelTime],w.IsServiceEnd,w.IsHasReport,ROW_NUMBER() over(order by w." + sort + ") as num  from  WorkOrder w  left join ServiceNetwork oo on w.ServiceNetworkID=oo.ID left join Staff s on w.ServiceStaffID=s.ID left join Customer c on w.CustomerID=c.ID left join Product p on w.ProductID=p.ID left join ServiceCategory sc on w.ServiceCategoryID=sc.ID left join ServiceMode sm on w.ServiceModeID=sm.ID left join FaultCategory f on w.FaultCategoryID=f.ID  where 1=1 " + where + ") as t where num between(@pageIndex - 1) * @pageSize + 1  and @pageIndex*@pageSize order by " + sort;
            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@pageIndex", condition.PageIndex));
            parList.Add(new SqlParameter("@pageSize", condition.PageSize));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                workOrders = getWorkOrdersFromReader(reader);
            }

            return workOrders;
        }

        /// <summary>
        /// 获取待处理工单
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns></returns>
        public List<DataEntity> GetPend(Condition condition)
        {
            List<DataEntity> pendList = null;
            string where = GetPendWhereStr(condition.Where);
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @" SELECT 
                                  pw.[ID]
                                  ,[WorkOrderID]
                                  ,[WorkOrderNumber]
                                  ,pw.[ServiceSpaceID]
                                  ,[ServiceNetworkID]
                                  ,[ServiceCategoryID]
                                  ,sc.Name [ServiceCategoryName]
                                  ,pw.[ServiceModeID]
                                  ,sm.name [ServiceModeName]
                                  ,[ServiceStaffID]
                                  ,pw.[ProductID]
                                  ,p.name [ProductName]
                                  ,p.type [ProductType]
                                  ,[State]
                                  ,[PendState]
                                  ,[IsReaded]
                                  ,pw.[CreateTime]
                                  ,c.Name CustomerName
                                  ,c.Mobile CustomerPhone
                            FROM [PendWorkOrder] pw left join Product p on pw.ProductID=p.id left join ServiceMode sm on pw.ServiceModeID=sm.id left join
                           ServiceCategory sc on sc.ID=pw.ServiceCategoryID left join customer c on pw.customerid=c.id Where 1=1 " + where + " order by pw.CreateTime desc";

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, null))
            {
                pendList = new List<DataEntity>();
                DataEntity pendWork = null;
                while (reader.Read())
                {
                    pendWork = new DataEntity();
                    if (DBNull.Value != reader["ID"])
                        pendWork.Add("ID", reader["ID"].ToString());
                    else
                        pendWork.Add("ID", string.Empty);
                    if (DBNull.Value != reader["WorkOrderID"])
                        pendWork.Add("WorkOrderID", reader["WorkOrderID"].ToString());
                    else
                        pendWork.Add("WorkOrderID", string.Empty);
                    if (DBNull.Value != reader["WorkOrderNumber"])
                        pendWork.Add("WorkOrderNumber", reader["WorkOrderNumber"].ToString());
                    else
                        pendWork.Add("WorkOrderNumber", string.Empty);
                    if (DBNull.Value != reader["ServiceSpaceID"])
                        pendWork.Add("ServiceSpaceID", reader["ServiceSpaceID"].ToString());
                    else
                        pendWork.Add("ServiceSpaceID", string.Empty);
                    if (DBNull.Value != reader["ServiceNetworkID"])
                        pendWork.Add("ServiceNetworkID", reader["ServiceNetworkID"].ToString());
                    else
                        pendWork.Add("ServiceNetworkID", string.Empty);
                    if (DBNull.Value != reader["ServiceCategoryID"])
                        pendWork.Add("ServiceCategoryID", reader["ServiceCategoryID"].ToString());
                    else
                        pendWork.Add("ServiceCategoryID", string.Empty);
                    if (DBNull.Value != reader["ServiceCategoryName"])
                        pendWork.Add("ServiceCategoryName", reader["ServiceCategoryName"].ToString());
                    else
                        pendWork.Add("ServiceCategoryName", string.Empty);
                    if (DBNull.Value != reader["ServiceModeID"])
                        pendWork.Add("ServiceModeID", reader["ServiceModeID"].ToString());
                    else
                        pendWork.Add("ServiceModeID", string.Empty);
                    if (DBNull.Value != reader["ServiceModeName"])
                        pendWork.Add("ServiceModeName", reader["ServiceModeName"].ToString());
                    else
                        pendWork.Add("ServiceModeName", string.Empty);
                    if (DBNull.Value != reader["ServiceStaffID"])
                        pendWork.Add("ServiceStaffID", reader["ServiceStaffID"].ToString());
                    else
                        pendWork.Add("ServiceStaffID", string.Empty);
                    if (DBNull.Value != reader["ProductID"])
                        pendWork.Add("ProductID", reader["ProductID"].ToString());
                    else
                        pendWork.Add("ProductID", string.Empty);
                    if (DBNull.Value != reader["ProductName"])
                        pendWork.Add("ProductName", reader["ProductName"].ToString());
                    else
                        pendWork.Add("ProductName", string.Empty);
                    if (DBNull.Value != reader["ProductType"])
                        pendWork.Add("ProductType", reader["ProductType"].ToString());
                    else
                        pendWork.Add("ProductType", string.Empty);
                    if (DBNull.Value != reader["State"])
                        pendWork.Add("State", Convert.ToInt32(reader["State"]));
                    else
                        pendWork.Add("State", 0);
                    if (DBNull.Value != reader["PendState"])
                        pendWork.Add("PendState", Convert.ToInt32(reader["PendState"]));
                    else
                        pendWork.Add("PendState", 0);
                    if (DBNull.Value != reader["IsReaded"])
                        pendWork.Add("IsReaded", Convert.ToBoolean(reader["IsReaded"]));
                    else
                        pendWork.Add("IsReaded", false);
                    if (DBNull.Value != reader["CreateTime"])
                        pendWork.Add("CreateTime", Convert.ToDateTime(reader["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss"));
                    else
                        pendWork.Add("CreateTime", string.Empty);
                    if (DBNull.Value != reader["CustomerName"])
                        pendWork.Add("CustomerName", reader["CustomerName"].ToString());
                    else
                        pendWork.Add("CustomerName", string.Empty);
                    if (DBNull.Value != reader["CustomerPhone"])
                        pendWork.Add("CustomerPhone", reader["CustomerPhone"].ToString());
                    else
                        pendWork.Add("CustomerPhone", string.Empty);

                    pendList.Add(pendWork);
                }
            }

            return pendList;
        }

        /// <summary>
        /// 根据检索条件获取记录总数
        /// </summary>
        /// <param name="condition">检索条件</param>
        /// <returns>记录总数</returns>
        public int GetCount(Condition condition)
        {
            int totalNum = 0;
            string where = GetWhereStr(condition.Where);
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select count(w.ID) from WorkOrder w   where 1=1 " + where;//查询有多少条记录
            totalNum = Convert.ToInt32(dbHelper.ExecuteScalar(strSqlQuery, null));
            return totalNum;
        }


        /// <summary>
        /// 根据条件获取动作记录数（不重复）
        /// </summary>
        /// <param name="condition">检索条件</param>
        /// <returns>记录总数</returns>
        public int GetProcessCount(Condition condition)
        {
            int totalNum = 0;
            string where = GetProWhereStr(condition.Where);
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select count(distinct WorkOrderID) from WorkOrderProcess wp where 1=1 " + where;//查询有多少条记录
            totalNum = Convert.ToInt32(dbHelper.ExecuteScalar(strSqlQuery, null));
            return totalNum;
        }

        /// <summary>
        /// 根据服务空间id、服务网点id获取所属员工的工单数量
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <param name="serviceNetworkId">服务网点id</param>
        /// <returns>员工id、工单数量组成的键值对集合</returns>
        public Dictionary<string, int> GetCount(string serviceSpaceId, string serviceNetworkId)
        {
            Dictionary<string, int> keyvalue = new Dictionary<string, int>();
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT count(w.ID) Count,s.StaffID from [ServicePerson] s left join  WorkOrder w on (w.[ServiceStaffID]=s.StaffID)  where  s.ServiceSpaceID='" + serviceSpaceId + "' and s.ServiceNetworkID='" + serviceNetworkId + "' Group by s.StaffID";

            DataTable dt = dbHelper.ExecuteDataTable(strSql, null);//员工服务的工单

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string staffId = row["StaffID"].ToString();
                    int count = (int)row["Count"];
                    if (keyvalue.ContainsKey(staffId))
                    {
                        keyvalue[staffId] = keyvalue[staffId] + count;
                    }
                    else
                    {
                        keyvalue.Add(staffId, count);
                    }
                }
            }

            return keyvalue;
        }

        /// <summary>
        /// 获取最大工单编号
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <returns>工单编号</returns>
        public string GetMaxNumber(string serviceSpaceId)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"select MAX(Number) from WorkOrder where ServiceSpaceId = '" + serviceSpaceId + "' and  CONVERT(varchar(10), createtime, 120)=CONVERT(varchar(10), getdate(), 120)";
            object obj = dbHelper.ExecuteScalar(strSql, null);
            if (obj != null)
                return obj.ToString();
            return "";
        }

        /// <summary>
        /// 根据视图id获取一个工单视图
        /// </summary>
        /// <param name="viewId">视图id</param>
        /// <param name="isPublic">公有视图标识</param>
        /// <returns>过滤器</returns>
        private string GetViewCondition(string viewId, bool isPublic)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string viewConditionSql = string.Empty;
            string strSql = isPublic ? @"SELECT [Condition] FROM [PublicWorkOrderView] Where [ID]=@ID" : @"SELECT [Condition] FROM [PrivateWorkOrderView] Where [ID]=@ID";

            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@ID", viewId));
            object obj = dbHelper.ExecuteScalar(strSql, parList);

            viewConditionSql = obj == null ? "" : obj.ToString();

            foreach (KeyValuePair<string, string> item in GetMatchSqlList())
            {
                if (viewConditionSql.Contains(item.Key))
                {
                    viewConditionSql = viewConditionSql.Replace(item.Key, item.Value);
                }
            }

            return viewConditionSql;
        }

        /// <summary>
        /// 匹配筛选时间sql字段
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetMatchSqlList()
        {
            Dictionary<string, string> matchStrList = new Dictionary<string, string>();
            #region 日期处理
            DateTime dt = DateTime.Now;
            int dayOfWeek = Convert.ToInt32(dt.DayOfWeek.ToString("d"));
            DateTime weekStart = dt.AddDays(1 - ((dayOfWeek == 0) ? 7 : dayOfWeek));   //本周周一
            DateTime weekEnd = weekStart.AddDays(6);  //本周周日
            DateTime lastWeekStart = weekStart.AddDays(-7);  //上周周一
            DateTime lastWeekEnd = weekEnd.AddDays(-7);  //上周周日
            DateTime monthStart = dt.AddDays(1 - (dt.Day));//本月第一天
            DateTime monthEnd = monthStart.AddDays(DateTime.DaysInMonth(dt.Date.Year, dt.Date.Month) - 1);//本月最后一天
            DateTime lastMonthStart = dt.AddMonths(-1).AddDays(1 - dt.AddMonths(-1).Day);//上月第一天
            DateTime lastMonthEnd = lastMonthStart.AddDays(DateTime.DaysInMonth(dt.AddMonths(-1).Date.Year, dt.AddMonths(-1).Date.Month) - 1);//上月最后一天
            DateTime yearStart = new DateTime(dt.Year, 1, 1);//本年第一天
            DateTime yearEnd = new DateTime(dt.Year, 12, 31);//本年最后一天

            #endregion

            #region 当筛选条件为“是”
            //创建时间
            matchStrList.Add("createtime='today'", "convert(varchar(10),w.createtime,120)='" + dt.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("createtime='yesterday'", "convert(varchar(10),w.createtime,120)='" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("createtime='thisweek'", "convert(varchar(10),w.createtime,120)>='" + weekStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.createtime,120)<='" + weekEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("createtime='lastweek'", "convert(varchar(10),w.createtime,120)>='" + lastWeekStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.createtime,120)<='" + lastWeekEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("createtime='thismonth'", "convert(varchar(10),w.createtime,120)>='" + monthStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.createtime,120)<='" + monthEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("createtime='lastmonth'", "convert(varchar(10),w.createtime,120)>='" + lastMonthStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.createtime,120)<='" + lastMonthEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("createtime='thisyear'", "convert(varchar(10),w.createtime,120)>='" + yearStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.createtime,120)<='" + yearEnd.ToString("yyyy-MM-dd") + "'");

            //分配时间
            matchStrList.Add("assigntime='today'", "convert(varchar(10),w.assigntime,120)='" + dt.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("assigntime='yesterday'", "convert(varchar(10),w.assigntime,120)='" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("assigntime='thisweek'", "convert(varchar(10),w.assigntime,120)>='" + weekStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.assigntime,120)<='" + weekEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("assigntime='lastweek'", "convert(varchar(10),w.assigntime,120)>='" + lastWeekStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.assigntime,120)<='" + lastWeekEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("assigntime='thismonth'", "convert(varchar(10),w.assigntime,120)>='" + monthStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.assigntime,120)<='" + monthEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("assigntime='lastmonth'", "convert(varchar(10),w.assigntime,120)>='" + lastMonthStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.assigntime,120)<='" + lastMonthEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("assigntime='thisyear'", "convert(varchar(10),w.assigntime,120)>='" + yearStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.assigntime,120)<='" + yearEnd.ToString("yyyy-MM-dd") + "'");

            //关闭时间
            matchStrList.Add("finishtime='today'", "convert(varchar(10),w.finishtime,120)='" + dt.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("finishtime='yesterday'", "convert(varchar(10),w.finishtime,120)='" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("finishtime='thisweek'", "convert(varchar(10),w.finishtime,120)>='" + weekStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.finishtime,120)<='" + weekEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("finishtime='lastweek'", "convert(varchar(10),w.finishtime,120)>='" + lastWeekStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.finishtime,120)<='" + lastWeekEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("finishtime='thismonth'", "convert(varchar(10),w.finishtime,120)>='" + monthStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.finishtime,120)<='" + monthEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("finishtime='lastmonth'", "convert(varchar(10),w.finishtime,120)>='" + lastMonthStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.finishtime,120)<='" + lastMonthEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("finishtime='thisyear'", "convert(varchar(10),w.finishtime,120)>='" + yearStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.finishtime,120)<='" + yearEnd.ToString("yyyy-MM-dd") + "'");

            //派工时间
            matchStrList.Add("dispatchtime='today'", "convert(varchar(10),w.dispatchtime,120)='" + dt.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("dispatchtime='yesterday'", "convert(varchar(10),w.dispatchtime,120)='" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("dispatchtime='thisweek'", "convert(varchar(10),w.dispatchtime,120)>='" + weekStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.dispatchtime,120)<='" + weekEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("dispatchtime='lastweek'", "convert(varchar(10),w.dispatchtime,120)>='" + lastWeekStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.dispatchtime,120)<='" + lastWeekEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("dispatchtime='thismonth'", "convert(varchar(10),w.dispatchtime,120)>='" + monthStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.dispatchtime,120)<='" + monthEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("dispatchtime='lastmonth'", "convert(varchar(10),w.dispatchtime,120)>='" + lastMonthStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.dispatchtime,120)<='" + lastMonthEnd.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("dispatchtime='thisyear'", "convert(varchar(10),w.dispatchtime,120)>='" + yearStart.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.dispatchtime,120)<='" + yearEnd.ToString("yyyy-MM-dd") + "'");
            #endregion

            #region 当筛选条件为“非”
            //创建时间
            matchStrList.Add("createtime<>'today'", "convert(varchar(10),w.createtime,120)<>'" + dt.ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("createtime<>'yesterday'", "convert(varchar(10),w.createtime,120)<>'" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "'");
            matchStrList.Add("createtime<>'thisweek'", "(convert(varchar(10),w.createtime,120)<'" + weekStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.createtime,120)>'" + weekEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("createtime<>'lastweek'", "(convert(varchar(10),w.createtime,120)<'" + lastWeekStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.createtime,120)>'" + lastWeekEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("createtime<>'thismonth'", "(convert(varchar(10),w.createtime,120)<'" + monthStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.createtime,120)>'" + monthEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("createtime<>'lastmonth'", "(convert(varchar(10),w.createtime,120)<'" + lastMonthStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.createtime,120)>'" + lastMonthEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("createtime<>'thisyear'", "(convert(varchar(10),w.createtime,120)<'" + yearStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.createtime,120)>'" + yearEnd.ToString("yyyy-MM-dd") + "')");

            //分配时间
            matchStrList.Add("assigntime<>'today'", "convert(varchar(10),w.assigntime,120)<>'" + dt.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.assigntime,120)>'1900-01-01'");
            matchStrList.Add("assigntime<>'yesterday'", "convert(varchar(10),w.assigntime,120)<>'" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.assigntime,120)>'1900-01-01'");
            matchStrList.Add("assigntime<>'thisweek'", "convert(varchar(10),w.assigntime,120)>'1900-01-01' and (convert(varchar(10),w.assigntime,120)<'" + weekStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.assigntime,120)>'" + weekEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("assigntime<>'lastweek'", "convert(varchar(10),w.assigntime,120)>'1900-01-01' and (convert(varchar(10),w.assigntime,120)<'" + lastWeekStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.assigntime,120)>'" + lastWeekEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("assigntime<>'thismonth'", "convert(varchar(10),w.assigntime,120)>'1900-01-01' and (convert(varchar(10),w.assigntime,120)<'" + monthStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.assigntime,120)>'" + monthEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("assigntime<>'lastmonth'", "convert(varchar(10),w.assigntime,120)>'1900-01-01' and (convert(varchar(10),w.assigntime,120)<'" + lastMonthStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.assigntime,120)>'" + lastMonthEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("assigntime<>'thisyear'", "convert(varchar(10),w.assigntime,120)>'1900-01-01' and (convert(varchar(10),w.assigntime,120)<'" + yearStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.assigntime,120)>'" + yearEnd.ToString("yyyy-MM-dd") + "')");

            //关闭时间
            matchStrList.Add("finishtime<>'today'", "convert(varchar(10),w.finishtime,120)<>'" + dt.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.finishtime,120)>'1900-01-01'");
            matchStrList.Add("finishtime<>'yesterday'", "convert(varchar(10),w.finishtime,120)<>'" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.finishtime,120)>'1900-01-01'");
            matchStrList.Add("finishtime<>'thisweek'", "convert(varchar(10),w.finishtime,120)>'1900-01-01' and (convert(varchar(10),w.finishtime,120)<'" + weekStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.finishtime,120)>'" + weekEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("finishtime<>'lastweek'", "convert(varchar(10),w.finishtime,120)>'1900-01-01' and (convert(varchar(10),w.finishtime,120)<'" + lastWeekStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.finishtime,120)>'" + lastWeekEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("finishtime<>'thismonth'", "convert(varchar(10),w.finishtime,120)>'1900-01-01' and (convert(varchar(10),w.finishtime,120)<'" + monthStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.finishtime,120)>'" + monthEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("finishtime<>'lastmonth'", "convert(varchar(10),w.finishtime,120)>'1900-01-01' and (convert(varchar(10),w.finishtime,120)<'" + lastMonthStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.finishtime,120)>'" + lastMonthEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("finishtime<>'thisyear'", "convert(varchar(10),w.finishtime,120)>'1900-01-01' and (convert(varchar(10),w.finishtime,120)<'" + yearStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.finishtime,120)>'" + yearEnd.ToString("yyyy-MM-dd") + "')");

            //派工时间
            matchStrList.Add("dispatchtime<>'today'", "convert(varchar(10),w.dispatchtime,120)<>'" + dt.ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.dispatchtime,120)>'1900-01-01'");
            matchStrList.Add("dispatchtime<>'yesterday'", "convert(varchar(10),w.dispatchtime,120)<>'" + dt.AddDays(-1).ToString("yyyy-MM-dd") + "' and convert(varchar(10),w.dispatchtime,120)>'1900-01-01'");
            matchStrList.Add("dispatchtime<>'thisweek'", "convert(varchar(10),w.dispatchtime,120)>'1900-01-01' and (convert(varchar(10),w.dispatchtime,120)<'" + weekStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.dispatchtime,120)>'" + weekEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("dispatchtime<>'lastweek'", "convert(varchar(10),w.dispatchtime,120)>'1900-01-01' and (convert(varchar(10),w.dispatchtime,120)<'" + lastWeekStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.dispatchtime,120)>'" + lastWeekEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("dispatchtime<>'thismonth'", "convert(varchar(10),w.dispatchtime,120)>'1900-01-01' and (convert(varchar(10),w.dispatchtime,120)<'" + monthStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.dispatchtime,120)>'" + monthEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("dispatchtime<>'lastmonth'", "convert(varchar(10),w.dispatchtime,120)>'1900-01-01' and (convert(varchar(10),w.dispatchtime,120)<'" + lastMonthStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.dispatchtime,120)>'" + lastMonthEnd.ToString("yyyy-MM-dd") + "')");
            matchStrList.Add("dispatchtime<>'thisyear'", "convert(varchar(10),w.dispatchtime,120)>'1900-01-01' and (convert(varchar(10),w.dispatchtime,120)<'" + yearStart.ToString("yyyy-MM-dd") + "' or convert(varchar(10),w.dispatchtime,120)>'" + yearEnd.ToString("yyyy-MM-dd") + "')");
            #endregion

            return matchStrList;
        }

        /// <summary>
        /// 获取where条件语句
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns></returns>
        private string GetWhereStr(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return string.Empty;
            StringBuilder whereBuilder = new StringBuilder();
            //JObject job = (JObject)JsonConvert.DeserializeObject(jsonStr);
  
            //if (job != null && job.Count > 0)
            //{
            //    //工单公有视图
            //    if (job["publicfilter"] != null && !string.IsNullOrEmpty(job["publicfilter"].ToString()))
            //    {
            //        string publicfilter = job["publicfilter"].ToString();
            //        string condition = GetViewCondition(publicfilter, true);
            //        if (!string.IsNullOrEmpty(condition))
            //        {
            //            whereBuilder.Append(" and " + condition);
            //        }
            //    }

            //    //工单私有视图
            //    if (job["privatefilter"] != null && !string.IsNullOrEmpty(job["privatefilter"].ToString()))
            //    {
            //        string privatefilter = job["privatefilter"].ToString();
            //        string condition = GetViewCondition(privatefilter, false);
            //        if (!string.IsNullOrEmpty(condition))
            //        {
            //            whereBuilder.Append(" and " + condition);
            //        }
            //    }

            //    //搜索关键词工单编号或者客户名称
            //    if (job["searchkey"] != null && !string.IsNullOrEmpty(job["searchkey"].ToString()))
            //    {
            //        whereBuilder.Append(" and (w.number like '" + job["searchkey"] + "%' or c.Name like '%" + job["searchkey"] + "%')");
            //    }
            //    //服务空间id
            //    if (job["servicespaceid"] != null && !string.IsNullOrEmpty(job["servicespaceid"].ToString()))
            //    {
            //        whereBuilder.Append(" and w.servicespaceid ='" + job["servicespaceid"] + "'");
            //    }
            //    //服务网点id
            //    if (job["servicenetworkid"] != null && !string.IsNullOrEmpty(job["servicenetworkid"].ToString()))
            //    {
            //        whereBuilder.Append(" and w.servicenetworkid ='" + job["servicenetworkid"] + "'");
            //    }
            //    //客户id
            //    if (job["customerid"] != null && !string.IsNullOrEmpty(job["customerid"].ToString()))
            //    {
            //        whereBuilder.Append(" and w.customerid ='" + job["customerid"] + "'");
            //    }
            //    //评价标识
            //    if (job["isevaluated"] != null)
            //    {
            //        whereBuilder.Append(" and w.isevaluated ='" + job["isevaluated"] + "'");
            //    }
            //    //产品id
            //    if (job["productid"] != null && !string.IsNullOrEmpty(job["productid"].ToString()))
            //    {
            //        whereBuilder.Append(" and w.productID ='" + job["productid"] + "'");
            //    }

            //    //工单状态
            //    if (job["state"] != null && job["state"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and w.State in('" + string.Join("','", job["state"]) + "')");
            //    }

            //    //工单待处理状态
            //    if (job["pendstate"] != null && job["pendstate"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and w.pendstate in('" + string.Join("','", job["pendstate"]) + "')");
            //    }

            //    //工单渠道
            //    if (job["channel"] != null && job["channel"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and w.channel in('" + string.Join("','", job["channel"]) + "')");
            //    }

            //    //结算状态
            //    if (job["statementstate"] != null && job["statementstate"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and w.statementstate in('" + string.Join("','", job["statementstate"]) + "')");
            //    }

            //    //工单类型
            //    if (job["servicecategoryid"] != null && !string.IsNullOrEmpty(job["servicecategoryid"].ToString()))
            //    {
            //        whereBuilder.Append(" and w.servicecategoryid =('" + job["servicecategoryid"].ToString() + "')");
            //    }

            //    //服务模式
            //    if (job["servicemodeid"] != null && !string.IsNullOrEmpty(job["servicemodeid"].ToString()))
            //    {
            //        whereBuilder.Append(" and w.servicemodeid =('" + job["servicemodeid"].ToString() + "')");
            //    }

            //    //创建时间
            //    if (job["startdate"] != null && !string.IsNullOrEmpty(job["startdate"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), w.createtime, 120)>='" + job["startdate"].ToString() + "'");
            //    }

            //    if (job["enddate"] != null && !string.IsNullOrEmpty(job["enddate"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), w.createtime, 120)<='" + job["enddate"].ToString() + "'");
            //    }
            //    //下一个处理者
            //    if (job["nexthandlerid"] != null && !string.IsNullOrEmpty(job["nexthandlerid"].ToString()))
            //    {
            //        whereBuilder.Append(" and  w.nexthandlerid='" + job["nexthandlerid"].ToString() + "'");
            //    }
            //    //当前处理者
            //    if (job["currenthandlerid"] != null && !string.IsNullOrEmpty(job["currenthandlerid"].ToString()))
            //    {
            //        whereBuilder.Append(" and  w.currenthandlerid='" + job["currenthandlerid"].ToString() + "'");
            //    }
            //    //服务人员
            //    if (job["servicestaffid"] != null && !string.IsNullOrEmpty(job["servicestaffid"].ToString()))
            //    {
            //        whereBuilder.Append(" and  w.servicestaffid='" + job["servicestaffid"].ToString() + "'");
            //    }
            //    //删除标识
            //    if (job["isdeleted"] != null && !string.IsNullOrEmpty(job["isdeleted"].ToString()))
            //    {
            //        whereBuilder.Append(" and  w.IsDeleted='" + job["isdeleted"].ToString() + "'");
            //    }

            //    //是否结算
            //    if (job["needstatemented"] != null && !string.IsNullOrEmpty(job["needstatemented"].ToString()))
            //    {
            //        whereBuilder.Append(" and  w.needstatemented='" + job["needstatemented"].ToString() + "'");
            //    }

            //    //接收时间
            //    if (job["receivestartdate"] != null && !string.IsNullOrEmpty(job["receivestartdate"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), w.ReceivedTime, 120)>='" + job["receivestartdate"].ToString() + "'");
            //    }

            //    if (job["receiveenddate"] != null && !string.IsNullOrEmpty(job["receiveenddate"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), w.ReceivedTime, 120)<='" + job["receiveenddate"].ToString() + "'");
            //    }

            //    //完成时间
            //    if (job["finishstartdate"] != null && !string.IsNullOrEmpty(job["finishstartdate"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), w.FinishTime, 120)>='" + job["finishstartdate"].ToString() + "'");
            //    }

            //    if (job["finishenddate"] != null && !string.IsNullOrEmpty(job["finishenddate"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), w.FinishTime, 120)<='" + job["finishenddate"].ToString() + "'");
            //    }

            //    //新增筛选集合条件

            //    //工单类型集合
            //    if (job["categorylist"] != null && job["categorylist"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and w.ServiceCategoryID in('" + string.Join("','", job["categorylist"]) + "')");
            //    }


            //    //产品集合
            //    if (job["prolist"] != null && job["prolist"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and w.productID in('" + string.Join("','", job["prolist"]) + "')");
            //    }


            //    //服务网点集合
            //    if (job["networklist"] != null && job["networklist"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and w.serviceNetworkId in('" + string.Join("','", job["networklist"]) + "')");
            //    }

            //    //评价标识
            //    if (job["isevaluated"] != null && !string.IsNullOrEmpty(job["isevaluated"].ToString()))
            //    {
            //        whereBuilder.Append(" and  w.isevaluated='" + job["isevaluated"].ToString() + "'");
            //    }

            //    //服务网点集合
            //    if (job["servicemodelist"] != null && job["servicemodelist"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and w.servicemodeid in('" + string.Join("','", job["servicemodelist"]) + "')");
            //    }

            //    //服务人员集合
            //    if (job["servicepersonlist"] != null && job["servicepersonlist"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and w.servicestaffid in('" + string.Join("','", job["servicepersonlist"]) + "')");
            //    }

            //    //服务结束标示
            //    if (job["isserviceend"] != null && !string.IsNullOrEmpty(job["isserviceend"].ToString()))
            //    {
            //        whereBuilder.Append(" and  w.isserviceend='" + job["isserviceend"].ToString() + "'");
            //    }

            //    //过滤
            //    if (job["filter"] != null && !string.IsNullOrEmpty(job["filter"].ToString()))
            //    {
            //        if (job["filter"].ToString() == "unfinished")
            //        {
            //            //whereBuilder.Append(" and  w.state <> " + (int)WorkOrderState.Canceled);
            //        }
            //    }
           // }

            return whereBuilder.ToString();
        }


        /// <summary>
        /// 获取where条件语句
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns></returns>
        private string GetProWhereStr(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return string.Empty;
            StringBuilder whereBuilder = new StringBuilder();
            //JObject job = (JObject)JsonConvert.DeserializeObject(jsonStr);

            //if (job != null && job.Count > 0)
            //{

            //    //服务空间id
            //    if (job["servicenetworkid"] != null && !string.IsNullOrEmpty(job["servicenetworkid"].ToString()))
            //    {
            //        whereBuilder.Append(" and wp.servicenetworkid ='" + job["servicenetworkid"] + "'");
            //    }

            //    //当前处理人员Id
            //    if (job["staffid"] != null && !string.IsNullOrEmpty(job["staffid"].ToString()))
            //    {
            //        whereBuilder.Append(" and wp.staffid =('" + job["staffid"].ToString() + "')");
            //    }

            //    //工单Id
            //    if (job["workorderid"] != null && !string.IsNullOrEmpty(job["workorderid"].ToString()))
            //    {
            //        whereBuilder.Append(" and wp.workorderid =('" + job["workorderid"].ToString() + "')");
            //    }

            //    //服务时间
            //    if (job["startdate"] != null && !string.IsNullOrEmpty(job["startdate"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), wp.HandedTime, 120)>='" + job["startdate"].ToString() + "'");
            //    }

            //    if (job["enddate"] != null && !string.IsNullOrEmpty(job["enddate"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), wp.HandedTime, 120)<='" + job["enddate"].ToString() + "'");
            //    }
            //}

            return whereBuilder.ToString();
        }


        /// <summary>
        /// 获取where条件语句
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns></returns>
        private string GetPendWhereStr(string jsonStr)
        {
            //if (string.IsNullOrEmpty(jsonStr))
            //    return string.Empty;
            //StringBuilder whereBuilder = new StringBuilder();
            //JObject job = (JObject)JsonConvert.DeserializeObject(jsonStr);



            //if (job != null && job.Count > 0)
            //{

            //    //服务空间id
            //    if (job["servicespaceid"] != null && !string.IsNullOrEmpty(job["servicespaceid"].ToString()))
            //    {
            //        whereBuilder.Append(" and pw.servicespaceid ='" + job["servicespaceid"] + "'");
            //    }

            //    //服务网点Id
            //    if (job["servicenetworkid"] != null && !string.IsNullOrEmpty(job["servicenetworkid"].ToString()))
            //    {
            //        whereBuilder.Append(" and servicenetworkid =('" + job["servicenetworkid"].ToString() + "')");
            //    }

            //    //工单Id
            //    if (job["workorderid"] != null && !string.IsNullOrEmpty(job["workorderid"].ToString()))
            //    {
            //        whereBuilder.Append(" and workorderid =('" + job["workorderid"].ToString() + "')");
            //    }

            //    //服务人员Id
            //    if (job["servicestaffid"] != null && !string.IsNullOrEmpty(job["servicestaffid"].ToString()))
            //    {
            //        whereBuilder.Append(" and servicestaffid =('" + job["servicestaffid"].ToString() + "')");
            //    }


            //    //工单状态
            //    if (job["state"] != null && job["state"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and state in('" + string.Join("','", job["state"]) + "')");
            //    }

            //    //工单待处理状态
            //    if (job["pendstate"] != null && job["pendstate"].Count() > 0)
            //    {
            //        whereBuilder.Append(" and pendstate in('" + string.Join("','", job["pendstate"]) + "')");
            //    }

            //}

            return "";// whereBuilder.ToString();
        }

        /// <summary>
        /// 从reader中读取工单集合
        /// </summary>
        /// <param name="reader">reader</param>
        /// <returns>工单集合</returns>
        private List<DataEntity> getWorkOrdersFromReader(DbDataReader reader)
        {
            List<DataEntity> workOrders = new List<DataEntity>();
            DataEntity workOrder = null;
            while (reader.Read())
            {
                workOrder = new DataEntity();
                #region 组装数据
                workOrder.Add("AbnormalMsg", ReaderExists(reader, "AbnormalMsg") && DBNull.Value != reader["AbnormalMsg"] ? reader["AbnormalMsg"].ToString() : "");
                workOrder.Add("AcceptTime", ReaderExists(reader, "AcceptTime") && DBNull.Value != reader["AcceptTime"] && !reader["AcceptTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["AcceptTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("AbnormalTime", ReaderExists(reader, "AbnormalTime") && DBNull.Value != reader["AbnormalTime"] && !reader["AbnormalTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["AbnormalTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("AssignTime", ReaderExists(reader, "AssignTime") && DBNull.Value != reader["AssignTime"] && !reader["AssignTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["AssignTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("CustomerPhone", ReaderExists(reader, "CustomerPhone") && DBNull.Value != reader["CustomerPhone"] ? reader["CustomerPhone"].ToString() : "");
                workOrder.Add("CallCount", ReaderExists(reader, "CallCount") && DBNull.Value != reader["CallCount"] ? Convert.ToInt32(reader["CallCount"]) : 0);
                workOrder.Add("Caption", ReaderExists(reader, "Caption") && DBNull.Value != reader["Caption"] ? reader["Caption"].ToString() : "");
                workOrder.Add("CurrentHandlerID", ReaderExists(reader, "CurrentHandlerID") && DBNull.Value != reader["CurrentHandlerID"] ? reader["CurrentHandlerID"].ToString() : "");
                workOrder.Add("CurrentHandlerName", ReaderExists(reader, "CurrentHandlerName") && DBNull.Value != reader["CurrentHandlerName"] ? reader["CurrentHandlerName"].ToString() : "");
                workOrder.Add("CustomerAddress", ReaderExists(reader, "CustomerAddress") && DBNull.Value != reader["CustomerAddress"] ? reader["CustomerAddress"].ToString() : "");
                workOrder.Add("CustomerArea", ReaderExists(reader, "CustomerArea") && DBNull.Value != reader["CustomerArea"] ? reader["CustomerArea"].ToString() : "");
                
                workOrder.Add("CustomerID", ReaderExists(reader, "CustomerID") && DBNull.Value != reader["CustomerID"] ? reader["CustomerID"].ToString() : "");
                workOrder.Add("CustomerName", ReaderExists(reader, "CustomerName") && DBNull.Value != reader["CustomerName"] ? reader["CustomerName"].ToString() : "");
                workOrder.Add("Contact", ReaderExists(reader, "Contact") && DBNull.Value != reader["Contact"] ? reader["Contact"].ToString() : "");
                workOrder.Add("ContactNumber", ReaderExists(reader, "ContactNumber") && DBNull.Value != reader["ContactNumber"] ? reader["ContactNumber"].ToString() : "");
                workOrder.Add("CustomFields", ReaderExists(reader, "CustomFields") && DBNull.Value != reader["CustomFields"] ? reader["CustomFields"].ToString() : "");
                workOrder.Add("CreateTime", ReaderExists(reader, "CreateTime") && DBNull.Value != reader["CreateTime"] ? Convert.ToDateTime(reader["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("Channel", ReaderExists(reader, "Channel") && DBNull.Value != reader["Channel"] ? Convert.ToInt32(reader["Channel"]) : 0);
                workOrder.Add("Description", ReaderExists(reader, "Description") && DBNull.Value != reader["Description"] ? reader["Description"].ToString() : "");
                workOrder.Add("DispatchTime", ReaderExists(reader, "DispatchTime") && DBNull.Value != reader["DispatchTime"] && !reader["DispatchTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["DispatchTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("FaultCategoryID", ReaderExists(reader, "FaultCategoryID") && DBNull.Value != reader["FaultCategoryID"] ? reader["FaultCategoryID"].ToString() : "");
                workOrder.Add("FaultCategoryName", ReaderExists(reader, "FaultCategoryName") && DBNull.Value != reader["FaultCategoryName"] ? reader["FaultCategoryName"].ToString() : "");
                workOrder.Add("FinishTime", ReaderExists(reader, "FinishTime") && DBNull.Value != reader["FinishTime"] && !reader["FinishTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["FinishTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("GuaranteedState", ReaderExists(reader, "GuaranteedState") && DBNull.Value != reader["GuaranteedState"] ? Convert.ToInt32(reader["GuaranteedState"]) : 0);
                workOrder.Add("ID", DBNull.Value != reader["ID"] ? reader["ID"].ToString() : "");
                workOrder.Add("IsDeleted", ReaderExists(reader, "IsDeleted") && DBNull.Value != reader["CreateTime"] ? (bool)reader["IsDeleted"] : false);

                workOrder.Add("IsEvaluated", ReaderExists(reader, "IsEvaluated") && DBNull.Value != reader["IsEvaluated"] ? (bool)reader["IsEvaluated"] : false);
                workOrder.Add("Number", DBNull.Value != reader["Number"] ? reader["Number"].ToString() : "");
                workOrder.Add("NextHandlerID", ReaderExists(reader, "NextHandlerID") && DBNull.Value != reader["NextHandlerID"] ? reader["NextHandlerID"].ToString() : "");
                workOrder.Add("NeedStatemented", ReaderExists(reader, "NeedStatemented") && DBNull.Value != reader["NeedStatemented"] ? (bool)reader["NeedStatemented"] : false);
                workOrder.Add("OriginalNetworkID", ReaderExists(reader, "OriginalNetworkID") && DBNull.Value != reader["OriginalNetworkID"] ? reader["OriginalNetworkID"].ToString() : "");
                workOrder.Add("OriginalNetworkName", ReaderExists(reader, "OriginalNetworkName") && DBNull.Value != reader["OriginalNetworkName"] ? reader["OriginalNetworkName"].ToString() : "");
                workOrder.Add("ProductID", DBNull.Value != reader["ProductID"] ? reader["ProductID"].ToString() : "");
                workOrder.Add("ProductType", ReaderExists(reader, "ProductType") && DBNull.Value != reader["ProductType"] ? reader["ProductType"].ToString() : "");
                workOrder.Add("ProductName", ReaderExists(reader, "ProductName") && DBNull.Value != reader["ProductName"] ? reader["ProductName"].ToString() : "");
                workOrder.Add("ProductCategoryID", ReaderExists(reader, "ProductCategoryID") && DBNull.Value != reader["ProductCategoryID"] ? reader["ProductCategoryID"].ToString() : "");
                workOrder.Add("ProductCategoryName", ReaderExists(reader, "ProductCategoryName") && DBNull.Value != reader["ProductCategoryName"] ? reader["ProductCategoryName"].ToString() : "");
                
                workOrder.Add("ProcessDesc", ReaderExists(reader, "ProcessDesc") && DBNull.Value != reader["ProcessDesc"] ? reader["ProcessDesc"].ToString() : "");
                workOrder.Add("Priority", ReaderExists(reader, "Priority") && DBNull.Value != reader["Priority"] ? Convert.ToInt32(reader["Priority"]) : 0);
                workOrder.Add("QualityCheckTime", ReaderExists(reader, "QualityCheckTime") && DBNull.Value != reader["QualityCheckTime"] && !reader["QualityCheckTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["QualityCheckTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("ReceivedTime", ReaderExists(reader, "ReceivedTime") && DBNull.Value != reader["ReceivedTime"] && !reader["ReceivedTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["ReceivedTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("CancelTime", ReaderExists(reader, "CancelTime") && DBNull.Value != reader["CancelTime"] && !reader["CancelTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["CancelTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("ReportTime", ReaderExists(reader, "ReportTime") && DBNull.Value != reader["ReportTime"] && !reader["ReportTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["ReportTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("ReturnVisitTime", ReaderExists(reader, "ReturnVisitTime") && DBNull.Value != reader["ReturnVisitTime"] && !reader["ReturnVisitTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["ReturnVisitTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("ServiceSpaceID", ReaderExists(reader, "ServiceSpaceID") && DBNull.Value != reader["ServiceSpaceID"] ? reader["ServiceSpaceID"].ToString() : "");
                workOrder.Add("ServiceSpaceName", ReaderExists(reader, "ServiceSpaceName") && DBNull.Value != reader["ServiceSpaceName"] ? reader["ServiceSpaceName"].ToString() : "");
                workOrder.Add("Source", ReaderExists(reader, "Source") && DBNull.Value != reader["Source"] ? Convert.ToInt32(reader["Source"]) : 0);
                workOrder.Add("ServiceNetworkID", ReaderExists(reader, "ServiceNetworkID") && DBNull.Value != reader["ServiceNetworkID"] ? reader["ServiceNetworkID"].ToString() : "");
                workOrder.Add("ServiceNetworkName", ReaderExists(reader, "ServiceNetworkName") && DBNull.Value != reader["ServiceNetworkName"] ? reader["ServiceNetworkName"].ToString() : "");
                workOrder.Add("ServiceStaffID", ReaderExists(reader, "ServiceStaffID") && DBNull.Value != reader["ServiceStaffID"] ? reader["ServiceStaffID"].ToString() : "");
                workOrder.Add("ServiceStaffName", ReaderExists(reader, "ServiceStaffName") && DBNull.Value != reader["ServiceStaffName"] ? reader["ServiceStaffName"].ToString() : "");
                workOrder.Add("ServiceCategoryID", DBNull.Value != reader["ServiceCategoryID"] ? reader["ServiceCategoryID"].ToString() : "");
                workOrder.Add("ServiceCategoryName", DBNull.Value != reader["ServiceCategoryID"] ? reader["ServiceCategoryName"].ToString() : "");
                workOrder.Add("ServiceModeID", ReaderExists(reader, "ServiceModeID") && DBNull.Value != reader["ServiceModeID"] ? reader["ServiceModeID"].ToString() : "");
                workOrder.Add("ServiceModeName", ReaderExists(reader, "ServiceModeName") && DBNull.Value != reader["ServiceModeName"] ? reader["ServiceModeName"].ToString() : "");
                workOrder.Add("State", DBNull.Value != reader["State"] ? Convert.ToInt32(reader["State"]) : 0);
                workOrder.Add("PendState", DBNull.Value != reader["PendState"] ? Convert.ToInt32(reader["PendState"]) : 0);
                workOrder.Add("PrevOperation", ReaderExists(reader, "PrevOperation") && DBNull.Value != reader["PrevOperation"] ? reader["PrevOperation"].ToString() : "");
                workOrder.Add("StatementState", ReaderExists(reader, "StatementState") && DBNull.Value != reader["StatementState"] ? Convert.ToInt32(reader["StatementState"]) : 0);
                workOrder.Add("SubscribeTime", ReaderExists(reader, "SubscribeTime") && DBNull.Value != reader["SubscribeTime"] && !reader["SubscribeTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["SubscribeTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("SubscribeServiceTime", ReaderExists(reader, "SubscribeServiceTime") && DBNull.Value != reader["SubscribeServiceTime"] && !reader["SubscribeServiceTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["SubscribeServiceTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("Signature", ReaderExists(reader, "Signature") && DBNull.Value != reader["Signature"] ? reader["Signature"].ToString() : "");
                workOrder.Add("StartServiceTime", ReaderExists(reader, "StartServiceTime") && DBNull.Value != reader["StartServiceTime"] && !reader["StartServiceTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["StartServiceTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("StatementTime", ReaderExists(reader, "StatementTime") && DBNull.Value != reader["StatementTime"] && !reader["StatementTime"].ToString().Contains("1900") ? Convert.ToDateTime(reader["StatementTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                workOrder.Add("Remark", ReaderExists(reader, "Remark") && DBNull.Value != reader["Remark"] ? reader["Remark"].ToString() : "");
                workOrder.Add("IsServiceEnd", ReaderExists(reader, "IsServiceEnd") && DBNull.Value != reader["IsServiceEnd"] ? (bool)reader["IsServiceEnd"] : false);
                workOrder.Add("IsHasReport", ReaderExists(reader, "IsHasReport") && DBNull.Value != reader["IsHasReport"] ? (bool)reader["IsHasReport"] : false);
                workOrder.Add("IsSubmitImg", ReaderExists(reader, "IsSubmitImg") && DBNull.Value != reader["IsSubmitImg"] ? (bool)reader["IsSubmitImg"] : false);
                workOrder.Add("IsCustomerEvaluate", ReaderExists(reader, "IsCustomerEvaluate") && DBNull.Value != reader["IsCustomerEvaluate"] ? (bool)reader["IsCustomerEvaluate"] : false);
                #endregion
                workOrders.Add(workOrder);
            }
            return workOrders;
        }

        #endregion

        #region 工单编辑
        /// <summary>
        /// 修改工单信息
        /// </summary>
        /// <param name="WorkOrder">工单信息</param>
        /// <returns>修改成功标识</returns>
        public bool Update(DataEntity WorkOrder)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update [WorkOrder]
                              Set NeedStatemented=@NeedStatemented,CallCount=@CallCount,ServiceNetworkID=@ServiceNetworkID,ServiceStaffID=@ServiceStaffID,ProductID=@ProductID,ServiceCategoryID=@ServiceCategoryID,ServiceModeID=@ServiceModeID,FaultCategoryID=@FaultCategoryID,
                                  CurrentHandlerID=@CurrentHandlerID,NextHandlerID=@NextHandlerID,Caption=@Caption,Description=@Description,ProcessDesc=@ProcessDesc,StatementState=@StatementState,
                                  State=@State,PendState=@PendState,PrevOperation=@PrevOperation,Priority=@Priority,Contact=@Contact,ContactNumber=@ContactNumber,GuaranteedState=@GuaranteedState,
                                  IsDeleted=@IsDeleted,StatementTime=@StatementTime,StartServiceTime=@StartServiceTime,SubscribeServiceTime=@SubscribeServiceTime,ReturnVisitTime=@ReturnVisitTime,QualityCheckTime=@QualityCheckTime,FinishTime=@FinishTime,AbnormalMsg=@AbnormalMsg,AbnormalTime=@AbnormalTime,AcceptTime=@AcceptTime,AssignTime=@AssignTime,DispatchTime=@DispatchTime,ReceivedTime=@ReceivedTime,CancelTime=@CancelTime,SubscribeTime=@SubscribeTime,ReportTime=@ReportTime,CustomFields=@CustomFields,IsEvaluated=@IsEvaluated,Signature=@Signature,CustomerID=@CustomerID,Remark=@Remark,IsServiceEnd=@IsServiceEnd,IsHasReport=@IsHasReport,IsSubmitImg=@IsSubmitImg,IsCustomerEvaluate=@IsCustomerEvaluate 
                              Where ID=@ID";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@AcceptTime", WorkOrder["AcceptTime"].Value));
            parametersList.Add(new SqlParameter("@AbnormalTime", WorkOrder["AbnormalTime"].Value));
            parametersList.Add(new SqlParameter("@AssignTime", WorkOrder["AssignTime"].Value));
            parametersList.Add(new SqlParameter("@AbnormalMsg", WorkOrder["AbnormalMsg"].Value));
            parametersList.Add(new SqlParameter("@Contact", WorkOrder["Contact"].Value));
            parametersList.Add(new SqlParameter("@ContactNumber", WorkOrder["ContactNumber"].Value));
            parametersList.Add(new SqlParameter("@CurrentHandlerID", WorkOrder["CurrentHandlerID"].Value));
            parametersList.Add(new SqlParameter("@Caption", WorkOrder["Caption"].Value));
            parametersList.Add(new SqlParameter("@CallCount", WorkOrder["CallCount"].Value));
            parametersList.Add(new SqlParameter("@CustomFields", WorkOrder["CustomFields"].Value));
            parametersList.Add(new SqlParameter("@Description", WorkOrder["Description"].Value));
            parametersList.Add(new SqlParameter("@DispatchTime", WorkOrder["DispatchTime"].Value));
            parametersList.Add(new SqlParameter("@FaultCategoryID", WorkOrder["FaultCategoryID"].Value));
            parametersList.Add(new SqlParameter("@FinishTime", WorkOrder["FinishTime"].Value));
            parametersList.Add(new SqlParameter("@GuaranteedState", WorkOrder["GuaranteedState"].Value));
            parametersList.Add(new SqlParameter("@ID", WorkOrder["ID"].Value));
            parametersList.Add(new SqlParameter("@IsDeleted", WorkOrder["IsDeleted"].Value));
            parametersList.Add(new SqlParameter("@IsEvaluated", WorkOrder["IsEvaluated"].Value));
            parametersList.Add(new SqlParameter("@NextHandlerID", WorkOrder["NextHandlerID"].Value));
            parametersList.Add(new SqlParameter("@NeedStatemented", WorkOrder["NeedStatemented"].Value));
            parametersList.Add(new SqlParameter("@ProductID", WorkOrder["ProductID"].Value));
            parametersList.Add(new SqlParameter("@ProcessDesc", WorkOrder["ProcessDesc"].Value));
            parametersList.Add(new SqlParameter("@Priority", WorkOrder["Priority"].Value));
            parametersList.Add(new SqlParameter("@QualityCheckTime", WorkOrder["QualityCheckTime"].Value));
            parametersList.Add(new SqlParameter("@ReceivedTime", WorkOrder["ReceivedTime"].Value));
            parametersList.Add(new SqlParameter("@ReturnVisitTime", WorkOrder["ReturnVisitTime"].Value));
            parametersList.Add(new SqlParameter("@ReportTime", WorkOrder["ReportTime"].Value));
            parametersList.Add(new SqlParameter("@CancelTime", WorkOrder["CancelTime"].Value));
            parametersList.Add(new SqlParameter("@ServiceNetworkID", WorkOrder["ServiceNetworkID"].Value));
            parametersList.Add(new SqlParameter("@ServiceStaffID", WorkOrder["ServiceStaffID"].Value));
            parametersList.Add(new SqlParameter("@ServiceCategoryID", WorkOrder["ServiceCategoryID"].Value));
            parametersList.Add(new SqlParameter("@ServiceModeID", WorkOrder["ServiceModeID"].Value));
            parametersList.Add(new SqlParameter("@StatementState", WorkOrder["StatementState"].Value));
            parametersList.Add(new SqlParameter("@State", WorkOrder["State"].Value));
            parametersList.Add(new SqlParameter("@PendState", WorkOrder["PendState"].Value));
            parametersList.Add(new SqlParameter("@PrevOperation", WorkOrder["PrevOperation"].Value));
            parametersList.Add(new SqlParameter("@SubscribeTime", WorkOrder["SubscribeTime"].Value));
            parametersList.Add(new SqlParameter("@SubscribeServiceTime", WorkOrder["SubscribeServiceTime"].Value));
            parametersList.Add(new SqlParameter("@StartServiceTime", WorkOrder["StartServiceTime"].Value));
            parametersList.Add(new SqlParameter("@StatementTime", WorkOrder["StatementTime"].Value));
            parametersList.Add(new SqlParameter("@Signature", WorkOrder["Signature"].Value));
            parametersList.Add(new SqlParameter("@CustomerID", WorkOrder["CustomerID"].Value));
            parametersList.Add(new SqlParameter("@Remark", WorkOrder["Remark"].Value));
            parametersList.Add(new SqlParameter("@IsServiceEnd", WorkOrder["IsServiceEnd"].Value));
            parametersList.Add(new SqlParameter("@IsHasReport", WorkOrder["IsHasReport"].Value));
            parametersList.Add(new SqlParameter("@IsSubmitImg", WorkOrder["IsSubmitImg"].Value));
            parametersList.Add(new SqlParameter("@IsCustomerEvaluate", WorkOrder["IsCustomerEvaluate"].Value));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除工单（对于客户端）
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns></returns>
        public bool Delete(string workOrderId)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update [WorkOrder]
                              Set IsDeleted=1
                              Where ID=@ID";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", workOrderId));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }


        /// <summary>
        /// 删除待处理工单（用于app端工单动态提醒）
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <returns></returns>
        public bool DeletePendWork(string workOrderId)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"delete [PendWorkOrder]
                              Where workOrderId=@workOrderId";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@workOrderId", workOrderId));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 添加待处理信息
        /// </summary>
        /// <param name="workOrder">工单信息</param>
        /// <returns></returns>
        public bool AddPendWork(DataEntity workOrder)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"delete PendWorkOrder where WorkOrderID=@WorkOrderID and State<>@State;
                              Insert Into [PendWorkOrder]([WorkOrderID],[CustomerID],[WorkOrderNumber],[ServiceSpaceID],[ServiceNetworkID],[ServiceCategoryID],[ServiceModeID],[ServiceStaffID],[ProductID],[State],PendState) Values(@WorkOrderID,@CustomerID,@WorkOrderNumber,@ServiceSpaceID,@ServiceNetworkID,@ServiceCategoryID,@ServiceModeID,@ServiceStaffID,@ProductID,@State,@PendState)";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@WorkOrderID", workOrder["WorkOrderID"].Value));
            parametersList.Add(new SqlParameter("@CustomerID", workOrder["CustomerID"].Value));
            parametersList.Add(new SqlParameter("@WorkOrderNumber", workOrder["WorkOrderNumber"].Value));
            parametersList.Add(new SqlParameter("@ServiceSpaceID", workOrder["ServiceSpaceID"].Value));
            parametersList.Add(new SqlParameter("@ServiceNetworkID", workOrder["ServiceNetworkID"].Value));
            parametersList.Add(new SqlParameter("@ServiceCategoryID", workOrder["ServiceCategoryID"].Value));
            parametersList.Add(new SqlParameter("@ServiceModeID", workOrder["ServiceModeID"].Value));
            parametersList.Add(new SqlParameter("@ServiceStaffID", workOrder["ServiceStaffID"].Value));
            parametersList.Add(new SqlParameter("@ProductID", workOrder["ProductID"].Value));
            parametersList.Add(new SqlParameter("@State", workOrder["State"].Value));
            parametersList.Add(new SqlParameter("@PendState", workOrder["PendState"].Value));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }


        /// <summary>
        /// 添加工单信息
        /// </summary>
        /// <param name="workOrder">工单信息</param>
        /// <returns>添加成功标识</returns>
        public bool Add(DataEntity workOrder)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into WorkOrder(ID,Number,ServiceCategoryID,OriginalNetworkID,ServiceSpaceID,ServiceNetworkID,ServiceStaffID,CustomerID,ProductID,CurrentHandlerID ,NextHandlerID ,Caption,Description,Source,Channel,State,PendState,Priority,Contact,ContactNumber,CustomFields,Remark,StatementState,IsSubmitImg,ServiceModeID)
                                             Values(@ID,@Number,@ServiceCategoryID,@OriginalNetworkID,@ServiceSpaceID,@ServiceNetworkID,@ServiceStaffID,@CustomerID,@ProductID ,@CurrentHandlerID,@NextHandlerID,@Caption,@Description,@Source,@Channel,@State,@PendState,@Priority,@Contact,@ContactNumber,@CustomFields,@Remark,@StatementState,@IsSubmitImg,@ServiceModeID) ";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", workOrder["ID"].Value));
            parametersList.Add(new SqlParameter("@Number", workOrder["Number"].Value));
            parametersList.Add(new SqlParameter("@ServiceCategoryID", workOrder["ServiceCategoryID"].Value));
            parametersList.Add(new SqlParameter("@ServiceSpaceID", workOrder["ServiceSpaceID"].Value));
            parametersList.Add(new SqlParameter("@OriginalNetworkID", workOrder["OriginalNetworkID"].Value));
            parametersList.Add(new SqlParameter("@ServiceNetworkID", workOrder["ServiceNetworkID"].Value));
            parametersList.Add(new SqlParameter("@ServiceStaffID", workOrder["ServiceStaffID"].Value));
            parametersList.Add(new SqlParameter("@CustomerID", workOrder["CustomerID"].Value));
            parametersList.Add(new SqlParameter("@ProductID", workOrder["ProductID"].Value));
            parametersList.Add(new SqlParameter("@CurrentHandlerID", workOrder["CurrentHandlerID"].Value));
            parametersList.Add(new SqlParameter("@NextHandlerID", workOrder["NextHandlerID"].Value));
            parametersList.Add(new SqlParameter("@Caption", workOrder["Caption"].Value));
            parametersList.Add(new SqlParameter("@Description", workOrder["Description"].Value));
            parametersList.Add(new SqlParameter("@Source", workOrder["Source"].Value));
            parametersList.Add(new SqlParameter("@Channel", workOrder["Channel"].Value));
            parametersList.Add(new SqlParameter("@State", workOrder["State"].Value));
            parametersList.Add(new SqlParameter("@PendState", workOrder["PendState"].Value));
            parametersList.Add(new SqlParameter("@Priority", workOrder["Priority"].Value));
            parametersList.Add(new SqlParameter("@Contact", workOrder["Contact"].Value));
            parametersList.Add(new SqlParameter("@ContactNumber", workOrder["ContactNumber"].Value));
            parametersList.Add(new SqlParameter("@CustomFields", workOrder["CustomFields"].Value));
            parametersList.Add(new SqlParameter("@Remark", workOrder["Remark"].Value));
            parametersList.Add(new SqlParameter("@StatementState", workOrder["StatementState"].Value));
            parametersList.Add(new SqlParameter("@IsSubmitImg", workOrder["IsSubmitImg"].Value));
            parametersList.Add(new SqlParameter("@ServiceModeID", string.Empty));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 改变工单的已读状态
        /// </summary>
        /// <param name="staffId">员工id</param>
        /// <param name="id">工单id</param>
        /// <returns></returns>
        public bool ChangeReadState(string staffId, string workOrderId, int state)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update [PendWorkorder]
                              Set IsReaded=1
                              Where workOrderId=@workOrderId and ServiceStaffID=@ServiceStaffID and State=@state";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@workOrderId", workOrderId));
            parametersList.Add(new SqlParameter("@ServiceStaffID", staffId));
            parametersList.Add(new SqlParameter("@state", state));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion

        #region 处理记录
        /// <summary>
        /// 添加工单处理记录
        /// </summary>
        /// <param name="workOrderProceses">工单处理记录</param>
        /// <returns>成功标识</returns>
        public bool AddProceses(DataEntity workOrderProceses)
        {

            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into [WorkOrderProcess]( [ID] ,[WorkOrderID] ,[Action] ,[Description],[ServiceNetworkID] ,[ServiceNetworkName] ,[StaffID] ,[StaffName] ,[NextHandlerID] ,[NextHandlerName] ,[Remark] ,IsTimeOut,ServiceEndTime)
                              Values(@ID,@WorkOrderID,@Action,@Description,@ServiceNetworkID,@ServiceNetworkName,@StaffID,@StaffName,@NextHandlerID,@NextHandlerName,@Remark,@IsTimeOut,@ServiceEndTime) ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", workOrderProceses["ID"].Value));
            parametersList.Add(new SqlParameter("@WorkOrderID", workOrderProceses["WorkOrderID"].Value));
            parametersList.Add(new SqlParameter("@Action", workOrderProceses["Action"].Value));
            parametersList.Add(new SqlParameter("@Description", workOrderProceses["Description"].Value));
            parametersList.Add(new SqlParameter("@ServiceNetworkID", workOrderProceses["ServiceNetworkID"].Value));
            parametersList.Add(new SqlParameter("@ServiceNetworkName", workOrderProceses["ServiceNetworkName"].Value));
            parametersList.Add(new SqlParameter("@StaffID", workOrderProceses["StaffID"].Value));
            parametersList.Add(new SqlParameter("@StaffName", workOrderProceses["StaffName"].Value));
            parametersList.Add(new SqlParameter("@NextHandlerID", workOrderProceses["NextHandlerID"].Value));
            parametersList.Add(new SqlParameter("@NextHandlerName", workOrderProceses["NextHandlerName"].Value));
            parametersList.Add(new SqlParameter("@Remark", workOrderProceses["Remark"].Value));
            parametersList.Add(new SqlParameter("@IsTimeOut", workOrderProceses["IsTimeOut"].Value));
            parametersList.Add(new SqlParameter("@ServiceEndTime", workOrderProceses["ServiceEndTime"].Value));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 获取工单处理记录
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <returns>工单处理记录集合</returns>
        public List<DataEntity> GetProceses(string workOrderId)
        {
            List<DataEntity> proceses = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT 
                                   w.[ID]
                                  ,w.[WorkOrderID]
                                  ,w.[Action]
                                  ,w.[Description]
                                  ,[HandedTime]
                                  ,[ServiceNetworkID]
                                  ,w.StaffName HandleName
                                  ,sn.Name [ServiceNetworkName]
                                  ,[StaffID]
                                  ,sf.Name [StaffName]
                                  ,[NextHandlerID]
                                  ,[NextHandlerName]
                                  ,w.[Remark]
                                  ,IsTimeOut
                                  ,ServiceEndTime
                            FROM [WorkOrderProcess] w left join serviceNetwork sn on w.ServiceNetworkID=sn.ID left join staff sf on w.staffId=sf.Id 
                            Where [WorkOrderID]=@WorkOrderID order by HandedTime desc";
            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@WorkOrderID", workOrderId));
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                proceses = new List<DataEntity>();
                DataEntity process = null;
                while (reader.Read())
                {
                    process = new DataEntity();
                    if (DBNull.Value != reader["ID"])
                        process.Add("ID", reader["ID"].ToString());
                    else
                        process.Add("ID", string.Empty);
                    if (DBNull.Value != reader["WorkOrderID"])
                        process.Add("WorkOrderID", reader["WorkOrderID"].ToString());
                    else
                        process.Add("WorkOrderID", string.Empty);
                    if (DBNull.Value != reader["Action"])
                        process.Add("Action", Convert.ToInt32(reader["Action"]));
                    else
                        process.Add("Action", 0);
                    if (DBNull.Value != reader["Description"])
                        process.Add("Description", reader["Description"].ToString());
                    else
                        process.Add("Description", string.Empty);
                    if (DBNull.Value != reader["HandedTime"])
                        process.Add("HandedTime", reader["HandedTime"].ToString());
                    else
                        process.Add("HandedTime", string.Empty);
                    if (DBNull.Value != reader["ServiceNetworkID"])
                        process.Add("ServiceNetworkID", reader["ServiceNetworkID"].ToString());
                    else
                        process.Add("ServiceNetworkID", string.Empty);
                    if (DBNull.Value != reader["ServiceNetworkName"])
                        process.Add("ServiceNetworkName", reader["ServiceNetworkName"].ToString());
                    else
                        process.Add("ServiceNetworkName", string.Empty);
                    if (DBNull.Value != reader["StaffID"])
                        process.Add("StaffID", reader["StaffID"].ToString());
                    else
                        process.Add("StaffID", string.Empty);
                    if (DBNull.Value != reader["StaffName"])
                        process.Add("StaffName", reader["StaffName"].ToString());
                    else
                        process.Add("StaffName", string.Empty);
                    if (DBNull.Value != reader["NextHandlerID"])
                        process.Add("NextHandlerID", reader["NextHandlerID"].ToString());
                    else
                        process.Add("NextHandlerID", string.Empty);
                    if (DBNull.Value != reader["NextHandlerName"])
                        process.Add("NextHandlerName", reader["NextHandlerName"].ToString());
                    else
                        process.Add("NextHandlerName", string.Empty);
                    if (DBNull.Value != reader["HandleName"])
                        process.Add("HandleName", reader["HandleName"].ToString());
                    else
                        process.Add("HandleName", string.Empty);
                    if (DBNull.Value != reader["Remark"])
                        process.Add("Remark", reader["Remark"].ToString());
                    else
                        process.Add("Remark", string.Empty);
                    if (DBNull.Value != reader["ServiceEndTime"] && !reader["ServiceEndTime"].ToString().Contains("1900"))
                        process.Add("ServiceEndTime", Convert.ToDateTime(reader["ServiceEndTime"]).ToString("yyyy-MM-dd HH:mm"));
                    else
                        process.Add("ServiceEndTime", string.Empty);

                    process.Add("IsTimeOut", DBNull.Value != reader["IsTimeOut"] ? (bool)reader["IsTimeOut"] : false);
                    proceses.Add(process);
                }
            }

            return proceses;
        }
        #endregion

        #region 工单签到
        /// <summary>
        /// 添加签到记录
        /// </summary>
        /// <param name="signIn">签到信息</param>
        /// <returns>成功标识</returns>
        public bool AddSignIn(DataEntity signIn)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into [SignIn]( [WorkOrderID] ,[StaffID] ,[Address] ,[Title],[WorkProcessID])
                              Values(@WorkOrderID,@StaffID,@Address,@Title,@WorkProcessID) ";

            List<DbParameter> parametersList = new List<DbParameter>();

            parametersList.Add(new SqlParameter("@WorkOrderID", signIn["WorkOrderID"].Value));
            parametersList.Add(new SqlParameter("@StaffID", signIn["StaffID"].Value));
            parametersList.Add(new SqlParameter("@Address", signIn["Address"].Value));
            parametersList.Add(new SqlParameter("@Title", signIn["Title"].Value));
            parametersList.Add(new SqlParameter("@WorkProcessID", signIn["WorkProcessID"].Value));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 根据工单id、服务人员id获取签到信息
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <param name="staffId">服务人员id</param>
        /// <returns>签到信息集合</returns>
        public List<DataEntity> GetSignIns(string workOrderId, string staffId)
        {
            List<DataEntity> signIns = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT 
                                   [ID]
                                  ,[StaffID]
                                  ,[WorkOrderID]
                                  ,[Address]
                                  ,[Title]
                                  ,[WorkProcessID]
                                  ,[CreateTime]
                            FROM [SignIn]
                            Where [StaffID]=@StaffID and WorkOrderID=@WorkOrderID order by [CreateTime] desc";
            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@StaffID", staffId));
            parList.Add(new SqlParameter("@WorkOrderID", workOrderId));
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                signIns = new List<DataEntity>();
                DataEntity signIn = null;
                while (reader.Read())
                {
                    signIn = new DataEntity();
                    signIn.Add("ID", DBNull.Value != reader["ID"] ? reader["ID"].ToString() : string.Empty);
                    signIn.Add("Address", DBNull.Value != reader["Address"] ? reader["Address"].ToString() : string.Empty);
                    signIn.Add("Title", DBNull.Value != reader["Title"] ? reader["Title"].ToString() : string.Empty);
                    signIn.Add("WorkProcessID", DBNull.Value != reader["WorkProcessID"] ? reader["WorkProcessID"].ToString() : string.Empty);
                    signIn.Add("CreateTime", DBNull.Value != reader["CreateTime"] ? Convert.ToDateTime(reader["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                    signIn.Add("StaffID", staffId);
                    signIn.Add("WorkOrderID", workOrderId);
                    signIns.Add(signIn);
                }
            }

            return signIns;
        }
        #endregion

        #region 工单回访
        /// <summary>
        /// 添加回访记录
        /// </summary>
        /// <param name="returnVisit">回访信息</param>
        /// <returns>成功标识</returns>
        public bool AddReturnVisit(DataEntity returnVisit)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into [ReturnVisit]( [WorkOrderID],[IsResolved] ,[IsFeeChecked],Description,OperatorID)
                                      Values(@WorkOrderID,@IsResolved,@IsFeeChecked,@Description,@OperatorID) ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@WorkOrderID", returnVisit["WorkOrderID"].Value));
            parametersList.Add(new SqlParameter("@IsResolved", returnVisit["IsResolved"].Value));
            parametersList.Add(new SqlParameter("@IsFeeChecked", returnVisit["IsFeeChecked"].Value));
            parametersList.Add(new SqlParameter("@Description", returnVisit["Description"].Value));
            parametersList.Add(new SqlParameter("@OperatorID", returnVisit["OperatorID"].Value));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 根据工单id获取回访信息
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <returns>回访信息集合</returns>
        public DataEntity GetReturnVisit(string workOrderId)
        {
            DataEntity returnVisit = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT r.[ID]
                                  ,r.[WorkOrderID]
                                  ,r.[IsResolved]
                                  ,r.[IsFeeChecked]
                                  ,e.OverallLevel [Evalution]
                                  ,r.[Description]
                                  ,r.[CreateTime]
                                  ,r.OperatorID
                                  ,s.Name OperatorName
                            FROM [ReturnVisit] r left Join Staff s on r.OperatorID=s.ID left join [WorkOrderEvaluation] e on r.WorkOrderID=e.WorkOrderID
                            Where r.[WorkOrderID]=@WorkOrderID ";
            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@WorkOrderID", workOrderId));
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                while (reader.Read())
                {
                    returnVisit = new DataEntity();
                    returnVisit.Add("ID", DBNull.Value != reader["ID"] ? reader["ID"].ToString() : string.Empty);
                    returnVisit.Add("IsResolved", DBNull.Value != reader["IsResolved"] ? (bool)reader["IsResolved"] : false);
                    returnVisit.Add("IsFeeChecked", DBNull.Value != reader["IsFeeChecked"] ? (bool)reader["IsFeeChecked"] : false);
                    returnVisit.Add("Evalution", DBNull.Value != reader["Evalution"] ? (int)reader["Evalution"] : 0);
                    returnVisit.Add("Description", DBNull.Value != reader["Description"] ? reader["Description"].ToString() : string.Empty);
                    returnVisit.Add("OperatorID", DBNull.Value != reader["OperatorID"] ? reader["OperatorID"].ToString() : string.Empty);
                    returnVisit.Add("OperatorName", DBNull.Value != reader["OperatorName"] ? reader["OperatorName"].ToString() : string.Empty);
                    returnVisit.Add("CreateTime", DBNull.Value != reader["CreateTime"] ? Convert.ToDateTime(reader["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                    returnVisit.Add("WorkOrderID", workOrderId);

                }
            }

            return returnVisit;
        }

        /// <summary>
        /// 修改回访信息
        /// </summary>
        /// <param name="returnVisit">回访实体</param>
        /// <returns></returns>
        public bool UpdateReturnVisit(DataEntity returnVisit)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update [ReturnVisit] 
                                set  [IsResolved]=@IsResolved
                                    ,[IsFeeChecked]=@IsFeeChecked                               
                                    ,[Description]=@Description
                                    ,[OperatorID]=@OperatorID
                                    ,[CreateTime]=getdate()
                                     Where WorkOrderID=@WorkOrderID";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@WorkOrderID", returnVisit["WorkOrderID"].Value));
            parametersList.Add(new SqlParameter("@IsResolved", returnVisit["IsResolved"].Value));
            parametersList.Add(new SqlParameter("@IsFeeChecked", returnVisit["IsFeeChecked"].Value));
            parametersList.Add(new SqlParameter("@Description", returnVisit["Description"].Value));
            parametersList.Add(new SqlParameter("@OperatorID", returnVisit["OperatorID"].Value));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion

        #region 质检信息
        /// <summary>
        /// 添加质检信息
        /// </summary>
        /// <param name="qualityCheck">质检信息</param>
        /// <returns></returns>
        public bool AddQualityCheck(DataEntity qualityCheck)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into QualityCheck([WorkOrderID],[WorkProcessID],[IsQualified],[OperatorID],[Remark])
                                      Values(@WorkOrderID,@WorkProcessID,@IsQualified,@OperatorID,@Remark) ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@WorkOrderID", qualityCheck["WorkOrderID"].Value));
            parametersList.Add(new SqlParameter("@WorkProcessID", qualityCheck["WorkProcessID"].Value));
            parametersList.Add(new SqlParameter("@IsQualified", qualityCheck["IsQualified"].Value));
            parametersList.Add(new SqlParameter("@OperatorID", qualityCheck["OperatorID"].Value));
            parametersList.Add(new SqlParameter("@Remark", qualityCheck["Remark"].Value));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 获取质检信息
        /// </summary>
        /// <param name="workProcessId">工单处理Id</param>
        /// <returns>质检信息</returns>
        public DataEntity GetQualityCheck(string workProcessId)
        {
            DataEntity qualityCheck = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT qc.[ID]
                                  ,qc.[WorkOrderID]
                                  ,qc.[WorkProcessID]
                                  ,qc.[IsQualified]
                                  ,qc.[CreateTime]
                                  ,qc.OperatorID
                                  ,qc.Remark
                                  ,s.Name OperatorName
                            FROM [QualityCheck] qc left Join Staff s on qc.OperatorID=s.ID  Where qc.[workProcessId]=@workProcessId ";
            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@workProcessId", workProcessId));
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                if (reader.Read())
                {
                    qualityCheck = new DataEntity();
                    qualityCheck.Add("ID", DBNull.Value != reader["ID"] ? reader["ID"].ToString() : string.Empty);
                    qualityCheck.Add("WorkOrderID", DBNull.Value != reader["WorkOrderID"] ? reader["WorkOrderID"].ToString() : string.Empty);
                    qualityCheck.Add("WorkProcessID", DBNull.Value != reader["WorkProcessID"] ? reader["WorkProcessID"].ToString() : string.Empty);
                    qualityCheck.Add("IsQualified", DBNull.Value != reader["IsQualified"] ? (bool)reader["IsQualified"] : false);
                    qualityCheck.Add("OperatorID", DBNull.Value != reader["OperatorID"] ? reader["OperatorID"].ToString() : string.Empty);
                    qualityCheck.Add("OperatorName", DBNull.Value != reader["OperatorName"] ? reader["OperatorName"].ToString() : string.Empty);
                    qualityCheck.Add("Remark", DBNull.Value != reader["Remark"] ? reader["Remark"].ToString() : string.Empty);
                    qualityCheck.Add("CreateTime", DBNull.Value != reader["CreateTime"] ? Convert.ToDateTime(reader["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);

                }
            }

            return qualityCheck;
        }

        /// <summary>
        /// 获取质检信息
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>质检信息集合</returns>
        public List<DataEntity> GetQualityChecks(string workOrderId)
        {
            List<DataEntity> qualityCheckList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT qc.[ID]
                                  ,qc.[WorkOrderID]
                                  ,qc.[WorkProcessID]
                                  ,qc.[IsQualified]
                                  ,qc.[CreateTime]
                                  ,qc.OperatorID
                                  ,qc.Remark
                                  ,s.Name OperatorName
                            FROM [QualityCheck] qc left Join Staff s on qc.OperatorID=s.ID  Where qc.[WorkOrderID]=@WorkOrderID ";
            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@WorkOrderID", workOrderId));
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                qualityCheckList = new List<DataEntity>();

                while (reader.Read())
                {
                    DataEntity qualityCheck = new DataEntity();
                    qualityCheck.Add("ID", DBNull.Value != reader["ID"] ? reader["ID"].ToString() : string.Empty);
                    qualityCheck.Add("WorkOrderID", DBNull.Value != reader["WorkOrderID"] ? reader["WorkOrderID"].ToString() : string.Empty);
                    qualityCheck.Add("WorkProcessID", DBNull.Value != reader["WorkProcessID"] ? reader["WorkProcessID"].ToString() : string.Empty);
                    qualityCheck.Add("IsQualified", DBNull.Value != reader["IsQualified"] ? (bool)reader["IsQualified"] : false);
                    qualityCheck.Add("OperatorID", DBNull.Value != reader["OperatorID"] ? reader["OperatorID"].ToString() : string.Empty);
                    qualityCheck.Add("OperatorName", DBNull.Value != reader["OperatorName"] ? reader["OperatorName"].ToString() : string.Empty);
                    qualityCheck.Add("Remark", DBNull.Value != reader["Remark"] ? reader["Remark"].ToString() : string.Empty);
                    qualityCheck.Add("CreateTime", DBNull.Value != reader["CreateTime"] ? Convert.ToDateTime(reader["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                    qualityCheckList.Add(qualityCheck);
                }
            }

            return qualityCheckList;
        }

        /// <summary>
        /// 更新质检信息
        /// </summary>
        /// <param name="qualityCheck">质检信息</param>
        /// <returns></returns>
        public bool UpdateQualityCheck(DataEntity qualityCheck)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update [QualityCheck] 
                                set  [IsQualified]=@IsQualified
                                    ,[OperatorID]=@OperatorID
                                    ,[Remark]=@Remark
                                    ,[CreateTime]=getdate()
                                     Where ID=@ID";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@IsQualified", qualityCheck["IsQualified"].Value));
            parametersList.Add(new SqlParameter("@OperatorID", qualityCheck["OperatorID"].Value));
            parametersList.Add(new SqlParameter("@Remark", qualityCheck["Remark"].Value));
            parametersList.Add(new SqlParameter("@ID", qualityCheck["ID"].Value));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        #endregion

        #region 工单图片
        /// <summary>
        /// 获取工单图片
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <param name="imgType">图片类型</param>
        /// <returns></returns>
        public List<DataEntity> GetWorkOrderImgs(string workOrderId, int imgType)
        {
            List<DataEntity> reportImgList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT [ID]
                                  ,[WorkOrderID]
                                  ,[Type]
                                  ,[Name]
                                  ,[Path]
                            FROM [WorkOrderImg] Where [WorkOrderID]=@WorkOrderID and Type=@Type";
            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@WorkOrderID", workOrderId));
            parList.Add(new SqlParameter("@Type", (int)imgType));
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                reportImgList = new List<DataEntity>();

                while (reader.Read())
                {
                    DataEntity reportImg = new DataEntity();
                    reportImg.Add("ID", DBNull.Value != reader["ID"] ? reader["ID"].ToString() : string.Empty);
                    reportImg.Add("WorkOrderID", DBNull.Value != reader["WorkOrderID"] ? reader["WorkOrderID"].ToString() : string.Empty);
                    reportImg.Add("Name", DBNull.Value != reader["Name"] ? reader["Name"].ToString() : string.Empty);
                    reportImg.Add("Type", DBNull.Value != reader["Type"] ? (int)reader["Type"] : 0);
                    reportImg.Add("Path", DBNull.Value != reader["Path"] ? reader["Path"].ToString() : string.Empty);

                    reportImgList.Add(reportImg);
                }
            }

            return reportImgList;
        }

        /// <summary>
        /// 更新工单图片
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <param name="imgType">图片类型</param>
        /// <param name="imgList">图片集合</param>
        /// <returns></returns>
        public bool AddOrUpdWorkOrderImgs(string workOrderId, int imgType, List<DataEntity> imgList)
        {
            bool isSuccess = false;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            List<DbParameter> parametersList = new List<DbParameter>();

            string strDel = "delete WorkOrderImg where WorkOrderID=@WorkOrderID and Type=@Type";
            string strSql = @"Insert Into WorkOrderImg([WorkOrderID],[Type],[Name],[Path])
                                      Values(@WorkOrderID,@Type,@Name,@Path) ";

            dbHelper.BeginTrans();
            parametersList.Add(new SqlParameter("@WorkOrderID", workOrderId));
            parametersList.Add(new SqlParameter("@Type", (int)imgType));
            isSuccess = dbHelper.ExecuteNonQueryInTrans(strDel, parametersList) > 0;

            if (isSuccess)
            {
                if (imgList != null && imgList.Count > 0)
                {
                    foreach (DataEntity item in imgList)
                    {
                        parametersList = new List<DbParameter>();
                        parametersList.Add(new SqlParameter("@WorkOrderID", item["WorkOrderID"].Value));
                        parametersList.Add(new SqlParameter("@Type", item["Type"].Value));
                        parametersList.Add(new SqlParameter("@Name", item["Name"].Value));
                        parametersList.Add(new SqlParameter("@Path", item["Path"].Value));

                        isSuccess = dbHelper.ExecuteNonQueryInTrans(strSql, parametersList) > 0;

                        if (!isSuccess) break;
                    }
                }
            }

            if (isSuccess)
            {
                dbHelper.CommitTrans();
            }
            else
            {
                dbHelper.RollBack();
            }

            return isSuccess;
        }
        #endregion

        #region 工单评价
        /// <summary>
        /// 添加评价
        /// </summary>
        /// <param name="workOrderEvalution">评价信息</param>
        /// <returns></returns>
        public bool AddEvaluation(DataEntity workOrderEvalution)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlInsert = @"Insert into [WorkOrderEvaluation]( [WorkOrderID] ,[CustomerID] ,[OverallLevel] ,[Evaluation] ,[Lables])
                                    values(@WorkOrderID,@CustomerID,@OverallLevel,@Evaluation,@Lables);";
            string strSqlUpdate = @" Update WorkOrder set IsEvaluated=1,IsCustomerEvaluate=@IsCustomerEvaluate where ID=@WorkOrderID";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@WorkOrderID", workOrderEvalution["WorkOrderID"].Value));
            parametersList.Add(new SqlParameter("@CustomerID", workOrderEvalution["CustomerID"].Value));
            parametersList.Add(new SqlParameter("@OverallLevel", workOrderEvalution["OverallLevel"].Value));
            parametersList.Add(new SqlParameter("@Evaluation", workOrderEvalution["Evaluation"].Value));
            parametersList.Add(new SqlParameter("@Lables", workOrderEvalution["Lables"].Value));

            dbHelper.BeginTrans();
            bool isSuccessed = dbHelper.ExecuteNonQueryInTrans(strSqlInsert, parametersList) > 0;
            if (isSuccessed)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@WorkOrderID", workOrderEvalution["WorkOrderID"].Value));
                parametersList.Add(new SqlParameter("@IsCustomerEvaluate", workOrderEvalution["IsCustomerEvaluate"].Value));
                isSuccessed = dbHelper.ExecuteNonQueryInTrans(strSqlUpdate, parametersList) > 0;
            }

            if (isSuccessed)
            {
                dbHelper.CommitTrans();
            }
            else
            {
                dbHelper.RollBack();
            }

            return isSuccessed;
        }

        /// <summary>
        /// 根据工单获取评价信息
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <returns></returns>
        public DataEntity GetEvaluation(string workOrderId)
        {
            DataEntity evaluation = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT e.[ID]
                                  ,e.[WorkOrderID]
                                  ,e.[CustomerID]
                                  ,c.Name [CustomerName]
                                  ,e.[OverallLevel]
                                  ,e.[Evaluation]
                                  ,e.[Lables]
                                  ,e.[CreateTime]
                            FROM [WorkOrderEvaluation] e left join Customer c on e.CustomerID=c.ID
                            Where [WorkOrderID]=@WorkOrderID Order by CreateTime desc ";

            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@WorkOrderID", workOrderId));
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                while (reader.Read())
                {
                    evaluation = new DataEntity();
                    evaluation.Add("ID", DBNull.Value != reader["ID"] ? reader["ID"].ToString() : string.Empty);
                    evaluation.Add("WorkOrderID", DBNull.Value != reader["WorkOrderID"] ? reader["WorkOrderID"].ToString() : string.Empty);
                    evaluation.Add("CustomerID", DBNull.Value != reader["CustomerID"] ? reader["CustomerID"].ToString() : string.Empty);
                    evaluation.Add("CustomerName", DBNull.Value != reader["CustomerName"] ? reader["CustomerName"].ToString() : string.Empty);
                    evaluation.Add("OverallLevel", DBNull.Value != reader["OverallLevel"] ? reader["OverallLevel"].ToString() : string.Empty);
                    evaluation.Add("Evaluation", DBNull.Value != reader["Evaluation"] ? reader["Evaluation"].ToString() : string.Empty);
                    evaluation.Add("Lables", DBNull.Value != reader["Lables"] ? reader["Lables"].ToString() : string.Empty);
                    evaluation.Add("CreateTime", DBNull.Value != reader["CreateTime"] ? reader["CreateTime"].ToString() : string.Empty);

                }
            }
            return evaluation;
        }

        /// <summary>
        /// 修改评价信息
        /// </summary>
        /// <param name="workOrderEvalution">评价实体</param>
        /// <returns></returns>
        public bool UpdateEvaluation(DataEntity workOrderEvalution)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update [WorkOrderEvaluation] 
                                set  [OverallLevel]=@OverallLevel
                                    ,[Evaluation]=@Evaluation                               
                                    ,[Lables]=@Lables
                                    ,[IsDeleted]=@IsDeleted
                                    ,[CreateTime]=getdate()
                                     Where WorkOrderID=@WorkOrderID";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@WorkOrderID", workOrderEvalution["WorkOrderID"].Value));
            parametersList.Add(new SqlParameter("@OverallLevel", workOrderEvalution["OverallLevel"].Value));
            parametersList.Add(new SqlParameter("@Evaluation", workOrderEvalution["Evaluation"].Value));
            parametersList.Add(new SqlParameter("@Lables", workOrderEvalution["Lables"].Value));
            parametersList.Add(new SqlParameter("@IsDeleted", workOrderEvalution["IsDeleted"].Value));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

      

        #endregion
    }
}
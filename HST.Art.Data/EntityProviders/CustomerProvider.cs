/*----------------------------------------------------------------
// 文件名：CustomerProviderProvider.cs
// 功能描述： 客户数据提供者
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

namespace HST.Art.Data
{
    /// <summary>
    /// 客户数据提供者
    /// </summary>
    public class CustomerProvider : EntityProviderBase
    {
        #region 查询客户
        /// <summary>
        /// 根据条件所有获取客户信息
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public List<DataEntity> GetAll(string serviceSpaceId, Condition condition)
        {
            string sort = string.IsNullOrEmpty(condition.Sort) ? "createtime desc" : condition.Sort + " " + condition.Direction;
            string where = GetWhereStr(condition.Where);
            List<DataEntity> customers = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT distinct c.[ID]
                                ,c.ServiceSpaceID                                
                                ,c.[Name]
                                ,c.[Gender]
                                ,c.[Mobile]
                                ,c.[Contact]
                                ,c.[LabelIDs]
                                ,c.[Mail]
                                ,c.[WeChat]
                                ,c.[HeadImg]
                                ,c.[UserName]
                                ,c.[Password]
                                ,c.[CompanyName]
                                ,c.[Area]
                                ,c.[Address]
                                ,c.[CreateTime]
                                ,c.[UpdateTime],c.RecentService,c.Remark  from   Customer c left join NetworkCustomer nc on c.ID=nc.customerid where c.IsDeleted=0 and c.ServiceSpaceID=@ServiceSpaceID" + where + " order by " + sort;
            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@ServiceSpaceID", serviceSpaceId));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                customers = new List<DataEntity>();
                DataEntity customer = null;
                while (reader.Read())
                {
                    customer = GetDataEntityFromReader(reader);
                    customers.Add(customer);
                }
                return customers;
            }
        }

        /// <summary>
        /// 根据条件分页获取客户信息
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <param name="condition">条件</param>
        /// <param name="totalNum"></param>
        /// <returns></returns>
        public List<DataEntity> GetPage(string serviceSpaceId, Condition condition, out int totalNum)
        {
            totalNum = 0;
            string sort = string.IsNullOrEmpty(condition.Sort) ? "createtime desc" : condition.Sort + " " + condition.Direction;
            string where = GetWhereStr(condition.Where);

            List<DataEntity> customers = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select  count(distinct c.ID) from  Customer c left join NetworkCustomer nc on c.ID=nc.customerid  where c.IsDeleted=0 and c.ServiceSpaceID='" + serviceSpaceId + "' " + where;//查询有多少条记录
            totalNum = Convert.ToInt32(dbHelper.ExecuteScalar(strSqlQuery, null));

            string strSql = @"SELECT [ID]
                                ,ServiceSpaceID
                                ,[Name]
                                ,[Gender]
                                ,[Mobile]
                                ,[Mail]
                                ,[Contact]
                                ,[LabelIDs]
                                ,[WeChat]
                                ,[HeadImg]
                                ,UserName
                                ,[Password]
                                ,[CompanyName]
                                ,[Area]
                                ,[Address]
                                ,[CreateTime]
                                ,[UpdateTime]
                                ,RecentService
                                ,Remark
                            FROM  (select *,ROW_NUMBER() over (order by " + sort + @") as num from (select distinct c.[ID]
                                ,c.ServiceSpaceID                                
                                ,c.[Name]
                                ,c.[Gender]
                                ,c.[Mobile]
                                ,c.[Mail]
                                ,c.[Contact]
                                ,c.[LabelIDs]
                                ,c.[WeChat]
                                ,c.[HeadImg]
                                ,c.UserName
                                ,c.[Password]
                                ,c.[CompanyName]
                                ,c.[Area]
                                ,c.[Address]
                                ,c.[CreateTime]
                                ,c.[UpdateTime],c.RecentService,c.Remark from   Customer c left join NetworkCustomer nc on c.ID=nc.customerid  where c.IsDeleted=0 and  c.ServiceSpaceID=@ServiceSpaceID " + where + ") as t) as temp where num between(@pageIndex - 1) * @pageSize + 1  and @pageIndex*@pageSize order by " + sort;

            IList<DbParameter> parList = new List<DbParameter>();
            parList.Add(new SqlParameter("@pageIndex", condition.PageIndex));
            parList.Add(new SqlParameter("@pageSize", condition.PageSize));
            parList.Add(new SqlParameter("@ServiceSpaceID", serviceSpaceId));
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parList))
            {
                customers = new List<DataEntity>();
                DataEntity customer = null;
                while (reader.Read())
                {
                    customer = GetDataEntityFromReader(reader);
                    customers.Add(customer);
                }
                return customers;
            }
        }

        /// <summary>
        /// 根据所属区域、服务空间id获取客户集合
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <param name="area">所属区域</param>
        /// <returns>客户集合</returns>
        public List<DataEntity> GetAllByArea(string serviceSpaceId, string area)
        {
            List<DataEntity> customers = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT [ID]
                                ,ServiceSpaceID
                                ,[Name]
                                ,[Gender]
                                ,[Mobile]
                                ,[Mail]
                                ,[Contact]
                                ,[LabelIDs]
                                ,[WeChat]
                                ,[HeadImg]
                                ,[UserName]
                                ,[Password]
                                ,[CompanyName]
                                ,[Area]
                                ,[Address]
                                ,[CreateTime]
                                ,[UpdateTime]
                                ,RecentService
                                ,Remark
                            FROM   Customer c  where c.IsDeleted=0 and ServiceSpaceID='" + serviceSpaceId + "' and Area like '%" + area + "%'";

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, null))
            {
                customers = new List<DataEntity>();
                DataEntity customer = null;
                while (reader.Read())
                {
                    customer = GetDataEntityFromReader(reader);
                    customers.Add(customer);
                }
                return customers;
            }
        }

        /// <summary>
        /// 根据客户名称获取客户
        /// </summary>
        ///<param name="serviceSpaceId">服务空间id</param>
        /// <param name="name">客户名称</param>
        /// <param name="totalNum">记录条数</param>
        /// <returns>客户集合</returns>
        public List<DataEntity> GetAllByName(string serviceSpaceId, string name, out int totalNum)
        {
            List<DataEntity> customers = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string where = string.IsNullOrEmpty(name) ? "" : string.Format(" and (name like '%{0}%' or dbo.f_GetPy([name]) like '{0}%' or Mobile like '{0}%')", name);

            string strSqlQuery = @"select count(ID) from  Customer where IsDeleted=0 and ServiceSpaceID='" + serviceSpaceId + "' " + where;//查询有多少条记录
            totalNum = Convert.ToInt32(dbHelper.ExecuteScalar(strSqlQuery, null));

            string strSql = @"SELECT top 10
                                [ID]
                                ,ServiceSpaceID
                                ,[Name]
                                ,[Gender]
                                ,[Mobile]
                                ,[Mail]
                                ,[Contact]
                                ,[LabelIDs]
                                ,[WeChat]
                                ,[HeadImg]
                                ,[UserName]
                                ,[Password]
                                ,[CompanyName]
                                ,[Area]
                                ,[Address]
                                ,[CreateTime]
                                ,[UpdateTime]
                                ,RecentService
                                ,Remark
                            FROM [Customer]  Where IsDeleted=0 and ServiceSpaceID='" + serviceSpaceId + "' " + where;

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, null))
            {
                customers = new List<DataEntity>();
                DataEntity customer = null;
                while (reader.Read())
                {
                    customer = GetDataEntityFromReader(reader);
                    customers.Add(customer);
                }
            }
            return customers;
        }

        /// <summary>
        /// 根据ID获取客户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataEntity Get(string id)
        {
            DataEntity customer = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT 
                                [ID]
                                ,ServiceSpaceID
                                ,[Name]
                                ,[Gender]
                                ,[Mobile]
                                ,[Mail]
                                ,[Contact]
                                ,[LabelIDs]
                                ,[WeChat]
                                ,[HeadImg]
                                ,[UserName]
                                ,[Password]
                                ,[CompanyName]
                                ,[Area]
                                ,[Address]
                                ,[CreateTime]
                                ,[UpdateTime]
                                ,RecentService
                                ,Remark
                            FROM [Customer]  Where IsDeleted=0 and [ID]=@ID ";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {

                while (reader.Read())
                {
                    customer = GetDataEntityFromReader(reader);
                    break;
                }
            }
            return customer;
        }

        /// <summary>
        /// 根据客户微信openId获取客户信息
        /// </summary>
        /// <param name="OpenId">客户微信openId</param>
        /// <returns>客户信息</returns>
        public DataEntity GetByOpenId(string serviceSpaceId, string OpenId)
        {
            DataEntity customer = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT 
                                [ID]
                                ,ServiceSpaceID
                                ,[Name]
                                ,[Gender]
                                ,[Mobile]
                                ,[Mail]
                                ,[Contact]
                                ,[LabelIDs]
                                ,[WeChat]
                                ,[HeadImg]
                                ,[UserName]
                                ,[Password]
                                ,[CompanyName]
                                ,[Area]
                                ,[Address]
                                ,[CreateTime]
                                ,[UpdateTime]
                                ,RecentService
                                ,Remark
                            FROM [Customer]  Where IsDeleted=0 and [WeChat]=@WeChat and ServiceSpaceID=@ServiceSpaceID";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@WeChat", OpenId));
            parametersList.Add(new SqlParameter("@ServiceSpaceID", serviceSpaceId));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    customer = GetDataEntityFromReader(reader);
                    break;
                }
            }
            return customer;
        }

        /// <summary>
        /// 根据手机号、服务空间id获取客户信息
        /// </summary>
        ///  <param name="serviceSpaceId">服务空间Id</param>
        /// <param name="phoneNum">手机号</param>
        /// <returns></returns>
        public DataEntity GetByPhone(string serviceSpaceId, string phoneNum)
        {
            DataEntity customer = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT 
                                [ID]
                                ,ServiceSpaceID
                                ,[Name]
                                ,[Gender]
                                ,[Mobile]
                                ,[Mail]
                                ,[Contact]
                                ,[LabelIDs]
                                ,[WeChat]
                                ,[HeadImg]
                                ,[UserName]
                                ,[Password]
                                ,[CompanyName]
                                ,[Area]
                                ,[Address]
                                ,[CreateTime]
                                ,[UpdateTime]
                                ,RecentService
                                ,Remark
                            FROM [Customer]  Where [Mobile]=@Mobile and IsDeleted=0 and ServiceSpaceID=@ServiceSpaceID ";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Mobile", phoneNum));
            parametersList.Add(new SqlParameter("@ServiceSpaceID", serviceSpaceId));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {

                while (reader.Read())
                {
                    customer = GetDataEntityFromReader(reader);
                    break;
                }
            }
            return customer;
        }
        #endregion

        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="customer">客户信息</param>
        /// <returns>修改成功标识</returns>
        public bool Update(DataEntity customer)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update [Customer] 
                                set  [Name]=@Name
                                    ,[Gender]=@Gender
                                    ,[Mobile]=@Mobile
                                    ,[Mail]=@Mail
                                    ,[Contact]=@Contact
                                    ,[LabelIDs]=@LabelIDs
                                    ,[WeChat]=@WeChat
                                    ,[HeadImg]=@HeadImg
                                    ,[UserName]=@UserName
                                    ,[Password]=@Password
                                    ,[CompanyName]=@CompanyName
                                    ,[Area]=@Area
                                    ,[Address]=@Address
                                    ,[UpdateTime]=getdate()
                                    ,[RecentService]=@RecentService,[Remark]=@Remark Where ID=@ID";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", customer["ID"].Value));
            parametersList.Add(new SqlParameter("@Name", customer["Name"].Value));
            parametersList.Add(new SqlParameter("@Gender", customer["Gender"].Value));
            parametersList.Add(new SqlParameter("@Mobile", customer["Mobile"].Value));
            parametersList.Add(new SqlParameter("@Mail", customer["Mail"].Value));
            parametersList.Add(new SqlParameter("@WeChat", customer["WeChat"].Value));
            parametersList.Add(new SqlParameter("@Contact", customer["Contact"].Value));
            parametersList.Add(new SqlParameter("@LabelIDs", customer["LabelIDs"].Value));
            parametersList.Add(new SqlParameter("@HeadImg", customer["HeadImg"].Value));
            parametersList.Add(new SqlParameter("@UserName", customer["UserName"].Value));
            parametersList.Add(new SqlParameter("@Password", customer["Password"].Value));
            parametersList.Add(new SqlParameter("@CompanyName", customer["CompanyName"].Value));
            parametersList.Add(new SqlParameter("@Area", customer["Area"].Value));
            parametersList.Add(new SqlParameter("@Address", customer["Address"].Value));
            parametersList.Add(new SqlParameter("@RecentService", customer["RecentService"].Value));
            parametersList.Add(new SqlParameter("@Remark", customer["Remark"].Value));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="customer">客户信息</param>
        /// <returns>添加成功标识</returns>
        public bool Add(DataEntity customer)
        {
            bool isSuccess = false;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into Customer(
                                          [ID]
                                        ,ServiceSpaceID
                                        ,[Name]
                                        ,[Gender]
                                        ,[Mobile]
                                        ,[Mail]
                                        ,[Contact]
                                        ,[LabelIDs]
                                        ,[WeChat]
                                        ,[UserName]
                                        ,[Password]
                                        ,[CompanyName]
                                        ,[Area]
                                        ,[Address]
                                        ,[Remark]
                                         )
                              Values(@ID,@ServiceSpaceID,@Name,@Gender,@Mobile,@Mail,@Contact,@LabelIDs,@WeChat,@UserName,@Password,@CompanyName,@Area,@Address,@Remark) ";
            string strNetCus = "Insert Into NetworkCustomer(ServiceSpaceID,ServiceNetworkID,CustomerID) Values(@ServiceSpaceID,@ServiceNetworkID,@ID)";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", customer["ID"].Value));
            parametersList.Add(new SqlParameter("@ServiceSpaceID", customer["ServiceSpaceID"].Value));
            parametersList.Add(new SqlParameter("@Name", customer["Name"].Value));
            parametersList.Add(new SqlParameter("@Gender", customer["Gender"].Value));
            parametersList.Add(new SqlParameter("@Mobile", customer["Mobile"].Value));
            parametersList.Add(new SqlParameter("@Mail", customer["Mail"].Value));
            parametersList.Add(new SqlParameter("@WeChat", customer["WeChat"].Value));
            parametersList.Add(new SqlParameter("@Contact", customer["Contact"].Value));
            parametersList.Add(new SqlParameter("@LabelIDs", customer["LabelIDs"].Value));
            parametersList.Add(new SqlParameter("@UserName", customer["UserName"].Value));
            parametersList.Add(new SqlParameter("@Password", customer["Password"].Value));
            parametersList.Add(new SqlParameter("@CompanyName", customer["CompanyName"].Value));
            parametersList.Add(new SqlParameter("@Area", customer["Area"].Value));
            parametersList.Add(new SqlParameter("@Address", customer["Address"].Value));
            parametersList.Add(new SqlParameter("@Remark", customer["Remark"].Value));
            parametersList.Add(new SqlParameter("@ServiceNetworkID", customer["ServiceNetworkID"].Value));

            dbHelper.BeginTrans();

            isSuccess = dbHelper.ExecuteNonQueryInTrans(strSql, parametersList) > 0;

            if (isSuccess)
            {
                isSuccess = dbHelper.ExecuteNonQueryInTrans(strNetCus, parametersList) > 0;
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

        /// <summary>
        /// 添加网点客户关联关系
        /// </summary>
        /// <param name="networkCus">关系实体</param>
        /// <returns></returns>
        public bool AddNetworkCustomer(DataEntity networkCus)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = "select count(id) from NetworkCustomer where ServiceSpaceID=@ServiceSpaceID and ServiceNetworkID=@ServiceNetworkID and CustomerID=@CustomerID";

            string strInsSql = @"Insert Into NetworkCustomer(ServiceSpaceID,ServiceNetworkID,CustomerID) Values(@ServiceSpaceID,@ServiceNetworkID,@CustomerID) ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ServiceSpaceID", networkCus["ServiceSpaceID"].Value));
            parametersList.Add(new SqlParameter("@ServiceNetworkID", networkCus["ServiceNetworkID"].Value));
            parametersList.Add(new SqlParameter("@CustomerID", networkCus["CustomerID"].Value));

            object obj = dbHelper.ExecuteScalar(strSql, parametersList);
            if (Convert.ToInt16(obj) > 0) return true;

            return dbHelper.ExecuteNonQuery(strInsSql, parametersList) > 0;
        }


        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="idList">客户id集合</param>
        /// <returns>删除成功标识</returns>
        public bool Delete(List<string> idList)
        {
            bool isSuccessed = true;

            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "update Customer set IsDeleted=1 where id=@Id";
            IList<DbParameter> parameList = null;

            dbHelper.BeginTrans();//开启事务

            for (int i = 0; i < idList.Count; i++)
            {
                parameList = new List<DbParameter>();
                parameList.Add(new SqlParameter("@Id", idList[i]));
                isSuccessed = dbHelper.ExecuteNonQueryInTrans(strSql, parameList) > 0;
            }

            if (isSuccessed)
            {
                dbHelper.CommitTrans();//事务提交
            }
            else
            {
                dbHelper.RollBack();//事务回滚
            }

            return isSuccessed;
        }

        /// <summary>
        /// 根据检索条件获取记录总数
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <param name="condition">检索条件</param>
        /// <returns>记录总数</returns>
        public int GetCount(string serviceSpaceId, Condition condition)
        {
            int totalNum = 0;
            string where = GetWhereStr(condition.Where);
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select count(c.ID) from  Customer c  where c.IsDeleted=0 and ServiceSpaceID='" + serviceSpaceId + "' " + where;//查询有多少条记录
            totalNum = Convert.ToInt32(dbHelper.ExecuteScalar(strSqlQuery, null));
            return totalNum;
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
            //    if (job["name"] != null && !string.IsNullOrEmpty(job["name"].ToString()))
            //    {
            //        whereBuilder.Append(string.Format(" and (c.name like '%{0}%' or c.Mobile like '{0}%')", job["name"].ToString()));
            //    }

            //    if (job["servicenetworkid"] != null && !string.IsNullOrEmpty(job["servicenetworkid"].ToString()))
            //    {
            //        whereBuilder.Append(" and nc.servicenetworkid =('" + job["servicenetworkid"].ToString() + "')");
            //    }

            //    if (job["area"] != null && !string.IsNullOrEmpty(job["area"].ToString()))
            //    {
            //        whereBuilder.Append(string.Format(" and (c.Area like '%{0}%')", job["area"].ToString()));
            //    }

            //    if (job["startdate"] != null && !string.IsNullOrEmpty(job["startdate"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), c.createtime, 120)>='" + job["startdate"].ToString() + "'");
            //    }

            //    if (job["enddate"] != null && !string.IsNullOrEmpty(job["enddate"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), c.createtime, 120)<='" + job["enddate"].ToString() + "'");
            //    }
            //    if (job["recentService"] != null && !string.IsNullOrEmpty(job["recentService"].ToString()))
            //    {
            //        whereBuilder.Append(" and CONVERT(varchar(10), c.RecentService, 120)<>'" + job["recentService"].ToString() + "'");
            //    }
            //    if (job["startservicedate"] != null && !string.IsNullOrEmpty(job["startservicedate"].ToString()))
            //    {
            //        if (job["startservicedate"].ToString() == "1900-01-01")
            //        {
            //            whereBuilder.Append(" and CONVERT(varchar(10), c.RecentService, 120)<>'" + job["startservicedate"].ToString() + "'");
            //        }
            //        else
            //        {
            //            whereBuilder.Append(" and CONVERT(varchar(10), c.RecentService, 120)>='" + job["startservicedate"].ToString() + "'");
            //        }
            //    }

            //    if (job["endservicedate"] != null && !string.IsNullOrEmpty(job["endservicedate"].ToString()))
            //    {
            //        if (job["endservicedate"].ToString() == "1900-01-01")
            //        {
            //            whereBuilder.Append(" and CONVERT(varchar(10), c.RecentService, 120)<>'" + job["endservicedate"].ToString() + "'");
            //        }
            //        else
            //        {
            //            whereBuilder.Append(" and CONVERT(varchar(10), c.RecentService, 120)<='" + job["endservicedate"].ToString() + "'");
            //        }
            //    }

            //    //客户标签
            //    if (job["label"] != null && !string.IsNullOrEmpty(job["label"].ToString()))
            //    {
            //        whereBuilder.Append(" and c.labelids like '%" + job["label"].ToString() + "%'");
            //    }
            //}

            return whereBuilder.ToString();
        }

        /// <summary>
        /// 从游标中读取数据实体
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private DataEntity GetDataEntityFromReader(DbDataReader reader)
        {
            DataEntity customer = new DataEntity();
            #region 组装数据
            customer.Add("ID", DBNull.Value != reader["ID"] ? reader["ID"].ToString() : string.Empty);
            customer.Add("ServiceSpaceID", DBNull.Value != reader["ServiceSpaceID"] ? reader["ServiceSpaceID"].ToString() : string.Empty);
            customer.Add("Name", DBNull.Value != reader["Name"] ? reader["Name"].ToString() : string.Empty);
            customer.Add("Gender", DBNull.Value != reader["Gender"] ? (int)reader["Gender"] : 0);
            customer.Add("Mobile", DBNull.Value != reader["Mobile"] ? reader["Mobile"].ToString() : string.Empty);
            customer.Add("Mail", DBNull.Value != reader["Mail"] ? reader["Mail"].ToString() : string.Empty);
            customer.Add("WeChat", DBNull.Value != reader["WeChat"] ? reader["WeChat"].ToString() : string.Empty);
            customer.Add("HeadImg", DBNull.Value != reader["HeadImg"] ? reader["HeadImg"].ToString() : string.Empty);
            customer.Add("UserName", DBNull.Value != reader["UserName"] ? reader["UserName"].ToString() : string.Empty);
            customer.Add("Password", DBNull.Value != reader["Password"] ? reader["Password"].ToString() : string.Empty);
            customer.Add("CompanyName", DBNull.Value != reader["CompanyName"] ? reader["CompanyName"].ToString() : string.Empty);
            customer.Add("Contact", DBNull.Value != reader["Contact"] ? reader["Contact"].ToString() : string.Empty);
            customer.Add("LabelIDs", DBNull.Value != reader["LabelIDs"] ? reader["LabelIDs"].ToString() : string.Empty);
            customer.Add("Area", DBNull.Value != reader["Area"] ? reader["Area"].ToString() : string.Empty);
            customer.Add("Remark", DBNull.Value != reader["Remark"] ? reader["Remark"].ToString() : string.Empty);
            customer.Add("Address", DBNull.Value != reader["Address"] ? reader["Address"].ToString() : string.Empty);
            customer.Add("CreateTime", DBNull.Value != reader["CreateTime"] ? Convert.ToDateTime(reader["CreateTime"]).ToString("yyyy-MM-dd") : string.Empty);
            customer.Add("UpdateTime", DBNull.Value != reader["UpdateTime"] ? Convert.ToDateTime(reader["UpdateTime"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
            customer.Add("RecentService", DBNull.Value != reader["RecentService"] && !reader["RecentService"].ToString().Contains("1900") ? Convert.ToDateTime(reader["RecentService"]).ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
            #endregion
            return customer;
        }

        #region 客户标签
        /// <summary>
        /// 获取所有客户标签
        /// </summary>
        /// <returns></returns>
        public List<DataEntity> GetCustomerLabels(string serviceSpaceId)
        {
            List<DataEntity> customerLabels = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT  [ID],[ServiceSpaceId],[Name] FROM [CustomerLabel] where serviceSpaceId='" + serviceSpaceId + "'";
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, null))
            {
                customerLabels = new List<DataEntity>();
                DataEntity customerLabel = null;
                while (reader.Read())
                {
                    customerLabel = new DataEntity();
                    customerLabel.Add("ID", DBNull.Value != reader["ID"] ? reader["ID"].ToString() : "");
                    customerLabel.Add("Name", DBNull.Value != reader["Name"] ? reader["Name"].ToString() : "");
                    customerLabel.Add("ServiceSpaceId", DBNull.Value != reader["ServiceSpaceId"] ? reader["ServiceSpaceId"].ToString() : "");
                    customerLabels.Add(customerLabel);
                }
                return customerLabels;
            }
        }

        /// <summary>
        /// 获取客户标签
        /// </summary>
        /// <param name="id">标签id</param>
        /// <returns></returns>
        public DataEntity GetCustomerLabel(string id)
        {
            DataEntity customerLabel = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT  [ID],[ServiceSpaceId],[Name] FROM [CustomerLabel] Where ID='" + id + "'";
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, null))
            {
                if (reader.Read())
                {
                    customerLabel = new DataEntity();
                    customerLabel.Add("ID", DBNull.Value != reader["ID"] ? reader["ID"].ToString() : "");
                    customerLabel.Add("Name", DBNull.Value != reader["Name"] ? reader["Name"].ToString() : "");
                    customerLabel.Add("ServiceSpaceId", DBNull.Value != reader["ServiceSpaceId"] ? reader["ServiceSpaceId"].ToString() : "");
                }
                return customerLabel;
            }
        }

        /// <summary>
        /// 保存客户标签
        /// </summary>
        /// <param name="customerLabel">客户标签</param>
        /// <returns></returns>
        public bool SaveCustomerLabel(DataEntity customerLabel)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"if exists(select ID from [CustomerLabel] where ID=@ID)
                                begin
                                update CustomerLabel Set Name=@Name Where ID=@ID
                                end
                                else
                                begin
                                insert into CustomerLabel(ServiceSpaceId,Name) values(@ServiceSpaceId,@Name)
                                end";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", customerLabel["ID"].Value));
            parametersList.Add(new SqlParameter("@Name", customerLabel["Name"].Value));
            parametersList.Add(new SqlParameter("@ServiceSpaceId", customerLabel["ServiceSpaceId"].Value));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除客户标签
        /// </summary>
        /// <param name="id">标签id</param>
        /// <returns></returns>
        public bool DeleteCustomerLabel(string id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Delete from CustomerLabel  Where ID=@ID";
            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", id));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion
    }
}
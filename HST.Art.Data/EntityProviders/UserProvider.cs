/*----------------------------------------------------------------
// 文件名：UserProvider.cs
// 功能描述： 会员数据提供者
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
    /// 会员数据提供者
    /// </summary>
    public class UserProvider : EntityProviderBase
    {
        #region 查询会员
        /// <summary>
        /// 根据ID获取会员信息
        /// </summary>
        /// <param name="id">会员ID</param>
        /// <returns>会员信息</returns>
        public User Get(int id)
        {
            User userInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT Id, UserName, Password, Salt, Name, Email, Telephone, HeadImg, IsAdmin, State, CreateDate  from [User] where id=@Id and IsDeleted=0";
            string strRole = "select * from UserRole where UserId=@UserId";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));            

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    userInfo = GetUserFromReader(reader);
                }
            }

            if (userInfo != null)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@UserId", id));
                DataTable dtRole = dbHelper.ExecuteDataTable(strRole, parametersList);
                if (dtRole != null && dtRole.Rows != null && dtRole.Rows.Count > 0)
                {
                    userInfo.RoleList = new List<int>();
                    foreach (DataRow item in dtRole.Rows)
                    {
                        userInfo.RoleList.Add(Convert.ToInt32(item["RoleId"]));
                    }
                }
            }

            return userInfo;
        }

        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="userQuery">查询实体</param>
        /// <returns>会员信息</returns>
        public User GetByQuery(UserQuery userQuery)
        {
            User userInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("SELECT Id, UserName, Password, Salt, Name, Email, Telephone, HeadImg, IsAdmin, State, CreateDate  from [User] where IsDeleted=0 ");

            List<DbParameter> parametersList = new List<DbParameter>();
            switch (userQuery.Key)
            {
                case LoginType.UserName:
                    sBuilder.Append(" and UserName=@UserName");
                    parametersList.Add(new SqlParameter("@UserName", userQuery.Value));
                    break;
                case LoginType.Telephone:
                    sBuilder.Append(" and Telephone=@Telephone");
                    parametersList.Add(new SqlParameter("@Telephone", userQuery.Value));
                    break;
                case LoginType.Email:
                    sBuilder.Append(" and Email=@Email");
                    parametersList.Add(new SqlParameter("@Email", userQuery.Value));
                    break;
            }

            using (DbDataReader reader = dbHelper.ExecuteReader(sBuilder.ToString(), parametersList))
            {
                while (reader.Read())
                {
                    userInfo = GetUserFromReader(reader);
                }
            }

            return userInfo;
        }

        /// <summary>
        /// 获取所有会员信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>会员集合</returns>
        public List<User> GetAll(FilterEntityModel condition)
        {
            string whereSort = condition == null ? "" : condition.Where + condition.OrderBy;

            List<User> userList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT [ID]
                                    ,[UserName]
                                    ,[IsAdmin]
                                    ,[Name]
                                    ,[State]
                                    ,[Telephone]
                                    ,[Email]
                                    ,[CreateDate]
                            FROM [user]  where IsDeleted=0 " + whereSort;

            IList<DbParameter> parameList = null;
            if (condition != null && condition.SqlParList.Count > 0)
            {
                parameList = new List<DbParameter>();
                foreach (var item in condition.SqlParList)
                {
                    parameList.Add(new SqlParameter(item.Key, item.Value));
                }
            }

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                userList = new List<User>();
                User userInfo = null;
                while (reader.Read())
                {
                    userInfo = GetUserFromReader(reader);
                    userList.Add(userInfo);
                }
            }

            return userList;
        }

        /// <summary>
        /// 获取会员分页信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <param name="totalNum">总条数</param>
        /// <returns>会员集合</returns>
        public List<User> GetPage(FilterEntityModel condition, out int totalNum)
        {
            totalNum = 0;
            string sort = condition.OrderBy;
            string where = condition.Where;

            List<User> userList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select count(ID) from [user] where IsDeleted=0 " + where;//查询有多少条记录
            IList<DbParameter> parameList = new List<DbParameter>();
            parameList.Add(new SqlParameter("@pageSize", condition.PageSize));
            parameList.Add(new SqlParameter("@pageIndex", condition.PageIndex));

            if (condition.SqlParList.Count > 0)
            {
                foreach (var item in condition.SqlParList)
                {
                    parameList.Add(new SqlParameter(item.Key, item.Value));
                }
            }

            totalNum = Convert.ToInt32(dbHelper.ExecuteScalar(strSqlQuery, parameList));

            string strSql = @"SELECT [ID]
                                    ,[UserName]
                                    ,[IsAdmin]
                                    ,[Name]
                                    ,[State]
                                    ,[Telephone]
                                    ,[Email]
                                    ,[CreateDate]
                            FROM (select top (@pageSize*@pageIndex)  [ID]
                                    ,[UserName]
                                    ,[IsAdmin]
                                    ,[Name]
                                    ,[State]
                                    ,[Telephone]
                                    ,[Email]
                                    ,[CreateDate]
                                    ,ROW_NUMBER() over(" + sort + ") as num  from [user] where IsDeleted=0 " + where + ") as t where num between (@pageIndex - 1) * @pageSize + 1  and @pageIndex*@pageSize " + sort;
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                userList = new List<User>();
                User userInfo = null;
                while (reader.Read())
                {
                    userInfo = GetUserFromReader(reader);
                    userList.Add(userInfo);
                }
            }

            return userList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private User GetUserFromReader(DbDataReader reader)
        {
            User userInfo = new User();
            userInfo.Id = Convert.ToInt32(reader["Id"]);
            userInfo.UserName = reader["UserName"].ToString();

            if (ReaderExists(reader, "Password") && DBNull.Value != reader["Password"])
            {
                userInfo.Password = reader["Password"].ToString();
            }

            if (ReaderExists(reader, "Salt") && DBNull.Value != reader["Salt"])
            {
                userInfo.Salt = reader["Salt"].ToString();
            }

            if (ReaderExists(reader, "HeadImg") && DBNull.Value != reader["HeadImg"])
            {
                userInfo.HeadImg = reader["HeadImg"].ToString();
            }

            userInfo.Name = reader["Name"].ToString();
            userInfo.Email = reader["Email"].ToString();
            userInfo.Telephone = reader["Telephone"].ToString();
            userInfo.IsAdmin = Convert.ToBoolean(reader["IsAdmin"]);
            userInfo.State = Convert.ToInt32(reader["State"]) < 1 ? PublishState.Lower : PublishState.Upper;
            userInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

            return userInfo;
        }

        #endregion

        #region 编辑会员

        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="userInfo">会员信息</param>
        /// <returns>添加成功标识</returns>
        public bool Add(User userInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"if exists(select Id from [User] where userName=@UserName)
                                begin
                                    update [User] set Password=@Password,IsAdmin=@IsAdmin,Email=@Email,Name=@Name,Telephone=@Telephone,State=@State,UserId=@UserId,HeadImg=@HeadImg,Salt=@Salt,CreateDate=getdate(),IsDeleted=0 where userName=@UserName 
                                end
                                else
                                begin
                                Insert Into [User] (UserName, Password, Salt, Name, Email, Telephone, HeadImg, IsAdmin, State,UserId) 
                                   Values(@UserName, @Password, @Salt, @Name, @Email, @Telephone, @HeadImg, @IsAdmin, @State,@UserId) 
                                end ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@UserName", userInfo.UserName));
            parametersList.Add(new SqlParameter("@Password", userInfo.Password));
            parametersList.Add(new SqlParameter("@Salt", userInfo.Salt));
            parametersList.Add(new SqlParameter("@IsAdmin", userInfo.IsAdmin));
            parametersList.Add(new SqlParameter("@Name", userInfo.Name));
            parametersList.Add(new SqlParameter("@Telephone", userInfo.Telephone));
            parametersList.Add(new SqlParameter("@Email", userInfo.Email));
            parametersList.Add(new SqlParameter("@HeadImg", userInfo.HeadImg));
            parametersList.Add(new SqlParameter("@State", (int)userInfo.State));
            parametersList.Add(new SqlParameter("@UserId", userInfo.UserId));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 修改会员
        /// </summary>
        /// <param name="userInfo">会员信息</param>
        /// <returns>修改成功标识</returns>
        public bool Update(User userInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update [User]
                              Set [UserName]=@UserName
                                  ,[Password]=@Password
                                  ,[Salt]=@Salt
                                  ,[Name]=@Name
                                  ,[Email]=@Email
                                  ,[Telephone]=@Telephone
                                  ,[HeadImg]=@HeadImg 
                                  ,[IsAdmin]=@IsAdmin                         
                                  ,[State]=@State
                                  ,[UserId]=@UserId
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", userInfo.Id));
            parametersList.Add(new SqlParameter("@UserName", userInfo.UserName));
            parametersList.Add(new SqlParameter("@Password", userInfo.Password));
            parametersList.Add(new SqlParameter("@Salt", userInfo.Salt));
            parametersList.Add(new SqlParameter("@IsAdmin", userInfo.IsAdmin));
            parametersList.Add(new SqlParameter("@Name", userInfo.Name));
            parametersList.Add(new SqlParameter("@Telephone", userInfo.Telephone));
            parametersList.Add(new SqlParameter("@Email", userInfo.Email));
            parametersList.Add(new SqlParameter("@HeadImg", userInfo.HeadImg));
            parametersList.Add(new SqlParameter("@State", (int)userInfo.State));
            parametersList.Add(new SqlParameter("@UserId", userInfo.UserId));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除会员
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete [User] where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion
    }
}

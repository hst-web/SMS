/*----------------------------------------------------------------
// 文件名：OrganizationProvider.cs
// 功能描述： 单位数据提供者
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
    /// 企业信息数据提供者
    /// </summary>
    public class OrganizationProvider : EntityProviderBase
    {
        #region 查询企业信息
        /// <summary>
        /// 根据ID获取单位信息
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns>单位信息</returns>
        public Organization Get(int id)
        {
            Organization orgInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT Id,Number, Name, Logo, Telephone, Email,Address, WeChat, Blog, Description, Framework,Detail, CreateDate  from Organization where id=@Id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    orgInfo = GetOrgFromReader(reader);
                }
            }

            return orgInfo;
        }

        public Organization GetByNumber(string number)
        {
            Organization orgInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT Id,Number, Name, Logo, Telephone, Email, WeChat,Address, Blog, Description, Framework,Detail, CreateDate  from Organization where Number=@Number";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Number", number));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    orgInfo = GetOrgFromReader(reader);
                }
            }

            return orgInfo;
        }

        /// <summary>
        /// 获取所有单位信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>单位集合</returns>
        public List<Organization> GetAll()
        {
            List<Organization> orgList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT Id, Name,Number, Logo, Telephone, Email,Address, WeChat, Blog,CreateDate  from Organization";

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, null))
            {
                orgList = new List<Organization>();
                Organization OrganizationInfo = null;
                while (reader.Read())
                {
                    OrganizationInfo = GetOrgFromReader(reader);
                    orgList.Add(OrganizationInfo);
                }
            }

            return orgList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Organization GetOrgFromReader(DbDataReader reader)
        {
            Organization orgInfo = new Organization();
            orgInfo.Id = Convert.ToInt32(reader["Id"]);

            if (ReaderExists(reader, "Description") && DBNull.Value != reader["Description"])
            {
                orgInfo.Description = reader["Description"].ToString();
            }

            if (ReaderExists(reader, "Framework") && DBNull.Value != reader["Framework"])
            {
                orgInfo.Framework = reader["Framework"].ToString();
            }
            if (ReaderExists(reader, "Detail") && DBNull.Value != reader["Detail"])
            {
                orgInfo.Detail = reader["Detail"].ToString();
            }
            if (ReaderExists(reader, "Address") && DBNull.Value != reader["Address"])
            {
                orgInfo.Address = reader["Address"].ToString();
            }
            orgInfo.Number = reader["Number"].ToString();
            orgInfo.Name = reader["Name"].ToString();
            orgInfo.Email = reader["Email"].ToString();
            orgInfo.Telephone = reader["Telephone"].ToString();
            orgInfo.Logo = reader["Logo"].ToString();
            orgInfo.WeChat = reader["WeChat"].ToString();
            orgInfo.Blog = reader["Blog"].ToString();       
            orgInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

            return orgInfo;
        }
        #endregion

        #region 编辑单位

        /// <summary>
        /// 修改单位
        /// </summary>
        /// <param name="OrganizationInfo">单位信息</param>
        /// <returns>添加成功标识</returns>
        public bool Update(Organization OrganizationInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"if exists(select Id from Organization where Number=@Number)
                                begin
                                    update Organization set Name=@Name,Logo=@Logo,Email=@Email,Telephone=@Telephone,WeChat=@WeChat,Blog=@Blog,Description=@Description,Framework=@Framework,Detail=@Detail,Address=@Address  where Number=@Number 
                                end
                                else
                                begin
                                Insert Into Organization( Name, Logo, Number, Telephone, Email, WeChat, Blog, Description, Framework,Detail,Address) 
                                   Values(@Name, @Logo, @Number, @Telephone, @Email, @WeChat, @Blog, @Description, @Framework,@Detail,@Address) 
                                end ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Name", OrganizationInfo.Name));
            parametersList.Add(new SqlParameter("@Logo", OrganizationInfo.Logo));
            parametersList.Add(new SqlParameter("@Telephone", OrganizationInfo.Telephone));
            parametersList.Add(new SqlParameter("@Email", OrganizationInfo.Email));
            parametersList.Add(new SqlParameter("@WeChat", OrganizationInfo.WeChat));
            parametersList.Add(new SqlParameter("@Blog", OrganizationInfo.Blog));
            parametersList.Add(new SqlParameter("@Description", OrganizationInfo.Description));
            parametersList.Add(new SqlParameter("@Framework", OrganizationInfo.Framework));
            parametersList.Add(new SqlParameter("@Detail", OrganizationInfo.Detail));
            parametersList.Add(new SqlParameter("@Number", OrganizationInfo.Number));
            parametersList.Add(new SqlParameter("@Address", OrganizationInfo.Address));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete Organization where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion
    }
}

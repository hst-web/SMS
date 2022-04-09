/*----------------------------------------------------------------
// 文件名：CategoryDictionaryProvider.cs
// 功能描述： 类别字典数据提供者
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
    public class CategoryDictionaryProvider : EntityProviderBase
    {
        #region 查询类别字典
        /// <summary>
        /// 根据ID获取类别字典信息
        /// </summary>
        /// <param name="id">类别字典ID</param>
        /// <returns>类别字典信息</returns>
        public CategoryDictionary Get(int id)
        {
            CategoryDictionary categoryInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT cd.Id, cd.UserId, cd.Parent, cd.Name, cd.Type, cd.State, cd.CreateDate,u.Name UserName,cdp.Name ParentName from CategoryDictionary cd left join [User] u on cd.UserId=u.Id left join CategoryDictionary cdp on cd.Parent=cdp.Id where cd.IsDeleted=0 and cd.id=@Id ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    categoryInfo = GetCategoryDictionaryFromReader(reader);
                }
            }

            return categoryInfo;
        }

        /// <summary>
        /// 获取所有类别字典信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>类别字典集合</returns>
        public List<CategoryDictionary> GetAll(FilterEntityModel condition)
        {
            string whereSort = string.Empty;

            if (condition != null)
            {
                condition.DefaultSort = SortType.Desc;
                whereSort = condition.Where + condition.OrderBy;
            }

            List<CategoryDictionary> categoryList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT cd.Id, cd.UserId, cd.Parent, cd.Name, cd.Type, cd.State, cd.CreateDate,u.Name UserName,cdp.Name ParentName from CategoryDictionary cd left join [User] u on cd.UserId=u.Id left join CategoryDictionary cdp on cd.Parent=cdp.Id where cd.IsDeleted=0 " + whereSort;

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
                categoryList = new List<CategoryDictionary>();
                CategoryDictionary categoryInfo = null;
                while (reader.Read())
                {
                    categoryInfo = GetCategoryDictionaryFromReader(reader);
                    categoryList.Add(categoryInfo);
                }
            }

            return categoryList;
        }

        public List<int> GetCategorysByPartentId(int partentId)
        {
            List<int> categoryList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT Id  from CategoryDictionary  where IsDeleted=0 and state=1 and  parent=@parent";

            IList<DbParameter> parameList = new List<DbParameter>();
            parameList.Add(new SqlParameter("@parent", partentId));


            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                categoryList = new List<int>();
                while (reader.Read())
                {                   
                    categoryList.Add(Convert.ToInt32(reader["Id"]));
                }
            }

            return categoryList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private CategoryDictionary GetCategoryDictionaryFromReader(DbDataReader reader)
        {
            CategoryDictionary categoryInfo = new CategoryDictionary();
            categoryInfo.Id = Convert.ToInt32(reader["Id"]);
            categoryInfo.UserId = Convert.ToInt32(reader["UserId"]);
            categoryInfo.Name = reader["Name"].ToString();
            categoryInfo.Parent = Convert.ToInt32(reader["Parent"]);
            categoryInfo.Type = (CategoryType)reader["Type"];
            categoryInfo.State = Convert.ToInt32(reader["State"]) < 1 ? PublishState.Lower : PublishState.Upper;
            categoryInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);
            categoryInfo.ParentName = reader["ParentName"].ToString();
            categoryInfo.UserName = reader["UserName"].ToString();
            return categoryInfo;
        }
        #endregion

        #region 编辑类别字典
        /// <summary>
        /// 添加类别字典
        /// </summary>
        /// <param name="categoryInfo">类别字典信息</param>
        /// <returns>添加成功标识</returns>
        public bool Add(CategoryDictionary categoryInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"if exists(select Id from CategoryDictionary where [Name]=@Name)
                                begin
                                    update CategoryDictionary set UserId=@UserId,Parent=@Parent,State=@State,Type=@Type,CreateDate=getdate(),IsDeleted=0 where Name=@Name 
                                end
                                else
                                begin
                                Insert Into CategoryDictionary (UserId, Parent, [Name], [Type], [State]) Values (@UserId, @Parent, @Name, @Type, @State)
                                end ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@UserId", categoryInfo.UserId));
            parametersList.Add(new SqlParameter("@Parent", categoryInfo.Parent));
            parametersList.Add(new SqlParameter("@Name", categoryInfo.Name));
            parametersList.Add(new SqlParameter("@State", (int)categoryInfo.State));
            parametersList.Add(new SqlParameter("@Type", (int)categoryInfo.Type));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 修改类别字典
        /// </summary>
        /// <param name="categoryInfo">类别字典信息</param>
        /// <returns>修改成功标识</returns>
        public bool Update(CategoryDictionary categoryInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update CategoryDictionary
                              Set [UserId]=@UserId
                                  ,[Parent]=@Parent
                                  ,[State]=@State
                                  ,[Name]=@Name
                                  ,[Type]=@Type
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", categoryInfo.Id));
            parametersList.Add(new SqlParameter("@UserId", categoryInfo.UserId));
            parametersList.Add(new SqlParameter("@Parent", categoryInfo.Parent));
            parametersList.Add(new SqlParameter("@Name", categoryInfo.Name));
            parametersList.Add(new SqlParameter("@State", (int)categoryInfo.State));
            parametersList.Add(new SqlParameter("@Type", (int)categoryInfo.Type));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除类别字典
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete CategoryDictionary where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion
    }
}

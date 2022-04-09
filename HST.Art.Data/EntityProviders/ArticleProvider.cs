/*----------------------------------------------------------------
// 文件名：ArticleProvider.cs
// 功能描述： 文章数据提供者
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
    public class ArticleProvider : EntityProviderBase
    {
        #region 查询文章
        /// <summary>
        /// 根据ID获取文章信息
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns>文章信息</returns>
        public Article Get(int id)
        {
            Article articleInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT a.Id, a.UserId, a.Title, a.HeadImg, a.Content, a.Author, a.Section, a.State, a.ParCategory, a.Category,cd.Name as CategoryName,u.Name as UserName,pcd.Name as ParCategoryName, a.UpdateDate, a.CreateDate,a.Synopsis,a.PublishDate  from Article a  inner join CategoryDictionary cd on a.category=cd.id left join CategoryDictionary pcd on a.ParCategory=pcd.id left join [User] u on a.userid=u.id where  a.IsDeleted=0 and a.id=@Id ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    articleInfo = GetArticleFromReader(reader);
                }
            }

            return articleInfo;
        }

        /// <summary>
        /// 获取所有文章信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>文章集合</returns>
        public List<Article> GetAll(FilterEntityModel condition)
        {
            string whereSort = string.Empty;

            if (condition != null)
            {
                condition.DefaultSort = SortType.Desc;
                whereSort = condition.Where + condition.OrderBy;
            }

            List<Article> articleList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT a.Id, a.UserId, a.Title, a.HeadImg, a.Content, a.Author, a.Section, a.State, a.ParCategory, a.Category,cd.Name as CategoryName,u.Name as UserName,pcd.Name as ParCategoryName, a.UpdateDate, a.CreateDate, a.Synopsis,a.PublishDate  from Article a  inner join CategoryDictionary cd on a.category=cd.id left join CategoryDictionary pcd on a.ParCategory=pcd.id left join [User] u on a.userid=u.id where  a.IsDeleted=0 " + whereSort;

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
                articleList = new List<Article>();
                Article articleInfo = null;
                while (reader.Read())
                {
                    articleInfo = GetArticleFromReader(reader);
                    articleList.Add(articleInfo);
                }
            }

            return articleList;
        }

        /// <summary>
        /// 获取文章分页信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <param name="totalNum">总条数</param>
        /// <returns>文章集合</returns>
        public List<Article> GetPage(FilterEntityModel condition, out int totalNum)
        {
            totalNum = 0;
            condition.DefaultSort = SortType.Desc;
            condition.SortTbAsName = Constant.ARTICLE_AS_NAME;
            string sort = condition.OrderBy;
            string asSort = condition.AsOrderBy;
            string where = condition.Where;

            List<Article> articleList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select count(a.ID) from [Article] a inner join CategoryDictionary cd on a.Category=cd.id where  a.IsDeleted=0 " + where;//查询有多少条记录
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
                                  ,[UserId]
                                  ,[Title]
                                  ,Synopsis
                                  ,[HeadImg]
                                  ,[Author]
                                  ,[Section]
                                  ,[State]
                                  ,[ParCategory]
                                  ,[Category]
                                  ,[UpdateDate]
                                  ,[CreateDate]
                                  ,[PublishDate]
                                  ,[UserName]
                                  ,[CategoryName]
                                  ,[ParCategoryName]
                            FROM (select top (@pageSize*@pageIndex)  a.[ID]
                                  ,a.[UserId]
                                  ,a.[Title]
                                  ,a.Synopsis
                                  ,a.[HeadImg]
                                  ,a.[Author]
                                  ,a.[Section]
                                  ,a.[State]
                                  ,a.[ParCategory]
                                  ,a.[Category]
                                  ,a.[UpdateDate]
                                  ,a.[CreateDate]
                                  ,a.[PublishDate]
                                  ,u.[Name] as UserName
                                  ,cd.Name as [CategoryName]
                                  ,pcd.Name as [ParCategoryName]
                                    ,ROW_NUMBER() over(" + asSort + ") as num  from [Article] a  inner join CategoryDictionary cd on a.category=cd.id left join CategoryDictionary pcd on a.ParCategory=pcd.id left join [User] u on a.userid=u.id  where  a.IsDeleted=0 " + where + ") as t where num between (@pageIndex - 1) * @pageSize + 1  and @pageIndex*@pageSize " + sort;
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                articleList = new List<Article>();
                Article articleInfo = null;
                while (reader.Read())
                {
                    articleInfo = GetArticleFromReader(reader);
                    articleList.Add(articleInfo);
                }
            }

            return articleList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Article GetArticleFromReader(DbDataReader reader)
        {
            Article articleInfo = new Article();
            articleInfo.Id = Convert.ToInt32(reader["Id"]);
            articleInfo.UserId = Convert.ToInt32(reader["UserId"]);
            articleInfo.Title = reader["Title"].ToString();
            articleInfo.Author = reader["Author"].ToString();
            articleInfo.Section = (SectionType)reader["Section"];
            articleInfo.State = Convert.ToInt32(reader["State"]) < 1 ? PublishState.Lower : PublishState.Upper;
            articleInfo.Category = Convert.ToInt32(reader["Category"]);
            articleInfo.ParCategory = Convert.ToInt32(reader["ParCategory"]);
            articleInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

            if (ReaderExists(reader, "CategoryName") && DBNull.Value != reader["CategoryName"])
            {
                articleInfo.CategoryName = reader["CategoryName"].ToString();
            }
            if (ReaderExists(reader, "Synopsis") && DBNull.Value != reader["Synopsis"])
            {
                articleInfo.Synopsis = reader["Synopsis"].ToString();
            }
            if (ReaderExists(reader, "ParCategoryName") && DBNull.Value != reader["ParCategoryName"])
            {
                articleInfo.ParCategoryName = reader["ParCategoryName"].ToString();
            }
            if (ReaderExists(reader, "UserName") && DBNull.Value != reader["UserName"])
            {
                articleInfo.UserName = reader["UserName"].ToString();
            }
            if (ReaderExists(reader, "HeadImg") && DBNull.Value != reader["HeadImg"])
            {
                articleInfo.HeadImg = reader["HeadImg"].ToString();
            }
            if (ReaderExists(reader, "Content") && DBNull.Value != reader["Content"])
            {
                articleInfo.Content = reader["Content"].ToString();
            }
            if (ReaderExists(reader, "UpdateDate") && DBNull.Value != reader["UpdateDate"])
            {
                articleInfo.UpdateDate = Convert.ToDateTime(reader["UpdateDate"]);
            }
            if (ReaderExists(reader, "PublishDate") && DBNull.Value != reader["PublishDate"])
            {
                articleInfo.PublishDate = Convert.ToDateTime(reader["PublishDate"]);
            }

            return articleInfo;
        }
        #endregion

        #region 编辑文章
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="articleInfo">文章信息</param>
        /// <returns>添加成功标识</returns>
        public bool Add(Article articleInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into Article (UserId, Title, HeadImg, Content, Author, Section, State, ParCategory, Category,Synopsis) Values (@UserId, @Title, @HeadImg, @Content, @Author, @Section, @State,@ParCategory, @Category,@Synopsis)";

            if (articleInfo.State == PublishState.Upper)
            {
               strSql = @"Insert Into Article (UserId, Title, HeadImg, Content, Author, Section, State, ParCategory, Category,Synopsis,PublishDate) Values (@UserId, @Title, @HeadImg, @Content, @Author, @Section, @State,@ParCategory, @Category,@Synopsis,getdate())";         
            }

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@UserId", articleInfo.UserId));
            parametersList.Add(new SqlParameter("@Title", articleInfo.Title));
            parametersList.Add(new SqlParameter("@HeadImg", articleInfo.HeadImg));
            parametersList.Add(new SqlParameter("@Content", articleInfo.Content));
            parametersList.Add(new SqlParameter("@Author", articleInfo.Author));
            parametersList.Add(new SqlParameter("@Section", (int)articleInfo.Section));
            parametersList.Add(new SqlParameter("@Category", articleInfo.Category));
            parametersList.Add(new SqlParameter("@ParCategory", articleInfo.ParCategory));
            parametersList.Add(new SqlParameter("@State", (int)articleInfo.State));
            parametersList.Add(new SqlParameter("@Synopsis", articleInfo.Synopsis));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="articleInfo">文章信息</param>
        /// <returns>修改成功标识</returns>
        public bool Update(Article articleInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update Article
                              Set [UserId]=@UserId
                                  ,[HeadImg]=@HeadImg 
                                  ,[Title]=@Title
                                  ,[Content]=@Content
                                  ,[State]=@State
                                  ,[Category]=@Category
                                  ,[Author]=@Author
                                  ,[Section]=@Section
                                  ,[ParCategory]=@ParCategory
                                  ,[Synopsis]=@Synopsis
                                  ,[UpdateDate]=getdate()
                                  ,[PublishDate]=@PublishDate
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", articleInfo.Id));
            parametersList.Add(new SqlParameter("@UserId", articleInfo.UserId));
            parametersList.Add(new SqlParameter("@Title", articleInfo.Title));
            parametersList.Add(new SqlParameter("@HeadImg", articleInfo.HeadImg));
            parametersList.Add(new SqlParameter("@Content", articleInfo.Content));
            parametersList.Add(new SqlParameter("@Author", articleInfo.Author));
            parametersList.Add(new SqlParameter("@Section", (int)articleInfo.Section));
            parametersList.Add(new SqlParameter("@Category", articleInfo.Category));
            parametersList.Add(new SqlParameter("@ParCategory", articleInfo.ParCategory));
            parametersList.Add(new SqlParameter("@State", (int)articleInfo.State));
            parametersList.Add(new SqlParameter("@Synopsis", articleInfo.Synopsis));
            parametersList.Add(new SqlParameter("@PublishDate", articleInfo.PublishDate != null && articleInfo.PublishDate > DateTime.MinValue ? articleInfo.PublishDate.ToString() : ""));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete Article where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 上架特殊处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Publish(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "update Article set state=1,PublishDate=getdate()  where Id=@Id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion

        #region 统计
        public List<ArticleStatistic> GetStatisticArticles()
        {
            string strSql = @"select Section,COUNT(*) as ArticleCount from Article where IsDeleted = 0 group by section";
            List<ArticleStatistic> articleStatisticList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, null))
            {
                articleStatisticList = new List<ArticleStatistic>();
                while (reader.Read())
                {
                    ArticleStatistic artStatistic = new ArticleStatistic();
                    artStatistic.SectionType = (SectionType)reader["Section"];
                    artStatistic.SectionCount = Convert.ToInt32(reader["ArticleCount"]);
                    articleStatisticList.Add(artStatistic);
                }
            }

            return articleStatisticList;
        }
        #endregion
    }
}

/*----------------------------------------------------------------
// 文件名：FileDownloadProvider.cs
// 功能描述： 文件下载数据提供者
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
    public class FileDownloadProvider : EntityProviderBase
    {
        #region 查询文件下载
        /// <summary>
        /// 根据ID获取文件下载信息
        /// </summary>
        /// <param name="id">文件下载ID</param>
        /// <returns>文件下载信息</returns>
        public FileDownload Get(int id)
        {
            FileDownload fileDownloadInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT fd.Id, fd.UserId, fd.Name, fd.Title, fd.Category, fd.Type, fd.Src, fd.State, fd.Description, fd.Synopsis,fd.HeadImg, fd.CreateDate ,cd.Name as CategoryName,u.Name as UserName  from FileDownload fd  inner join CategoryDictionary cd on fd.category=cd.id  left join [User] u on fd.userid=u.id where  fd.IsDeleted=0 and fd.id=@Id ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    fileDownloadInfo = GetFileDownloadFromReader(reader);
                }
            }

            return fileDownloadInfo;
        }

        /// <summary>
        /// 获取所有文件下载信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>文件下载集合</returns>
        public List<FileDownload> GetAll(FilterEntityModel condition)
        {
            string whereSort = string.Empty;

            if (condition != null)
            {
                condition.DefaultSort = SortType.Desc;
                whereSort = condition.Where + condition.OrderBy;
            }

            List<FileDownload> fileDownloadList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT fd.Id, fd.UserId, fd.Name, fd.Title, fd.Category, fd.Type, fd.Src, fd.State, fd.HeadImg, fd.Synopsis,fd.CreateDate ,cd.Name as CategoryName,u.Name as UserName  from FileDownload fd  inner join CategoryDictionary cd on fd.category=cd.id  left join [User] u on fd.userid=u.id where  fd.IsDeleted=0 " + whereSort;

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
                fileDownloadList = new List<FileDownload>();
                FileDownload fileDownloadInfo = null;
                while (reader.Read())
                {
                    fileDownloadInfo = GetFileDownloadFromReader(reader);
                    fileDownloadList.Add(fileDownloadInfo);
                }
            }

            return fileDownloadList;
        }

        /// <summary>
        /// 获取文件下载分页信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <param name="totalNum">总条数</param>
        /// <returns>文件下载集合</returns>
        public List<FileDownload> GetPage(FilterEntityModel condition, out int totalNum)
        {
            totalNum = 0;
            condition.DefaultSort = SortType.Desc;
            condition.SortTbAsName = Constant.FILE_DOWNLOAD_AS_NAME;
            string sort = condition.OrderBy;
            string asSort = condition.AsOrderBy;
            string where = condition.Where;

            List<FileDownload> fileDownloadList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select count(fd.ID) from [FileDownload] fd inner join CategoryDictionary cd on fd.Category=cd.id where  fd.IsDeleted=0 " + where;//查询有多少条记录
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
                                  ,[Name]
                                  ,[Title]
                                  ,[Category]
                                  ,[Type]
                                  ,[Src]
                                  ,[State]
                                  ,[HeadImg]
                                  ,[Synopsis]
                                  ,[CreateDate]
                                  ,[UserName]
                                  ,[CategoryName]
                            FROM (select top (@pageSize*@pageIndex)  fd.[ID]
                                  ,fd.[UserId]
                                  ,fd.Name
                                  ,fd.[Title]
                                  ,fd.[Category]
                                  ,fd.[Type]
                                  ,fd.[Src]
                                  ,fd.[State]
                                  ,fd.[HeadImg]
                                  ,fd.Synopsis
                                  ,fd.[CreateDate]
                                  ,u.[Name] as UserName
                                  ,cd.Name as [CategoryName]
                                    ,ROW_NUMBER() over(" + asSort + ") as num  from [FileDownload] fd  inner join CategoryDictionary cd on fd.category=cd.id left join [User] u on fd.userid=u.id  where  fd.IsDeleted=0 " + where + ") as t where num between (@pageIndex - 1) * @pageSize + 1  and @pageIndex*@pageSize " + sort;
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                fileDownloadList = new List<FileDownload>();
                FileDownload fileDownloadInfo = null;
                while (reader.Read())
                {
                    fileDownloadInfo = GetFileDownloadFromReader(reader);
                    fileDownloadList.Add(fileDownloadInfo);
                }
            }

            return fileDownloadList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private FileDownload GetFileDownloadFromReader(DbDataReader reader)
        {
            FileDownload fileDownloadInfo = new FileDownload();
            fileDownloadInfo.Id = Convert.ToInt32(reader["Id"]);
            fileDownloadInfo.UserId = Convert.ToInt32(reader["UserId"]);
            fileDownloadInfo.Name = reader["Name"].ToString();
            fileDownloadInfo.Title = reader["Title"].ToString();
            fileDownloadInfo.Category = Convert.ToInt32(reader["Category"]);
            fileDownloadInfo.Type = (FileFormat)reader["Type"];
            fileDownloadInfo.Src = reader["Src"].ToString();
            fileDownloadInfo.State = Convert.ToInt32(reader["State"]) < 1 ? PublishState.Lower : PublishState.Upper;          
            fileDownloadInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

            if (ReaderExists(reader, "CategoryName") && DBNull.Value != reader["CategoryName"])
            {
                fileDownloadInfo.CategoryName = reader["CategoryName"].ToString();
            }           
            if (ReaderExists(reader, "UserName") && DBNull.Value != reader["UserName"])
            {
                fileDownloadInfo.UserName = reader["UserName"].ToString();
            }
            if (ReaderExists(reader, "HeadImg") && DBNull.Value != reader["HeadImg"])
            {
                fileDownloadInfo.HeadImg = reader["HeadImg"].ToString();
            }
            if (ReaderExists(reader, "Synopsis") && DBNull.Value != reader["Synopsis"])
            {
                fileDownloadInfo.Synopsis = reader["Synopsis"].ToString();
            }
            if (ReaderExists(reader, "Description") && DBNull.Value != reader["Description"])
            {
                fileDownloadInfo.Description = reader["Description"].ToString();
            }

            return fileDownloadInfo;
        }
        #endregion

        #region 编辑文件下载
        /// <summary>
        /// 添加文件下载
        /// </summary>
        /// <param name="fileDownloadInfo">文件下载信息</param>
        /// <returns>添加成功标识</returns>
        public bool Add(FileDownload fileDownloadInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into FileDownload (UserId, Name, Title, Category, Type, Src, State, Description,Synopsis, HeadImg) Values (@UserId, @Name, @Title, @Category, @Type, @Src, @State, @Description,@Synopsis, @HeadImg)";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@UserId", fileDownloadInfo.UserId));
            parametersList.Add(new SqlParameter("@Title", fileDownloadInfo.Title));
            parametersList.Add(new SqlParameter("@Name", fileDownloadInfo.Name));
            parametersList.Add(new SqlParameter("@Category", fileDownloadInfo.Category));
            parametersList.Add(new SqlParameter("@Type", (int)fileDownloadInfo.Type));
            parametersList.Add(new SqlParameter("@HeadImg", fileDownloadInfo.HeadImg));
            parametersList.Add(new SqlParameter("@Description", fileDownloadInfo.Description));  
            parametersList.Add(new SqlParameter("@Src", fileDownloadInfo.Src));
            parametersList.Add(new SqlParameter("@State", (int)fileDownloadInfo.State));
            parametersList.Add(new SqlParameter("@Synopsis", fileDownloadInfo.Synopsis));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 修改文件下载
        /// </summary>
        /// <param name="fileDownloadInfo">文件下载信息</param>
        /// <returns>修改成功标识</returns>
        public bool Update(FileDownload fileDownloadInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update FileDownload
                              Set [UserId]=@UserId
                                  ,[Name]=@Name
                                  ,[Title]=@Title
                                  ,[Category]=@Category
                                  ,[Type]=@Type
                                  ,[Src]=@Src
                                  ,[State]=@State
                                  ,[HeadImg]=@HeadImg 
                                  ,[Description]=@Description
                                  ,[Synopsis]=@Synopsis
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", fileDownloadInfo.Id));
            parametersList.Add(new SqlParameter("@UserId", fileDownloadInfo.UserId));
            parametersList.Add(new SqlParameter("@Title", fileDownloadInfo.Title));
            parametersList.Add(new SqlParameter("@HeadImg", fileDownloadInfo.HeadImg));
            parametersList.Add(new SqlParameter("@Description", fileDownloadInfo.Description));
            parametersList.Add(new SqlParameter("@Name", fileDownloadInfo.Name));
            parametersList.Add(new SqlParameter("@Category", fileDownloadInfo.Category));
            parametersList.Add(new SqlParameter("@Src", fileDownloadInfo.Src));
            parametersList.Add(new SqlParameter("@State", (int)fileDownloadInfo.State));
            parametersList.Add(new SqlParameter("@Type", (int)fileDownloadInfo.Type));
            parametersList.Add(new SqlParameter("@Synopsis", fileDownloadInfo.Synopsis));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除文件下载
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete FileDownload where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion
    }
}

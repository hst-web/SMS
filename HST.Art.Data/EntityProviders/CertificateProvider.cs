/*----------------------------------------------------------------
// 文件名：CertificateProvider.cs
// 功能描述： 证书数据提供者
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
    /// 证书信息数据提供者
    /// </summary>
    public class CertificateProvider : EntityProviderBase
    {
        #region 教师证书
        /// <summary>
        /// 根据ID获取证书信息
        /// </summary>
        /// <param name="id">证书ID</param>
        /// <returns>证书信息</returns>
        public TeaCertificate GetTea(int id)
        {
            TeaCertificate teaInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT tc.Id, tc.UserId, tc.Name, tc.Gender, tc.HeadImg, tc.Number,u.Name as UserName, tc.State, tc.Category, tc.Level, tc.Province, tc.City, tc.County, tc.CreateDate  from TeaCertificate tc inner join  [User] u on tc.UserId=u.Id where tc.IsDeleted=0 and tc.id=@Id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    teaInfo = GetTeaFromReader(reader);
                }
            }

            return teaInfo;
        }

        public TeaCertificate GetTeaByNumber(string number)
        {
            TeaCertificate teaInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT tc.Id, tc.UserId, tc.Name, tc.Gender, tc.HeadImg, tc.Number,u.Name as UserName, tc.State, tc.Category, tc.Level, tc.Province, tc.City, tc.County, tc.CreateDate  from TeaCertificate tc inner join  [User] u on tc.UserId=u.Id where tc.IsDeleted=0 and tc.Number=@Number";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Number", number));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    teaInfo = GetTeaFromReader(reader);
                }
            }

            return teaInfo;
        }

        /// <summary>
        /// 获取所有证书信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>证书集合</returns>
        public List<TeaCertificate> GetTeaAll(FilterEntityModel condition)
        {
            string whereSort = string.Empty;

            if (condition != null)
            {
                condition.DefaultSort = SortType.Desc;
                whereSort = condition.Where + condition.OrderBy;
            }

            List<TeaCertificate> teaList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT tc.Id, tc.UserId, tc.Name, tc.Gender, tc.HeadImg, tc.Number,u.Name as UserName, tc.State, tc.Category, tc.Level, tc.Province, tc.City, tc.County, tc.CreateDate  from TeaCertificate tc inner join  [User] u on tc.UserId=u.Id where tc.IsDeleted=0 " + whereSort;

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
                teaList = new List<TeaCertificate>();
                TeaCertificate teaInfo = null;
                while (reader.Read())
                {
                    teaInfo = GetTeaFromReader(reader);
                    teaList.Add(teaInfo);
                }
            }

            return teaList;
        }

        public List<TeaCertificate> GetTeaPage(FilterEntityModel condition, out int totalNum)
        {
            totalNum = 0;
            condition.DefaultSort = SortType.Desc;
            condition.SortTbAsName = Constant.TEA_CERTIFICATE_AS_NAME;
            string sort = condition.OrderBy;
            string asSort = condition.AsOrderBy;
            string where = condition.Where;

            List<TeaCertificate> teaList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select count(tc.ID) from [TeaCertificate] tc inner join [User] u on tc.UserId=u.Id  where tc.IsDeleted=0 " + where;//查询有多少条记录
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
                                  ,[Gender]
                                  ,[HeadImg]
                                  ,[Number]
                                  ,[State]
                                  ,[Category]
                                  ,[Level]
                                  ,[Province]
                                  ,[City]
                                  ,[County]
                                  ,[CreateDate]
                                  ,[UserName]
                            FROM (select top (@pageSize*@pageIndex)  tc.[ID]
                                  ,tc.[UserId]
                                  ,tc.[Name]
                                  ,tc.[Gender]
                                  ,tc.[HeadImg]
                                  ,tc.[Number]
                                  ,tc.[State]
                                  ,tc.[Category]
                                  ,tc.[Level]
                                  ,tc.[Province]
                                  ,tc.[City]
                                  ,tc.[County]
                                  ,tc.[CreateDate]
                                  ,u.[Name] as UserName
                                    ,ROW_NUMBER() over(" + asSort + ") as num  from [TeaCertificate] tc inner join [User] u on tc.UserId=u.Id  where tc.IsDeleted=0 " + where + ") as t where num between (@pageIndex - 1) * @pageSize + 1  and @pageIndex*@pageSize " + sort;
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                teaList = new List<TeaCertificate>();
                TeaCertificate teaInfo = null;
                while (reader.Read())
                {
                    teaInfo = GetTeaFromReader(reader);
                    teaList.Add(teaInfo);
                }
            }

            return teaList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private TeaCertificate GetTeaFromReader(DbDataReader reader)
        {
            TeaCertificate teaInfo = new TeaCertificate();
            teaInfo.Id = Convert.ToInt32(reader["Id"]);
            teaInfo.UserId = Convert.ToInt32(reader["UserId"]);
            teaInfo.Number = reader["Number"].ToString();
            teaInfo.Name = reader["Name"].ToString();
            teaInfo.Gender = (Gender)Convert.ToInt32(reader["Gender"]);
            teaInfo.State = (PublishState)reader["State"];
            teaInfo.Category = (CertificateType)reader["Category"];
            teaInfo.Level = (LevelType)reader["Level"];
            teaInfo.UserName = reader["UserName"].ToString();

            if (ReaderExists(reader, "HeadImg") && DBNull.Value != reader["HeadImg"])
            {
                teaInfo.HeadImg = reader["HeadImg"].ToString();
            }
            if (ReaderExists(reader, "Province") && DBNull.Value != reader["Province"])
            {
                teaInfo.Province = reader["Province"].ToString();
            }
            if (ReaderExists(reader, "City") && DBNull.Value != reader["City"])
            {
                teaInfo.City = reader["City"].ToString();
            }
            if (ReaderExists(reader, "County") && DBNull.Value != reader["County"])
            {
                teaInfo.County = reader["County"].ToString();
            }

            teaInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

            return teaInfo;
        }


        /// <summary>
        /// 添加教师证书
        /// </summary>
        /// <param name="teaInfo">教师证书信息</param>
        /// <returns>添加成功标识</returns>
        public bool AddTea(TeaCertificate teaInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"if exists(select Id from TeaCertificate where Number=@Number)
                                begin
                                    update TeaCertificate set UserId=@UserId,Name=@Name,Gender=@Gender,HeadImg=@HeadImg,State=@State,Category=@Category,Level=@Level,Province=@Province,City=@City,County=@County,CreateDate=getdate(),IsDeleted=0 where Number=@Number 
                                end
                                else
                                begin
                                Insert Into TeaCertificate(UserId, Name, Gender, HeadImg, Number, State, Category, Level, Province, City, County) 
                                   Values(@UserId, @Name, @Gender, @HeadImg, @Number, @State, @Category, @Level, @Province, @City, @County) 
                                end ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@UserId", teaInfo.UserId));
            parametersList.Add(new SqlParameter("@Name", teaInfo.Name));
            parametersList.Add(new SqlParameter("@Gender", (int)teaInfo.Gender));
            parametersList.Add(new SqlParameter("@HeadImg", teaInfo.HeadImg));
            parametersList.Add(new SqlParameter("@Number", teaInfo.Number));
            parametersList.Add(new SqlParameter("@State", (int)teaInfo.State));
            parametersList.Add(new SqlParameter("@Category", (int)teaInfo.Category));
            parametersList.Add(new SqlParameter("@Level", (int)teaInfo.Level));
            parametersList.Add(new SqlParameter("@Province", teaInfo.Province));
            parametersList.Add(new SqlParameter("@City", teaInfo.City));
            parametersList.Add(new SqlParameter("@County", teaInfo.County));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        public bool AddTea(List<TeaCertificate> teaInfos, out List<TeaCertificate> failList)
        {
            failList = new List<TeaCertificate>();
            bool success = false;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"if exists(select Id from TeaCertificate where Number=@Number)
                                begin
                                    update TeaCertificate set UserId=@UserId,Name=@Name,Gender=@Gender,HeadImg=@HeadImg,State=@State,Category=@Category,Level=@Level,Province=@Province,City=@City,County=@County,CreateDate=getdate(),IsDeleted=0 where Number=@Number 
                                end
                                else
                                begin
                                Insert Into TeaCertificate(UserId, Name, Gender, HeadImg, Number, State, Category, Level, Province, City, County) 
                                   Values(@UserId, @Name, @Gender, @HeadImg, @Number, @State, @Category, @Level, @Province, @City, @County) 
                                end ";

            List<DbParameter> parametersList = null;
            foreach (TeaCertificate item in teaInfos)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@UserId", item.UserId));
                parametersList.Add(new SqlParameter("@Name", item.Name));
                parametersList.Add(new SqlParameter("@Gender", (int)item.Gender));
                parametersList.Add(new SqlParameter("@HeadImg", item.HeadImg));
                parametersList.Add(new SqlParameter("@Number", item.Number));
                parametersList.Add(new SqlParameter("@State", (int)item.State));
                parametersList.Add(new SqlParameter("@Category", (int)item.Category));
                parametersList.Add(new SqlParameter("@Level", (int)item.Level));
                parametersList.Add(new SqlParameter("@Province", item.Province));
                parametersList.Add(new SqlParameter("@City", item.City));
                parametersList.Add(new SqlParameter("@County", item.County));

                success = dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;

                if (!success)
                {
                    failList.Add(item);
                }
            }

            return success && failList.Count == 0;
        }

        /// <summary>
        /// 修改教师证书
        /// </summary>
        /// <param name="teaInfo">教师证书信息</param>
        /// <returns>修改成功标识</returns>
        public bool UpdatTea(TeaCertificate teaInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update TeaCertificate 
                              Set [Name]=@Name
                                  ,[UserId]=@UserId 
                                  ,[HeadImg]=@HeadImg 
                                  ,[Gender]=@Gender
                                  ,[Number]=@Number
                                  ,[State]=@State
                                  ,[Category]=@Category
                                  ,[Level]=@Level
                                  ,[Province]=@Province
                                  ,[City]=@City
                                  ,[County]=@County
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", teaInfo.Id));
            parametersList.Add(new SqlParameter("@UserId", teaInfo.UserId));
            parametersList.Add(new SqlParameter("@Name", teaInfo.Name));
            parametersList.Add(new SqlParameter("@HeadImg", teaInfo.HeadImg));
            parametersList.Add(new SqlParameter("@Gender", (int)teaInfo.Gender));
            parametersList.Add(new SqlParameter("@Number", teaInfo.Number));
            parametersList.Add(new SqlParameter("@State", (int)teaInfo.State));
            parametersList.Add(new SqlParameter("@Category", (int)teaInfo.Category));
            parametersList.Add(new SqlParameter("@Province", teaInfo.Province));
            parametersList.Add(new SqlParameter("@City", teaInfo.City));
            parametersList.Add(new SqlParameter("@County", teaInfo.County));
            parametersList.Add(new SqlParameter("@Level", (int)teaInfo.Level));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除教师证书
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool DeleteTea(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete TeaCertificate where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }



        #endregion

        #region 学生证书
        /// <summary>
        /// 根据ID获取证书信息
        /// </summary>
        /// <param name="id">证书ID</param>
        /// <returns>证书信息</returns>
        public StuCertificate GetStu(int id)
        {
            StuCertificate stuInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT sc.Id, sc.UserId, sc.Name, sc.Gender, sc.HeadImg, sc.Number,u.Name as UserName, sc.State, sc.Category,  sc.Province, sc.City, sc.County, sc.CreateDate  from StuCertificate sc inner join  [User] u on sc.UserId=u.Id where sc.IsDeleted=0 and sc.id=@Id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    stuInfo = GetStuFromReader(reader);
                }
            }

            return stuInfo;
        }

        public StuCertificate GetStuByNumber(string number)
        {
            StuCertificate stuInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT sc.Id, sc.UserId, sc.Name, sc.Gender, sc.HeadImg, sc.Number,u.Name as UserName, sc.State, sc.Category,  sc.Province, sc.City, sc.County, sc.CreateDate  from StuCertificate sc inner join  [User] u on sc.UserId=u.Id where sc.IsDeleted=0 and sc.Number=@Number";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Number", number));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    stuInfo = GetStuFromReader(reader);
                }
            }

            return stuInfo;
        }

        /// <summary>
        /// 获取所有证书信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>证书集合</returns>
        public List<StuCertificate> GetStuAll(FilterEntityModel condition)
        {
            string whereSort = string.Empty;

            if (condition != null)
            {
                condition.DefaultSort = SortType.Desc;

                whereSort = condition.Where + condition.OrderBy;
            }

            List<StuCertificate> stuList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT sc.Id, sc.UserId, sc.Name, sc.Gender, sc.HeadImg, sc.Number,u.Name as UserName, sc.State, sc.Category,  sc.Province, sc.City, sc.County, sc.CreateDate  from StuCertificate sc inner join  [User] u on sc.UserId=u.Id where sc.IsDeleted=0 " + whereSort;

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
                stuList = new List<StuCertificate>();
                StuCertificate stuInfo = null;
                while (reader.Read())
                {
                    stuInfo = GetStuFromReader(reader);
                    stuList.Add(stuInfo);
                }
            }

            return stuList;
        }

        public List<StuCertificate> GetStuPage(FilterEntityModel condition, out int totalNum)
        {
            totalNum = 0;
            condition.DefaultSort = SortType.Desc;
            condition.SortTbAsName = Constant.STU_CERTIFICATE_AS_NAME;
            string sort = condition.OrderBy;
            string asSort = condition.AsOrderBy;
            string where = condition.Where;

            List<StuCertificate> stuList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select count(sc.ID) from [StuCertificate] sc inner join [User] u on sc.UserId=u.Id  where sc.IsDeleted=0 " + where;//查询有多少条记录
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
                                  ,[Gender]
                                  ,[HeadImg]
                                  ,[Number]
                                  ,[State]
                                  ,[Category]
                                  ,[Province]
                                  ,[City]
                                  ,[County]
                                  ,[CreateDate]
                                  ,[UserName]
                            FROM (select top (@pageSize*@pageIndex)  sc.[ID]
                                  ,sc.[UserId]
                                  ,sc.[Name]
                                  ,sc.[Gender]
                                  ,sc.[HeadImg]
                                  ,sc.[Number]
                                  ,sc.[State]
                                  ,sc.[Category]
                                  ,sc.[Province]
                                  ,sc.[City]
                                  ,sc.[County]
                                  ,sc.[CreateDate]
                                  ,u.[Name] as UserName
                                    ,ROW_NUMBER() over(" + asSort + ") as num  from [StuCertificate] sc inner join [User] u on sc.UserId=u.Id  where sc.IsDeleted=0 " + where + ") as t where num between (@pageIndex - 1) * @pageSize + 1  and @pageIndex*@pageSize " + sort;
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                stuList = new List<StuCertificate>();
                StuCertificate stuInfo = null;
                while (reader.Read())
                {
                    stuInfo = GetStuFromReader(reader);
                    stuList.Add(stuInfo);
                }
            }

            return stuList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private StuCertificate GetStuFromReader(DbDataReader reader)
        {
            StuCertificate stuInfo = new StuCertificate();
            stuInfo.Id = Convert.ToInt32(reader["Id"]);
            stuInfo.UserId = Convert.ToInt32(reader["UserId"]);
            stuInfo.Number = reader["Number"].ToString();
            stuInfo.Name = reader["Name"].ToString();
            stuInfo.Gender = (Gender)Convert.ToInt32(reader["Gender"]);
            stuInfo.State = (PublishState)reader["State"];
            stuInfo.Category = (CertificateType)reader["Category"];
            stuInfo.UserName = reader["UserName"].ToString();

            if (ReaderExists(reader, "HeadImg") && DBNull.Value != reader["HeadImg"])
            {
                stuInfo.HeadImg = reader["HeadImg"].ToString();
            }
            if (ReaderExists(reader, "Province") && DBNull.Value != reader["Province"])
            {
                stuInfo.Province = reader["Province"].ToString();
            }
            if (ReaderExists(reader, "City") && DBNull.Value != reader["City"])
            {
                stuInfo.City = reader["City"].ToString();
            }
            if (ReaderExists(reader, "County") && DBNull.Value != reader["County"])
            {
                stuInfo.County = reader["County"].ToString();
            }

            stuInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

            return stuInfo;
        }


        /// <summary>
        /// 添加学生证书
        /// </summary>
        /// <param name="stuInfo">学生证书信息</param>
        /// <returns>添加成功标识</returns>
        public bool AddStu(StuCertificate stuInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"if exists(select Id from StuCertificate where Number=@Number)
                                begin
                                    update StuCertificate set UserId=@UserId,Name=@Name,Gender=@Gender,HeadImg=@HeadImg,State=@State,Category=@Category,Province=@Province,City=@City,County=@County,CreateDate=getdate(),IsDeleted=0 where Number=@Number 
                                end
                                else
                                begin
                                Insert Into StuCertificate(UserId, Name, Gender, HeadImg, Number, State, Category,  Province, City, County) 
                                   Values(@UserId, @Name, @Gender, @HeadImg, @Number, @State, @Category,  @Province, @City, @County) 
                                end ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@UserId", stuInfo.UserId));
            parametersList.Add(new SqlParameter("@Name", stuInfo.Name));
            parametersList.Add(new SqlParameter("@Gender", (int)stuInfo.Gender));
            parametersList.Add(new SqlParameter("@HeadImg", stuInfo.HeadImg));
            parametersList.Add(new SqlParameter("@Number", stuInfo.Number));
            parametersList.Add(new SqlParameter("@State", (int)stuInfo.State));
            parametersList.Add(new SqlParameter("@Category", (int)stuInfo.Category));
            parametersList.Add(new SqlParameter("@Province", stuInfo.Province));
            parametersList.Add(new SqlParameter("@City", stuInfo.City));
            parametersList.Add(new SqlParameter("@County", stuInfo.County));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 添加学生证书
        /// </summary>
        /// <param name="stuInfo">学生证书信息</param>
        /// <returns>添加成功标识</returns>
        public bool AddStu(List<StuCertificate> stuInfos, out List<StuCertificate> failList)
        {
            failList = new List<StuCertificate>();
            bool success = false;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"if exists(select Id from StuCertificate where Number=@Number)
                                begin
                                    update StuCertificate set UserId=@UserId,Name=@Name,Gender=@Gender,HeadImg=@HeadImg,State=@State,Category=@Category,Province=@Province,City=@City,County=@County,CreateDate=getdate(),IsDeleted=0 where Number=@Number 
                                end
                                else
                                begin
                                Insert Into StuCertificate(UserId, Name, Gender, HeadImg, Number, State, Category,  Province, City, County) 
                                   Values(@UserId, @Name, @Gender, @HeadImg, @Number, @State, @Category,  @Province, @City, @County) 
                                end ";

            List<DbParameter> parametersList = null;

            foreach (StuCertificate item in stuInfos)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@UserId", item.UserId));
                parametersList.Add(new SqlParameter("@Name", item.Name));
                parametersList.Add(new SqlParameter("@Gender", (int)item.Gender));
                parametersList.Add(new SqlParameter("@HeadImg", item.HeadImg));
                parametersList.Add(new SqlParameter("@Number", item.Number));
                parametersList.Add(new SqlParameter("@State", (int)item.State));
                parametersList.Add(new SqlParameter("@Category", (int)item.Category));
                parametersList.Add(new SqlParameter("@Province", item.Province));
                parametersList.Add(new SqlParameter("@City", item.City));
                parametersList.Add(new SqlParameter("@County", item.County));
                success = dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;

                if (!success)
                {
                    failList.Add(item);
                }
            }

            return success && failList.Count == 0;
        }

        /// <summary>
        /// 修改学生证书
        /// </summary>
        /// <param name="stuInfo">学生证书信息</param>
        /// <returns>修改成功标识</returns>
        public bool UpdatStu(StuCertificate stuInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update StuCertificate 
                              Set [Name]=@Name
                                  ,[UserId]=@UserId 
                                  ,[HeadImg]=@HeadImg 
                                  ,[Gender]=@Gender
                                  ,[Number]=@Number
                                  ,[State]=@State
                                  ,[Category]=@Category
                                  ,[Province]=@Province
                                  ,[City]=@City
                                  ,[County]=@County
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", stuInfo.Id));
            parametersList.Add(new SqlParameter("@UserId", stuInfo.UserId));
            parametersList.Add(new SqlParameter("@Name", stuInfo.Name));
            parametersList.Add(new SqlParameter("@HeadImg", stuInfo.HeadImg));
            parametersList.Add(new SqlParameter("@Gender", (int)stuInfo.Gender));
            parametersList.Add(new SqlParameter("@Number", stuInfo.Number));
            parametersList.Add(new SqlParameter("@State", (int)stuInfo.State));
            parametersList.Add(new SqlParameter("@Category", (int)stuInfo.Category));
            parametersList.Add(new SqlParameter("@Province", stuInfo.Province));
            parametersList.Add(new SqlParameter("@City", stuInfo.City));
            parametersList.Add(new SqlParameter("@County", stuInfo.County));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除学生证书
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool DeleteStu(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete StuCertificate where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion
    }
}

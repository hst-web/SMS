using HST.Art.Core;
using HST.Utillity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HST.Art.Data
{
    public class MemberUnitProvider : EntityProviderBase
    {
        #region 查询会员单位
        /// <summary>
        /// 根据ID获取会员单位单位信息
        /// </summary>
        /// <param name="id">会员单位ID</param>
        /// <returns>会员单位信息</returns>
        public MemberUnit Get(int id)
        {
            MemberUnit MemberUnitInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT m.Id, m.Name, m.HeadImg, m.Star, m.Number, m.State, m.Category,cd.name as  CategoryName,Description, Province, City, County, m.CreateDate,m.Synopsis  from MemberUnit m left join CategoryDictionary cd  on  m.category=cd.id where m.IsDeleted=0 and m.id=@Id ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    MemberUnitInfo = GetMemberUnitFromReader(reader);
                }
            }

            return MemberUnitInfo;
        }

        /// <summary>
        /// 获取所有会员单位信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>会员集合</returns>
        public List<MemberUnit> GetAll(FilterEntityModel condition)
        {
            string whereSort = string.Empty;

            if (condition != null)
            {
                condition.DefaultSort = SortType.Desc;
                whereSort = condition.Where + condition.OrderBy;
            }

            List<MemberUnit> MemberUnitList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT m.Id, m.Name, m.HeadImg, m.Star, m.Number, m.State, m.Category,cd.name as  CategoryName, Province, City, County, m.CreateDate,m.Synopsis  from MemberUnit m left join CategoryDictionary cd on m.category=cd.id where m.IsDeleted=0 " + whereSort;

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
                MemberUnitList = new List<MemberUnit>();
                MemberUnit MemberUnitInfo = null;
                while (reader.Read())
                {
                    MemberUnitInfo = GetMemberUnitFromReader(reader);
                    MemberUnitList.Add(MemberUnitInfo);
                }
            }

            return MemberUnitList;
        }

        /// <summary>
        /// 获取会员单位分页信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <param name="totalNum">总条数</param>
        /// <returns>会员集合</returns>
        public List<MemberUnit> GetPage(FilterEntityModel condition, out int totalNum)
        {
            totalNum = 0;
            condition.DefaultSort = SortType.Desc;
            condition.SortTbAsName = Constant.MEMBER_UNIT_AS_NAME;
            string sort = condition.OrderBy;
            string asSort = condition.AsOrderBy;
            string where = condition.Where;

            List<MemberUnit> MemberUnitList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSqlQuery = @"select count(m.ID) from [MemberUnit] m left join CategoryDictionary cd on m.Category=cd.id  where  m.IsDeleted=0 " + where;//查询有多少条记录
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
                                  ,[Name]
                                  ,[HeadImg]
                                  ,[Star]
                                  ,[Number]
                                  ,[State]
                                  ,[Category]
                                  ,[CategoryName]
                                  ,[Province]
                                  ,[City]
                                  ,[County]
                                  ,[CreateDate]
                                  ,[Synopsis]
                            FROM (select top (@pageSize*@pageIndex)  m.[ID]
                                  ,m.[Name]
                                  ,m.[HeadImg]
                                  ,m.[Star]
                                  ,m.[Number]
                                  ,m.[State]
                                  ,m.[Category]
                                  ,cd.[Name] as CategoryName
                                  ,m.[Province]
                                  ,m.[City]
                                  ,m.[County]
                                  ,m.[CreateDate]
                                  ,m.[Synopsis]
                                    ,ROW_NUMBER() over(" + asSort + ") as num  from [MemberUnit] m  left join CategoryDictionary cd  on m.Category=cd.id  where  m.IsDeleted=0 " + where + ") as t where num between (@pageIndex - 1) * @pageSize + 1  and @pageIndex*@pageSize " + sort;
            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parameList))
            {
                MemberUnitList = new List<MemberUnit>();
                MemberUnit MemberUnitInfo = null;
                while (reader.Read())
                {
                    MemberUnitInfo = GetMemberUnitFromReader(reader);
                    MemberUnitList.Add(MemberUnitInfo);
                }
            }

            return MemberUnitList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private MemberUnit GetMemberUnitFromReader(DbDataReader reader)
        {
            MemberUnit MemberUnitInfo = new MemberUnit();
            MemberUnitInfo.Id = Convert.ToInt32(reader["Id"]);
            MemberUnitInfo.Name = reader["Name"].ToString();
            MemberUnitInfo.Star = Convert.ToInt32(reader["Star"]);
            MemberUnitInfo.Number = reader["Number"].ToString();
            MemberUnitInfo.State = Convert.ToInt32(reader["State"]) < 1 ? PublishState.Lower : PublishState.Upper;
            MemberUnitInfo.Category = Convert.ToInt32(reader["Category"]);
            MemberUnitInfo.Province = reader["Province"].ToString();
            MemberUnitInfo.City = reader["City"].ToString();
            MemberUnitInfo.County = reader["County"].ToString();
            MemberUnitInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

            if (ReaderExists(reader, "CategoryName") && DBNull.Value != reader["CategoryName"])
            {
                MemberUnitInfo.CategoryName = reader["CategoryName"].ToString();
            }
            if (ReaderExists(reader, "HeadImg") && DBNull.Value != reader["HeadImg"])
            {
                MemberUnitInfo.HeadImg = reader["HeadImg"].ToString();
            }
            if (ReaderExists(reader, "Description") && DBNull.Value != reader["Description"])
            {
                MemberUnitInfo.Description = reader["Description"].ToString();
            }
            if (ReaderExists(reader, "Synopsis") && DBNull.Value != reader["Synopsis"])
            {
                MemberUnitInfo.Synopsis = reader["Synopsis"].ToString();
            }

            return MemberUnitInfo;
        }

        #endregion

        #region 编辑会员单位

        /// <summary>
        /// 添加会员单位
        /// </summary>
        /// <param name="memberUnitInfo">会员单位信息</param>
        /// <returns>添加成功标识</returns>
        public bool Add(MemberUnit memberUnitInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"if exists(select Id from MemberUnit where Number=@Number)
                                begin
                                    update MemberUnit set Name=@Name,HeadImg=@HeadImg,Star=@Star,State=@State,Category=@Category,Description=@Description,Synopsis=@Synopsis,Province=@Province,City=@City,County=@County,UserId=@UserId,CreateDate=getdate(),IsDeleted=0 where Number=@Number 
                                end
                                else
                                begin
                                Insert Into MemberUnit(Name, HeadImg, Star, Number, State, Category, Description,Synopsis, Province, City, County,UserId) 
                                   Values(@Name, @HeadImg, @Star, @Number, @State, @Category, @Description,@Synopsis, @Province, @City, @County,@UserId) 
                                end ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Name", memberUnitInfo.Name));
            parametersList.Add(new SqlParameter("@HeadImg", memberUnitInfo.HeadImg));
            parametersList.Add(new SqlParameter("@Star", memberUnitInfo.Star));
            parametersList.Add(new SqlParameter("@Number", memberUnitInfo.Number));
            parametersList.Add(new SqlParameter("@State", (int)memberUnitInfo.State));
            parametersList.Add(new SqlParameter("@Category", memberUnitInfo.Category));
            parametersList.Add(new SqlParameter("@Description", memberUnitInfo.Description));
            parametersList.Add(new SqlParameter("@Synopsis", memberUnitInfo.Synopsis));
            parametersList.Add(new SqlParameter("@Province", memberUnitInfo.Province));
            parametersList.Add(new SqlParameter("@City", memberUnitInfo.City));
            parametersList.Add(new SqlParameter("@County", memberUnitInfo.County));
            parametersList.Add(new SqlParameter("@UserId", memberUnitInfo.UserId));
            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 添加会员单位
        /// </summary>
        /// <param name="MemberUnitInfo">会员单位信息</param>
        /// <returns>添加成功标识</returns>
        public bool Add(List<MemberUnit> memberUnitInfos, out List<MemberUnit> failList)
        {
            bool success = false;
            failList = new List<MemberUnit>();
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"if exists(select Id from MemberUnit where Number=@Number)
                                begin
                                    update MemberUnit set Name=@Name,Star=@Star,State=@State,Category=@Category,Description=@Description,Province=@Province,City=@City,County=@County,UserId=@UserId,CreateDate=getdate(),IsDeleted=0 where Number=@Number 
                                end
                                else
                                begin
                                Insert Into MemberUnit(Name, HeadImg, Star, Number, State, Category, Description, Province, City, County,UserId) 
                                   Values(@Name, @HeadImg, @Star, @Number, @State, @Category, @Description, @Province, @City, @County,@UserId) 
                                end ";

            List<DbParameter> parametersList = null;
            foreach (MemberUnit item in memberUnitInfos)
            {
                parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@Name", item.Name));
                parametersList.Add(new SqlParameter("@HeadImg", item.HeadImg));
                parametersList.Add(new SqlParameter("@Star", item.Star));
                parametersList.Add(new SqlParameter("@Number", item.Number));
                parametersList.Add(new SqlParameter("@State", (int)item.State));
                parametersList.Add(new SqlParameter("@Category", item.Category));
                parametersList.Add(new SqlParameter("@Description", item.Description));
                parametersList.Add(new SqlParameter("@Province", item.Province));
                parametersList.Add(new SqlParameter("@City", item.City));
                parametersList.Add(new SqlParameter("@County", item.County));
                parametersList.Add(new SqlParameter("@UserId", item.UserId));

                success = dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
                if (!success)
                {
                    failList.Add(item);
                }

            }

            return success && failList.Count == 0;
        }

        /// <summary>
        /// 修改会员单位
        /// </summary>
        /// <param name="memberUnitInfo">会员单位信息</param>
        /// <returns>修改成功标识</returns>
        public bool Update(MemberUnit memberUnitInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update MemberUnit
                              Set [Name]=@Name
                                  ,[HeadImg]=@HeadImg 
                                  ,[Star]=@Star
                                  ,[Number]=@Number
                                  ,[State]=@State
                                  ,[Category]=@Category
                                  ,[Description]=@Description
                                  ,[Synopsis]=@Synopsis
                                  ,[Province]=@Province
                                  ,[City]=@City
                                  ,[County]=@County
                                  ,[UserId]=@UserId
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", memberUnitInfo.Id));
            parametersList.Add(new SqlParameter("@Name", memberUnitInfo.Name));
            parametersList.Add(new SqlParameter("@HeadImg", memberUnitInfo.HeadImg));
            parametersList.Add(new SqlParameter("@Star", memberUnitInfo.Star));
            parametersList.Add(new SqlParameter("@Number", memberUnitInfo.Number));
            parametersList.Add(new SqlParameter("@State", (int)memberUnitInfo.State));
            parametersList.Add(new SqlParameter("@Category", memberUnitInfo.Category));
            parametersList.Add(new SqlParameter("@Description", memberUnitInfo.Description));
            parametersList.Add(new SqlParameter("@Province", memberUnitInfo.Province));
            parametersList.Add(new SqlParameter("@City", memberUnitInfo.City));
            parametersList.Add(new SqlParameter("@County", memberUnitInfo.County));
            parametersList.Add(new SqlParameter("@UserId", memberUnitInfo.UserId));
            parametersList.Add(new SqlParameter("@Synopsis", memberUnitInfo.Synopsis));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 删除会员单位
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete MemberUnit where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion
    }
}

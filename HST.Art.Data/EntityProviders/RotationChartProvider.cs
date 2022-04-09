/*----------------------------------------------------------------
// 文件名：RotationChartProvider.cs
// 功能描述： 轮播数据提供者
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
    public class RotationChartProvider : EntityProviderBase
    {
        #region 查询轮播
        /// <summary>
        /// 根据ID获取轮播信息
        /// </summary>
        /// <param name="id">轮播ID</param>
        /// <returns>轮播信息</returns>
        public RotationChart Get(int id)
        {
            RotationChart rotationInfo = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"SELECT Id, ImgSrc, WebLink, State, Type, CreateDate from RotationChart where IsDeleted=0 and id=@Id ";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@Id", id));

            using (DbDataReader reader = dbHelper.ExecuteReader(strSql, parametersList))
            {
                while (reader.Read())
                {
                    rotationInfo = GetRotationChartFromReader(reader);
                }
            }

            return rotationInfo;
        }

        /// <summary>
        /// 获取所有轮播信息
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns>轮播集合</returns>
        public List<RotationChart> GetAll(FilterEntityModel condition)
        {
            string whereSort = string.Empty;

            if (condition != null)
            {
                condition.DefaultSort = SortType.Desc;
                whereSort = condition.Where + condition.OrderBy;
            }

            List<RotationChart> rotationList = null;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);

            string strSql = @"SELECT Id, ImgSrc, WebLink, State, Type, CreateDate from RotationChart where IsDeleted=0 " + whereSort;

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
                rotationList = new List<RotationChart>();
                RotationChart rotationInfo = null;
                while (reader.Read())
                {
                    rotationInfo = GetRotationChartFromReader(reader);
                    rotationList.Add(rotationInfo);
                }
            }

            return rotationList;
        }

        /// <summary>
        /// 从游标中读取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private RotationChart GetRotationChartFromReader(DbDataReader reader)
        {
            RotationChart rotationInfo = new RotationChart();
            rotationInfo.Id = Convert.ToInt32(reader["Id"]);
            rotationInfo.WebLink = reader["WebLink"].ToString();
            rotationInfo.Type = (RotationType)reader["Type"];
            rotationInfo.ImgSrc = reader["ImgSrc"].ToString();
            rotationInfo.State = Convert.ToInt32(reader["State"]) < 1 ? PublishState.Lower : PublishState.Upper;
            rotationInfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

            return rotationInfo;
        }
        #endregion

        #region 编辑轮播
        /// <summary>
        /// 添加轮播
        /// </summary>
        /// <param name="rotationInfo">轮播信息</param>
        /// <returns>添加成功标识</returns>
        public bool Add(RotationChart rotationInfo)
        {
            bool isSuccess = false;
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Insert Into RotationChart (ImgSrc, WebLink, State, Type) Values (@ImgSrc, @WebLink, @State, @Type);select SCOPE_IDENTITY()";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ImgSrc", rotationInfo.ImgSrc));
            parametersList.Add(new SqlParameter("@WebLink", rotationInfo.WebLink));
            parametersList.Add(new SqlParameter("@State", (int)rotationInfo.State));
            parametersList.Add(new SqlParameter("@Type", (int)rotationInfo.Type));

            int iResultInsert = Convert.ToInt32(dbHelper.ExecuteScalar(strSql, parametersList));

            if (iResultInsert > 0)
            {
                //若成功，则记录成功信息：患者信息，发卡时间，索引信息
                rotationInfo.Id = iResultInsert;
                isSuccess = true;
            }

            return isSuccess;
        }

        /// <summary>
        /// 修改轮播
        /// </summary>
        /// <param name="rotationInfo">轮播信息</param>
        /// <returns>修改成功标识</returns>
        public bool Update(RotationChart rotationInfo)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = @"Update RotationChart
                              Set [ImgSrc]=@ImgSrc
                                  ,[WebLink]=@WebLink
                                  ,[State]=@State
                                  ,[Type]=@Type
                                  Where ID=@ID";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@ID", rotationInfo.Id));
            parametersList.Add(new SqlParameter("@ImgSrc", rotationInfo.ImgSrc));
            parametersList.Add(new SqlParameter("@WebLink", rotationInfo.WebLink));
            parametersList.Add(new SqlParameter("@State", (int)rotationInfo.State));
            parametersList.Add(new SqlParameter("@Type", (int)rotationInfo.Type));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }

        /// <summary>
        /// 修改轮播
        /// </summary>
        /// <param name="rotations">轮播集合</param>
        /// <returns>修改成功标识</returns>
        public bool Update(List<RotationChart> rotations)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string ids = string.Join(",", rotations.Select(g => g.Id));
            string sqlDel = "delete RotationChart where id in ( " + ids + " )";
            string sqlAdd = "Insert Into RotationChart (ImgSrc, WebLink, State, Type) Values (@@ImgSrc, @WebLink, @State, @Type)";

            dbHelper.BeginTrans();//开启事务
            bool isSuccess = dbHelper.ExecuteNonQueryInTrans(sqlDel, null) > 0;

            if (!isSuccess)
            {
                dbHelper.RollBack();
                return false;
            }

            foreach (RotationChart item in rotations)
            {
                List<DbParameter> parametersList = new List<DbParameter>();
                parametersList.Add(new SqlParameter("@ImgSrc", item.ImgSrc));
                parametersList.Add(new SqlParameter("@WebLink", item.WebLink));
                parametersList.Add(new SqlParameter("@State", (int)item.State));
                parametersList.Add(new SqlParameter("@Type", (int)item.Type));
                isSuccess = dbHelper.ExecuteNonQueryInTrans(sqlAdd, parametersList) > 0;

                if (!isSuccess)
                {
                    dbHelper.RollBack();
                    return false;
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

        /// <summary>
        /// 删除轮播
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            DBHelper dbHelper = new DBHelper(ConnectionString, DbProviderType.SqlServer);
            string strSql = "delete RotationChart where id =@id";

            List<DbParameter> parametersList = new List<DbParameter>();
            parametersList.Add(new SqlParameter("@id", id));

            return dbHelper.ExecuteNonQuery(strSql, parametersList) > 0;
        }
        #endregion
    }
}

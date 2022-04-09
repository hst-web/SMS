/*----------------------------------------------------------------

// 文件名：MathHelper.cs
// 功能描述：数据计算辅助操作类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;


namespace HST.Utillity
{
    /// <summary>
    /// 数据计算辅助操作类
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// 获取两个坐标的距离
        /// </summary>
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }
    }
}
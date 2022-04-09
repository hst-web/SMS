/*----------------------------------------------------------------
// 文件名：ModelBase.cs
// 功能描述：业务模型基类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;

namespace HST.Art.Core
{
    /// <summary>
    /// 业务模型基类
    /// </summary>
    public class ModelBase : IDisposable
    {
        /// <summary>
        /// 模型主键ID
        /// </summary>
        public virtual string ID { get; set; }
        
        /// <summary>
        /// 判断两个实体是否是同一数据记录的实体
        /// </summary>
        /// <param name="obj">要比较的实体信息</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            ModelBase entity = obj as ModelBase;

            if (entity == null)
            {
                return false;
            }
            return ID.Equals(entity.ID);
        }
        
        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>当前 的哈希代码</returns>
        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(ID))
            {
                return 0;
            }
            return ID.GetHashCode();
        }
        
        /// <summary>
        /// 实现IDisposable接口
        /// </summary>
        public void Dispose() { }
    }
}

/*----------------------------------------------------------------
// 文件名：Role.cs
// 功能描述：角色信息
// 创建者：sysmenu
// 创建时间：2019-4-14
//----------------------------------------------------------------*/
using System;
namespace HST.Art.Core
{
	/// <summary>
	/// Role
	/// </summary>
	[Serializable]
	public partial class Role
	{
		public Role()
		{}
		#region Model
		private int _id;
		private string _name;
		private string _description;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		#endregion Model

	}
}


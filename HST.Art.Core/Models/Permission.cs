﻿/*----------------------------------------------------------------
// 文件名：Permission.cs
// 功能描述：权限信息
// 创建者：sysmenu
// 创建时间：2019-4-14
//----------------------------------------------------------------*/
using System;
namespace ZT.SMS.Core
{
	/// <summary>
	/// Permission
	/// </summary>
	[Serializable]
	public partial class Permission
	{
		public Permission()
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


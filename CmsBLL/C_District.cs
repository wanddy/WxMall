﻿/**  版本信息模板在安装目录下，可自行修改。
* C_District.cs
*
* 功 能： N/A
* 类 名： C_District
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/2/28 14:12:43   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using Cms.Common;
using Cms.Model;
namespace Cms.BLL
{
	/// <summary>
	/// C_District
	/// </summary>
	public partial class C_District
	{
        private readonly SQLServerDAL.C_District dal=new SQLServerDAL.C_District();
		public C_District()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long DistrictID)
		{
			return dal.Exists(DistrictID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(Cms.Model.C_District model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Cms.Model.C_District model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(long DistrictID)
		{
			
			return dal.Delete(DistrictID);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string DistrictIDlist )
		{
			return dal.DeleteList(DistrictIDlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Cms.Model.C_District GetModel(long DistrictID)
		{
			
			return dal.GetModel(DistrictID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public Cms.Model.C_District GetModelByCache(long DistrictID)
		{
			
			string CacheKey = "C_DistrictModel-" + DistrictID;
            object objModel = Cms.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(DistrictID);
					if (objModel != null)
					{
                        int ModelCache = Cms.Common.ConfigHelper.GetConfigInt("ModelCache");
                        Cms.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (Cms.Model.C_District)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Cms.Model.C_District> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Cms.Model.C_District> DataTableToList(DataTable dt)
		{
			List<Cms.Model.C_District> modelList = new List<Cms.Model.C_District>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Cms.Model.C_District model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

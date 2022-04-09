using HST.Art.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using HST.Art.Service;
using Newtonsoft.Json;

namespace HST.Art.Web.Areas.manage.Controllers
{
    public class CategoryController : ApplicationBase
    {
        CategoryDictionaryService cdService = new CategoryDictionaryService();

        // GET: Home
        public ActionResult Index()
        {
            InitData();
            ViewBag.DefaultCategory = CategoryType.Industry;
            return View();
        }

        /// <summary>
        /// List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetJsonData(SearchDataTable dt, FilterViewModel filter)
        {
            int totalNum = 0;
            List<CategoryDictionary> categoryList = cdService.GetAll(filter.CategoryType);
            totalNum = categoryList != null ? categoryList.Count : 0;

            if (filter.CategoryType == CategoryType.Examination && categoryList != null && categoryList.Count > 0)
            {
                if (filter.IsParent)
                {
                    categoryList.RemoveAll(g => g.Parent > 0);
                }
                else
                {
                    categoryList.RemoveAll(g => g.Parent == 0);
                }

            }

            ReturnPageResultIList<CategoryDictionary> data = new ReturnPageResultIList<Core.CategoryDictionary>(categoryList, totalNum);
            IList<CategoryViewModel> gmList = new List<CategoryViewModel>();

            if (data != null && data.DataT != null)
                gmList = data.DataT.Select(g => new CategoryViewModel() { Id = g.Id, CategoryName = g.Name, CategoryType = g.Type, ParentId = g.Parent, ParentName = g.ParentName, State = (int)g.State, UserName = g.UserName, CreateTime = g.CreateDate.ToString("yyyy-MM-dd HH:mm") }).ToList();

            return Json(new
            {
                recordsFiltered = data.totalRecords,
                recordsTotal = data.totalPages,
                data = gmList
            }, JsonRequestBehavior.AllowGet);

        }
        #region 新增

        [HttpPost]
        public JsonResult Add(CategoryViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();

            rmodel = CheckName(model.Id, model.CategoryName, model.CategoryType);
            if (!rmodel.isSuccess)
            {
                return Json(rmodel);
            }

            CategoryDictionary downModel = new CategoryDictionary()
            {
                Name = model.CategoryName,
                Parent = model.ParentId,
                State = PublishState.Upper,
                Type = model.CategoryType,
                UserId = GetAccount().Id
            };

            bool isSuccess = cdService.Add(downModel);
            if (!isSuccess)
            {
                rmodel.message = "操作失败";
            }
            rmodel.isSuccess = isSuccess;

            return Json(rmodel);
        }
        #endregion

        #region 修改
        [HttpPost]
        public JsonResult Edit(CategoryViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            rmodel = CheckName(model.Id, model.CategoryName, model.CategoryType);
            if (!rmodel.isSuccess)
            {
                return Json(rmodel);
            }

            CategoryDictionary data = cdService.Get(model.Id);
            data.Name = model.CategoryName;
            data.Type = model.CategoryType;
            data.Parent = model.ParentId;

            bool isSuccess = cdService.Update(data);
            if (!isSuccess)
            {
                rmodel.message = "操作失败";
            }
            rmodel.isSuccess = isSuccess;

            return Json(rmodel);
        }

        #endregion

        #region 状态操作
        public override JsonResult Delete(int id)
        {
            ResultRetrun rmodel = new ResultRetrun();
            try
            {
                rmodel.isSuccess = cdService.LogicDelete(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "操作失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }

        public override JsonResult Publish(int id)
        {
            ResultRetrun rmodel = new ResultRetrun();
            try
            {
                rmodel.isSuccess = cdService.Publish(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "操作失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }

        public override JsonResult Shelves(int id)
        {
            ResultRetrun rmodel = new ResultRetrun();
            try
            {
                rmodel.isSuccess = cdService.Recovery(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "操作失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult GetParentCategory()
        {
            List<CategoryDictionary> cdAllList = cdService.GetAll(CategoryType.Examination);
            List<CategoryDictionary> cdEnabledList = new List<CategoryDictionary>();
            if (cdAllList != null && cdAllList.Count > 0)
            {
                cdAllList.RemoveAll(g => g.Parent > 0);
                cdEnabledList = cdAllList.FindAll(g => g.State == PublishState.Upper);
            }

            return Json(new { allList = cdAllList, enabledList = cdEnabledList }, JsonRequestBehavior.AllowGet);
        }

        private void InitData()
        {
            List<CategoryDictionary> cdAllList = cdService.GetAll(CategoryType.Examination);
            List<CategoryDictionary> cdEnabledList = new List<CategoryDictionary>();
            if (cdAllList != null && cdAllList.Count > 0)
            {
                cdAllList.RemoveAll(g => g.Parent > 0);
                cdEnabledList = cdAllList.FindAll(g => g.State == PublishState.Upper);
            }

            ViewBag.AllCategory = cdAllList == null ? new List<CategoryDictionary>() : cdAllList;
            ViewBag.EnabledCategory = cdEnabledList;
        }

        private ResultRetrun CheckName(int id, string categoryName, CategoryType categoryType)
        {
            ResultRetrun rmodel = new ResultRetrun();
            FilterEntityModel filterModel = new FilterEntityModel();
            filterModel.KeyValueList = new List<KeyValueObj>();
            filterModel.KeyValueList.Add(new KeyValueObj() { Key = "Name", Value = categoryName, FieldType = FieldType.String });

            List<CategoryDictionary> cdList = cdService.GetAll(categoryType);
            if (cdList != null && cdList.Count > 0 && cdList.Where(g => g.Name.Equals(categoryName)).Count() > 0)
            {
                if (cdList.FindAll(g => g.Name.Equals(categoryName)).Where(g => !g.Id.Equals(id)).Count() > 0)
                    rmodel.message = "类别名称已经存在";
                else
                    rmodel.isSuccess = true;
            }
            else
                rmodel.isSuccess = true;
            return rmodel;
        }
    }
}

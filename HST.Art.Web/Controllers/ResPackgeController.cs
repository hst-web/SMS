using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HST.Art.Web.Controllers
{
    public class ResPackgeController : ApplicationBase
    {

        // GET: ResPage
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// DataTable读取数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name"></param>
        /// <param name="state"></param>
        /// <param name="upgradetime"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJsonData(SreachDataTable dt, string name, string state, string upgradetime)
        {
            FilterBaseModel fillter = new FilterBaseModel()
            {
                Name = name,
                pageNumber = dt.pageIndex,
                pageSize = dt.length
            };

            if (!string.IsNullOrEmpty(state))
            {
                fillter.State = Convert.ToInt16(state);
            }

            if (!string.IsNullOrEmpty(upgradetime))
            {
                fillter.StartTime = Convert.ToDateTime(upgradetime);
                fillter.EndTime = new DateTime(fillter.StartTime.Year, fillter.StartTime.Month, fillter.StartTime.Day, 23, 59, 59);
            }

            ReturnPageResultIList<ResourcePackage> data = new ResourcePackageController().GetAllWhere(fillter);
            IList<ResourcePackage> gmList = new List<ResourcePackage>();

            if (data != null && data.DataT != null)
                gmList = data.DataT;

            return Json(new
            {
                recordsFiltered = data.totalRecords,
                recordsTotal = data.totalPages,
                data = gmList
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id <= 0) return null;

            Dictionary<int, string> dicArray = new Dictionary<int, string>();
            dicArray.Add(0, "禁用");
            dicArray.Add(1, "启用");
            ViewBag.select = dicArray;

            ResourcePackage model = new ResourcePackageController().Get(id);
            ResourcePackageViewModel viewModel = new ResourcePackageViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                State = model.State ? 1 : 0,
                Sort = model.Sort.Value
            };

            return View(viewModel);
        }

        [HttpPost]
        public string Edit(ResourcePackageViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResourcePackage rpackge = new ResourcePackage()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Sort = model.Sort,
                    State = model.State > 0
                };
                bool isSucessed = new ResourcePackageController().Update(rpackge);

                return isSucessed ? "ok" : "error";
            }

            return "error";
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View(new ResourcePackageViewModel());
        }

        [HttpPost]
        public string Add(ResourcePackageViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResourcePackage rpackge = new ResourcePackage()
                {
                    Name = model.Name,
                    Sort = model.Sort,
                    State = model.State > 0
                };
                bool isSucessed = new ResourcePackageController().Add(rpackge);

                return isSucessed ? "ok" : "error";
            }

            return "error";
        }

        [HttpPost]
        public string Delete(int id)
        {
            if (id <= 0) return "error";
            List<Resource> resourceList = new cncbk_resource_application.Controller.ResourceController().GetByPackgeId(id);
            if (resourceList != null && resourceList.Count > 0)
            {
                return "该资料包下还有资料，请先移除或变更该包中的资料后，再操作！";
            }
            bool isSucessed = new ResourcePackageController().Delete(id);

            return isSucessed ? "ok" : "error";

        }

        /// <summary>
        /// 检查包名是否存在
        /// </summary>
        /// <param name="name">包名</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CheckPackgeName(string name, int id)
        {
            List<ResourcePackage> resPackgeList = new ResourcePackageController().GetByName(name);

            if (resPackgeList != null && resPackgeList.Count > 0)
            {
                if (resPackgeList.Where(g => g.Id == id).Count() <= 0)
                {
                    return Json("资料包名称已存在", JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
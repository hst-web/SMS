using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace HST.Art.Web.Controllers
{
    public class ResRoleController : ApplicationBase
    {
        #region 列表
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
        public JsonResult GetJsonData(SreachDataTable dt, string name, string stime)
        {
            ReturnPageResultIList<ResourceRole> data = new cncbk_resource_application.Controller.ResourceRoleController().GetAllWhere(
              new FilterBaseModel()
              {
                  Name = name,
                  StartTime = string.IsNullOrEmpty(stime) ? Convert.ToDateTime("0001-01-01") : Convert.ToDateTime(stime),
                  EndTime = string.IsNullOrEmpty(stime) ? Convert.ToDateTime("0001-01-01") : new DateTime(Convert.ToDateTime(stime).Year, Convert.ToDateTime(stime).Month, Convert.ToDateTime(stime).Day, 23, 59, 59),
                  pageNumber = dt.pageIndex,
                  pageSize = dt.length
              }
            );
            IList<ResourceRole> rList = new List<ResourceRole>();
            if (data != null && data.DataT != null)
                rList = data.DataT;
            return Json(new
            {
                recordsFiltered = data.totalRecords,
                recordsTotal = data.totalPages,
                data = rList.Select(d=>new { Id = d.Id, RoleType=d.RoleType, RoleName=d.RoleName,RoleCtime=d.RoleCtime })
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 新增
        public ActionResult Add()
        {
            List<ResourceMenu> data = new cncbk_resource_application.Controller.ResourceMenuController().GetByFilter(new FilterBaseModel());
            ViewBag.select = JsonConvert.SerializeObject(data);
            return View(new ResRoleViewModel());
        }
        [HttpPost]
        public JsonResult Add(ResRoleViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                List<ResourceRoleMenu> mulist = new List<ResourceRoleMenu>();
                string[] slist = Request["rolelist"].ToString().Split(',');
                foreach (string item in slist)
                    mulist.Add(new ResourceRoleMenu() { Menuid = Convert.ToInt32(item) });
                ResourceRole ResModel = new ResourceRole()
                {
                    RoleName = model.RoleName,
                    RmList = mulist
                };
                rmodel.isSuccess = new cncbk_resource_application.Controller.ResourceRoleController().Add(ResModel) > 0;
            }
            return Json(rmodel);
        }
        #endregion

        #region 修改
        public ActionResult Update(int id)
        {
            ResourceRole data = new cncbk_resource_application.Controller.ResourceRoleController().GetModelById(id);
            if (data != null)
            {
                List<ResourceMenu> dataObj = new cncbk_resource_application.Controller.ResourceMenuController().GetByFilter(new FilterBaseModel());
                ViewBag.select = JsonConvert.SerializeObject(dataObj);
                return View(new ResRoleViewModel
                {
                    RoleName = data.RoleName,
                    Id = data.Id,
                    RoleCtime = data.RoleCtime,
                    menuList = data.RmList.Select(a => a.Menuid.ToString()).ToList()
                });
            }
            else
                return View();
        }
        [HttpPost]
        public JsonResult Update(ResRoleViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            List<ResourceRoleMenu> mulist = new List<ResourceRoleMenu>();
            string[] slist = Request["rolelist"].ToString().Split(',');
            foreach (string item in slist)
                mulist.Add(new ResourceRoleMenu() { Menuid = Convert.ToInt32(item) });

            if (ModelState.IsValid)
            {
                ResourceRole ResModel = new ResourceRole()
                {
                    Id = model.Id,
                    RoleName = model.RoleName,
                    RmList = mulist
                };
                rmodel.isSuccess = new cncbk_resource_application.Controller.ResourceRoleController().Update(ResModel);
            }
            return Json(rmodel);
        }
        #endregion

        #region 删除
        public JsonResult Delete(int id)
        {
            ResultRetrun rmodel = new ResultRetrun();
            try
            {
                rmodel.isSuccess = new cncbk_resource_application.Controller.ResourceRoleController().Delete(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "删除失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 检查角色名是否存在
        /// </summary>
        /// <param name="name">角色名</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CheckRoleName(string roleName, int id)
        {
            List<ResourceRole> resRoleList = new cncbk_resource_application.Controller.ResourceRoleController().GetByName(roleName);

            if (resRoleList != null && resRoleList.Count > 0)
            {
                if (resRoleList.Where(g => g.Id == id).Count() <= 0)
                {
                    return Json("角色名称已存在", JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
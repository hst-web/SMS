using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace HST.Art.Web.Controllers
{
    public class ResAdminController : ApplicationBase
    {
        #region 列表
        public ActionResult List()
        {
            List<ResourceRole> dataRole = new cncbk_resource_application.Controller.ResourceRoleController().GetByFilter(new FilterBaseModel());
            ViewBag.state = JsonConvert.SerializeObject(dataRole.Select(d => new { RoleName = d.RoleName, RoleCtime = d.RoleCtime, Id = d.Id }));
            ViewBag.IsAdmin = GetAccount().IsSuperAdmin;
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
        public JsonResult GetJsonData(SreachDataTable dt, string name, int state, string stime, string etime)
        {
            ReturnPageResultIList<ResourceMember> data = new cncbk_resource_application.Controller.ResourceMemberController().GetAllWhere(
              new ResourceMemberFilltersModel()
              {
                  userName = name,
                  StartTime = string.IsNullOrEmpty(stime) ? Convert.ToDateTime("0001-01-01") : Convert.ToDateTime(stime),
                  EndTime = string.IsNullOrEmpty(stime) ? Convert.ToDateTime("0001-01-01") : new DateTime(Convert.ToDateTime(stime).Year, Convert.ToDateTime(stime).Month, Convert.ToDateTime(stime).Day, 23, 59, 59),
                  pageNumber = dt.pageIndex,
                  pageSize = dt.length,
                  State = state
              }
            );
            IList<ResourceMember> memberList = new List<ResourceMember>();
            if (data != null && data.DataT != null)
                memberList = data.DataT;


            return Json(new
            {
                recordsFiltered = data.totalRecords,
                recordsTotal = data.totalPages,
                data = memberList.Select(a => new { Id = a.Id, UserName = a.UserName,Phone = a.Phone, Email = a.Email,State = a.State, RoleId = a.RoleId, Remark = a.Remark, CreateTime = a.CreateTime, IsSupAdmin=a.ResRole.RoleType==1 })
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Update(int id)
        {
            ResourceMember data = new cncbk_resource_application.Controller.ResourceMemberController().GetModelById(id);
            List<ResourceRole> dataRole = new cncbk_resource_application.Controller.ResourceRoleController().GetByFilter(new FilterBaseModel());
            dataRole = dataRole.Where(g => g.RoleType != 1).ToList();
            ViewBag.IsSupAdmin = GetAccount().IsSuperAdmin;
            if (data != null)
                return View(new ResAdminViewModel
                {
                    Id=data.Id,
                    Remark=data.Remark,
                    UserName = data.UserName,
                    Phone = data.Phone,
                    Email = data.Email,
                    RoleId = data.RoleId > 0 ? data.RoleId.ToString() : "",
                    State = data.State ? 0 : 1,
                    RoleList = (dataRole != null ? dataRole : null),
                    IsSupAdmin = data.ResRole.RoleType == 1
                });
            else
                return View();
        }
        [HttpPost]
        public JsonResult Update(ResAdminViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            ResourceMember ResModel = new ResourceMember()
            {
                Id = model.Id,
                Email = model.Email,
                RoleId = Convert.ToInt16(model.RoleId),
                Phone = model.Phone,
                Remark = model.Remark,
                State = model.State == 0,
                UserName = model.UserName
            };
            rmodel.isSuccess = new cncbk_resource_application.Controller.ResourceMemberController().Update(ResModel);
            return Json(rmodel);
        }

        /// <summary>
        /// 初始化密码
        /// </summary>
        /// <param name="uid">id</param>
        /// <returns></returns>
        [HttpPost]
        public string InitPwd(int uid)
        {
            bool isSuccess = new cncbk_resource_application.Controller.ResourceMemberController().UpdatePwd(uid);
            return isSuccess ? "ok" : "error";
        }
        #endregion

        #region 新增
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Add()
        {
            List<ResourceRole> dataRole = new cncbk_resource_application.Controller.ResourceRoleController().GetByFilter(new FilterBaseModel());
            dataRole = dataRole.Where(g => g.RoleType != 1).ToList();
            return View(new ResAdminViewModel() { RoleList = (dataRole != null ? dataRole : null) });
        }
        [HttpPost]
        public JsonResult Add(ResAdminViewModel model)
        {
            model.SaltPassword = new Random().Next(1, 10);
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                ResourceMember ResModel = new ResourceMember()
                {
                    Id = model.Id,
                    Email = model.Email,
                    RoleId = Convert.ToInt16(model.RoleId),
                    SaltPassword = model.SaltPassword,
                    Password = CNCBK.Common.Tools.encryption.Md5Hash(model.Password + model.SaltPassword),
                    Phone = model.Phone,
                    Remark = model.Remark,
                    State = model.State == 0,
                    UserName = model.UserName
                };
                rmodel.isSuccess = new cncbk_resource_application.Controller.ResourceMemberController().Add(ResModel) > 0;
            }
            if (!rmodel.isSuccess)
                rmodel.message = ModelState.Values.First().ToString();
            return Json(rmodel);
        }
        #endregion

        #region 删除
        public JsonResult Delete(int id)
        {
            ResultRetrun rmodel = new ResultRetrun();
            try
            {
                rmodel.isSuccess = new cncbk_resource_application.Controller.ResourceMemberController().Delete(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "删除失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 验证方法
        [HttpGet]
        public JsonResult CheckUserName(int id, string username)
        {
            return CheckIsExist(id, username, 0);
        }
        [HttpGet]
        public JsonResult CheckPhone(int id, string phone)
        {
            return CheckIsExist(id, phone, 1);
        }

        /// <summary>
        /// 检查资料名是否存在
        /// </summary>
        /// <param name="value">名称</param>
        /// <param name="type">id</param>
        /// <returns></returns>
        private JsonResult CheckIsExist(int id, string value, int type)
        {
            ResultRetrun rmodel = new ResultRetrun();
            ResourceMemberFilltersModel filter = null;
            string nameTemp = string.Empty;
            switch (type)
            {
                case 0://用户名
                    filter = new ResourceMemberFilltersModel() { userName = value, State = -1, StartTime = Convert.ToDateTime("0001-01-01") };
                    nameTemp = "用户名";
                    break;
                case 1://手机号
                    filter = new ResourceMemberFilltersModel() { phone = value, State = -1, StartTime = Convert.ToDateTime("0001-01-01") };
                    nameTemp = "手机号";
                    break;
            }
            List<ResourceMember> resList = new cncbk_resource_application.Controller.ResourceMemberController().GetByFilter(filter);
            if (resList != null && resList.Count > 0)
            {
                if (resList.Where(g => g.Id == id).Count() <= 0)
                    rmodel.message = nameTemp + "已经存在";
                else
                    rmodel.isSuccess = true;
            }
            else
                rmodel.isSuccess = true;
            return Json(rmodel.isSuccess, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
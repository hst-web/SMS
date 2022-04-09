using HST.Art.Core;
using HST.Art.Service;
using System;
using System.Web.Mvc;
using HST.Utillity;
using System.Collections.Generic;
using System.Linq;

namespace HST.Art.Web.Areas.manage.Controllers
{
    public class UserController : ApplicationBase
    {
        UserService uService = new UserService();

        public ActionResult List()
        {
            Account accouont = GetAccount();
            ViewBag.IsSupAdmin = accouont.IsAdmin;
            ViewBag.UserId = accouont.Id;
            return View();
        }

        [HttpPost]
        public ActionResult GetJsonData(SearchDataTable dt, SearchViewModel svm)
        {
            int totalNum = 0;
            FilterEntityModel fillter = new FilterEntityModel();
            fillter.PageIndex = dt.pageIndex;
            fillter.PageSize = dt.length;
            fillter.KeyValueList = new List<KeyValueObj>();
            if (svm != null && !string.IsNullOrEmpty(svm.FilterKey) && !string.IsNullOrEmpty(svm.FilterVal))
            {
                string fkey = string.Empty;
                SearchType ftype = (SearchType)Convert.ToInt16(svm.FilterKey);

                switch (ftype)
                {
                    case SearchType.Name:
                        fkey = "Name";
                        fillter.FilterType = FilterType.Like;
                        break;
                    case SearchType.State:
                        fkey = "State";
                        break;
                    case SearchType.Number:
                        fkey = "Telephone";
                        break;
                }

                fillter.KeyValueList.Add(new KeyValueObj() { Key = fkey, Value = svm.FilterVal });
            }

            List<User> userList = uService.GetPage(fillter, out totalNum);
            ReturnPageResultIList<User> data = new ReturnPageResultIList<Core.User>(userList, totalNum);
            IList<UserViewModel> gmList = new List<UserViewModel>();

            if (data != null && data.DataT != null)
                gmList = data.DataT.Select(g => new UserViewModel() { Id = g.Id, RealName = g.Name, UserName = g.UserName, Phone = g.Telephone, State = (int)g.State, CreateTime = g.CreateDate.ToString("yyyy-MM-dd HH:mm"), Email = g.Email, IsSupAdmin = g.IsAdmin }).ToList();

            return Json(new
            {
                recordsFiltered = data.totalRecords,
                recordsTotal = data.totalPages,
                data = gmList
            }, JsonRequestBehavior.AllowGet);

        }


        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            User data = uService.Get(id);
            Account account = GetAccount();
            ViewBag.IsSupAdmin = account.IsAdmin;
            ViewBag.IsSelf = id == account.Id;

            if (data != null)
                return View(new UserViewModel
                {
                    Id = data.Id,
                    UserName = data.UserName,
                    Phone = data.Telephone,
                    Email = data.Email,
                    State = (int)data.State,
                    RealName = data.Name,
                    IsSupAdmin = data.IsAdmin,
                    Password = Constant.INIT_MARKET_PASSWORD
                });
            else
                return View();
        }

        [HttpPost]
        public JsonResult Edit(UserViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                User data = uService.Get(model.Id);
                data.Name = model.RealName;
                data.Telephone = model.Phone;
                data.Email = model.Email;
                data.State = (PublishState)model.State;

                rmodel.isSuccess = uService.Update(data);
            }

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
            bool isSuccess = uService.UpdatePassword(uid);
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
            return View();
        }
        [HttpPost]
        public JsonResult Add(UserViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                User userModel = new User()
                {
                    Email = model.Email,
                    Name = model.RealName,
                    Password = model.Password,
                    Telephone = model.Phone,
                    State = (PublishState)model.State,
                    UserName = model.UserName
                };
                rmodel.isSuccess = uService.Add(userModel);
            }

            return Json(rmodel);
        }
        #endregion

        #region 状态操作
        public override JsonResult Delete(int id)
        {
            ResultRetrun rmodel = new ResultRetrun();
            try
            {
                rmodel.isSuccess = uService.LogicDelete(id);
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
                rmodel.isSuccess = uService.Publish(id);
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
                rmodel.isSuccess = uService.Recovery(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "操作失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 验证方法
        [HttpGet]
        public JsonResult CheckUserName(int id, string username)
        {
            return CheckIsExist(id, username, LoginType.UserName);
        }
        [HttpGet]
        public JsonResult CheckPhone(int id, string phone)
        {
            return CheckIsExist(id, phone, LoginType.Telephone);
        }

        [HttpGet]
        public JsonResult CheckEmail(int id, string email)
        {
            return CheckIsExist(id, email, LoginType.Email);
        }

        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="value">名称</param>
        /// <param name="type">id</param>
        /// <returns></returns>
        private JsonResult CheckIsExist(int id, string value, LoginType loginType)
        {
            ResultRetrun rmodel = new ResultRetrun();
            string nameTemp = loginType.GetDescription();
            FilterEntityModel filterModel = new FilterEntityModel();
            filterModel.KeyValueList = new List<KeyValueObj>();

            switch (loginType)
            {
                case LoginType.UserName:
                    filterModel.KeyValueList.Add(new KeyValueObj() { Key = "UserName", Value = value, FieldType = FieldType.String });
                    break;
                case LoginType.Telephone:
                    filterModel.KeyValueList.Add(new KeyValueObj() { Key = "Telephone", Value = value, FieldType = FieldType.String });
                    break;
                case LoginType.Email:
                    filterModel.KeyValueList.Add(new KeyValueObj() { Key = "Email", Value = value, FieldType = FieldType.String });
                    break;
            }

            List<User> userList = uService.GetAll(filterModel);
            if (userList != null && userList.Count > 0)
            {
                if (userList.Where(g => !g.Id.Equals(id)).Count() > 0)
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
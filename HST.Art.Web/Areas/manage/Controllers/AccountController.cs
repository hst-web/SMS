using HST.Art.Service;
using HST.Art.Core;
using HST.Utillity;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace HST.Art.Web.Areas.manage.Controllers
{
    public class AccountController : ApplicationBase
    {
        UserService uService = new UserService();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                bool result = LoginBase(model.Account, model.Password);
                if (result)
                {
                    AddLog("login", Constant.LOG_LOGIN_SUCCESS, string.Format(Constant.LOG_ACCOUNT_USER_NAME, model.Account), LogType.Operation);
                    return RedirectLogin();
                }
                else
                {
                    AddLog("login", string.Format(Constant.LOG_LOGIN_FAIL, ErrorMsg), string.Format(Constant.LOG_ACCOUNT_USER_NAME, model.Account), LogType.Operation);
                    ModelState.AddModelError("ErrorMessage", ErrorMsg);
                    return View(model);
                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("ErrorMessage", Constant.USER_EXCEPTION_ERROR);
                AddLog("login", ex.ToString(), string.Format(Constant.LOG_ACCOUNT_USER_NAME, model.Account));
            }
            return View(model);
        }

        public ActionResult LoginOut()
        {
            AddLog("logout", Constant.LOG_LOGOUT_SUCCESS, string.Format(Constant.LOG_ACCOUNT_USER_NAME, UserName), LogType.Operation);
            LogoutBase();
            return RedirectLogin();
        }

        public ActionResult Index()
        {
            try
            {
                Account account = GetAccount();
                User userInfo = uService.Get(account.Id);
                AccountViewModel accountModel = new AccountViewModel()
                {
                    Id = userInfo.Id,
                    UserName = userInfo.UserName,
                    Email = userInfo.Email,
                    Phone = userInfo.Telephone,
                    RealName = userInfo.Name,
                    IsSupAdmin = userInfo.IsAdmin,
                    CreateTime = userInfo.CreateDate
                };

                return View(accountModel);
            }
            catch (System.Exception ex)
            {
                AddLog("profile", ex.ToString(), string.Format(Constant.LOG_ACCOUNT_USER_NAME, UserName));
                // RedirectError();
            }

            return View(new AccountViewModel());
        }

        /// <summary>
        /// 更新账户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public string Update(AccountViewModel model)
        {
            try
            {
                ResultRetrun rmodel = new ResultRetrun();
                User userModel = uService.Get(model.Id);
                userModel.Email = model.Email;
                userModel.Telephone = model.Phone;
                userModel.Name = model.RealName;
                bool isSuccess = uService.Update(userModel);
                if (isSuccess)
                {
                    return "ok";
                }
            }
            catch (System.Exception ex)
            {
                // 用于记录日志查看
                model.UserName = UserName;
                AddLog("updateProfile", ex.ToString(), JsonConvert.SerializeObject(model));
            }

            return "error";
        }

        /// <summary>
        /// 修改密码
        /// </summary>                                          
        /// <param name="oldPwd">原密码</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="renewPwd">确认新密码</param>
        /// <returns></returns>
        [HttpPost]
        public string UpdatePwd(string oldPwd, string newPwd, string renewPwd)
        {
            try
            {
                bool isSuccess = false;
                var model = uService.Get(GetAccount().Id);
                if (model == null) return "error";

                if (!string.Equals(EncryptHelper.Encode(oldPwd, model.Salt), model.Password))
                {
                    return "旧密码输入错误";
                }

                isSuccess = uService.UpdatePassword(GetAccount().Id, newPwd);

                if (isSuccess)
                {
                    AddLog("updatePwd", Constant.LOG_UPDATE_PWD, string.Format(Constant.LOG_ACCOUNT_USER_NAME, UserName), LogType.Operation);
                    RemoveStoredData();
                    return "ok";
                }
            }
            catch (System.Exception ex)
            {
                AddLog("updatePwd", ex.ToString(), string.Format(Constant.LOG_ACCOUNT_USER_NAME, UserName));
            }

            return "error";
        }
    }
}

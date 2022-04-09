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
    public class HomeController : ApplicationBase
    {
        OrganizationService orgService = new OrganizationService();
        RotationChartService rotationService = new RotationChartService();
        // GET: Home
        public ActionResult Index()
        {
            Account account = GetAccount();          
            return View(account);
        }
        public ActionResult welcome()
        { 
            return View();
        }

        [HttpPost]
        public ActionResult GetJsonData(SearchDataTable dt, RotationType rotationType)
        {
            if (rotationType == RotationType.UnKnown)
            {
                return Json(new
                {
                    data = new List<RotationChartViewModel>()
                }, JsonRequestBehavior.AllowGet);
            }

            int totalNum = 0;
            List<RotationChart> rotationList = rotationService.GetAll(rotationType);
            totalNum = rotationList != null ? rotationList.Count : 0;

            ReturnPageResultIList<RotationChart> data = new ReturnPageResultIList<Core.RotationChart>(rotationList, totalNum);
            IList<RotationChartViewModel> gmList = new List<RotationChartViewModel>();

            if (data != null && data.DataT != null)
                gmList = data.DataT.Select(g => new RotationChartViewModel() { Id = g.Id, HeadImg = g.ImgSrc, SmallHeadImg = GetThumb(g.ImgSrc), WebLink = g.WebLink, RotationType = g.Type }).ToList();

            return Json(new
            {
                recordsFiltered = data.totalRecords,
                recordsTotal = data.totalPages,
                data = gmList
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit()
        {
            Organization model = orgService.GetByNumber(Constant.INIT_ORG_NUMBER);
            if (model != null)
            {
                return View(new OrganizationViewModel()
                {
                    Id = model.Id,
                    Description = model.Description,
                    Detail = model.Detail,
                    Blog = model.Blog,
                    Email = model.Email,
                    Framework = model.Framework,
                    Number = model.Number,
                    OrgName = model.Name,
                    SmallHeadImg = GetThumb(model.WeChat),
                    WeChat = model.WeChat,
                    Telephone = model.Telephone,
                    Logo = model.Logo,
                    Address = model.Address
                });
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(OrganizationViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                Organization data = orgService.Get(model.Id);
                data.Name = model.OrgName;
                data.Framework = model.Framework;
                data.Description = model.Description;
                data.Telephone = model.Telephone;
                data.WeChat = model.WeChat;
                data.Blog = model.Blog;
                data.Logo = model.Logo;
                data.Detail = model.Detail;
                data.Email = model.Email;
                data.Address = model.Address;

                rmodel.isSuccess = orgService.Update(data);
            }

            return Json(rmodel);
        }


        #region 新增
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddRotation(RotationType rotationType)
        {
            ViewBag.RotationType = rotationType;
            return View();
        }

        [HttpPost]
        public JsonResult AddRotation(RotationChartViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                RotationChart downModel = new RotationChart()
                {
                    ImgSrc = model.HeadImg,
                    Type = model.RotationType,
                    WebLink = model.WebLink,
                    State = PublishState.Upper
                };


                List<RotationChart> rlist = rotationService.GetAll(model.RotationType);
                if (rlist != null && model.RotationType == RotationType.Banner && rlist.Count >= 5)
                {
                    rmodel.message = "最多添加Banner轮播不可超过5条";
                    return Json(rmodel);
                }

                if (rlist != null && model.RotationType == RotationType.Logo && rlist.Count >= 20)
                {
                    rmodel.message = "最多添加Logo轮播不可超过20条";
                    return Json(rmodel);
                }

                rmodel.isSuccess = rotationService.Add(downModel);
            }

            return Json(rmodel);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditRotation(int id)
        {
            RotationChart model = rotationService.Get(id);
            if (model != null)
            {
                return View(new RotationChartViewModel()
                {
                    Id = model.Id,
                    HeadImg = model.ImgSrc,
                    WebLink = model.WebLink,
                    RotationType = model.Type,
                    SmallHeadImg = GetThumb(model.ImgSrc)
                });
            }
            return View();
        }

        [HttpPost]
        public JsonResult EditRotation(RotationChartViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                RotationChart data = rotationService.Get(model.Id);
                data.ImgSrc = model.HeadImg;
                data.Type = model.RotationType;
                data.WebLink = model.WebLink;

                rmodel.isSuccess = rotationService.Update(data);
            }

            return Json(rmodel);
        }


        [HttpPost]
        public JsonResult UpdateSort(RotationChartViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (string.IsNullOrEmpty(model.SortJson))
            {
                return Json(rmodel);
            }

            List<RotationSort> rSortList = rotationService.GetRotationSorts();
            if (rSortList.Find(g => g.RotationType == model.RotationType) != null)
            {
                rSortList.Find(g => g.RotationType == model.RotationType).SortList = JsonConvert.DeserializeObject<List<int>>(model.SortJson);
            }
            else
            {
                rSortList.Add(new RotationSort() { RotationType = model.RotationType, SortList = JsonConvert.DeserializeObject<List<int>>(model.SortJson) });
            }

            rmodel.isSuccess = rotationService.UpdateRotationSort(rSortList);

            return Json(rmodel);
        }

        #endregion

        #region 状态操作
        public override JsonResult Delete(int id)
        {
            ResultRetrun rmodel = new ResultRetrun();
            try
            {
                rmodel.isSuccess = rotationService.Delete(id);
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
                rmodel.isSuccess = rotationService.Publish(id);
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
                rmodel.isSuccess = rotationService.Recovery(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "操作失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult SearchLog(string SearchDate)
        {
            if (string.IsNullOrEmpty(SearchDate)) return Json("", JsonRequestBehavior.AllowGet);
            string fileName = SearchDate.Replace("-", "") + ".txt";
            string path = AppDomain.CurrentDomain.BaseDirectory + "log\\";
            if (!Directory.Exists(path)) return Json("", JsonRequestBehavior.AllowGet);
            FileInfo fi = new FileInfo(path + fileName);
            if (!fi.Exists) return Json("", JsonRequestBehavior.AllowGet);

            LogViewModel model = new LogViewModel();
            model.SearchDate = SearchDate;
            model.FileCount = 1;
            model.DownloadUrl = @"\log\" + fileName;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileResult DownloadFile(string SearchDate)
        {
            string fileName = SearchDate.Replace("-", "") + ".txt";
            string downFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\log\" + fileName;
            FileStream fs = new FileStream(downFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            return File(fs, "text/plain", Url.Encode(fileName));
        }
    }
}

using HST.Art.Core;
using HST.Art.Service;
using System;
using System.Web.Mvc;
using HST.Utillity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace HST.Art.Web.Areas.manage.Controllers
{
    public class DownloadController : ApplicationBase
    {
        FileDownloadService downService = new FileDownloadService();
        CategoryDictionaryService cdService = new CategoryDictionaryService();
        List<CategoryDictionary> cdEnabledList = new List<CategoryDictionary>();
        public ActionResult List()
        {
            InitData();
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
                    case SearchType.Title:
                        fkey = "Title";
                        fillter.FilterType = FilterType.Like;
                        break;
                    case SearchType.Type:
                        fkey = "Category";
                        break;
                    case SearchType.State:
                        fkey = "State";
                        break;
                }

                fillter.KeyValueList.Add(new KeyValueObj() { Key = fkey, Value = svm.FilterVal });
            }

            List<FileDownload> downList = downService.GetPage(fillter, out totalNum);
            ReturnPageResultIList<FileDownload> data = new ReturnPageResultIList<Core.FileDownload>(downList, totalNum);
            IList<DownloadViewModel> gmList = new List<DownloadViewModel>();

            if (data != null && data.DataT != null)
                gmList = data.DataT.Select(g => new DownloadViewModel() { Id = g.Id, UserId = g.UserId, FileName = g.Name, CategoryName = g.CategoryName, State = (int)g.State, CreateTime = g.CreateDate.ToString("yyyy-MM-dd HH:mm"), Category = g.Category, FileType = g.Type, FileTitle = g.Title, UserName = g.UserName,Src=g.Src }).ToList();

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
            FileDownload data = downService.Get(id);
            InitData();
            DownloadViewModel model = new DownloadViewModel();
            if (data != null)
            {
                model.Id = data.Id;
                model.FileName = data.Name;
                model.FileTitle = data.Title;
                model.CategoryName = data.CategoryName;
                model.UserName = data.UserName;
                model.State = (int)data.State;
                model.UserId = data.UserId;
                model.Description = data.Description;
                model.FileImg = data.HeadImg;
                model.SmallFileImg = GetThumb(data.HeadImg);
                model.FileType = data.Type;
                model.Src = data.Src;
                model.Category = data.Category;

                #region 类别可用判断  预留
                //if (cdEnabledList != null && cdEnabledList.Count > 0 && cdEnabledList.Where(g => g.Id == data.Category).Count() > 0)
                //{
                //    model.Category = data.Category;
                //}
                #endregion

                return View(model);
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(DownloadViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                FileDownload data = downService.Get(model.Id);
                data.Title = model.FileTitle;
                data.Name = model.FileName;
                data.Description = !string.IsNullOrEmpty(model.Description) ? model.Description.Replace("\r\n", "") : string.Empty;
                data.Category = model.Category;
                data.State = (PublishState)model.State;
                data.HeadImg = model.FileImg;
                data.Src = model.Src;
                data.Type = string.IsNullOrEmpty(model.Extension) ? model.FileType : getFileFormat(model.Extension);

                rmodel.isSuccess = downService.Update(data);
            }

            return Json(rmodel);
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
            InitData();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add(DownloadViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                FileDownload downModel = new FileDownload()
                {
                    Title = model.FileTitle,
                    Name = model.FileName,
                    Description = !string.IsNullOrEmpty(model.Description) ? model.Description.Replace("\r\n", "") : string.Empty,
                    Category = model.Category,
                    State = (PublishState)model.State,
                    HeadImg = model.FileImg,
                    Src = model.Src,
                    Type = getFileFormat(model.Extension),
                    UserId = GetAccount().Id
                };
                rmodel.isSuccess = downService.Add(downModel);
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
                rmodel.isSuccess = downService.LogicDelete(id);
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
                rmodel.isSuccess = downService.Publish(id);
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
                rmodel.isSuccess = downService.Recovery(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "操作失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Detail(int id)
        {
            FileDownload data = downService.Get(id);

            if (data != null)
                return View(new DownloadViewModel
                {
                    Id = data.Id,
                    FileName = data.Name,
                    FileTitle = data.Title,
                    CategoryName = data.CategoryName,
                    UserName = data.UserName,
                    State = (int)data.State,
                    Category = data.Category,
                    UserId = data.UserId,
                    Description = data.Description,
                    FileImg = data.HeadImg,
                    FileType = data.Type,
                    Src = data.Src,
                    CreateTime = data.CreateDate.ToString("yyyy-MM-dd HH;MM")
                });
            else
                return View();
        }

        private void InitData()
        {
            List<CategoryDictionary> cdAllList = cdService.GetAll(CategoryType.Download);
            if (cdAllList != null && cdAllList.Count > 0)
            {
                cdEnabledList = cdAllList.FindAll(g => g.State == PublishState.Upper);
            }

            ViewBag.AllCategory = cdAllList != null ? cdAllList : new List<CategoryDictionary>();
            ViewBag.EnabledCategory = cdEnabledList;
        }

        private FileFormat getFileFormat(string extension)
        {
            if (string.IsNullOrEmpty(extension)) return FileFormat.UnKnow;

            string imgList = ".jpg,.jpeg,.gif,.bmp,.png";
            string wordList = ".doc,.docx";
            string excelList = ".xlsx,.xls";
            string pptList = ".ppt,.pptx";
            string txtList = ".txt,.csv";

            if (txtList.Split(',').Where(g => g.Equals(extension, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return FileFormat.TXT;
            }

            if (extension.Equals(".pdf", StringComparison.InvariantCulture))
            {
                return FileFormat.PDF;
            }

            if (wordList.Split(',').Where(g => g.Equals(extension, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return FileFormat.Word;
            }

            if (excelList.Split(',').Where(g => g.Equals(extension, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return FileFormat.XLSX;
            }

            if (pptList.Split(',').Where(g => g.Equals(extension, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return FileFormat.PPT;
            }

            if (imgList.Split(',').Where(g => g.Equals(extension, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return FileFormat.Img;
            }

            return FileFormat.UnKnow;
        }
    }
}
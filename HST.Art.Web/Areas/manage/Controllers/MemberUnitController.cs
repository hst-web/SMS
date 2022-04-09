using HST.Art.Core;
using HST.Art.Service;
using System;
using System.Web.Mvc;
using HST.Utillity;
using System.Collections.Generic;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using System.Text;
using System.IO;
using System.Web.Configuration;
using System.Web;

namespace HST.Art.Web.Areas.manage.Controllers
{
    public class MemberUnitController : ApplicationBase
    {
        MemberUnitService muService = new MemberUnitService();
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
                    case SearchType.Name:
                        fkey = "Name";
                        fillter.FilterType = FilterType.Like;
                        break;
                    case SearchType.Number:
                        fkey = "Number";
                        break;
                    case SearchType.Type:
                        fkey = "Category";
                        break;
                    case SearchType.Area:
                        fkey = "City";
                        break;
                }

                fillter.KeyValueList.Add(new KeyValueObj() { Key = fkey, Value = svm.FilterVal });
            }

            List<MemberUnit> downList = muService.GetPage(fillter, out totalNum);
            ReturnPageResultIList<MemberUnit> data = new ReturnPageResultIList<Core.MemberUnit>(downList, totalNum);
            IList<MemberUnitViewModel> gmList = new List<MemberUnitViewModel>();

            if (data != null && data.DataT != null)
                gmList = data.DataT.Select(g => new MemberUnitViewModel() { Id = g.Id, UserId = g.UserId, MemberUnitName = g.Name, CategoryName = g.CategoryName, State = (int)g.State, CreateTime = g.CreateDate.ToString("yyyy-MM-dd HH:mm"), Category = g.Category, UserName = g.UserName, Number = g.Number, Star = g.Star, HeadImg = g.HeadImg, SmallHeadImg = GetThumb(g.HeadImg), Province = Convert.ToInt32(g.Province), City = Convert.ToInt32(g.City), Area = GetAreaStr(g.Province, g.City) }).ToList();

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
            MemberUnit data = muService.Get(id);
            InitData();
            if (data != null)
            {
                MemberUnitViewModel model = new MemberUnitViewModel();
                model.Id = data.Id;
                model.MemberUnitName = data.Name;
                model.CategoryName = data.CategoryName;
                model.UserName = data.UserName;
                model.State = (int)data.State;
                model.UserId = data.UserId;
                model.Description = data.Description;
                model.HeadImg = data.HeadImg;
                model.SmallHeadImg = GetThumb(data.HeadImg);
                model.Star = data.Star;
                model.City = string.IsNullOrEmpty(data.City) ? 0 : Convert.ToInt32(data.City);
                model.Province = string.IsNullOrEmpty(data.Province) ? 0 : Convert.ToInt32(data.Province);
                model.Number = data.Number;
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
        public JsonResult Edit(MemberUnitViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                MemberUnit data = muService.Get(model.Id);
                data.Name = model.MemberUnitName;
                data.Description = !string.IsNullOrEmpty(model.Description) ? model.Description.Replace("\r\n", "") : string.Empty;
                data.Category = model.Category;
                data.State = (PublishState)model.State;
                data.HeadImg = model.HeadImg;
                data.Star = model.Star;
                data.City = model.City.ToString();
                data.Province = model.Province < 1 ? Constant.DEFAULT_PROVINCE : model.Province.ToString();
                data.Number = model.Number;

                rmodel.isSuccess = muService.Update(data);
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
        public JsonResult Add(MemberUnitViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                MemberUnit downModel = new MemberUnit()
                {
                    Name = model.MemberUnitName,
                    Description = !string.IsNullOrEmpty(model.Description) ? model.Description.Replace("\r\n", "") : string.Empty,
                    Category = model.Category,
                    State = (PublishState)model.State,
                    HeadImg = model.HeadImg,
                    Star = model.Star,
                    UserId = GetAccount().Id,
                    Number = model.Number,
                    City = model.City.ToString(),
                    Province = model.Province.ToString()
                };
                rmodel.isSuccess = muService.Add(downModel);
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
                rmodel.isSuccess = muService.LogicDelete(id);
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
                rmodel.isSuccess = muService.Publish(id);
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
                rmodel.isSuccess = muService.Recovery(id);
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
            MemberUnit data = muService.Get(id);

            if (data != null)
                return View(new MemberUnitViewModel
                {
                    Id = data.Id,
                    MemberUnitName = data.Name,
                    CategoryName = data.CategoryName,
                    UserName = data.UserName,
                    State = (int)data.State,
                    Category = data.Category,
                    UserId = data.UserId,
                    Description = data.Description,
                    HeadImg = data.HeadImg,
                    SmallHeadImg = GetThumb(data.HeadImg),
                    Star = data.Star,
                    City = string.IsNullOrEmpty(data.City) ? 0 : Convert.ToInt32(data.City),
                    Province = string.IsNullOrEmpty(data.Province) ? 0 : Convert.ToInt32(data.Province),
                    Number = data.Number,
                    CreateTime = data.CreateDate.ToString("yyyy-MM-dd HH;MM")
                });
            else
                return View();
        }

        private void InitData()
        {
            List<CategoryDictionary> cdAllList = cdService.GetAll(CategoryType.Member);
            if (cdAllList != null && cdAllList.Count > 0)
            {
                cdEnabledList = cdAllList.FindAll(g => g.State == PublishState.Upper);
            }

            ViewBag.AreaCity = City;
            ViewBag.AreaProvince = Province;
            ViewBag.AllCategory = cdAllList != null ? cdAllList : new List<CategoryDictionary>();
            ViewBag.EnabledCategory = cdEnabledList;
        }

        [HttpGet]
        public JsonResult CheckNumber(int id, string number)
        {
            ResultRetrun rmodel = new ResultRetrun();
            FilterEntityModel filterModel = new FilterEntityModel();
            filterModel.KeyValueList = new List<KeyValueObj>();
            filterModel.KeyValueList.Add(new KeyValueObj() { Key = "number", Value = number, FieldType = FieldType.String });

            List<MemberUnit> muList = muService.GetAll(filterModel);
            if (muList != null && muList.Count > 0)
            {
                if (muList.Where(g => !g.Id.Equals(id)).Count() > 0)
                    rmodel.message = "会员单位编号已经存在";
                else
                    rmodel.isSuccess = true;
            }
            else
                rmodel.isSuccess = true;
            return Json(rmodel.isSuccess, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Import()
        {
            return View();
        }

        //导入
        [HttpPost]
        public JsonResult ImportExcel(HttpPostedFileBase file)
        {
            string locAddr = WebConfigurationManager.AppSettings["FileAddr"].ToString();
            string filePath = "/uploadFiles/";
            string localPath = string.IsNullOrEmpty(locAddr) ? System.Web.Hosting.HostingEnvironment.MapPath(filePath) : locAddr + filePath;

            string errorMsg = "";
            int userid = Convert.ToInt16(GetAccount().Id);
            List<MemberUnit> failList = new List<MemberUnit>();

            UploadViewModel um_mod = new UploadViewModel();
            string suffixstr = Path.GetExtension(file.FileName);
            um_mod.Extension = suffixstr;

            if (file == null || file.ContentLength == 0)
                um_mod.Message = "情选择文件";
            else if (!IsExcel(suffixstr))
                um_mod.Message = "请上传xls、xlsx类型文件";
            else
            {
                List<MemberUnit> memberList = GetData(file.FileName, file.InputStream, out errorMsg);
                if (memberList != null && memberList.Count > 0)
                {
                    memberList.ForEach(g => g.UserId = userid);
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    um_mod.Message = errorMsg;
                }
                else
                {
                    //文件后缀
                    string Suffixstr = Path.GetExtension(file.FileName);
                    um_mod.Extension = Suffixstr;
                    um_mod.FileName = file.FileName;

                    List<MemberUnit> failOutList = null;
                    failList = memberList.FindAll(g => string.IsNullOrEmpty(g.Name) || string.IsNullOrEmpty(g.Number) || string.IsNullOrEmpty(g.City));
                    memberList.RemoveAll(g => string.IsNullOrEmpty(g.Name) || string.IsNullOrEmpty(g.Number) || string.IsNullOrEmpty(g.City));

                    bool result = muService.Add(memberList, out failOutList);

                    if (!result)
                    {
                        failList.AddRange(failOutList);
                    }

                    if (failList.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString("N") + ".csv";
                        um_mod.Message = string.Format("导入失败条数{0}条,成功条数{1}条", failList.Count, memberList.Count == 0 ? 0 : memberList.Count - failOutList.Count);
                        um_mod.FilePath = filePath + fileName;
                        SaveFile(localPath + fileName, failList);
                    }
                    else
                    {
                        um_mod.IsSuccess = result;
                    }
                }
            }

            return Json(um_mod);
        }

        private void SaveFile(string path, List<MemberUnit> failList)
        {
            try
            {
                List<MemberUnitViewModel> memberViewModelList = failList.Select(g => new MemberUnitViewModel() { MemberUnitName = g.Name, Area = GetAreaStr(g.Province, g.City), Category = g.Category, Number = g.Number, Star = g.Star }).ToList();

                List<ExcelHeaderColumn> excelList = new List<ExcelHeaderColumn>(){
                new ExcelHeaderColumn() { DisplayName = "单位名称", Name = "MemberUnitName" },
                new ExcelHeaderColumn() { DisplayName = "单位编号", Name = "Number" },
                new ExcelHeaderColumn() { DisplayName = "单位星级", Name = "StarName" },
                new ExcelHeaderColumn() { DisplayName = "所在地区", Name = "Area" },
                new ExcelHeaderColumn() { DisplayName = "单位类别", Name = "CategoryName" },
                };

                byte[] bytes = ExcelHelper.ExportCsvByte(memberViewModelList, excelList);

                System.IO.StreamWriter stream = new System.IO.StreamWriter(path, false, Encoding.UTF8);
                stream.Write(Encoding.Default.GetString(bytes));
                stream.Flush();
                stream.Close();
                stream.Dispose();
            }
            catch
            {

            }
        }

        private List<MemberUnit> GetData(string fileName, Stream fs, out string errorMsg)
        {
            errorMsg = string.Empty;
            List<MemberUnit> dicList = new List<MemberUnit>();
            IWorkbook workbook = null;//工作表

            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook(fs);
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook(fs);
            fs.Close();

            ISheet sheet = workbook.GetSheetAt(0);//取默认第一个sheet表的数据
            if (sheet.SheetName.StartsWith("association", StringComparison.InvariantCultureIgnoreCase))
            {
                dicList = GetSheetData(sheet);
                if (dicList.Count <= 0)
                {
                    errorMsg = "没有找到要导入的数据";
                }
            }
            else
            {
                errorMsg = "当前导入的Excel不是协会会员模板";
            }

            return dicList;
        }

        private List<MemberUnit> GetSheetData(ISheet sheet)
        {
            List<CategoryDictionary> cdAllList = cdService.GetAll(CategoryType.Member);
            List<MemberUnit> memberCertList = null;
            const int minrownum = 1;//最小行数，如果小于1行证明sheet无数据
            if (sheet != null && sheet.LastRowNum >= minrownum)
            {
                int startrownum = 1;//从第2行开始取数据（row和cell从0开始）
                int endrownum = sheet.LastRowNum;
                memberCertList = new List<MemberUnit>();
                MemberUnit memberCertInfo = null;
                for (int index = startrownum; index <= endrownum; index++)
                {
                    IRow row = sheet.GetRow(index);

                    int star = 0;
                    int.TryParse(Regex.Replace(CellSwitch(row.GetCell(2)), @"\s", ""), out star);
                    CategoryDictionary cdict = string.IsNullOrEmpty(CellSwitch(row.GetCell(4))) ? null : cdAllList.Find(g => g.Name.Equals(CellSwitch(row.GetCell(4))));

                    memberCertInfo = new MemberUnit()
                    {
                        Name = Regex.Replace(CellSwitch(row.GetCell(0)), @"\s", ""),
                        Number = Regex.Replace(CellSwitch(row.GetCell(1)), @"\s", ""),
                        Star = star,
                        State = PublishState.Lower,
                        Province = Constant.DEFAULT_PROVINCE,
                        City = string.IsNullOrWhiteSpace(CellSwitch(row.GetCell(3))) ? "" : City.Where(g => g.Value.Equals(CellSwitch(row.GetCell(3)))).FirstOrDefault().Key + "",
                        Category = cdict != null ? cdict.Id : 0
                    };

                    memberCertList.Add(memberCertInfo);
                }
            }

            return memberCertList;
        }
    }
}
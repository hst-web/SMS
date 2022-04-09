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
    public class TeacherController : ApplicationBase
    {
        TeaCertificateService teaService = new TeaCertificateService();

        public ActionResult List()
        {
            ViewBag.AreaCity = City;
            ViewBag.AreaProvince = Province;

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
                    case SearchType.Type:
                        fkey = "Category";
                        break;
                    case SearchType.Number:
                        fkey = "Number";
                        break;
                    case SearchType.Area:
                        fkey = "City";
                        break;
                }

                fillter.KeyValueList.Add(new KeyValueObj() { Key = fkey, Value = svm.FilterVal });
            }

            List<TeaCertificate> teaList = teaService.GetPage(fillter, out totalNum);
            ReturnPageResultIList<TeaCertificate> data = new ReturnPageResultIList<Core.TeaCertificate>(teaList, totalNum);
            IList<TeaViewModel> gmList = new List<TeaViewModel>();

            if (data != null && data.DataT != null)
                gmList = data.DataT.Select(g => new TeaViewModel() { Id = g.Id, UserId = g.UserId, TeacherName = g.Name, Number = g.Number, Gender = g.Gender, State = (int)g.State, CreateTime = g.CreateDate.ToString("yyyy-MM-dd HH:mm"), Category = g.Category, Level = g.Level, Province = Convert.ToInt32(g.Province), City = Convert.ToInt32(g.City), UserName = g.UserName, Area = GetAreaStr(g.Province, g.City) }).ToList();

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
            TeaCertificate data = teaService.Get(id);
            ViewBag.AreaCity = City;
            ViewBag.AreaProvince = Province;

            if (data != null)
                return View(new TeaViewModel
                {
                    Id = data.Id,
                    TeacherName = data.Name,
                    Number = data.Number,
                    City = string.IsNullOrEmpty(data.City) ? 0 : Convert.ToInt32(data.City),
                    Province = string.IsNullOrEmpty(data.Province) ? 0 : Convert.ToInt32(data.Province),
                    State = (int)data.State,
                    Category = data.Category,
                    Level = data.Level,
                    Gender = data.Gender,
                    HeadImg = data.HeadImg
                });
            else
                return View();
        }

        [HttpPost]
        public JsonResult Edit(TeaViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                TeaCertificate data = teaService.Get(model.Id);
                data.Number = model.Number;
                data.Name = model.TeacherName;
                data.Level = model.Level;
                data.Category = model.Category;
                data.State = (PublishState)model.State;
                data.Gender = model.Gender;
                data.City = model.City.ToString();
                data.Province = model.Province < 1 ? Constant.DEFAULT_PROVINCE : model.Province.ToString();
                data.HeadImg = model.HeadImg;

                rmodel.isSuccess = teaService.Update(data);
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
            ViewBag.AreaCity = City;
            ViewBag.AreaProvince = Province;
            return View();
        }
        [HttpPost]
        public JsonResult Add(TeaViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                TeaCertificate teaModel = new TeaCertificate()
                {
                    Number = model.Number,
                    Name = model.TeacherName,
                    Level = model.Level,
                    Category = model.Category,
                    State = (PublishState)model.State,
                    Gender = model.Gender,
                    City = model.City.ToString(),
                    Province = model.Province.ToString(),
                    UserId = GetAccount().Id,
                    HeadImg = model.HeadImg
                };
                rmodel.isSuccess = teaService.Add(teaModel);
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
                rmodel.isSuccess = teaService.LogicDelete(id);
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
                rmodel.isSuccess = teaService.Publish(id);
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
                rmodel.isSuccess = teaService.Recovery(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "操作失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public JsonResult CheckTeaNumber(int id, string number)
        {
            ResultRetrun rmodel = new ResultRetrun();
            FilterEntityModel filterModel = new FilterEntityModel();
            filterModel.KeyValueList = new List<KeyValueObj>();
            filterModel.KeyValueList.Add(new KeyValueObj() { Key = "number", Value = number, FieldType = FieldType.String });

            List<TeaCertificate> teaList = teaService.GetAll(filterModel);
            if (teaList != null && teaList.Count > 0)
            {
                if (teaList.Where(g => !g.Id.Equals(id)).Count() > 0)
                    rmodel.message = "证书编号已经存在";
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
            List<TeaCertificate> failList = new List<TeaCertificate>();

            UploadViewModel um_mod = new UploadViewModel();
            string suffixstr = Path.GetExtension(file.FileName);
            um_mod.Extension = suffixstr;

            if (file == null || file.ContentLength == 0)
                um_mod.Message = "情选择文件";
            else if (!IsExcel(suffixstr))
                um_mod.Message = "请上传xls、xlsx类型文件";
            else
            {             
                List<TeaCertificate> teaList = GetData(file.FileName, file.InputStream, out errorMsg);
                if (teaList != null && teaList.Count > 0)
                {
                    teaList.ForEach(g => g.UserId = userid);
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

                    List<TeaCertificate> failOutList = null;
                    failList = teaList.FindAll(g => string.IsNullOrEmpty(g.Name) || string.IsNullOrEmpty(g.Number) || string.IsNullOrEmpty(g.City));
                    teaList.RemoveAll(g => string.IsNullOrEmpty(g.Name) || string.IsNullOrEmpty(g.Number) || string.IsNullOrEmpty(g.City));

                    bool result = teaService.Add(teaList, out failOutList);

                    if (!result)
                    {
                        failList.AddRange(failOutList);
                    }

                    if (failList.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString("N") + ".csv";
                        um_mod.Message = string.Format("导入失败条数{0}条,成功条数{1}条", failList.Count, teaList.Count == 0 ? 0 : teaList.Count - failOutList.Count);
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

        private void SaveFile(string path, List<TeaCertificate> failList)
        {
            try
            {
                List<TeaViewModel> teaViewModelList = failList.Select(g => new TeaViewModel() { TeacherName = g.Name, Area = GetAreaStr(g.Province, g.City), Category = g.Category, Gender = g.Gender, Number = g.Number, Level = g.Level }).ToList();

                List<ExcelHeaderColumn> excelList = new List<ExcelHeaderColumn>(){
                new ExcelHeaderColumn() { DisplayName = "教师姓名", Name = "TeacherName" },
                new ExcelHeaderColumn() { DisplayName = "性别", Name = "GenderName" },
                 new ExcelHeaderColumn() { DisplayName = "教师级别", Name = "LevelName" },
                new ExcelHeaderColumn() { DisplayName = "证书编号", Name = "Number" },
                new ExcelHeaderColumn() { DisplayName = "所在地区", Name = "Area" },
                new ExcelHeaderColumn() { DisplayName = "证书类型", Name = "CategoryName" },
                };

                byte[] bytes = ExcelHelper.ExportCsvByte(teaViewModelList, excelList);

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

        private List<TeaCertificate> GetData(string fileName, Stream fs, out string errorMsg)
        {
            errorMsg = string.Empty;
            List<TeaCertificate> dicList = new List<TeaCertificate>();
            IWorkbook workbook = null;//工作表

            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook(fs);
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook(fs);
            fs.Close();

            ISheet sheet = workbook.GetSheetAt(0);//取默认第一个sheet表的数据
            if (sheet.SheetName.StartsWith("teacher", StringComparison.InvariantCultureIgnoreCase))
            {
                dicList = GetSheetData(sheet);
                if (dicList.Count <= 0)
                {
                    errorMsg = "没有找到要导入的数据";
                }
            }
            else
            {
                errorMsg = "当前导入的Excel不是教师证书模板";
            }

            return dicList;
        }

        private List<TeaCertificate> GetSheetData(ISheet sheet)
        {
            List<TeaCertificate> teaCertList = null;
            const int minrownum = 1;//最小行数，如果小于1行证明sheet无数据
            if (sheet != null && sheet.LastRowNum >= minrownum)
            {
                int startrownum = 1;//从第2行开始取数据（row和cell从0开始）
                int endrownum = sheet.LastRowNum;
                teaCertList = new List<TeaCertificate>();
                TeaCertificate teaCertInfo = null;
                for (int index = startrownum; index <= endrownum; index++)
                {
                    IRow row = sheet.GetRow(index);
                    teaCertInfo = new TeaCertificate()
                    {
                        Name = Regex.Replace(CellSwitch(row.GetCell(0)), @"\s", ""),
                        Gender = CellSwitch(row.GetCell(1)).Equals("男") ? Gender.Male : Gender.Female,
                        Level = CellSwitch(row.GetCell(2)).Equals("特级教师") ? LevelType.Super : CellSwitch(row.GetCell(2)).Equals("高级教师") ? LevelType.Senior : LevelType.General,
                        Number = Regex.Replace(CellSwitch(row.GetCell(3)), @"\s", ""),
                        State = PublishState.Lower,
                        Province = Constant.DEFAULT_PROVINCE,
                        City = string.IsNullOrWhiteSpace(CellSwitch(row.GetCell(4))) ? "" : City.Where(g => g.Value.Equals(CellSwitch(row.GetCell(4)))).FirstOrDefault().Key + "",
                        Category = CellSwitch(row.GetCell(5)).Equals("师资认证") ? CertificateType.Train : CertificateType.Prize
                    };

                    teaCertList.Add(teaCertInfo);
                }
            }

            return teaCertList;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZT.SMS.Core;
using ZT.SMS.Service;
using Newtonsoft.Json;
using System.Web.Configuration;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using ZT.Utillity;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.ComponentModel;

namespace ZT.SMS.Web.Areas.manage.Controllers
{
    public class MessageRecordController : ApplicationBase
    {
        MessageRecordService msgService = new MessageRecordService();

        // GET: manage/MessageRecord
        public ActionResult List()
        {
            ViewBag.IsSupAdmin = IsAdmin;
            return View();
        }


        [HttpPost]
        public ActionResult GetJsonData(SearchDataTable dt, MsgViewQuery svm)
        {
            int totalNum = 0;
            MessageRecordQuery fillter = new MessageRecordQuery();
            fillter.PageIndex = dt.pageIndex;
            fillter.PageSize = dt.length;
            if (svm != null)
            {
                fillter.CreateDate = svm.CreateDate;
                fillter.MessageId = svm.MsgId;
                fillter.SendState = svm.SendState;
            }

            List<MessageRecord> msgList = msgService.GetPage(fillter, out totalNum);
            ReturnPageResultIList<MessageRecord> data = new ReturnPageResultIList<MessageRecord>(msgList, totalNum);
            IList<MsgViewModel> gmList = new List<MsgViewModel>();

            if (data != null && data.DataT != null)
                gmList = data.DataT.Select(g => new MsgViewModel() { Id = g.Id, UserId = g.OperatorId, Number = g.MessageId, State = g.SendState, CreateTime = g.CreateDate.ToString("yyyy-MM-dd HH:mm"), Phone = g.ToAddress, SendDate = g.SendDate == null || g.SendDate == DateTime.MinValue ? "--" : g.SendDate.ToString("yyyy-MM-dd HH:mm"), OrderDate = DateTime.MinValue.Equals(g.MsgData.OrderDate) ? "--" : g.MsgData.OrderDate.ToString("yyyy-MM-dd") }).ToList();

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
            MessageRecord data = msgService.Get(id);
            if (data != null)
                return View(new MsgViewModel
                {
                    Id = data.Id,
                    Number = data.MessageId,
                    Phone = data.ToAddress,
                    OrderDate = DateTime.MinValue.Equals(data.MsgData.OrderDate) ? "" : data.MsgData.OrderDate.ToString("yyyy-MM-dd")
                });
            else
                return View();
        }

        [HttpPost]
        public JsonResult Edit(MsgViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (!ValidateHelper.IsDateTime(model.OrderDate))
            {
                rmodel.message = "订单日期输入有误";
            }

            if (ModelState.IsValid)
            {
                MessageRecord data = msgService.Get(model.Id);
                data.MessageId = model.Number;
                data.SendState = model.State;
                data.ToAddress = model.Phone;
                data.MsgData.OrderDate = Convert.ToDateTime(model.OrderDate);
                data.OperatorId = UserId;
                rmodel.isSuccess = msgService.Update(data);
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
            return View();
        }
        [HttpPost]
        public JsonResult Add(MsgViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();

            if (!ValidateHelper.IsDateTime(model.OrderDate))
            {
                rmodel.message = "订单日期输入有误";
            }

            if (ModelState.IsValid)
            {
                MessageRecord msgModel = new MessageRecord()
                {
                    MessageId = model.Number,
                    OperatorId = UserId,
                    SendState = MsgSendState.Unsent,
                    ToAddress = model.Phone,
                    MsgData = new MsgDataInfo() { OrderDate = Convert.ToDateTime(model.OrderDate) }
                };
                rmodel.isSuccess = msgService.Add(msgModel);
            }

            return Json(rmodel);
        }
        #endregion

        public ActionResult Progress()
        {

            HttpContext.Response.Write("<script>alert(1)</script>");
            HttpContext.Response.Flush();
            return View();
        }


        #region 状态操作
        public override JsonResult Delete(int id)
        {
            ResultRetrun rmodel = new ResultRetrun();
            try
            {
                rmodel.isSuccess = msgService.Delete(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "操作失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Send(int id)
        {
            ResultRetrun rmodel = new ResultRetrun();
            MessageRecord data = msgService.Get(id);
            try
            {
                rmodel.isSuccess = msgService.Send(data);
            }
            catch (BizException biz)
            {
                rmodel.message = biz.Message;
            }
            catch (Exception ex)
            {
                rmodel.message = "操作失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BatchSend()
        {
            ResultRetrun rmodel = new ResultRetrun();
            List<MessageRecord> data = msgService.GetAll(new MessageRecordQuery() { SendState = (int)MsgSendState.Unsent });


            //BackgroundWorker backgroundWorker = new BackgroundWorker();
            //backgroundWorker.DoWork += (s, e1) =>
            //{
            //    List<MessageRecord> recordArgument = e1.Argument as List<MessageRecord>;
            //    //BatchSendSMS(recordArgument);
            //};

            //backgroundWorker.RunWorkerCompleted += (s, e1) =>
            //{


            //};
            //backgroundWorker.RunWorkerAsync(data);


            for (int i = 0; i < data.Count; i++)
            {
                rmodel = new ResultRetrun();
                try
                {
                    rmodel.isSuccess = msgService.Send(data[i]);
                    Thread.Sleep(5);
                }
                catch (BizException biz)
                {
                    rmodel.message = biz.Message;
                }
                catch (Exception ex)
                {
                    rmodel.message = "操作失败，原因：" + ex.Message;
                }

                Logger.Info(typeof(MessageRecordController), "BatchSend:" + data[i].MessageId + JsonUtils.Serialize(rmodel));
            }

            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }

        private void BatchSendSMS(List<MessageRecord> recordData, out StatisticalResult result)
        {
            result = new StatisticalResult(recordData.Count);
            int totalCount = recordData.Count;
            for (int i = 0; i < recordData.Count; i++)
            {
                ResultRetrun rmodel = new ResultRetrun();
                msgService.Send(recordData[i]);
                rmodel.isSuccess = true;
                rmodel.sign = (i + 1) * 100 / totalCount;
                Thread.Sleep(5);
                ViewBag.ProData = rmodel;
                result.SuccessCount += 1;
            }
        }

        [HttpGet]
        public JsonResult CheckProcess()
        {
            ResultRetrun rmodel = new ResultRetrun();
            int aaaa = 10;
            return Json(aaaa, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public JsonResult CheckMsgNumber(int id, string number)
        {
            ResultRetrun rmodel = new ResultRetrun();

            MessageRecord msgInfo = msgService.GetByMsgId(number);
            if (msgInfo != null)
            {
                if (!msgInfo.Id.Equals(id))
                    rmodel.message = "该订单编号已经存在";
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
            List<MessageRecord> failList = new List<MessageRecord>();

            UploadViewModel um_mod = new UploadViewModel();
            string suffixstr = Path.GetExtension(file.FileName);
            um_mod.Extension = suffixstr;

            if (file == null || file.ContentLength == 0)
                um_mod.Message = "情选择文件";
            else if (!IsExcel(suffixstr))
                um_mod.Message = "请上传xls、xlsx类型文件";
            else
            {
                List<MessageRecord> msgList = GetData(file.FileName, file.InputStream, out errorMsg);

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

                    List<MessageRecord> failOutList = null;
                    int msgTotalCount = msgList.Count;
                    failList = msgList.FindAll(g => !string.IsNullOrEmpty(g.Remark));
                    msgList.RemoveAll(g => !string.IsNullOrEmpty(g.Remark));
                    bool result = msgService.Add(msgList, out failOutList);

                    if (!result&& CollectionUtils.IsNotEmpty(failOutList))
                    {
                        failList.AddRange(failOutList);
                    }

                    if (failList.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString("N") + ".csv";
                        um_mod.Message = string.Format("导入失败条数{0}条,成功条数{1}条", failList.Count, msgTotalCount == 0 ? 0 : msgTotalCount - failList.Count);
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

        private void SaveFile(string path, List<MessageRecord> failList)
        {
            try
            {
                List<MsgViewModel> teaViewModelList = failList.Select(g => new MsgViewModel() { Number = g.MessageId, Phone = g.ToAddress, OrderName = g.MsgData.OrderName, Remark = g.Remark, OrderDate = DateTime.MinValue.Equals(g.MsgData.OrderDate) ? "" : g.MsgData.OrderDate.ToString("yyyy-MM-dd") }).ToList();

                List<ExcelHeaderColumn> excelList = new List<ExcelHeaderColumn>(){
                new ExcelHeaderColumn() { DisplayName = "订单编号", Name = "Number" },
                  new ExcelHeaderColumn() { DisplayName = "商品名称", Name = "OrderName" },
                new ExcelHeaderColumn() { DisplayName = "手机号", Name = "Phone" },
                 new ExcelHeaderColumn() { DisplayName = "订单日期", Name = "OrderDate" },
                  new ExcelHeaderColumn() { DisplayName = "备注", Name = "Remark" }
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

        private List<MessageRecord> GetData(string fileName, Stream fs, out string errorMsg)
        {
            errorMsg = string.Empty;
            List<MessageRecord> dicList = new List<MessageRecord>();
            IWorkbook workbook = null;//工作表

            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook(fs);
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook(fs);
            fs.Close();

            ISheet sheet = workbook.GetSheetAt(0);//取默认第一个sheet表的数据
            try
            {
                if (sheet.SheetName.StartsWith("sms", StringComparison.InvariantCultureIgnoreCase))
                {
                    dicList = GetSheetData(sheet);
                    if (dicList.Count <= 0)
                    {
                        errorMsg = "没有找到要导入的数据";
                    }
                }
                else
                {
                    errorMsg = "当前导入的Excel不是短信数据的模板";
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return dicList;
        }

        private List<MessageRecord> GetSheetData(ISheet sheet)
        {
            List<MessageRecord> msgRecordList = null;
            const int minrownum = 1;//最小行数，如果小于1行证明sheet无数据
            if (sheet != null && sheet.LastRowNum >= minrownum)
            {
                int startrownum = 1;//从第2行开始取数据（row和cell从0开始）
                int endrownum = sheet.LastRowNum;
                msgRecordList = new List<MessageRecord>();
                MessageRecord msgRecordInfo = null;
                for (int index = startrownum; index <= endrownum; index++)
                {
                    IRow row = sheet.GetRow(index);
                    if (row == null)
                    {
                        continue;
                    }

                    string orderDataStr = CellSwitch(row.GetCell(3));
                    msgRecordInfo = new MessageRecord()
                    {
                        MessageId = Regex.Replace(CellSwitch(row.GetCell(0)), @"\s", ""),
                        ToAddress = Regex.Replace(CellSwitch(row.GetCell(2)), @"\s", ""),
                        SendState = MsgSendState.Unsent,
                        OperatorId = UserId,
                        MsgData = new MsgDataInfo() { OrderDate = ValidateHelper.IsDateTime(orderDataStr) ? Convert.ToDateTime(orderDataStr) : DateTime.MinValue, OrderName = Regex.Replace(CellSwitch(row.GetCell(1)), @"\s", "") }
                    };

                    if (CheckMsg(msgRecordInfo))
                    {
                        msgRecordList.Add(msgRecordInfo);
                    }
                }
            }

            return msgRecordList;
        }

        private bool CheckMsg(MessageRecord msg)
        {
            if (string.IsNullOrEmpty(msg.MessageId) && string.IsNullOrEmpty(msg.ToAddress) && DateTime.MinValue.Equals(msg.MsgData.OrderDate) && string.IsNullOrEmpty(msg.MsgData.OrderName))
            {
                return false;
            }

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(msg.MessageId))
            {
                sb.AppendLine("订单编号不能为空;");
            }

            if (string.IsNullOrEmpty(msg.MsgData.OrderName))
            {
                sb.AppendLine("商品名称不能为空;");
            }

            if (!ValidateHelper.IsMobile(msg.ToAddress))
            {
                sb.AppendLine("手机号验证失败;");

            }

            if (DateTime.MinValue.Equals(msg.MsgData.OrderDate))
            {
                sb.AppendLine("订单日期验证失败;");
            }

            msg.Remark = sb.ToString();

            return true;
        }
    }
}
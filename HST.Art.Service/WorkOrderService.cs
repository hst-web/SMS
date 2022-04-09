/*----------------------------------------------------------------
// Copyright (C) 2015 可为天下（北京）科技有限公司 版权所有。
// 文件名：WorkOrderService.cs
// 功能描述：工单服务
// 创建者：huangyz
// 创建时间：2015-10-2
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using HST.Art.Core;
using System.Data;
using HST.Utillity;

namespace Canve.ESH.Service
{
    /// <summary>
    /// 工单服务
    /// </summary>
    public class WorkOrderService : ServiceBase, IWorkOrderService
    {
        IWorkOrderProvider _workOrderProvider;
        IWorkOrderWFE _workFlowEngine;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderService(IWorkOrderProvider workOrderProvider, IWorkOrderWFE workOrderWFE)
        {
            _workOrderProvider = workOrderProvider;
            _workFlowEngine = workOrderWFE;

            AddDisposableObject(workOrderProvider);
            AddDisposableObject(workOrderWFE);
        }

        #region 工单查询
        /// <summary>
        /// 根据条件获取全部工单
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>工单信息集合</returns>
        public List<WorkOrder> GetAll(Condition condition)
        {
            //参数验证
            if (condition == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            List<WorkOrder> workOrders = null;
            List<DataEntity> workOrderEntityList = _workOrderProvider.GetAll(condition);
            if (workOrderEntityList != null && workOrderEntityList.Count > 0)
            {
                workOrders = new List<WorkOrder>();
                WorkOrder workOrder = null;
                foreach (DataEntity workOrderEntity in workOrderEntityList)
                {
                    workOrder = GetWorkOrderFromEntity(workOrderEntity);
                    workOrders.Add(workOrder);
                }
            }

            return workOrders;
        }

        /// <summary>
        /// 获取一页工单信息集合
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="totalNum">总记录数</param>
        /// <returns>工单信息集合</returns>
        public List<WorkOrder> GetPage(Condition condition, out int totalNum)
        {
            totalNum = 0;
            //参数验证
            if (condition == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            List<WorkOrder> workOrders = null;
            List<DataEntity> workOrderEntityList = _workOrderProvider.GetPage(condition, out totalNum);
            if (workOrderEntityList != null && workOrderEntityList.Count > 0)
            {
                workOrders = new List<WorkOrder>();
                WorkOrder workOrder = null;
                foreach (DataEntity workOrderEntity in workOrderEntityList)
                {
                    workOrder = GetWorkOrderFromEntity(workOrderEntity);
                    workOrders.Add(workOrder);
                }
            }

            return workOrders;
        }

        /// <summary>
        /// 获取待处理工单数据
        /// </summary>
        /// <param name="condition">筛选条件</param>
        /// <returns></returns>
        public List<WorkOrder> GetPend(Condition condition)
        {
            //参数验证
            if (condition == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            List<WorkOrder> workOrders = null;
            List<DataEntity> workOrderEntityList = _workOrderProvider.GetPend(condition);
            if (workOrderEntityList != null && workOrderEntityList.Count > 0)
            {
                workOrders = new List<WorkOrder>();

                foreach (DataEntity workOrderEntity in workOrderEntityList)
                {
                    WorkOrder workOrder = new WorkOrder();
                    workOrder.ID = workOrderEntity["WorkOrderID"].Value.ToString();
                    workOrder.Number = workOrderEntity["WorkOrderNumber"].Value.ToString();
                    workOrder.ServiceSpaceID = workOrderEntity["ServiceSpaceID"].Value.ToString();
                    workOrder.ServiceNetworkID = workOrderEntity["ServiceNetworkID"].Value.ToString();
                    workOrder.ServiceCategoryID = workOrderEntity["ServiceCategoryID"].Value.ToString();
                    workOrder.ServiceCategoryName = workOrderEntity["ServiceCategoryName"].Value.ToString();
                    workOrder.ServiceModeID = workOrderEntity["ServiceModeID"].Value.ToString();
                    workOrder.ServiceModeName = workOrderEntity["ServiceModeName"].Value.ToString();
                    workOrder.ServiceStaffID = workOrderEntity["ServiceStaffID"].Value.ToString();
                    workOrder.ProductID = workOrderEntity["ProductID"].Value.ToString();
                    workOrder.ProductName = workOrderEntity["ProductName"].Value.ToString();
                    workOrder.ProductType = workOrderEntity["ProductType"].Value.ToString();
                    workOrder.CustomerName = workOrderEntity["CustomerName"].Value.ToString();
                    workOrder.CustomerPhone = workOrderEntity["CustomerPhone"].Value.ToString();
                    workOrder.IsReaded = (bool)workOrderEntity["IsReaded"].Value;
                    workOrder.State = (WorkOrderState)workOrderEntity["State"].Value;
                    workOrder.PendState = (WorkOrderPendState)workOrderEntity["PendState"].Value;
                    workOrder.CreateTime = workOrderEntity["CreateTime"].Value.ToString();
                    workOrders.Add(workOrder);
                }
            }

            return workOrders;
        }


        /// <summary>
        /// 获取一个工单信息
        /// </summary>
        /// <param name="id">工单id</param>
        /// <returns></returns>
        public WorkOrder Get(string id)
        {
            //参数验证
            if (string.IsNullOrEmpty(id))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            #region 数据获取
            WorkOrder workOrder = null;
            DataEntity workOrderEntity = _workOrderProvider.Get(id);

            if (workOrderEntity != null)
            {
                workOrder = GetWorkOrderFromEntity(workOrderEntity);
            }
            #endregion

            return workOrder;
        }

        /// <summary>
        /// 根据检索条件获取记录总数
        /// </summary>
        /// <param name="condition">检索条件</param>
        /// <returns>记录总数</returns>
        public int GetCount(Condition condition)
        {
            //参数验证
            if (condition == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return 0;
            }
            return _workOrderProvider.GetCount(condition);
        }

        /// <summary>
        /// 获取处理动作工单
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <param name="serviceNetworkId">服务网点id</param>
        /// <param name="staffId">员工id</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public List<WorkOrder> GetProWork(string serviceSpaceId, string serviceNetworkId, string staffId, string startDate, string endDate)
        {
            Condition conPro = new Condition() { Where = "{'servicenetworkid':'" + serviceNetworkId + "','staffid':'" + staffId + "','startdate':'" + startDate + "','enddate':'" + endDate + "'}" };

            List<WorkOrder> workOrders = new List<WorkOrder>();      
            List<DataEntity> workOrderEntityList = _workOrderProvider.GetProWork(conPro);
            if (workOrderEntityList != null && workOrderEntityList.Count > 0)
            {
                WorkOrder workOrder = null;
                foreach (DataEntity workOrderEntity in workOrderEntityList)
                {
                    workOrder = GetWorkOrderFromEntity(workOrderEntity);
                    workOrders.Add(workOrder);
                }
            }

            return workOrders;
        }


        /// <summary>
        /// 根据条件获取动作记录数（不重复）
        /// </summary>
        /// <param name="condition">检索条件</param>
        /// <returns>记录总数</returns>
        public int GetProcessCount(Condition condition)
        {
            //参数验证
            if (condition == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return 0;
            }
            return _workOrderProvider.GetProcessCount(condition);
        }

        /// <summary>
        /// 获取工单处理记录
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <returns>工单处理记录集合</returns>
        public List<WorkOrderProcess> GetProceses(string workOrderId)
        {
            //参数验证
            if (string.IsNullOrEmpty(workOrderId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }
            #region 获取数据
            List<WorkOrderProcess> workOrderProcesses = new List<WorkOrderProcess>();

            List<DataEntity> workOrderProcessEntityList = _workOrderProvider.GetProceses(workOrderId);
            if (workOrderProcessEntityList != null && workOrderProcessEntityList.Count > 0)
            {
                WorkOrderProcess workOrderProcess = null;
                foreach (DataEntity workOrderProcessEntity in workOrderProcessEntityList)
                {
                    workOrderProcess = new WorkOrderProcess();
                    workOrderProcess.ID = workOrderProcessEntity["ID"].Value.ToString();
                    workOrderProcess.Action = (WorkOrderAction)workOrderProcessEntity["Action"].Value;
                    workOrderProcess.ServiceNetworkID = workOrderProcessEntity["ServiceNetworkID"].Value.ToString();
                    workOrderProcess.ServiceNetworkName = workOrderProcessEntity["ServiceNetworkName"].Value.ToString();
                    workOrderProcess.StaffID = workOrderProcessEntity["StaffID"].Value.ToString();
                    workOrderProcess.StaffName = workOrderProcessEntity["StaffName"].Value.ToString();
                    workOrderProcess.NextHandlerID = workOrderProcessEntity["NextHandlerID"].Value.ToString();
                    workOrderProcess.NextHandlerName = workOrderProcessEntity["NextHandlerName"].Value.ToString();
                    workOrderProcess.Description = workOrderProcessEntity["Description"].Value.ToString();
                    workOrderProcess.HandedTime = workOrderProcessEntity["HandedTime"].Value.ToString();
                    workOrderProcess.Remark = workOrderProcessEntity["Remark"].Value.ToString();
                    workOrderProcess.WorkOrderID = workOrderProcessEntity["WorkOrderID"].Value.ToString();
                    workOrderProcess.IsTimeOut = (bool)workOrderProcessEntity["IsTimeOut"].Value;
                    workOrderProcess.ServiceEndTime = workOrderProcessEntity["ServiceEndTime"].Value.ToString();
                    workOrderProcess.HandleName = workOrderProcessEntity["HandleName"].Value.ToString();
                    workOrderProcesses.Add(workOrderProcess);
                }
            }
            #endregion

            return workOrderProcesses;
        }

        /// <summary>
        /// 获取工单实体从DataEntity
        /// </summary>
        /// <param name="workOrderEntity">数据实体</param>
        /// <returns>工单实体</returns>
        private WorkOrder GetWorkOrderFromEntity(DataEntity workOrderEntity)
        {
            WorkOrder workOrder = new WorkOrder();
            workOrder.AbnormalMsg = workOrderEntity["AbnormalMsg"].Value.ToString();
            workOrder.AbnormalTime = workOrderEntity["AbnormalTime"].Value.ToString();
            workOrder.AcceptTime = workOrderEntity["AcceptTime"].Value.ToString();
            workOrder.AssignTime = workOrderEntity["AssignTime"].Value.ToString();
            workOrder.CallCount = (int)workOrderEntity["CallCount"].Value;
            workOrder.Caption = workOrderEntity["Caption"].Value.ToString();
            workOrder.Channel = (int)workOrderEntity["Channel"].Value;
            workOrder.Contact = workOrderEntity["Contact"].Value.ToString();
            workOrder.ContactNumber = workOrderEntity["ContactNumber"].Value.ToString();
            workOrder.CreateTime = workOrderEntity["CreateTime"].Value.ToString();
            workOrder.CurrentHandlerID = workOrderEntity["CurrentHandlerID"].Value.ToString();
            workOrder.CurrentHandlerName = workOrderEntity["CurrentHandlerName"].Value.ToString();
            workOrder.CustomerID = workOrderEntity["CustomerID"].Value.ToString();
            workOrder.CustomerAddress = workOrderEntity["CustomerAddress"].Value.ToString();
            workOrder.CustomerName = workOrderEntity["CustomerName"].Value.ToString();
            workOrder.CustomFields = workOrderEntity["CustomFields"].Value.ToString();
            workOrder.CustomerPhone = workOrderEntity["CustomerPhone"].Value.ToString();
            workOrder.Description = workOrderEntity["Description"].Value.ToString();
            workOrder.DispatchTime = workOrderEntity["DispatchTime"].Value.ToString();
            workOrder.FaultCategoryID = workOrderEntity["FaultCategoryID"].Value.ToString();
            workOrder.FaultCategoryName = workOrderEntity["FaultCategoryName"].Value.ToString();
            workOrder.FinishTime = workOrderEntity["FinishTime"].Value.ToString();
            workOrder.GuaranteedState = (GuaranteedState)workOrderEntity["GuaranteedState"].Value;
            workOrder.ID = workOrderEntity["ID"].Value.ToString();
            workOrder.IsDeleted = (bool)workOrderEntity["IsDeleted"].Value;
            workOrder.IsEvaluated = (bool)workOrderEntity["IsEvaluated"].Value;
            workOrder.NextHandlerID = workOrderEntity["NextHandlerID"].Value.ToString();
            workOrder.Number = workOrderEntity["Number"].Value.ToString();
            workOrder.NeedStatemented = (bool)workOrderEntity["NeedStatemented"].Value;
            workOrder.OriginalNetworkID = workOrderEntity["OriginalNetworkID"].Value.ToString();
            workOrder.OriginalNetworkName = workOrderEntity["OriginalNetworkName"].Value.ToString();
            workOrder.Priority = (WorkOrderPriority)workOrderEntity["Priority"].Value;
            workOrder.ProcessDesc = workOrderEntity["ProcessDesc"].Value.ToString();
            workOrder.ProductID = workOrderEntity["ProductID"].Value.ToString();
            workOrder.ProductType = workOrderEntity["ProductType"].Value.ToString();
            workOrder.ProductName = workOrderEntity["ProductName"].Value.ToString();
            workOrder.ProductCategoryID = workOrderEntity["ProductCategoryID"].Value.ToString();
            workOrder.ReportTime = workOrderEntity["ReportTime"].Value.ToString();
            workOrder.ReceivedTime = workOrderEntity["ReceivedTime"].Value.ToString();
            workOrder.ReturnVisitTime = workOrderEntity["ReturnVisitTime"].Value.ToString();
            workOrder.ServiceSpaceID = workOrderEntity["ServiceSpaceID"].Value.ToString();
            workOrder.ServiceSpaceName = workOrderEntity["ServiceSpaceName"].Value.ToString();
            workOrder.ServiceCategoryID = workOrderEntity["ServiceCategoryID"].Value.ToString();
            workOrder.ServiceCategoryName = workOrderEntity["ServiceCategoryName"].Value.ToString();
            workOrder.ServiceNetworkID = workOrderEntity["ServiceNetworkID"].Value.ToString();
            workOrder.ServiceNetworkName = workOrderEntity["ServiceNetworkName"].Value.ToString();
            workOrder.ServiceModeID = workOrderEntity["ServiceModeID"].Value.ToString();
            workOrder.ServiceModeName = workOrderEntity["ServiceModeName"].Value.ToString();
            workOrder.ServiceStaffID = workOrderEntity["ServiceStaffID"].Value.ToString();
            workOrder.ServiceStaffName = workOrderEntity["ServiceStaffName"].Value.ToString();
            workOrder.Source = (WorkOrderSource)workOrderEntity["Source"].Value;
            workOrder.State = (WorkOrderState)workOrderEntity["State"].Value;
            workOrder.PendState = (WorkOrderPendState)workOrderEntity["PendState"].Value;
            workOrder.PrevOperation = SerializationHelper.JsonDeserialize<PreviousOperation>(workOrderEntity["PrevOperation"].Value.ToString());
            workOrder.StatementState = (StatementState)workOrderEntity["StatementState"].Value;
            workOrder.Signature = workOrderEntity["Signature"].Value.ToString();
            workOrder.SubscribeTime = workOrderEntity["SubscribeTime"].Value.ToString();
            workOrder.SubscribeServiceTime = workOrderEntity["SubscribeServiceTime"].Value.ToString();
            workOrder.StartServiceTime = workOrderEntity["StartServiceTime"].Value.ToString();
            workOrder.CancelTime = workOrderEntity["CancelTime"].Value.ToString();
            workOrder.QualityCheckTime = workOrderEntity["QualityCheckTime"].Value.ToString();
            workOrder.StatementTime = workOrderEntity["StatementTime"].Value.ToString();
            workOrder.Remark = workOrderEntity["Remark"].Value.ToString();
            workOrder.IsServiceEnd = (bool)workOrderEntity["IsServiceEnd"].Value;
            workOrder.IsHasReport = (bool)workOrderEntity["IsHasReport"].Value;
            workOrder.IsSubmitImg=(bool)workOrderEntity["IsSubmitImg"].Value;
            workOrder.IsCustomerEvaluate = (bool)workOrderEntity["IsCustomerEvaluate"].Value;
            workOrder.CustomerArea = workOrderEntity["CustomerArea"].Value.ToString();
            workOrder.ProductCategoryName = workOrderEntity["ProductCategoryName"].Value.ToString();
            
            return workOrder;
        }
        #endregion

        #region 工单操作
        /// <summary>
        /// 处理工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="action">处理动作</param>
        /// <param name="additionalData">辅助数据</param>
        /// <returns></returns>
        public bool Process(WorkOrder workOrder, WorkOrderAction action, object additionalData = null)
        {
            bool isSuccessed = false;
            isSuccessed = _workFlowEngine.Excute(workOrder, action, additionalData);
            return isSuccessed;
        }

        /// <summary>
        /// 获取工单下一个处理动作
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <returns></returns>
        public WorkOrderAction GetWorkNextAction(WorkOrder workOrder)
        {
            return _workFlowEngine.GetWorkNextAction(workOrder);
        }

        /// <summary>
        /// 修改工单客户签名
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <param name="signature">签名</param>
        /// <returns></returns>
        public bool UpdateSignature(string workOrderId, string signature)
        {
            //参数验证
            if (string.IsNullOrEmpty(workOrderId) || string.IsNullOrEmpty(signature))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            DataEntity workOrderEntity = _workOrderProvider.Get(workOrderId);
            workOrderEntity["Signature"].Value = string.IsNullOrEmpty(signature) ? "" : signature;

            return _workOrderProvider.Update(workOrderEntity);
        }

        /// <summary>
        /// 删除工单(仅用用客户端逻辑删除)
        /// </summary>
        /// <param name="workorderId">工单id</param>
        /// <returns></returns>
        public bool Delete(string workorderId)
        {
            //参数验证
            if (string.IsNullOrEmpty(workorderId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            } //参数验证

            return _workOrderProvider.Delete(workorderId);
        }

        /// <summary>
        /// 改变工单的已读状态
        /// </summary>
        /// <param name="staffId">员工id</param>
        /// <param name="workOrderId">工单id</param>
        /// <param name="state">工单状态</param>
        /// <returns></returns>
        public bool ChangeReadState(string staffId, string workOrderId, int state)
        {
            return _workOrderProvider.ChangeReadState(staffId, workOrderId, state);
        }
        #endregion

        #region 签到信息
        /// <summary>
        /// 获取工单id、服务人员id的签到信息集合
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <param name="staffId">服务人员id</param>
        /// <returns>签到信息集合</returns>
        public List<SignIn> GetSignIns(string workOrderId, string staffId)
        {
            //参数验证
            if (string.IsNullOrEmpty(workOrderId) || string.IsNullOrEmpty(staffId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            #region 获取数据
            List<SignIn> signIns = new List<SignIn>();

            List<DataEntity> signInEntityList = _workOrderProvider.GetSignIns(workOrderId, staffId);
            if (signInEntityList != null && signInEntityList.Count > 0)
            {
                SignIn signIn = null;
                foreach (DataEntity signInEntity in signInEntityList)
                {
                    signIn = new SignIn();
                    signIn.ID = signInEntity["ID"].Value.ToString();
                    signIn.WorkOrderID = signInEntity["WorkOrderID"].Value.ToString();
                    signIn.Address = signInEntity["Address"].Value.ToString();
                    signIn.Title = signInEntity["Title"].Value.ToString();
                    signIn.StaffID = signInEntity["StaffID"].Value.ToString();
                    signIn.CreateTime = signInEntity["CreateTime"].Value.ToString();
                    signIn.WorkProcessID = signInEntity["WorkProcessID"].Value.ToString();
                    signIns.Add(signIn);
                }
            }
            #endregion

            return signIns;
        }

        /// <summary>
        /// 添加签到
        /// </summary>
        /// <param name="signIn">签到信息</param>
        /// <returns></returns>
        public bool AddSignIn(SignIn signIn)
        {
            //参数验证
            if (signIn == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            DataEntity signInEntity = new DataEntity();
            signInEntity.Add("ID", string.IsNullOrEmpty(signIn.ID) ? "" : signIn.ID);
            signInEntity.Add("WorkOrderID", string.IsNullOrEmpty(signIn.WorkOrderID) ? "" : signIn.WorkOrderID);
            signInEntity.Add("Address", string.IsNullOrEmpty(signIn.Address) ? "" : signIn.Address);
            signInEntity.Add("StaffID", string.IsNullOrEmpty(signIn.StaffID) ? "" : signIn.StaffID);
            signInEntity.Add("Title", string.IsNullOrEmpty(signIn.Title) ? "" : signIn.Title);
            signInEntity.Add("WorkProcessID", string.IsNullOrEmpty(signIn.WorkProcessID) ? "" : signIn.WorkProcessID);
            return _workOrderProvider.AddSignIn(signInEntity);
        }
        #endregion

        #region 回访查询
        /// <summary>
        /// 根据工单id获取回访信息
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <returns>回访信息</returns>
        public ReturnVisit GetReturnVisit(string workOrderId)
        {
            //参数验证
            if (string.IsNullOrEmpty(workOrderId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            ReturnVisit returnVisit = null;
            DataEntity returnVisitEntity = _workOrderProvider.GetReturnVisit(workOrderId);
            if (returnVisitEntity != null)
            {
                returnVisit = new ReturnVisit();
                returnVisit.ID = returnVisitEntity["ID"].Value.ToString();
                returnVisit.WorkOrderID = returnVisitEntity["WorkOrderID"].Value.ToString();
                returnVisit.Description = returnVisitEntity["Description"].Value.ToString();
                returnVisit.Evalution = (int)returnVisitEntity["Evalution"].Value;
                returnVisit.IsFeeChecked = (bool)returnVisitEntity["IsFeeChecked"].Value;
                returnVisit.IsResolved = (bool)returnVisitEntity["IsResolved"].Value;
                returnVisit.CreateTime = returnVisitEntity["CreateTime"].Value.ToString();
                returnVisit.OperatorID = returnVisitEntity["OperatorID"].Value.ToString();
                returnVisit.OperatorName = returnVisitEntity["OperatorName"].Value.ToString();
            }

            return returnVisit;
        }

        #endregion

        #region 质检
        /// <summary>
        /// 获取质检信息
        /// </summary>
        /// <param name="workProcessId">工单处理流程id</param>
        /// <returns></returns>
        public QualityCheck GetQualityCheck(string workProcessId)
        {
            //参数验证
            if (string.IsNullOrEmpty(workProcessId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            QualityCheck qualityCheck = null;
            DataEntity qualityCheckEntity = _workOrderProvider.GetQualityCheck(workProcessId);
            if (qualityCheckEntity != null)
            {
                qualityCheck = new QualityCheck();
                qualityCheck.ID = qualityCheckEntity["ID"].Value.ToString();
                qualityCheck.WorkOrderID = qualityCheckEntity["WorkOrderID"].Value.ToString();
                qualityCheck.WorkProcessID = qualityCheckEntity["WorkProcessID"].Value.ToString();
                qualityCheck.IsQualified = (bool)qualityCheckEntity["IsQualified"].Value;
                qualityCheck.CreateTime = qualityCheckEntity["CreateTime"].Value.ToString();
                qualityCheck.OperatorID = qualityCheckEntity["OperatorID"].Value.ToString();
                qualityCheck.OperatorName = qualityCheckEntity["OperatorName"].Value.ToString();
                qualityCheck.Remark = qualityCheckEntity["Remark"].Value.ToString();
            }

            return qualityCheck;
        }

        /// <summary>
        /// 获取质检信息集合
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <returns></returns>
        public List<QualityCheck> GetQualityChecks(string workOrderId)
        {
            //参数验证
            if (string.IsNullOrEmpty(workOrderId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }
            List<QualityCheck> qualityCheckList = null;
            List<DataEntity> qualityCheckEntityList = _workOrderProvider.GetQualityChecks(workOrderId);
            if (qualityCheckEntityList != null && qualityCheckEntityList.Count > 0)
            {
                qualityCheckList = new List<QualityCheck>();
                foreach (DataEntity qualityCheckEntity in qualityCheckEntityList)
                {
                    QualityCheck qualityCheck = new QualityCheck();
                    qualityCheck.ID = qualityCheckEntity["ID"].Value.ToString();
                    qualityCheck.WorkOrderID = qualityCheckEntity["WorkOrderID"].Value.ToString();
                    qualityCheck.WorkProcessID = qualityCheckEntity["WorkProcessID"].Value.ToString();
                    qualityCheck.IsQualified = (bool)qualityCheckEntity["IsQualified"].Value;
                    qualityCheck.CreateTime = qualityCheckEntity["CreateTime"].Value.ToString();
                    qualityCheck.OperatorID = qualityCheckEntity["OperatorID"].Value.ToString();
                    qualityCheck.OperatorName = qualityCheckEntity["OperatorName"].Value.ToString();
                    qualityCheck.Remark = qualityCheckEntity["Remark"].Value.ToString();

                    qualityCheckList.Add(qualityCheck);
                }

            }

            return qualityCheckList;
        }

        #endregion

        #region 获取工单图片
        /// <summary>
        /// 获取回单图片
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <returns></returns>
        public List<WorkOrderImg> GetReportImgs(string workOrderId)
        {
            //参数验证
            if (string.IsNullOrEmpty(workOrderId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }
            List<WorkOrderImg> workImgList = null;
            List<DataEntity> workImgEntityList = _workOrderProvider.GetWorkOrderImgs(workOrderId, WorkOrderImgType.After);
            if (workImgEntityList != null && workImgEntityList.Count > 0)
            {
                workImgList = new List<WorkOrderImg>();
                foreach (DataEntity item in workImgEntityList)
                {
                    WorkOrderImg workImg = new WorkOrderImg();
                    workImg.ID = item["ID"].Value.ToString();
                    workImg.WorkOrderID = item["WorkOrderID"].Value.ToString();
                    workImg.Name = item["Name"].Value.ToString();
                    workImg.Type = (WorkOrderImgType)item["Type"].Value;
                    workImg.Path = item["Path"].Value.ToString();
                    workImgList.Add(workImg);
                }

            }

            return workImgList;
        }

        /// <summary>
        /// 获取提单图片
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <returns></returns>
        public List<WorkOrderImg> GetSubmitImgs(string workOrderId)
        {
            //参数验证
            if (string.IsNullOrEmpty(workOrderId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }
            List<WorkOrderImg> workImgList = null;
            List<DataEntity> workImgEntityList = _workOrderProvider.GetWorkOrderImgs(workOrderId, WorkOrderImgType.Submit);
            if (workImgEntityList != null && workImgEntityList.Count > 0)
            {
                workImgList = new List<WorkOrderImg>();
                foreach (DataEntity item in workImgEntityList)
                {
                    WorkOrderImg workImg = new WorkOrderImg();
                    workImg.ID = item["ID"].Value.ToString();
                    workImg.WorkOrderID = item["WorkOrderID"].Value.ToString();
                    workImg.Name = item["Name"].Value.ToString();
                    workImg.Type = (WorkOrderImgType)item["Type"].Value;
                    workImg.Path = item["Path"].Value.ToString();
                    workImgList.Add(workImg);
                }

            }

            return workImgList;
        }
        #endregion

        #region 工单评价
        /// <summary>
        /// 添加评价
        /// </summary>
        /// <param name="workOrderEvalution">评价信息</param>
        /// <returns></returns>
        public bool AddEvaluation(WorkOrderEvaluation workOrderEvalution)
        {
            if (workOrderEvalution == null)
            {
                return false;
            }

            DataEntity workOrderEvalutionDataEntity = new DataEntity();
            workOrderEvalutionDataEntity.Add("WorkOrderID", string.IsNullOrEmpty(workOrderEvalution.WorkOrderID) ? "" : workOrderEvalution.WorkOrderID);
            workOrderEvalutionDataEntity.Add("OverallLevel", workOrderEvalution.OverallLevel);
            workOrderEvalutionDataEntity.Add("Lables", workOrderEvalution.Lables != null ? SerializationHelper.JsonSerialize(workOrderEvalution.Lables) : "");
            workOrderEvalutionDataEntity.Add("Evaluation", string.IsNullOrEmpty(workOrderEvalution.Evaluation) ? "" : workOrderEvalution.Evaluation);
            workOrderEvalutionDataEntity.Add("CustomerName", string.IsNullOrEmpty(workOrderEvalution.CustomerName) ? "" : workOrderEvalution.CustomerName);
            workOrderEvalutionDataEntity.Add("CustomerID", string.IsNullOrEmpty(workOrderEvalution.CustomerID) ? "" : workOrderEvalution.CustomerID);
            workOrderEvalutionDataEntity.Add("IsCustomerEvaluate",workOrderEvalution.IsCustomerEvaluate);
            return _workOrderProvider.AddEvaluation(workOrderEvalutionDataEntity);
        }

        /// <summary>
        /// 根据工单获取评价信息
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <returns></returns>
        public WorkOrderEvaluation GetEvaluation(string workOrderId)
        {
            if (string.IsNullOrEmpty(workOrderId))
            {
                return null;
            }
            WorkOrderEvaluation evalution = null;
            DataEntity evalutionDataEntity = _workOrderProvider.GetEvaluation(workOrderId);
            if (evalutionDataEntity != null)
            {
                evalution = new WorkOrderEvaluation();
                evalution.ID = evalutionDataEntity["ID"].Value.ToString();
                evalution.WorkOrderID = evalutionDataEntity["WorkOrderID"].Value.ToString();
                evalution.CustomerID = evalutionDataEntity["CustomerID"].Value.ToString();
                evalution.CustomerName = evalutionDataEntity["CustomerName"].Value.ToString();
                evalution.Evaluation = evalutionDataEntity["Evaluation"].Value.ToString();
                evalution.OverallLevel = Convert.ToInt32(evalutionDataEntity["OverallLevel"].Value);
                evalution.CreateTime = evalutionDataEntity["CreateTime"].Value.ToString();
            }

            return evalution;
        }

      

        #endregion
    }
}
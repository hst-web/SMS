/*----------------------------------------------------------------
// 文件名：CustomerService.cs
// 功能描述：客户服务
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using HST.Art.Core;
using System;
using HST.Utillity;
using HST.Art.Data;

namespace HST.Art.Service
{
    /// <summary>
    /// 客户服务
    /// </summary>
    public class CustomerService : ServiceBase
    {
        CustomerProvider _customerProvider=new CustomerProvider();

        

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomerService(CustomerProvider customerProvider)
        {
            _customerProvider = customerProvider;
            AddDisposableObject(customerProvider);
        }

        /// <summary>
        /// 获取根据条件获取所有客户集合
        /// </summary>
        /// <param name="organizationId">服务空间id</param>
        /// <param name="condition">条件</param>
        /// <returns>客户集合</returns>
        public List<Customer> GetAll(string serviceSpaceId, Condition condition)
        {
            List<Customer> customers = null;
            List<DataEntity> customerEntityList = _customerProvider.GetAll(serviceSpaceId, condition);
            if (customerEntityList != null && customerEntityList.Count > 0)
            {
                customers = new List<Customer>();
                Customer customer = null;
                foreach (DataEntity customerEntity in customerEntityList)
                {
                    customer = GetCustomerFromEntity(customerEntity);
                    customers.Add(customer);
                }
            }

            return customers;
        }

        /// <summary>
        /// 获取根据条件分页获取客户集合
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <param name="condition">条件</param>
        /// <param name="totalNum">总记录数</param>
        /// <returns>客户集合</returns>
        public List<Customer> GetPage(string serviceSpaceId, Condition condition, out int totalNum)
        {
            totalNum = 0;
            //获取数据
            List<Customer> customers = null;
            List<DataEntity> customerEntityList = _customerProvider.GetPage(serviceSpaceId, condition, out totalNum);
            if (customerEntityList != null && customerEntityList.Count > 0)
            {
                customers = new List<Customer>();
                Customer customer = null;
                foreach (DataEntity customerEntity in customerEntityList)
                {
                    customer = GetCustomerFromEntity(customerEntity);
                    customers.Add(customer);
                }
            }

            return customers;
        }

        /// <summary>
        /// 根据客户名称获取客户集合
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <param name="name">客户名称</param>
        /// <param name="totalNum">记录条数</param>
        /// <returns>客户集合</returns>
        public List<Customer> GetAllByName(string serviceSpaceId, string name, out int totalNum)
        {
            totalNum = 0;
            //参数验证
            if (string.IsNullOrEmpty(name))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            List<Customer> customers = null;
            List<DataEntity> customerEntityList = _customerProvider.GetAllByName(serviceSpaceId, name, out totalNum);
            if (customerEntityList != null && customerEntityList.Count > 0)
            {
                customers = new List<Customer>();
                Customer customer = null;
                foreach (DataEntity customerEntity in customerEntityList)
                {
                    customer = GetCustomerFromEntity(customerEntity);
                    customers.Add(customer);
                }
            }

            return customers;
        }

        /// <summary>
        /// 根据手机号、服务空间id获取客户信息
        /// </summary>
        /// <param name="serviceSpaceId">服务空间Id</param>
        /// <param name="phoneNum">手机号</param>
        /// <returns>客户信息</returns>
        public Customer GetByPhone(string serviceSpaceId, string phoneNum)
        {
            //参数验证
            if (string.IsNullOrEmpty(phoneNum) || string.IsNullOrEmpty(serviceSpaceId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            Customer customer = null;
            DataEntity customerEntity = _customerProvider.GetByPhone(serviceSpaceId, phoneNum);

            if (customerEntity != null)
            {
                customer = GetCustomerFromEntity(customerEntity);
            }
            return customer;
        }

        /// <summary>
        /// 获取一个客户信息
        /// </summary>
        /// <param name="id">客户id</param>
        /// <returns></returns>
        public Customer Get(string id)
        {
            //参数验证
            if (string.IsNullOrEmpty(id))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            Customer customer = null;
            DataEntity customerEntity = _customerProvider.Get(id);

            if (customerEntity != null)
            {
                customer = GetCustomerFromEntity(customerEntity);
            }
            return customer;
        }

        /// <summary>
        /// 根据客户微信openId获取客户信息
        /// </summary>
        /// <param name="OpenId">客户微信openId</param>
        /// <returns>客户信息</returns>
        public Customer GetByOpenId(string serviceSpaceId, string OpenId)
        {
            //参数验证
            if (string.IsNullOrEmpty(OpenId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            Customer customer = null;
            DataEntity customerEntity = _customerProvider.GetByOpenId(serviceSpaceId, OpenId);

            if (customerEntity != null)
            {
                customer = GetCustomerFromEntity(customerEntity);
            }
            return customer;
        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="customerIds">客户id集合</param>
        /// <returns></returns>
        public bool Delete(List<string> customerIds)
        {
            if (customerIds == null || customerIds.Count == 0)
            {
                return false;
            }

            return _customerProvider.Delete(customerIds);
        }

        /// <summary>
        /// 保存客户信息
        /// </summary>
        /// <param name="customer">客户模型</param>
        /// <returns></returns>
        public bool Save(Customer customer)
        {
            //参数验证
            if (customer == null || string.IsNullOrEmpty(customer.Mobile) || string.IsNullOrEmpty(customer.ServiceSpaceID))
            {
                return false;
            }

            //客户是否存在
            DataEntity customerEntity = null;
            if (!string.IsNullOrEmpty(customer.ID))
            {
                customerEntity = _customerProvider.Get(customer.ID);
            }
            if (customerEntity == null)
            {
                customer.ID = System.Guid.NewGuid().ToString();
                customerEntity = new DataEntity();
                customerEntity.Add("ID", customer.ID);
                customerEntity.Add("ServiceSpaceID", string.IsNullOrEmpty(customer.ServiceSpaceID) ? "" : customer.ServiceSpaceID);
                customerEntity.Add("Address", string.IsNullOrEmpty(customer.Address) ? "" : customer.Address);
                customerEntity.Add("Area", string.IsNullOrEmpty(customer.Area) ? "" : customer.Area);
                customerEntity.Add("Remark", string.IsNullOrEmpty(customer.Remark) ? "" : customer.Remark);
                customerEntity.Add("CompanyName", string.IsNullOrEmpty(customer.CompanyName) ? "" : customer.CompanyName);
                customerEntity.Add("Gender", (int)customer.Gender);
                customerEntity.Add("Mail", string.IsNullOrEmpty(customer.Mail) ? "" : customer.Mail);
                customerEntity.Add("Mobile", string.IsNullOrEmpty(customer.Mobile) ? "" : customer.Mobile);
                customerEntity.Add("Name", string.IsNullOrEmpty(customer.Name) ? "" : customer.Name);
                customerEntity.Add("UserName", string.IsNullOrEmpty(customer.UserName) ? customer.Mobile : customer.UserName);
                customerEntity.Add("Password", string.IsNullOrEmpty(customer.Password) ? "" : customer.Password);
                customerEntity.Add("WeChat", string.IsNullOrEmpty(customer.WeChat) ? "" : customer.WeChat);
                customerEntity.Add("RecentService", string.IsNullOrEmpty(customer.RecentService) ? "" : customer.RecentService);
                customerEntity.Add("HeadImg", string.IsNullOrEmpty(customer.HeadImg) ? "" : customer.HeadImg);
                customerEntity.Add("Contact", string.IsNullOrEmpty(customer.Contact) ? "" : customer.Contact);
                customerEntity.Add("LabelIDs", string.IsNullOrEmpty(customer.LabelIDs) ? "" : customer.LabelIDs);
                customerEntity.Add("ServiceNetworkID", string.IsNullOrEmpty(customer.ServiceNetworkID) ? "" : customer.ServiceNetworkID);
                return _customerProvider.Add(customerEntity);
            }
            else
            {
                customerEntity["Address"].Value = string.IsNullOrEmpty(customer.Address) ? "" : customer.Address;
                customerEntity["Area"].Value = string.IsNullOrEmpty(customer.Area) ? "" : customer.Area;
                customerEntity["Remark"].Value = string.IsNullOrEmpty(customer.Remark) ? "" : customer.Remark;
                customerEntity["CompanyName"].Value = string.IsNullOrEmpty(customer.CompanyName) ? "" : customer.CompanyName;
                customerEntity["Gender"].Value = (int)customer.Gender;
                customerEntity["Mail"].Value = string.IsNullOrEmpty(customer.Mail) ? "" : customer.Mail;
                customerEntity["Mobile"].Value = string.IsNullOrEmpty(customer.Mobile) ? "" : customer.Mobile;
                customerEntity["Name"].Value = string.IsNullOrEmpty(customer.Name) ? "" : customer.Name;
                customerEntity["Password"].Value = string.IsNullOrEmpty(customer.Password) ? "" : customer.Password;
                customerEntity["WeChat"].Value = string.IsNullOrEmpty(customer.WeChat) ? "" : customer.WeChat;
                customerEntity["RecentService"].Value = string.IsNullOrEmpty(customer.RecentService) ? "" : customer.RecentService;
                customerEntity["HeadImg"].Value = string.IsNullOrEmpty(customer.HeadImg) ? "" : customer.HeadImg;
                customerEntity["Contact"].Value = string.IsNullOrEmpty(customer.Contact) ? "" : customer.Contact;
                customerEntity["LabelIDs"].Value = string.IsNullOrEmpty(customer.LabelIDs) ? "" : customer.LabelIDs;

                return _customerProvider.Update(customerEntity);
            }
        }

        /// <summary>
        /// 创建网点客户管理关系
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <param name="serviceNetworkId">服务网点id</param>
        /// <param name="customerId">客户id</param>
        /// <returns></returns>
        public bool CreateNetworkCustomer(string serviceSpaceId, string serviceNetworkId, string customerId)
        {
            //参数验证
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(serviceSpaceId) || string.IsNullOrEmpty(serviceNetworkId))
            {
                return false;
            }

            DataEntity networkCusEntity = new DataEntity();
            networkCusEntity.Add("ServiceSpaceID", serviceSpaceId);
            networkCusEntity.Add("ServiceNetworkID", serviceNetworkId);
            networkCusEntity.Add("CustomerID", customerId);

            return _customerProvider.AddNetworkCustomer(networkCusEntity);
        }

        /// <summary>
        /// 根据检索条件获取记录总数
        /// </summary>
        /// <param name="ServiceSpaceId">组织id</param>
        /// <param name="condition">检索条件</param>
        /// <returns>记录总数</returns>
        public int GetCount(string ServiceSpaceId, Condition condition)
        {
            return _customerProvider.GetCount(ServiceSpaceId, condition);
        }

        /// <summary>
        /// 从数据实体转换为业务实体
        /// </summary>
        /// <param name="customerEntity"></param>
        /// <returns></returns>
        private Customer GetCustomerFromEntity(DataEntity customerEntity)
        {
            Customer customer = new Customer();
            customer.ID = customerEntity["ID"].Value.ToString();
            customer.ServiceSpaceID = customerEntity["ServiceSpaceID"].Value.ToString();
            customer.Name = customerEntity["Name"].Value.ToString();
            customer.Gender = (Gender)customerEntity["Gender"].Value;
            customer.Mobile = customerEntity["Mobile"].Value.ToString();
            customer.Mail = customerEntity["Mail"].Value.ToString();
            customer.WeChat = customerEntity["WeChat"].Value.ToString();
            customer.HeadImg = customerEntity["HeadImg"].Value.ToString();
            customer.UserName = customerEntity["UserName"].Value.ToString();
            customer.Password = customerEntity["Password"].Value.ToString();
            customer.CompanyName = customerEntity["CompanyName"].Value.ToString();
            customer.Area = customerEntity["Area"].Value.ToString();
            customer.Address = customerEntity["Address"].Value.ToString();
            customer.CreateTime = customerEntity["CreateTime"].Value.ToString();
            customer.RecentService = customerEntity["RecentService"].Value.ToString();
            customer.Remark = customerEntity["Remark"].Value.ToString();
            customer.UpdateTime = customerEntity["UpdateTime"].Value.ToString();
            customer.Contact = string.IsNullOrEmpty(customerEntity["Contact"].Value.ToString()) ? customerEntity["Name"].Value.ToString() : customerEntity["Contact"].Value.ToString();
            customer.LabelIDs = customerEntity["LabelIDs"].Value.ToString();
            return customer;
        }

        #region 组织标签
        /// <summary>
        /// 获取所有组织标签
        /// </summary>
        /// <param name="serviceSpaceId">服务空间id</param>
        /// <returns></returns>
        public List<CustomerLabel> GetCustomerLabels(string serviceSpaceId)
        {
            List<CustomerLabel> labels = null;
            List<DataEntity> labelDataEntityList = _customerProvider.GetCustomerLabels(serviceSpaceId);
            if (labelDataEntityList != null && labelDataEntityList.Count > 0)
            {
                labels = new List<CustomerLabel>();
                foreach (DataEntity label in labelDataEntityList)
                {
                    labels.Add(new CustomerLabel()
                    {
                        ID = label["ID"].Value.ToString(),
                        Name = label["Name"].Value.ToString(),
                        ServiceSpaceID = label["ServiceSpaceId"].Value.ToString()
                    });
                }
            }

            return labels;
        }

        /// <summary>
        /// 获取客户标签
        /// </summary>
        /// <param name="id">标签id</param>
        /// <returns></returns>
        public CustomerLabel GetCustomerLabel(string id)
        {
            CustomerLabel label = null;
            DataEntity labelDataEntity = _customerProvider.GetCustomerLabel(id);
            if (labelDataEntity != null)
            {
                label = new CustomerLabel()
                {
                    ID = labelDataEntity["ID"].Value.ToString(),
                    Name = labelDataEntity["Name"].Value.ToString(),
                    ServiceSpaceID = labelDataEntity["ServiceSpaceId"].Value.ToString()
                };
            }
            return label;
        }

        /// <summary>
        /// 保存客户标签
        /// </summary>
        /// <param name="customerLabel">客户标签</param>
        /// <returns></returns>
        public bool SaveCustomerLabel(CustomerLabel customerLabel)
        {
            if (customerLabel == null || string.IsNullOrEmpty(customerLabel.ServiceSpaceID))
            {
                return false;
            }

            DataEntity labelDataEntity = new DataEntity();
            labelDataEntity.Add("ID", string.IsNullOrEmpty(customerLabel.ID) ? "" : customerLabel.ID);
            labelDataEntity.Add("Name", string.IsNullOrEmpty(customerLabel.Name) ? "" : customerLabel.Name);
            labelDataEntity.Add("ServiceSpaceId", string.IsNullOrEmpty(customerLabel.ServiceSpaceID) ? "" : customerLabel.ServiceSpaceID);
            bool isSuccessed = _customerProvider.SaveCustomerLabel(labelDataEntity);

            return isSuccessed;
        }

        /// <summary>
        /// 删除客户标签
        /// </summary>
        /// <param name="id">标签id</param>
        /// <returns></returns>
        public bool DeleteCustomerLabel(string id)
        {
            bool isSuccessed = _customerProvider.DeleteCustomerLabel(id);

            return isSuccessed;
        }
        #endregion
    }
}
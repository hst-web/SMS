using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace HST.Art.Web.Controllers
{
    public class ResPermissionController : ApplicationBase
    {
        // GET: ResPermission
        public ActionResult Index()
        {
            List<KeyValueViewModel> packges = new List<KeyValueViewModel>();
            List<ResourcePackage> packgeList = new ResourcePackageController().GetAll();

            if (packgeList != null && packgeList.Count > 0)
                packges = packgeList.Select(g => (new KeyValueViewModel() { Key = g.Id, Value = g.Name })).ToList();
            ViewBag.packges = packges;

            return View(new FilterViewModel());
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
        public PartialViewResult Get(FilterViewModel filter, string resName, int packgeId)
        {
            InitData();

            ResourceFilterModel fillter = new ResourceFilterModel()
            {
                Name = resName,
                PackageId = packgeId,
                pageNumber = filter.PageIndex,
                pageSize = filter.PageSize
            };


            ReturnPageResultIList<Resource> data = new cncbk_resource_application.Controller.ResourceController().GetAllWhere(fillter);
            IList<ResourceViewModel> gmList = new List<ResourceViewModel>();

            if (data != null && data.DataT != null)
            {               
                gmList = data.DataT.Select(g => new ResourceViewModel() { Id = g.Id, Title = g.Title, PackageName = g.ResourcePackage.Name, MemberTypes = GetMemberTypes(g.ResourceResourcePermissions) }).ToList();
            }

            PageListViewModel<ResourceViewModel> mpage = new PageListViewModel<ResourceViewModel>(gmList, filter.PageIndex, filter.PageSize, data.totalRecords);

            return PartialView(mpage);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        [HttpPost]
        public string Save(int id, string json)
        {
            if (id <= 0) return "error";

            Resource res = new Resource();
            res.Id = id;
            List<MemberTypeViewModel> mtv = JsonConvert.DeserializeObject<List<MemberTypeViewModel>>(json);
            if (mtv!=null)
                res.ResourceResourcePermissions = GetResPers(mtv, id);

            bool isSucessed = new cncbk_resource_application.Controller.ResourceController().UpdateResourcePermissions(res);
            return isSucessed ? "ok" : "error";
        }

        private List<MemberTypeViewModel> GetMemberTypes(List<ResourceResourcePermission> resPermissions)
        {
            List<MemberTypeViewModel> memberTypeList = new List<MemberTypeViewModel>();

            if (resPermissions == null || resPermissions.Count <= 0)
                return memberTypeList;

            foreach (ResourceResourcePermission item in resPermissions)
            {
                if (memberTypeList.Where(g => g.Id == item.MemberTypeId).Count() == 0)
                {
                    MemberTypeViewModel mt = new MemberTypeViewModel();
                    List<int> rpList = new List<int>();
                    mt.Id = item.MemberTypeId;
                    rpList.Add(item.PermissionId);
                    mt.ResPermissionIds = rpList;
                    memberTypeList.Add(mt);
                }
                else
                {
                    MemberTypeViewModel viewModel = memberTypeList.Where(g => g.Id == item.MemberTypeId).FirstOrDefault();

                    viewModel.ResPermissionIds.Add(item.PermissionId);
                }
            }

            return memberTypeList;
        }


        /// <summary>
        /// 获取权限中间集合
        /// </summary>
        /// <param name="rpList"></param>
        /// <param name="mtList"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        private List<ResourceResourcePermission> GetResPers(List<MemberTypeViewModel> GetMemberTypes, int resourceId = 0)
        {
            List<ResourceResourcePermission> resPerList = new List<ResourceResourcePermission>();
            if (GetMemberTypes == null || GetMemberTypes.Count <= 0)
                return null;

            foreach (MemberTypeViewModel item in GetMemberTypes)
            {
                if (item.ResPermissionIds != null && item.ResPermissionIds.Count > 0)
                {
                    foreach (int per in item.ResPermissionIds)
                    {
                        ResourceResourcePermission model = new ResourceResourcePermission();
                        model.MemberTypeId = item.Id;
                        model.PermissionId = per;
                        model.ResourceId = resourceId;
                        resPerList.Add(model);
                    }

                }
            }

            return resPerList;
        }

        private void InitData()
        {
            List<KeyValueViewModel> memberTypes = new List<KeyValueViewModel>();
            List<KeyValueViewModel> permissions = new List<KeyValueViewModel>();

            List<MemberType> memberTypeList = new ResourceMemberController().GetMemberTypes();
            if (memberTypeList != null && memberTypeList.Count > 0)
                memberTypes = memberTypeList.Select(g => new KeyValueViewModel() { Key = g.Id, Value = g.MemberName }).OrderBy(g => g.Key).ToList();
            ViewBag.memberTypes = memberTypes;

            List<ResourcePermission> permissionList = new ResourcePermissionController().GetAll();
            if (permissionList != null && permissionList.Count > 0)
                permissions = permissionList.Select(g => new KeyValueViewModel() { Key = g.Id, Value = g.Name }).ToList();
            ViewBag.permissions = permissions;
        }
    }
}
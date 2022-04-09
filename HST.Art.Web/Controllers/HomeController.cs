using HST.Art.Core;
using HST.Art.Service;
using System;
using System.Web.Mvc;
using HST.Utillity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace HST.Art.Web.Controllers
{
    public class HomeController : ApplicationBase
    {
        ArticleService _articleService = new ArticleService();
        MemberUnitService _memberUnitService = new MemberUnitService();
        RotationChartService _rotationChartService = new RotationChartService();
        OrganizationService _orgService = new OrganizationService();
        CategoryDictionaryService _cdService = new CategoryDictionaryService();
        TeaCertificateService _teaService = new TeaCertificateService();
        StuCertificateService _stuService = new StuCertificateService();
        FileDownloadService _downService = new FileDownloadService();

        // GET: Home
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            int totalNum = 0;
            FilterEntityModel fillter = new FilterEntityModel();
            fillter.PageSize = 5;
            fillter.SortDict = new KeyValuePair<string, SortType>("PublishDate", SortType.Desc);
            fillter.KeyValueList = new List<KeyValueObj>();
            fillter.KeyValueList.Add(new KeyValueObj() { Key = "State", Value = (int)PublishState.Upper });

            FilterEntityModel associationFillter = new FilterEntityModel() { KeyValueList = new List<KeyValueObj>() { new KeyValueObj() { Key = "State", Value = (int)PublishState.Upper } }, KeyValueReserves = new List<KeyValueObj>() { new KeyValueObj() { Key = "Section", Value = (int)SectionType.Association } }, PageSize = 5, SortDict = new KeyValuePair<string, SortType>("PublishDate", SortType.Desc) };
            FilterEntityModel industryFillter = new FilterEntityModel() { KeyValueList = new List<KeyValueObj>() { new KeyValueObj() { Key = "State", Value = (int)PublishState.Upper } }, KeyValueReserves = new List<KeyValueObj>() { new KeyValueObj() { Key = "Section", Value = (int)SectionType.Industry } }, PageSize = 5, SortDict = new KeyValuePair<string, SortType>("PublishDate", SortType.Desc) };
            FilterEntityModel socialFillter = new FilterEntityModel() { KeyValueList = new List<KeyValueObj>() { new KeyValueObj() { Key = "State", Value = (int)PublishState.Upper } }, KeyValueReserves = new List<KeyValueObj>() { new KeyValueObj() { Key = "Section", Value = (int)SectionType.Social } }, PageSize = 3, SortDict = new KeyValuePair<string, SortType>("PublishDate", SortType.Desc) };

            List<RotationChart> bannerList = _rotationChartService.GetAll(RotationType.Banner);
            List<RotationChart> logoList = _rotationChartService.GetAll(RotationType.Logo);
            model.AssociationList = _articleService.GetPage(associationFillter, out totalNum);
            model.IndustryList = _articleService.GetPage(industryFillter, out totalNum);
            model.SocialList = _articleService.GetPage(socialFillter, out totalNum);

            model.BannerList = bannerList != null ? bannerList.FindAll(g => g.State == PublishState.Upper) : null;
            model.LogoList = logoList != null ? logoList.FindAll(g => g.State == PublishState.Upper) : null;

            #region 分组
            //if (logoList != null)
            //{
            //    List<List<RotationChart>> listGroup = new List<List<RotationChart>>();
            //    int j = 6;
            //    for (int i = 0; i < logoList.Count; i += 6)
            //    {
            //        List<RotationChart> cList = new List<RotationChart>();
            //        cList = logoList.Take(j).Skip(i).ToList();
            //        j += 6;
            //        listGroup.Add(cList);
            //    }

            //    model.LogoList = listGroup;
            //}   
            #endregion

            #region 公告
            List<BulletinViewModel> bulletinList = new List<BulletinViewModel>();
            FilterEntityModel otherFilter = new FilterEntityModel() { KeyValueList = new List<KeyValueObj>() { new KeyValueObj() { Key = "State", Value = (int)PublishState.Upper } }, PageSize = 5, SortDict = new KeyValuePair<string, SortType>("CreateDate", SortType.Desc) };
            List<Article> newestList = _articleService.GetPage(fillter, out totalNum);
            List<MemberUnit> unitList = _memberUnitService.GetPage(otherFilter, out totalNum);
            List<FileDownload> fileList = _downService.GetPage(otherFilter, out totalNum);

            if (unitList != null && unitList.Count > 0)
            {
                bulletinList.AddRange(unitList.Take(1).Select(g => new BulletinViewModel() { Id = g.Id, Category = g.Category, CreateTime = g.CreateDate, Title = g.Name, SectionType = CategoryType.Member }));
            }

            if (fileList != null && fileList.Count > 0)
            {
                bulletinList.AddRange(fileList.Take(1).Select(g => new BulletinViewModel() { Id = g.Id, Category = g.Category, CreateTime = g.CreateDate, Title = g.Title, SectionType = CategoryType.Download }));
            }

            if (newestList != null && newestList.Count > 0)
            {
                bulletinList.AddRange(newestList.Select(g => new BulletinViewModel() { Id = g.Id, Category = g.Category, CreateTime = g.CreateDate, Title = g.Title, SectionType = GetCategoryType((int)g.Section) }));
            }

            model.BulletinList = bulletinList.OrderByDescending(g => g.CreateTime).Take(5).ToList();

            #endregion

            return View(model);
        }

        public ActionResult About(QueryViewModel model)
        {
            int category = 0;
            WebContentViewModel viewModel = new WebContentViewModel();
            InitData(CategoryType.Member);
            Organization orgInfo = _orgService.GetChacheData();
            model = model == null ? new QueryViewModel() : model;
            viewModel.QType = model.QType;

            switch (model.QType)
            {
                case QSType.synopsis:
                    viewModel.DetailModel = new DetailViewModel()
                    {
                        Title = orgInfo.Name,
                        Description = orgInfo.Detail,
                        CreateDate = orgInfo.CreateDate
                    };
                    break;
                case QSType.frame:
                    viewModel.DetailModel = new DetailViewModel()
                    {
                        Title = orgInfo.Name,
                        Description = orgInfo.Framework,
                        CreateDate = orgInfo.CreateDate
                    };
                    break;
                case QSType.list:
                    int.TryParse(model.FCType, out category);
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category
                    };
                    break;
                case QSType.detail:
                    MemberUnit mInfo = _memberUnitService.Get(model.Id);
                    int.TryParse(model.FCType, out category);
                    viewModel.DetailModel = new DetailViewModel()
                    {
                        Title = mInfo.Name,
                        Description = mInfo.Description,
                        CreateDate = mInfo.CreateDate
                    };
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category
                    };
                    break;
            }

            return View(viewModel);
        }
        public ActionResult Download(QueryViewModel model)
        {
            int category = 0;
            WebContentViewModel viewModel = new WebContentViewModel();
            InitData(CategoryType.Download);
            model = model == null ? new QueryViewModel() : model;
            model.QType = model.QType != QSType.list && model.QType != QSType.detail ? QSType.list : model.QType;
            viewModel.QType = model.QType;

            switch (model.QType)
            {
                case QSType.list:
                    int.TryParse(model.FCType, out category);
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category
                    };
                    break;
                case QSType.detail:
                    FileDownload mInfo = _downService.Get(model.Id);
                    int.TryParse(model.FCType, out category);
                    viewModel.DetailModel = new DetailViewModel()
                    {
                        Id = mInfo.Id,
                        Title = mInfo.Title,
                        Description = mInfo.Description,
                        CreateDate = mInfo.CreateDate,
                        Author = mInfo.UserName,
                        FileName = mInfo.Name,
                        FileUrl = mInfo.Src
                    };
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category
                    };
                    break;
            }

            return View(viewModel);
        }

        public ActionResult Examination(QueryViewModel model)
        {
            int category = 0;
            int parCategory = 0;
            WebContentViewModel viewModel = new WebContentViewModel();
            InitData(CategoryType.Examination);
            model = model == null ? new QueryViewModel() : model;
            model.QType = model.QType != QSType.list && model.QType != QSType.detail ? QSType.list : model.QType;
            viewModel.QType = model.QType;

            switch (model.QType)
            {
                case QSType.list:
                    int.TryParse(model.FCType, out category);
                    int.TryParse(model.PCType, out parCategory);
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category,
                        ParCategory = parCategory,
                        SectionType = SectionType.Examination
                    };
                    break;
                case QSType.detail:
                    Article mInfo = _articleService.Get(model.Id);
                    int.TryParse(model.FCType, out category);
                    int.TryParse(model.PCType, out parCategory);
                    viewModel.DetailModel = new DetailViewModel()
                    {
                        Id = mInfo.Id,
                        Title = mInfo.Title,
                        Description = mInfo.Content,
                        CreateDate = mInfo.CreateDate,
                        Author = mInfo.UserName
                    };
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category,
                        ParCategory = parCategory,
                        SectionType = SectionType.Examination
                    };
                    break;
            }

            return View(viewModel);
        }

        public ActionResult Industry(QueryViewModel model)
        {
            int category = 0;
            WebContentViewModel viewModel = new WebContentViewModel();
            InitData(CategoryType.Industry);
            model = model == null ? new QueryViewModel() : model;
            model.QType = model.QType != QSType.list && model.QType != QSType.detail ? QSType.list : model.QType;
            viewModel.QType = model.QType;

            switch (model.QType)
            {
                case QSType.list:
                    int.TryParse(model.FCType, out category);
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category,
                        SectionType = SectionType.Industry
                    };
                    break;
                case QSType.detail:
                    Article mInfo = _articleService.Get(model.Id);
                    int.TryParse(model.FCType, out category);
                    viewModel.DetailModel = new DetailViewModel()
                    {
                        Id = mInfo.Id,
                        Title = mInfo.Title,
                        Description = mInfo.Content,
                        CreateDate = mInfo.CreateDate,
                        Author = mInfo.UserName
                    };
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category,
                        SectionType = SectionType.Industry
                    };
                    break;
            }

            return View(viewModel);
        }

        public ActionResult Social(QueryViewModel model)
        {
            int category = 0;
            WebContentViewModel viewModel = new WebContentViewModel();
            InitData(CategoryType.Social);
            model = model == null ? new QueryViewModel() : model;
            model.QType = model.QType != QSType.list && model.QType != QSType.detail ? QSType.list : model.QType;
            viewModel.QType = model.QType;

            switch (model.QType)
            {
                case QSType.list:
                    int.TryParse(model.FCType, out category);
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category,
                        SectionType = SectionType.Social
                    };
                    break;
                case QSType.detail:
                    Article mInfo = _articleService.Get(model.Id);
                    int.TryParse(model.FCType, out category);
                    viewModel.DetailModel = new DetailViewModel()
                    {
                        Id = mInfo.Id,
                        Title = mInfo.Title,
                        Description = mInfo.Content,
                        CreateDate = mInfo.CreateDate,
                        Author = mInfo.UserName
                    };
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category,
                        SectionType = SectionType.Social
                    };
                    break;
            }

            return View(viewModel);
        }

        public ActionResult Association(QueryViewModel model)
        {
            int category = 0;
            WebContentViewModel viewModel = new WebContentViewModel();
            InitData(CategoryType.Association);
            model = model == null ? new QueryViewModel() : model;
            model.QType = model.QType != QSType.list && model.QType != QSType.detail ? QSType.list : model.QType;
            viewModel.QType = model.QType;

            switch (model.QType)
            {
                case QSType.list:
                    int.TryParse(model.FCType, out category);
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category,
                        SectionType = SectionType.Association
                    };
                    break;
                case QSType.detail:
                    Article mInfo = _articleService.Get(model.Id);
                    int.TryParse(model.FCType, out category);
                    viewModel.DetailModel = new DetailViewModel()
                    {
                        Id = mInfo.Id,
                        Title = mInfo.Title,
                        Description = mInfo.Content,
                        CreateDate = mInfo.CreateDate,
                        Author = mInfo.UserName
                    };
                    viewModel.PageFilter = new PageViewModel()
                    {
                        Category = category,
                        SectionType = SectionType.Association
                    };
                    break;
            }

            return View(viewModel);
        }

        [HttpPost]
        public PartialViewResult GetArticList(PageViewModel query)
        {
            int totalNum = 0;
            FilterEntityModel fillter = new FilterEntityModel();
            fillter.PageIndex = query.PageIndex;
            fillter.PageSize = query.PageSize;
            fillter.KeyValueList = new List<KeyValueObj>();
            fillter.KeyValueReserves = new List<KeyValueObj>() { new KeyValueObj() { Key = "Section", Value = (int)query.SectionType }, new KeyValueObj() { Key = "State", Value = (int)PublishState.Upper } };

            if (query.ParCategory > 0)
            {
                List<int> chridlen = _cdService.GetCategorysByPartentId(query.ParCategory);
                if (chridlen != null && chridlen.Count > 0)
                {
                    fillter.FilterType = FilterType.In;

                    fillter.KeyValueList.Add(new KeyValueObj() { Key = "Category", Value = chridlen, FieldType = FieldType.Int });
                }
            }
            else
            {
                if (query.Category > 0)
                {
                    fillter.KeyValueList.Add(new KeyValueObj() { Key = "Category", Value = query.Category });
                }
            }

            List<WebNewsViewModel> webModelList = new List<WebNewsViewModel>();
            List<Article> newsestList = _articleService.GetPage(fillter, out totalNum);
            if (newsestList != null && newsestList.Count > 0)
            {
                webModelList = newsestList.Select(g => new WebNewsViewModel() { Id = g.Id, Title = g.Title, Synopsis = g.Synopsis, CreateTime = g.PublishDate, Author = g.UserName, HeadImg = GetThumb(g.HeadImg), Category = g.Category, ParCategory = g.ParCategory }).ToList();
            }

            ReturnPageResultIList<WebNewsViewModel> data = new ReturnPageResultIList<WebNewsViewModel>(webModelList, totalNum);
            PageListViewModel<WebNewsViewModel> mpage = new PageListViewModel<WebNewsViewModel>(data.DataT, query.PageIndex, query.PageSize, data.totalRecords);

            ViewBag.SectionType = query.SectionType;

            return PartialView(mpage);
        }

        public ActionResult Certificate()
        {
            Organization org = _orgService.GetChacheData();
            CertViewModel model = new CertViewModel()
            {
                Title = org.Name,
                Description = org.Description,
                CertType = CertType.teacher
            };

            model.PageFilter.PageSize = 30;

            ViewBag.City = City;
            return View(model);
        }

        /// <summary>
        /// 获取推荐新闻分布视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult GetRecommend(QueryViewModel query)
        {
            if (query == null)
            {
                query = new QueryViewModel();
            }

            int totalNum = 0;
            SidebarViewModel model = new SidebarViewModel();
            model.SectionType = query.SectionType;
            FilterEntityModel fillter = new FilterEntityModel();
            fillter.SortDict = new KeyValuePair<string, SortType>("PublishDate", SortType.Desc);
            fillter.KeyValueList = new List<KeyValueObj>();
            fillter.KeyValueList.Add(new KeyValueObj() { Key = "State", Value = (int)PublishState.Upper });
            List<WebNewsViewModel> webModelList = new List<WebNewsViewModel>();

            switch (query.SectionType)
            {
                case CategoryType.Industry:
                    fillter.KeyValueReserves = new List<KeyValueObj>() { new KeyValueObj() { Key = "Section", Value = (int)SectionType.Industry } };
                    break;
                case CategoryType.Association:
                    fillter.KeyValueReserves = new List<KeyValueObj>() { new KeyValueObj() { Key = "Section", Value = (int)SectionType.Association } };
                    break;
                case CategoryType.Examination:
                    fillter.KeyValueReserves = new List<KeyValueObj>() { new KeyValueObj() { Key = "Section", Value = (int)SectionType.Examination } };
                    break;
                case CategoryType.Social:
                    fillter.KeyValueReserves = new List<KeyValueObj>() { new KeyValueObj() { Key = "Section", Value = (int)SectionType.Social } };
                    break;
                case CategoryType.Member:
                    fillter.SortDict = new KeyValuePair<string, SortType>("CreateDate", SortType.Desc);
                    break;
                case CategoryType.Download:
                    fillter.SortDict = new KeyValuePair<string, SortType>("CreateDate", SortType.Desc);
                    break;
            }

            if (query.SectionType == CategoryType.Member)
            {
                List<MemberUnit> unitList = _memberUnitService.GetPage(fillter, out totalNum);
                if (unitList != null && unitList.Count > 0)
                {
                    webModelList = unitList.Select(g => new WebNewsViewModel() { Id = g.Id, Title = g.Name, Synopsis = g.Synopsis, CreateTime = g.CreateDate, Author = g.UserName, HeadImg = GetThumb(g.HeadImg), Category = g.Category }).ToList();
                }
            }
            else if (query.SectionType == CategoryType.Download)
            {
                List<FileDownload> downList = _downService.GetPage(fillter, out totalNum);
                if (downList != null && downList.Count > 0)
                {
                    webModelList = downList.Select(g => new WebNewsViewModel() { Id = g.Id, Title = g.Title, Synopsis = g.Synopsis, CreateTime = g.CreateDate, Author = g.UserName, HeadImg = GetThumb(g.HeadImg), Category = g.Category }).ToList();
                }
            }
            else
            {
                List<Article> newsestList = _articleService.GetPage(fillter, out totalNum);
                if (newsestList != null && newsestList.Count > 0)
                {
                    webModelList = newsestList.Select(g => new WebNewsViewModel() { Id = g.Id, Title = g.Title, Synopsis = g.Synopsis, CreateTime = g.PublishDate, Author = g.UserName, HeadImg = GetThumb(g.HeadImg), Category = g.Category, ParCategory = g.ParCategory }).ToList();
                }
            }

            if (webModelList != null)
            {
                webModelList = query.Id > 0 && webModelList.Count > 5 ? webModelList.FindAll(g => g.Id != query.Id).Take(5).ToList() : webModelList.Take(5).ToList();
            }

            model.NewestList = webModelList;
            return PartialView(model);
        }

        public PartialViewResult Header()
        {
            Organization model = _orgService.GetChacheData();
            return PartialView(model);
        }

        public PartialViewResult Footer()
        {
            Organization model = _orgService.GetChacheData();
            return PartialView(model);
        }

        public PartialViewResult SecondMenu(CategoryType sectionType)
        {
            HeaderViewModel model = new HeaderViewModel();
            model.SectionType = sectionType;
            List<CategoryDictionary> cdList = _cdService.GetAll(sectionType);
            if (cdList != null && sectionType != CategoryType.Examination)
            {
                cdList = cdList.FindAll(g => g.State == PublishState.Upper);
            }

            if (sectionType == CategoryType.Examination)
            {
                cdList = cdList.FindAll(g => g.Parent == 0);
            }

            model.CategoryList = cdList;
            return PartialView(model);
        }

        /// <summary>
        /// DataTable读取数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MemberList(PageViewModel model)
        {
            int totalNum = 0;
            FilterEntityModel fillter = new FilterEntityModel();
            fillter.PageIndex = model.PageIndex;
            fillter.PageSize = model.PageSize;
            fillter.KeyValueList = new List<KeyValueObj>();
            fillter.KeyValueList.Add(new KeyValueObj() { Key = "State", Value = (int)PublishState.Upper });

            if (model.Category > 0)
            {
                fillter.KeyValueList.Add(new KeyValueObj() { Key = "Category", Value = model.Category });
            }

            List<MemberUnit> memberList = _memberUnitService.GetPage(fillter, out totalNum);
            ReturnPageResultIList<MemberUnit> data = new ReturnPageResultIList<Core.MemberUnit>(memberList, totalNum);
            IList<MemberUnitViewModel> gmList = new List<MemberUnitViewModel>();

            if (data != null && data.DataT != null)
                gmList = data.DataT.Select(g => new MemberUnitViewModel() { Id = g.Id, UserId = g.UserId, MemberUnitName = g.Name, CategoryName = g.CategoryName, State = (int)g.State, CreateTime = g.CreateDate.ToLongDateString(), Category = g.Category, UserName = g.UserName, Number = g.Number, Star = g.Star, HeadImg = g.HeadImg, SmallHeadImg = GetThumb(g.HeadImg), Province = Convert.ToInt32(g.Province), City = Convert.ToInt32(g.City), Area = GetAreaStr(g.Province, g.City), Synopsis = g.Synopsis }).ToList();

            PageListViewModel<MemberUnitViewModel> mpage = new PageListViewModel<MemberUnitViewModel>(gmList, model.PageIndex, model.PageSize, data.totalRecords);

            return PartialView(mpage);
        }

        /// <summary>
        /// DataTable读取数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult DownloadList(PageViewModel model)
        {
            int totalNum = 0;
            FilterEntityModel fillter = new FilterEntityModel();
            fillter.PageIndex = model.PageIndex;
            fillter.PageSize = model.PageSize;
            fillter.KeyValueList = new List<KeyValueObj>();
            fillter.KeyValueList.Add(new KeyValueObj() { Key = "State", Value = (int)PublishState.Upper });

            if (model.Category > 0)
            {
                fillter.KeyValueList.Add(new KeyValueObj() { Key = "Category", Value = model.Category });
            }

            List<FileDownload> downList = _downService.GetPage(fillter, out totalNum);
            ReturnPageResultIList<FileDownload> data = new ReturnPageResultIList<Core.FileDownload>(downList, totalNum);
            IList<DownloadViewModel> gmList = new List<DownloadViewModel>();

            if (data != null && data.DataT != null)
                gmList = data.DataT.Select(g => new DownloadViewModel() { Id = g.Id, UserId = g.UserId, FileName = g.Name, State = (int)g.State, CreateTime = g.CreateDate.ToLongDateString(), Category = g.Category, UserName = g.UserName, SmallFileImg = GetThumb(g.HeadImg), Synopsis = g.Synopsis, FileTitle = g.Title }).ToList();

            PageListViewModel<DownloadViewModel> mpage = new PageListViewModel<DownloadViewModel>(gmList, model.PageIndex, model.PageSize, data.totalRecords);

            return PartialView(mpage);
        }

        /// <summary>
        /// DataTable读取数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CertificateList(PageViewModel model)
        {
            int totalNum = 0;
            FilterEntityModel fillter = new FilterEntityModel();
            fillter.PageIndex = model.PageIndex;
            fillter.PageSize = model.PageSize;
            fillter.FilterType = FilterType.Or;
            fillter.KeyValueList = new List<KeyValueObj>();
            fillter.KeyValueReserves = new List<KeyValueObj>();
            fillter.KeyValueReserves.Add(new KeyValueObj() { Key = "State", Value = (int)PublishState.Upper });

            if (!string.IsNullOrEmpty(model.CityCode))
            {
                fillter.KeyValueReserves.Add(new KeyValueObj() { Key = "City", Value = model.CityCode });
            }

            if (!string.IsNullOrEmpty(model.NameOrNumber))
            {
                fillter.KeyValueReserves.Add(new KeyValueObj() { Key = "Name", Value = model.NameOrNumber });
                fillter.KeyValueList.Add(new KeyValueObj() { Key = "Number", Value = model.NameOrNumber });
            }

            List<ListViewModel> modelList = new List<ListViewModel>();

            #region 数据处理
            switch (model.CertType)
            {
                case CertType.student:
                    List<StuCertificate> stuList = _stuService.GetPage(fillter, out totalNum);
                    modelList = stuList.Select(g => new ListViewModel() { Name = g.Name, Gender = g.Gender.GetDescription(), Number = g.Number, Area = GetAreaStr(Constant.DEFAULT_PROVINCE, g.City).Replace("-", "") }).ToList();
                    break;
                case CertType.member:
                    List<MemberUnit> memberList = _memberUnitService.GetPage(fillter, out totalNum);
                    modelList = memberList.Select(g => new ListViewModel() { Name = g.Name, Number = g.Number, Area = GetAreaStr(Constant.DEFAULT_PROVINCE, g.City).Replace("-", ""), Star = g.Star }).ToList();
                    break;
                default:
                    List<TeaCertificate> teaList = _teaService.GetPage(fillter, out totalNum);
                    modelList = teaList.Select(g => new ListViewModel() { Name = g.Name, Gender = g.Gender.GetDescription(), Number = g.Number, Area = GetAreaStr(Constant.DEFAULT_PROVINCE, g.City).Replace("-", ""), LevelName = g.Level.GetDescription() }).ToList();
                    break;
            }

            ViewBag.CertType = model.CertType;
            #endregion

            ReturnPageResultIList<ListViewModel> data = new ReturnPageResultIList<ListViewModel>(modelList, totalNum);
            PageListViewModel<ListViewModel> mpage = new PageListViewModel<ListViewModel>(data.DataT, model.PageIndex, model.PageSize, data.totalRecords);

            return PartialView(mpage);
        }

        public void InitData(CategoryType type = CategoryType.UnKnown)
        {
            List<CategoryDictionary> cdList = _cdService.GetAll(type);
            Dictionary<CategoryDictionary, List<CategoryDictionary>> dicCategorys = new Dictionary<CategoryDictionary, List<CategoryDictionary>>();

            if (cdList != null && type != CategoryType.Examination)
            {
                cdList.RemoveAll(g => g.State == PublishState.Lower);
            }

            ViewBag.Categorys = cdList;

            if (type == CategoryType.Examination && cdList != null)
            {
                List<CategoryDictionary> parList = cdList.FindAll(g => g.Parent == 0);
                foreach (CategoryDictionary item in parList)
                {
                    dicCategorys.Add(item, cdList.FindAll(g => g.Parent == item.Id && g.State == PublishState.Upper));
                }

                ViewBag.Categorys = dicCategorys;
            }
        }

        public CategoryType GetCategoryType(int type)
        {
            return (CategoryType)type;
        }
    }
}
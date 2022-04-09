using HST.Art.Core;
using HST.Art.Service;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HST.Art.Web.Areas.manage.Controllers
{
    public class ArticleController : ApplicationBase
    {
        ArticleService articleService = new ArticleService();
        CategoryDictionaryService cdService = new CategoryDictionaryService();
        List<CategoryDictionary> cdEnabledList = new List<CategoryDictionary>();
        List<CategoryDictionary> cdAllList = new List<CategoryDictionary>();
        Dictionary<CategoryDictionary, List<CategoryDictionary>> dicCategorys = new Dictionary<CategoryDictionary, List<CategoryDictionary>>();
        public ActionResult List()
        {
            InitData();
            ViewBag.Statistic = GetStatistic();
            return View();
        }

        [HttpPost]
        public ActionResult GetJsonData(SearchDataTable dt, SearchViewModel svm)
        {
            try
            {
                int totalNum = 0;
                FilterEntityModel fillter = new FilterEntityModel();
                fillter.PageIndex = dt.pageIndex;
                fillter.PageSize = dt.length;
                fillter.KeyValueList = new List<KeyValueObj>();

                if (svm != null)
                {
                    if (!string.IsNullOrEmpty(svm.ReserveField))
                    {
                        fillter.KeyValueReserves = new List<KeyValueObj>() { new KeyValueObj() { Key = "Section", Value = svm.ReserveField } };
                    }

                    if (!string.IsNullOrEmpty(svm.FilterKey) && !string.IsNullOrEmpty(svm.FilterVal))
                    {
                        SearchType ftype = (SearchType)Convert.ToInt16(svm.FilterKey);
                        KeyValueObj kvb = new KeyValueObj() { Value = svm.FilterVal };
                        switch (ftype)
                        {
                            case SearchType.Title:
                                kvb.Key = "Title";
                                fillter.FilterType = FilterType.Like;
                                break;
                            case SearchType.State:
                                kvb.Key = "State";
                                break;
                            case SearchType.Type:
                                kvb.Key = "Category";
                                break;
                            case SearchType.Date:
                                kvb.Key = "PublishDate";
                                kvb.FieldType = FieldType.Date;
                                fillter.FilterType = FilterType.Between;
                                break;
                        }

                        fillter.KeyValueList.Add(kvb);
                    }

                }

                List<Article> downList = articleService.GetPage(fillter, out totalNum);
                ReturnPageResultIList<Article> data = new ReturnPageResultIList<Core.Article>(downList, totalNum);
                IList<ArticleViewModel> gmList = new List<ArticleViewModel>();

                if (data != null && data.DataT != null)
                    gmList = data.DataT.Select(g => new ArticleViewModel() { Id = g.Id, UserId = g.UserId, Title = g.Title, CategoryName = !string.IsNullOrEmpty(g.ParCategoryName) ? g.ParCategoryName + "-" + g.CategoryName : g.CategoryName, State = (int)g.State, CreateTime = g.CreateDate.ToString("yyyy-MM-dd HH:mm"), PublishDate = g.PublishDate <= DateTime.MinValue ? "无发布日期" : g.PublishDate.ToString("yyyy-MM-dd HH:mm"), Category = g.Category, UserName = g.UserName, ParCategory = g.ParCategory, HeadImg = g.HeadImg, SmallHeadImg = GetThumb(g.HeadImg), Section = g.Section, ParCategoryName = g.ParCategoryName }).ToList();

                return Json(new
                {
                    recordsFiltered = data.totalRecords,
                    recordsTotal = data.totalPages,
                    data = gmList
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                AddLog("GetList",ex.ToString());
            }

            return EmptyJsonResult();
        }


        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            if (cdAllList == null || cdAllList.Count <= 0)
            {
                InitData();
            }

            Article data = articleService.Get(id);
            ViewBag.ParentCategory = cdAllList.FindAll(g => g.Type == CategoryType.Examination).FindAll(g => g.Parent == 0);
            ViewBag.ExCategorys = cdAllList.FindAll(g => g.Parent == data.ParCategory);
            ViewBag.Categorys = cdAllList.FindAll(g => (int)g.Type == (int)data.Section);

            if (data != null)
            {
                ArticleViewModel model = new ArticleViewModel();
                model.Id = data.Id;
                model.Title = data.Title;
                model.CategoryName = data.CategoryName;
                model.UserName = data.UserName;
                model.State = (int)data.State;
                model.UserId = data.UserId;
                model.Description = data.Content;
                model.HeadImg = data.HeadImg;
                model.SmallHeadImg = GetThumb(data.HeadImg);
                model.Section = data.Section;
                model.Category = data.Category;
                model.ParCategory = data.ParCategory;

                #region 类别可用判断  预留
                //if (cdEnabledList != null && cdEnabledList.Count > 0)
                //{
                //    if (cdEnabledList.Where(g => g.Id == data.Category).Count() > 0)
                //        model.Category = data.Category;
                //    if (cdEnabledList.Where(g => g.Id == data.ParCategory).Count() > 0)
                //        model.ParCategory = data.ParCategory;
                //}
                #endregion

                return View(model);
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(ArticleViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                Article data = articleService.Get(model.Id);

                if (data.State == PublishState.Lower && model.State == (int)PublishState.Upper)
                {
                    data.PublishDate = DateTime.Now;
                }

                data.Title = model.Title;
                data.Content = !string.IsNullOrEmpty(model.Description) ? model.Description.Replace("\r\n", "") : string.Empty;
                data.Category = model.Category;
                data.State = (PublishState)model.State;
                data.HeadImg = model.HeadImg;
                data.Section = model.Section;
                data.ParCategory = model.ParCategory;

                rmodel.isSuccess = articleService.Update(data);
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
            if (cdAllList == null || cdAllList.Count <= 0)
            {
                InitData();
            }

            ViewBag.ParentCategory = cdEnabledList.FindAll(g => g.Type == CategoryType.Examination).FindAll(g => g.Parent == 0);

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add(ArticleViewModel model)
        {
            ResultRetrun rmodel = new ResultRetrun();
            if (ModelState.IsValid)
            {
                Account account = GetAccount();
                Article downModel = new Article()
                {
                    Title = model.Title,
                    Content = !string.IsNullOrEmpty(model.Description) ? model.Description.Replace("\r\n", "") : string.Empty,
                    Category = model.Category,
                    State = (PublishState)model.State,
                    HeadImg = model.HeadImg,
                    ParCategory = model.ParCategory,
                    UserId = account.Id,
                    Section = model.Section,
                    Author = account.UserName
                };

                rmodel.isSuccess = articleService.Add(downModel);
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
                rmodel.isSuccess = articleService.LogicDelete(id);
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
                rmodel.isSuccess = articleService.Publish(id);
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
                rmodel.isSuccess = articleService.Recovery(id);
            }
            catch (Exception ex)
            {
                rmodel.message = "操作失败，原因：" + ex.Message;
            }
            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 通过模块类别获取类别集合
        /// </summary>
        /// <param name="sectionType">模块类别</param>
        /// <param name="searchType">筛选类型（0：所有类别，1：可用类别）</param>
        /// <returns></returns>
        public JsonResult GetCategorysBySection(string sectionType, int searchType = 0)
        {
            List<KeyValueViewModel> rmodel = new List<KeyValueViewModel>();

            if (cdAllList == null || cdAllList.Count <= 0)
            {
                InitData();
            }

            if (cdAllList != null && cdAllList.Count > 0)
            {
                if (string.IsNullOrEmpty(sectionType))
                {
                    rmodel.AddRange(cdAllList.FindAll(g => g.Type != CategoryType.Examination).Select(s => new KeyValueViewModel() { Key = s.Id, Value = s.Name }));

                    foreach (var item in dicCategorys)
                    {
                        rmodel.AddRange(item.Value.Select(g => new KeyValueViewModel() { Key = g.Id, Value = item.Key.Name + "-" + g.Name }));
                    }

                    return Json(rmodel, JsonRequestBehavior.AllowGet);
                }

                if (searchType > 0)
                {
                    rmodel = cdEnabledList.FindAll(g => g.Type == (CategoryType)Convert
                    .ToInt32(sectionType)).Select(g => new KeyValueViewModel() { Key = g.Id, Value = g.Name }).ToList();
                }
                else
                {
                    rmodel = cdAllList.FindAll(g => g.Type == (CategoryType)Convert
                    .ToInt32(sectionType)).Select(g => new KeyValueViewModel() { Key = g.Id, Value = g.Name }).ToList();
                }
            }

            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过父级id获取类别集合
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <param name="searchType">筛选类型（0：所有类别，1：可用类别）</param>
        /// <returns></returns>
        public JsonResult GetCategorysById(int parentId, int searchType = 0)
        {
            List<KeyValueViewModel> rmodel = new List<KeyValueViewModel>();
            if (parentId <= 0) return Json(rmodel, JsonRequestBehavior.AllowGet);

            if (cdAllList == null || cdAllList.Count <= 0)
            {
                InitData();
            }

            if (cdAllList != null && cdAllList.Count > 0)
            {
                if (searchType > 0)
                {
                    rmodel = cdEnabledList.FindAll(g => g.Parent == parentId).Select(g => new KeyValueViewModel() { Key = g.Id, Value = g.Name }).ToList();
                }
                else
                {
                    rmodel = cdAllList.FindAll(g => g.Parent == parentId).Select(g => new KeyValueViewModel() { Key = g.Id, Value = g.Name }).ToList();
                }
            }

            return Json(rmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int id)
        {
            Article data = articleService.Get(id);

            if (data != null)
                return View(new ArticleViewModel
                {
                    Id = data.Id,
                    CategoryName = data.CategoryName,
                    UserName = data.UserName,
                    State = (int)data.State,
                    Category = data.Category,
                    UserId = data.UserId,
                    Description = data.Content,
                    HeadImg = data.HeadImg,
                    SmallHeadImg = GetThumb(data.HeadImg)
                });
            else
                return View();
        }

        private void InitData()
        {
            List<CategoryType> categoryList = new List<CategoryType>() {
                 CategoryType.Association,
                 CategoryType.Examination,
                 CategoryType.Industry,
                 CategoryType.Social
            };

            cdAllList = cdService.GetAll(categoryList);
            if (cdAllList != null && cdAllList.Count > 0)
            {
                cdEnabledList = cdAllList.FindAll(g => g.State == PublishState.Upper);
                List<CategoryDictionary> examList = cdAllList.FindAll(g => g.Type == CategoryType.Examination);

                if (examList.Count > 0)
                {
                    List<CategoryDictionary> parList = examList.FindAll(g => g.Parent == 0);
                    foreach (CategoryDictionary item in parList)
                    {
                        dicCategorys.Add(item, examList.FindAll(g => g.Parent == item.Id));
                    }
                }
            }

            ViewBag.ExamCategory = dicCategorys;
        }

        private List<StatisticViewModel> GetStatistic()
        {
            List<ArticleStatistic> list = articleService.GetStatistics();
            List<StatisticViewModel> model = new List<StatisticViewModel>();
            foreach (SectionType item in Enum.GetValues(typeof(SectionType)))
            {
                if (item != SectionType.UnKnown)
                {
                    model.Add(new StatisticViewModel() { Section = item, Count = 0, Percent = "0.0%" });
                }
            }

            model = model.OrderBy(g => g.Section).ToList();

            if (list != null && list.Count > 0)
            {
                double totalCount = list.Sum(g => g.SectionCount);
                foreach (ArticleStatistic item in list)
                {
                    model.Find(g => g.Section == item.SectionType).Count = item.SectionCount;
                }

                model.ForEach(g => g.Percent = string.Format("{0:0.0%}", g.Count / totalCount));
            }

            return model;
        }
    }
}
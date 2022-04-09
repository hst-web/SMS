using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HST.Art.Core;


namespace HST.Art.Web
{
    public class HomeViewModel
    {
        public List<RotationChart> BannerList { get; set; }

        public List<RotationChart> LogoList { get; set; }
        /// <summary>
        /// 协会活动集合
        /// </summary>
        public List<Article> AssociationList { get; set; }
        /// <summary>
        /// 行业资讯集合
        /// </summary>
        public List<Article> IndustryList { get; set; }
        /// <summary>
        /// 社会公益集合
        /// </summary>
        public List<Article> SocialList { get; set; }
        /// <summary>
        /// 协会公告最新集合
        /// </summary>
        public List<BulletinViewModel> BulletinList { get; set; }
    }

    public class BulletinViewModel
    {
        public int Id { get; set; }
        public CategoryType SectionType { get; set; }  
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public int Category { get; set; }
        public int ParCategory { get; set; }
    }

    public class DetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
    }

    public class ListViewModel
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Area { get; set; }
        public string Number { get; set; }
        public string LevelName { get; set; }
        public int Star { get; set; }
    }

    public class WebContentViewModel
    {
        public WebContentViewModel()
        {
            DetailModel = new DetailViewModel();
            PageFilter = new PageViewModel();
        }
        public DetailViewModel DetailModel { get; set; }
        //public PageListViewModel<object> PageListModel { get; set; }
        public PageViewModel PageFilter { get; set; }
        public QSType QType { get; set; }
    }

    public class CertViewModel
    {
        public CertViewModel()
        {
            PageFilter = new PageViewModel();
        }
        public CertType CertType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public PageViewModel PageFilter { get; set; }
    }

    public class PageViewModel
    {
        public PageViewModel()
        {
            PageIndex = 1;
            PageSize = 10;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Category { get; set; }
        public int ParCategory { get; set; }
        public string CityCode { get; set; }
        public string NameOrNumber { get; set; }
        public SectionType SectionType { get; set; }
        public CertType CertType { get; set; }
    }

    public class QueryViewModel
    {
        public int Id { get; set; }
        public QSType QType { get; set; }
        /// <summary>
        /// 筛选类别
        /// </summary>
        public string FCType { get; set; }
        /// <summary>
        /// 父级筛选类别
        /// </summary>
        public string PCType { get; set; }
        public CategoryType SectionType { get; set; }
    }

    public class WebNewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string HeadImg { get; set; }
        public DateTime CreateTime { get; set; }
        public string Synopsis { get; set; }
        public string Author { get; set; }
        public int Category { get; set; }
        public int ParCategory { get; set; }
    }

    public enum QSType
    {
        synopsis,
        frame,
        list,
        detail
    }

    /// <summary>
    /// 前端查询枚举
    /// </summary>
    public enum CertType
    {
        teacher,
        student,
        member
    }
}
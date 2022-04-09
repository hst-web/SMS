using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
namespace HST.Art.Web
{
    public static class PageHtmlhelper
    {
        /// <summary>
        /// 分页辅助器方法
        /// </summary>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="fun">点击页码调用js方法名</param>
        /// <returns></returns>
        public static MvcHtmlString Page(this HtmlHelper html, int currentPageIndex, int totalPageCount, string fun)
        {
            if (totalPageCount == 0 || currentPageIndex > totalPageCount) { return new MvcHtmlString(""); }
            TagBuilder pagewrap = new TagBuilder("div");//分页最外层
            #region 上一页
            if (currentPageIndex == 1)
                pagewrap.InnerHtml = @"<a href=""javascript:void(0)"" class=""paginate_button previous disabled"" title=""上一页"">上一页</a>";
            else
                pagewrap.InnerHtml = string.Format(@"<a href=""javascript:void(0)"" title=""上一页"" class=""paginate_button previous""  onclick=""{0}({1})"" title=""上一页"">上一页</a>", fun, currentPageIndex - 1);
            #endregion

            TagBuilder page = new TagBuilder("span");//分页第二层
            #region 第1页
            if (currentPageIndex == 1)
                page.InnerHtml += string.Format(@"<a href=""javascript:void(0)"" class=""paginate_button current"" onclick=""{0}({1})"">{2}</a>", fun, 1, 1);
            else
                page.InnerHtml += string.Format(@"<a href=""javascript:void(0)"" class=""paginate_button"" onclick=""{0}({1})"">{2}</a>", fun, 1, 1);
            #endregion

            TagBuilder pageNum = new TagBuilder("span");//分页算法层
            List<string> pageNumList = new List<string>();

            int startindex = 2;//起始,从第2页开始
            int endindex = totalPageCount > 6 ? totalPageCount - 1 : totalPageCount;//结束

            //最多显示7页
            if (totalPageCount > 7)
            {
                if (currentPageIndex <= 4)
                {
                    startindex = 2;
                    endindex = 6;
                }
                else if (totalPageCount <= currentPageIndex + 3)
                {
                    startindex = totalPageCount - 5;
                    endindex = totalPageCount - 1;
                }
                else
                {
                    startindex = currentPageIndex - 2;
                    endindex = currentPageIndex + 2;
                }
            }

            for (int index = startindex; index <= endindex; index++)
            {
                if (index == currentPageIndex)
                {
                    pageNumList.Add(string.Format(@"<a href=""javascript:void(0)"" class=""paginate_button current"" onclick=""{0}({1})"">{2}</a>", fun, index, index));
                }
                else
                {
                    pageNumList.Add(string.Format(@"<a href=""javascript:void(0)"" class=""paginate_button"" onclick=""{0}({1})"">{2}</a>", fun, index, index));
                }
            }

            if (totalPageCount > 7)
            {
                if (currentPageIndex > 4) pageNumList[0] = "<span>…</span>";
                if (currentPageIndex + 3 < totalPageCount) pageNumList[pageNumList.Count-1] = "<span>…</span>";
            }

            foreach (string item in pageNumList) pageNum.InnerHtml += item;
            page.InnerHtml += pageNum.ToString();

            #region 最后1页
            if (totalPageCount > 6)
            {
                if (totalPageCount == currentPageIndex)
                    page.InnerHtml += string.Format(@"<a href=""javascript:void(0)"" class=""paginate_button current"" onclick=""{0}({1})"">{2}</a>", fun, totalPageCount, totalPageCount);
                else
                    page.InnerHtml += string.Format(@"<a href=""javascript:void(0)"" class=""paginate_button"" onclick=""{0}({1})"">{2}</a>", fun, totalPageCount, totalPageCount);
            }
            #endregion

            pagewrap.InnerHtml += page.ToString();

            #region 下一页
            if (currentPageIndex == totalPageCount)
                pagewrap.InnerHtml += string.Format(@"<a href=""javascript:void(0)"" class=""paginate_button next disabled"" title=""下一页"">下一页</a>");
            else
                pagewrap.InnerHtml += string.Format(@"<a href=""javascript:void(0)"" class=""paginate_button next"" title=""下一页"" onclick=""{0}({1})"">下一页</a>", fun, currentPageIndex + 1);
            #endregion

            return new MvcHtmlString(pagewrap.ToString());
        }
    }
}

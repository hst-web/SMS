/*----------------------------------------------------------------
// 文件名：ArticleService.cs
// 功能描述：文章服务
// 创建者：sysmenu
// 创建时间：2019-4-18
//----------------------------------------------------------------*/
using HST.Art.Core;
using System.Collections.Generic;
using HST.Art.Data;
using System.Text.RegularExpressions;

namespace HST.Art.Service
{
    public class ArticleService : ServiceBase, IArticleService
    {
        ArticleProvider _articleProvider = new ArticleProvider();

        public Article Get(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            Article ArticleInfo = _articleProvider.Get(id);
            return ArticleInfo;
        }

        public List<Article> GetAll(FilterEntityModel filterModel = null)
        {
            if (filterModel != null) filterModel.FillWhereTbAsName(Constant.ARTICLE_AS_NAME);//筛选器添加表别名
            List<Article> articleList = _articleProvider.GetAll(filterModel);
            return articleList;
        }

        public List<Article> GetPage(FilterEntityModel filterModel, out int totalNum)
        {
            totalNum = 0;
            //参数验证
            if (filterModel == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            filterModel.FillWhereTbAsName(Constant.ARTICLE_AS_NAME);//筛选器添加表别名
            //获取数据
            List<Article> articleList = _articleProvider.GetPage(filterModel, out totalNum);

            return articleList;
        }

        public bool Add(Article articleInfo)
        {
            //参数验证
            if (articleInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            //文章简介处理
            articleInfo.Synopsis = DisposeHtmlStr(articleInfo.Content);
            return _articleProvider.Add(articleInfo);
        }

        public bool Delete(int id)
        {
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _articleProvider.Delete(id);
        }

        public bool LogicDelete(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _articleProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "IsDeleted",
                Value = 1,
                TableName = "Article"
            });
        }

        public bool Publish(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _articleProvider.Publish(id);
        }

        public bool Recovery(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _articleProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "State",
                Value = (int)PublishState.Lower,
                TableName = "Article"
            });
        }

        public bool Update(Article articleInfo)
        {
            //参数验证
            if (articleInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            //文章简介处理
            articleInfo.Synopsis = DisposeHtmlStr(articleInfo.Content);
            return _articleProvider.Update(articleInfo);
        }

        public List<ArticleStatistic> GetStatistics()
        {
            return _articleProvider.GetStatisticArticles();
        }

        /// <summary>
        /// 文章处理（简介）
        /// </summary>
        private void DisposeArticle(Article articleInfo)
        {
            if (articleInfo == null) return;
            if (string.IsNullOrWhiteSpace(articleInfo.Content)) return;
            if (!string.IsNullOrWhiteSpace(articleInfo.Synopsis)) return;
            articleInfo.Synopsis = GetLength(Regex.Replace(articleInfo.Content, "<[^>]+>", "", RegexOptions.Singleline), 100);
        }

        /// <summary>
        /// 获取设定长度的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        private string GetLength(string str, int length)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            string strR = str;
            if (str.Length > length)
            {
                strR = str.Substring(0, length).TrimEnd(',');
            }
            return strR;
        }
    }
}

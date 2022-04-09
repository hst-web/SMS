using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.SMS.Core
{
    public interface IArticleService : IBaseService
    {
        List<Article> GetPage(FilterEntityModel filterModel, out int totalNum);
        List<Article> GetAll(FilterEntityModel filterModel);
        Article Get(int id);
        List<ArticleStatistic> GetStatistics();
        bool Update(Article articleInfo);
        bool Add(Article articleInfo);
    }
}

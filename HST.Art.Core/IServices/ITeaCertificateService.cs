using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HST.Art.Core
{
    public interface ITeaCertificateService : IBaseService
    {
        List<TeaCertificate> GetPage(FilterEntityModel filterModel, out int totalNum);
        List<TeaCertificate> GetAll(FilterEntityModel filterModel);
        TeaCertificate Get(int id);
        TeaCertificate GetByNumber(string number);
        bool Update(TeaCertificate teaInfo);
        bool Add(TeaCertificate teaInfo);
        bool Add(List<TeaCertificate> teaInfos, out List<TeaCertificate> failList);
    }
}

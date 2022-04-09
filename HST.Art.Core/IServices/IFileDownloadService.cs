using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HST.Art.Core
{
    public interface IFileDownloadService : IBaseService
    {
        List<FileDownload> GetPage(FilterEntityModel filterModel, out int totalNum);
        List<FileDownload> GetAll(FilterEntityModel filterModel);
        FileDownload Get(int id);
        bool Update(FileDownload fileDownloadInfo);
        bool Add(FileDownload fileDownloadInfo);
    }
}

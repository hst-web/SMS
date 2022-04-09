using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HST.Art.Core
{
    public interface IUserService: IBaseService
    {
        List<User> GetPage(FilterEntityModel filterModel, out int totalNum);
        List<User> GetAll(FilterEntityModel filterModel);
        User Get(int id);
        User GetByPhone(string phoneNum);
        User GetByUserName(string userName);
        User GetByEmail(string Email);
        bool Update(User userInfo);
        bool Add(User userInfo);
        bool UpdatePassword(int id, string password);
    }
}

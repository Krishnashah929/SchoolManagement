using SM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Repositories.IRepository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int UserId);
        User Update(User user);
        User Delete(User user);
        //void Insert(User user);
        //void Update(User user);
        //void Delete(User user);
    }
}

using SM.Entity;
using SM.Repositories.IRepository;
using System;
using System.Collections.Generic;
using SM.Web.Data;
using System.Linq;

namespace SM.Repositories.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SchoolManagementContext _schoolManagementContext;

        public UserRepository(SchoolManagementContext schoolManagementContext)
        {
            _schoolManagementContext = schoolManagementContext;
        }

        public IEnumerable<User> GetAll()
        {
           return _schoolManagementContext.Users.ToList();
        }

        public User GetById(int UserId)
        {
            return _schoolManagementContext.Users.Where(x => x.UserId == UserId).FirstOrDefault();
        }
    }
}

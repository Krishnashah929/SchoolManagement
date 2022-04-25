using SM.Repositories.IRepository;
using SM.Web.Data;

namespace SM.Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private SchoolManagementContext _schoolManagementContext;
        private IUserRepository _userRepository;
        public UnitOfWork(SchoolManagementContext schoolManagementContext)
        {
            _schoolManagementContext = schoolManagementContext;
        }  
        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository = _userRepository ?? new UserRepository(_schoolManagementContext);
            }
        }
        public void Save()
        {
            _schoolManagementContext.SaveChanges();
        }
    }
}

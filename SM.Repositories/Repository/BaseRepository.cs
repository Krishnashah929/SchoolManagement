using SM.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SM.Web.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SM.Repositories.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly SchoolManagementContext schoolManagementContext;
        private readonly DbSet<T> databaseSet;

        public BaseRepository(SchoolManagementContext schoolManagementContext)
        {
            this.schoolManagementContext = schoolManagementContext;
            databaseSet = schoolManagementContext.Set<T>();
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}

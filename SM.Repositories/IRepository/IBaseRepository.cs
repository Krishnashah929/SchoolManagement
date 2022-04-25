using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Repositories.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        void Add(T entity);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}

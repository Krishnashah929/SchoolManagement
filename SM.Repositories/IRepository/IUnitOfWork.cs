﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Repositories.IRepository
{
    public interface IUnitOfWork
    {  
        IUserRepository UserRepository { get; }

        void Save();
    }
}

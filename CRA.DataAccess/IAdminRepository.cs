﻿using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public interface IAdminRepository
    {
        IEnumerable<Admin> GetAllAdmin();
        Admin GetAdminById(Guid id);
    }
}

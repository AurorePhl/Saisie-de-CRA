﻿using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public interface IPeriodRepository
    {
        IEnumerable<Period> GetAllPeriods();
    }
}

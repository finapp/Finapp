﻿using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface IDebtorViewModelService
    {
        DebtorListViewModel GetAllDebtorsViewModel();

        DebtorListViewModel GetWithDebetDebtorsViewModel();
    }
}

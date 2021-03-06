﻿using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finapp.IServices
{
    public interface ICreditorViewModelService
    {
        CreditorListViewModel GetAllCreditorsViewModel();

        CreditorListViewModel GetWithBalanceCreditorsViewModel();
    }
}

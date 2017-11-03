using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finapp.ViewModels
{
    public class CreditorViewModel
    {
        public string Username { get; set; }
        public int ROI { get; set; }
        public int EROI { get; set; }
        public int Balance { get; set; }
        public int FinappBalance { get; set; }
    }
}
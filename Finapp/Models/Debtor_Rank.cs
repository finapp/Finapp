//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Finapp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Debtor_Rank
    {
        public int Debtor_Rank_Id { get; set; }
        public int Debtor_Id { get; set; }
        public Nullable<int> Associate_Counter { get; set; }
        public Nullable<int> Delta_APR { get; set; }
        public Nullable<int> Trials { get; set; }
    
        public virtual Debtor Debtor { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class FinapEntities : DbContext
    {
        public FinapEntities()
            : base("name=FinapEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Creditor> Creditor { get; set; }
        public virtual DbSet<Creditor_Account> Creditor_Account { get; set; }
        public virtual DbSet<Debtor> Debtor { get; set; }
        public virtual DbSet<Debtor_Account> Debtor_Account { get; set; }
        public virtual DbSet<Transaction_Out> Transaction_Out { get; set; }
    }
}

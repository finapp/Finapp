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
    
    public partial class FinapEntities1 : DbContext
    {
        public FinapEntities1()
            : base("name=FinapEntities1")
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
        public virtual DbSet<Return_Transaction> Return_Transaction { get; set; }
        public virtual DbSet<Associate> Associate { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BytonOpcService.DBOperator
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BytonEntities : DbContext
    {
        public BytonEntities()
            : base("name=BytonEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<dev_diagnosisRecord> dev_diagnosisRecord { get; set; }
        public virtual DbSet<dev_eventRecord> dev_eventRecord { get; set; }
        public virtual DbSet<dev_productionRecord> dev_productionRecord { get; set; }
        public virtual DbSet<dev_realTimeStationData> dev_realTimeStationData { get; set; }
        public virtual DbSet<dev_stationReadRecord> dev_stationReadRecord { get; set; }
        public virtual DbSet<dev_sys_status> dev_sys_status { get; set; }
        public virtual DbSet<dev_sys_statusRecord> dev_sys_statusRecord { get; set; }
    }
}

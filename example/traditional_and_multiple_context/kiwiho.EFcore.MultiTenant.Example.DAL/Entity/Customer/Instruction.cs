using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using kiwiho.EFcore.MultiTenant.Example.DAL.Entity.Store;

namespace kiwiho.EFcore.MultiTenant.Example.DAL.Entity.Customer
{
    public class Instruction
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double TotalAmount { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

    }
}

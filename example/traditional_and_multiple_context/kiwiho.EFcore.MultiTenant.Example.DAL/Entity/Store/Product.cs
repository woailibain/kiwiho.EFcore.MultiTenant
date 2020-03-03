using System;
using System.ComponentModel.DataAnnotations;

namespace kiwiho.EFcore.MultiTenant.Example.DAL.Entity.Store
{
    public class Product
    {
        [Key]
        public int Id { get; set; } 

        [StringLength(50), Required]
        public string Name { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        public double? Price { get; set; }
    }
}

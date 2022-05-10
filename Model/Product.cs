using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace NorthwindConsole.Model
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        [Required(ErrorMessage = "ProductName missing")]
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "QuantityPerUnit missing")]
        public string QuantityPerUnit { get; set; }
        [Required(ErrorMessage = "UnitPrice missing")]
        public decimal? UnitPrice { get; set; }
        [Required(ErrorMessage = "UnitsInStock missing")]
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        public override string ToString()
        {
            return $"{this.ProductName}, id:{this.ProductId}\nCategory: {this.Category.CategoryName}\nSupplier: {this.Supplier.CompanyName}\nQuantity Per Unit: {this.QuantityPerUnit}\nUnit Price: {this.UnitPrice:C}\nUnits In Stock: {this.UnitsInStock}\nUnits In Order: {this.UnitsOnOrder}\nReorder Level: {this.ReorderLevel}\nDiscontinued: {this.Discontinued}";
        }

        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

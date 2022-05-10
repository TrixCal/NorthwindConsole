using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace NorthwindConsole.Model
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Name is missing")]
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public override string ToString(){
            return $"{this.CategoryName}, id:{this.CategoryId}\nDescription: {this.Description}";
        }

        public virtual ICollection<Product> Products { get; set; }
    }
}

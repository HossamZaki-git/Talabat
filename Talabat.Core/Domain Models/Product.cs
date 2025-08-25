using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Domain_Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public double Price { get; set; }
        public Brand brand { get; set; }
        public int BrandID { get; set; }
        public Category category { get; set; }
        public int CategoryID { get; set; }
    }
}

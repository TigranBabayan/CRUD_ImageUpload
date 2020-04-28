using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCrud.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }

        public IFormFile ImageName { get; set; }
    }
}

﻿using System.ComponentModel.DataAnnotations;

namespace bano_mart_mvc.Models
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        [Range(1,100)]
        public int Quantity { get; set; } = 1;
    }
}

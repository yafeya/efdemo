﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfDemo
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

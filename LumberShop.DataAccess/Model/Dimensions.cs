﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.DataAccess.Model
{
    public class Dimensions
    { 
        public int Id { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int? Height { get; set; }
    }
}

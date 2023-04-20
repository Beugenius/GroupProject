﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.ViewModels
{
    public class ItemDesc
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public Decimal Cost { get; set; }
        public int LineItemNum { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcApi.Models
{
    public class CreatePalletDto
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int BoardId { get; set; }
    }
}

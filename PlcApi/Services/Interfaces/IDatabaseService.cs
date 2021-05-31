﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Entities;
using PlcApi.Models;
using S7.Net;

namespace PlcApi.Services.Interfaces
{
    public interface IDatabaseService
    {
        public int AddPlcToDb(PlcEntity dto);
    }
}

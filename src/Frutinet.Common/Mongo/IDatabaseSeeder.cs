﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Common.Mongo
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}
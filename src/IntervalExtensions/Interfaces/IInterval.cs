﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervalExtensions
{

    public interface IInterval<T>
        where T : struct
    {
        T? Start { get; set; }
        T? End { get; set; }
    }
}

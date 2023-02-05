﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntervalExtensions.Interfaces;

namespace IntervalExtensions.Comparers
{
    public class OverlapComparer<T> : IIntervalOverlapComparer<T>
        where T : struct, IComparable<T>, IComparable
    {
        private readonly bool _hasInclusiveEnd;

        public OverlapComparer(bool hasInclusiveEnd)
        {
            _hasInclusiveEnd = hasInclusiveEnd;
        }

        public int Compare(IInterval<T>? x, IInterval<T>? y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException();
            }

            return x.Compare(y, _hasInclusiveEnd);
        }
    }
}

﻿using InfinityComparable;

namespace IntervalRecord
{
    public static partial class Interval
    {
        public static IEnumerable<T> Iterate<T>(this Interval<T> value, Func<T, T> AddStep)
            where T : struct, IEquatable<T>, IComparable<T>, IComparable
        {
            var start = value.StartInclusive ? value.Start.Value : AddStep(value.Start.Value);
            return value.Iterate(start, AddStep);
        }

        public static IEnumerable<T> Iterate<T>(this Interval<T> value, T start, Func<T, T> AddStep)
            where T : struct, IEquatable<T>, IComparable<T>, IComparable
        {
            if (value.Contains(start) && !value.Start.IsInfinite && !value.End.IsInfinite)
            {
                for (var i = start; value.EndInclusive ? i <= value.End : i < value.End; i = AddStep(i))
                {
                    yield return i;
                }
            }
        }
    }
}
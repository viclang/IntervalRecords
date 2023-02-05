﻿using InfinityComparable;

namespace IntervalRecord
{
    public static partial class Interval
    {
        public static Interval<T> Intersect<T>(this Interval<T> value, Interval<T> other)
            where T : struct, IComparable<T>, IComparable
        {
            if (!value.Overlaps(other, true))
            {
                throw new ArgumentOutOfRangeException("other", "Intersection is only supported for connected intervals.");
            }
            var maxByStart = MaxBy(value, other, x => x.Start);
            var minByEnd = MinBy(value, other, x => x.End);

            var startInclusive = value.Start == other.Start
                ? value.StartInclusive && other.StartInclusive
                : maxByStart.StartInclusive;

            var endInclusive = value.End == other.End
                ? value.EndInclusive && other.EndInclusive
                : minByEnd.EndInclusive;

            return value with { Start = maxByStart.Start, End = minByEnd.End, StartInclusive = startInclusive, EndInclusive = endInclusive };
        }
    }
}

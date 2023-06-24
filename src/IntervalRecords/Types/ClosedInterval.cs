﻿using IntervalRecords.Types;
using System;
using Unbounded;

namespace IntervalRecords;
public sealed record ClosedInterval<T> : Interval<T>, IOverlaps<ClosedInterval<T>>
    where T : struct, IEquatable<T>, IComparable<T>, IComparable
{

    public static new readonly ClosedInterval<T> Empty = new(Unbounded<T>.NaN, Unbounded<T>.NaN);

    public static new readonly ClosedInterval<T> Unbounded = new(Unbounded<T>.NegativeInfinity, Unbounded<T>.PositiveInfinity);

    public override IntervalType IntervalType => IntervalType.Closed;

    /// <summary>
    /// Gets a value indicating whether the start of the interval is inclusive.
    /// The Start of a ClosedInterval is inclusive, except when it is Unbounded.
    /// </summary>
    public override bool StartInclusive => true;

    /// <summary>
    /// Gets a value indicating whether the end of the interval is inclusive.
    /// The End of a ClosedInterval is inclusive, except when it is Unbounded.
    /// </summary>
    public override bool EndInclusive => true;

    public override bool IsValid => Start <= End && !Start.IsNaN || Start.IsNaN && End.IsNaN;

    public override bool IsEmpty => !IsValid || Start.IsNaN && End.IsNaN;

    public override bool IsSingleton => Start == End;

    /// <summary>
    /// Creates a Closed interval.
    /// </summary>
    /// <param name="start">Represents the start value of the interval.</param>
    /// <param name="end">Represents the end value of the interval.</param>
    public ClosedInterval(Unbounded<T> start, Unbounded<T> end) : base(start, end)
    {
    }


    public static ClosedInterval<T> Singleton(T value) => new ClosedInterval<T>(value, value);


    public static ClosedInterval<T> LeftBounded(T start) => new ClosedInterval<T>(start, Unbounded<T>.PositiveInfinity);

    public static ClosedInterval<T> RightBounded(T end) => new ClosedInterval<T>(Unbounded<T>.NegativeInfinity, end);

    protected override int CompareStart(Interval<T> other)
    {
        var result = Start.CompareTo(other.Start);
        if (result == 0 && !other.StartInclusive)
        {
            return 1;
        }
        return result;
    }


    protected override int CompareEnd(Interval<T> other)
    {
        var result = End.CompareTo(other.End);
        if (result == 0 && !other.EndInclusive)
        {
            return 1;
        }
        return result;
    }

    protected override IntervalOverlapping CompareEndStart(Interval<T> other)
    {
        if (End < other.Start || (End == other.Start && !other.StartInclusive))
        {
            return IntervalOverlapping.Before;
        }
        if (End == other.Start)
        {
            return IntervalOverlapping.Meets;
        }
        return IntervalOverlapping.Overlaps;
    }

    protected override IntervalOverlapping CompareStartEnd(Interval<T> other)
    {
        if (Start > other.End || (Start == other.End && !other.EndInclusive))
        {
            return IntervalOverlapping.After;
        }
        if (Start == other.End)
        {
            return IntervalOverlapping.MetBy;
        }
        return IntervalOverlapping.OverlappedBy;
    }

    /// <summary>
    /// Returns a boolean value indicating if the current interval contains the specified value.
    /// </summary>
    /// <param name="value">The value to check if it is contained by the current interval</param>
    /// <returns></returns>
    public override bool Contains(Unbounded<T> value)
    {
        return Start <= value && value <= End;
    }

    public bool Overlaps(ClosedInterval<T> other)
    {
        return IsConnected(other);
    }

    public override bool Overlaps(Interval<T> other)
    {
        return Start < other.End && other.Start < End
            || Start == other.End && other.EndInclusive
            || End == other.Start && other.StartInclusive;
    }

    public override bool IsConnected(Interval<T> other)
    {
        return Start <= other.End && other.Start <= End;
    }

    public static bool operator >(ClosedInterval<T> left, ClosedInterval<T> right)
    {
        return left.End >= right.End && right.Start > left.Start;
    }

    public static bool operator <(ClosedInterval<T> left, ClosedInterval<T> right)
    {
        return left.End <= right.End && right.Start < left.Start;
    }

    public static bool operator >=(ClosedInterval<T> left, ClosedInterval<T> right)
    {
        return left == right || left > right;
    }

    public static bool operator <=(ClosedInterval<T> left, ClosedInterval<T> right)
    {
        return left == right || left < right;
    }
}
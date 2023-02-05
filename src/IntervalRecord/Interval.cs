﻿using InfinityComparable;
using IntervalRecord.Enums;
using System.Diagnostics.Contracts;
using System.Text;

namespace IntervalRecord
{
    public readonly record struct Interval<T> : IComparable<Interval<T>>
        where T : struct, IEquatable<T>, IComparable<T>, IComparable
    {
        private readonly Infinity<T> _start;
        private readonly Infinity<T> _end;
        private readonly bool _startInclusive;
        private readonly bool _endInclusive;

        public Infinity<T> Start { get => _start; init => _start = new Infinity<T>(value, false); }
        public Infinity<T> End { get => _end; init => _end = new Infinity<T>(value, true); }
        public bool StartInclusive { get => _startInclusive; init { _startInclusive = !Start.IsInfinite && value; } }
        public bool EndInclusive { get => _endInclusive; init { _endInclusive = !End.IsInfinite && value; } }
        public bool IsValid => Start.IsInfinite || End.IsInfinite || End.CompareTo(Start) >= 0;

        public Interval()
            : this(Infinity<T>.NegativeInfinity, Infinity<T>.PositiveInfinity, false, false)
        {
        }

        public Interval(Infinity<T> start, Infinity<T> end, bool startInclusive, bool endInclusive)
        {
            _start = -start;
            _end = +end;
            _startInclusive = !_start.IsInfinite && startInclusive;
            _endInclusive = !_end.IsInfinite && endInclusive;
        }

        [Pure]
        public InfinityState GetInfinityState() => (Start.IsInfinite, End.IsInfinite) switch
        {
            (false, false) => InfinityState.Bounded,
            (true, true) => InfinityState.Unbounded,
            (true, false) => InfinityState.RightBounded,
            (false, true) => InfinityState.LeftBounded
        };

        [Pure]
        public bool IsHalfBounded() => Start.IsInfinite && !End.IsInfinite || !Start.IsInfinite && End.IsInfinite;

        [Pure]
        public BoundaryType GetBoundaryType() => (StartInclusive, EndInclusive) switch
        {
            (true, true) => BoundaryType.Closed,
            (true, false) => BoundaryType.ClosedOpen,
            (false, true) => BoundaryType.OpenClosed,
            (false, false) => BoundaryType.Open,
        };

        [Pure]
        public bool IsHalfOpen() => StartInclusive && !EndInclusive || !StartInclusive && EndInclusive;

        [Pure]
        public bool IsEmpty() => (GetBoundaryType() != BoundaryType.Closed && Start == End) || !IsValid;

        [Pure]
        public bool IsSingleton() => GetBoundaryType() == BoundaryType.Closed && Start == End;

        [Pure]
        public bool Overlaps(Interval<T> other, bool includeHalfOpen = false)
            => !this.IsBefore(other, includeHalfOpen) && !this.IsAfter(other, includeHalfOpen);

        [Pure]
        public bool Contains(T other)
        {
            return StartInclusive
                ? Start.CompareTo(other) <= 0
                : Start.CompareTo(other) == -1
                && EndInclusive
                ? End.CompareTo(other) >= 0
                : End.CompareTo(other) == 1;
        }

        public static bool operator >(Interval<T> a, Interval<T> b)
            => a.End.CompareTo(b.End) == 1
                || a.End.CompareTo(b.End) == 0 && a.Start.CompareTo(b.Start) == -1;
        public static bool operator <(Interval<T> a, Interval<T> b)
            => a.End.CompareTo(b.End) == -1
                || a.End.CompareTo(b.End) == 0 && a.Start.CompareTo(b.Start) == 1;
        public static bool operator >=(Interval<T> a, Interval<T> b) => a == b || a > b;
        public static bool operator <=(Interval<T> a, Interval<T> b) => a == b || a < b;

        [Pure]
        public int CompareTo(Interval<T> other)
        {
            if (this > other)
            {
                return 1;
            }
            if (this < other)
            {
                return -1;
            }
            return 0;
        }

        [Pure]
        public override string ToString()
        {
            return new StringBuilder()
                .Append(StartInclusive ? "[" : "(")
                .Append(Start)
                .Append(", ")
                .Append(End)
                .Append(EndInclusive ? "]" : ")")
                .ToString();
        }

        [Pure]
        public void Deconstruct(out Infinity<T> start, out Infinity<T> end, out bool startInclusive, out bool endInclusive)
        {
            start = Start;
            end = End;
            startInclusive = StartInclusive;
            endInclusive = EndInclusive;
        }
    }
}

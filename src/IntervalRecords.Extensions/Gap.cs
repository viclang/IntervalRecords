﻿namespace IntervalRecords.Extensions
{
    public static partial class IntervalExtensions
    {
        /// <summary>
        /// Returns the gap between two intervals, or null if the two intervals overlap.
        /// </summary>
        /// <typeparam name="T">The type of the interval bounds.</typeparam>
        /// <param name="first">The first interval.</param>
        /// <param name="second">The second interval.</param>
        /// <returns>The gap between the two intervals, if any, or null if the two intervals overlap.</returns>
        public static Interval<T>? Gap<T>(this Interval<T> first, Interval<T> second)
            where T : struct, IEquatable<T>, IComparable<T>, IComparable
            => first.GetOverlap(second) switch
            {
                IntervalOverlapping.Before => Interval.Create(first.End, second.Start, !first.EndInclusive, !second.StartInclusive),
                IntervalOverlapping.After => Interval.Create(second.End, first.Start, !second.EndInclusive, !first.StartInclusive),
                _ => null
            };

        /// <summary>
        /// Returns the gap between two intervals, or an empty interval if the two intervals overlap.
        /// </summary>
        /// <typeparam name="T">The type of the interval bounds.</typeparam>
        /// <param name="first">The first interval.</param>
        /// <param name="second">The second interval.</param>
        /// <returns>The gap between the two intervals, or a default interval if the two intervals overlap.</returns>
        public static Interval<T> GapOrEmpty<T>(this Interval<T> first, Interval<T> second)
            where T : struct, IEquatable<T>, IComparable<T>, IComparable
            => first.GetOverlap(second) switch
            {
                IntervalOverlapping.Before => Interval.Create(first.End, second.Start, !first.EndInclusive, !second.StartInclusive),
                IntervalOverlapping.After => Interval.Create(second.End, first.Start, !second.EndInclusive, !first.StartInclusive),
                _ => Interval<T>.Empty(first.IntervalType)
            };

        /// <summary>
        /// Returns the complement (or Gaps) of a collection of intervals.
        /// </summary>
        /// <typeparam name="T">The type of the interval bounds.</typeparam>
        /// <param name="source">The collection of intervals.</param>
        /// <returns>The complement of the collection of intervals, represented as a sequence of intervals.</returns>
        public static IEnumerable<Interval<T>> Complement<T>(
            this IEnumerable<Interval<T>> source)
            where T : struct, IEquatable<T>, IComparable<T>, IComparable
            => source.Pairwise((a, b) => a.Gap(b));
    }
}
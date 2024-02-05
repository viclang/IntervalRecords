﻿//using IntervalRecords.Endpoints;
//using System.Net;
//using System.Text.RegularExpressions;

//namespace IntervalRecords.Extensions
//{
//    public static class IntervalParser
//    {
//        private static readonly Regex intervalRegex = new(@"(?:\[|\()(?:[^[\](),]*,[^,()[\]]*)(?:\)|\])");
//        private const string intervalNotFound = "Interval not found in string. Please provide an interval string in correct format";

//        /// <summary>
//        /// Parses a string representation of an interval and returns a new interval object.
//        /// </summary>
//        /// <param name="value">The string representation of the interval to parse.</param>
//        /// <returns>A new interval object representing the interval described by the input string.</returns>
//        public static Interval<T> Parse<T>(string value)
//            where T : struct, IEquatable<T>, IComparable<T>, ISpanParsable<T>
//        {
//            var match = intervalRegex.Match(value);
//            if (!match.Success)
//            {
//                throw new ArgumentException(intervalNotFound);
//            }
//            return ParseInterval<T>(match.Value);
//        }

//        /// <summary>
//        /// Attempts to parse a string representation of an interval and returns a boolean indicating whether or not the parse was successful.
//        /// </summary>
//        /// <typeparam name="T">The type of values represented in the interval.</typeparam>
//        /// <param name="value">The string representation of the interval to parse.</param>
//        /// <param name="result">The resulting interval object if the parse was successful, or null if the parse was not successful.</param>
//        /// <returns>True if the parse was successful, False otherwise.</returns>
//        public static bool TryParse<T>(string value, out Interval<T>? result)
//            where T : struct, IEquatable<T>, IComparable<T>, ISpanParsable<T>
//        {
//            try
//            {
//                result = Parse<T>(value);
//                return true;
//            }
//            catch
//            {
//                result = null;
//                return false;
//            }
//        }

//        /// <summary>
//        /// Parses all intervals within a string and returns an enumerable collection of interval objects.
//        /// </summary>
//        /// <typeparam name="T">The type of values represented in the interval.</typeparam>
//        /// <param name="value">The string representation of the intervals to parse.</param>
//        /// <returns>An enumerable collection of interval objects representing the intervals described by the input string.</returns>
//        public static IEnumerable<Interval<T>> ParseAll<T>(string value)
//            where T : struct, IEquatable<T>, IComparable<T>, ISpanParsable<T>
//        {
//            var matches = intervalRegex.Matches(value);
//            return matches.Select(match => ParseInterval<T>(match.Value));
//        }

//        private static Interval<T> ParseInterval<T>(string value)
//            where T : struct, IEquatable<T>, IComparable<T>, ISpanParsable<T>
//        {
//            var parts = value.Split(',');
//            var startString = parts[0].Trim();
//            var endString = parts[1].Trim();

//            var start = Endpoint<T>.Parse(startString[1..]);
//            var end = Endpoint<T>.Parse(endString[..^1]);

//            return IntervalFactory.Create(
//                start.Value,
//                end.Value,
//                value.StartsWith('['),
//                value.EndsWith(']'));
//        }
//    }
//}
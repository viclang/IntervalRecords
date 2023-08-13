﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IntervalRecords.Tests.TestData;
public class Int32DisjointClassData : IEnumerable<object[]>
{
    private const int _offset = 1;
    private readonly static Interval<int> _reference = new ClosedInterval<int>(5, 9);

    public IEnumerator<object[]> GetEnumerator()
    {
        var testData = new List<object[]>();
        foreach (var intervalType in (IntervalType[])Enum.GetValues(typeof(IntervalType)))
        {
            var builder = new Int32OverlapTestDataBuilder(_reference.Canonicalize(intervalType, 0), _offset);
            if (intervalType == IntervalType.Open)
            {
                builder.WithMeets()
                    .WithMetBy();
            }
            testData.AddRange(
                (List<object[]>)builder
                    .WithBefore()
                    .WithAfter());
        }
        return testData.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
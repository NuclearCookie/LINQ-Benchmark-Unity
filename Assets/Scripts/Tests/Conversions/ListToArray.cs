using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tests
{
    public class ListToArray
    {
        private List<int> list;
        private int[] result;

        [SetUp]
        public void Setup()
        {
            int counter = 0;
            list = Enumerable.Range(0, 10000).Select(x => counter++).ToList();
        }

        // This is provided by default by the List class, so there is no comparison.

        [Test, Performance]
        public void NoLinq()
        {
            Measure.Method(()=>
            {
                result = list.ToArray();
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }
 
   }
}

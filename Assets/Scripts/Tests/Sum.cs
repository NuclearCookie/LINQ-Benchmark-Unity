using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tests
{
    public class Sum
    {
        private List<int> list;
        [SetUp]
        public void Setup()
        {
            int counter = 0;
            list = Enumerable.Range(0, 10000).Select(x => counter++).ToList();
        }

        [Test, Performance]
        public void Linq()
        {
            Measure.Method(( )=>
            {
                var result = list.Sum();
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }
 
        [Test, Performance]
        public void NoLinq()
        {
            Measure.Method(()=>
            {
                var count = list.Count;
                var result = 0;
                for( int i = 0; i < count; ++i )
                {
                    result += list[i];
                }
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }
   }
}

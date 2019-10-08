using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tests
{
    public class DistinctElements
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
                list = list.Distinct().ToList();
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }
 
        [Test, Performance]
        public void NoLinqNaiveNoPrealloc()
        {
            Measure.Method(()=>
            {
                var list_copy = new List<int>();
                var count = list.Count;
                for( int i = 0; i < count; ++i )
                {
                    var elem = list[i];
                    if ( !list_copy.Contains(elem))
                    {
                        list_copy.Add(elem);
                    }
                }
                list = list_copy;
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }

        [Test, Performance]
        public void NoLinqNaivePrealloc()
        {
            Measure.Method(()=>
            {
                var list_copy = new List<int>(list.Count);
                var count = list.Count;
                for( int i = 0; i < count; ++i )
                {
                    var elem = list[i];
                    if ( !list_copy.Contains(elem))
                    {
                        list_copy.Add(elem);
                    }
                }
                list = list_copy;
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }

 
        [Test, Performance]
        public void NoLinqHashSet()
        {
            Measure.Method(( )=>
            {
                list = new List<int>(new HashSet<int>(list));
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }
    } 

}

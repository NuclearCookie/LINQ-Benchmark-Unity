using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tests
{
    public class WhereDivideBy3
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
                list = list.Where( x => x % 3 == 0).ToList();
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }
 
        [Test, Performance]
        public void LinqQuery()
        {
            Measure.Method(()=>
            {
                list = ( from n in list 
                         where n % 3 == 0 
                         select n ).ToList();
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
                var list_copy = new List<int>();
                var count = list.Count;
                for( int i = 0; i < count; ++i )
                {
                    if ( list[i] % 3 == 0)
                    {
                        list_copy.Add(list[i]);
                    }
                }
                list = list_copy;
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }
 
   }
}

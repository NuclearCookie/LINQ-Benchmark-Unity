using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tests
{
    public class ArrayToList
    {
        private int[] list;
        private List<int> result;

        [SetUp]
        public void Setup()
        {
            int counter = 0;
            list = Enumerable.Range(0, 10000).Select(x => counter++).ToArray();
        }

        [Test, Performance]
        public void Linq()
        {
            Measure.Method(( )=>
            {
                result = list.ToList();
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
                result = new List<int>(list);
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }
 
   }
}

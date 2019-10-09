using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tests
{
    public class SelectSubType 
    {
        private class DummyClass
        {
            public float Value;
        }

        private List<DummyClass> list;

        [SetUp]
        public void Setup()
        {
            list = Enumerable.Range(0, 10000).Select(x => new DummyClass { Value = Random.value } ).ToList();
        }

        [Test, Performance]
        public void Linq()
        {
            Measure.Method(() =>
            {
                var values = list.Select(x => x.Value).ToList();
            } ).WarmupCount(10)
               .MeasurementCount(10)
               .IterationsPerMeasurement(5)
               .GC()
               .Run();
        }

        [Test, Performance]
        public void LinqQuery()
        {
            Measure.Method(() =>
            {
                var values = 
                    ( from n in list
                    select n.Value ).ToList();
            } ).WarmupCount(10)
               .MeasurementCount(10)
               .IterationsPerMeasurement(5)
               .GC()
               .Run();
        }


        [Test, Performance]
        public void NoLinq()
        {
            Measure.Method(() =>
            {
                var values = new List<float>(list.Count);
                var count = list.Count;
                for( int i = 0; i < count; ++i )
                {
                    values.Add(list[i].Value);
                }
            } ).WarmupCount(10)
               .MeasurementCount(10)
               .IterationsPerMeasurement(5)
               .GC()
               .Run();
        }

 }
}

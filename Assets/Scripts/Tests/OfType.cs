using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tests
{
    public class OfType
    {
        private interface IBase { };
        private class Child1 : IBase { };
        private class Child2 : IBase { };
        private class Child3 : IBase { };
            
        private List<IBase> list;
        [SetUp]
        public void Setup()
        {
            list = RandomList();
        }

        private List<IBase> RandomList()
        {
            return Enumerable.Range( 0, 10000 ).Select<int, IBase>( x =>
            {
                var value = UnityEngine.Random.value;
                if ( value < 0.33 )
                {
                    return new Child1();
                }
                else if ( value < 0.66 )
                {
                    return new Child2();
                }
                else
                {
                    return new Child3();
                }
            } ).ToList();
        }

        [Test, Performance]
        public void Linq()
        {
            Measure.Method(( )=>
            {
                var new_list = list.OfType<Child2>().ToList();
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }
 
        [Test, Performance]
        public void NoLinqByType()
        {
            List<IBase> results = null;
            Measure.Method(()=>
            {
                results = GetListOfSubTypeByType(typeof(Child2));
            } ).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }

        [Test, Performance]
        public void NoLinqByGeneric()
        {
            List<Child2> results = null;
            Measure.Method(()=>
            {
                results = GetListOfSubTypeByGeneric<Child2>();
            } ).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }

        private List<IBase> GetListOfSubTypeByType(Type child_type)
        {
            var new_list = new List<IBase>();
            var count = list.Count;
            for ( int i = 0; i < count; ++i )
            {
                var elem = list [ i ];
                if ( elem.GetType() == child_type )
                {
                    new_list.Add( elem );
                }
            }
            return new_list;
        }

        private List<T> GetListOfSubTypeByGeneric<T>() where T : IBase
        {
            var new_list = new List<T>();
            var count = list.Count;
            for ( int i = 0; i < count; ++i )
            {
                var elem = list [ i ];
                if ( elem.GetType() == typeof( T ) )
                {
                    new_list.Add( (T) elem );
                }
            }
            return new_list;
        }

    }
}

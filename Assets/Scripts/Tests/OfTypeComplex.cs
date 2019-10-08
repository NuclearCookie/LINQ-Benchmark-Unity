using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tests
{
    public class OfTypeComplex
    {
        private interface IBase { };
        private class Child1 : IBase { };
        private class Child2 : IBase { };
        private class Child3 : IBase { };
            
        private List<IBase> list1;
        private List<IBase> list2;
        private List<IBase> list3;
        private List<IBase> list4;

        [SetUp]
        public void Setup()
        {
            list1 = RandomListNoChild2();
            list2 = RandomListNoChild2();
            list3 = RandomListNoChild2();
            list4 = RandomList();
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

         private List<IBase> RandomListNoChild2()
        {
            return Enumerable.Range( 0, 10000 ).Select<int, IBase>( x =>
            {
                var value = UnityEngine.Random.value;
                if ( value < 0.5 )
                {
                    return new Child1();
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
                var result = list1.Concat(list2).Concat(list3).Concat(list4).OfType<Child2>().FirstOrDefault();
            }).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }

        [Test, Performance]
        public void NoLinqByType()
        {
            IBase results = null;
            Measure.Method(()=>
            {
                results = GetFirstSubTypeByType(list1, typeof(Child2));
                if ( results != null )
                {
                    return;
                }
                results = GetFirstSubTypeByType(list2, typeof(Child2));
                if ( results != null )
                {
                    return;
                }
                results = GetFirstSubTypeByType(list3, typeof(Child2));
                if ( results != null )
                {
                    return;
                }
                results = GetFirstSubTypeByType(list4, typeof(Child2));
                if ( results != null )
                {
                    return;
                }
            } ).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }

        [Test, Performance]
        public void NoLinqByGeneric()
        {
            Child2 result = null;
            Measure.Method(()=>
            {
                result = GetListOfSubTypeByGeneric<Child2>(list1);
                if ( result != null )
                {
                    return;
                }
                result = GetListOfSubTypeByGeneric<Child2>(list2);
                if ( result != null )
                {
                    return;
                }
                result = GetListOfSubTypeByGeneric<Child2>(list3);
                if ( result != null )
                {
                    return;
                }
                result = GetListOfSubTypeByGeneric<Child2>(list4);
                if ( result != null )
                {
                    return;
                }
            } ).WarmupCount(10)
              .MeasurementCount(10)
              .IterationsPerMeasurement(5)
              .GC()
              .Run();
        }

        private IBase GetFirstSubTypeByType(List<IBase> list, Type child_type)
        {
            var count = list.Count;
            for ( int i = 0; i < count; ++i )
            {
                var elem = list [ i ];
                if ( elem.GetType() == child_type )
                {
                    return elem;
                }
            }
            return null;
        }

        private T GetListOfSubTypeByGeneric<T>(List<IBase> list) where T : IBase
        {
            var count = list.Count;
            for ( int i = 0; i < count; ++i )
            {
                var elem = list [ i ];
                if ( elem.GetType() == typeof( T ) )
                {
                    return (T)elem;
                }
            }
            return default(T);
        }

    }
}

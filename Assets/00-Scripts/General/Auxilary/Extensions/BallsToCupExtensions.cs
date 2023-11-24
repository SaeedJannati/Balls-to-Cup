using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BallsToCup.General
{
    public static class BallsToCupExtensions
    {
        #region Transfrom

        public static void ClearChildren(this Transform transform, int fromIndex = 0)
        {
            var childCount = transform.childCount;
            if (childCount == 0)
                return;
            for (int i = childCount - 1; i >= fromIndex; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }

        #endregion

        #region GameObject

        public static void ClearChildren(this GameObject go, int fromIndex = 0)
        {
            go.transform.ClearChildren(fromIndex);
        }

        public static Coroutine CallNextFrame(this MonoBehaviour mono, Action action)
        {
            return mono.StartCoroutine(CallNextFrameRoutine(action));
        }

        public static Coroutine CallWithDelay(this MonoBehaviour mono, Action action, float delay)
        {
            return mono.StartCoroutine(DelayedCallRoutine(action, delay));
        }

        static IEnumerator DelayedCallRoutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        static IEnumerator CallNextFrameRoutine(Action action)
        {
            yield return null;
            action?.Invoke();
        }

        #endregion

        #region ColourExtentions

        public static string GetHexadecimal(this Color colour)
        {
            return $"#{GetIntOfChannel(colour.r):X2}{GetIntOfChannel(colour.g):X2}{GetIntOfChannel(colour.b):X2}";
        }

        static int GetIntOfChannel(float value)
        {
            var output = (int)(value * 255);

            return output;
        }

        #endregion

        #region ObjectExtentions

        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        #endregion

        #region IEnumerables

        public static void Shuffle<T>(this List<T> list)
        {
            var rng = new System.Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        public static TSource MaxBy<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> selector)
        {
            var enumerable = items as TSource[] ?? items.ToArray();
            var max = enumerable.Select(selector).Max();
            return enumerable.FirstOrDefault(i => selector(i).Equals(max));
        }

        public static TSource MinBy<TSource, TResult>(this IEnumerable<TSource> items, Func<TSource, TResult> selector)
        {
            var enumerable = items as TSource[] ?? items.ToArray();
            var min = enumerable.Select(selector).Min();
            return enumerable.FirstOrDefault(i => selector(i).Equals(min));
        }

        public static List<T> GetUniqueMultipleElements<T>(this List<T> list, int elementsCount)
        {
            var randomGenerator = new System.Random(DateTime.Now.Millisecond);

            var outPut = list.OrderBy(x => randomGenerator.Next()).Take(elementsCount).ToList();
            return outPut;
        }

        #endregion

        #region TimeSpan

        public static string GetFormattedString(this TimeSpan timeSpan)
        {
            var outPut = "";
            if (timeSpan.Days > 0)
                outPut += $"{timeSpan.Days:D2}:";
            outPut += $"{timeSpan.Hours:D2}:";
            outPut += $"{timeSpan.Minutes:D2}':";
            outPut += $"{timeSpan.Seconds:D2}\"";
            return outPut;
        }

        #endregion
    }
}
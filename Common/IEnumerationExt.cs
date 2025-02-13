using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace c1tr00z.AssistLib.Common;

public static class IEnumerationExt {
    
    #region Class Implementation

    public static T Random<T>(this List<T> list) {
        return list[GD.RandRange(0, list.Count - 1)];
    }

    public static List<T> ToUniqueList<T>(this IEnumerable<T> enumerable) {
        var list = new List<T>();

        foreach (var item in enumerable) {
            if (!list.Contains(item)) {
                list.Add(item);
            }
        }
        
        return list;
    }

    public static Dictionary<TKey, TValue> ToUniqueDictionary<T, TKey, TValue>(this IEnumerable<T> enumerable,
        Func<T, TKey> keySelector, Func<T, TValue> valueSelector) {

        var dictionary = new Dictionary<TKey, TValue>();

        foreach (var item in enumerable) {
            var key = keySelector(item);
            if (!dictionary.ContainsKey(key)) {
                dictionary.Add(key, valueSelector(item));
            }
        }
        
        return dictionary;
    }

    public static void Shuffle<T>(this List<T> list) {
        var other = new List<T>(list);
        var targetSize = list.Count;
        list.Clear();
        while (list.Count < targetSize) {
            var item = other.Random();
            other.Remove(item);
            list.Add(item);
        }
    }

    public static List<T> ToShuffledList<T>(this List<T> list) {
        var newList = new List<T>(list);
        newList.Shuffle();
        return newList;
    }

    public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> enumerable) {
        foreach (var item in enumerable) {
            queue.Enqueue(item);
        }
    }

    public static IEnumerable<T2> SelectNotNull<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> selector) {
        var list = new List<T2>();

        foreach (var item in enumerable) {
            var resultItem = selector(item);

            if (resultItem == null) {
                continue;
            }
            
            list.Add(resultItem);
        }

        return list;
    }

    public static List<T> MinElements<T>(this List<T> list, Func<T, IComparable> selector) {
        var minElements = new List<T>();
        IComparable minValue = 0;
        
        for (var i = 0; i < list.Count; i++) {
            var item = list[i];
            var minValueCandidate = selector(item);
            if (i == 0) {
                minValue = minValueCandidate;
                minElements.Add(item);
                continue;
            }

            var compareResult = minValueCandidate.CompareTo(minValue);

            switch (compareResult) {
                case -1:
                    minValue = minValueCandidate;
                    minElements.Clear();
                    minElements.Add(item);
                    break;
                case 0:
                    minElements.Add(item);
                    break;
            }
        }
        
        return minElements;
    }

    public static T MinElement<T>(this List<T> list, Func<T, IComparable> selector) {
        return list.MinElements(selector).FirstOrDefault();
    }

    public static Queue<T> ToQueue<T>(this IEnumerable<T> items) {
        var queue = new Queue<T>();
        foreach (var item in items) {
            queue.Enqueue(item);
        }
        return queue;
    }

    #endregion
}
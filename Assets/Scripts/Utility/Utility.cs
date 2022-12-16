//***
// Author: Nate
// Description: Utility is a static class housing lots of useful helper methods
//***

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utility
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();
    /// <summary>
    /// Caches wait for seconds so you do not have to create new on Coroutines.
    /// Reduces trash for the garbage collector
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out WaitForSeconds wait)) { return wait; }

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    /// <summary>
    /// Returns true if mouse pointer is currently hovering the UI
    /// </summary>
    /// <returns></returns>
    public static bool PointerIsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    /// <summary>
    /// Destroys all the children of a given object
    /// </summary>
    /// <param name="t"></param>
    public static void DeleteAllChildren(this Transform t)
    {
        foreach(Transform child in t)
        {
            Object.Destroy(child.gameObject);
        }
    }
    
    /// <summary>
    ///  Fisher-Yates Shuffle that accepts an IList (array or a list etc) to shuffle the order
    /// </summary>
    /// <param name="collection"></param>
    /// <typeparam name="T"></typeparam>
    public static void ShuffleCollection<T>(IList<T> collection)
    {
        int collectionCount = collection.Count;
        for (int i = 0; i < collectionCount; i++) {
            // Pick a new index higher than current for each item in the IList
            int randomIndex = i + Random.Range(0, collectionCount - i);
            // Swap item into a new spot
            (collection[randomIndex], collection[i]) = (collection[i], collection[randomIndex]);
        }
    }
}
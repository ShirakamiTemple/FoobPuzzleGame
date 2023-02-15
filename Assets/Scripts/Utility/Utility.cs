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

    /// <summary>
    /// Checks quaternions for approximate equality
    /// </summary>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    public static bool Approximately(Quaternion q1, Quaternion q2)
    {
        return Quaternion.Dot(q1, q2) > 1f - 0.0000004f;
    }
    
    /// <summary>
    /// Checks vector3 for approximate equality
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static bool Approximately(Vector3 v1, Vector3 v2)
    {
        return Vector3.Dot(v1, v2) > 1f - 0.0000004f;
    }

    /// <summary>
    /// Give the absolute value of a vector3
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 Vector3Abs(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    /// <summary>
    /// Give the absolute value of a Quaternion
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    public static Quaternion QuaternionAbs(Quaternion q)
    {
        return new Quaternion(Mathf.Abs(q.x), Mathf.Abs(q.y), Mathf.Abs(q.z), Mathf.Abs(q.w));
    }
}
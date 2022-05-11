using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static T FindChild<T>(GameObject go, string name = null) where T : Object
    {
        foreach (T child in go.GetComponentsInChildren<T>())
        {
            if (string.IsNullOrEmpty(name) || child.name == name)
                return child;
        }

        return null;
    }

    public static List<T> FindChildren<T>(GameObject go, string name = null) where T : Object
    {
        List<T> collisions = new List<T>();
        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform child = go.transform.GetChild(i);
            if (child.name.Contains(name))
                collisions.Add(child.GetComponent<T>());
        }

        return collisions;
    }
}
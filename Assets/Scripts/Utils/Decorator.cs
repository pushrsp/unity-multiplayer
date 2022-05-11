using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Decorator
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        return Helper.GetOrAddComponent<T>(go);
    }
}
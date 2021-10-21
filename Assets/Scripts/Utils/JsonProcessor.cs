﻿using System.Collections;
using UnityEngine;

public class JsonProcessor
{
    /// <summary>
    /// service class to allow JsonUtility.FromJson parse json {array:[...]} into an object
    /// </summary>
    /// <typeparam name="T">Serializable class</typeparam>
    private class Wrapper<T>
    {
        public T[] array;
    }

    /// <summary>
    /// Parse json with array into array of objects
    /// </summary>
    /// <typeparam name="T">Serializable class</typeparam>
    /// <param name="jsonStr">json string with array</param>
    /// <returns>array of T class instances</returns>
    public static T[] JsonToArray<T>(string jsonStr)
    {
        string arrayJsonStr = "{\"array\":" + jsonStr +"}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(arrayJsonStr);
        return wrapper.array;
    }
}

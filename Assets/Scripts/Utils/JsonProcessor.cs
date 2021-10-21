using System.Collections;
using UnityEngine;

public class JsonProcessor
{
    private class Wrapper<T>
    {
        public T[] array;
    }

    public static T[] JsonToArray<T>(string jsonStr)
    {
        string arrayJsonStr = "{\"array\":" + jsonStr +"}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(arrayJsonStr);       
        return wrapper.array;
    }
}

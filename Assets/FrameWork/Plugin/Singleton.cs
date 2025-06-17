using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ��ͨ����
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : new()
{
    private static T _instance;
    private static object mutex = new object();
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (mutex)
                {
                    //��֤�����̰߳�ȫ
                    if (_instance == null)
                        _instance = new T();
                }
            }
            return _instance;
        }
    }
}


/// <summary>
/// ���Թ����ڳ������еĵ���
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnitySingleton<T>:MonoBehaviour where T : Component
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if(_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = (T)obj.AddComponent(typeof(T));
                    obj.name = typeof(T).Name;
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ��Դ���ؿ�� �������ء���ȡ��ж��
/// ���أ�ͬ�����أ��첽���أ�Ԥ����
/// �����
/// </summary>
public class ResourceMgr : Singleton<ResourceMgr>
{
    Dictionary<string, Object> assetDic = new Dictionary<string, Object>();

    /// <summary>
    /// Resources���ͼ�����Դ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T ResLoadAsset<T>(string path) where T : Object
    {
        //���·��Ϊ�� ��ֱ��return null
        if (string.IsNullOrEmpty(path))
            return null;
        Object obj = null;//����һ�����ն��� 
        if (assetDic.TryGetValue(path, out obj)) //ͨ��·�����ֵ����ҵ�ֵ value
        {
            return obj as T;
        }
        obj = Resources.Load<T>(path);
        assetDic.Add(path, obj);
        return obj as T;
    }
    /// <summary>
    /// AssetDataBase���ͼ�����Դ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T AssetDataBaseLoadAsset<T>(string path) where T : Object
    {
        //���·��Ϊ�� ��ֱ��return null
        if (string.IsNullOrEmpty(path))
            return null;
        Object obj = null;//����һ�����ն��� 
        if (assetDic.TryGetValue(path, out obj)) //ͨ��·�����ֵ����ҵ�ֵ value
        {
            return obj as T;
        }
#if UNITY_EDITOR
        obj = AssetDatabase.LoadAssetAtPath<T>(path);
        assetDic.Add(path, obj);
        return obj as T;
#endif
        return null;
    }
}

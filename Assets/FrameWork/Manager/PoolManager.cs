using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : UnitySingleton<PoolManager>
{
    Dictionary<string, List<GameObject>> poolDic = new Dictionary<string, List<GameObject>>();

    /// <summary>
    /// �Ӷ���ػ�ȡ����
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetGameObject(string name)
    {
        GameObject obj = null;
        //�ж϶��������û����������б�
        if (poolDic.ContainsKey(name) && poolDic[name].Count > 0)
        {
            obj = poolDic[name][0];
            poolDic[name].RemoveAt(0);
            return obj;
        }
        obj = ResourceMgr.Instance.ResLoadAsset<GameObject>("Prefab/" + name);
        return Instantiate(obj);
    }

    /// <summary>
    /// ���ն����߼�
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void ReleaseGameObject(string name, GameObject obj)
    {
        obj.transform.position = new Vector3(5000, 5000, 5000);
        if (!poolDic.ContainsKey(name))
            poolDic[name] = new List<GameObject>();
        poolDic[name].Add(obj);
    }
}

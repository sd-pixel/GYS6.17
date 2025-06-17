using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ui���游�ڵ������
/// </summary>
public enum UIType
{
    NULL,//û��������ΪĬ��
    TIP,//����
    NORMAL,//��ͨ
    BACKGROUND,//����
}

/// <summary>
/// ui�򿪵�����
/// </summary>
public enum ShowType
{
    NULL,//û��������ΪĬ��
    TIP,//����
    NORMAL,//��ͨ�����ظ�����
    EXCLUSIVE,//����
    BACKGROUND,//һֱ����
}

public class UIManager : Singleton<UIManager>
{
    string uiPrefabPath = "UIPrefab/";//Ԥ����·��
    Transform tipsRoot, normalRoot, backRoot;//�������ڵ�
    public Dictionary<string, UIWindow> allUIDic = new Dictionary<string, UIWindow>();//����ui�ֵ�
    public Dictionary<string, UIWindow> poolDic = new Dictionary<string, UIWindow>();//�����б��ֵ�
    UIWindow exclusivePanel;//���ⴰ�ڱ���

    /// <summary>
    /// ��ʼ��ui��� ��ʼ��ui����ĸ��ڵ�
    /// </summary>
    public void Init(Transform tipsRoot, Transform normalRoot, Transform backRoot)
    {
        this.tipsRoot = tipsRoot;
        this.normalRoot = normalRoot;
        this.backRoot = backRoot;
    }

    /// <summary>
    /// �򿪽���ķ���
    /// </summary>
    /// <param name="uiName">��Ҫ�򿪽��������</param>
    /// <returns></returns>
    public UIWindow OpenWindow(string uiName)
    {
        UIWindow uiBase = GetWindowFunc(uiName);
        if (uiBase == null)
        {
            Debug.Log("�������ʧ��");
            return null;
        }
        else
        {
            //����������ʾ
            switch (uiBase.m_ShowType)
            {
                case ShowType.NULL:
                    NormalOpenFunc(uiBase);
                    break;
                case ShowType.TIP:
                    uiBase.OpenAsTop();
                    break;
                case ShowType.NORMAL:
                    NormalOpenFunc(uiBase);
                    break;
                case ShowType.EXCLUSIVE:
                    ExclusiveOpenFunc(uiBase);
                    break;
                case ShowType.BACKGROUND:
                    uiBase.Show();
                    break;
                default:
                    break;
            }
            return uiBase;
        }
    }

    /// <summary>
    /// ����򿪷�ʽ
    /// </summary>
    /// <param name="uiBase"></param>
    private void ExclusiveOpenFunc(UIWindow uiBase)
    {
        exclusivePanel = uiBase;
        //�����н��涼�ر�
        foreach (var panel in poolDic.Values)
        {
            panel.Hide();
        }
        exclusivePanel.Show();
    }

    /// <summary>
    /// ��ͨ���ڴ��߼�
    /// </summary>
    /// <param name="uiBase"></param>
    private void NormalOpenFunc(UIWindow uiBase)
    {
        //�ж���û�л��ⴰ�� �еĻ��رջ��ⴰ��
        if (exclusivePanel != null)
        {
            exclusivePanel.Hide();
        }
        //�жϳ�����û��������� ���û�� ������� ����������ⴰ�ڴ򿪵�ʱ�� ���������еĽ��涼�ر�
        if (!poolDic.ContainsKey(uiBase.m_Name))
        {
            poolDic.Add(uiBase.m_Name, uiBase);
        }
        //�������
        uiBase.Show();
    }

    /// <summary>
    /// ��ȡui����
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    private UIWindow GetWindowFunc(string uiName)
    {
        UIWindow uiBase;
        #region ��һ�ֱ�д��ʽ
        //if(!allUIDic.TryGetValue(uiName,out uiBase))
        //{
        //    string name = uiName.Replace("Panel", "");
        //    GameObject uiPrefab = Resources.Load<GameObject>(uiPrefabPath + uiName);
        //    if (uiPrefab == null)
        //    {
        //        Debug.Log("��Դ��û��" + uiName + "�����Դ");
        //        return null;
        //    }
        //    uiBase = uiPrefab.GetComponent<UIBase>();
        //    if (uiBase == null)
        //    {
        //        Debug.Log("��Դû�й���UIBase�ű�");
        //        return null;
        //    }
        //    uiPrefab = InstantiateUIPrefab(uiPrefab, uiBase.m_Type);
        //    uiBase = uiPrefab.GetComponent<UIBase>();
        //    uiBase.m_Name = uiName;
        //    Type model = Type.GetType(name + "Model");
        //    if (model != null)
        //    {
        //        uiBase.model = Activator.CreateInstance(model) as ModelBase;
        //    }
        //    else
        //    {
        //        Debug.Log("��Դû�ж�Ӧ��Model�ű�");
        //    }
        //    Type view = Type.GetType(name + "View");
        //    if (view != null)
        //    {
        //        uiBase.view = Activator.CreateInstance(view) as ViewBase;
        //    }
        //    else
        //    {
        //        Debug.Log("��Դû�ж�Ӧ��View�ű�");
        //    }
        //    Type control = Type.GetType(name + "Control");
        //    if (control != null)
        //    {
        //        uiBase.control = Activator.CreateInstance(control) as ControlBase;
        //    }
        //    else
        //    {
        //        Debug.Log("��Դû�ж�Ӧ��Control�ű�");
        //    }
        //    uiBase.Init();
        //    allUIDic.Add(uiName, uiBase);
        //}
        //else
        //{
        //    //˵����������Ѿ��򿪹���
        //}
        #endregion
        if (allUIDic.ContainsKey(uiName))
        {
            uiBase = allUIDic[uiName];
            //˵����δ򿪹���
        }
        else
        {
            ///��ȡ���ui������ �����GamePanel �õ��Ľ����Game
            string name = uiName.Replace("Panel", "");
            GameObject uiPrefab = ResourceMgr.Instance.ResLoadAsset<GameObject>(uiPrefabPath + uiName);// Resources.Load<GameObject>(uiPrefabPath + uiName);
            if (uiPrefab == null)
            {
                Debug.Log("��Դ��û��" + uiName + "�����Դ");
                return null;
            }
            uiBase = uiPrefab.GetComponent<UIWindow>();
            if (uiBase == null)
            {
                Debug.Log(uiName+ "��Դû�й���UIWindow�ű�");
                return null;
            }
            uiPrefab = InstantiateUIPrefab(uiPrefab, uiBase.m_Type);
            uiBase = uiPrefab.GetComponent<UIWindow>();
            uiBase.m_Name = uiName;
            Type model = Type.GetType(name + "Model");
            if (model != null)
            {
                uiBase.model = Activator.CreateInstance(model) as ModelBase;
            }
            else
            {
                Debug.Log(uiName + "��Դû�ж�Ӧ��Model�ű�");
            }
            Type view = Type.GetType(name + "View");
            if (view != null)
            {
                uiBase.view = Activator.CreateInstance(view) as ViewBase;
            }
            else
            {
                Debug.Log(uiName + "��Դû�ж�Ӧ��View�ű�");
            }
            Type control = Type.GetType(name + "Control");
            if (control != null)
            {
                uiBase.control = Activator.CreateInstance(control) as ControlBase;
            }
            else
            {
                Debug.Log(uiName + "��Դû�ж�Ӧ��Control�ű�");
            }
            uiBase.Init();
            allUIDic.Add(uiName, uiBase);
        }
        return uiBase;
    }

    private GameObject InstantiateUIPrefab(GameObject uiPrefab, UIType m_Type)
    {
        Debug.Log(uiPrefab.name + "����Ϊ" + m_Type.ToString());
        if (uiPrefab == null)
            return null;
        switch (m_Type)
        {
            case UIType.NULL:
                return GameObject.Instantiate(uiPrefab, normalRoot);
            case UIType.TIP:
                return GameObject.Instantiate(uiPrefab, tipsRoot);
            case UIType.NORMAL:
                return GameObject.Instantiate(uiPrefab, normalRoot);
            case UIType.BACKGROUND:
                return GameObject.Instantiate(uiPrefab, backRoot);
            default:
                return null;
        }
    }

    public void CloseAllWindow()
    {
        foreach(var item in allUIDic.Values)
        {
            item.Hide();
        }
    }

    public void CloseWindow(string uiName)
    {
        if (!allUIDic.ContainsKey(uiName)) return;
        switch (allUIDic[uiName].m_ShowType)
        {
            case ShowType.NULL:
                NormalCloseFunc(allUIDic[uiName]);
                break;
            case ShowType.TIP:
                break;
            case ShowType.NORMAL:
                NormalCloseFunc(allUIDic[uiName]);
                break;
            case ShowType.EXCLUSIVE:
                ExclusiveCloseFunc(allUIDic[uiName]);
                break;
            case ShowType.BACKGROUND:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ����view�رս���
    /// </summary>
    public void CloseWidnow(UIWindow uiBase)
    {
        switch (uiBase.m_ShowType)
        {
            case ShowType.NULL:
                NormalCloseFunc(uiBase);
                break;
            case ShowType.TIP:
                uiBase.Hide();
                break;
            case ShowType.NORMAL:
                NormalCloseFunc(uiBase);
                break;
            case ShowType.EXCLUSIVE:
                ExclusiveCloseFunc(uiBase);
                break;
            case ShowType.BACKGROUND:
                uiBase.Hide();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ���ⴰ�ڹرշ���
    /// </summary>
    /// <param name="uiBase"></param>
    private void ExclusiveCloseFunc(UIWindow uiBase)
    {
        if (poolDic.Count > 0)
        {
            foreach (var panel in poolDic.Values)
            {
                panel.Show();
            }
        }
        exclusivePanel.Hide();
        exclusivePanel = null;
    }

    /// <summary>
    /// ��ͨ����رյķ���
    /// </summary>
    /// <param name="uiBase"></param>
    private void NormalCloseFunc(UIWindow uiBase)
    {
        uiBase.Hide();
        if(poolDic.ContainsKey(uiBase.m_Name))
        {
            poolDic.Remove(uiBase.m_Name);
        }
        if (exclusivePanel != null)
            exclusivePanel.Show();
    }

    /// <summary>
    /// ��ȡUI
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public UIWindow GetWindow(string uiName)
    {
        if (allUIDic.ContainsKey(uiName))
        {
            return allUIDic[uiName];
        }
        else
        {
            Debug.Log("������û�м��ع�" + uiName);
            return null;
        }
    }
}

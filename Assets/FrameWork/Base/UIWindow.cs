using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ű�������UI��
/// </summary>
public class UIWindow : MonoBehaviour
{
    [HideInInspector]
    public string m_Name = ""; //ui����
    public UIType m_Type; //ui����
    public ShowType m_ShowType; //ui�򿪵�����
    public ModelBase model; //������ڵ�model�ű�
    public ViewBase view; //������ڵ�view�ű�
    public ControlBase control; //������ڵ�control�ű�
    CanvasGroup canvasGroup;

    /// <summary>
    /// ��ʼ��mvc�����ű� �൱��ִ����mvc�����ű���Awack
    /// </summary>
    public void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            Debug.Log("��ǰUIû�й���CanvasGroup�ű�");
        if (model != null)
            model.Init(this);
        if (view != null) 
            view.Init(this);
        if (control != null)
            control.Init(this);
    }

    /// <summary>
    /// mvc�����ű���OnEnable
    /// </summary>
    public void Enable()
    {
        if (model != null)
            model.OnEnable();
        if (view != null)
            view.OnEnable();
        if (control != null)
            control.OnEnable();
    }

    /// <summary>
    /// mvc�����ű���OnDestory
    /// </summary>
    public void Destory()
    {
        if (model != null)
            model.OnDestory();
        if (view != null)
            view.OnDestory();
        if (control != null)
            control.OnDestory();
    }

    public void Update()
    {
        if (view != null)
            view.Update();
    }

    internal void Hide()
    {
        transform.name = m_Name + "_Hide";
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        Destory();
    }

    internal void Show()
    {
        transform.name = m_Name + "_Show";
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        Enable();
    }

    internal void OpenAsTop()
    {
        transform.SetAsLastSibling();
        Show();
    }
}

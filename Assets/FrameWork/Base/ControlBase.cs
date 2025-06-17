using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��View��Model���м������ view����ִ��ʲô���� �Ҳ�����������model�����
/// </summary>
public class ControlBase
{
    public UIWindow uiWindow;
    public virtual void Init(UIWindow uiWindow)
    {
        this.uiWindow = uiWindow;
    }

    public virtual void OnEnable()
    {

    }

    public virtual void OnDestory()
    {

    }
}

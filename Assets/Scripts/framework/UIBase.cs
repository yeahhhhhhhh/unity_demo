// UI基类，所有UI界面继承此类
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    [Header("UI基础设置")]
    public string uiName;
    public UILevel level = UILevel.Normal;
    public bool isCached = false;  // 是否缓存界面

    public virtual void OnOpen()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnClose()
    {
        if (!isCached)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void OnUpdate() { }
}

// UI层级定义
public enum UILevel
{
    Background = 0,    // 背景层
    Normal = 100,      // 普通层
    Tips = 200,        // 提示层
    Top = 300          // 顶层
}
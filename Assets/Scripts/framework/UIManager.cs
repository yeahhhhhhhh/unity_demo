using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private Dictionary<string, UIBase> _uiDict = new();
    private Dictionary<string, UIBase> _shownUIs = new();
    private Transform _uiRoot;
    public GameObject rootParent;

    [Header("UI配置")]
    public List<UIInfo> uiPrefabs = new();

    [System.Serializable]
    public class UIInfo
    {
        public string uiName;
        public UIBase uiPrefab;
        public UILevel level;
        public bool isGlobal = true;  // 是否全局UI
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUIRoot();
            PreloadGlobalUIs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeUIRoot()
    {
        DontDestroyOnLoad(rootParent);
        // 创建UI根节点
        GameObject rootObj = new GameObject("UIRoot");
        rootObj.transform.SetParent(rootParent.transform);
        rootObj.transform.localPosition = Vector3.zero;
        rootObj.transform.localScale = Vector3.one;
        _uiRoot = rootObj.transform;
        DontDestroyOnLoad(rootObj);

        // 创建不同层级父节点
        CreateUILayer("BackgroundLayer", UILevel.Background);
        CreateUILayer("NormalLayer", UILevel.Normal);
        CreateUILayer("TipsLayer", UILevel.Tips);
        CreateUILayer("TopLayer", UILevel.Top);
    }

    void CreateUILayer(string layerName, UILevel level)
    {
        GameObject layer = new GameObject(layerName);
        layer.transform.SetParent(_uiRoot);
        layer.transform.localPosition = Vector3.zero;
        layer.transform.localScale = Vector3.one;
    }

    void PreloadGlobalUIs()
    {
        foreach (var uiInfo in uiPrefabs)
        {
            if (uiInfo.isGlobal)
            {
                // 预加载全局UI但不立即显示
                PreloadUI(uiInfo.uiName);
                Debug.Log("PreloadGlobalUIs:" + uiInfo.uiName);
            }
        }
    }

    // 预加载UI到内存
    public void PreloadUI(string uiName)
    {
        if (_uiDict.ContainsKey(uiName)) return;

        UIInfo uiInfo = uiPrefabs.Find(info => info.uiName == uiName);
        if (uiInfo != null)
        {
            UIBase uiInstance = Instantiate(uiInfo.uiPrefab, GetUILayer(uiInfo.level));
            uiInstance.gameObject.SetActive(false);
            _uiDict[uiName] = uiInstance;
        }
    }

    // 打开UI界面
    public UIBase OpenUI(string uiName, object data = null)
    {
        if (!_uiDict.ContainsKey(uiName))
        {
            PreloadUI(uiName);
        }

        UIBase ui = _uiDict[uiName];
        ui.OnOpen();
        _shownUIs[uiName] = ui;

        // 设置到正确的层级
        ui.transform.SetParent(GetUILayer(ui.level));
        ui.transform.SetAsLastSibling();  // 确保在层级内最前

        return ui;
    }

    // 关闭UI界面
    public void CloseUI(string uiName)
    {
        if (_uiDict.ContainsKey(uiName))
        {
            _uiDict[uiName].OnClose();
            _shownUIs.Remove(uiName);
        }
    }

    Transform GetUILayer(UILevel level)
    {
        string layerName = level.ToString() + "Layer";
        return _uiRoot.Find(layerName);
    }

    public UIBase GetUI(string uiName)
    {
        if (_uiDict.ContainsKey(uiName))
        {
            return _uiDict[uiName];
        }

        return null;
    }
}
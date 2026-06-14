using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipsUI : UIBase
{
    [Header("设置界面组件")]
    public Button confirmButton;
    public TextMeshProUGUI tipsMsgText;

    // Start is called before the first frame update
    void Start()
    {
        confirmButton.onClick.AddListener(OnComfirmClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnComfirmClick()
    {
        UIManager.Instance.CloseUI("Tips");
    }

    public void SetTipsMsg(string msg)
    {
        tipsMsgText.text = msg;
    }
}

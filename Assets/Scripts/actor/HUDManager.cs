using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    public GameObject hp_bar_prefab_; // 在Inspector中拖入创建好的血条预制体
    private GameObject hp_bar_instance_;

    //public GameObject damage_text_prefab_;
    //public GameObject damage_text_pos_;

    void Start()
    {
        if (hp_bar_prefab_ != null)
        {
            // 实例化血条，父对象为当前的锚点
            hp_bar_instance_ = Instantiate(hp_bar_prefab_, transform);
            // 重置血条的本地位置和旋转，确保其在锚点正中心
            hp_bar_instance_.transform.localPosition = Vector3.zero;
            hp_bar_instance_.transform.localRotation = Quaternion.identity;

            // 在初始化代码中（如HUDManager的Start方法）添加事件监听
            //Slider slider = GetComponentInChildren<Slider>();
            //slider.onValueChanged.AddListener(OnHealthValueChanged);
        }
    }

    // 提供一个方法用于更新血条显示
    public void UpdateHealth(float current_health, float max_health)
    {
        if (hp_bar_instance_ != null)
        {
            Slider slider = hp_bar_instance_.GetComponentInChildren<Slider>();
            if (slider != null)
            {
                slider.value = current_health / max_health; // 计算血量百分比
            }
        }
    }

    // 当血量改变时调用的方法
    private void OnHealthValueChanged(float value)
    {
        // 例如：血量低于20%时，将血条颜色变为红色以示警告
        Slider slider = GetComponentInChildren<Slider>();
        if (value < slider.maxValue * 0.2f)
        {
            // 查找Fill图像并改变其颜色
            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = Color.red;
            }
        }
    }
}
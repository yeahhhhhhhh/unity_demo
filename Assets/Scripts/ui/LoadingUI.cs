using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : UIBase
{
    public Image fadeImage;
    public Slider progressSlider;
    public TextMeshProUGUI progressText;

    public float delay_close_time_ = 1f;
    public bool start_close_ = false;

    public void Start()
    {
        if (progressSlider != null)
        {
            progressSlider.value = 0;
        }

        if (progressText != null)
        {
            progressText.text = $"加载中... {(0 * 100):F0}%";
        }
    }
    public void UpdateProgress(float progress)
    {
        if (progressSlider != null)
        {
            progressSlider.value = progress;
        }

        if (progressText != null)
        {
            progressText.text = $"加载中... {(progress * 100):F0}%";
        }

        if (progress == 1f)
        {
            start_close_ = true;
            delay_close_time_ = 1f;
        }
    }

    public void Update()
    {
        if (start_close_)
        {
            delay_close_time_ -= Time.deltaTime;
            if (delay_close_time_ <= 0)
            {
                start_close_ = false;
                UIManager.Instance.CloseUI("Loading");
            }
        }
    }
}
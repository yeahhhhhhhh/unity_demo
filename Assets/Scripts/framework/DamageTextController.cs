using UnityEngine;
using TMPro;
using System.Collections;

public class DamageTextController : MonoBehaviour
{
    public GameObject damageTextPrefab; // 拖入上面创建的TMP文本预制体
    public Vector2 randomPosOffset = new Vector2(-0.5f, 0.5f); // 随机位置偏移
    public float fontSize = 50f;

    /// <summary>
    /// 在UI容器内创建一个伤害跳字
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="isCritical">是否为暴击</param>
    public void CreateDamageText(int damage, bool isCritical = false)
    {
        if (damageTextPrefab == null)
        {
            Debug.LogError("DamageTextPrefab is not assigned!");
            return;
        }

        // 实例化伤害文本
        GameObject textInstance = Instantiate(damageTextPrefab, transform);
        TextMeshProUGUI tmpComponent = textInstance.GetComponent<TextMeshProUGUI>();

        if (tmpComponent == null) return;

        // 设置文本和样式
        if (damage < 0)
        {
            tmpComponent.text = "+" + damage.ToString();
            tmpComponent.color = Color.green;
        }
        else if (damage > 0)
        {
            tmpComponent.text = "-" + damage.ToString();
            tmpComponent.color = Color.red;
        }
        else
        {
            tmpComponent.text = damage.ToString();
            tmpComponent.color = Color.red;
        }
        
        tmpComponent.fontSize = fontSize;
        if (isCritical)
        {
            tmpComponent.fontSize *= fontSize * 1.5f; // 暴击字体更大
        }

        // 设置随机位置偏移
        Vector3 offset = new Vector3(
            Random.Range(randomPosOffset.x, randomPosOffset.y),
            Random.Range(randomPosOffset.x, randomPosOffset.y),
            0
        );
        textInstance.transform.position += offset;

        // 启动动画协程
        StartCoroutine(AnimateDamageText(textInstance, tmpComponent));
    }

    private IEnumerator AnimateDamageText(GameObject textObj, TextMeshProUGUI text)
    {
        float duration = 1.5f; // 动画总时长
        float floatSpeed = 4f; // 上浮速度
        float elapsedTime = 0f;
        Color originalColor = text.color;
        Vector3 startPos = textObj.transform.localPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            // 上浮：随时间在Y轴向上移动
            textObj.transform.Translate(0, floatSpeed * Time.deltaTime, 0);

            // 淡出：随时间降低透明度
            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(1f, 0f, progress);
            text.color = newColor;

            // 可选：添加缩放效果
            // textObj.transform.localScale = Vector3.one * Mathf.Lerp(1f, 1.2f, progress);

            yield return null; // 等待下一帧
        }

        // 动画结束后销毁对象
        Destroy(textObj);
    }
}
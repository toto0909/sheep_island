using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI sheepCountText;

    void Awake()
    {
        Instance = this;
    }

    // ゲーム内表示時間を更新
    public void UpdateTimeText(string text)
    {
        if (timeText != null)
        {
            timeText.text = text;
        }
    }

    // 羊の総数を更新
    public void UpdateSheepCount(int count)
    {
        if (sheepCountText != null)
        {
            sheepCountText.text = $"sheep: {count}";
        }
    }
}

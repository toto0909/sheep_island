using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI sheepCountText;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateSheepCount(int count)
    {
        if (sheepCountText != null)
        {
            sheepCountText.text = $"sheep: {count}";
        }
    }
}

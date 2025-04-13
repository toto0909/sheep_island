using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float gameTimeSeconds = 0f; // ゲーム内時間（秒）
    public float secondsPerDay = 180f; // 3分で1日
    public int currentDay = 1;

    public float GetGameTimeInSeconds() => gameTimeSeconds;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        gameTimeSeconds += Time.deltaTime;

        // 時刻を文字列化（例: Day 1 - 03:45）
        int currentDay = Mathf.FloorToInt(gameTimeSeconds / secondsPerDay) + 1;
        float timeInDay = gameTimeSeconds % secondsPerDay;
        int hour = Mathf.FloorToInt(timeInDay / (secondsPerDay / 24f));
        int minute = Mathf.FloorToInt((timeInDay % (secondsPerDay / 24f)) / (secondsPerDay / 24f / 60f));

        string timeString = $"Day {currentDay} - {hour:00}:{minute:00}";
        UIManager.Instance.UpdateTimeText(timeString);
    }
}

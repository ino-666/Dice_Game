using UnityEngine;
using TMPro; // TextMeshProを使うために必要
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Main Stats")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Roll Info")]
    [SerializeField] private TextMeshProUGUI lastRollText;
    [SerializeField] private TextMeshProUGUI roleLogText;

    [Header("History")]
    [SerializeField] private TextMeshProUGUI[] historyTexts; // 5個の配列

    private float playTime = 0;
    private bool isTimerRunning = false;

void Update()
{
    if (isTimerRunning)
    {
        playTime += Time.deltaTime;
        timeText.text = $"Time: {playTime:F2}s";
    }
}

    public void StartTimer() => isTimerRunning = true;

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score:N0}"; // カンマ区切りで表示
    }

    public void UpdateRollInfo(string diceDetails, int total)
    {
        lastRollText.text = $"Last: [{diceDetails}] = {total}";
    }

    public void PushRoleLog(string message)
    {
        roleLogText.text = message; // 最新の役を表示
    }

    public void UpdateHistory(IReadOnlyList<int> history)
    {
        for (int i = 0; i < historyTexts.Length; i++)
        {
            if (i < history.Count)
                historyTexts[i].text = history[i].ToString();
            else
                historyTexts[i].text = "-";
        }
    }
}
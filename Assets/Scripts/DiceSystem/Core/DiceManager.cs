using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class DiceManager : MonoBehaviour
{
    [Header("Core References")]
    [SerializeField] private DiceRoller roller;
    [SerializeField] private DicePresenter presenter;
    [SerializeField] private DiceRoleTable roleTable;
    [SerializeField] private UIManager uiManager;

    [Header("Game Settings")]
    [SerializeField] private int currentDiceCount = 2;

    // ===== 内部状態 =====
    private Dictionary<string, int> roleCounts = new();
    private DiceHistory history;
    private int score;

    // ===== 強化ステータス（Shopから変更される）=====
    private float scoreMultiplier = 1f;
    private float rollSpeedMultiplier = 1f; // 将来の演出速度用

    // =============================
    // 初期化
    // =============================
    private void Awake()
    {
        if (roleTable != null)
            roleTable.LoadFromJson();

        history ??= new DiceHistory();
    }

    private void Start()
    {
        if (uiManager != null)
        {
            uiManager.StartTimer();
            uiManager.UpdateScore(score);
        }
    }

    // =============================
    // 入力処理
    // =============================
    private void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            // UI上クリックは無視
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            RollDice();
        }
    }

    // =============================
    // メインロジック
    // =============================
    public void RollDice()
    {
        presenter.ClearOldDice();

        List<int> currentResults = new();

        // サイコロを振る
        for (int i = 0; i < currentDiceCount; i++)
        {
            int val = roller.Roll();
            currentResults.Add(val);
            presenter.SpawnAndRoll(val, i);
        }

        string diceDetails = string.Join(", ", currentResults);
        int currentTotal = currentResults.Sum();
        history.Add(currentTotal);

        Debug.Log($"🎲 ロール結果: [{diceDetails}] 合計: {currentTotal}");

        // ===== スコア加算（倍率反映）=====
        AddScore(currentTotal);

        // ===== 役判定 =====
        var matchedRoles = DiceRoleChecker.Check(
            currentResults,
            history.GetAll(),
            roleTable
        );

        foreach (var role in matchedRoles)
        {
            int count = ProcessRoleBonus(role);
            AddScore(role.bonusScore);

            if (uiManager != null)
            {
                string logMsg = count == 1
                    ? $"初成立！ {role.roleName}"
                    : $"再成立 {role.roleName}";

                uiManager.PushRoleLog(logMsg);
            }
        }

        // ===== UI更新 =====
        if (uiManager != null)
        {
            uiManager.UpdateRollInfo(diceDetails, currentTotal);
            uiManager.UpdateScore(score);
            uiManager.UpdateHistory(history.GetAll());
        }

        Debug.Log($"Total Score: {score}");
    }

    private void AddScore(int baseValue)
    {
        int finalValue = Mathf.RoundToInt(baseValue * scoreMultiplier);
        score += finalValue;
    }

    private int ProcessRoleBonus(DiceRoleDefinition role)
    {
        roleCounts.TryGetValue(role.roleName, out int count);
        count++;
        roleCounts[role.roleName] = count;

        return count;
    }

    // =============================
    // ShopManager から呼ばれるAPI
    // =============================

    public int GetScore()
    {
        return score;
    }

    public void SpendScore(int amount)
    {
        score -= amount;

        if (uiManager != null)
            uiManager.UpdateScore(score);
    }

    public void SetDiceCount(int count)
    {
        currentDiceCount = count;
        Debug.Log($"🎲 サイコロ数変更: {currentDiceCount}");
    }

    public void AddDiceCount(int amount)
    {
        currentDiceCount += amount;
        Debug.Log($"🎲 サイコロ増加: {currentDiceCount}");
    }

    public void SetScoreMultiplier(float multiplier)
    {
        scoreMultiplier = multiplier;
        Debug.Log($"✨ スコア倍率変更: {scoreMultiplier}");
    }

    public void MultiplyScoreMultiplier(float multiplier)
    {
        scoreMultiplier *= multiplier;
        Debug.Log($"✨ スコア倍率累積: {scoreMultiplier}");
    }

    public void SetRollSpeed(float multiplier)
    {
        rollSpeedMultiplier = multiplier;
        Debug.Log($"⚡ ロール速度倍率: {rollSpeedMultiplier}");
    }
}
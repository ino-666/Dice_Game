using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private DiceRoller roller;
    [SerializeField] private DicePresenter presenter;
    [SerializeField] private DiceRoleTable roleTable;
    private Dictionary<string, int> roleCounts = new();
    private DiceHistory history;
    private int score;

    private void Awake()
    {
        roleTable.LoadFromJson();
        history = new DiceHistory();
        history.Reset();
        score = 0;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RollDice();
        }
    
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            RollDice();
        }
    }

    public void RollDice()
    {
        int result = roller.Roll();
        presenter.SpawnAndRoll(result);

        history.Add(result);
        Debug.Log($"Rolled: {result}");
        Debug.Log($"History: {string.Join(",", history.GetAll())}");

        // 基本スコア加算
        score += result;

        var matchedRoles = DiceRoleChecker
            .Check(history.GetAll(), roleTable);

        foreach (var role in matchedRoles)
        {
            roleCounts.TryGetValue(role.roleName, out int count);
            count++;
            roleCounts[role.roleName] = count;

            if (count == 1)
            {
                Debug.Log($"🆕 初成立！ {role.roleName} +{role.bonusScore}");
            }
            else
            {
                Debug.Log($"🔁 再成立 {role.roleName}（{count}回目） +{role.bonusScore}");
            }

            score += role.bonusScore;
        }

        // ★ 現在の合計スコアを毎回ログに出す
        Debug.Log($"💯 Current Score: {score}");
    }
}

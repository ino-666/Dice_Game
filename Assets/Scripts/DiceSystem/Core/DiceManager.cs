using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private DiceRoller roller;
    [SerializeField] private DicePresenter presenter;
    [SerializeField] private DiceRoleTable roleTable;
    [SerializeField] private UIManager uiManager; // 連携用
    
    [SerializeField] private int currentDiceCount = 2;

    private Dictionary<string, int> roleCounts = new();
    private DiceHistory history;
    private int score;

private void Awake()
{
    // 一旦、外部ロードを止めてエディタの設定を優先させる
    if (roleTable != null) roleTable.LoadFromJson(); 
    
    if (history == null) history = new DiceHistory();
}

    private void Start()
    {
        if (uiManager != null)
        {
            uiManager.StartTimer(); // ゲーム開始時にタイマー起動
            uiManager.UpdateScore(score);
        }
    }

    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            RollDice();
        }
    }

    public void RollDice()
    {
        // 1. 演出：以前のサイコロをクリア
        presenter.ClearOldDice();

        List<int> currentResults = new List<int>();

        // 2. 指定された数だけサイコロを振る
        for (int i = 0; i < currentDiceCount; i++)
        {
            int val = roller.Roll();
            currentResults.Add(val);
            presenter.SpawnAndRoll(val, i);
        }

        // 3. ログと履歴の処理
        string diceDetails = string.Join(", ", currentResults);
        int currentTotal = currentResults.Sum();
        history.Add(currentTotal);

        Debug.Log($"History: {string.Join(",", history.GetAll())}");
        Debug.Log($"🎲 ロール結果: [{diceDetails}] (合計: {currentTotal})");

        // 4. スコア加算と役判定
        score += currentTotal;
        var matchedRoles = DiceRoleChecker.Check(currentResults, history.GetAll(), roleTable);
        Debug.Log($"判定された役の数: {matchedRoles.Count()} 件 / ...");
        // --- UI 更新の処理 (メソッド内に含める) ---
        if (uiManager != null)
        {
            uiManager.UpdateRollInfo(diceDetails, currentTotal);
            uiManager.UpdateScore(score);
            uiManager.UpdateHistory(history.GetAll());
        }

        foreach (var role in matchedRoles)
        {
            int count = ProcessRoleBonus(role); // 成立回数を受け取るよう修正
            score += role.bonusScore;

            if (uiManager != null)
            {
                string logMsg = count == 1 ? $" 初成立！ {role.roleName}" : $" 再成立 {role.roleName}";
                uiManager.PushRoleLog(logMsg);
            }
        }

        Debug.Log($"Total Score: {score}");
    }

    private int ProcessRoleBonus(DiceRoleDefinition role)
    {
        roleCounts.TryGetValue(role.roleName, out int count);
        count++;
        roleCounts[role.roleName] = count;

        if (count == 1)
            Debug.Log($"🆕 初成立！ {role.roleName} +{role.bonusScore}");
        else
            Debug.Log($"🔁 再成立 {role.roleName}（{count}回目） +{role.bonusScore}");

        return count;
    }
}
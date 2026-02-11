using System;

public enum CheckTarget { SingleRoll, HistorySum } // 今回の出目リストか、過去の合計履歴か

[Serializable]
public class DiceRoleDefinition
{
    public string roleName;
    public string category;
    public int bonusScore;
    public int minDiceRequired; // この数以上のサイコロを振った時のみ有効

    public CheckTarget target;  // 判定対象
    public RoleType type;
    
    // パラメータ類
    public int requiredCount; 
    public int targetValue;
    public bool requireAllDifferent;
    public bool requireAllEven;
    public bool requireAllOdd;

    public void ResolveType()
    {
        type = category switch
        {
            "Consecutive" => RoleType.Consecutive,
            "Sum"         => RoleType.Sum,
            "Special"     => RoleType.Special,
            "StepUp"      => RoleType.StepUp, // 新規：右肩上がり
            _             => RoleType.Special
        };
    }
}
using System;
using UnityEngine;

[Serializable]
public class DiceRoleDefinition
{
    public string roleName;

    // JSONでは文字列 → 後で変換
    public string category;

    public int requiredCount = 0;
    public int targetValue = 0;
    public int bonusScore = 0;

    // Special 用
    public bool requireAllDifferent;
    public bool requireAllEven;
    public bool requireAllOdd;

    // 実行時に使う enum
    [NonSerialized]
    public RoleType type;

    public void ResolveType()
    {
        type = category switch
        {
            "Consecutive" => RoleType.Consecutive,
            "Sum"         => RoleType.Sum,
            "Special"     => RoleType.Special,
            _             => RoleType.Special
        };
    }
}

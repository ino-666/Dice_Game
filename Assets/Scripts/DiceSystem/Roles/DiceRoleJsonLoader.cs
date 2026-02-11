using UnityEngine;

public static class DiceRoleJsonLoader
{
    public static void LoadFromJsonText(DiceRoleTable table, string jsonText)
    {
        if (string.IsNullOrEmpty(jsonText))
        {
            Debug.LogWarning("DiceRole JSON Text が空です。");
            return;
        }

        var data = JsonUtility.FromJson<DiceRoleJsonData>(jsonText);

        foreach (var role in data.roles)
        {
            role.ResolveType();
        }

        table.roles = data.roles;
        Debug.Log($"🎲 役定義JSONを読み込みました ({data.roles.Length}件)");
    }
}

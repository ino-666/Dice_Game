using UnityEngine;

[CreateAssetMenu(menuName = "Dice/Dice Role Table")]
public class DiceRoleTable : ScriptableObject
{
    public DiceRoleDefinition[] roles;

    [Header("Optional: JSONでロード")]
    public string jsonFileName = "dice_roles"; // Resources 内のファイル名（拡張子不要）

    [ContextMenu("Load From JSON")]
    public void LoadFromJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile == null)
        {
            Debug.LogWarning($"JSONファイルが見つかりません: {jsonFileName}");
            return;
        }

        DiceRoleJsonData data = JsonUtility.FromJson<DiceRoleJsonData>(jsonFile.text);
        if (data == null || data.roles == null)
        {
            Debug.LogError("JSONパース失敗");
            return;
        }

        foreach (var role in data.roles)
        {
            role.ResolveType();
        }

        roles = data.roles;
        Debug.Log($"🎲 DiceRoleTable: JSONをロードしました ({roles.Length}件)");
    }
}

using System.Collections.Generic;
using System.Linq;

public static class DiceRoleChecker
{
    public static List<DiceRoleDefinition> Check(
        IReadOnlyList<int> currentRoll, 
        IReadOnlyList<int> totalHistory,
        DiceRoleTable table)
    {
        List<DiceRoleDefinition> result = new();
        if (table == null || table.roles == null) return result;

        int diceCount = currentRoll.Count;

        foreach (var role in table.roles)
        {
            if (diceCount < role.minDiceRequired) continue;

            // 判定対象のリストを選択
            var targetList = (role.target == CheckTarget.SingleRoll) ? currentRoll : totalHistory;

            if (IsMatch(targetList, role))
            {
                result.Add(role);
            }
        }
        return result;
    }

    private static bool IsMatch(IReadOnlyList<int> list, DiceRoleDefinition role)
    {
        if (list.Count < role.requiredCount) return false;

        var slice = list.Skip(list.Count - role.requiredCount).ToList();

        return role.type switch
        {
            RoleType.Consecutive => slice.All(v => v == slice[0]),
            RoleType.Sum => slice.Sum() == role.targetValue,
            RoleType.StepUp => IsStepUp(slice),
            RoleType.Special => CheckSpecial(slice, role),
            _ => false
        };
    }

    private static bool IsStepUp(List<int> slice)
    {
        for (int i = 1; i < slice.Count; i++)
        {
            if (slice[i] <= slice[i - 1]) return false;
        }
        return true;
    }

    private static bool CheckSpecial(List<int> slice, DiceRoleDefinition role)
    {
        if (role.requireAllDifferent) return slice.Distinct().Count() == slice.Count;
        if (role.requireAllEven) return slice.All(v => v % 2 == 0);
        if (role.requireAllOdd) return slice.All(v => v % 2 == 1);
        
        // ハイ・アンド・ロー (1と6が両方ある)
        if (role.roleName == "HighAndLow") return slice.Contains(1) && slice.Contains(6);
        
        // フルハウス (3つ同じ + 2つ同じ)
        if (role.roleName == "FullHouse")
        {
            var groups = slice.GroupBy(v => v).Select(g => g.Count()).ToList();
            return groups.Contains(3) && groups.Contains(2);
        }

        return false;
    }
}
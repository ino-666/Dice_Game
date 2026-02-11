using System.Collections.Generic;
using System.Linq;

public static class DiceRoleChecker
{
    public static List<DiceRoleDefinition> Check(
        IReadOnlyList<int> history,
        DiceRoleTable table)
    {
        List<DiceRoleDefinition> result = new();
    
        if (table == null || table.roles == null)
            return result;
    
        foreach (var role in table.roles)
        {
            if (role == null) continue;
    
            if (IsMatch(history, role))
            {
                result.Add(role);
            }
        }
    
        return result;
    }

    private static bool IsMatch(
        IReadOnlyList<int> history,
        DiceRoleDefinition role)
    {
        if (history.Count < role.requiredCount) return false;

        var slice = history
            .Skip(history.Count - role.requiredCount)
            .ToList();

        switch (role.type)
        {
            case RoleType.Consecutive:
                return slice.All(v => v == slice[0]);

            case RoleType.Sum:
                return slice.Sum() == role.targetValue;

            case RoleType.Special:
                if (role.requireAllDifferent)
                    return slice.Distinct().Count() == slice.Count;

                if (role.requireAllEven)
                    return slice.All(v => v % 2 == 0);

                if (role.requireAllOdd)
                    return slice.All(v => v % 2 == 1);

                break;
        }

        return false;
    }
}

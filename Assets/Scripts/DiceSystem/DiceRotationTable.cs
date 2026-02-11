using UnityEngine;

public static class DiceRotationTable
{
    public static Quaternion Get(int value)
    {
        switch (value)
        {
            case 1: return Quaternion.Euler(0, 0, 0);
            case 2: return Quaternion.Euler(0, 0, 90);
            case 3: return Quaternion.Euler(90, 0, 0);
            case 4: return Quaternion.Euler(-90, 0, 0);
            case 5: return Quaternion.Euler(0, 0, -90);
            case 6: return Quaternion.Euler(180, 0, 0);
            default: return Quaternion.identity;
        }
    }
}

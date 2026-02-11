using UnityEngine;

public class DiceRoller
{
    public int Roll()
    {
        // 1〜6
        return Random.Range(1, 7);
    }
}

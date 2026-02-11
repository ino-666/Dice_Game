using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public bool useFixedValue = true;

    [Range(1, 6)]
    public int fixedValue = 1;

    public int Roll()
    {
        if (useFixedValue)
        {
            return fixedValue;
        }

        return Random.Range(1, 7);
    }
}

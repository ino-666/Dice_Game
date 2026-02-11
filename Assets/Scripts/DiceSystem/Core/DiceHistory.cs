using System.Collections.Generic;

public class DiceHistory
{
    private const int MaxCount = 5;

    private List<int> history = new();

    public void Reset()
    {
        history.Clear();
    }

    public void Add(int value)
    {
        history.Add(value);

        // ★ 5件超えたら古いものを削除
        if (history.Count > MaxCount)
        {
            history.RemoveAt(0);
        }
    }

    public IReadOnlyList<int> GetAll()
    {
        return history;
    }
}

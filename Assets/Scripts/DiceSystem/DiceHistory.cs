using System.Collections.Generic;
using System.Linq;

public class DiceHistory
{
    private const int MaxCount = 5;
    private Queue<int> history = new Queue<int>();

    public void Add(int value)
    {
        if (history.Count >= MaxCount)
        {
            history.Dequeue();
        }
        history.Enqueue(value);
    }

    public IReadOnlyList<int> GetAll()
    {
        return history.ToList();
    }

    public IReadOnlyList<int> GetLast(int n)
    {
        if (history.Count < n) return new List<int>();
        return history.Skip(history.Count - n).ToList();
    }

    public void Reset()
    {
        history.Clear();
    }

    public bool IsFull()
    {
        return history.Count >= MaxCount;
    }
}

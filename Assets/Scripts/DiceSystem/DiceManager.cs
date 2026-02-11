using UnityEngine;

public class DiceManager : MonoBehaviour
{
    private DiceRoller roller;
    private DiceHistory history;

    [SerializeField] private DicePresenter presenter;

    private void Awake()
    {
        roller = new DiceRoller();
        history = new DiceHistory();
        history.Reset();
    }

    // UIボタンから呼ぶ
    public void RollDice()
    {
        int result = roller.Roll();

        presenter.SpawnAndRoll(result);
        history.Add(result);

        Debug.Log($"Rolled: {result}");
        Debug.Log($"History: {string.Join(",", history.GetAll())}");
    }
}

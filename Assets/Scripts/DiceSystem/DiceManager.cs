using UnityEngine;
using UnityEngine.InputSystem;

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

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RollDice();
        }
    
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            RollDice();
        }
    }

    public void RollDice()
    {
        int result = roller.Roll();

        presenter.SpawnAndRoll(result);
        history.Add(result);

        Debug.Log($"Rolled: {result}");
        Debug.Log($"History: {string.Join(",", history.GetAll())}");
    }
}

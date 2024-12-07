using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResultDisplayer resultDisplayer;
    [SerializeField] private DiceController diceController;

    private int currentScore = 0;
    private void Awake()
    {
        if (resultDisplayer != null)
        {
            resultDisplayer.RollButton.onClick.AddListener(OnRollButtonClicked);
        }

        if (diceController != null)
        {
            diceController.OnRollFinishedOrCanceled += DiceController_OnRollFinishedOrCanceled;
        }
    }

    private void DiceController_OnRollFinishedOrCanceled(int result)
    {
        Debug.Log($"Dice result : {result}");

        if (resultDisplayer == null)
        {
            return;
        }

        if (result == 6)
        {
            resultDisplayer.UpdateScoreText(++currentScore);
        }
    }

    private void OnRollButtonClicked()
    {
        diceController.RollDice();
    }
}

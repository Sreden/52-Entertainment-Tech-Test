using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResultDisplayer resultDisplayer;
    [SerializeField] private DiceController diceController;
    [SerializeField] private VrPopupDisplayer VrPopupDisplayer;

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

        if (VrPopupDisplayer != null)
        {
            VrPopupDisplayer.PopupButton.onClick.AddListener(OnPopupButtonClicked);
            VrPopupDisplayer.OnPopupClose += VrPopupDisplayer_OnPopupClose;
            VrPopupDisplayer.OnPopupOpen += VrPopupDisplayer_OnPopupOpen;
        }
    }
    private void Start()
    {
        //TODO Fetch Data here
    }

    private void VrPopupDisplayer_OnPopupOpen()
    {
        if (diceController != null)
        {
            diceController.DisplayDice(false);
        }

        if (resultDisplayer != null)
        {
            resultDisplayer.RollButton.enabled = false;
        }
    }

    private void VrPopupDisplayer_OnPopupClose()
    {
        if (diceController != null)
        {
            diceController.DisplayDice(true);
        }

        if (resultDisplayer != null)
        {
            resultDisplayer.RollButton.enabled = true;
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

    private void OnPopupButtonClicked()
    {
        VrPopupDisplayer.Toggle();
    }
}

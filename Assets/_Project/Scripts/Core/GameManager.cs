using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResultDisplayer resultDisplayer;
    [SerializeField] private DiceController diceController;
    [SerializeField] private VrPopupDisplayer VrPopupDisplayer;
    [SerializeField] private VirtualRegattaWikiFetcher VrFetcher;

    private int currentScore = 0;

    private void Awake()
    {
        Application.targetFrameRate = 60;

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

            if (VrFetcher != null)
            {
                VrFetcher.OnDescriptionLoaded += VrFetcher_OnDescriptionLoaded;
                VrFetcher.OnTextureDownloadedOrLoaded += VrFetcher_OnTextureDownloadedOrLoaded;
            }
        }
    }
    private void Start()
    {
        if (VrFetcher == null)
        {
            return;
        }

        VrFetcher.FetchData();
    }

    private void VrFetcher_OnTextureDownloadedOrLoaded(Texture2D texture)
    {
        VrPopupDisplayer.UpdateLogo(texture);
    }

    private void VrFetcher_OnDescriptionLoaded(string desc)
    {
        VrPopupDisplayer.UpdateDescription(desc);
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
            diceController.Shake();
#if UNITY_IOS || UNITY_ANDROID
            Handheld.Vibrate();
#endif
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

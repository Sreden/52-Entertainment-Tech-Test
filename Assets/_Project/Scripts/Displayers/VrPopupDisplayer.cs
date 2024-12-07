using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VrPopupDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text desc;
    [SerializeField] private Image logoImage;
    private bool isPopupOpened = false;

    public Button PopupButton;

    public event Action OnPopupOpen;
    public event Action OnPopupClose;

    private void Start()
    {
        Close(); // Init to close
    }

    public void Open()
    {
        OnPopupOpen?.Invoke();
        // TODO: Add coroutine animation or something here
        panel.SetActive(true);
        isPopupOpened = true;
    }

    public void Close()
    {
        isPopupOpened = false;
        panel.SetActive(false);
        // TODO: Add coroutine animation or something here
        OnPopupClose?.Invoke();
    }

    public void Toggle()
    {
        if (isPopupOpened)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void UpdateLogo(Texture2D texture)
    {
        if (logoImage == null)
        {
            Debug.LogWarning("There is no image registered here", this);
            return;
        }

        logoImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        logoImage.preserveAspect = true;
    }

    public void UpdateDescription(string newDesc)
    {
        if (desc == null)
        {
            Debug.LogWarning("There is no TMP_Text description registered here", this);
            return;
        }

        desc.text = newDesc;
    }
}
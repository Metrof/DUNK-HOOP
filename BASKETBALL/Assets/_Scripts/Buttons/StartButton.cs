using UnityEngine.UI;
using UnityEngine;
using System;

public class StartButton : MonoBehaviour
{
    public Action OnClickButton;

    private Button _button;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(StartGame);
    }
    private void StartGame()
    {
        OnClickButton?.Invoke();
    }
    public void HideButton()
    {
        _image.enabled = false;
    }
    public void ShowButton()
    {
        _image.enabled = true;
    }
}

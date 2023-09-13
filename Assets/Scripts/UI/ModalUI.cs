using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ModalUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private Button _confirmButton;

    public static ModalUI Instance;
    public Action OnConfirm;

    private void Awake()
    {
        Instance = this;
        _confirmButton.onClick.AddListener(() => OnConfirm?.Invoke());
        gameObject.SetActive(false);
    }

    public void ShowModal(string promptText, string buttonText)
    {
        Time.timeScale = 0;
        _promptText.text = promptText;
        _confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        OnConfirm = null;
    }
}

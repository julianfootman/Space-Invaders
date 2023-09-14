using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Button references")]
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _highScoreButton;
    [SerializeField] private Button _quitButton;
    [Header("Other UIs")]
    [SerializeField] private HUDController _HUDcontrol;

    private void Awake()
    {
        _resumeButton.onClick.AddListener(ResumeGame);
        _startButton.onClick.AddListener(StartGame);
        _highScoreButton.onClick.AddListener(OnHighScores);
        _quitButton.onClick.AddListener(QuitGame);
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        if (GameManager.Instance.IsPlaying)
        {
            _resumeButton.gameObject.SetActive(true);
            _startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Restart Game";
        }
        else
        {
            _resumeButton.gameObject.SetActive(false);
            _startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Game";
        }

        _HUDcontrol.gameObject.SetActive(false);
        ModalUI.Instance.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        if (GameManager.Instance.IsPlaying)
        {
            _HUDcontrol.gameObject.SetActive(true);
        }
    }

    private void ResumeGame()
    {
        gameObject.SetActive(false);
    }
    
    private void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    private void OnHighScores()
    {

    }

    private void QuitGame()
    {
        Application.Quit();
    }

}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _highScoreButton;
    [SerializeField] private Button _quitButton;

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
        
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void ResumeGame()
    {
        gameObject.SetActive(false);
    }
    
    private void StartGame()
    {
        GameManager.Instance.InitializeGame();
    }

    private void OnHighScores()
    {

    }

    private void QuitGame()
    {
        Application.Quit();
    }

}

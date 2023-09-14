using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager.OnUpdateScore += OnUpdateScore;
        _gameManager.OnUpdateHP += OnUpdateHealthPoint;
        _gameManager.OnUpdateLevel += OnUpdateLevel;
    }

    private void OnUpdateScore(int score)
    {
        _scoreText.text = $"Score : {score}";
    }

    private void OnUpdateHealthPoint(int hp)
    {
        _hpText.text = hp.ToString();
    }

    private void OnUpdateLevel(int level)
    {
        _levelText.text = $"Level : {level}";
    }
}

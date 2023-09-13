using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _hpText;
    //[SerializeField] private Slider _hpSlider;

    private void Start()
    {
        GameManager.Instance.OnUpdateScore += OnUpdateScore;
        GameManager.Instance.OnUpdateHP += OnUpdateHealthPoint;
    }

    private void OnUpdateScore(int score)
    {
        _scoreText.text = $"Score : {score}";
    }

    private void OnUpdateHealthPoint(int hp)
    {
        _hpText.text = hp.ToString();
    }
}

using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsPlaying => _isPlaying;
    public Action<int> OnUpdateScore;
    public Action<int> OnUpdateHP;

    [Header("Boundary")]
    [SerializeField] private Vector2 _gameBoundary = new Vector2(100, 50);
    [Header("Level")]
    [SerializeField] private LevelData _levelData;
    [SerializeField] private int _currentLevel;

    [Header("Player")]
    [SerializeField] private PlayerController _mainPlayer;
    [SerializeField] private int _bonusPoint = 50;
    [SerializeField] private int _maxHP = 10;

    [Header("Enemy Group")]
    [SerializeField] private EnemyGroupBehaviour _enemyGroup;
    [SerializeField] private float _colLimit = 50;
    [SerializeField] private float _rowMargin = 10;

    [Header("Tags")]
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private string _enemyTag = "Enemy";
    [SerializeField] private string _bonusTag = "Bonus";

    private GameObject _bonusObject;
    private bool _isPlaying;
    private int _score;
    private int _currentHP;

    private void Awake()
    {
        Instance = this;
        _currentHP = _maxHP;
    }

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame(int level = 0)
    {        
        _currentLevel = level;
        OnUpdateHP?.Invoke(_currentHP);
        _mainPlayer.transform.position = new Vector3(0, 0, -_gameBoundary.y / 2);
        InitializeEnemyGroup();
        var levelInfo = _levelData._levelInfos[_currentLevel];
        InvokeRepeating(nameof(GenerateBonusObject), levelInfo._bonusAppearCoolDown, levelInfo._bonusAppearCoolDown);
    }

    public void OnHitTarget(GameObject target)
    {
        if (target.tag == _playerTag)
        {
            _currentHP--;
            OnUpdateHP?.Invoke(_currentHP);
            if (_currentHP <= 0)
            {
                OnEndGame();
            }
            else
            {
                ModalUI.Instance.ShowModal($"You have {_currentHP} live" + (_currentHP > 1?"s":"") + " available.", "Continue");
                ModalUI.Instance.OnConfirm += OnContinueGame;
            }
        }
        else if (target.tag == _enemyTag)
        {
            _score += target.GetComponent<EnemyBehaviour>().Point;
            Destroy(target);
            OnUpdateScore?.Invoke(_score);

            if (_enemyGroup.transform.childCount == 0)
            {
                OnEndGame();
            }
        }
        else if(target.tag== _bonusTag)
        {
            _score += _bonusPoint;
            Destroy(target);
            OnUpdateScore?.Invoke(_score);
        }
    }

    public void OnEndGame()
    {

        if (_enemyGroup.transform.childCount == 0 && _currentHP > 0)
        {
            // you win
            ModalUI.Instance.ShowModal("You win", "Main menu");
        }
        else
        {
            // you lose
            ModalUI.Instance.ShowModal("You lose", "Main menu");
        }
    }

    private void OnContinueGame()
    {
        InitializeGame(_currentLevel);
    }

    private void GoToMainMenu()
    {

    }

    public void InitializeEnemyGroup()
    {
        foreach(Transform trans in _enemyGroup.transform)
        {
            Destroy(trans.gameObject);
        }

        var levelInfo = _levelData._levelInfos[_currentLevel];
        var colMargin = _colLimit * 2 / (levelInfo._colCount + 2);
       
        for (int col = 0; col < levelInfo._colCount; col ++)
        {
            for (int row = 0; row < levelInfo._rowCount; row++)
            {
                // clone random enemy prefab                    
                GameObject enemyItem = Instantiate(_levelData.GetRandomEnemyPrefab(), _enemyGroup.transform);
                    enemyItem.transform.localPosition
                        = new Vector3(col * colMargin, 0, row * _rowMargin);
            }
        }

        _enemyGroup.movementLimit = colMargin;
        _enemyGroup.transform.position += new Vector3(levelInfo._colCount * colMargin / 2, 0, - _currentLevel * _rowMargin);
    }

    private void GenerateBonusObject()
    {
        if (_bonusObject != null)
        {
            Destroy(_bonusObject);
        }
        _bonusObject = Instantiate(_levelData._bonusPrefab);
        float direction = UnityEngine.Random.value > 0.5f ? 1 : -1;
        _bonusObject.transform.position = new Vector3(_gameBoundary.x / 2 * direction, 0, _gameBoundary.y / 2);
        _bonusObject.transform.forward = _mainPlayer.transform.right * -direction;
    }
}

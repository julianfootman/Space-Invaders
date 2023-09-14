using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsPlaying => _isPlaying;
    public Action<int> OnUpdateScore;
    public Action<int> OnUpdateHP;
    public Action<int> OnUpdateLevel;

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

    [Header("Others")]
    [SerializeField] private MenuUI _menuUI;
    [SerializeField] private HUDController _hudControl;
    [SerializeField] private ScoreData _scoreData;
    public Transform _bulletParent;

    private GameObject _bonusObject;
    private bool _isPlaying;
    private int _score;
    private int _currentHP;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        _currentHP = _maxHP;
        _score = 0;
        InitializeLevel(0);
    }

    public void InitializeLevel(int level)
    {
        Cleanup();        

        _currentLevel = level;        
        _hudControl.gameObject.SetActive(true);
        OnUpdateHP?.Invoke(_currentHP);
        OnUpdateLevel?.Invoke(_currentLevel + 1);

        // initialize player info
        _mainPlayer.transform.localPosition = new Vector3(0, 0, -_gameBoundary.y / 2);
        _mainPlayer.gameObject.SetActive(true);

        // initialize enemies
        InitializeEnemyGroup();

        var levelInfo = _levelData._levelInfos[_currentLevel];
        InvokeRepeating(nameof(GenerateBonusObject), levelInfo._bonusAppearCoolDown, levelInfo._bonusAppearCoolDown);

        _isPlaying = true;
    }

    public void OnHitTarget(GameObject target)
    {
        CancelInvoke(nameof(GenerateBonusObject));
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
                ModalUI.Instance.ShowModal($"You have {_currentHP} " + (_currentHP > 1?"lives":"life") + " available." , "Continue");
                ModalUI.Instance.OnConfirm += OnContinueGame;
            }
        }
        else if (target.tag == _enemyTag)
        {
            _score += target.GetComponent<EnemyBehaviour>().Point;
            Destroy(target);
            _enemyGroup.OnChildDestroyed();
            OnUpdateScore?.Invoke(_score);
            CancelInvoke(nameof(CheckForEnemies));
            Invoke(nameof(CheckForEnemies), 1);
        }
        else if(target.tag== _bonusTag)
        {
            _score += _bonusPoint;
            Destroy(target);
            OnUpdateScore?.Invoke(_score);
        }
    }

    private void CheckForEnemies()
    {
        if (_enemyGroup.transform.childCount == 0)
        {
            Invoke(nameof(OnLevelUp), 1);
        }
    }

    private void OnLevelUp()
    {
        _currentLevel++;

        if (_levelData._levelInfos.Length > _currentLevel)
        {
            InitializeLevel(_currentLevel);
        }
        else
        {
            OnEndGame();
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

        _scoreData.AddNewScore(_score);
        _hudControl.gameObject.SetActive(false);
        _mainPlayer.gameObject.SetActive(false);
        _isPlaying = false;

        ModalUI.Instance.OnConfirm += GoToMainMenu;
    }

    private void OnContinueGame()
    {
        InitializeLevel(_currentLevel);
    }

    private void GoToMainMenu()
    {
        Cleanup();
        _menuUI.gameObject.SetActive(true);
    }

    public void InitializeEnemyGroup()
    {
        var levelInfo = _levelData._levelInfos[_currentLevel];
        var colMargin = _colLimit * 2 / (levelInfo._colCount + 2);
       
        for (int col = 0; col < levelInfo._colCount; col ++)
        {
            for (int row = 0; row < levelInfo._rowCount; row++)
            {
                // clone random enemy prefab                    
                GameObject enemyItem = Instantiate(_levelData.GetRandomEnemyPrefab(_currentLevel), _enemyGroup.transform);
                    enemyItem.transform.localPosition = new Vector3(col * colMargin, 0, row * _rowMargin);
            }
        }

        _enemyGroup.movementLimit = colMargin;
        _enemyGroup.IntializePosition(
            new Vector3((float)levelInfo._colCount * colMargin / 2f, 0, _gameBoundary.y / 2 - 5 - _currentLevel * _rowMargin));
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

    private void Cleanup()
    {
        foreach (Transform trans in _bulletParent)
        {
            Destroy(trans.gameObject);
        }

        _enemyGroup.CleanUp();
    }
}

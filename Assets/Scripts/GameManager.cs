using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool CanShoot => _magSlider.value < 1;
    [SerializeField] private Slider _magSlider;
    [SerializeField] private int _maxBulletCount = 100;

    [Header("Level")]
    [SerializeField] private LevelData _levelData;
    [SerializeField] private int _currentLevel;

    [Header("Enemy Group")]
    [SerializeField] private Transform _enemyGroupParent;
    [SerializeField] private float _colLimit = 50;
    [SerializeField] private float _rowMargin = 10;


    private void Awake()
    {
        Instance = this;
        InitializeEnemyGroup();
    }

    public void InitializeEnemyGroup()
    {
        var levelInfo = _levelData._levelInfos[_currentLevel];
        var colMargin = _colLimit * 2 / levelInfo._colCount;
       
        for (int col = 0; col < levelInfo._colCount; col ++)
        {
            for (int row = 0; row < levelInfo._rowCount; row++)
            {
                // clone random enemy prefab                    
                GameObject enemyItem = Instantiate(_levelData.GetRandomEnemyPrefab(), _enemyGroupParent);
                    enemyItem.transform.localPosition
                        = new Vector3(col * colMargin, 0, row * _rowMargin);
            }
        }

        _enemyGroupParent.position += new Vector3(levelInfo._colCount * colMargin / 2, 0, 0);
    }
}

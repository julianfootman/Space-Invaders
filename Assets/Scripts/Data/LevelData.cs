using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Data / Level Data", fileName = "Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Bonus")]
    public GameObject _bonusPrefab;
    [Header("Enemy")]
    public GameObject[] _enemyPrefabs;
    [Header("Level Informations")]
    [Space(10)]
    public LevelInfo[] _levelInfos; 

    public GameObject GetRandomEnemyPrefab()
    {
        int randomIndex = UnityEngine.Random.Range(0, _enemyPrefabs.Length);
        return _enemyPrefabs[randomIndex];
    }

    [Serializable]
    public class LevelInfo
    {
        public int _enemySpeed;
        [Header("Enemy Group Info")]
        public int _colCount;
        public int _rowCount;
        public float _bonusAppearCoolDown = 10;
    }
}

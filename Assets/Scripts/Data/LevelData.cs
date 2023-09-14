using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Data / Level Data", fileName = "Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Bonus")]
    public GameObject _bonusPrefab;
    [Header("Level Informations")]
    [Space(10)]
    public LevelInfo[] _levelInfos; 

    public GameObject GetRandomEnemyPrefab(int level)
    {
        int randomIndex = UnityEngine.Random.Range(0, _levelInfos[level]._enemyPrefabs.Length);
        return _levelInfos[level]._enemyPrefabs[randomIndex];
    }

    [Serializable]
    public class LevelInfo
    {
        public int _enemySpeed;
        [Header("Enemy")]
        public GameObject[] _enemyPrefabs;
        [Header("Enemy Group Info")]
        public int _colCount;
        public int _rowCount;
        public float _bonusAppearCoolDown = 10;
    }
}

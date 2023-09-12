using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool CanShoot => _magSlider.value < 1;
    [SerializeField] private Slider _magSlider;
    [SerializeField] private int _maxBulletCount = 100;

    private int _bulletCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void OnNewBullet()
    {
        _bulletCount++;
        _magSlider.value = (float)_bulletCount / (float)_maxBulletCount;

    }

    public void OnDestroyBullet()
    {
        _bulletCount--;
        _magSlider.value = (float)_bulletCount / (float)_maxBulletCount;
    }
}

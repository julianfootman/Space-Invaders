using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _shootingCoolDown;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private int _point = 10;
    [SerializeField] private string _endTriggerTag = "EndTrigger";

    public int Point => _point;
    private float _shootingTimer;

    private void Update()
    {
        if (_shootingTimer <= _shootingCoolDown)
        {
            _shootingTimer += Time.deltaTime;
        }

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (_shootingTimer >= _shootingCoolDown 
            && !Physics.Raycast(_muzzle.position, transform.TransformDirection(Vector3.forward), 100))
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        _shootingTimer = 0;
        Instantiate(_bulletPrefab, transform.position, transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _endTriggerTag)
        {
            GameManager.Instance.OnEndGame();
        }
    }

}

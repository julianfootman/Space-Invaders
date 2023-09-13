using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _shootingCoolDown;
    [SerializeField] private Transform _muzzle;

    private float _shootingTimer;

    private void Awake()
    {
        
    }

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

}

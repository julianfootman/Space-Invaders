using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _moveLimit;

    [Header("Shoot")]
    [SerializeField] private KeyCode _shootBinidng = KeyCode.Space;
    [SerializeField] private Bullet _bulletPrefab;
        
    private float _movementX;
    private Vector3 _originPosition;

    const string _movementAxis = "Horizontal";
    private void Awake()
    {
        _originPosition = transform.localPosition;
    }

    private void Update()
    {
        _movementX += Input.GetAxis(_movementAxis) * _speed * Time.deltaTime;
        _movementX = Mathf.Clamp(_movementX, -_moveLimit, _moveLimit);
        transform.localPosition = _originPosition + new Vector3(_movementX, 0, 0);

        if (Input.GetKeyDown(_shootBinidng))
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        Bullet projectile = Instantiate(_bulletPrefab, transform.position, transform.rotation);
    }
}

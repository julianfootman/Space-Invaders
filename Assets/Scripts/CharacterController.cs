using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Control")]
    [Range(0f, 200f)]
    [SerializeField] private float _xMoveLimit = 88;
    [Range(0f, 200f)]
    [SerializeField] private float _yMoveLimit = 88;
    [Range(0.1f, 9f)]
    [SerializeField] float _moveSpeed = 2f;
    [Header("Camera Control")]
    [SerializeField] private Transform _cameraTransform;

    [Range(0.1f, 9f)]
    [SerializeField] float _sensitivity = 2f;
    [Range(0f, 90f)]
    [SerializeField] float _xRotationLimit = 88f;
    [Range(0f, 90f)]
    [SerializeField] float _yRotationLimit = 88f;

    [Space(10)]
    [Header("Shooter Control")]
    [SerializeField] private Transform _muzzleTransform;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _maxBulletDistance = 1000f;
    [Range(0.1f, 0.9f)]
    [SerializeField] private float _shootingRate = 0.5f;
    
    private float _shootingCooldown = 0;

    const string _xMovementAxis = "Horizontal";
    const string _yMovementAxis = "Vertical";
    const string _xRotationAxis = "Mouse X";
    const string _yRotationAxis = "Mouse Y";

    private Vector2 _movement = Vector2.zero;
    private Vector2 _rotation = Vector2.zero;
    private Vector3 _originPos;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _shootingCooldown = 1 - _shootingRate;
        _originPos = _cameraTransform.position;
    }

    void Update()
    {
        // camera control
        _rotation.x += Input.GetAxis(_xRotationAxis) * _sensitivity;
        _rotation.x = Mathf.Clamp(_rotation.x, -_xRotationLimit, _xRotationLimit);
        _rotation.y += Input.GetAxis(_yRotationAxis) * _sensitivity;
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);
        var xQuat = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(_rotation.y, Vector3.left);

        _cameraTransform.localRotation = xQuat * yQuat;

        // move control
        _movement.x += Input.GetAxis(_xMovementAxis) * _moveSpeed;
        _movement.x = Mathf.Clamp(_movement.x, -_xMoveLimit, _xMoveLimit);
        _movement.y += Input.GetAxis(_yMovementAxis) * _moveSpeed;
        _movement.y = Mathf.Clamp(_movement.y, -_yMoveLimit, _yMoveLimit);
        _cameraTransform.position = _originPos + new Vector3(_movement.x, _movement.y, 0);

        // shooter control

        if (Input.GetMouseButton(0))
        {
            if (GameManager.Instance.CanShoot)
            {
                _shootingCooldown += Time.deltaTime;
                if (_shootingCooldown >= 1 - _shootingRate)
                {
                    _shootingCooldown = 0;
                    ShootProjectile();
                }
            }    
        }
        else
        {
            _shootingCooldown = 1 - _shootingRate;
        }
    }

    private void ShootProjectile()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 destination = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(_maxBulletDistance);
        }

        Bullet bullet = Instantiate(_bulletPrefab, _muzzleTransform.position, Quaternion.identity);
        bullet.transform.forward = (destination - _muzzleTransform.position).normalized;
    }

}

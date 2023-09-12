using UnityEngine;
using UnityEngine.XR;

public class CharacterController : MonoBehaviour
{
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
    
        private Vector2 _rotation = Vector2.zero;
    const string _xAxis = "Mouse X";
    const string _yAxis = "Mouse Y";


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _shootingCooldown = 1 - _shootingRate;
    }

    void Update()
    {
        // camera control
        _rotation.x += Input.GetAxis(_xAxis) * _sensitivity;
        _rotation.x = Mathf.Clamp(_rotation.x, -_xRotationLimit, _xRotationLimit);
        _rotation.y += Input.GetAxis(_yAxis) * _sensitivity;
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);
        var xQuat = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(_rotation.y, Vector3.left);

        _cameraTransform.localRotation = xQuat * yQuat; 

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

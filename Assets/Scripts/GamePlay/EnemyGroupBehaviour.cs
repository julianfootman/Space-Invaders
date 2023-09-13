using UnityEngine;

public class EnemyGroupBehaviour : MonoBehaviour
{
    // left and right movement
    [SerializeField] private float _speed;
    [SerializeField] private float _AICooldown = 2;

    public float movementLimit { get; set; }

    private Vector3 _originPosition;
    private float _movementX;
    private float _decisionTimer;
    private float _currentAxis;

    private void Start()
    {
        _originPosition = transform.localPosition;
        _decisionTimer = _AICooldown;
    }

    private void Update()
    {
        if (_decisionTimer < _AICooldown)
        {
            _decisionTimer += Time.deltaTime;
        }
        else
        {
            _decisionTimer = 0;
            _currentAxis = Random.Range(1, 4) - 2;
        }

        _movementX += _currentAxis * _speed * Time.deltaTime;
        _movementX = Mathf.Clamp(_movementX, -movementLimit, movementLimit);

        transform.localPosition = new Vector3(_movementX + _originPosition.x, 0, transform.localPosition.z);
    }
}

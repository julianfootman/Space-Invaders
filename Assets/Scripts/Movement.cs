using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 10;

    public float Speed { get { return _speed; } set { _speed = value; } }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }
}

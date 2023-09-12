using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2;
    [SerializeField] private float _speed = 10;
    [SerializeField] private GameObject _explisionVFX;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = Instantiate(_explisionVFX);
        go.transform.position = transform.position;
        Destroy(go, 0.5f);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        GameManager.Instance.OnNewBullet();
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnDestroyBullet();
    }
}

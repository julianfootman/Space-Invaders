using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2;
    [SerializeField] private GameObject _explisionVFX;
    [SerializeField] private bool _destroyTargetInstantly;
    [SerializeField] private string _enemyTag;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _enemyTag)
        {            
            if (_destroyTargetInstantly)
            {
                Destroy(other.gameObject);
            }

            GameObject go = Instantiate(_explisionVFX);
            go.transform.position = transform.position;
            Destroy(go, 0.5f);
            Destroy(gameObject);
        }
    }


}

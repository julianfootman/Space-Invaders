using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2;
    [SerializeField] private GameObject _explisionVFX;
    [SerializeField] private string[] _enemyTags;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
        transform.SetParent(GameManager.Instance._bulletParent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_enemyTags.Contains(other.tag))
        {
            GameManager.Instance.OnHitTarget(other.gameObject);
            GameObject go = Instantiate(_explisionVFX, GameManager.Instance._bulletParent);
            go.transform.position = transform.position;
            Destroy(go, 0.5f);
            Destroy(gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNew : MonoBehaviour
{
    [SerializeField] private GameObject _wallHitEffect;
    [SerializeField] private GameObject _enemyHitEffect;
    private int _damage;

    public void SetDamage(int _dam)
    {
        _damage = _dam;
    }

    void OnCollisionEnter(Collision col)// исходя из mode рассчитать урон по col
    {
        if (col.gameObject.TryGetComponent(out EnemyNew enem))
        {
            enem.GetDamage(_damage);
            ContactPoint contactPoint = col.contacts[0];
            GameObject temp = Instantiate(_enemyHitEffect, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            temp.transform.SetParent(contactPoint.otherCollider.transform);
        }
        else
        {
            ContactPoint contactPoint = col.contacts[0];
            GameObject temp = Instantiate(_wallHitEffect, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            temp.transform.SetParent(contactPoint.otherCollider.transform);
            Destroy(gameObject);
        }
    }
}

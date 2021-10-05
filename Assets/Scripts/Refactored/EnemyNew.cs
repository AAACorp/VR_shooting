using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNew : MonoBehaviour
{
    int _hp;
    public void GetDamage(int _damage)
    {
        _hp -= _damage;
    }

    void SetHp(int _healthPoint)
    {
        _hp = _healthPoint;
    }    
}

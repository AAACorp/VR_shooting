using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for test

public class EnemyNew : MonoBehaviour
{
    [SerializeField] private int _hp;
    public Text TextArea; //for test
    public void GetDamage(int _damage)
    {
        _hp -= _damage;
    }

    void SetHp(int _healthPoint)
    {
        _hp = _healthPoint;
    }

    private void Update() //for test
    {
        if (_hp <= 0)
        {
            Debug.Log("Enemy killed!");
            Destroy(gameObject);
        }

        TextArea.text = "HP = " + _hp;
    }//
}

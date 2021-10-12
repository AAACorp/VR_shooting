using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNew : MonoBehaviour
{
    int _damage;

    public void SetDamage(int _dam)
    {
        _damage = _dam;
    }
    void OnColliderEnter(Collider col)// исходя из mode рассчитать урон по col
    {
        col.TryGetComponent(out EnemyNew enem); 
        enem.GetDamage(_damage); //если будет компонент энеми, значит это враг, если нет, то просто на месте удара сделать дырку или что-то типа того
    }
}

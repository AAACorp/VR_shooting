using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRefact : MonoBehaviour
{

}

class ExampleWeaponsa// на каждом оружии
{
    GameObject Barrel;
    GameObject bullet;//префаб пули
    int damage; //из инспектора на каждом оружии прописываем его урон
    float fireRate; //в миллисекундах, для каждого оружия из инспектора будем прописывать частоту (раз в сколько миллисек может стрелять)
    int weaponType; //какой тип оружия, из описанных выше
    //bool canShoot;//можем ли стрелять - меняется когда кулдаун проходит. Еще и надо проверить, есть ли магазин, есть ли в нем патроны, перезаряжено ли оружие
    private void Update() 
    { //OnPress only for Semi-Auto
        //если родитель - рука
        if(InputGetStateDown)
        {
            switch (weaponType)
            {
                case 1:
                    if (checkOfPossibilityShoot)
                    {
                        Shoot();
                        отдача1, флеш1 и звук1;
                        nextShootTime = Time.time + 1f / fireRate;
                    }
                    break;
                case 2:
                    if(checkOfPossibilityShoot)
                    {
                        Shoot();
                        отдача2, флеш2 и звук2;
                        pressedButton = true;
                    }
                    break;
                case 3:
                    if (checkOfPossibilityShoot)
                    {
                        Shoot();
                        отдача3, флеш3 и звук3;
                        hasSlide = false;
                        pressedButton = true;
                    }
            }
        }
        pressedButton = false;
    }

    bool checkOfPossibilityShoot() //тут должна быть проверка на кд, перезаряжено ли оружие, есть ли магазин в оружии, и есть ли вообще патроны в магазине.
                                   //Опять же, здесь нужно знать тип оружия, потому что у какого-то оружия нет кд. А у некоторых(2 и 3) нужно проверять
                                   //отпустили ли кнопку и только потом можно нажать. Что у всех общее? Перезаряжено ли оружие, есть ли магаз и патроны - общее.
                                   //Для начала проверить общее, а потом частное. Пока без дробовиков.
    {
        if (hasSlide)
        {
            if (magazine)
            {
                if(magazine.GetComponent<Magazine>().ammo > 0)
                {
                    switch(weaponType)
                    {
                        case 1: //auto
                            if (Time.time > nextShootTime) return true;
                            else return false;
                            break;
                        case 2: //semi-auto
                            if (pressedButton == false) return true;
                            break;
                        case 3: //need slide every shoot 
                            if (pressedButton == false return true;
                            break;
                    }
                }
            }
        }
    }



}

class Shoot
{
    void Shooting(GameObject bullet)
    {
        Instantiate как-нибудь Bullet bullet = new Bullet(); на Barrel.transform.position, Quaternion.Identity;
        bullet.GetComponent<Bullet>().SetDamage(damage);
        AddForce(bullet);
    }
}


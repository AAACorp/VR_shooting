using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int hp;
    public Text TextArea;
    void Start()
    {
        hp = 10000;
    }

    private void Update()
    {
        if (hp <= 0)
        {
            Debug.Log("Enemy killed!");
            Destroy(gameObject);
        }

        TextArea.text = "HP = " + hp;
    }
}

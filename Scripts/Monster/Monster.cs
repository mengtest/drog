using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    private float HP =  200, maxHP = 200;
    public GameObject HP_UI, HP_UI_Point;
    // Start is called before the first frame update
    void Start()
    {
        HP = 200;
        maxHP = 200;
        HP_UI = Instantiate (HP_UI, HP_UI_Point.transform.position, Quaternion.identity);
        //HP_UI_Point = transform.Find("HP_UI_Point");
    }

    // Update is called once per frame
    void Update()
    {
        HP_UI.transform.position = HP_UI_Point.transform.position;
        HP_UI.transform.Find("Slider").GetComponent<Slider>().value = HP/maxHP;
    }
    public void AddHP(float delta) {
        HP += delta;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理对象头顶HPUI信息
/// </summary>
public class HeadHPUIManager
{
    public GameObject HPUI;
    public Transform HPUIPoint;
    public void Init(Transform nHPUIPoint) {
        HPUIPoint = nHPUIPoint;
        //Debug.Log("HEADHPUIMANGER INIT DEBUG BEGIN");
        HPUI = Resources.Load("HPUI") as GameObject;
        HPUI = GameObject
            .Instantiate(HPUI,HPUIPoint.position,HPUIPoint.rotation);
        //Debug.Log(HPUI.name);
        //HPUI.transform.parent = HPUIPoint;
    }

    /// <summary>
    /// 更新头顶血条UI
    /// </summary>
    /// <param name="newHPRatio">血量百分比</param>
    public void Update(float newHPRatio, string name) {
        HPUI.transform.position = HPUIPoint.position;
        HPUI.transform.Find("Slider").GetComponent<Slider>().value = newHPRatio;
        HPUI.transform.Find("Name").GetComponent<Text>().text = name;
        //Debug.Log(HPUI.transform.Find("Slider").GetComponent<Slider>().value);
    }
}

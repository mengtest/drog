using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class PlayerData {
    public string  name = "Alex";
    public float curHP = 100;
    public float maxHP = 100;
    public float curMP = 100;
    public float maxMP = 100;
    public float attack = 10;
    public float ForwardSpeed = 3;
    public float BackSpeed = 0;

    public int skill1ID;
    public int skill2ID;
    public int skill3ID;
    public int skillAID;
    public int skillDID;

}
[CreateAssetMenu(menuName = "Config/Player")]
[Serializable]
public class PlayerConfig : ScriptableObject {
    public List<PlayerData> PlayerDataList = new List<PlayerData>();
}

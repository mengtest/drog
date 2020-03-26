using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// 
/// </summary>
[Serializable]
public class CreatureData
{
    /// <summary>
    /// 生物ID
    /// </summary>
    public int ID = 0;

    /// <summary>
    /// 生物名称
    /// </summary>
    public string Name = "";

    /// <summary>
    /// 生物体型半径
    /// </summary>
    public float Radius = 0;

    /// <summary>
    /// 生物当前血量
    /// </summary>
    public float curHP = 0;
    /// <summary>
    /// 生物最大血量
    /// </summary>
    public float maxHP = 0;

    /// <summary>
    /// 生物当前魔量
    /// </summary>
    public float curMP = 0;
    /// <summary>
    /// 生物最大魔量
    /// </summary>
    public float maxMP = 0;

    /// <summary>
    /// 生物攻击力
    /// </summary>
    public float Attack = 0;

    /// <summary>
    /// 生物防御力
    /// </summary>
    public float Defence = 0;

    /// <summary>
    /// 生物移动速度
    /// </summary>
    public float Speed = 0;

    /// <summary>
    /// 生物可使用技能ID表
    /// List对应下标下的技能类型
    /// idex0  :站立
    /// idex1  :奔跑
    /// idex2  :普通攻击
    /// idex3  :闪避技能
    /// idex4  :小技能
    /// idex5  :远程技能
    /// idex6  :buff技能
    /// idex...:可扩展
    /// </summary>
    public List<int> EnableSkillIDList = new List<int>();
    public CreatureData(CreatureData a = null) {
        if(a==null) {
            ID = 0;
            Name = null;
            Radius = 0;
            curHP = 0;
            maxHP = 0;
            curMP = 0;
            maxMP = 0;
            Attack = 0;
            Defence = 0;
            Speed = 0;
            EnableSkillIDList =  new List<int>();
        }
        else {
            ID = a.ID;
            Name = a.Name;
            Radius = a.Radius;
            curHP = a.curHP;
            maxHP = a.maxHP;
            curMP = a.curMP;
            maxMP = a.maxMP;
            Attack = a.Attack;
            Defence = a.Defence;
            Speed = a.Speed;
            EnableSkillIDList = new List<int>();
            foreach(var e in a.EnableSkillIDList)
                EnableSkillIDList.Add(e);
        }
        
    }
}

/// <summary>
/// 生物配置表
/// </summary>
[Serializable]
public class CreatureConfig : ScriptableObject {
    public List<CreatureData> CreatureDataList = new List<CreatureData>();
}
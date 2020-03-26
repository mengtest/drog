using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
/*
[Serializable]
public class Rect {
    public float Width;
    public float Height;
}

//s

[Serializable]
public class SkillSEDate {
    public string SkillSEName;
    public string PosName;
    public bool IfFollow;
    public float BeginTime;
    public float DuringTime;
}

[Serializable]
public class SkillData
{
    /// <summary>
    /// ID
    /// </summary>
    public int SkillID; 

    /// <summary>
    /// 技能名称
    /// </summary>
    public string SkillName;

    /// <summary>
    /// 当前技能状态名
    /// </summary>
    public string StateName;

    /// <summary>
    /// 技能动画名称
    /// </summary>
    public string AniName;

    /// <summary>
    /// 技能特效 使用表 (名称  挂载点名称  是否跟随挂载点  开始时间/帧)
    /// </summary>
    /// <typeparam name="SkillSEDate"></typeparam>
    /// <returns></returns>
    public List<SkillSEDate> SkillSEDates = new List<SkillSEDate>();

    /// <summary>
    /// 技能时长
    /// </summary>
    public float SkillDuringTime;

    /// <summary>
    /// 技能效果开始帧/时间
    /// </summary>
    public float SkillBeginTime;

    /// <summary>
    /// 技能对应人物状态
    /// </summary>
    public int PlayerState;

    /// <summary>
    /// 技能生成时与人物的中心距离/偏离向量
    /// </summary>
    public float DistanceToPlayer;

    /// <summary>
    /// 技能（矩形）范围（长  宽）
    /// </summary>
    public Rect SKillEffectRect;

    /// <summary>
    /// 技能对人物移动的影响（单位时间变化 可用向量表示）
    /// </summary>
    public float DistanceMovePlayer;

    /// <summary>
    /// 技能对基础攻击的伤害增长率（最后值为每段效果数值）
    /// </summary>
    public float InfluenceRatio;

    /// <summary>
    /// 技能冷却时间
    /// </summary>
    public float CoolingTime;
    public float Usetime;

    /// <summary>
    /// 技能持续段数
    /// </summary>
    public int CastTimes;

    /// <summary>
    /// 每段持续时间
    /// </summary>
    public float EachCastTime;

    /// <summary>
    /// 技能是否为飞行类
    /// </summary>
    public bool IsFly;

    /// <summary>
    /// 是飞行类->飞行速度
    /// </summary>
    public float FlyV;

    /// <summary>
    /// 技能是否为BUFF/DEBUFF类
    /// </summary>
    public bool IsBUFF;

    /// <summary>
    /// 是BUFF/DEFUU类->作用属性
    /// </summary>
    public int OnAttribute;

    /// <summary>
    /// 是BUFF/DEFUU类->作用是否回收（总段数的数值）
    /// </summary>
    public bool IfBack;

    //ssssss

}

[CreateAssetMenu(menuName = "Config/Skill")]
[Serializable]
public class SkillConfig : ScriptableObject {
    public List<SkillData> SkillDataList = new List<SkillData>();
}*/
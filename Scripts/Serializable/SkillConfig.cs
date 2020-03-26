using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


/// /// <summary>
/// 技能特效信息
/// </summary>
[Serializable]
public class SkillSE {
    /// <summary>
    /// 技能特效ID
    /// </summary>
    public int SEID = 0;

    /// <summary>
    /// 技能特效名称
    /// </summary>
    public string SEName = "";

    /// <summary>
    /// 技能特效加载点名称
    /// </summary>
    public string SELoadPointName = "";

    /// <summary>
    /// 是否跟随技能挂载点
    /// </summary>
    public bool IfFollowLoadPoint = false;

    /// <summary>
    /// 是否随动画停止消亡
    /// </summary>
    public bool IfFollowAnimationDestroy = false; //true时表示人物状态驱动特效销毁，false时表示时间驱动特效销毁
    
    /// <summary>
    /// 技能特效开始时间
    /// </summary>
    public float SkillSEBeginTime = 0;

    /// <summary>
    /// 技能特效持续时间
    /// </summary>
    public float SkillSEDuringTime = 0;
}



/// <summary>
/// 攻击附加特性
/// </summary>
[Serializable]
public class AttackAdditionalFeatures {
    /// <summary>
    /// 特性类型  1:击退 2:击飞 4:减速 8:破格挡 16:吸血 32:减MP 64:镜头震动 128:破防
    /// </summary>
    public int ftype;

    /// <summary>
    /// 击退距离
    /// </summary>
    public float BeakBackDis;

    /// <summary>
    /// 浮空高度
    /// </summary>
    public float FloatingHeight;
    /// <summary>
    /// 浮空持续时间
    /// </summary>
    public float FloatingDuringTime;

    /// <summary>
    /// 削减速度比率
    /// </summary>
    public float CutSpeedRatio;
    /// <summary>
    /// 减速持续时间
    /// </summary>
    public float CutSpeedDuringTime;

    /// <summary>
    /// 从打出伤害中吸血
    /// </summary>
    public float SuckBloodFromDamgeRatio;

    /// <summary>
    /// 削减的MP量
    /// </summary>
    public float CutMp;

    public float CutDefenceRatio;
    public float CutDefenceDuringTime;
}

/// <summary>
/// 攻击型技能
/// </summary>
[Serializable]
public class SkillAttackType {
    /*/// <summary>
    /// 技能攻击段数
    /// </summary>
    public int SkillAttackTimes;

    /// <summary>
    /// 技能每段攻击信息表
    /// </summary>
    public List<SkillAttackJudgeInf> SkillAttackJudgeInfList;*/

    /// <summary>
    /// 技能判定开始时间
    /// </summary>
    public float SkillAttackJudgeBeginTime;

    /// <summary>
    /// 技能判定中心点离施法者的距离
    /// </summary>
    public float SkillAttackJudgeDistance;

    /// <summary>
    /// 技能判定区域类型 1:矩形   2:圆形   3:扇形
    /// </summary>
    public int SkillAttackJudgeAreaType;

    /// <summary>
    /// 技能判定区域半径，供 2,3 类型使用
    /// </summary>
    public float SkillAttackJudgeAreaRadius;
    /// <summary>
    /// 技能判定区域角度，供 3 类型使用
    /// </summary>
    public float AttackJudgeAreaAngle;
    /// <summary>
    /// 技能判定区域宽度，供 1 类型使用
    /// </summary>
    public float AttackJudgeAreaWidth;
    /// <summary>
    /// 技能判定区域长度，供 1 类型使用
    /// </summary>
    public float AttackJudgeAreaHeight;

    /// <summary>
    /// 技能判定伤害系数 与攻击力Attack相关
    /// </summary>
    public float SkillAttackJudgeDamageRatio;

    public AttackAdditionalFeatures AAF;
}  


/// <summary>
/// BUFF型技能
/// </summary>
[Serializable]
public class SkillBUFFType {

    /// <summary>
    /// BUFF作用属性 1:curHP 2:curMP 4:移动速度 8:防御力 16:攻击力
    /// </summary>
    public int RoleAttribute;

    /// <summary>
    /// BUFF作用开始时间
    /// </summary>
    public float BeginTime;

    /// <summary>
    /// BUFF作用次数  移速和防御属性固定作用一次
    /// </summary>
    public int RoleTimes;

    /// <summary>
    /// BUFF每次作用间隔时间，移速和防御属性间隔时间即持续时间
    /// </summary>
    public float EachRoleTime;

    /// <summary>
    /// BUFF效果值
    /// </summary>
    public float EffectValue;

    /// <summary>
    /// BUFF效果率
    /// </summary>
    public float EffectRatio;
    /// <summary>
    /// BUFF效果是否在BUFF结束后回收
    /// </summary>
    public bool IfRecycle;
}

/// <summary>
/// 属性效果
/// </summary>
[Serializable]
public class AttributeEffect {
    /// <summary>
    /// 作用属性 1:curHP 2:curMP 4:移动速度 8:防御力 16:攻击力
    /// </summary>
    public int RoleAttribute;

    /// <summary>
    /// 作用值 供1、2用
    /// </summary>
    public float RoleValue;

    /// <summary>
    /// 作用率 供4+用
    /// </summary>
    public float RoleRatio;

    /// <summary>
    /// 持续类型 1:跟随分段 2:范围内持续作用
    /// </summary>
    public int DuringType;

    /// <summary>
    /// 作用的属性是否会被收回
    /// </summary>
    public bool IfRecycle;
}
/// <summary>
/// 子弹型技能
/// </summary>
[Serializable]
public class SkillBulletType {
    /// <summary>
    /// 子弹加载的特效名称
    /// </summary>
    public string BulletSEName;

    /// <summary>
    /// 子弹发射时间
    /// </summary>
    public float BulletFireTime;

    /// <summary>
    /// 是否释放至施法范围内最近的敌人处
    /// </summary>
    public bool BulletFireEnemyPos;

    /// <summary>
    /// 是否锁定目标
    /// </summary>
    public bool IfLockEnemy;

    /// <summary>
    /// 最大检索距离
    /// </summary>
    public float MaxSearchDistance;

    /// <summary>
    /// 是否进行攻击判定
    /// </summary>
    public bool IfAttackJudge;
    /// <summary>
    /// 子弹攻击单次判定信息
    /// </summary>
    public SkillAttackType Attack;

    /// <summary>
    /// 法术场所带的BUFF效果
    /// </summary>
    //public SkillBUFFType BUFF;

    /// <summary>
    /// 子弹飞行速度
    /// </summary>
    public float FlySpeed;

    /// <summary>
    /// 子弹最大持续时间
    /// </summary>
    public float MaxDuringTime;

    /// <summary>
    /// 子弹是否能够穿透目标，对具有飞行速度的子弹而言，不能穿透就会碰触消亡
    /// </summary>
    public bool IfPenetrate;

    /// <summary>
    /// 是否能够作用多个目标
    /// </summary>
    public bool IfRoleOnMultipleGoals;

    /// <summary>
    /// 子弹攻击段数  对于法术场子弹而言
    /// </summary>
    public int JudgeTimes;

    /// <summary>
    /// 子弹每次攻击判定时间间隔
    /// </summary>
    public float EachAttackTime;

    
    /// <summary>
    /// 减速率
    /// </summary>
    public float CutSpeedRatio;
    
    /// <summary>
    /// 属性效果
    /// </summary>
    public List<AttributeEffect> AE;
}


/// <summary>
/// 位移型技能
/// </summary>
[Serializable]
public class SkillMoveType {
    /// <summary>
    /// 开始移动的时间
    /// </summary>
    public float BeginTime;
    /// <summary>
    /// 移动持续的时间
    /// </summary>
    public float DuringTime;
    /// <summary>
    /// 移动的速度
    /// </summary>
    public float Speed;
}

/// <summary>
/// 上空效果
/// </summary>
[Serializable]
public class SkillFloatType {
    /// <summary>
    /// 上空开始时间
    /// </summary>
    public float BeginTime;
    /// <summary>
    /// 上空速度
    /// </summary>
    public float UPv;
}

/// <summary>
/// 技能信息
/// </summary>
[Serializable]
public class SkillData
{
    

    /// <summary>
    /// 技能名称
    /// </summary>
    public string SkillName = "";

    /// <summary>
    /// 技能ID
    /// </summary>
    public int ID = 0;

    /// <summary>
    /// 技能图标
    /// </summary>
    public Sprite SkillIcon;

    /// <summary>
    /// 技能状态名称
    /// </summary>
    public string StateName = "";

    /// <summary>
    /// 技能动画名称
    /// </summary>
    public string AnimationName = "";

    /// <summary>
    /// 技能特效信息表
    /// </summary>
    /// <typeparam name="SkillSE"></typeparam>
    /// <returns></returns>
    public List<SkillSE> SkillSEList = new List<SkillSE>();

    /// <summary>
    /// 格挡特效名字
    /// </summary>
    public string BlockSEName = "";

    //格挡特效持续时间
    public float BlockSEDuringTime = 0;

    /// <summary>
    /// 连续技下个技能的在总表中的ID
    /// </summary>
    public int NextSkillID = -1;

    /// <summary>
    /// 技能存在时间
    /// </summary>
    public float DuringTime = 0;

    /// <summary>
    /// 技能前摇判定结束时间
    /// </summary>
    public float SkillRollEndTime = 0;

    /// <summary>
    /// 技能后摇判定开始时间
    /// </summary>
    public float ShakeBackBeginTime = 0;

    /// <summary>
    /// 技能冷却时间
    /// </summary>
    public float CoolingTime = 0;

    /// <summary>
    /// 技能MP消耗
    /// </summary>
    public float CostMP;

    /*
    /// <summary>
    /// 技能类型 1:攻击  2:BUFF  4:子弹。二进制表示，可同时存在多种状态
    /// </summary>
    public int SkillType = 0;
    public float SkillJudgeTime = 0;
    public float SkillDamage = 0;
    public float MoveSpeed = 0;
    public SkillData(int I=0, string SKN="", string STN="", string AN="", float DT=0, float RT=0, float BB=0, float CT=0, float JT=0, float MS=0, float DM=0) {
        ID = I;
        SkillName = SKN;
        StateName = STN;
        AnimationName = AN;
        DuringTime = DT;
        SkillRollEndTime = RT;
        ShakeBackBeginTime = BB;
        CoolingTime = CT;
        SkillJudgeTime = JT;
        MoveSpeed = MS;
        SkillDamage = DM;
    }*/



    /// <summary>
    /// 多段攻击判定信息表
    /// </summary>
    /// <typeparam name="SkillAttackType"></typeparam>
    /// <returns></returns>
    public List<SkillAttackType> ATKs = new List<SkillAttackType>();
    /// <summary>
    /// 多段BUFF判定信息表
    /// </summary>
    /// <typeparam name="SkillBUFFType"></typeparam>
    /// <returns></returns>
    public List<SkillBUFFType> BUFFs = new List<SkillBUFFType>();
    /// <summary>
    /// 多子弹类信息表
    /// </summary>
    /// <typeparam name="SkillBulletType"></typeparam>
    /// <returns></returns>
    public List<SkillBulletType> Bullets = new List<SkillBulletType>();
    /// <summary>
    /// 多段位移信息表
    /// </summary>
    /// <typeparam name="SkillMoveType"></typeparam>
    /// <returns></returns>
    public List<SkillMoveType> MOVEs = new List<SkillMoveType>();
    /// <summary>
    /// 上空效果表
    /// </summary>
    /// <typeparam name="SkillFloatType"></typeparam>
    /// <returns></returns>
    public List<SkillFloatType> Floats = new List<SkillFloatType>();


    //--------------1:攻击类型-----------------------------
    /// <summary>
    /// 攻击类技能信息
    /// </summary>
    //public SkillAttackType SkillAttack;


    //---------------2:BUFF/DEBUFF------------------------
    /// <summary>
    /// BUFF类技能信息
    /// </summary>
    //public SkillBUFFType SkillBUFF;

    /// <summary>
    /// 子弹类技能信息
    /// </summary>
    //public SkillBulletType SkillBullet;

}

//[CreateAssetMenu(menuName = "Config/SkillConfig")]
public class SkillConfig : ScriptableObject {
    public List<SkillData> SkillDataList = new List<SkillData>();
}



using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 生物体
/// </summary>
public class Creature : ObjectInf {
    const int IDLE = 0;
    const int RUN = 1;
    const int BEATTACKED = 2;
    const int DEAD = 3;
    const int FLOATING = 4;

    /// <summary>
    /// 当前创建的生命体的唯一标识
    /// </summary>
    //public int ID;

    /// <summary>
    /// 当前生物体的信息
    /// </summary>
    public CreatureData  curCreatureData = new CreatureData();

    /// <summary>
    /// 控制当前生物体状态的状态机
    /// </summary>
    public FSM curFSM = new FSM();

    /*
    /// <summary>
    /// 所有的技能配置表
    /// </summary>
    public SkillConfig allSkillConfig;
    */

//
    /// <summary>
    /// 当前生物可使用的技能数据表
    /// idex0  :站立
    /// idex1  :奔跑
    /// idex2  :受击
    /// idex3  :死亡
    /// idex4  :浮空
    /// idex5  :普通攻击
    /// idex6  :闪避技能
    /// idex7  :格挡
    /// idex8  :技能1
    /// idex9  :技能2
    /// idex10 :技能3
    /// diex11 :技能4
    /// </summary>
    public List<SkillData> curSkillDataList = new List<SkillData>();  

    /// <summary>
    /// 当前生物使用的技能表中的技能上一次施法时间
    /// </summary>
    public List<float> SkillLastFireTime = new List<float>();

    /// <summary>
    /// 当前正在使用的技能在可使用技能表中的下标
    /// </summary>
    public int curSID = 0;

    /// <summary>
    /// 当前正在使用的技能达到的阶段
    /// </summary>
    //public ValueTuple<int,int,int> curSkillStage;

    /// <summary>
    /// 当前生物具有的BUFF,BUFF装载时间，BUFF执行阶段
    /// </summary>
    //public Queue<ValueTuple<SkillBUFFType,float,int>> BUFFQueue;

    /// <summary>
    /// 头顶血条UI管理器
    /// </summary>
    //public HeadHPUIManager headHPUIManager;

    /// <summary>
    /// 用来处理人物技能事件的计时器，当前技能结束时重置
    /// </summary>
    /// <returns></returns>
    public LogicTimer timer = new LogicTimer();

    /// <summary>
    /// 处理只会自动消除的BUFF
    /// </summary>
    /// <returns></returns>
    public LogicTimer BUFFtimer = new LogicTimer();


    public bool IfBeAttacked = false;
    /// <summary>
    /// 初始化人物
    /// </summary>
    /// <param name="LivingCreatureID"></param>
    public void InitLivinngCreature(int LivingCreatureID = 0) {

        //---------更改名字
        //gameObject.name = PlayerName;
        //---------加载全局技能表-----------------------------------------------------------------------------------
        //SkillConfig allSkillConfig = Resources.Load("SkillConfig") as SkillConfig;
        //---------加载全局生物信息--------------------------------------------------------------------------------
        //LivingCreatureConfig allLivingCreature = Resources.Load("LivingCreatureConfig") as LivingCreatureConfig;
        //---------初始化当前生物为ID==LivingCreatureID的生物信息-------------------------------------------------------------
        //Debug.Log("creature skill count : " + RSDB.creatureConfig.CreatureDataList[0].EnableSkillIDList.Count);
        curCreatureData = new CreatureData(RSDB.creatureConfig.CreatureDataList[LivingCreatureID]);
        
        //---------初始化当前生物使用的技能表------------------------------------------------------------------------------
        //Debug.Log("Skill Count : " + RSDB.skillConfig.SkillDataList.Count);
        foreach(var e in curCreatureData.EnableSkillIDList) 
            curSkillDataList.Add(RSDB.skillConfig.SkillDataList[e]);
        //---------初始化当前生物使用技能的时间戳--------------------------------------------------------------------------
        foreach(var e in curSkillDataList)
            SkillLastFireTime.Add(-10000);
        //---------初始化当前生物正在使用的技能为Idle idex=0---------------------------------------------------------------------
        //curSkillIdex = 0;
        //---------初始化当前生物正在使用的技能已经到达的阶段------------------------------------------------------------------
        //curSkillStage = (0,0,0);
        // ---------初始化当前生物的状态机--------------------------------------------------------------------------------------
        foreach(var e in curSkillDataList) {
            curFSM.AddState(new IState(e,name));
        }
        //curFSM.AddState()
        //curFSM.ChangeState("Idle");
        //---------获取当前生物的唯一标识
        // = IDManager.GetID();
        //---------初始化BUFF队列-----------------------------------------------------------------------------------------------
        //BUFFQueue = new Queue<(SkillBUFFType, float, int)>();
        //---------初始化头顶HPUI-------------------------------------------------------------------------------------------
        //headHPUIManager = new HeadHPUIManager();
        //headHPUIManager.Init(transform.Find("HPUIPoint"));
        Radius = curCreatureData.Radius;
        AutoHPMPRecover();
    }

    public void AutoHPMPRecover() {
        if(curCreatureData.curHP>0) {
            HPRecover(2);
            MPRecover(1);
        }
        BUFFtimer.Add((Logic.time+3,delegate() {
            AutoHPMPRecover();
        }));
    }
    public void UpdateCreature() {
        if(curCreatureData.curHP<=0) {
            //死亡
            ToUseSkill(DEAD);
        }
        timer.Update();
        BUFFtimer.Update();
        curFSM.Update();
        //浮空受击y轴不移动
        //Debug.Log("UPv " + UPv);
        //if(!IfBeAttacked) {
            //y轴物理位移
            pos.y += (UPv+UPv-G*Logic.eachframtime)*Logic.eachframtime/2;
            UPv -= G*Logic.eachframtime;
        //} else if(UPv<0&&pos.y>0) {
        //    Debug.Log("浮空受击");
        //    UPv = 5;
        //}
        if(pos.y<=0) {
            if(UPv<0) {
                UPv = 0;
            }
            pos.y = 0;
        }
        if(IfBeAttacked) {
            //Debug.Log("受击态!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");                                                
        }
        IfBeAttacked = false;
    }

    /// <summary>
    /// 控制生命体移动
    /// </summary>
    /// <param name="Forward">前进向量</param>
    /// <param name="Direction">方向角</param>
    public virtual void ToMove(int d) {
        ToUseSkill(1);
        if(curSID==1) {
            dir = d;
            Move(curCreatureData.Speed);
            //float angle = 15.0f * dir;
            //float f = (float)(angle*Math.PI/180.0f);
            //pos += (new Vector3((float)Math.Sin(f),0,(float)Math.Cos(f)))*((float)(curCreatureData.Speed*Logic.eachframtime));
            rot = new Vector3(0,15.0f*dir,0);
        }
    }

    //
    /// <summary>
    /// 使用技能
    /// </summary>
    /// <param name="useSkillIdex">使用的技能在当前生物可用技能表中的下标</param>
    public virtual bool ToUseSkill(int useSID) {
        //技能冷却、MP判定
        if(Logic.time-SkillLastFireTime[useSID] <curSkillDataList[useSID].CoolingTime) 
            return false; //技能未冷却
        if(curCreatureData.curMP<curSkillDataList[useSID].CostMP)
            return false; //MP不足
        //询问状态机状态是否能够改变
        if(!curFSM.ChangeState(curSkillDataList[useSID].StateName)) 
            return false; //不可更改为当前技能;
        
        //更改当前正在释放的技能为请求释放的技能
        curSID = useSID;

        //技能进入冷却时间
        SkillReset(curSID);
        curCreatureData.curMP -= curSkillDataList[useSID].CostMP;

        //--------------------------------攻击属性----------------------------------------------------------------
        foreach(var t in curSkillDataList[useSID].ATKs) {
            SkillAttackType e = t;
            timer.Add((Logic.time+e.SkillAttackJudgeBeginTime,delegate() {
                MeleeMultiplayerAttack(e);
            }));
        }
        //--------------------------------BUFF属性----------------------------------------------------------------
        foreach(var t in curSkillDataList[useSID].BUFFs) {
            SkillBUFFType e = t;
            timer.Add((Logic.time+e.BeginTime,delegate(){
                for(int i = 0;i < e.RoleTimes;i++) {
                    int j = i;
                    BUFFtimer.Add((Logic.time+e.BeginTime+j*e.EachRoleTime,delegate() {
                        if((e.RoleAttribute&1) != 0) {
                            HPRecover(e.EffectValue);
                            //curCreatureData.curHP += e.EffectValue;
                            //if(curCreatureData.curHP>curCreatureData.maxMP) curCreatureData.curHP = curCreatureData.maxHP;
                        }
                        if((e.RoleAttribute&2) != 0) {
                            MPRecover(e.EffectValue);
                            //curCreatureData.curMP += e.EffectValue;
                            //if(curCreatureData.curMP>curCreatureData.maxMP) curCreatureData.curMP = curCreatureData.maxMP;
                        }
                        if((e.RoleAttribute&4) != 0) {
                            //curCreatureData.Speed *= e.EffectRatio>1?0:(1-e.EffectRatio);
                            SpeedChange(e.EffectRatio);
                        }
                        if((e.RoleAttribute&8) != 0) {
                            //curCreatureData.Defence *= e.EffectRatio>1?0:(1-e.EffectRatio);
                            DefenceChange(e.EffectRatio);
                        }
                        if((e.RoleAttribute&16) != 0) {
                            //curCreatureData.Attack *= e.EffectRatio>1?0:(1-e.EffectRatio);
                            AttackChange(e.EffectRatio);
                        }
                    }));
                    if(e.IfRecycle) {
                        BUFFtimer.Add((Logic.time+e.BeginTime+e.RoleTimes*e.EachRoleTime,delegate() {
                            if((e.RoleAttribute&1) != 0) {
                                HPRecover(-e.EffectValue*e.RoleTimes);
                                //curCreatureData.curHP -= e.EffectValue*e.RoleTimes;
                            }
                            if((e.RoleAttribute&2) != 0) {
                                MPRecover(-e.EffectValue*e.RoleTimes);
                                //curCreatureData.curMP -= e.EffectValue*e.RoleTimes;
                            }    
                            if((e.RoleAttribute&4) != 0) {
                            //curCreatureData.Speed *= e.EffectRatio>1?0:(1-e.EffectRatio);
                                SpeedChange(-e.EffectRatio);
                            }
                            if((e.RoleAttribute&8) != 0) {
                                //curCreatureData.Defence *= e.EffectRatio>1?0:(1-e.EffectRatio);
                                DefenceChange(-e.EffectRatio);
                            }
                            if((e.RoleAttribute&16) != 0) {
                                //curCreatureData.Attack *= e.EffectRatio>1?0:(1-e.EffectRatio);
                                AttackChange(-e.EffectRatio);
                            }
                        }));
                    }
                }
            }));
        }
        //--------------------------------子弹属性----------------------------------------------------------------
        foreach(var t in curSkillDataList[useSID].Bullets) {
            SkillBulletType e = t;
            timer.Add((Logic.time+e.BulletFireTime,delegate() {
                BulletsManager.CreateBullet(name,e);
            }));
        }
        //--------------------------------位移属性----------------------------------------------------------------
        foreach(var t in curSkillDataList[useSID].MOVEs) {
            SkillMoveType e = t;
            for(float i = 0;i<e.DuringTime;i+=Logic.eachframtime) {
                timer.Add((Logic.time+e.BeginTime+i,delegate() {
                    Move(e.Speed);
                    //float angle = 15.0f * dir;
                    //float f = (float)(angle*Math.PI/180.0f);
                    //pos += (new Vector3((float)Math.Sin(f),0,(float)Math.Cos(f)))*((float)(e.Speed*Logic.eachframtime));
                    //rot = new Vector3(0,15.0f*dir,0);
                }));
            }
        }


        //curSkillStage = (0,0,0);

        //技能释放成功
        Debug.Log("use skill " + useSID);
        return true;
    }

    /// <summary>
    /// 攻击敌人
    /// </summary>
    /// <param name="e"></param>
    public virtual void Attack(SkillAttackType atk) {

    }

    /// <summary>
    /// 攻击多个敌人
    /// </summary>
    /// <param name="atk"></param>
    public virtual void MeleeMultiplayerAttack(SkillAttackType atk) {
        if(atk.SkillAttackJudgeAreaType==3) {
            Vector3 center = new Vector3(0,0,0);
            float angleRadian = (float)(dir*15f*Math.PI/180.0f);
            float hudu = (float)(atk.AttackJudgeAreaAngle*Math.PI/180.0f);

            //扇形中心
            center.z = pos.z+(float)(atk.SkillAttackJudgeDistance*Math.Cos(angleRadian));
            center.x = pos.x+(float)(atk.SkillAttackJudgeDistance*Math.Sin(angleRadian));
            center.y = pos.y;

            //扇形正前方最远点
            Vector3 centerTo = new Vector3(0,0,0);
            centerTo.z = center.z+(float)(atk.SkillAttackJudgeAreaRadius*Math.Cos(angleRadian));
            centerTo.x = center.x+(float)(atk.SkillAttackJudgeAreaRadius*Math.Sin(angleRadian));
            centerTo.y = center.y;

            foreach(var t in Logic.obinfmp) {
                var e = t.Value;
                if(tag==e.tag||e.curCreatureData.curHP<=0) {
                    continue;
                }
                if(Judge.IsCircleIntersectFan(
                    e.pos.z,e.pos.x,e.curCreatureData.Radius,
                    center.z,center.x,
                    centerTo.z,centerTo.x,
                    hudu)) {
                        e.ToBeAttacked(atk.SkillAttackJudgeDamageRatio*curCreatureData.Attack, name, atk.AAF);
                    }
            }
        }
        else if(atk.SkillAttackJudgeAreaType==2) {
            foreach(var t in Logic.obinfmp) {
                var e = t.Value;
                if(tag==e.tag||e.curCreatureData.curHP==0) {
                    continue;
                }
                if(Vector3.Distance(pos,e.pos)<=atk.SkillAttackJudgeAreaRadius+e.curCreatureData.Radius) {
                    e.ToBeAttacked(atk.SkillAttackJudgeDamageRatio*curCreatureData.Attack, name, atk.AAF);
                }
            }
        }
    }

    /// <summary>
    /// 恢复HP
    /// </summary>
    /// <param name="HP"></param>
    public virtual void HPRecover(float HP) {
        curCreatureData.curHP += HP;
        if(curCreatureData.curHP>curCreatureData.maxHP) {
            curCreatureData.curHP = curCreatureData.maxHP;
        } else if(curCreatureData.curHP<0) {
            curCreatureData.curHP = 0;
        }
    }
    /// <summary>
    /// 恢复MP
    /// </summary>
    /// <param name="MP"></param>
    public virtual void MPRecover(float MP) {
        curCreatureData.curMP += MP;
        if(curCreatureData.curMP>curCreatureData.maxMP) {
            curCreatureData.curMP = curCreatureData.maxMP;
        } else if(curCreatureData.curMP<0) {
            curCreatureData.curMP = 0;
        }
    }
    public virtual void SpeedChange(float ratio) {
        
        if(ratio>=0) {
            curCreatureData.Speed *= ratio;
        } else {
            curCreatureData.Speed /= -ratio;
        }
        //ratio>=0?(curCreatureData.Speed*=(1-ratio)):(curCreatureData/=(1-ratio));
    }
    public virtual void AttackChange(float ratio) {
        
        if(ratio>=0) {
            curCreatureData.Attack *= (ratio);
        } else {
            curCreatureData.Attack /= (-ratio);
        }
    }
    public virtual void DefenceChange(float ratio) {
        
        if(ratio>=0) {
            curCreatureData.Defence *= (ratio);
        } else {
            curCreatureData.Defence /= (-ratio);
        }
    }

    /// <summary>
    /// 人物受击
    /// </summary>
    /// <param name="Damage"></param>
    public virtual void ToBeAttacked(float Damage, string from, AttackAdditionalFeatures aaf) {
        if(curFSM.CurState.GetStateName()=="Dodge") {
            Debug.Log("闪避");
            return;
        } else if(curFSM.CurState.GetStateName()=="Block") {
            if((aaf.ftype&8)>0) {
                BUFFtimer.Add((Logic.time,delegate() {
                    DefenceChange(0.7f);
                }));
                BUFFtimer.Add((Logic.time+2f,delegate() {
                    DefenceChange(0.7f);
                }));
                ToUseSkill(BEATTACKED);
            } else {
                Debug.Log("格挡");
                //TODO:特效
                int id = Logic.GetNewID();
                //Move()
                MoveContrast(0.4f);
                View.timer.Add((Logic.time,delegate() {
                    View.CreatePlayerEffect(id,curFSM.CurState.GetState().BlockSEName,name,"zhengqianfang", false);
                }));
                View.timer.Add((Logic.time+curFSM.CurState.GetState().BlockSEDuringTime,delegate() {
                    View.RemovePlayerEffect(id);
                }));
                return;
            }
        } else {
            //curCreatureData.curHP -= Damage;
            ToUseSkill(BEATTACKED);
            //if(curCreatureData.curHP < 0)
            //    curCreatureData.curHP = 0;
        }
        if((aaf.ftype&1)>0) {
            //击退
            MoveContrast(aaf.BeakBackDis);
        }
        if((aaf.ftype&2)>0) {
            //TODO:击飞
            Debug.Log("击飞！！！！！！");
            ToUseSkill(FLOATING);
        }
        if((aaf.ftype&4)>0) {
            //减速
            BUFFtimer.Add((Logic.time, delegate() {
                curCreatureData.Speed *= aaf.CutSpeedRatio>1?0:(1-aaf.CutSpeedRatio);
            }));
            BUFFtimer.Add((Logic.time+aaf.CutSpeedDuringTime, delegate() {
                curCreatureData.Speed /= aaf.CutSpeedRatio>1?0:(1-aaf.CutSpeedRatio);
            }));
        }
        if((aaf.ftype&16)>0) {
            //吸血
            Logic.obinfmp[from].HPRecover(Damage*aaf.SuckBloodFromDamgeRatio);
        }
        if((aaf.ftype&32)>0) {
            //减蓝
            Logic.obinfmp[from].MPRecover(-aaf.CutMp);
        }
        if((aaf.ftype&64)>0) {
            //TODO:镜头震动
            if(name==Client.name) {
                View.timer.Add((Logic.time,delegate() {
                    GameObject.Find("MainCamera").GetComponent<ShakeCamera>().timer = 0;
                }));
            }
        }
        if((aaf.ftype&128)>0) {
            BUFFtimer.Add((Logic.time,delegate() {
                DefenceChange(aaf.CutDefenceRatio);
            }));
            BUFFtimer.Add((Logic.time,delegate() {
                DefenceChange(-aaf.CutDefenceRatio);
            }));
        }
        if((aaf.ftype&256)>0) {
            
        }
        HPRecover(-(Damage*(1-(curCreatureData.Defence>100?1:curCreatureData.Defence/100f))+1));
        IfBeAttacked=true;
        Debug.Log("受击态 转换");
        if(UPv<0) {
            UPv = 4;
        } else if(UPv>0) {
            UPv += 2;
        }
        if(curFSM.CurState.GetStateName()!="BeAttacked")
                return ;
        timer.Clear();
    }

    /// <summary>
    /// 重置技能，即重新开始计算冷却
    /// </summary>
    /// <param name="SkillIdex">需要重置的技能在当前生物可用技能表中的下标</param>
    public virtual void SkillReset(int SID) {
        SkillLastFireTime[SID] = Logic.time;
    }

    /// <summary>
    /// 获得技能的剩余冷却时间
    /// </summary>
    /// <param name="SkillIdex">需要获得剩余冷却时间的技能在当前生物可用技能表中的下标</param>
    /// <returns>目标技能的剩余冷却时间</returns>
    public virtual float GetSkillRemainingCoolingTime(int SkillIdex) {
        return (Logic.time-SkillLastFireTime[SkillIdex]>curSkillDataList[SkillIdex].CoolingTime)?0:(curSkillDataList[SkillIdex].CoolingTime-Logic.time+SkillLastFireTime[SkillIdex]);
    }

}




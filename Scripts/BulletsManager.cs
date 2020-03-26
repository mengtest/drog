using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 子弹管理器
/// </summary>
public class BulletsManager
{
    public static Vector3 ERRORV = new Vector3(-100000,-100000,-100000);
    public static Dictionary<int, Bullet> buob = new Dictionary<int, Bullet>();
    public static List<int> waitToRemove = new List<int>();
    public static void Init() {

    }
    public static void Update() {
        waitToRemove.Clear();
        foreach(var t in buob) {
            var e = buob[t.Key];
            e.Update();
            if(e.ifEnd) {
                waitToRemove.Add(t.Key);
            }
        }
        foreach(var e in waitToRemove) {
            if(buob.ContainsKey(e)) {
                DeleteBullet(e);
            }
        }
        waitToRemove.Clear();
    }
    public static void CreateBullet(string mastername, SkillBulletType bi) {
        var bu = new Bullet(mastername,Logic.time,bi);
        buob[bu.id] = bu;
        View.CreateBullet(mastername,bu.id,bu.bu.BulletSEName,bu.pos, bu.rot, bu.bu.FlySpeed>0?true:false);
    }
    public static void DeleteBullet(int id) {
        buob.Remove(id);
        View.RemoveBullet(id);
    }
    public static Vector3 GetBulletPos(int id) {
        if(!buob.ContainsKey(id)) {
            return ERRORV;
        }
        return buob[id].pos;
    }
    public static Vector3 GetBulletRot(int id) {
        if(!buob.ContainsKey(id)) {
            return ERRORV;
        }
        return buob[id].rot;
    }
}


/// <summary>
/// 逻辑子弹类
/// </summary>
public class Bullet : ObjectInf {
    public float BeginTime = 0;
    public string Mastername = "";
    public string Lockname = "";
    public SkillBulletType bu = new SkillBulletType();
    public Dictionary<string,int> HitMap = new Dictionary<string, int>();
    public bool ifEnd = false;

    public LogicTimer timer = new LogicTimer();

    public Bullet(string mn="", float bt=0, SkillBulletType b=null) {
        if(mn!=null) {
            Mastername = mn;
        }
        BeginTime = bt;

        if(b!=null) {
            bu = b;
        }
        //pos = p;
        //rot = r;
        var ob = Logic.obinfmp[Mastername];
        tag = ob.tag;
        pos = ob.pos;
        rot = ob.rot;
        dir = ob.dir;
        //rot.y = 15f*dir;
        //Debug.Log("bullet " + rot);
        id = Logic.GetNewID();
        ifEnd = false;
        if(b!=null) {
            if(bu.FlySpeed<=0) {
                //法术场类
                for(int i = 0;i < bu.JudgeTimes;i++) {
                    timer.Add((Logic.time+bu.EachAttackTime*i,delegate() {
                        AttackJudge();
                        AEJudge(1);
                    }));
                }
            }
            else {
                //飞行类
            }
        }
        /*if(bu.BulletFireEnemyPos) {
            Creature temp = null;
            foreach(var t in Logic.obinfmp) {
                var e = Logic.obinfmp[t.Key];
                if(e.curCreatureData.curHP<=0) {
                    continue;
                }
                if(e.tag==Logic.obinfmp[Mastername].tag) {
                    continue;
                }
                if(temp==null) {
                    temp = e;
                } else {
                    if(Dis2(pos,e.pos)<Dis2(pos,temp.pos)) {
                        Debug.Log("SUO DING " + e.name + " pos " + e.pos);
                        temp = e;
                    }
                }
            }
            if(temp!=null && Dis2(pos,temp.pos)<bu.MaxSearchDistance) {
                pos = temp.pos;
            }
        }*/
        //if(bu.IfLockEnemy) {
            Creature temp = null;
            foreach(var t in Logic.obinfmp) {
                var e = Logic.obinfmp[t.Key];
                if(e.curCreatureData.curHP<=0) {
                    continue;
                }
                if(e.tag==Logic.obinfmp[Mastername].tag) {
                    continue;
                }
                if(temp==null) {
                    temp = e;
                } else if(Dis2(pos,e.pos)<Dis2(pos,temp.pos)) {
                    temp = e;
                }
            }
            if(temp!=null && Dis2(pos,temp.pos)<bu.MaxSearchDistance && bu.IfLockEnemy) {
                Lockname = temp.name;
            } else {
                Lockname = "";
            }
            if(temp!=null && Dis2(pos,temp.pos)<bu.MaxSearchDistance && bu.BulletFireEnemyPos) {
                pos = temp.pos;
                Debug.Log("fire " + temp.name + " " + temp.pos);
            }
        //}
    }
    public void Update() {
        //if(bu.BulletFireEnemyPos) {

        //} else {
        if(Lockname!="") {
            //Debug.Log("锁定 " + Lockname);
            dir = GetDir(pos,Logic.obinfmp[Lockname].pos);
        } 
        Move(bu.FlySpeed);
        timer.Update();
        if(Logic.time-BeginTime>=bu.MaxDuringTime) {
            ifEnd = true;
        }

        //法术场的持续减速
        if(bu.CutSpeedRatio>0) {
            foreach(var t in Logic.obinfmp) {
                var e = Logic.obinfmp[t.Key];
                if(Vector3.Distance(new Vector3(pos.x,0,pos.z),new Vector3(e.pos.x,0,e.pos.z)) <= bu.Attack.SkillAttackJudgeAreaRadius+e.curCreatureData.Radius) {
                    //TODO:判断敌友
                    if(Logic.obinfmp[Mastername].tag==e.tag) {
                        continue;
                    }
                    e.BUFFtimer.Add((Logic.time+Logic.eachframtime,delegate() {
                        e.curCreatureData.Speed *= bu.CutSpeedRatio>1?1:(1-bu.CutSpeedRatio);
                    }));
                    e.BUFFtimer.Add((Logic.time+Logic.eachframtime*2,delegate() {
                        e.curCreatureData.Speed /= bu.CutSpeedRatio>1?1:(1-bu.CutSpeedRatio);
                    }));
                }
            }
        }

        //TOD:攻击判定
        if(bu.FlySpeed>0) {
            AttackJudge();
        }
        if(!bu.IfPenetrate&&HitMap.Count>0) {
            ifEnd = true;
        }
        AEJudge(2);
        //}
    }
    /// <summary>
    /// 攻击判断
    /// </summary>
    void AttackJudge() {
        if(!bu.IfAttackJudge) {
            return ;
        }
        foreach(var t in Logic.obinfmp) {
            var e = Logic.obinfmp[t.Key];
            //TODO:判断敌友
            if(Logic.obinfmp[Mastername].tag==e.tag || e.curCreatureData.curHP<=0) {
                continue;
            }
            if(Dis2(pos,e.pos)<=bu.Attack.SkillAttackJudgeAreaRadius+e.curCreatureData.Radius) {
                if(HitMap.ContainsKey(e.name)) {
                    if(bu.FlySpeed>0) {
                        continue;
                    } else {
                        HitMap[e.name]++;
                    }
                } else {
                    HitMap[e.name]=1;
                }
                e.ToBeAttacked(bu.Attack.SkillAttackJudgeDamageRatio*Logic.obinfmp[Mastername].curCreatureData.Attack,Mastername,bu.Attack.AAF, pos);
                if(!bu.IfRoleOnMultipleGoals) {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 属性效果判断
    /// </summary>
    /// <param name="dt">对哪种作用时间的属性进行判断</param>
    void AEJudge(int dt) {
        //发出该作用的人的tag
        string mastertag = Logic.obinfmp[Mastername].tag;

        foreach(var t in bu.AE) {
            //作用的持续时间
            float eachtime = dt==1?Logic.eachframtime:bu.EachAttackTime;
            var tt = t;
            if(t.DuringType==dt) {
                foreach(var g in Logic.obinfmp) {
                    var e = Logic.obinfmp[g.Key];
                    //距离不足
                    if(Dis2(pos,e.pos)>bu.Attack.SkillAttackJudgeAreaRadius+e.curCreatureData.Radius) {
                        continue;
                    }
                    if((t.RoleAttribute&1)>0) {
                        if(e.tag==mastertag&&t.RoleValue>0 || e.tag!=mastertag&&t.RoleValue<0) {
                            e.BUFFtimer.Add((Logic.time+Logic.eachframtime,delegate() {
                                Debug.Log("test test HP");
                                e.HPRecover(tt.RoleValue);
                            }));
                            if(t.IfRecycle) {
                                e.BUFFtimer.Add((Logic.time+Logic.eachframtime+eachtime,delegate() {
                                    e.HPRecover(-tt.RoleValue);
                                }));
                            }
                        }
                    }
                    if((t.RoleAttribute&2)>0) {
                        if(e.tag==mastertag&&t.RoleValue>0 || e.tag!=mastertag&&t.RoleValue<0) {
                            e.BUFFtimer.Add((Logic.time+Logic.eachframtime,delegate() {
                                e.MPRecover(tt.RoleValue);
                            }));
                            if(t.IfRecycle) {
                                e.BUFFtimer.Add((Logic.time+Logic.eachframtime+eachtime,delegate() {
                                    e.MPRecover(-tt.RoleValue);
                                }));
                            }
                        }
                    }

                    
                    if((t.RoleAttribute&4)>0) {
                        if(e.tag==mastertag&&t.RoleRatio>1 || e.tag!=mastertag&&t.RoleRatio<1) {
                            e.BUFFtimer.Add((Logic.time+Logic.eachframtime,delegate() {
                                e.SpeedChange(tt.RoleRatio);
                            }));
                            if(t.IfRecycle) {
                                e.BUFFtimer.Add((Logic.time+Logic.eachframtime+eachtime,delegate() {
                                    e.SpeedChange(-tt.RoleRatio);
                                }));
                            }
                        }
                    }
                    if((t.RoleAttribute&16)>0) {
                        if(e.tag==mastertag&&t.RoleRatio>1 || e.tag!=mastertag&&t.RoleRatio<1) {
                            e.BUFFtimer.Add((Logic.time+Logic.eachframtime,delegate() {
                                e.AttackChange(tt.RoleRatio);
                            }));
                            if(t.IfRecycle) {
                                e.BUFFtimer.Add((Logic.time+Logic.eachframtime+eachtime,delegate() {
                                    e.AttackChange(-tt.RoleRatio);
                                }));
                            }
                        }
                    }
                    if((t.RoleAttribute&8)>0) {
                        if(e.tag==mastertag&&t.RoleRatio>1 || e.tag!=mastertag&&t.RoleRatio<1) {
                            e.BUFFtimer.Add((Logic.time+Logic.eachframtime,delegate() {
                                e.DefenceChange(tt.RoleRatio);
                            }));
                            if(t.IfRecycle) {
                                e.BUFFtimer.Add((Logic.time+Logic.eachframtime+eachtime,delegate() {
                                    e.DefenceChange(-tt.RoleRatio);
                                }));
                            }
                        }
                    }
                }
            }
        }
    }
    public static float Dis2(Vector3 a, Vector3 b) {
        return Vector3.Distance(new Vector3(a.x,0,a.z), new Vector3(b.x,0,b.z));
    }

    public static int GetDir(Vector3 from, Vector3 to) {
        Vector3 direction = new Vector3(to.x, 0, to.z)- new Vector3(from.x, 0, from.z);
        var Direction = Quaternion.LookRotation(direction);
        var dir = (int)((Direction.eulerAngles.y+7.5f)/15.0f);
        return dir;
    }
}

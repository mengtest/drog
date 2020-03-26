using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象状态机
/// </summary>


public class FSM
{
    private Dictionary<string,IState> StateDict; //存储状态的字典
    public IState CurState = null;
    public IState DefaultState = null;
    public FSM() {
        StateDict = new Dictionary<string, IState>();
        CurState = null;
    }

    //当前状态的持续运行
    public void Update() {
        //Debug.Log("FSM UPDATE BEGIN");
        CurState.OnUpdate();
        //if(CurState.IfExit()) CurState = DefaultState;
    }

    //添加新的状态
    public void AddState(IState cState) {
        IState tState = null;

        if(!StateDict.TryGetValue(cState.GetStateName(),out tState)) {  //cState状态不存在
            StateDict[cState.GetStateName()] = cState;
        }
        else {                                             //cState状态存在
            Debug.Log(cState.GetStateName() + " 添加状态已经存在！");
        }
        //Debug.Log("FSM add " + cState.GetStateName() + " ok ");
        //ChangeState(cState.GetStateName());
    }

    //移出状态cState
    public void RemoveState(IState cState) {
        IState tState = null;

        if(!StateDict.TryGetValue(cState.GetStateName(),out tState)) {
            StateDict.Remove(cState.GetStateName());
        }
        else {
            //Debug.Log(cState.GetStateName() + "删除状态不存在！");
        }
    }

    //改变当前状态为cState
    public bool ChangeState(string cState) {
        IState tState = GetState(cState);
        if(tState==null)
            return false;
        if(CurState==null) {
            CurState = tState;
            CurState.OnEnter();
            return true;
        }
        if(tState.GetStateName()==CurState.GetStateName()) {
                 if(tState.GetStateName()=="Idle"      ) return true;
            else if(tState.GetStateName()=="Run"       ) return true;
            else if(tState.GetStateName()=="BeAttacked" || tState.GetStateName()=="Floating") {
                CurState.OnExit();
                CurState = tState;
                CurState.OnEnter();
                return true;
            }
            else if(tState.GetStateName()=="Dead"      ) return true;
            else return false;
        }
        if(!IfCanChang(CurState,StateDict[cState]))
            return false;

        CurState.OnExit();
        CurState = tState;
        CurState.OnEnter();
        return true;
    }

    public void RemoveAllState() {

    }
    IState GetState(string cState) {
        IState tState = null;
        if(!StateDict.TryGetValue(cState,out tState)) return null;
        return StateDict[cState];
    }

    public virtual bool IfCanChang(IState StateNameA, IState StateNameB) {
        //当前默认不能转化为被攻击态
        //if(StateNameB.GetStateName()=="BeAttacked")
        //    return false;
        //奔跑态和站立态可以转化为任意态//
        if(StateNameA.GetStateName()=="Run"||StateNameA.GetStateName()=="Idle")
            return true;
        //死亡态不能转化为其他态
        if(StateNameA.GetStateName()=="Dead")
            return false;
        //必定能够转化为死亡态
        if(StateNameB.GetStateName()=="Dead")
            return true;
        //必定能够转化为击飞态
        if(StateNameB.GetStateName()=="Floating")
            return true;
        
        if(StateNameA.GetStateName()==StateNameB.GetStateName()&&StateNameA.GetStateName()=="BeAttacked")
            return true;
        //闪避不会被打断
        if(StateNameA.GetStateName()=="Dodge"&&!StateNameA.IfCanExitTo())
            return false;

        //前摇会被攻击打断
        if(!StateNameA.IfRollEnd() && StateNameB.GetStateName()=="BeAttacked")
            return true;
        
        //后摇可以用移动取消
        if(StateNameA.IfShakeBegin() && (StateNameB.GetStateName()=="Run" || StateNameB.GetStateName()=="Dodge"))
            return true;
        if(StateNameA.IfShakeBegin() && StateNameB.GetStateName() != "BeAttacked" 
                                     && StateNameB.GetStateName() != "Idle")
            return true;
        if(StateNameA.IfCanExitTo())
            return true;
        else 
            return false;
    }
}


/// <summary>
/// 人物的角色状态类
/// </summary>
public class IState {
    /// <summary>
    /// 状态数据
    /// </summary>
    SkillData StateData;
    /// <summary>
    /// 进入状态的时间
    /// </summary>
    float StateEnterTime;
    /// <summary>
    /// 状态的归属人物的名称
    /// </summary>
    string StateMasterName = "";
    /// <summary>
    /// 会跟随状态结束而销毁的特效在View层的ID
    /// </summary>
    List<int> FollowStateDestroySEID;
    /// <summary>
    /// 状态用定时器
    /// </summary>
    /// <returns></returns>
    LogicTimer timer = new LogicTimer();
    float UPv = 0;

    /// <summary>
    /// 新建状态且初始化
    /// </summary>
    /// <param name="newSkillData">状态数据</param>
    /// <param name="newname">状态归属人的名称</param>
    public IState(SkillData newStateData=null, string newStateMasterName="") {
        StateData = newStateData;
        StateMasterName = newStateMasterName;
        StateEnterTime = -1000;   //进入该状态的时间初始化为-1000s
        FollowStateDestroySEID = new List<int>();
    }

    /// <summary>
    /// 进入该状态需要做的操作
    /// </summary>
    public void OnEnter() {
        Debug.Log("master name " + StateMasterName + "   " + "state name " + StateData.StateName + " !!!!!!!!!!!!!!!!!!!");
        timer.Clear();
        //播放动画
        //gameObject.GetComponent<Animator>().CrossFade(StateData.SkillAnimationName,0.1f);

        //状态进入需要让视图层做的操作包成事件交给试图层的事件系统
        //特效绑定动画？
        //每一个状态在view层都有对应的资源管理器？

        //特效：
        //是否绑定在某一GameObject下
        //是否定时销毁
        //是否跟随动画销毁

        //特性：
        //产生点
        //绑定人物对应点
        //随动画停止销毁=逻辑层的状态驱动
        //定时销毁=逻辑层的时间驱动

        foreach(var t in StateData.Floats) {
            SkillFloatType e = t;
            timer.Add((Logic.time+e.BeginTime,delegate() {
                var ob = Logic.obinfmp[StateMasterName];
                //Debug.Log("e.UPv " + e.UPv);
                if(ob.UPv <= 0) {
                    ob.UPv += e.UPv;
                } else {
                    ob.UPv += e.UPv*0.2f;
                }
            }));
        }

        //特效事件的发出
        foreach(var t in StateData.SkillSEList) {
            SkillSE e = new SkillSE();
            e = t;
            int curID = Logic.GetNewID();
            //FollowStateDestroySEID.Add(curID);
            //特效生成事件加入View层的timer中
            timer.Add((Logic.time+e.SkillSEBeginTime,delegate() {
                View.timer.Add((Logic.time,delegate() {
                    View.CreatePlayerEffect(curID,e.SEName,StateMasterName,e.SELoadPointName,e.IfFollowLoadPoint);
                }));
                if(e.IfFollowAnimationDestroy) {
                    //如果特效随动画销毁，就加入状态的特效控制表中
                    FollowStateDestroySEID.Add(curID);
                } else {
                    //如果特效随时间销毁，就将销毁事件加入视图层的timer中
                    //Debug.Log("remove event " + curID);
                    View.timer.Add((e.SkillSEDuringTime+Logic.time,delegate() {
                        View.RemovePlayerEffect(curID);
                    }));
                }
            }));
        }

        //将动画播放事件加入View层中的timer中
        View.timer.Add((Logic.time,delegate() {
            /*if(StateData.AnimationName=="W_BeAttacked") {
                Debug.Log("BeAttcked!!!!!!!!");
            }*/
            //View.obmp[StateMasterName].GetComponent<Animator>().CrossFade(StateData.AnimationName,0.1f);
            //View.obmp[StateMasterName].GetComponent<Animator>();
            View.obmp[StateMasterName].GetComponent<Animator>().Play(StateData.AnimationName,0,0f);
        }));
        //View.EventHandle += delegate() {
        //    View.obmp[StateMasterName].GetComponent<Animator>().CrossFade(StateData.AnimationName,0.1f);
        //};

        //进入状态的时间更新
        StateEnterTime = Logic.time;
    }
    /// <summary>
    /// 状态更新时的操作
    /// </summary>
    public void OnUpdate() {
        //Debug.Log("state update");
        timer.Update();
    }
    /// <summary>
    /// 状态退出时需要做的操作
    /// </summary>
    public void OnExit() {
        //退出时发出销毁特效的事件
        foreach(var t in FollowStateDestroySEID) {
            int e = new int();
            e = t;
            View.timer.Add((Logic.time,delegate() {
                View.RemovePlayerEffect(e);
            }));
        }
        timer.Clear();
    }
 
    /// <summary>
    /// 判断当前状态的生命周期是否已经结束
    /// </summary>
    /// <returns>当前状态生命周期是否结束</returns>
    public bool IfCanExitTo() {
        if(GetStateName()=="Floating") {
            if(Logic.obinfmp[StateMasterName].pos.y>0) {
                return false;
            } else {
                return true;
            }
        }
        if(Logic.time-StateEnterTime>StateData.DuringTime)
            return true;
        else
            return false;
    }
    /// <summary>
    /// 判断状态前摇是否结束
    /// </summary>
    /// <returns>前摇是否结束</returns>
    public bool IfRollEnd() {
        if(Logic.time-StateEnterTime < StateData.SkillRollEndTime) {
            return false;
        } else { 
            return true;
        }
    }
    /// <summary>
    /// 判断状态的后摇是否开始
    /// </summary>
    /// <returns>后摇是否开始</returns>
    public bool IfShakeBegin() {
        if(Logic.time-StateEnterTime>StateData.ShakeBackBeginTime) {
            return true;
        } else {
            return false;
        }
    }
    /// <summary>
    /// 获取当前状态的名称
    /// </summary>
    /// <returns>当前状态的名称</returns>
    public string GetStateName() {
        return StateData.StateName;
    }
    public SkillData GetState() {
        return StateData;
    }
}


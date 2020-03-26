using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class PlayerFSM : FSM
{
    public static string IDLE = "IdleS";
    public static string RUN = "RunS";
    public static string SKILLA = "Skill_A_S";
    public static string SKILL1 = "Skill_1_S";
    public static string SKILL2 = "Skill_2_S";
    public static string SKILL3 = "Skill_3_S";
    public static string SKILLD = "Skill_dodge_S";
    public override bool IfCanChang(string StateNameA, string StateNameB) {
        Debug.Log("IfCanChang " + StateNameA + "," + StateNameB + "," + SKILL1 + "," + RUN);
        if(StateNameA==SKILLA || StateNameA==SKILL1 || StateNameA==SKILL2 || StateNameA==SKILL3 || StateNameA==SKILLD) return false;
        if(StateNameA==SKILL1&&StateNameB==RUN || StateNameA==SKILL1&&StateNameB==IDLE) {
            Debug.Log("FALSE CHANGE " + StateNameA + "," + StateNameB);
            return false;
        }
        else return true;
    }
}

public class IdleS:IState {
    public bool ifExit;
    private Animator ani;
    private GameObject gameobject;
    public void Init(GameObject Newgameobject, SkillData temp) { 
        gameobject = Newgameobject;
        ani = gameobject.GetComponent<Animator> (); 
    }
    public void OnEnter() {
        ifExit = false;
        //ani.Play("")
        ani.SetBool("IsRun",false);
        ani.SetTrigger("Idle");
    }
    public void OnUpdate() { }
    public void OnExit() { 
        ifExit = true;
    }
    public bool IfExit() { return ifExit; }
    public string GetStateName() { return "IdleS"; }
}

public class RunS:IState {
    public bool ifExit;
    private Animator ani;
    private GameObject gameobject;
    public void Init(GameObject Newgameobject, SkillData temp) { 
        gameobject = Newgameobject;
        ani = gameobject.GetComponent<Animator> (); 
    }
    public void OnEnter() { 
        ifExit = false;
        ani.SetBool("IsRun",true); 
    }
    public void OnUpdate() { }
    public void OnExit() { 
        ani.SetBool("IsRun",false);
        ifExit = true; 
    }
    public bool IfExit() { return ifExit; }
    public string GetStateName() { return "RunS"; }
}

public class Skill_A_S:IState {
    public bool ifExit;            //判断当前状态是否退出
    private GameObject gameobject; //玩家对象
    private Animator ani;          //动画控制器
    private SkillData curSkillData; //当前技能数据
    private List<GameObject> SkillSEList; //技能特效对象表
    List<bool> IfSEUse;
    public void Init(GameObject Newgameobject, SkillData NewSkillData) { 
        //Debug.Log("Skill1 init begin");
        gameobject = Newgameobject; 
        ani = gameobject.GetComponent<Animator> ();
        curSkillData = NewSkillData; 
        SkillSEList = new List<GameObject> ();
        IfSEUse = new List<bool> ();
        //SkillSEPosList = new List<GameObject> ();
        //Debug.Log("GameObject load begin");
        GameObject temp0 = null, temp1 = null;
        foreach (var SEData in curSkillData.SkillSEDates) {
            temp0 = Resources.Load(SEData.SkillSEName) as GameObject;
            temp1 = GameObject.Find(SEData.PosName) as GameObject;
            temp0 = GameObject.Instantiate (temp0, temp1.transform.position, temp1.transform.rotation);
            if(SEData.IfFollow) temp0.transform.parent = temp1.transform;
            temp0.SetActive(false); //默认关闭特效
            SkillSEList.Add(temp0);
            IfSEUse.Add(false);
            //Debug.Log(SEData.SkillSEName + " load ok");
        }
    }
    public void OnEnter() { 
        ani.SetTrigger("SkillA"); 
        ifExit = false;
        for(int i = 0;i < IfSEUse.Count;i++) IfSEUse[i] = true; //还原技能特效使用情况
        for(int i = 0;i < IfSEUse.Count;i++) {
            if(!curSkillData.SkillSEDates[i].IfFollow) { //特效不跟随技能点，重置当前位置
                SkillSEList[i].transform.position = 
                    new Vector3(gameobject.transform.position.x,
                        gameobject.transform.position.y,
                            gameobject.transform.position.z);
            }
            float temp = curSkillData.SkillSEDates[i].BeginTime;
            MyTimer.DelayToInvokeDo(temp, 0, 1, delegate() {
                for(int j = 0;j < IfSEUse.Count;j++) {
                    if(IfSEUse[j]) {
                        SkillSEList[j].SetActive(true);
                        IfSEUse[j] = false;
                        break;
                    }
                }
            });
        }
        MyTimer.DelayToInvokeDo(curSkillData.SkillDuringTime, 0, 1, delegate() {
            OnExit();
        });
    }
    public void OnUpdate() { 

    }
    public void OnExit() { 
        ani.SetBool("IsRun",false); 
        for(int i = 0;i < IfSEUse.Count;i++) SkillSEList[i].SetActive(false);
        ifExit = true;
    }
    public bool IfExit() {
        return ifExit;
    }
    public string GetStateName() { return "Skill_A_S"; }
}
public class Skill_1_S:IState {
    public bool ifExit;            //判断当前状态是否退出
    private GameObject gameobject; //玩家对象
    private Animator ani;          //动画控制器
    private SkillData curSkillData; //当前技能数据
    private List<GameObject> SkillSEList; //技能特效对象表
    List<bool> IfSEUse;
    public void Init(GameObject Newgameobject, SkillData NewSkillData) { 
        //Debug.Log("Skill1 init begin");
        gameobject = Newgameobject; 
        ani = gameobject.GetComponent<Animator> ();
        curSkillData = NewSkillData; 
        SkillSEList = new List<GameObject> ();
        IfSEUse = new List<bool> ();
        //SkillSEPosList = new List<GameObject> ();
        //Debug.Log("GameObject load begin");
        GameObject temp0 = null, temp1 = null;
        foreach (var SEData in curSkillData.SkillSEDates) {
            temp0 = Resources.Load(SEData.SkillSEName) as GameObject;
            temp1 = GameObject.Find(SEData.PosName) as GameObject;
            temp0 = GameObject.Instantiate (temp0, temp1.transform.position, temp1.transform.rotation);
            if(SEData.IfFollow) temp0.transform.parent = temp1.transform;
            temp0.SetActive(false); //默认关闭特效
            SkillSEList.Add(temp0);
            IfSEUse.Add(false);
            //Debug.Log(SEData.SkillSEName + " load ok");
        }
    }
    public void OnEnter() { 
        ani.SetTrigger("Skill1"); 
        ifExit = false;
        for(int i = 0;i < IfSEUse.Count;i++) IfSEUse[i] = true; //还原技能特效使用情况
        for(int i = 0;i < IfSEUse.Count;i++) {
            if(!curSkillData.SkillSEDates[i].IfFollow) { //特效不跟随技能点，重置当前位置
                SkillSEList[i].transform.position = 
                    new Vector3(gameobject.transform.position.x,
                        gameobject.transform.position.y,
                            gameobject.transform.position.z);
            }
            float temp = curSkillData.SkillSEDates[i].BeginTime;
            MyTimer.DelayToInvokeDo(temp, 0, 1, delegate() {
                for(int j = 0;j < IfSEUse.Count;j++) {
                    if(IfSEUse[j]) {
                        SkillSEList[j].SetActive(true);
                        IfSEUse[j] = false;
                        break;
                    }
                }
            });
        }
        MyTimer.DelayToInvokeDo(curSkillData.SkillDuringTime, 0, 1, delegate() {
            OnExit();
        });
    }
    public void OnUpdate() { 

    }
    public void OnExit() { 
        ani.SetBool("IsRun",false); 
        for(int i = 0;i < IfSEUse.Count;i++) SkillSEList[i].SetActive(false);
        ifExit = true;
    }
    public bool IfExit() {
        return ifExit;
    }
    public string GetStateName() { return "Skill_1_S"; }
}
public class Skill_2_S:IState {
    public bool ifExit;            //判断当前状态是否退出
    private GameObject gameobject; //玩家对象
    private Animator ani;          //动画控制器
    private SkillData curSkillData; //当前技能数据
    private List<GameObject> SkillSEList; //技能特效对象表
    List<bool> IfSEUse;
    public void Init(GameObject Newgameobject, SkillData NewSkillData) { 
        //Debug.Log("Skill1 init begin");
        gameobject = Newgameobject; 
        ani = gameobject.GetComponent<Animator> ();
        curSkillData = NewSkillData; 
        SkillSEList = new List<GameObject> ();
        IfSEUse = new List<bool> ();
        //SkillSEPosList = new List<GameObject> ();
        //Debug.Log("GameObject load begin");
        GameObject temp0 = null, temp1 = null;
        foreach (var SEData in curSkillData.SkillSEDates) {
            temp0 = Resources.Load(SEData.SkillSEName) as GameObject;
            temp1 = GameObject.Find(SEData.PosName) as GameObject;
            temp0 = GameObject.Instantiate (temp0, temp1.transform.position, temp1.transform.rotation);
            if(SEData.IfFollow) temp0.transform.parent = temp1.transform;
            temp0.SetActive(false); //默认关闭特效
            SkillSEList.Add(temp0);
            IfSEUse.Add(false);
            //Debug.Log(SEData.SkillSEName + " load ok");
        }
    }
    public void OnEnter() { 
        ani.SetTrigger("Skill2"); 
        ifExit = false;
        for(int i = 0;i < IfSEUse.Count;i++) IfSEUse[i] = true; //还原技能特效使用情况
        for(int i = 0;i < IfSEUse.Count;i++) {
            if(!curSkillData.SkillSEDates[i].IfFollow) { //特效不跟随技能点，重置当前位置
                SkillSEList[i].transform.position = 
                    new Vector3(gameobject.transform.position.x,
                        gameobject.transform.position.y,
                            gameobject.transform.position.z);
            }
            float temp = curSkillData.SkillSEDates[i].BeginTime;
            MyTimer.DelayToInvokeDo(temp, 0, 1, delegate() {
                for(int j = 0;j < IfSEUse.Count;j++) {
                    if(IfSEUse[j]) {
                        SkillSEList[j].SetActive(true);
                        IfSEUse[j] = false;
                        break;
                    }
                }
            });
        }
        MyTimer.DelayToInvokeDo(curSkillData.SkillDuringTime, 0, 1, delegate() {
            OnExit();
        });
    }
    public void OnUpdate() { 

    }
    public void OnExit() { 
        ani.SetBool("IsRun",false); 
        for(int i = 0;i < IfSEUse.Count;i++) SkillSEList[i].SetActive(false);
        ifExit = true;
    }
    public bool IfExit() {
        return ifExit;
    }
    public string GetStateName() { return "Skill_2_S"; }
}
public class Skill_3_S:IState {
    public bool ifExit;            //判断当前状态是否退出
    private GameObject gameobject; //玩家对象
    private Animator ani;          //动画控制器
    private SkillData curSkillData; //当前技能数据
    private List<GameObject> SkillSEList; //技能特效对象表
    List<bool> IfSEUse;
    public void Init(GameObject Newgameobject, SkillData NewSkillData) { 
        //Debug.Log("Skill1 init begin");
        gameobject = Newgameobject; 
        ani = gameobject.GetComponent<Animator> ();
        curSkillData = NewSkillData; 
        SkillSEList = new List<GameObject> ();
        IfSEUse = new List<bool> ();
        //SkillSEPosList = new List<GameObject> ();
        //Debug.Log("GameObject load begin");
        GameObject temp0 = null, temp1 = null;
        foreach (var SEData in curSkillData.SkillSEDates) {
            temp0 = Resources.Load(SEData.SkillSEName) as GameObject;
            temp1 = GameObject.Find(SEData.PosName) as GameObject;
            temp0 = GameObject.Instantiate (temp0, temp1.transform.position, temp1.transform.rotation);
            if(SEData.IfFollow) temp0.transform.parent = temp1.transform;
            temp0.SetActive(false); //默认关闭特效
            SkillSEList.Add(temp0);
            IfSEUse.Add(false);
            //Debug.Log(SEData.SkillSEName + " load ok");
        }
    }
    public void OnEnter() { 
        for(int i = 0;i < IfSEUse.Count;i++) SkillSEList[i].SetActive(false);
        ani.SetTrigger("Skill3"); 
        ifExit = false;
        for(int i = 0;i < IfSEUse.Count;i++) IfSEUse[i] = true; //还原技能特效使用情况
        for(int i = 0;i < IfSEUse.Count;i++) {
            if(!curSkillData.SkillSEDates[i].IfFollow) { //特效不跟随技能点，重置当前位置
                SkillSEList[i].transform.position = 
                    new Vector3(gameobject.transform.position.x,
                        gameobject.transform.position.y,
                            gameobject.transform.position.z);
            }
            float temp = curSkillData.SkillSEDates[i].BeginTime;
            MyTimer.DelayToInvokeDo(temp, 0, 1, delegate() {
                for(int j = 0;j < IfSEUse.Count;j++) {
                    if(IfSEUse[j]) {
                        Debug.Log(SkillSEList[j].name + " SetActive true  n=" + SkillSEList[j].activeInHierarchy);
                        //Debug.Log(SkillSEList[j].activeInHierarchy)
                        SkillSEList[j].SetActive(true);
                        Debug.Log(SkillSEList[j].name + " to = " + SkillSEList[j].activeInHierarchy);
                        IfSEUse[j] = false;
                        break;
                    }
                }
            });
        }
        MyTimer.DelayToInvokeDo(curSkillData.SkillSEDates[1].BeginTime+curSkillData.SkillSEDates[1].DuringTime, 0, 1, delegate() {
            SkillSEList[1].SetActive(false);            
        });
        MyTimer.DelayToInvokeDo(curSkillData.SkillDuringTime, 0, 1, delegate() {
            OnExit();
        });
    }
    public void OnUpdate() { 

    }
    public void OnExit() { 
        ani.SetBool("IsRun",false); 
        //for(int i = 0;i < IfSEUse.Count;i++) SkillSEList[i].SetActive(false);
        ifExit = true;
    }
    public bool IfExit() {
        return ifExit;
    }
    public string GetStateName() { return "Skill_3_S"; }
}
public class Skill_dodge_S:IState {
    public bool ifExit;            //判断当前状态是否退出
    private GameObject gameobject; //玩家对象
    private Animator ani;          //动画控制器
    private SkillData curSkillData; //当前技能数据
    private List<GameObject> SkillSEList; //技能特效对象表
    List<bool> IfSEUse;
    public void Init(GameObject Newgameobject, SkillData NewSkillData) { 
        //Debug.Log("Skill1 init begin");
        gameobject = Newgameobject; 
        ani = gameobject.GetComponent<Animator> ();
        curSkillData = NewSkillData; 
        SkillSEList = new List<GameObject> ();
        IfSEUse = new List<bool> ();
        //SkillSEPosList = new List<GameObject> ();
        //Debug.Log("GameObject load begin");
        GameObject temp0 = null, temp1 = null;
        foreach (var SEData in curSkillData.SkillSEDates) {
            temp0 = Resources.Load(SEData.SkillSEName) as GameObject;
            temp1 = GameObject.Find(SEData.PosName) as GameObject;
            temp0 = GameObject.Instantiate (temp0, temp1.transform.position, temp1.transform.rotation);
            if(SEData.IfFollow) temp0.transform.parent = temp1.transform;
            temp0.SetActive(false); //默认关闭特效
            SkillSEList.Add(temp0);
            IfSEUse.Add(false);
            //Debug.Log(SEData.SkillSEName + " load ok");
        }
    }
    public void OnEnter() { 
        ani.SetTrigger("SkillD"); 
        ifExit = false;
        for(int i = 0;i < IfSEUse.Count;i++) IfSEUse[i] = true; //还原技能特效使用情况
        for(int i = 0;i < IfSEUse.Count;i++) {
            if(!curSkillData.SkillSEDates[i].IfFollow) { //特效不跟随技能点，重置当前位置
                SkillSEList[i].transform.position = 
                    new Vector3(gameobject.transform.position.x,
                        gameobject.transform.position.y,
                            gameobject.transform.position.z);
            }
            float temp = curSkillData.SkillSEDates[i].BeginTime;
            MyTimer.DelayToInvokeDo(temp, 0, 1, delegate() {
                for(int j = 0;j < IfSEUse.Count;j++) {
                    if(IfSEUse[j]) {
                        SkillSEList[j].SetActive(true);
                        IfSEUse[j] = false;
                        break;
                    }
                }
            });
        }
        MyTimer.DelayToInvokeDo(curSkillData.SkillDuringTime, 0, 1, delegate() {
            OnExit();
        });
    }
    public void OnUpdate() { 

    }
    public void OnExit() { 
        ani.SetBool("IsRun",false); 
        for(int i = 0;i < IfSEUse.Count;i++) SkillSEList[i].SetActive(false);
        ifExit = true;
    }
    public bool IfExit() {
        return ifExit;
    }
    public string GetStateName() { return "Skill_dodge_S"; }
}*/
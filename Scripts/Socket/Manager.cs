using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel.Design;
using System.IO;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 逻辑对象数据
/// </summary>
public class ObjectInf {
    /// <summary>
    /// 逻辑对象id
    /// </summary>
    public int id = 0;
    /// <summary>
    /// 逻辑对象名字
    /// </summary>
    public string name = "";
    /// <summary>
    /// 逻辑对象标签属性
    /// </summary>
    public string tag = "";
    /// <summary>
    /// 逻辑对象全局位置
    /// </summary>
    public Vector3 pos = new Vector3();
    /// <summary>
    /// 对象半径
    /// </summary>
    public float Radius = 0;
    /// <summary>
    /// 逻辑对象全局旋转方向
    /// </summary>
    public Vector3 rot = new Vector3();
    public int dir = 0;
    /// <summary>
    /// y轴速度
    /// </summary>
    public float UPv = 0;
    /// <summary>
    /// 重力加速度
    /// </summary>
    public float G = 7.5f;
    public void Move(float speed) {
        if(speed==0) {
            return ;
        }
        float angle = 15.0f * dir;
        float f = (float)(angle*Math.PI/180.0f);
        Vector3 move = (new Vector3((float)Math.Sin(f),0,(float)Math.Cos(f)))*((float)(speed*Logic.eachframtime));
        bool ok = true;
        foreach(var t in Logic.obinfmp) {
            var e = Logic.obinfmp[t.Key];
            if(e.curCreatureData.curHP<=0) {
                continue;
            }
            if(Dis2(pos,e.pos)>Dis2(pos+move,e.pos)&&Dis2(pos+move,e.pos)<e.curCreatureData.Radius+Radius) {
                ok = false;
            }
            if(!ok) {
                break;
            }
        }
        if(!JudgeNewPosInScenceOK(pos+move)) {
            ok = false;
        }
        if(ok) {
            pos += move;
        }
        rot = new Vector3(0,15.0f*dir,0);
    }
    float Dis2(Vector3 a, Vector3 b) {
        return Vector3.Distance(new Vector3(a.x,0,a.z), new Vector3(b.x,0,b.z));
    }
    public void MoveContrast(float distance) {
        float angle = 15.0f * dir;
        float f = (float)(angle*Math.PI/180.0f);
        Vector3 move = (new Vector3((float)Math.Sin(f),0,(float)Math.Cos(f)))*((float)(distance));
        bool ok = true;
        foreach(var t in Logic.obinfmp) {
            var e = Logic.obinfmp[t.Key];
            if(e.curCreatureData.curHP<=0) {
                continue;
            }
            if(Dis2(pos,e.pos)>Dis2(pos-move,e.pos)&&Dis2(pos-move,e.pos)<e.curCreatureData.Radius+Radius) {
                ok = false;
            }
            if(!ok) {
                break;
            }
        }
        if(!JudgeNewPosInScenceOK(pos-move)) {
            ok = false;
        }
        if(ok) {
            pos -= move;
        }
        //pos -= move;
        //rot = new Vector3(0,15.0f*dir,0);
    }
    /// <summary>
    /// 判断该位置是否能满足场景要求
    /// </summary>
    /// <param name="npos"></param>
    /// <returns></returns>
    public bool JudgeNewPosInScenceOK(Vector3 npos) {
        //43.5    150.3
        float r = 21;
        Vector3 cir = new Vector3(43.5f,0,150.3f);
        if(Dis2(cir, npos) > r) {
            return false;
        } else {
            return true;
        }
    }
}
public class Manager : MonoBehaviour
{
    
    void Start()
    {
        //资源加载
        RSDB.Init();
        //逻辑层加载
        Logic.Init();
        //视图层加载
        View.Init();
    }

    float pret = 0;
    // Update is called once per frame
    void Update()
    {
        //ListenKey();
        if(Client.dataQueue.Count < 10) {
            Logic.Update();
        }
        else if(Client.dataQueue.Count < 30) {
            for(int i = 0;i < 3;i++) Logic.Update();
        }
        else if(Client.dataQueue.Count < 60) {
            for(int i = 0;i < 10;i++) Logic.Update(); 
        }
        else {
            for(int i = 0;i < 50;i++) Logic.Update();
        }
        View.Update();
    }


    void ListenKey() {
        
        FramOpt opt = new FramOpt();
        opt.Doplayername = Client.name;
        bool ok = false;
        if(Input.GetKey(KeyCode.UpArrow)) {
            opt.Optype = Client.MOVETYPE;
            opt.Dir = 0;
            Client.Send(Client.FRAMOPTYPE,opt);
            ok = true;
            pret = Time.time;
        }
        if(Input.GetKey(KeyCode.RightArrow)) {
            opt.Optype = Client.MOVETYPE;
            opt.Dir = 6;
            Client.Send(Client.FRAMOPTYPE,opt);
            ok = true;
            pret = Time.time;
        }
        if(Input.GetKey(KeyCode.DownArrow)) {
            opt.Optype = Client.MOVETYPE;
            opt.Dir = 12;
            Client.Send(Client.FRAMOPTYPE,opt);
            ok = true;
            pret = Time.time;
        }
        if(Input.GetKey(KeyCode.LeftArrow)) {
            opt.Optype = Client.MOVETYPE;
            opt.Dir = 18;
            Client.Send(Client.FRAMOPTYPE,opt);
            ok = true;
            pret = Time.time;
        }
        if(Input.GetKeyDown("x")||Input.GetKey("x")) {
            opt.Optype = Client.SKILLTYPE;
            opt.Skillid = 4;
            Client.Send(Client.FRAMOPTYPE,opt);
            pret = Time.time;
            ok = true;
        }
        if(Input.GetKeyDown("c")||Input.GetKey("c")) {
            opt.Optype = Client.SKILLTYPE;
            opt.Skillid = 5;
            Client.Send(Client.FRAMOPTYPE,opt);
            pret = Time.time;
            ok = true;
        }
        if(!ok && Time.time - pret >= 0.1f){
            opt.Optype = Client.SKILLTYPE;
            opt.Skillid = 0;
            Client.Send(Client.FRAMOPTYPE,opt);
            pret = Time.time+2f;
        }
    }
    void OnDestroy() {
        Client.ifrun = false;    
    }
}

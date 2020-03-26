using System.Net;
using System.Linq.Expressions;
using System;
using System.Runtime.ExceptionServices;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic
{
    /// <summary>
    /// 逻辑帧每一帧的时间 单位ms
    /// </summary>
    public const float eachframtime = 0.02f;

    public static List<Vector3> MoveVector3;

    /// <summary>
    /// 逻辑模块当前时间 所有的逻辑计时都要调用这个时间  单位s
    /// </summary>
    public static float time;

    /// <summary>
    /// 游戏中所有对象的信息（目前指可移动生物对象）
    /// </summary>
    public static Dictionary<string,Creature> obinfmp;

    /// <summary>
    /// 逻辑帧开始前的初始化
    /// </summary>
    public static void Init() {
        //初始化逻辑时间
        time = 0;
        //初始化对象信息字典
        obinfmp = new Dictionary<string, Creature>();
        //
        MoveVector3 = new List<Vector3>();                       
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public static void Update()
    {
        if(Client.dataQueue.Count > 0) {
            var data = Client.dataQueue.Dequeue();
            if(data.Item1 != Client.FRAMPACKAGETYPE) {
                Debug.Log("fram package error");
                Client.ifrun = false;
                return ;
            }

            var fp = Client.Deserialize<FramPackage>(data.Item2,0,data.Item2.Length);
            for(int i = 0;i < fp.Opts.Count;i++) {
                var opt = fp.Opts[i];
                if(!obinfmp.ContainsKey(opt.Doplayername)) {
                    LogicCreatPlayer(opt.Doplayername,opt.Id);
                    if(opt.Doplayername==Client.name) {
                        UIManager.Init();
                    }
                }
                if(opt.Optype==Client.MOVETYPE) {
                    obinfmp[opt.Doplayername].ToMove(opt.Dir);
                    //ToMove(opt.Doplayername,opt.Dir);
                }
                else {
                    obinfmp[opt.Doplayername].ToUseSkill(opt.Skillid);
                    //ToPlaySkill(opt.Doplayername,opt.Skillid);
                }
            }

            //逻辑时间更新
            time += eachframtime;
        }
        
        foreach(var e in obinfmp) {
            e.Value.UpdateCreature();
        }
        BulletsManager.Update();
    }

    /// <summary>
    /// 逻辑层创建未添加的角色
    /// </summary>
    /// <param name="name"></param>
    public static void LogicCreatPlayer(string name, int id) {

        var tempobject = new Creature();
        tempobject.name = name;
        tempobject.id = id;
        //Debug.Log("player name " + name);
        tempobject.tag = "player"+id;
        tempobject.pos = new Vector3(0,0,0);
        tempobject.rot = new Vector3(0,0,0);
        tempobject.InitLivinngCreature();
        //PlayerMp[name] = Instantiate(MyResources[PlayerResourcesName[i]],tempobject.pos,tempobject.rot);
        obinfmp[name] = tempobject;

    }

    /// <summary>
    /// 对对象进行移动操作
    /// </summary>
    /// <param name="name">对象名</param>
    /// <param name="dir">移动方向</param>
    public static void ToMove(string name, int dir) {
        float Speed = 2;
        var be = new Vector3(0,0,1);
        float angle = 15.0f * dir;
        float f = (float)(angle*Math.PI/180.0f);
        obinfmp[name].pos += (new Vector3((float)Math.Sin(f),0,(float)Math.Cos(f)))*((float)(Speed*eachframtime));
        obinfmp[name].rot = new Vector3(0,15.0f*dir,0);
    }

    /*
    /// <summary>
    /// 对对象下达施放技能指令
    /// </summary>
    /// <param name="name">对象名</param>
    /// <param name="skillid">技能在生物技能表中id</param>
    public static void ToPlaySkill(string name, int skillid) {

    }*/



    private static int LogicID = 0;
    /// <summary>
    /// 分配ID
    /// </summary>
    /// <returns></returns>
    public static int GetNewID() {
        return ++LogicID;
    }
}

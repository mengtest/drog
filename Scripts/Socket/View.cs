//using System.Numerics;
using System.Linq;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View
{
    /// <summary>
    /// 预先加载的资源库
    /// </summary>
    public static Dictionary<string,GameObject> MyResources = new Dictionary<string, GameObject>();
    /// <summary>
    /// 人物加载对应的资源名字
    /// </summary>
    public static List<string> PlayerResourcesName = new List<string>();
    /// <summary>
    /// 视图层对象映射:名称标识->GameOject实例
    /// </summary>
    public static Dictionary<string,GameObject> obmp = new Dictionary<string, GameObject>();
    /// <summary>
    /// 所有特效
    /// </summary>
    public static Dictionary<int,GameObject> effects = new Dictionary<int, GameObject>();
    public static Dictionary<int,GameObject> bullets = new Dictionary<int, GameObject>();

    //public static SortedDictionary<float,Action> timer = new SortedDictionary<float, Action>();
    public static LogicTimer timer = new LogicTimer();
    

    public delegate void EventMsg();
    public static event EventMsg EventHandle;

    public static void Init() {

        var obinfmp = Logic.obinfmp;
        //资源加载
        MyResources =  RSDB.DB;

        PlayerResourcesName.Add("blackS");
        PlayerResourcesName.Add("blueS");
        PlayerResourcesName.Add("yellowS");
        PlayerResourcesName.Add("redS");
        PlayerResourcesName.Add("PlayerW");

    }
    public static void Update() {
        /*if(EventHandle != null) {
            EventHandle();
            EventHandle -= EventHandle;
        }*/
        UIManager.Update();
        //Debug.Log("user counts " + obmp.Count);
        var obinfmp = Logic.obinfmp;
        var needDes = new List<string>();
        foreach(var e in obinfmp) {
            var name = e.Key;
            var obinf = e.Value;
            //用户不存在就新建
            if(!obmp.ContainsKey(name)) {
                Debug.Log("new palyer name " + name);
                obmp[name] = GameObject.Instantiate(MyResources[obinf.curCreatureData.Name],obinf.pos,Quaternion.identity);
                //obmp[name].transform.Find("body").GetComponent<SkinnedMeshRenderer>().material = RSDB.mat[obinf.id-1];
                obmp[name].name = name;

                //新建玩家为本机玩家，绑定相机
                if(Client.name==name) {
                    var camera = GameObject.Find("Main Camera");
                    camera.GetComponent<FollowPlayer>().player = obmp[name].transform;
                }
            }
            var obt = obmp[name].transform;
            //缓动
            float angle = obinf.rot.y;
            float f = (float)(angle*Math.PI/180.0f);
            obt.position = Vector3.Lerp(obt.position, obinf.pos, 0.2f);
            //obt.localEulerAngles = Vector3.Lerp(obt.localEulerAngles, obinf.rot, 0.1f);
            obt.rotation = Quaternion.Slerp(obt.rotation
                , Quaternion.LookRotation((new Vector3((float)Math.Sin(f),0,(float)Math.Cos(f)))), 0.1f);
            obt.Find("HPUI").transform.eulerAngles = new Vector3(0,0,0);
            obt.Find("HPUI").Find("Slider").GetComponent<Slider>().value = Logic.obinfmp[name].curCreatureData.curHP/Logic.obinfmp[name].curCreatureData.maxHP;

            Debug.DrawLine(obt.position,obt.position+Quaternion.Euler(0,-45,0)*obt.forward*0.8f, Color.white);
            Debug.DrawLine(obt.position,obt.position+Quaternion.Euler(0,-30,0)*obt.forward*0.8f,Color.white);
            Debug.DrawLine(obt.position,obt.position+Quaternion.Euler(0,-15,0)*obt.forward*0.8f,Color.white);
            Debug.DrawLine(obt.position,obt.position+Quaternion.Euler(0,0,0)*obt.forward*0.8f,Color.white);
            Debug.DrawLine(obt.position,obt.position+Quaternion.Euler(0,15,0)*obt.forward*0.8f,Color.white);
            Debug.DrawLine(obt.position,obt.position+Quaternion.Euler(0,30,0)*obt.forward*0.8f,Color.white);
            Debug.DrawLine(obt.position,obt.position+Quaternion.Euler(0,45,0)*obt.forward*0.8f,Color.white);
        }
        

        foreach(var t in bullets) {
            var e = bullets[t.Key];
            var b = BulletsManager.buob[t.Key];
            e.transform.position =  Vector3.Lerp(e.transform.position,new Vector3(b.pos.x,e.transform.position.y,b.pos.z),0.7f);
            e.transform.eulerAngles = b.rot;
            //Debug.Log("view " + b.rot);
        }

        //timer.First();
        timer.Update();
        //if(EventHandle != null) {
        //    EventHandle();
        //    EventHandle -= EventHandle;
        //}

    }


    /// <summary>
    /// 视图层创建特效
    /// </summary>
    /// <param name="id">创建的特效获得的ID</param>
    /// <param name="effectname">特效名称</param>
    /// <param name="mastername">特效归属者名称</param>
    /// <param name="loadpointname">特效载入点名称</param>
    /// <param name="iffollow">特效是否跟随载入点</param>
    public static void CreatePlayerEffect(int id, string effectname, string mastername, string loadpointname, bool iffollow) {
        //归属者GameObject
        var masterob = obmp[mastername];
        //载入点GameObject
        var loadpointob = masterob.transform.Find(loadpointname);
        //实例化的特效GameObject
        var effectob = GameObject.Instantiate(RSDB.DB[effectname], loadpointob.position,loadpointob.rotation) as GameObject;
        if(iffollow) {
            //跟随载入点
            effectob.transform.parent = loadpointob;
        }
        //将特效置入effect字典中，以ID查询
        effects[id] = effectob;
    }
    /// <summary>
    /// 销毁玩家特效
    /// </summary>
    /// <param name="id"></param>
    public static void RemovePlayerEffect(int id) {
        if(effects.ContainsKey(id)) {
            GameObject.Destroy(effects[id]);
            effects.Remove(id);
        }
    }
    /// <summary>
    /// 创建视图层子弹实例
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    public static void CreateBullet(string mastername, int id, string name, Vector3 pos, Vector3 rot, bool iffly) {
        var loadpointob = obmp[mastername].transform.Find("BulletPoint");
        var bulletob = GameObject.Instantiate(RSDB.DB[name],loadpointob.position,loadpointob.rotation);
        if(!iffly) {
            bulletob.transform.position = new Vector3(bulletob.transform.position.x,obmp[mastername].transform.position.y,bulletob.transform.position.z);
        }
        //bulletob.transform.eulerAngles = rot;
        bullets[id] = bulletob;
    }
    /// <summary>
    /// 销毁子弹
    /// </summary>
    /// <param name="id"></param>
    public static void RemoveBullet(int id) {
        if(bullets.ContainsKey(id)) {
            GameObject.Destroy(bullets[id]);
            bullets.Remove(id);
        }
    }
}

using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//登录场景
public class LoginScence : MonoBehaviour {

    public GameObject buttonLogin;
    public GameObject buttonRegistered;
    public Client myClient;
    public bool ifLogin = false;
    void Start()
    {


        //登录场景UI初始化
        buttonLogin = GameObject.Find("Login");
        buttonRegistered = GameObject.Find("Registered");
        buttonLogin.GetComponent<Button>().onClick.AddListener(Login);
        buttonRegistered.GetComponent<Button>().onClick.AddListener(Registered);
        
        //登录场景客户端初始化与运行
        bool ret;
        ret = Client.Init();
        if(ret) {
            Debug.Log("connect ok");
        } else {
            Debug.Log("connect error");
        }


        Client.ifrun = true;
        Thread thread = new Thread(myfunc);
        thread.IsBackground = true;
        thread.Name = "Recv";
        thread.Start();

        
        //PriorityQueue<ValueTuple<float,Action>> Q = new PriorityQueue<(float, Action)>();
        //LogicTimer timer = new LogicTimer();
        /*for(int i = 1;i <= 5;i++) {
            Q.Push((i,delegate(){}));
        }
        for(int i = 1;i <= 5;i++) {
            Q.Push((100-i,delegate(){}));
        }
        int c = 0;
        Q.Push((6,delegate() {
            Debug.Log("TEST");
        }));
        while(Q.Count > 0) {
            var temp = Q.Pop();
            temp.Item2();
            Debug.Log("Q element " + ++c + " : " + temp.Item1);
        }*/
/*
        for(int i = 1;i <= 5;i++) {
            timer.Add((i,delegate(){}));
        }
        for(int i = 1;i <= 5;i++) {
            timer.Add((100-i,delegate(){}));
        }
        int c = 0;
        timer.Add((6,delegate() {
            Debug.Log("TEST");
        }));
        while(timer.Count > 0) {
            var temp = timer.Pop();
            temp.Item2();
            Debug.Log("Q element " + ++c + " : " + temp.Item1);
        }*/

        //Client.RunAutoRecvThread();
    }
    void myfunc() {
        //Debug.Log("RunAutoRecvThread begin");
        while(true) {
            if(!Client.ifrun) {
                break;
            }
            Client.Recv();
        }
        Client._ClientSocket.Close();
    }


    void Update() {
        //Debug.Log("login sc dataq count " + Client.dataQueue.Count);
        if(ifLogin) {
            //已获得帧消息就开始游戏，场景跳
            SceneManager.LoadScene("ssstest2");
        }
    }

    void Login() {
        //Debug.Log("Login begin");
        string name = GameObject.Find("Account").GetComponent<InputField>().text;
        string password = GameObject.Find("Password").GetComponent<InputField>().text;
        LoginReq req = new LoginReq();
        req.Name = name;
        req.Password = password;
        Client.Send(Client.LOGINTYPE, req);
        LoginRes res = new LoginRes();
        res.Res = 0;
        /*if(Client.dataQueue.Count > 0) {
            var temp = Client.dataQueue.Dequeue();
            Debug.Log(temp.Item1 + " " + temp.Item2.ToString());
        }*/
        float beginTime = Time.time;
        while(true) {
            if(Time.time-beginTime > 5) {
                Debug.Log("超时");
                break;
            }
            if(Client.dataQueue.Count<=0) 
                continue;
            var temp = Client.dataQueue.Dequeue();
            if(temp.Item1 != Client.LOGINTYPE) {
                Debug.Log("TYPE ERROR");
                break;
                //continue;
            }
            else {
                res = Client.Deserialize<LoginRes>(temp.Item2,0,temp.Item2.Length);
                break;
            }
        }
        if(res.Res == 0) {
            Debug.Log("login no");
        }
        else {
            Debug.Log("login yes");
            Client.name = name;
            ifLogin = true;
            //SceneManager.LoadScene("ssstest");
        }
    }
    void Registered() {
        string name = GameObject.Find("Account").GetComponent<InputField>().text;
        string password = GameObject.Find("Password").GetComponent<InputField>().text;
        RegisteredReq req = new RegisteredReq();
        req.Name = name;
        req.Password = password;
        Client.Send(Client.REGISTEREDTYPE, Client.Serialize(req).Length,Client.Serialize(req));
        RegisteredRes res = new RegisteredRes();
        float beginTime = Time.time;
        while(true) {
            if(Time.time-beginTime > 5) {
                Debug.Log("超时");
                break;
            }
            if(Client.dataQueue.Count<=0)
                continue;
            var temp = Client.dataQueue.Dequeue();
            if(temp.Item1 != Client.REGISTEREDTYPE) {
                Debug.Log("TYPE ERROR");
                break;
                //continue;
            }
            else {
                res = Client.Deserialize<RegisteredRes>(temp.Item2,0,temp.Item2.Length);
                break;
            }
        }
        if(res.Res == 0) {
            Debug.Log("registered no");
        }
        else {
            Debug.Log("registered yes");
        }
    }

}



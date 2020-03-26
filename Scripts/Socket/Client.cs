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

public class Client
{
    /// <summary>
    /// 表示Client还是否运行
    /// </summary>
    public static bool ifrun = true;
    /// <summary>
    /// 当前客户端的用户名
    /// </summary>
    public static string name = "";
    /// <summary>
    /// 客户端通讯用的套接字
    /// </summary>
    public static Socket _ClientSocket;
    /// <summary>
    /// 客户端连接服务器要用的IP和端口信息
    /// </summary>
    public static IPEndPoint SeverEndPoint;

    /// <summary>
    /// 自定义数据流协议头长度
    /// </summary>
    public const int HEADLEN = 3;
        
    /// <summary>
    /// 数据类型 注册
    /// </summary>
    public static int REGISTEREDTYPE = 1;
    /// <summary>
    /// 数据类型 登录
    /// </summary>
    public static int LOGINTYPE = 2;
    /// <summary>
    /// 数据类型 操作
    /// </summary>
    public static int OPERATETYPE = 3;
    /// <summary>
    /// 数据类型 同步
    /// </summary>
    public static int POSSYNTYPE = 4;
    /// <summary>
    /// 数据类型 帧头
    /// </summary>
    public static int FRAMHEADTYPE = 5;
    /// <summary>
    /// 数据类型 帧操作
    /// </summary>
    public static int FRAMOPTYPE = 6;
    /// <summary>
    /// 数据类型 帧包
    /// </summary>
    public static int FRAMPACKAGETYPE = 7;


    /// <summary>
    /// 帧操作类型 移动
    /// </summary>
    public static int MOVETYPE = 1;
    /// <summary>
    /// 帧操作类型 技能
    /// </summary>
    public static int SKILLTYPE = 2;

    /// <summary>
    /// 处理好的数据包（未序列化）队列
    /// </summary>
    public static Queue<ValueTuple<int,byte[]>> dataQueue;

    /// <summary>
    /// 初始化客户端socket并连接
    /// </summary>
    /// <param name="serverEndPoint">需要连接的IP+端口</param>
    /// <returns>是否连接服务器成功</returns>
    public static bool Init(IPEndPoint serverEndPoint = null)
    {
        //客户端登录名字初始化
        name = "";
        //客户端数据包队列初始化
        dataQueue = new Queue<(int, byte[])>();

        //客户端所需连接的服务器地址设定
        if(serverEndPoint==null) {
            SeverEndPoint = new IPEndPoint(IPAddress.Parse("117.78.9.170"), 10009);
        } else { 
            SeverEndPoint = serverEndPoint;
        }

        //建立客户端Socket
        _ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try {   
            //连接客户端
            _ClientSocket.Connect(SeverEndPoint);
        } catch (Exception) {
            //连接失败
            Debug.Log("exception connect");
            return false;
        }

        //设置当前客户端为阻塞
        _ClientSocket.Blocking = true;

        //客户端连接服务器成功
        return true;
    }

    /// <summary>
    /// Protobuf类序列化成数据流
    /// </summary>
    /// <param name="obj">需要序列化的实例对象名</param>
    /// <typeparam name="T">Protobuf的类名</typeparam>
    /// <returns>byte数组</returns>
    public static byte[] Serialize<T>(T obj) where T : IMessage {
        return obj.ToByteArray();
    }

    /// <summary>
    /// 数据流反序列化成Protobuf类
    /// </summary>
    /// <typeparam name="T">Protobuf类名</typeparam>
    /// <param name="data">byte 数据</param>
    /// <param name="offset">偏移量，从byte数组的哪个位置开始读取</param>
    /// <param name="len">数据长度</param>
    public static T Deserialize<T>(byte[] data, int offset, int len) where T : class, IMessage, new(){
        T obj = new T();
        Google.Protobuf.IMessage message = obj.Descriptor.Parser.ParseFrom(data,offset,len);
        return message as T;
    }

    /// <summary>
    /// 向服务器发送数据
    /// </summary>
    /// <param name="type">数据类型</param>
    /// <param name="len">数据长度</param>
    /// <param name="msg">数据（byte串）</param>
    public static void Send(int type, int len, byte[] msg) {
        //处理数据头
        char[] c = new Char[3];
        c[0] = (char)type;
        c[1] = (char)((len>>6)+1);
        c[2] = (char)(len-(c[1]-1<<6)+1);
        byte[] head = Encoding.Default.GetBytes(c);

        //组合数据头和数据
        byte[] datapackage = new byte[head.Length+msg.Length];
        head.CopyTo(datapackage,0);
        msg.CopyTo(datapackage,head.Length);

        _ClientSocket.Send(datapackage);
    }
    /// <summary>
    /// 向服务器发送数据
    /// </summary>
    /// <typeparam name="T">Protobuf类的名称</typeparam>
    /// <param name="type">数据类型</param>
    /// <param name="t">需要序列化的protobuf类实例</param>
    public static void Send<T>(int type, T t) where T : class, IMessage, new() {
        //序列化protobuf类实例
        byte[] msg;
        msg = Serialize<T>(t);

        //处理数据头
        char[] c = new Char[3];
        int len = msg.Length;
        c[0] = (char)type;
        c[1] = (char)((len>>6)+1);
        c[2] = (char)(len-(c[1]-1<<6)+1);
        byte[] head = Encoding.Default.GetBytes(c);

        //组合数据头和数据
        byte[] datapackage = new byte[head.Length+msg.Length];
        head.CopyTo(datapackage,0);
        msg.CopyTo(datapackage,head.Length);
         _ClientSocket.Send(datapackage);
    }

    /// <summary>
    /// 接受消息的缓冲区大小
    /// </summary>
    const int MAXBUFF = 40960;
    /// <summary>
    /// 指向接收消息的缓冲区的未处理消息的尾部+1
    /// </summary>
    public static int tailp = 0;
    /// <summary>
    /// 接受服务器消息的缓冲区
    /// </summary>
    public static byte[] buff = new byte[MAXBUFF];
    public static void Recv() {
        
        //读取服务器发送的消息
        int len = _ClientSocket.Receive(buff,tailp,MAXBUFF-tailp,SocketFlags.None);
        if(len == 0) { //服务器关闭
            Debug.Log("server close");
        } else if(len <= 0) { //读取信息发生错误
            Debug.Log("recv error " + len);
            return ;
        }
        //缓冲区数据尾指针后移
        tailp += len;

        int type;  //表示数据流的数据类型
        int headp; //表示缓冲区数据的头指针位置

        //循环读取数据
        while(true) {
            headp = 0;
            //够拿头
            while(tailp-headp>=Client.HEADLEN) {
                //处理头
                type = (int)buff[headp];
                len = (int)buff[headp+1]-1;
                len <<= 6;
                len += (int)buff[headp+2]-1;
                
                //判断协议头后数据量是否满足协议头中的定义的量
                if(tailp-headp>=len+Client.HEADLEN) {
                    byte[] b = new byte[len];
                    for(int i = 0;i < len;i++) {
                        b[i] = buff[i+Client.HEADLEN+headp];
                    }
                    dataQueue.Enqueue((type,b));
                    headp += len+HEADLEN;
                } else {
                    break;
                }
            }
            //头指针位置不在初始位置，就把头指针后的数据挪到最前面
            if(headp!=0) {
                for(int i = 0;i < tailp - headp;i++) {
                    buff[i] = buff[i+headp];
                }
            }
            //更新尾指针
            tailp = tailp - headp;
            break;
        }
       
    }


    /// <summary>
    /// 运行不断接收服务器发送过来的信息流的线程
    /// </summary>
    public static void RunAutoRecvThread() {
        Client.ifrun = true;
        Thread thread = new Thread(delegate() {
            while(true) {
                if(!Client.ifrun) {
                    break;
                }
                Client.Recv();
            }
            Client._ClientSocket.Close();
        });
        thread.IsBackground = true;
        thread.Name = "Recv";
        thread.Start();
    }

    //关闭连接
    public static void Close() {
        _ClientSocket.Shutdown(SocketShutdown.Both);
        _ClientSocket.Close();
    }
}

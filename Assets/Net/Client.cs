using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class Client 
{
    public static  Socket S_Client { get; private set; }
    public   Thread Reader;//异步接收服务端数据包
    //回复路由
    public static Dictionary<Agreement, BaseResponse> Routting = new Dictionary<Agreement, BaseResponse>();
    public Client()
    {
        InitRouting();
        S_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
        EndPoint point = new IPEndPoint(ipaddress, 7879);
        try
        {
            S_Client.Connect(point);
        }
        catch (Exception e)
        {
            
            Debug.Log(e);
        }
        //开启线程去读取服务端回复
        Reader = new Thread(new ParameterizedThreadStart(Tool.Analysis))
        {
            IsBackground = true
        };    
        Reader.Start(S_Client);
    }
    private void InitRouting()
    {
        Routting.Add(Agreement.PGreet, new GreetResponse());
    }
}

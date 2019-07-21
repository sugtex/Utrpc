using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public static class Tool 
{
    //包装请求类
    public static Protocol PacketRequest(Agreement a,string f,object c)
    {
        Protocol p = new Protocol();
        p.Mark = a;
        p.Function = f;
        p.DataRequest = c;
        return p;
    }
    //封包
    public static void Packet(Protocol p,Socket client)
    {
        Shell shell = new Shell();
        shell.Header = "DDF-CXK";
        shell.Kernel = p;
        string data = JsonMapper.ToJson(shell);
        uint header = Convert.ToUInt32(data.Length);//得到整个包的长度
        //转byte数组
        byte[] bheader = BitConverter.GetBytes(header);//小端
        byte[] bcontent = Encoding.UTF8.GetBytes(data);
        //合并byte数组
        List<byte> all = new List<byte>();
        all.AddRange(bheader);
        all.AddRange(bcontent);
        client.Send(all.ToArray());
    }
    //解包
    public static void Analysis(object obj)
    {
        byte[] buf = new byte[1024];
        List<byte> cache = new List<byte>();
        Socket client = obj as Socket;
        while (true)
        {
            int templength = -1;
            try
            {
                templength =client.Receive(buf);
            }
            catch (Exception e)
            {
                client.Close();
                Debug.Log(e);
            }
            byte[] temp = new byte[templength];
            Array.Copy(buf, 0, temp, 0, templength);
            cache.AddRange(temp);
            int length = cache.Count;
            if (length == 0 || length <= 4)
            {
                continue;
            }
            uint header = BitConverter.ToUInt32(cache.ToArray(), 0);
            if (Convert.ToUInt32(length) < 4 + header)
            {
                continue;
            }
            string content = Encoding.UTF8.GetString(cache.ToArray(), 4, Convert.ToInt32(header));

            Shell s = JsonMapper.ToObject<Shell>(content);
            Protocol p = s.Kernel;
            Type t = Client.Routting[p.Mark].GetType();
            object[] data = p.DataResponse.Data;
            List<object> pg = new List<object>();
            object tp;
            for (int i = 0, len = data.Length; i < len; i++)
            {
                tp = data[i];
                pg.Add(tp);
            }
            //反射出方法进行调用
            t.GetMethod(s.Kernel.Function).Invoke(Client.Routting[p.Mark], pg.ToArray());
            cache.RemoveRange(0, 4 + Convert.ToInt32(header));
        }

    }
}

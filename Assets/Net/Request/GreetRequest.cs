using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreetRequest : MonoBehaviour
{
  
    public Button btn;

    
    public void SayHello()
    {
        Protocol p = Tool.PacketRequest(Agreement.PGreet, "SayHello", new Greet { Msg="I can up"}); 
        Tool.Packet(p, Client.S_Client);
    }
}

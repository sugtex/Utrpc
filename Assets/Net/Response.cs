using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Response 
{
    public int Status { get; set; }
    public string Msg { get; set; }
    public object[] Data { get; set; }
}

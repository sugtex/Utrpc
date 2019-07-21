using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Protocol
{
  public Agreement Mark { get; set; }
  public string Function { get; set; }
  public object DataRequest    { get; set; }
  public Response DataResponse { get; set; }
}
public enum Agreement
{
    PGreet,

}


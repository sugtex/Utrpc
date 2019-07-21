using UnityEngine;

public class Game : MonoBehaviour
{
    Client M_Socket;
    private void Start()
    {
        M_Socket = new Client();
    }
    private void OnDestroy()
    {
        M_Socket.Reader.Abort();
        Client.S_Client.Close();
    }
}

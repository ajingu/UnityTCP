using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntSeverTest : MonoBehaviour
{
    [SerializeField] int port;
    [SerializeField] string adapterName;

    Server server;
    void Start()
    {
        server = new IntServer();

        string address = IPv4Getter.GetIP(adapterName);
        if (address == "")
        {
            Debug.Log("Can't get IP address.");
            return;
        }

        server.Listen(address, port);
    }

    void OnApplicationQuit()
    {
        if (server != null)
        {
            server.Close();
        }
    }
}

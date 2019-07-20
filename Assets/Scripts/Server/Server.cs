using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Server
{
    private TcpListener _listener;
    private readonly List<TcpClient> _clients = new List<TcpClient>();

    public void Listen(string host, int port)
    {
        Debug.Log("ipAddress:" + host + " port:" + port);
        var ip = IPAddress.Parse(host);
        _listener = new TcpListener(ip, port);
        _listener.Start();
        _listener.BeginAcceptSocket(DoAcceptTcpClientCallback, _listener);
    }

    private void DoAcceptTcpClientCallback(IAsyncResult ar)
    {
        var listener = (TcpListener)ar.AsyncState;
        var client = listener.EndAcceptTcpClient(ar);
        _clients.Add(client);
        Debug.Log("Connect: " + client.Client.RemoteEndPoint);

        listener.BeginAcceptSocket(DoAcceptTcpClientCallback, listener);

        var stream = client.GetStream();
        var reader = new StreamReader(stream, Encoding.UTF8);

        while (client.Connected)
        {
            while (!reader.EndOfStream)
            {
                var str = reader.ReadLine();
                OnMessage(str);
            }

            if (client.Client.Poll(1000, SelectMode.SelectRead) && (client.Client.Available == 0))
            {
                Debug.Log("Disconnect: " + client.Client.RemoteEndPoint);
                client.Close();
                _clients.Remove(client);
                break;
            }
        }
    }

    protected virtual void OnMessage(string msg)
    {
        Debug.Log(msg);
    }

    protected void SendMessageToClient(string msg)
    {
        if (_clients.Count == 0)
        {
            return;
        }
        var body = Encoding.UTF8.GetBytes(msg);

        foreach (var client in _clients)
        {
            try
            {
                var stream = client.GetStream();
                stream.Write(body, 0, body.Length);
            }
            catch
            {
                _clients.Remove(client);
            }
        }
    }

    public void Close()
    {
        if (_listener == null)
        {
            return;
        }

        if (_clients.Count != 0)
        {
            foreach (var client in _clients)
            {
                client.Close();
            }
        }
        _listener.Stop();
    }
}
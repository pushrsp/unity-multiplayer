using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Google.Protobuf;
using ServerCore;
using UnityEngine;


public class NetworkManager
{
    private ServerSession _serverSession = new ServerSession();

    public void Send(IMessage packet)
    {
        _serverSession.Send(packet);
    }

    public void Init()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);

        Connector connector = new Connector();
        connector.Connect(endPoint, () => _serverSession, 1);
    }

    public void OnUpdate()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        if (list.Count == 0)
            return;

        foreach (PacketMessage packet in list)
        {
            Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.MessageId);
            if (handler != null)
                handler.Invoke(_serverSession, packet.Message);
        }
    }
}
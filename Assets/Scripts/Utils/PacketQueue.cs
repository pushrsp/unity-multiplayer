using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

public class PacketMessage
{
    public ushort MessageId { get; set; }
    public IMessage Message { get; set; }
}

public class PacketQueue
{
    public static PacketQueue Instance { get; } = new PacketQueue();

    private Queue<PacketMessage> _packetQueue = new Queue<PacketMessage>();
    private object _lock = new object();

    public void Push(ushort id, IMessage packet)
    {
        lock (_lock)
        {
            _packetQueue.Enqueue(new PacketMessage {MessageId = id, Message = packet});
        }
    }

    public void Push(PacketMessage msg)
    {
        lock (_lock)
        {
            _packetQueue.Enqueue(msg);
        }
    }

    public PacketMessage Pop()
    {
        lock (_lock)
        {
            if (_packetQueue.Count == 0)
                return null;

            return _packetQueue.Dequeue();
        }
    }

    public List<PacketMessage> PopAll()
    {
        List<PacketMessage> list = new List<PacketMessage>();

        lock (_lock)
        {
            while (_packetQueue.Count > 0)
                list.Add(_packetQueue.Dequeue());
        }

        return list;
    }
}
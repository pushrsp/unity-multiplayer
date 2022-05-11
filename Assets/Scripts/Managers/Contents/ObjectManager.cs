using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

// ReSharper disable All

public class ObjectManager
{
    public MyPlayerController MyPlayer { get; set; }

    private Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();

    public void Add(PlayerInfo playerInfo, bool myPlayer = false)
    {
        if (myPlayer)
        {
            GameObject go = Managers.Resource.Instantiate("Creatures/Player");
            go.name = $"MyPlayer-{playerInfo.PlayerId}";
            _players.Add(playerInfo.PlayerId, go);

            MyPlayer = go.GetOrAddComponent<MyPlayerController>();
            MyPlayer.PlayerInfo = playerInfo;
            MyPlayer.SyncPos();
        }
        else
        {
            GameObject go = Managers.Resource.Instantiate("Creatures/Player");
            go.name = $"Player-{playerInfo.PlayerId}";
            _players.Add(playerInfo.PlayerId, go);

            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            pc.PlayerInfo = playerInfo;
            pc.SyncPos();
        }
    }

    public GameObject Find(Vector3Int destPos)
    {
        foreach (GameObject go in _players.Values)
        {
            CreatureController cc = go.GetComponent<CreatureController>();
            if (cc == null)
                continue;

            if (cc.PlayerInfo.PlayerId == MyPlayer.PlayerInfo.PlayerId)
                continue;

            if ((cc.PosInfo - destPos).magnitude < 5.0f)
                return cc.gameObject;
        }

        return null;
    }

    public GameObject FindById(int id)
    {
        GameObject player;
        if (_players.TryGetValue(id, out player))
            return player;

        return null;
    }

    public void Remove(int playerId)
    {
        GameObject go = FindById(playerId);
        if (go == null)
            return;

        Managers.Resource.Destroy(go);
        _players.Remove(playerId);
    }
}
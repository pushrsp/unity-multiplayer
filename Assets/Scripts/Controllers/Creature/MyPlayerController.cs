using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

// ReSharper disable All

public class MyPlayerController : PlayerController
{
    private Vector3 _delta;

    void Start()
    {
        Init();
    }

    void LateUpdate()
    {
        Camera.main.transform.position = transform.position + _delta;
    }

    protected override void Init()
    {
        base.Init();
        _delta = Camera.main.transform.position - transform.position;
    }

    protected override void UpdateState()
    {
        switch (State)
        {
            case CreatureState.Idle:
                UpdateKeyboard();
                break;
            case CreatureState.Moving:
                UpdateKeyboard();
                break;
            case CreatureState.Jump:
                UpdateKeyboard();
                break;
        }

        base.UpdateState();
    }

    protected override void UpdateIdle()
    {
    }

    private Vector3 _moveVec;

    private void UpdateKeyboard()
    {
        if (Input.anyKey == false)
            return;

        if (Input.GetKey(KeyCode.W))
            _moveVec += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            _moveVec += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            _moveVec += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            _moveVec += Vector3.right;

        if (_isJump == false && Input.GetKeyDown(KeyCode.Space))
        {
            _isJump = true;
        }

        UpdateMove();
        CheckUpdated();
    }

    protected override void CollisionFloor(Collision collision)
    {
        if (_isJump == true)
        {
            State = CreatureState.Idle;
            _isJump = false;
        }

        base.CollisionFloor(collision);
    }

    private void UpdateMove()
    {
        if (_moveVec == Vector3.zero)
        {
            State = CreatureState.Idle;
            return;
        }

        Vector3Int destPos = Vector3Int.RoundToInt(transform.position + _moveVec.normalized);
        if (Managers.Map.CanGo(destPos))
        {
            if (Managers.Object.Find(destPos) == null)
                PosInfo = destPos;

            if (_isJump)
                State = CreatureState.Jump;
            else
                State = CreatureState.Moving;
        }
    }

    private void CheckUpdated()
    {
        if (_updated)
        {
            C_Move movePacket = new C_Move {PosInfo = new PositionInfo()};

            {
                movePacket.PosInfo.State = State;
                movePacket.PosInfo.PosY = PosInfo.y;
                movePacket.PosInfo.PosZ = PosInfo.z;
                movePacket.PosInfo.PosX = PosInfo.x;
            }

            Managers.Network.Send(movePacket);

            _updated = false;
            _moveVec = Vector3.zero;
        }
    }
}
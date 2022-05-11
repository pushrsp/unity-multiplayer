using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.AI;

// ReSharper disable All

public class CreatureController : MonoBehaviour
{
    protected bool _updated;
    protected float _fallMultiplyer = 2.5f;
    protected float _lowJumpMultiplyer = 2.0f;
    protected bool _isJump;

    private PlayerInfo _playerInfo = new PlayerInfo {PosInfo = new PositionInfo()};

    private Animator _anim;
    private Rigidbody _rigid;

    protected Rigidbody Rigid
    {
        get
        {
            if (_rigid == null)
                _rigid = GetComponent<Rigidbody>();

            return _rigid;
        }
    }

    private Animator Anim
    {
        get
        {
            if (_anim == null)
                _anim = transform.GetChild(0).GetComponent<Animator>();

            return _anim;
        }
    }

    [HideInInspector]
    public PlayerInfo PlayerInfo
    {
        get { return _playerInfo; }
        set
        {
            if (_playerInfo.Equals(value))
                return;

            _playerInfo.PlayerId = value.PlayerId;
            Speed = value.Speed;
            PosInfo = new Vector3Int(value.PosInfo.PosX, value.PosInfo.PosY, value.PosInfo.PosZ);
            State = value.PosInfo.State;
        }
    }

    [HideInInspector]
    public float Speed
    {
        get { return PlayerInfo.Speed; }
        set
        {
            if (_playerInfo.Speed == value)
                return;

            _playerInfo.Speed = value;
            UpdateAnimation();
        }
    }

    [HideInInspector]
    public Vector3Int PosInfo
    {
        get { return new Vector3Int(PlayerInfo.PosInfo.PosX, PlayerInfo.PosInfo.PosY, PlayerInfo.PosInfo.PosZ); }
        set
        {
            if (_playerInfo.PosInfo.Equals(value))
                return;

            _playerInfo.PosInfo.PosY = value.y;
            _playerInfo.PosInfo.PosZ = value.z;
            _playerInfo.PosInfo.PosX = value.x;

            _updated = true;
        }
    }

    [HideInInspector]
    public CreatureState State
    {
        get { return PlayerInfo.PosInfo.State; }
        set
        {
            if (_playerInfo.PosInfo.State == value)
                return;

            _playerInfo.PosInfo.State = value;
            UpdateAnimation();
        }
    }

    protected virtual void UpdateAnimation()
    {
        switch (State)
        {
            case CreatureState.Idle:
                Anim.SetFloat("Speed", 0.0f);
                break;
            case CreatureState.Moving:
                Anim.SetFloat("Speed", Speed);
                break;
        }
    }

    void Update()
    {
        UpdateState();
        UpdateJumpVelocity();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Floor":
                CollisionFloor(collision);
                break;
        }
    }

    private void UpdateJumpVelocity()
    {
        if (Rigid.velocity.y < 0)
        {
            Rigid.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplyer - 1) * Time.deltaTime;
        }
        else if (Rigid.velocity.y > 0)
        {
            Rigid.velocity += Vector3.up * Physics.gravity.y * (_lowJumpMultiplyer - 1) * Time.deltaTime;
        }
    }

    protected virtual void UpdateState()
    {
        switch (State)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Moving:
                UpdateMoving();
                break;
            case CreatureState.Jump:
                UpdateJump();
                break;
        }
    }

    protected virtual void UpdateIdle()
    {
    }

    protected virtual void UpdateMoving()
    {
        Vector3 destPos = PosInfo;
        Vector3 moveDir = destPos - transform.position;
        float dist = moveDir.magnitude;

        if (dist < Speed * Time.deltaTime)
        {
            State = CreatureState.Idle;
            transform.position = destPos;
        }
        else
        {
            Vector3 look = moveDir.normalized;
            look.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), 0.2f);
            transform.position += moveDir.normalized * Speed * Time.deltaTime;
        }
    }

    private bool _doJump;

    protected virtual void UpdateJump()
    {
        if (_doJump == false)
        {
            Anim.SetBool("IsJump", true);
            Anim.SetTrigger("DoJump");
            _doJump = true;
            Rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
        }

        UpdateMoving();
    }

    protected virtual void CollisionFloor(Collision collision)
    {
        if (_doJump == true)
        {
            Anim.SetBool("IsJump", false);
            State = CreatureState.Idle;
            _doJump = false;
        }
    }

    protected virtual void Init()
    {
    }

    public void SyncPos()
    {
        transform.position = PosInfo;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Apis;
using EventData;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMoveComponent : UnitMoveComponent
{
    private Player player;
    
    public override void MoveOn()
    {
        base.MoveOn();
    }
    public override void Init(IMovable mover, Collider2D col)
    {
        base.Init(mover, col);
        player = mover as Player;
    }

    public override void MoveCCOn()
    {
        base.MoveCCOn();
        player.ClimbOff();
    }

    public override void MoveCCOff()
    {
        base.MoveCCOff();
        player.ClimbOn();
        player.SetState(EPlayerState.Stop);
    }

    public override void Stop()
    {
        base.Stop();
        player.animator.SetBool("IsMove", false);
    }

    public override void KnockBack(Vector2 src, KnockBackData knockBackData, UnityAction OnBegin,
        UnityAction OnEnd)
    {
        player.ExecuteEvent(EventType.OnHitReaction, null);
        
        base.KnockBack(src, knockBackData, OnBegin, OnEnd);
    }
}

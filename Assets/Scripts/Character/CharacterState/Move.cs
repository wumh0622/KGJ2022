using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move : StateBase
{
    public float moveSpeed = 0.0f;

    public override void Init(CharacterBase character)
    {
        stateType = CharacterState.Move;
        base.Init(character);
    }

    public override void StateEnter()
    {
        base.StateEnter();
        owner.moveSpeed = moveSpeed;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if(owner.targetMoveSpeed == 0.0f)
        {
            owner.ChangeState(CharacterState.Idle);
        }
    }

    public override void StateEnd()
    {
        base.StateEnd();
    }
}

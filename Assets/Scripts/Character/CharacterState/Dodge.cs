using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dodge : StateBase
{
    public float dodgeTime;
    public float dodgeSpeed;
    public float dodgeHeight;
    public LayerMask layerMask;

    bool inTunnel;
    bool dodgeTimerEnd;

    public override void Init(CharacterBase character)
    {
        stateType = CharacterState.Dash;
        base.Init(character);
    }

    public override void StateEnter()
    {
        base.StateEnter();

        owner.OverrideMoveUpdate(true);
        owner.wallDetectHandler.gameObject.SetActive(false);
        owner.DisableMovement(true);

        CapsuleCollider2D collider = (CapsuleCollider2D)owner.bodyCollision;
        collider.size = new Vector2(collider.size.x, collider.size.y * dodgeHeight);
        collider.offset = new Vector2(collider.offset.x, collider.offset.y * dodgeHeight);
        SimpleTimerManager.Instance.RunTimer(DodgeEnd, dodgeTime);
    }

    public override bool IsStateAviliable()
    {
        return (owner.currentState == CharacterState.Idle || owner.currentState == CharacterState.Move || owner.currentState == CharacterState.Jump);
    }

    private void DodgeEnd()
    {
        dodgeTimerEnd = true;
        if (!inTunnel)
        {
            CapsuleCollider2D collider = (CapsuleCollider2D)owner.bodyCollision;
            owner.wallDetectHandler.gameObject.SetActive(true);
            collider.size = new Vector2(collider.size.x, collider.size.y / dodgeHeight);
            collider.offset = new Vector2(collider.offset.x, collider.offset.y / dodgeHeight);

            owner.ChangeState(CharacterState.Idle);
        }
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (Physics2D.Raycast(owner.transform.position + new Vector3(0, 0.1f, 0), owner.transform.up, owner.bodyCollision.bounds.size.y / dodgeHeight, layerMask))
        {
            Debug.DrawRay(owner.transform.position + new Vector3(0, 0.1f, 0), owner.transform.up * (owner.bodyCollision.bounds.size.y / dodgeHeight), Color.red);
            inTunnel = true;
        }
        else
        {
            inTunnel = false;
            if(dodgeTimerEnd)
            {
                DodgeEnd();
            }
        }
    }

    public override void StateEnd()
    {
        owner.OverrideMoveUpdate(false);
        owner.DisableMovement(false);
        dodgeTimerEnd = false;
        inTunnel = false;
        base.StateEnd();


    }

    public override float StateMoveUpdate()
    {
        return dodgeSpeed / dodgeTime;
    }
}

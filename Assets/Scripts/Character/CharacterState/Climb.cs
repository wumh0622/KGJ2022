using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Climb : StateBase
{
    public float stateTime = 0.3f;
    public float climbYOffset = 0.1f;

    private Vector2 climbPoint;

    public override void Init(CharacterBase character)
    {
        stateType = CharacterState.Climb;
        base.Init(character);
        
    }


    public void ResetClimbInfo(Vector2 newClimbPoint)
    {
        climbPoint = newClimbPoint;
    }

    public override void StateEnter()
    {
        base.StateEnter();
        owner.transform.position = new Vector2(climbPoint.x, climbPoint.y + climbYOffset);
        owner.characterRigidbody.velocity = new Vector2(0.0f, 0.0f);
        owner.DisableMovement(true);
        SimpleTimerManager.Instance.RunTimer(EndClimb, stateTime);
    }

    private void EndClimb()
    {
        owner.ChangeState(CharacterState.Idle);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        
    }

    public override void StateEnd()
    {
        base.StateEnd();
        owner.DisableMovement(false);
    }
}

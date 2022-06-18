using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Attack : StateBase
{
    public float startAttackForce = 10.0f;
    public float attackForecDrop = 3.0f;

    int attackCombo = 0;
    float attackRushForce;

    bool attackAnimStateReset = true;
    bool attackStateCombo;

    public override void Init(CharacterBase character)
    {
        stateType = CharacterState.Attack;
        base.Init(character);
    }

    public override void StateEnter()
    {
        base.StateEnter();
        owner.DisableMovement(true);
        owner.OverrideMoveUpdate(true);
        
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        owner.AddMovement(owner.faceDirection);

    }

    public override void StateEnd()
    {
        base.StateEnd();
        owner.DisableMovement(false);
        owner.OverrideMoveUpdate(false);
        attackCombo = 0;
    }

    public override bool IsStateAviliable()
    {
        if (owner.currentState == CharacterState.Damage)
        {
            return false;
        }
        return true;
    }

    public int DoAttack()
    {
        if(!IsStateAviliable())
        {
            return 0;
        }
        
        if(attackCombo == 0)
        {
            attackCombo++;
        }

        owner.ChangeState(CharacterState.Attack);
        
        
        attackStateCombo = true;
        //owner.characterRigidbody.AddForce(new Vector2(owner.faceDirection * 500.0f, 0.0f));
        return attackCombo;
    }

    public void AttackEnd()
    {
        owner.ChangeState(CharacterState.Idle);
        attackStateCombo = false;
        attackAnimStateReset = true;
    }

    public override float StateMoveUpdate()
    {
        if(attackRushForce > 0)
        {
            attackRushForce = Mathf.Lerp(attackRushForce, 0, Time.deltaTime * attackForecDrop);
            Debug.Log(attackRushForce);
        }
        return attackRushForce;
    }

    public void AttackAnimStart()
    {
        attackRushForce = startAttackForce;
        owner.ForceUpdateFaceDirection();
        attackCombo++;
    }
}

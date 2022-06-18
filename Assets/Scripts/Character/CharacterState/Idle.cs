using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Idle : StateBase
{

    public override void Init(CharacterBase character)
    {
        stateType = CharacterState.Idle;
        base.Init(character);
    }

    public override void StateEnter()
    {
        base.StateEnter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateEnd()
    {
        base.StateEnd();
    }
}

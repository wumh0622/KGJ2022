using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class StateBase
{
    public delegate void OnStateEnter();
    public delegate void OnStateEnd();
    public OnStateEnter OnStateEnterDelegate;
    public OnStateEnd OnStateEndDelegate;

    [HideInInspector]public CharacterState stateType;

    protected CharacterBase owner;

    public virtual void Init(CharacterBase character)
    {
        owner = character;
        character.stateObjectList.Add(stateType, this);
    }

    public virtual void StateEnter()
    {
        OnStateEnterDelegate?.Invoke();
    }
    public virtual void StateUpdate()
    {
        
    }

    public virtual void StateEnd()
    {
        OnStateEndDelegate?.Invoke();
    }

    public virtual bool IsStateAviliable()
    {
        return true;
    }

    public virtual float StateMoveUpdate()
    {
        return 0.0f;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_AttackEnd : StateMachineBehaviour
{
    Attack attackState = null;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackState == null)
        {
            attackState = (Attack)animator.gameObject.GetComponent<CharacterBase>().stateObjectList[CharacterState.Attack];
        }

        attackState.AttackEnd();
    }
}

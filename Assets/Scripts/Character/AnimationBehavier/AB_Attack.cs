using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_Attack : StateMachineBehaviour
{
    Attack attackState = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackState == null)
        {
            attackState = (Attack)animator.gameObject.GetComponent<CharacterBase>().stateObjectList[CharacterState.Attack];
        }

        attackState.AttackAnimStart();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public void Test()
    {
        Debug.Log("Attack");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackInfo
{
    public float damage;
    public float force;
    public GameObject attacker;
    public ObjectForceInfo AttackForceInfo;

}

public class AttackHandle : MonoBehaviour
{
    private AttackTrigger[] attackTrigger;
    private Dictionary<string, AttackTrigger> attackTriggerList = new Dictionary<string, AttackTrigger>();

    private void Awake()
    {
        attackTrigger = GetComponentsInChildren<AttackTrigger>();

        foreach (AttackTrigger trigger in attackTrigger)
        {
            attackTriggerList.Add(trigger.triggerID, trigger);
        }
    }

    public void Attack(string triggerID)
    {
        Debug.Log("Attack : " + triggerID);
        if(attackTriggerList.ContainsKey(triggerID))
        {
            attackTriggerList[triggerID].StartAttack();
        }
    }

    public void AttackEnd(string triggerID)
    {
        Debug.Log("AttackEnd : " + triggerID);
        if (attackTriggerList.ContainsKey(triggerID))
        {
            attackTriggerList[triggerID].StopAttack();
        }
    }
}

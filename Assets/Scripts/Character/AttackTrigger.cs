using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public string triggerID;
    public ContactFilter2D filter;
    public AttackInfo attackInfo;
    Collider2D attackTrigger;
    List<Collider2D> hitList = new List<Collider2D>();

    private void Awake()
    {
        attackTrigger = GetComponent<Collider2D>();
        attackTrigger.isTrigger = true;
        attackTrigger.enabled = false;
        attackInfo.attacker = this.transform.parent.gameObject;
    }

    public void StartAttack()
    {
        Debug.Log("StartAttack");
        attackTrigger.enabled = true;
    }

    public void StopAttack()
    {
        Debug.Log("StopAttack");
        attackTrigger.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitList.Add(collision);
        foreach (Collider2D item in hitList)
        {
            IDamageHandle damageTarget = item.GetComponent<IDamageHandle>();
            if (damageTarget != null)
            {
                damageTarget.ApplyDamage(attackInfo);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hitList.Remove(collision);
    }
}

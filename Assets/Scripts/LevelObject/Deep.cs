using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deep : MonoBehaviour
{
    public ContactFilter2D filter2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
        if(player)
        {
            player.BackToSafePos();
        }

        dialog_object dialog_Object = collision.gameObject.GetComponent<dialog_object>();
        if(dialog_Object)
        {
            dialog_Object.ReturnToStartPoint();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.BackToSafePos();
        }

        dialog_object dialog_Object = collision.gameObject.GetComponent<dialog_object>();
        if (dialog_Object)
        {
            dialog_Object.ReturnToStartPoint();
        }
    }
}

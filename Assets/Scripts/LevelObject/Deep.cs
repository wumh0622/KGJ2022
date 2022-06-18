using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deep : MonoBehaviour
{
    public ContactFilter2D filter2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!filter2D.IsFilteringLayerMask(collision.gameObject))
        {
            return;
        }

        PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
        if(player)
        {
            player.BackToSafePos();
        }
    }
}

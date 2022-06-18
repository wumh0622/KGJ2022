using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterFootCollision : MonoBehaviour
{
    public bool onGround;
    public UnityEvent OnLandEvent;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnLandEvent.Invoke();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        onGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onGround = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWallDetectHandler : MonoBehaviour
{
    public bool hitWall;
    Vector2 touchPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitWall = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hitWall = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hitWall = true;
    }
}

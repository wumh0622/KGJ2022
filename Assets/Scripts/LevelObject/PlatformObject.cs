using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObject : MonoBehaviour
{
    public int defaultThroughLayerMask = 7;
    public int playerThroughLayerMask = 8;
    public float resetTime = .5f;

    bool through;
    PlatformEffector2D platformEffector;
    Collider2D collider;

    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
        collider = GetComponent<Collider2D>();
    }

    public void ThroughPlatform()
    {
        through = true;
        gameObject.layer = playerThroughLayerMask;
        collider.isTrigger = true;
        SimpleTimerManager.Instance.RunTimer(ResetPlatform, resetTime);
        //if (platformEffector)
        //{
        //    platformEffector.colliderMask = playerThroughLayerMask;
        //}

    }

    public void ResetPlatform()
    {
        through = false;
        gameObject.layer = defaultThroughLayerMask;
        collider.isTrigger = false;
        //if (platformEffector)
        //{
        //    platformEffector.colliderMask = defaultThroughLayerMask;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!through)
        {
            return;
        }
        if (collision.gameObject.GetComponent<PlayerCharacter>())
        {
            Debug.Log("EnterPlatform");
            SimpleTimerManager.Instance.StopTimer(ResetPlatform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(!through)
        {
            return;
        }
        if (collision.gameObject.GetComponent<PlayerCharacter>())
        {
            Debug.Log("ExitPlatform");
            SimpleTimerManager.Instance.RunTimer(ResetPlatform, resetTime);
        }
    }
}

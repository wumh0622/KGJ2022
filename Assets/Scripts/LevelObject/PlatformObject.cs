using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObject : MonoBehaviour
{
    public LayerMask defaultThroughLayerMask;
    public LayerMask playerThroughLayerMask;
    public float resetTime;

    bool through;
    PlatformEffector2D platformEffector;

    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    public void ThroughPlatform()
    {
        through = true;
        platformEffector.colliderMask = playerThroughLayerMask;
    }

    public void ResetPlatform()
    {
        through = false;
        platformEffector.colliderMask = defaultThroughLayerMask;
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

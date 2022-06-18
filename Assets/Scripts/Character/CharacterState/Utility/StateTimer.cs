using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTimer : MonoBehaviour
{
    public float stateTimer;
    
    public void RunStateTimer(System.Action onStateOver, float stateTimer)
    {
        StartCoroutine(StateTimerUpdate(onStateOver, stateTimer));
    }

    private IEnumerator StateTimerUpdate(System.Action onStateOver, float stateTimer)
    {
        yield return new WaitForSeconds(stateTimer);
        onStateOver();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTimerManager : Singleton<SimpleTimerManager>
{
    Dictionary<System.Action, IEnumerator> timerList = new Dictionary<System.Action, IEnumerator>();

    public void RunTimer(System.Action onStateOver ,float stateTimer)
    {
        if(timerList.ContainsKey(onStateOver))
        {
            return;
        }
        IEnumerator newTimer = StateTimerUpdate(onStateOver, stateTimer);
        timerList.Add(onStateOver, newTimer);
        StartCoroutine(newTimer);
    }

    public void StopTimer(System.Action onStateOver)
    {
        if (timerList.ContainsKey(onStateOver))
        {
            StopCoroutine(timerList[onStateOver]);
            timerList.Remove(onStateOver);
        }
    }

    private IEnumerator StateTimerUpdate(System.Action onStateOver, float stateTimer)
    {
        yield return new WaitForSeconds(stateTimer);

        if(timerList.ContainsKey(onStateOver))
        {
            timerList.Remove(onStateOver);
        }
        onStateOver();
    }
}

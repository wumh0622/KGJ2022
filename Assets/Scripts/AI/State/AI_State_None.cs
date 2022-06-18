using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_None : StateInterface
{
    public void StartPerform(AI_Data iData)
    {
        iData.mRenderer.gameObject.SetActive(true);
        iData.mEventRenderer.gameObject.SetActive(false);
        Debug.Log("���ANone");
    }
    public void Skip()
    { }
}

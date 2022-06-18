using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Angry :  StateInterface
{
    public void StartPerform(AI_Data iData)
    {
        iData.mRenderer.gameObject.SetActive(false);
        iData.mEventRenderer.gameObject.SetActive(true);
        iData.mEventRenderer.sprite = iData.mAngrySprite;
        Debug.Log("ª¬ºAAngry");
    }
    public void Skip()
    { }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Shy : StateInterface
{
    public void StartPerform(AI_Data iData)
    {
        iData.mRenderer.gameObject.SetActive(false);
        iData.mEventRenderer.gameObject.SetActive(true);
        iData.mEventRenderer.sprite = iData.mShySprite;        
        Debug.Log("ª¬ºAShy");
    }
    public void Skip()
    { }
}

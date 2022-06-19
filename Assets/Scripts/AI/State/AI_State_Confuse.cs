using UnityEngine;

public class AI_State_Confuse : StateInterface
{
    public void StartPerform(AI_Data iData)
    {
        iData.mRenderer.gameObject.SetActive(false);
        iData.mEventRenderer.gameObject.SetActive(true);
        iData.mEventRenderer.sprite = iData.mConfuseSprite;
        Debug.Log("ª¬ºAConfuse");
    }

    public void Skip()
    {
    }
}
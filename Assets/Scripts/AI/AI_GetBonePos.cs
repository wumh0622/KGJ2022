using UnityEngine;
using DG.Tweening;

public class AI_GetBonePos : MonoBehaviour
{
    public Transform mCenterBone = null;

    public float mMaxScal = 1.21f;
    public float mMinScal = 1.05f;

    private Tweener aTweener = null;

    public void CenterModifyScale(float iValue)
    {

        float aSetValue = mMaxScal * iValue;
        if (aSetValue >= mMaxScal)
        {
            aSetValue = mMaxScal;
        }
        else if (aSetValue < mMinScal)
        {
            aSetValue = mMinScal;
        }        
        if (aTweener == null)
        {
            aTweener = mCenterBone.DOScale(new Vector3(aSetValue, aSetValue, 1), 0.3f).SetLoops(2, LoopType.Yoyo).OnComplete(() => aTweener = null);
        }
    }
}
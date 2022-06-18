using UnityEngine;
using DG.Tweening;

public class AI_State_TopAtk : MonoBehaviour, StateInterface
{
    public Transform mEnemyPos = null;
    public Transform Pos_TopLeft = null;
    public Transform Pos_TopRight = null;

    private int mCurTopAtkIndex = 0;
    private AI_Data mData = null;

    public void StartPerform(AI_Data iData)
    {
        mData = iData;
        mCurTopAtkIndex = Random.Range(mData.mTopAtkMinCount, mData.mTopAtkMaxCount);
        TopAtkStart();
    }
    public void Skip()
    {
        Pos_TopLeft.DOKill();
        Pos_TopRight.DOKill();
        AtkFinish();
    }

    private void TopAtkStart()
    {
        if (mCurTopAtkIndex == 0)
        {
            AtkFinish();
            return;
        }

        if (mCurTopAtkIndex % 2 == 0)
        {
            TopAtk(Pos_TopLeft);
        }
        else
        {
            TopAtk(Pos_TopRight);
        }
        mCurTopAtkIndex--;
    }
    private void TopAtk(Transform iTopPos)
    {
        float aSpeed = Random.Range(mData.mMinTopAtkSpeed, mData.mMaxTopAtkSpeed);
        Quaternion aRot = Quaternion.FromToRotation(iTopPos.right, (mEnemyPos.position - iTopPos.position));
        Sequence mTopAtk = DOTween.Sequence();
        mTopAtk.SetAutoKill(false);
        mTopAtk.Append(iTopPos.DOLocalRotateQuaternion(aRot, 0.01f));
        mTopAtk.Append(iTopPos.DOMove(mEnemyPos.position, aSpeed));
        mTopAtk.OnComplete(() =>
        {
            mTopAtk.PlayBackwards();
        });
        mTopAtk.OnRewind(() =>
        {
            mTopAtk.Kill();
            TopAtkStart();
        });
    }
    private void AtkFinish()
    {
        MediatorManager<string>.Instance.Publish(AI_State.State_Nothing, this, null);
    }
}
using UnityEngine;
using DG.Tweening;

public class AI_State_TopAtk : MonoBehaviour, StateInterface
{
    public Transform Pos_TopLeft = null;
    public Transform Pos_TopRight = null;

    private int mCurTopAtkIndex = 0;
    private bool mIsHitPlayer = false;
    private bool mIsAtkFinish = false;
    private AI_Data mData = null;

    public void StartPerform(AI_Data iData)
    {
        mData = iData;
        mIsAtkFinish = false;
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

        mIsHitPlayer = false;
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
        Quaternion aRot = Quaternion.FromToRotation(iTopPos.right, (GameManager.Instance.player.transform.position - iTopPos.position));
        Sequence aTopAtk = DOTween.Sequence();
        aTopAtk.SetAutoKill(false);
        aTopAtk.Append(iTopPos.DOLocalRotateQuaternion(aRot, 0.01f));
        aTopAtk.Append(iTopPos.DOMove(GameManager.Instance.player.transform.position, aSpeed)).OnUpdate(() => 
        {
            PushPlayer(iTopPos);
        });
        aTopAtk.OnComplete(() =>
        {
            aTopAtk.PlayBackwards();
        });
        aTopAtk.OnRewind(() =>
        {
            aTopAtk.Kill();
            TopAtkStart();
        });
    }
    private void AtkFinish()
    {
        if (!mIsAtkFinish)
        {
            mIsAtkFinish = true;
            MediatorManager<string>.Instance.Publish(AI_State.State_Nothing, this, null);
            GameManager.Instance.player.transform.DOKill();
        }
    }

    private void PushPlayer(Transform iTopPos)
    {
        bool aIsHit = Physics2D.OverlapBox(iTopPos.position, mData.mTopCheckAtkPos, 0, mData.mAtkLayerMask);
        if (aIsHit && !mIsHitPlayer)
        {
            mIsHitPlayer = true;
            Vector3 aMovePos = (GameManager.Instance.player.transform.position - iTopPos.position).normalized * 2.5f;
            GameManager.Instance.player.transform.DOBlendableLocalMoveBy(aMovePos, 0.3f).SetEase(Ease.OutBounce);
            Debug.Log("TopAtk Hit =>  Player");
        }
    }
}
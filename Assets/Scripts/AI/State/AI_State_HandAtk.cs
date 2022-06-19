using UnityEngine;
using DG.Tweening;

public class AI_State_HandAtk : MonoBehaviour, StateInterface
{
    public Transform Pos_HandLeft = null;
    public Transform Pos_HandRight = null;
    [Header("位移位置")]
    public Transform Move_TopLeft = null;
    public Transform Move_DownLeft = null;
    public Transform Move_TopRight = null;
    public Transform Move_DownRight = null;


    private AI_Data mData = null;
    private bool mIsLeftAtk = false;
    private bool mIsAtkFinish = false;

    //public Transform shakePos;

    public void StartPerform(AI_Data iData)
    {
        mData = iData;
        mIsAtkFinish = false;
        mIsLeftAtk = Vector3.Cross(transform.forward, GameManager.Instance.player.transform.position).y < 0;
        if (mIsLeftAtk)
        {
            HandAtk(Pos_HandLeft, Move_TopLeft.position, Move_DownLeft.position);
        }
        else
        {
            HandAtk(Pos_HandRight, Move_TopRight.position, Move_DownRight.position);
        }
    }
    public void Skip()
    {
        Pos_HandLeft.DOComplete();
        Pos_HandLeft.DOKill();
        Pos_HandRight.DOComplete();
        Pos_HandRight.DOKill();
        AtkFinish();
    }

    private void HandAtk(Transform iMovePos, Vector3 iTopPos, Vector3 iDownPos)
    {
        Sequence aHandAtk = DOTween.Sequence();
        aHandAtk.SetAutoKill(false);
        aHandAtk.Append(iMovePos.DOMove(iTopPos, mData.mHandMoveToTopTime).SetEase(Ease.OutCirc));
        aHandAtk.Append(iMovePos.DOMove(iDownPos, mData.mHandMoveDownTime).SetEase(Ease.OutQuint));
        aHandAtk.OnComplete(() =>
        {
            if ((mIsLeftAtk && Vector3.Cross(transform.forward, GameManager.Instance.player.transform.position).y < 0) ||
            (!mIsLeftAtk && Vector3.Cross(transform.forward, GameManager.Instance.player.transform.position).y > 0))
            {
                if (GameManager.Instance.player.transform.transform.position.y  <= 2)
                {
                    GameManager.Instance.player.transform.transform.DOBlendableLocalMoveBy(new Vector3(mIsLeftAtk ? -2 : 2, 2.5f, 0), 0.3f).SetEase(Ease.OutBounce);
                    /*if (shakePos != null)
                    {
                        shakePos.DOShakePosition(0.3f);
                    }*/
                }
            }
            aHandAtk.PlayBackwards();
        });
        aHandAtk.OnRewind(() =>
        {
            aHandAtk.Kill();
            AtkFinish();

        });
    }
    private void AtkFinish()
    {
        if (!mIsAtkFinish)
        {
            mIsAtkFinish = true;
            MediatorManager<string>.Instance.Publish(AI_State.State_Nothing, this, null);
        }
    }
}
using UnityEngine;
using DG.Tweening;

public interface StateInterface
{
    void StartPerform(AI_Data iData);
    void Skip();
}

public class AI_Controller : MonoBehaviour
{
    public AI_Data mAIData { get; private set; } = null;
    public FreqCon.AudioPeer mAudioPeer = null;

    private StateInterface mCurState = null;
    private AI_State_None mState_None = new AI_State_None();
    private AI_State_Angry mState_Angry = new AI_State_Angry();
    private AI_State_Shy mState_Shy = new AI_State_Shy();
    private AI_State_Confuse mState_Confuse = new AI_State_Confuse();
    private AI_State_TopAtk mState_TopAtk = null;
    private AI_State_HandAtk mState_HandAtk = null;

    private float mCurAtkGap = 0;

    private void Awake()
    {
        Register();
    }
    private void Update()
    {
        if (mAIData != null && mCurState == mState_None && Time.time >= mCurAtkGap)
        {
            if (Random.Range(0, 100) < 50)
            {
                mCurState = mState_TopAtk;
                mState_TopAtk.StartPerform(mAIData);
            }
            else
            {
                mCurState = mState_HandAtk;
                mState_HandAtk.StartPerform(mAIData);
            }
        }
    }

    private void Register()
    {
        MediatorManager<AI_Data>.Instance.Subscribe(AI_State.SetAI, SetAIData);
        MediatorManager<string>.Instance.Subscribe(AI_State.State_Nothing, F_State_Nothing);
        MediatorManager<string>.Instance.Subscribe(AI_State.State_Angry, F_State_Angry);
        MediatorManager<string>.Instance.Subscribe(AI_State.State_Shy, F_State_Shy);
        MediatorManager<string>.Instance.Subscribe(AI_State.State_Confuse, F_State_Confuse);
    }

    private void StartPerform()
    {
        mCurState.StartPerform(mAIData);
    }


    private void F_State_Nothing(object iKey, MediatorArgs<string> iArgs)
    {
        mCurAtkGap = mAIData.GetNextATKDelay();
        SkipState();
        mCurState = mState_None;
        StartPerform();
    }
    private void F_State_Angry(object iKey, MediatorArgs<string> iArgs)
    {
        SkipState();
        mCurState = mState_Angry;
        StartPerform();
    }
    private void F_State_Shy(object iKey, MediatorArgs<string> iArgs)
    {
        SkipState();
        mCurState = mState_Shy;
        StartPerform();
    }
    private void F_State_Confuse(object iKey, MediatorArgs<string> iArgs)
    {
        SkipState();
        mCurState = mState_Confuse;
        StartPerform();
    }

    private void SkipState()
    {
        if (mCurState != null)
        {
            mCurState.Skip();
        }
    }

    /// <summary>
    /// 生成AI
    /// </summary>
    /// <param name="iKey">生成Key</param>
    /// <param name="iArgs">AI數據</param>
    private void SetAIData(object iKey, MediatorArgs<AI_Data> iArgs)
    {
        if (mAIData == iArgs.Args)
        {
            Debug.LogError("不可生成同角色");
            return;
        }

        if (mAIData != null)
        {
            //清除狀態
            GameObject aClearObj = mAIData.mAIRoot;
            StartCoroutine(mAIData.FadeOut(mAIData.mRenderer));
            Sequence aSeq = DOTween.Sequence();
            mAudioPeer.mAI_Data = null;
            aSeq.AppendInterval(2).OnComplete(() => DestroyImmediate(aClearObj));
        }
        mAIData = iArgs.Args;
        mAIData.CreatAI(transform);
        mAudioPeer.mAI_Data = mAIData;
        mState_TopAtk = mAIData.mAIRoot.GetComponentInChildren<AI_State_TopAtk>();
        mState_HandAtk = mAIData.mAIRoot.GetComponentInChildren<AI_State_HandAtk>();
        StartCoroutine(mAIData.FadeIn(mAIData.mRenderer));
        MediatorManager<string>.Instance.Publish(AI_State.State_Nothing, this, null);
    }

    private void OnDestroy()
    {
        MediatorManager<AI_Data>.Instance.ClearAllSubcribe();
        MediatorManager<string>.Instance.ClearAllSubcribe();
    }
}
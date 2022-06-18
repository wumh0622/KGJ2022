using System.Collections;
using System.Collections.Generic;
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
    
    private StateInterface mCurState = null;
    private AI_State_None mState_None = new AI_State_None();
    private AI_State_Angry mState_Angry = new AI_State_Angry();
    private AI_State_Shy mState_Shy = new AI_State_Shy();
    private AI_State_TopAtk mState_TopAtk = null;

    private float mCurAtkGap = 0;

    private void Awake()
    {
        Register();
    }
    private void Update()
    {
        if (mAIData != null && mCurState == mState_None && Time.time >= mCurAtkGap)
        {
            mCurState = mState_TopAtk;
            mState_TopAtk.StartPerform(mAIData);
            mCurAtkGap = mAIData.GetNextATKDelay();
        }
    }

    private void Register()
    {
        MediatorManager<AI_Data>.Instance.Subscribe(AI_State.SetAI, SetAIData);
        MediatorManager<string>.Instance.Subscribe(AI_State.State_Nothing, F_State_Nothing);
        MediatorManager<string>.Instance.Subscribe(AI_State.State_Angry, F_State_Angry);
        MediatorManager<string>.Instance.Subscribe(AI_State.State_Shy, F_State_Shy);
    }

    private void StartPerform()
    {
        mCurState.StartPerform(mAIData);
    }


    private void F_State_Nothing(object iKey, MediatorArgs<string> iArgs)
    {
        mCurState = mState_None;
        StartPerform();
    }
    private void F_State_Angry(object iKey, MediatorArgs<string> iArgs)
    {
        mCurState = mState_Angry;
        StartPerform();
    }
    private void F_State_Shy(object iKey, MediatorArgs<string> iArgs)
    {
        mCurState = mState_Shy;
        StartPerform();
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
            aSeq.AppendInterval(2).OnComplete(() => DestroyImmediate(aClearObj));
        }
        mAIData = iArgs.Args;
        mCurAtkGap = Time.time + mAIData.mStartAtkDelay;
        mAIData.CreatAI(transform);
        mState_TopAtk = mAIData.mAIRoot.GetComponentInChildren<AI_State_TopAtk>();
        StartCoroutine(mAIData.FadeIn(mAIData.mRenderer));
        MediatorManager<string>.Instance.Publish(AI_State.State_Nothing, this, null);
    }

    private void OnDestroy()
    {
        MediatorManager<AI_Data>.Instance.ClearAllSubcribe();
        MediatorManager<string>.Instance.ClearAllSubcribe();
    }
}
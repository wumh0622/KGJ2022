using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI_Character", menuName = "AI_Character/AI", order = 0)]
public class AI_Data : ScriptableObject
{
    [HideInInspector] public SpriteRenderer mRenderer = null;
    [HideInInspector] public SpriteRenderer mEventRenderer = null;
    [HideInInspector] public GameObject mAIRoot = null;
    public GameObject mAIObj = null;
    public Sprite mAngrySprite = null;
    public Sprite mShySprite = null;

    public float mFadeSpeed = 0.005f;

    [Header("ÀH¾÷§ðÀ»")]
    public float mStartAtkDelay = 3;
    public float mMaxAtkDelay = 5f;
    public float mMinAtkDelay = 2f;


    [Header("Top§ðÀ»")]
    public int mTopAtkMaxCount = 7;
    public int mTopAtkMinCount = 3;
    public float mMaxTopAtkSpeed = 0.5f;
    public float mMinTopAtkSpeed = 0.25f;

    public void InitData()
    {
        
    }
    public void CreatAI(Transform iParent)
    {
        mAIRoot = Instantiate(mAIObj, iParent);
        mRenderer = mAIRoot.GetComponentInChildren<SpriteRenderer>();
        InitData();
        GameObject aEventRender = new GameObject("EventRender");
        aEventRender.transform.SetParent(mAIRoot.transform);
        aEventRender.transform.localPosition = Vector3.zero;
        aEventRender.transform.localScale = Vector3.one;
        mEventRenderer = aEventRender.AddComponent<SpriteRenderer>();
        mEventRenderer.gameObject.SetActive(false);
    }

    public float GetNextATKDelay()
    {
        return Time.time + Random.Range(mMinAtkDelay, mMinAtkDelay);
    }

    public IEnumerator FadeOut(SpriteRenderer iRender)
    {
        Color mCacheColor = iRender.color;
        for (float i = 1; i >= -mFadeSpeed; i -= mFadeSpeed)
        {
            mCacheColor.a = i;
            iRender.color = mCacheColor;
            yield return new WaitForSeconds(mFadeSpeed);
        }
    }
    public IEnumerator FadeIn(SpriteRenderer iRender)
    {
        Color mCacheColor = iRender.color;
        for (float i = mFadeSpeed; i <= 1; i += mFadeSpeed)
        {
            mCacheColor.a = i;
            iRender.color = mCacheColor;
            yield return new WaitForSeconds(mFadeSpeed);
        }
    }
}

public class AI_State
{
    public const string SetAI = "SET_AI";
    public const string State_Nothing = "STATE_NOTHING";
    public const string State_Angry = "STATE_ANGRY";
    public const string State_Shy = "STATE_SHY";
    public const string State_TopAtk = "STATE_TOPATK";
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_ModifyManager : MonoBehaviour
{
    public List<Sprite> mBG = null;
    public float mModifyGap = 5;

    private bool mIsFinish = false;
    private SpriteRenderer mCurRenderer = null;    
    private Queue<Sprite> mRandomSprite = new Queue<Sprite>();

    private void Awake()
    {
        mCurRenderer = GetComponent<SpriteRenderer>();
        RandomSortList();
        mIsFinish = false;
        StartCoroutine(ModifyBG());
    }    

    private void RandomSortList()
    {
        System.Random aRandom = new System.Random();
        List<Sprite> newList = new List<Sprite>();
        foreach (Sprite item in mBG)
        {
            newList.Insert(aRandom.Next(newList.Count), item);
        }
        for (int i = 0; i < mBG.Count; i++)
        {
            mRandomSprite.Enqueue(mBG[i]);
        }
    }

    private IEnumerator ModifyBG()
    {
        while (!mIsFinish)
        {
            if (mRandomSprite.Count != 0)
            {
                mCurRenderer.sprite = mRandomSprite.Dequeue();
            }
            else
            {
                RandomSortList();
                mCurRenderer.sprite = mRandomSprite.Dequeue();
            }
            yield return new WaitForSeconds(mModifyGap);
        }       
    }
}
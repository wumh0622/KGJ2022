using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_Box : MonoBehaviour
{
    public string characterName;
    public string[] feedBack_What;
    public string[] feedBack_Normal;
    public string[] feedBack_Good;
    public string[] feedBack_Bad;

    public string[] feedBack_Win;
    public string[] feedBack_lose;
    Vector3 OGPOSITION;
    Vector3 ProcessPOSITION;
    Vector3 UnderscoreProcessPOSITION;
    public GameObject Underscore;

    List<GameObject> UnderscoreObjs = new List<GameObject>();
    float UnderscopeSizeX;
    int entriesCOUNTER = 0;
    int currentENTRIES = 0;

    public QG qg;

    List<dialog_object> getWords = new List<dialog_object>();

    TextMesh textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    // Start is called before the first frame update
    void Start()
    {
        qg = FindObjectOfType<QG>();
        OGPOSITION = this.transform.position;
        ProcessPOSITION = OGPOSITION;
        UnderscoreProcessPOSITION = OGPOSITION;

        GameObject prefab = Instantiate(Underscore, transform);
        prefab.active = false;
        UnderscoreObjs.Add(prefab);
        var _textMesh = prefab.GetComponentInChildren<TextMesh>();
        var _newTextMesh = _textMesh.GetComponent<MeshRenderer>();
        UnderscopeSizeX = _newTextMesh.bounds.size.x;

        //SetQuestion(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetQuestion(int entries)
    {
        ProcessPOSITION = OGPOSITION;
        UnderscoreProcessPOSITION = OGPOSITION;
        currentENTRIES = entries;
        while (entries > UnderscoreObjs.Count)
        {
            GameObject prefab = Instantiate(Underscore, transform);
            prefab.active = false;
            UnderscoreObjs.Add(prefab);
        }

        int index = 0;
        foreach(GameObject underline in UnderscoreObjs)
        {
            
            if (index < entries)
            {
                underline.active = true;
                underline.transform.position = UnderscoreProcessPOSITION;
                UnderscoreProcessPOSITION.x += UnderscopeSizeX + 0.5f;
            }
            else
            {
                underline.active = false;
            }
            index++;
        }
        UnderscoreProcessPOSITION = OGPOSITION;
        entriesCOUNTER = 0;
    }

    public void GetNewDialog(dialog_object newDialog)
    {
        getWords.Add(newDialog);
        newDialog.transform.position = new Vector3(ProcessPOSITION.x, ProcessPOSITION.y, ProcessPOSITION.z);

        var _textMesh = newDialog.GetComponentInChildren<TextMesh>();
        var _newTextMesh = _textMesh.GetComponent<MeshRenderer>();

        ProcessPOSITION.x += _newTextMesh.bounds.size.x + 0.5f;

        entriesCOUNTER += 1;


        UnderscoreProcessPOSITION.x = ProcessPOSITION.x;
        int index = 0;
        foreach (GameObject underline in UnderscoreObjs)
        {

            if (index < currentENTRIES)
            {
                if (index < entriesCOUNTER)
                {
                    underline.active = false;
                }
                else
                {
                    underline.transform.position = UnderscoreProcessPOSITION;
                    UnderscoreProcessPOSITION.x += UnderscopeSizeX + 0.5f;
                }
            }
            else
            {
                underline.active = false;
            }
            index++;
        }

        if (entriesCOUNTER >= currentENTRIES)
        {
            //do send message
        }
    }

    public bool HaveWord()
    {
        if (getWords.Count == 0)
        {
            return false;
            
        }
        return true;
    }

    public int CheckWord()
    {
        int currentKey = -1;
        int lastIdx = -1;
        int score = 0;
        bool isGood = true;
        if(getWords.Count == 0)
        {
            return 0;
        }

        foreach (dialog_object item in getWords)
        {
            isGood = item.isGood;
            if (item.isGood)
            {
                Entries newEnt = new Entries(item.impression, qg.GOOD_entriesData[item.Sentencekey].text);
                if(newEnt.size == 1)
                {
                    currentKey = item.Sentencekey;
                    score = item.impression;
                    break;
                }
            }
            else
            {
                Entries newEnt = new Entries(item.impression, qg.BAD_entriesData[item.Sentencekey].text);
                if (newEnt.size == 1)
                {
                    currentKey = item.Sentencekey;
                    score = item.impression;
                    break;
                }
            }

            if(currentKey < 0)
            {
                currentKey = item.Sentencekey;
            }
            else
            {
                if(item.Sentencekey != currentKey)
                {
                    score = 0;
                    break;
                }
            }

            if(lastIdx < 0)
            {
                lastIdx = item.entriesSequence;
            }
            else
            {
                if (lastIdx + 1 == item.entriesSequence)
                {
                    lastIdx = item.entriesSequence;
                    score = item.impression;
                }
                else
                {
                    score = 0;
                    break;
                }
            }
        }

        if (score != 0)
        {
            if (isGood)
            {

                Entries newEnt = new Entries(score, qg.GOOD_entriesData[currentKey].text);
                if (newEnt.size != getWords.Count)
                {
                    score = 0;
                }

            }
            else
            {
                Entries newEnt = new Entries(score, qg.BAD_entriesData[currentKey].text);
                if (newEnt.size != getWords.Count)
                {
                    score = 0;
                }
            }
        }
        getWords.Clear();

        return score;
    }

    public void ShowRespond(string state)
    {
        switch (state)
        {
            case AI_State.State_Angry:
                textMesh.text = characterName + feedBack_Bad[Random.Range(0, feedBack_Bad.Length)];
                break;
            case AI_State.State_Shy:
                textMesh.text = characterName + feedBack_Good[Random.Range(0, feedBack_Good.Length)];
                break;
            case AI_State.State_Confuse:
                textMesh.text = characterName + feedBack_What[Random.Range(0, feedBack_What.Length)];
                break;
            default:
                break;
        }
    }

    public string GetEndWord(bool win)
    {
        string result;
        if(win)
        {
            result = characterName + feedBack_Win[Random.Range(0, feedBack_Win.Length)];
        }
        else
        {
            result = characterName + feedBack_lose[Random.Range(0, feedBack_lose.Length)];
        }
        return result;
    }

    public void ClearRespond()
    {
        textMesh.text = "";
    }
}

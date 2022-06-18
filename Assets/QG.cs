using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QG : MonoBehaviour
{
    public GameObject DialogObj;
    Camera _MainCamera;
    GameObject DialogBox;

    List<Entries> GOOD_entries_LIST = new List<Entries>();
    List<GameObject> inGameDialogEntry = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _MainCamera = Camera.main;
        DialogBox = GameObject.Find("Dialog_Box");

        Entries ent1 = new Entries(30, "我/喜歡/你");
        GOOD_entries_LIST.Add(ent1);
        Entries ent2 = new Entries(30, "請和我/交往");
        GOOD_entries_LIST.Add(ent2);
        Entries ent3 = new Entries(90, "我/老爸/是/連OO");
        GOOD_entries_LIST.Add(ent3);
        Entries ent4 = new Entries(30, "我在/每個宇宙/都愛你。");
        GOOD_entries_LIST.Add(ent4);
        Entries ent5 = new Entries(30, "留下來/或者/我跟你走。");
        GOOD_entries_LIST.Add(ent5);
        Entries ent6 = new Entries(50, "我喜歡/你是Vtuber/但我更愛OOO");
        GOOD_entries_LIST.Add(ent6);
        Entries ent7 = new Entries(70, "比起河流/我更想變成天空/如果能變成天空/就能永遠看見你");
        GOOD_entries_LIST.Add(ent7);



        createQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createQuestion()
    {
        int itemIndex = Random.Range(0, GOOD_entries_LIST.Count);
        setEntryList(GOOD_entries_LIST[itemIndex]);
    }

    void setEntryList(Entries _entry)
    {
        int EntryCount = _entry.size;
        var _dialogBox = DialogBox.GetComponent<Dialog_Box>();
        _dialogBox.SetQuestion(EntryCount);

        while (EntryCount > inGameDialogEntry.Count)
        {
            GameObject prefab = Instantiate(DialogObj, transform);
            prefab.active = false;
            inGameDialogEntry.Add(prefab);
        }

        int index = 0;
        foreach (GameObject underline in inGameDialogEntry)
        {

            if (index < EntryCount)
            {
                underline.active = true;

                float spawnY = Random.Range
                    (_MainCamera.ScreenToWorldPoint(new Vector2(0, 0)).y, _MainCamera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
                float spawnX = Random.Range
                    (_MainCamera.ScreenToWorldPoint(new Vector2(0, 0)).x, _MainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, -0.01f);
                underline.transform.position = spawnPosition;

                var _dialogScript = inGameDialogEntry[index].GetComponent<dialog_object>();
                _dialogScript.SetDialog(_entry.entry[index], index, true);
            }
            else
            {
                underline.active = false;
            }
            index++;
        }
    }
}

class Entries
{
    int _entriesSIZE;
    int _impression = 0;
    List<string> _List_entries = new List<string>();

    public Entries(int impression, string entry)
    {
        this._impression = impression;
        string[] subs = entry.Split('/');
        foreach (var sub in subs)
        {
            this._List_entries.Add(sub);
        }
        this._entriesSIZE = this._List_entries.Count;
    }

    public int size
    {
        get { return _entriesSIZE; }
    }

    public int score
    {
        get { return _impression; }
    }

    public List<string> entry
    {
        get { return _List_entries; }
    }

}

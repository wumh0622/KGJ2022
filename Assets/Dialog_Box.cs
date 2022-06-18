using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_Box : MonoBehaviour
{
    Vector3 OGPOSITION;
    Vector3 ProcessPOSITION;
    Vector3 UnderscoreProcessPOSITION;
    public GameObject Underscore;

    List<GameObject> UnderscoreObjs = new List<GameObject>();
    float UnderscopeSizeX;
    int entriesCOUNTER = 0;
    int currentENTRIES = 0;
    // Start is called before the first frame update
    void Start()
    {
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

    public void GetNewDialog(GameObject newDialog)
    {
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
}

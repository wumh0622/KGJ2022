using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialog_object : MonoBehaviour
{
    public string DialogText;
    public GameObject DialogBox;
    public int impression = 0;
    bool isCollect = false;
    bool isTarget = false;
    int entriesSequence = 0;

    // Start is called before the first frame update
    void Start()
    {
        DialogBox = GameObject.Find("Dialog_Box");
        //StartCoroutine(ExampleCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDialog(string newText, int Sequence, bool _isTarget)
    {
        isCollect = false;
        entriesSequence = Sequence;
        isTarget = _isTarget;

        DialogText = newText;

        var _textMesh = GetComponentInChildren<TextMesh>();
        var _collider = GetComponentInChildren<BoxCollider2D>();
        var _newTextMesh = _textMesh.GetComponent<MeshRenderer>();

        _textMesh.text = newText;

        //_collider.offset = new Vector2(_newTextMesh.bounds.center.x,
        //                               _newTextMesh.bounds.center.y);
        _collider.size = new Vector2(_newTextMesh.bounds.size.x,
                                     _newTextMesh.bounds.size.y);
        //_collider.size = new Vector3(_newTextMeshBounds.size.x / transform.lossyScale.x,
        //                             _newTextMeshBounds.size.y / transform.lossyScale.y,
        //                             _newTextMeshBounds.size.z / transform.lossyScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isCollect)
        { 
            return;
        }

        isCollect = true;
        var _dialogBox = DialogBox.GetComponent<Dialog_Box>();
        _dialogBox.GetNewDialog(this.gameObject);
    }

    string GetDialog()
    {
        return DialogText;
    }

    public void newPosition(Vector3 newPosition)
    {
        this.transform.position = newPosition;
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        //SetDialog(DialogText);
    }
}

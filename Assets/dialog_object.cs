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
    public int entriesSequence = 0;
    Rigidbody2D rigidbody2D;
    TextMesh textMesh;

    Vector3 startPoint;

    public int Sentencekey;
    public bool isGood;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        DialogBox = GameObject.Find("Dialog_Box");
    }

    // Start is called before the first frame update
    void Start()
    {

        //StartCoroutine(ExampleCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDialog(string newText, int Sequence, bool _isTarget, int key, int score)
    {
        rigidbody2D.simulated = true;
        impression = score;
        Sentencekey = key;
        startPoint = transform.position;
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

    public void Collect()
    {
        if(isCollect)
        { 
            return;
        }
        GameManager.Instance.TryGetRandomAudioClip(GameManager.AudioKey.PickWord);
        rigidbody2D.simulated = false;
        isCollect = true;
        var _dialogBox = DialogBox.GetComponent<Dialog_Box>();
        _dialogBox.GetNewDialog(this);
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

    public void ReturnToStartPoint()
    {
        transform.position = startPoint;
    }

    public void Select(Color color)
    {
        textMesh.color = color;
    }
}

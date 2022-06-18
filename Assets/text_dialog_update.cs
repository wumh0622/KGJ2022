using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text_dialog_update : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExampleCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        yield return new WaitForSeconds(3.0f);
        BroadcastMessage("SetDialog", "THIS IS¡@¢Ü¢Ó¢å");
    }
}

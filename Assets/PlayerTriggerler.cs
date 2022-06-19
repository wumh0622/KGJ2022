using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerler : MonoBehaviour
{
    public ContactFilter2D filter2D;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(filter2D.IsFilteringLayerMask(collision.gameObject))
        {
            return;
        }

        dialog_object dialog = collision.gameObject.GetComponent<dialog_object>();
        if(dialog)
        {
            dialog.Collect();
        }

    }
}

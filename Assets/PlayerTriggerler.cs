using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerler : MonoBehaviour
{
    public ContactFilter2D filter2D;

    List<GameObject> selected = new List<GameObject>();

    public Color selectColor;
    public Color defaultColor;

    GameObject currentSelect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(selected.Count > 0)
        {
            float minDistance = float.MaxValue;
            GameObject result = null;
            if (selected.Count > 0)
            {
                foreach (var item in selected)
                {
                    if (Vector2.Distance(transform.position, item.transform.position) < minDistance)
                    {
                        result = item;
                        result.GetComponent<dialog_object>().Select(defaultColor);

                    }
                }
            }

            result.GetComponent<dialog_object>().Select(selectColor);
            currentSelect = result;
        }
    }

    public void InputCollect()
    {
        if(currentSelect)
        {
            currentSelect.GetComponent<dialog_object>().Collect();
            currentSelect.GetComponent<dialog_object>().Select(defaultColor);
            currentSelect = null;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(filter2D.IsFilteringLayerMask(collision.gameObject))
        {
            return;
        }

        dialog_object dialog = collision.gameObject.GetComponent<dialog_object>();
        selected.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (filter2D.IsFilteringLayerMask(collision.gameObject))
        {
            return;
        }
        if (selected.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<dialog_object>().Select(defaultColor);
            selected.Remove(collision.gameObject);
            if(currentSelect == collision.gameObject)
            {
                currentSelect = null;
            }
        }
    }
}

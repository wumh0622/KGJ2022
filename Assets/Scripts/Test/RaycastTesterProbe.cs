using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTesterProbe : MonoBehaviour
{

    public GameObject probe;
    public Vector2 size;
    // Start is called before the first frame update
    void Start()
    {
        for (int YIdx = 0; YIdx < size.y; YIdx++)
        {
            for (int XIdx = 0; XIdx < size.x; XIdx++)
            {
                Instantiate(probe, transform.position + new Vector3(0.5f * XIdx, 0.5f * YIdx, 0), Quaternion.identity);
            }
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

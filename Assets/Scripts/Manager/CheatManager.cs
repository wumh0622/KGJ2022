using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    bool slowDown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if (!slowDown)
            {
                slowDown = true;
                Time.timeScale = 0.3f;
            }
            else
            {
                slowDown = false;
                Time.timeScale = 1.0f;
            }

        }
    }
}

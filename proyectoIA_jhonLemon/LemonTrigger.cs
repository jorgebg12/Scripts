using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonTrigger : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        this.gameObject.transform.Rotate(0,0,60*Time.deltaTime);

        
    }

}

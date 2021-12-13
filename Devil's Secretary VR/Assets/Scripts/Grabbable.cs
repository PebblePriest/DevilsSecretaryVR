using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Hand
{
    public Transform handler;
    public void Update()
    {
        if(!isGrabbing)
        {
            transform.position = handler.transform.position;
            transform.rotation = handler.transform.rotation;
        }
    }
}

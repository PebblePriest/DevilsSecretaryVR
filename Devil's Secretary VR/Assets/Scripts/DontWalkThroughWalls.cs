using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontWalkThroughWalls : MonoBehaviour
{
    public Transform head;
    public Transform feet;

    public void Update()
    {
        gameObject.transform.position = new Vector3(head.position.x, feet.position.y, head.position.z);
    }
}

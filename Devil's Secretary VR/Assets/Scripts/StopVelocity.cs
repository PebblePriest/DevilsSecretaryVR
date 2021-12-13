using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopVelocity : MonoBehaviour
{
    public Rigidbody rb;
    public void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
            rb.velocity = Vector3.zero;
        
        
    }
    
}

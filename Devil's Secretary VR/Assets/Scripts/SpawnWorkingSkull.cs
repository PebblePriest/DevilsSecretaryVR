using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWorkingSkull : MonoBehaviour
{
    public GameObject workingSoul;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Soul"))
        {
            Destroy(other.gameObject);
            this.workingSoul.SetActive(true);

        }
    }
}

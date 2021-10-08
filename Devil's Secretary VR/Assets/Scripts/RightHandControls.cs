using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RightHandControls : MonoBehaviour
{
    public InputActionReference rightTrigger;

    public GameObject lightning;

    public void Start()
    {
        rightTrigger.action.performed += lightningStart;
        rightTrigger.action.canceled += lightningCancel;
        lightning.SetActive(false);

    }
    public void lightningStart(InputAction.CallbackContext context)
    {
        Debug.Log("Working");
        lightning.SetActive(true);
    }
    public void lightningCancel(InputAction.CallbackContext context)
    {
        Debug.Log("Lightning off");
        lightning.SetActive(false);
    }
}

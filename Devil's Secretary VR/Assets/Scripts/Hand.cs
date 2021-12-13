using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    ////Variables used for hand animation
    //Animator animator;
    //SkinnedMeshRenderer mesh;
    //private float triggerTarget;
    //private float gripTarget;
    //private float gripCurrent;
    //private float triggerCurrent;
    //public float speed;
    //private string animatorGripParam = "Grip";
    //private string animatorTriggerParam = "Trigger";

    //Physics Movement for hands
    [SerializeField] private ActionBasedController controller;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private Transform palm;
    [SerializeField] float reachDistance = 0.1f, joinDistance = 0.05f;
    [SerializeField] private LayerMask grabbableLayer;

    private Transform followTarget;
    private Rigidbody body;
    public bool isGrabbing;
    private GameObject heldObject;
    private Transform grabPoint;
    private FixedJoint joint1, joint2;

    public InputActionReference rightTrigger;
    public GameObject lightning;
    public GameObject glow;
    public GameObject unGlow;
    
    // Start is called before the first frame update
    void Start()
    {
        ////Animation
        //animator = GetComponent<Animator>();
        //mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        //Physics Movement
        followTarget = controller.gameObject.transform;
        body = GetComponent<Rigidbody>();
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.mass = 20f;
        body.maxAngularVelocity = 20f;

        //Input Setup
        controller.selectAction.action.started += Grab;
        controller.selectAction.action.canceled += Release;

        //Teleport hands to body
        body.position = followTarget.position;
        body.rotation = followTarget.rotation;

        lightning.SetActive(false);
        glow.SetActive(false);
        unGlow.SetActive(true);

        //Lightning Effect on Staff
        controller.activateAction.action.started += lightningStart;
        controller.activateAction.action.canceled += lightningCancel;

        
    }
    public void lightningStart(InputAction.CallbackContext context)
    {
        if (isGrabbing)
        {
            lightning.SetActive(true);
            glow.SetActive(true);
            unGlow.SetActive(false);
        }
       
    }
    public void lightningCancel(InputAction.CallbackContext context)
    {
        if (isGrabbing)
        {
            lightning.SetActive(false);
            glow.SetActive(false);
            unGlow.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       // AnimateHand();

        PhysicsMove();
    }

    private void PhysicsMove()
    {
        //Make Offset for position
        var positionWithOffset =followTarget.TransformPoint(positionOffset);
        var rotationWithOffset = followTarget.rotation * Quaternion.Euler(rotationOffset);
        //Position
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);
        //Rotation
        var q = rotationWithOffset * Quaternion.Inverse(body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
    }

    public void Grab(InputAction.CallbackContext context)
    {
        if (isGrabbing || heldObject) return;

        Collider[] grabbablecolliders = Physics.OverlapSphere(palm.position, reachDistance, grabbableLayer);
        if (grabbablecolliders.Length < 1) return;

        var objectToGrab = grabbablecolliders[0].transform.gameObject;

        var objectBody = objectToGrab.GetComponent<Rigidbody>();

        if (objectBody != null)
        {
            heldObject = objectBody.gameObject;
        }
        else
        {
            objectBody = objectToGrab.GetComponentInParent<Rigidbody>();
            if (objectBody != null)
            {
                heldObject = objectBody.gameObject;
            }
            else
            {
                return;
            }
        }
        StartCoroutine(GrabObject(grabbablecolliders[0], objectBody));
    }
    private IEnumerator GrabObject(Collider collider, Rigidbody targetBody)
    {
        isGrabbing = true;

        //Create a grab point
        grabPoint = new GameObject().transform;
        grabPoint.position = collider.ClosestPoint(palm.position);
        grabPoint.parent = heldObject.transform;

        //Move hand to grab point
        followTarget = grabPoint;

        //Wait for hand to reach grab point
        while(grabPoint != null && Vector3.Distance(grabPoint.position, palm.position)> joinDistance && isGrabbing)
        {
            yield return new WaitForEndOfFrame();
        }

        //Freeze hand and object motion
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        targetBody.velocity = Vector3.zero;
        targetBody.angularVelocity = Vector3.zero;

        targetBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        targetBody.interpolation = RigidbodyInterpolation.Interpolate;

        //Attach joints
        //Joint is for hand to Object
        joint1 = gameObject.AddComponent<FixedJoint>();
        joint1.connectedBody = targetBody;
        joint1.breakForce = float.PositiveInfinity;
        joint1.breakTorque = float.PositiveInfinity;

        joint1.connectedMassScale = 1;
        joint1.massScale = 1;
        //Disables Physics with object
        joint1.enableCollision = false;
        joint1.enablePreprocessing = false;

        //Joint is for object to hand
        joint2 = heldObject.AddComponent<FixedJoint>();
        joint2.connectedBody = body;
        joint2.breakForce = float.PositiveInfinity;
        joint2.breakTorque = float.PositiveInfinity;
             
        joint2.connectedMassScale = 1;
        joint2.massScale = 1;
        //Disables physics with hand
        joint2.enableCollision = false;
        joint2.enablePreprocessing = false;
        //Reset follow joints
        followTarget = controller.gameObject.transform;
    }
    private void Release(InputAction.CallbackContext context)
    {
        if(joint1 != null)
        {
            Destroy(joint1);
        }
        if (joint2 != null)
        {
            Destroy(joint2);
        }
        if(grabPoint != null)
        {
            Destroy(grabPoint.gameObject);
        }
        if(heldObject != null)
        {
            var targetBody = heldObject.GetComponent<Rigidbody>();
            targetBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            targetBody.interpolation = RigidbodyInterpolation.None;
            heldObject = null;
        }
        isGrabbing = false;
        followTarget = controller.gameObject.transform;
    }
}

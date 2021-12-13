using UnityEngine;
using System.Collections;


public class Float : MonoBehaviour
{
    // User Inputs
    public float objectspinspeed = 10.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 temporaryPosition = new Vector3();

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        //posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        posOffset = transform.position;
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * objectspinspeed, 0f), Space.World);

        // Float up/down with a Sin()
        temporaryPosition = posOffset;
        temporaryPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = temporaryPosition;
    }
}
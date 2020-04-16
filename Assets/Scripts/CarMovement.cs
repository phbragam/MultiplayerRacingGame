using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    Rigidbody rb;

    public Vector3 thrustForce = new Vector3(0f, 0f, 45f);
    public Vector3 rotationTorque = new Vector3(0f, 8f, 0f);

    public bool ControlsEnabled;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ControlsEnabled = false;
        //obs.: the order in spector matters, we need to put CarMovement over PlayerSetup (in car's prefabs), so, the Start method from CarMovement will be called first
        Debug.Log("Controls disabled");
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlsEnabled)
        {
            // moving forward
            if (Input.GetKey("w"))
            {
                rb.AddRelativeForce(thrustForce);
            }

            // moving backward
            if (Input.GetKey("s"))
            {
                rb.AddRelativeForce(-thrustForce);
            }

            // moving left
            if (Input.GetKey("a"))
            {
                rb.AddRelativeTorque(-rotationTorque);
            }

            // moving right
            if (Input.GetKey("d"))
            {
                rb.AddRelativeTorque(rotationTorque);
            }
        }
    }
}

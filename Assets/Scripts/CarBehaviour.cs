using UnityEngine;
using System.Collections;
using System.IO;
public class CarBehaviour : MonoBehaviour
{
public WheelCollider wheelColliderFL;
public WheelCollider wheelColliderFR;
public WheelCollider wheelColliderRL;
public WheelCollider wheelColliderRR;
public float maxTorque = 500;
public float maxSteerAngle = 45;
public float sidewaysStiffness = 1.5f;
public float forewardStiffness = 1.5f;
public Transform centerOfMass;
private Rigidbody _rigidbody;
private float _currentSpeedKMH;
public float maxSpeedKMH = 150;
public float maxSpeedBackwardKMH = 30;
void Start () {
    SetWheelFrictionStiffness(forewardStiffness, sidewaysStiffness);
    _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = new Vector3(centerOfMass.localPosition.x,
        centerOfMass.localPosition.y,
        centerOfMass.localPosition.z);
}
void FixedUpdate ()
{
        // Determine if the car is driving forwards or backwards
        bool velocityIsForeward = Vector3.Angle(transform.forward,
         _rigidbody.velocity) < 50f;
        // get the current speed from the velocity vector
        _currentSpeedKMH = _rigidbody.velocity.magnitude * 3.6f;


        if (velocityIsForeward && _currentSpeedKMH < maxSpeedKMH)
        {
            SetMotorTorque(maxTorque * Input.GetAxis("Vertical"));
        }
        else if (!velocityIsForeward && _currentSpeedKMH < maxSpeedBackwardKMH)
        {
            SetMotorTorque(maxTorque * Input.GetAxis("Vertical"));
        }
        else
        {
            SetMotorTorque(0);
        }

        SetSteerAngle(maxSteerAngle * Input.GetAxis("Horizontal"));

        // Determine if the cursor key input means braking
        bool doBraking = _currentSpeedKMH > 0.5f &&
        (Input.GetAxis("Vertical") < 0 && velocityIsForeward ||
        Input.GetAxis("Vertical") > 0 && !velocityIsForeward);
        if (doBraking)
        {
            wheelColliderFL.brakeTorque = 5000;
            wheelColliderFR.brakeTorque = 5000;
            wheelColliderRL.brakeTorque = 5000;
            wheelColliderRR.brakeTorque = 5000;
            wheelColliderFL.motorTorque = 0;
            wheelColliderFR.motorTorque = 0;
        }
        else
        {
            wheelColliderFL.brakeTorque = 0;
            wheelColliderFR.brakeTorque = 0;
            wheelColliderRL.brakeTorque = 0;
            wheelColliderRR.brakeTorque = 0;
        }
    }
void SetSteerAngle(float angle)
{ wheelColliderFL.steerAngle = angle;
wheelColliderFR.steerAngle = angle;
}
void SetMotorTorque(float amount)
{ wheelColliderFL.motorTorque = amount;
wheelColliderFR.motorTorque = amount;
}

void SetWheelFrictionStiffness(float newForwardStiffness, float newSidewaysStiffness)
{
    WheelFrictionCurve fwWFC = wheelColliderFL.forwardFriction;
    WheelFrictionCurve swWFC = wheelColliderFL.sidewaysFriction;
    fwWFC.stiffness = newForwardStiffness;
    swWFC.stiffness = newSidewaysStiffness;
    wheelColliderFL.forwardFriction = fwWFC;
    wheelColliderFL.sidewaysFriction = swWFC;
    wheelColliderFR.forwardFriction = fwWFC;
    wheelColliderFR.sidewaysFriction = swWFC;
    wheelColliderRL.forwardFriction = fwWFC;
    wheelColliderRL.sidewaysFriction = swWFC;
    wheelColliderRR.forwardFriction = fwWFC;
    wheelColliderRR.sidewaysFriction = swWFC;
}
}
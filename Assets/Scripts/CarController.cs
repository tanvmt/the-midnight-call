using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Car Properties")]
    public float motorTorque = 2000f;
    public float brakeTorque = 2000f;
    public float idleBrakeTorque = 500f;
    public float maxSpeed = 20f;
    public float steeringRange = 30f;
    public float steeringRangeAtMaxSpeed = 10f;
    public float centreOfGravityOffset = -1f;

    [Header("Visuals")]
    public Transform steeringWheelTransform;
    public float maxSteeringWheelAngle = 45f;

    private WheelControl[] wheels;
    private Rigidbody rigidBody;

    private CarInputActions carControls;

    void Awake() {
        carControls = new CarInputActions();
    }

    void OnEnable() {
        carControls.Enable();
    }

    void OnDisable() {
        carControls.Disable();
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        Vector3 centerOfMass = rigidBody.centerOfMass;
        centerOfMass.y += centreOfGravityOffset;
        rigidBody.centerOfMass = centerOfMass;

        wheels = GetComponentsInChildren<WheelControl>();

    }

    void FixedUpdate()
    {
        Vector2 inputVector = carControls.Car.Movement.ReadValue<Vector2>();
        float vInput = inputVector.y;
        float hInput = inputVector.x;

        if (steeringWheelTransform != null)
        {
            float currentSteeringWheelAngle = hInput * maxSteeringWheelAngle;
            steeringWheelTransform.localRotation = Quaternion.Euler(0f, currentSteeringWheelAngle, 0f);
        }

        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        float speedFactor = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / maxSpeed);

        float currentSteeringRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteeringRange;
            }
        }

        if (Mathf.Abs(vInput) > 0)
        {
            float currentMotorTorque = motorTorque;
            if (Mathf.Sign(vInput) != Mathf.Sign(forwardSpeed) && forwardSpeed != 0)
            {
                foreach (var wheel in wheels)
                {
                    wheel.WheelCollider.motorTorque = 0f;
                    wheel.WheelCollider.brakeTorque = brakeTorque;
                }
            }
            else
            {
                foreach (var wheel in wheels)
                {
                    if (wheel.motorized)
                    {
                        wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                    }
                    wheel.WheelCollider.brakeTorque = 0f;
                }
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = idleBrakeTorque;
            }
        }
    }
}

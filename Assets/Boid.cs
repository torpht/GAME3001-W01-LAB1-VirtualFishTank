using System.Numerics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Vector3 = UnityEngine.Vector3;

public class Boid : MonoBehaviour
{
    private GameObject targetObject;
    public Rigidbody rigidBody;
    public bool arrived = false;

    public float speedMax = 4f;
    public float accelMax = 3f;

    private void Start()
    {
        targetObject = GameObject.Find("target"); //finds target and assigns to variable
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Draw velocity
        Debug.DrawRay(transform.position, transform.position + rigidBody.linearVelocity.normalized, Color.red);
    }
    private void FixedUpdate()
    {
        //enforce a top speed
        Vector3 vel = rigidBody.linearVelocity;
        float speed = vel.magnitude;

        if (speed > speedMax)
            { vel = vel.normalized * speedMax; }

        rigidBody.linearVelocity = vel;

        //orient towards velocity
        transform.forward = rigidBody.linearVelocity;
    }

    public Vector3 Seek(Vector3 target, float acceleration)
    {
        //Displacement Vector to Target
        Vector3 toTarget = target - transform.position;

        //Normalize
        Vector3 toTargetNormalized = toTarget.normalized;

        //Determine Acceleration
        Vector3 accel = toTargetNormalized * acceleration;

        return accel;
    }

    public Vector3 Pursue(Vector3 target, float acceleration, float desiredSpeed)
    {
        //Displacement Vector to Target
        Vector3 toTarget = target - transform.position;

        //Normalize
        Vector3 toTargetNormalized = toTarget.normalized;

        //Determine Velocity
        Vector3 desiredVelocity = toTargetNormalized * desiredSpeed;

        //Determine change in velocity
        Vector3 deltaVel = desiredVelocity - rigidBody.linearVelocity;

        Vector3 accel = deltaVel.normalized * acceleration;
        return accel;
    }

    public Vector3 Arrive(Vector3 target, float acceleration, bool arrive = false)
    {
        //Displacement to target
        Vector3 toTarget = target - transform.position;

        //normalize
        Vector3 toTargetNormalized = toTarget.normalized;

        //Determine Acceleration
        Vector3 accel = toTargetNormalized * acceleration;

        while (arrive)
        { return Vector3.zero;}

        return accel;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            arrived = true;
            Debug.Log("Target Collision True");
        }
        else if (collision.gameObject.tag != "Player")
        {
            arrived = false;
            Debug.Log("Target Collision False");
        }

    }
}

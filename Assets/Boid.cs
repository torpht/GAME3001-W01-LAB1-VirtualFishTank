using UnityEngine;

public class Boid : MonoBehaviour
{
    private GameObject targetObject;
    private Rigidbody rigidBody;

    public float speedMax;
    public float accelMax = 3f;

    private void Start()
    {
        targetObject = GameObject.Find("target");
        rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //get displacement vector to target
        Vector3 toTarget = targetObject.transform.position - transform.position;

        //normalize
        Vector3 toTargetNormalized = toTarget.normalized;
        //determine acceleration
        Vector3 acceleration = toTargetNormalized * accelMax;
        
        rigidBody.linearVelocity -= acceleration * Time.fixedDeltaTime;

        //enforce a top speed
        Vector3 vel = rigidBody.linearVelocity;
        float speed = vel.magnitude;

        if (speed > speedMax)
            { vel = vel.normalized * speedMax; }

        rigidBody.linearVelocity = vel;

        //orient towards velocity
        transform.forward = rigidBody.linearVelocity;
    }
}

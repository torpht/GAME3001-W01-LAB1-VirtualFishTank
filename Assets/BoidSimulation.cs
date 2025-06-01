using UnityEngine;
using System.Collections.Generic;

public class BoidSimulationControl : MonoBehaviour
{
    //To select/switch what the mouse buttons do for Assignment 1
    public enum ControlMode
    {
        Seek,
        Pursue,
        Food,
        Arrive
    }

    //Which mode we are in (selects what mouse buttons do)
    public ControlMode controlMode = ControlMode.Seek;
    
    public GameObject boidPrefab = null;
    public GameObject foodPrefab = null;
    private GameObject targetObject = null;
    public int numBoidsToSpawn = 10;
    public List<Boid> boids = null;

    private void Start()
    {
        targetObject = GameObject.Find("target"); //finds target and assigns to variable


        for (int i = 0; i <= numBoidsToSpawn; i++)
        {
            Vector3 position = new Vector3(Random.Range(-1.4f, 1.4f), Random.Range(0f, 1.4f), Random.Range(-0.9f, 0.9f));
            Quaternion rotation = Random.rotation;

            //Spawn Boids
            GameObject spawnedBoid = Instantiate(boidPrefab, position, rotation);
            /*Random Colour*/spawnedBoid.GetComponent<Renderer>().material.SetColor("_BaseColor", Random.ColorHSV(0f, 1f, 0.5f, 1f));
            /*Random Size*/spawnedBoid.transform.localScale *= Random.Range(0.9f, 3f);
            Boid boidComponent = spawnedBoid.GetComponent<Boid>();
            /*Random Speed*/boidComponent.speedMax = Random.Range(0.5f, 1.5f);
            /*Random Acceleration*/boidComponent.accelMax = Random.Range(0.5f, 1.5f);
            boids.Add(spawnedBoid.GetComponent<Boid>());
        }
    }

    private void Update()
    {
        //
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { controlMode = ControlMode.Seek; }
        //
        if (Input.GetKeyDown(KeyCode.Alpha2))
        { controlMode = ControlMode.Pursue; }
        //
        if (Input.GetKeyDown(KeyCode.Alpha3))
        { controlMode = ControlMode.Food; }
        //
        if (Input.GetMouseButtonDown(0) && controlMode == ControlMode.Food)
        { SpawnFood(); }
        //
        if (Input.GetKeyDown(KeyCode.Alpha4))
        { controlMode = ControlMode.Arrive; }
        
        
    }
    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool didHit = Physics.Raycast(ray, out hitInfo);
        //Problem with targetObject flying directly towards camera constantly; fixed by setting originalY to not change
        float originalY = targetObject.transform.position.y;
        
        if (didHit)
        {
            Vector3 newPosition = hitInfo.point;
            newPosition.y = originalY; // keeps Y position the same
            targetObject.transform.position = newPosition;
        }

        switch (controlMode)
        {
            case ControlMode.Seek:
                SeekModeControl();
                break;
            case ControlMode.Pursue:
                PursueModeControl();
                break;
            case ControlMode.Food:
                FoodModeControl();
                break;
            case ControlMode.Arrive:
                ArriveModeControl();
                break;
        }

    }

    private void SeekModeControl()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 accel = boids[i].Seek(targetObject.transform.position, boids[i].accelMax);

            if (Input.GetMouseButton(0))//hold left click
            {
                boids[i].rigidBody.linearVelocity += accel * Time.fixedDeltaTime;
                Debug.DrawRay(boids[i].transform.position, accel, Color.green);
            }
            else if (Input.GetMouseButton(1))//hold right click
            {
                boids[i].rigidBody.linearVelocity -= 2 * accel * Time.fixedDeltaTime; //*2 to stop it being slower than seek
                Debug.DrawRay(boids[i].transform.position, accel, Color.green);
            }

        }
    }

    private void PursueModeControl()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 accel = boids[i].Pursue(targetObject.transform.position, boids[i].accelMax, boids[i].speedMax);
            if (Input.GetMouseButton(0))//hold left click
            {
                boids[i].rigidBody.linearVelocity += accel * Time.fixedDeltaTime;
                Debug.DrawRay(boids[i].transform.position, accel, Color.green);
            }
            else if (Input.GetMouseButton(1))//hold right click
            {
                boids[i].rigidBody.linearVelocity -= accel * Time.fixedDeltaTime; //*2 to stop it being slower than seek
                Debug.DrawRay(boids[i].transform.position, accel, Color.green);
            }
        }
    }

    private void FoodModeControl()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            float foodSeekRadius = 0.5f;
            Collider[] colliders = Physics.OverlapSphere(boids[i].transform.position, foodSeekRadius);
            foreach (Collider collider in colliders)
            {
                Food food = collider.GetComponent<Food>();
                if (food != null)
                {
                    Vector3 accel = boids[i].Arrive(collider.transform.position, boids[i].accelMax, food.arrived);
                    boids[i].rigidBody.linearVelocity += accel * Time.fixedDeltaTime;
                    Debug.DrawRay(boids[i].transform.position, accel, Color.green);
                }

            }
        }
    }

    private void ArriveModeControl()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 accel = boids[i].Arrive(targetObject.transform.position, boids[i].accelMax, boids[i].arrived);
            boids[i].rigidBody.linearVelocity += accel * Time.fixedDeltaTime;
            Debug.DrawRay(boids[i].transform.position, accel, Color.green);
        }
    }

    private void SpawnFood()
    {
        Instantiate(foodPrefab, targetObject.transform.position, Random.rotation);
    }
}

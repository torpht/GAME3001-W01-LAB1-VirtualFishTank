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
        Obstacle
    }

    //Which mode we are in (selects what mouse buttons do)
    public ControlMode controlMode = ControlMode.Seek;

    public GameObject boidPrefab = null;
    public int numBoidsToSpawn = 10;
    public List<Boid> boids = null;

    private void Start()
    {
        for (int i = 0; i <= numBoidsToSpawn; i++)
        {
            Vector3 position = new Vector3(Random.Range(-1.4f, 1.4f), Random.Range(0f, 1.4f), Random.Range(-0.9f, 0.9f));
            Quaternion rotation = Random.rotation;

            //Spawn Boids
            GameObject spawnedBoid = Instantiate(boidPrefab, position, rotation);
            boids.Add(spawnedBoid.GetComponent<Boid>());
            /*Random Colour*/spawnedBoid.GetComponent<Renderer>().material.SetColor("_BaseColor", Random.ColorHSV(0f, 1f, 0.5f, 1f));
            /*Random Size*/spawnedBoid.transform.localScale *= Random.Range(0.9f, 3f); 
        }        
    }

    private void Update()
    {
        //
        if (Input.GetKeyDown(KeyCode.Alpha1))
            { controlMode = ControlMode.Seek;  }
        //
        if (Input.GetKeyDown(KeyCode.Alpha2))
            { controlMode = ControlMode.Pursue; }
        //
        if (Input.GetKeyDown(KeyCode.Alpha3))
            { controlMode = ControlMode.Food; }
        //
        if (Input.GetKeyDown(KeyCode.Alpha4))
            { controlMode = ControlMode.Obstacle; }
    }
}

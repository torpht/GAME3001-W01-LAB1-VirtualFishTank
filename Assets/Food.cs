using UnityEngine;

public class Food : MonoBehaviour
{
    public bool arrived = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "boid")
        {
            arrived = true;
            Destroy(gameObject, 0.2f);
            Debug.Log("Boid Collision True");
        }
        else if (collision.gameObject.tag != "boid")
        {
            arrived = false;
            Debug.Log("Boid Collision False");
        }

    }
}

using UnityEngine;
using System.Collections;

public class FlareControler : MonoBehaviour {
    public Light flare;

    public bool hitTheFloor = false;

	// Use this for initialization
	void Start () {
        var locVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        locVel.y = -10f;
        locVel.z = 50f;
        GetComponent<Rigidbody>().velocity = transform.TransformDirection(locVel);
	}

    // collision
    public void handleCollision(GameObject collider, bool enter = false)
    {
        if (collider.name == "Terrain")
        {
            var locVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
            locVel.z = 0f;
            locVel.y = 0f;
            GetComponent<Rigidbody>().useGravity = false;
            hitTheFloor = true;
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(locVel);
        }        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " + other.gameObject.name);
        handleCollision(other.gameObject, true);
    }

    // collision
    void OnTriggerStay(Collider other)
    {
        handleCollision(other.gameObject);
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        var locVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        if (hitTheFloor)
            flare.intensity -= flare.intensity / 200f;

        if (flare.intensity <= 0.2f)
        {
            Destroy(gameObject);
        }
	}
}

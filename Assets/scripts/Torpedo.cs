using UnityEngine;
using System.Collections;

public class Torpedo : MonoBehaviour 
{
    public GameObject m_bubbles;

    public AudioSource m_audioSource;
    public GameObject explosion;
    public AudioSource m_explosionSound;

    float m_ignFactor = 1.05f;

    public bool IsEnemy = false;

	// Use this for initialization
	void Start () 
    {
        ignite();
        m_audioSource.Play();
	}
	
    // collision
    void OnTriggerEnter(Collider other)
    {
		expolde (other);
    }

	void OnTriggerStay(Collider other)
	{
		expolde (other);
	}

	void OnTriggerExit(Collider other)
	{
		expolde (other);
	}

	void expolde(Collider other)
	{
		if (other.name != "Player" && other.tag != "Torpedo")
		{
			explosion.SetActive(true);
			explosion.transform.parent = null;
			m_explosionSound.Play();
			m_explosionSound.transform.parent = null;
			Destroy(gameObject);
		}
	}
	// Update is called once per frame
	void FixedUpdate () 
    {
        var locVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        
        locVel.z *= m_ignFactor;
        if (locVel.z < -300.0f)
            locVel.z = -300.0f;

        GetComponent<Rigidbody>().velocity = transform.TransformDirection(locVel); 
	}

    // fire torpedo
    void ignite()
    {
        //m_bubbles.particleSystem.Play();

        var locVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);

        locVel.z = -10.0f;

        GetComponent<Rigidbody>().velocity = transform.TransformDirection(locVel); 
    }
}

using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;

    void OnTriggerEnter(Collider other)
    {
        Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
        //if (other.tag == "Enemy")
        //{
        //    Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
        //}
        //Destroy(other.gameObject);
        Destroy(gameObject);
    }
}

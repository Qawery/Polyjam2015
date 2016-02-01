using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public Transform Player;
    float MoveSpeed = 20;
    float MaxDist = 350;
    float MinDist = 120;
    float AlertDist = 600;

    public AudioSource ExplosionSound;

    bool IsAlive = true;

	// Use this for initialization
	void Start () {
	
	}

    public GameObject explosion;

    void OnTriggerEnter(Collider other)
    {
        if (IsAlive)
        {
            explosion.SetActive(true);
            explosion.transform.parent = null;
            ExplosionSound.Play();
            ExplosionSound.transform.parent = null;
            Destroy(gameObject, 2.5f);
            if (other.name == "Player")
            {
                GameVariables.score += 30;
            }
            else if (other.name != "Terrain") GameVariables.score += 50;
        }

        IsAlive = false;
    }

    void KillAllHumanz()
    {

    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, Player.position) <= AlertDist)
        {
            transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, transform.position.z));

            if (Vector3.Distance(transform.position, Player.position) >= MinDist)
            {

                transform.position += transform.forward * MoveSpeed * Time.deltaTime;



                if (Vector3.Distance(transform.position, Player.position) <= MaxDist)
                {
                    KillAllHumanz();
                }

            }
        }
	
	}
}

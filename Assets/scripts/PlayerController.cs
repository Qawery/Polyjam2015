using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    float m_sensitivity = 2.0f; // mouse rotation sensitivity
    
    float m_maxSpeed = 600.0f;
    
    float m_stopFactor = 0.92f; // 0.0 <-> 1.0

    public static float m_thrustFactor = 0.0f; // 0.0 <-> 1.0

    bool bMoving = false; // if player is moving

    bool bPickingUp = false;
    bool bGoDown = true;
    int dockingDelay = 0;
    float m_maxPickingSpeed = 400.0f;
    float m_pickingSpeedFactor = 0.5f;
    float m_pickingSpeed = 0.0f;

    float m_initialHeight = 0.0f;

    bool bLoadingThrusters = false; // player is increasing speed value (only if bMoving is false)

    public GameObject Bubbles_left;
    public GameObject Bubbles_right;
    public GameObject flare;

    public GameObject ThrustLight_left;
    public GameObject ThrustLight_right;

    public GameObject PickUpLight_left;
    public GameObject PickUpLight_right;

    public GameObject torpedo1_left;  // dummy
    public GameObject torpedo2_left;  // dummy

    public GameObject torpedo1_right; // dummy
    public GameObject torpedo2_right; // dummy

    public AudioSource m_torpedoReloadSound;
    public AudioSource m_pickUpSound;
    public AudioSource m_flareLaunchSound;
    public AudioSource m_engineSound;

    bool engineSoundPLayed = false;

    public bool IsFallingBecauseWeFuckinDied = true;

    public GameObject torpedoLive; // torpedo projectile model
    Vector3 m_torpedoScale; // initial scale of dummy and real torpedos
    float m_torpedoScale_z = 0.1f; // scale of dummy rockets
    float m_torpedoScaleFactor = 1.1f; // how fast torpedos are reloading
    bool bReload = false; // reload button set reloading state
    bool bReloaded = true; // torpedos were reloaded

    public enum TORPEDO_ORDER
    {
        T_NONE = 0,
        T1_LEFT,
        T1_RIGHT,

        T2_LEFT,
        T2_RIGHT
    };

    public static TORPEDO_ORDER m_currentTorpedo = TORPEDO_ORDER.T2_RIGHT;


	// Use this for initialization
	void Start () 
    {
        m_initialHeight = GetComponent<Rigidbody>().position.y;
        m_torpedoScale = torpedo1_left.transform.localScale;
	}


    void pickUPItem()
    {

    }

    // collision
    void handleCollision(GameObject collider, bool enter = false)
    {
        bool isAnObstacle = true;

        if (collider.name == "O2Tank") 
            isAnObstacle = false;

        if (isAnObstacle == true)
        {
            // going down while !colliding
            if (bPickingUp == true)
            {
                if (bGoDown == true)
                {
                    m_pickingSpeed = 0.0f;
                    bGoDown = false;
                }            
            }
            else
            {
                var locVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);

                if (enter && GameVariables.life > 0)
                    GameVariables.life -= (int)(30.0f * (locVel.z / m_maxSpeed)) * (GameVariables.isAlive ? 1 : 30);

                if (collider.name == "vehicle_enemyShip")
                {
                    GameVariables.life -= (int)(10.0f * (locVel.z / m_maxSpeed));
                }

                if (GameVariables.isAlive)
                    locVel.z = -30.0f;
                else
                {
                    locVel.z = 0f;
                    locVel.y = 0f;
                    locVel.x = 0f;
                    IsFallingBecauseWeFuckinDied = false;
                }

                GetComponent<Rigidbody>().velocity = transform.TransformDirection(locVel);
                bMoving = true;
            }
        }

        if (collider.name == "O2Tank")
        {
            if (bPickingUp == true)
            {
                if (bGoDown == true)
                {
                    m_pickingSpeed = 0.0f;
                    bGoDown = false;
                }
            }
            m_pickUpSound.Play();
            DestroyObject(collider);
            GameVariables.oxygenState += 3000;
            if (GameVariables.oxygenState > GameVariables.oxygenMax)
            {
                GameVariables.oxygenState = GameVariables.oxygenMax;
            }
            GameVariables.score += 20;
        }

        if (collider.name == "space_station_4")
        {
            AutoFade.LoadLevel("DeathScene", 1, (float)0.2, Color.black);
            GameVariables.gameFinished = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        handleCollision(other.gameObject, true);
    }

    // collision
    void OnTriggerStay(Collider other)
    {
        //handleCollision(other.gameObject);
    }

    void SpawnFlare()
    {
        Vector3 pos = transform.position;
        pos.y -= 10;
        Instantiate((GameObject)flare, pos, transform.rotation);
        m_flareLaunchSound.Play();
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (dockingDelay > 0)
        {
            dockingDelay--;
        }
        var locVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        var worldPos = GetComponent<Rigidbody>().position;

        // falling when controls are dead
        GetComponent<Rigidbody>().useGravity = !GameVariables.isAlive && GameVariables.life > 0 && IsFallingBecauseWeFuckinDied;

        // quick fix to let us die (finally)
        if (!IsFallingBecauseWeFuckinDied)
            GameVariables.life -= 1;


        // if player is not moving and not picking items
        if (GameVariables.isAlive && bPickingUp == false)
        {
            // rotate to mouse
            float mouse_position_x = Input.GetAxis("Mouse X");
            transform.Rotate(0, mouse_position_x * m_sensitivity, 0);

            // if user pressed  mouse button 
            if (Input.GetMouseButtonDown(0) == true)
            {
                bLoadingThrusters = true;
            }
            else if (Input.GetMouseButtonDown(2) || Input.GetKeyDown("space"))
            {
                dockingDelay = 60;
                bPickingUp = true;
                PickUpLight_left.SetActive(true);
                PickUpLight_right.SetActive(true);
            }
            else if(Input.GetMouseButton(0) == true && bLoadingThrusters == true) // if user is holding LMB and thrusters was set on
            {
                m_thrustFactor += 0.04f;
                if (m_thrustFactor >= 1.0f && engineSoundPLayed == false)
                {
                    m_engineSound.Play();
                    engineSoundPLayed = true;
                }
                if (m_thrustFactor > 2.0f)
                    m_thrustFactor = 2.0f;

                ThrustLight_left.GetComponent<Light>().intensity = 1 + 7 * m_thrustFactor;
                ThrustLight_right.GetComponent<Light>().intensity = 1 + 7 * m_thrustFactor;
            }
            else if(Input.GetMouseButton(0) == false && bLoadingThrusters == true) // if user is no longer holding LMB 
            {
                bMoving = true;
                bLoadingThrusters = false;

                Bubbles_left.GetComponent<ParticleSystem>().Play();
                Bubbles_right.GetComponent<ParticleSystem>().Play();

                ThrustLight_left.GetComponent<Light>().intensity = 1.0f;
                ThrustLight_right.GetComponent<Light>().intensity = 1.0f;

              
                locVel.z = m_maxSpeed*m_thrustFactor;
                //rigidbody.velocity = transform.TransformDirection(locVel);

                m_thrustFactor = 0.0f;
                engineSoundPLayed = false;
            }
            else if (Input.GetMouseButtonDown(1) == true)
            {
                ejectTorpedo();
            }
            else if (Input.GetKeyDown("r") && bReloaded == false)
            {
                bReload = true;
                m_torpedoReloadSound.Play();
            }
            else if (Input.GetKeyDown("f"))
            {
				SpawnFlare();
            }

			//WSAD movement
			float vertical = Input.GetAxis ("Vertical") * 10;
			float horizontal = Input.GetAxis ("Horizontal") * 100;
			if(vertical > 0 || horizontal > 0 || vertical < 0 || horizontal < 0)
			{
				locVel.z += vertical;
				locVel.x = horizontal;
				bMoving = true;
			}
        }
        else if (GameVariables.isAlive && bPickingUp == true)
        {
            if (Input.GetMouseButtonDown(2) || Input.GetKeyDown("space"))
            {
                if (bGoDown == true && dockingDelay == 0)
                {
                    m_pickingSpeed = 0.0f;
                    bGoDown = false;
                }
            }
        }

        if(bMoving == true)
        {
            locVel.z *= m_stopFactor;
            if (locVel.z < 0.5 && locVel.z > -0.5)
            {
                locVel.z = 0.0f;
                bMoving = false;
            }
           // rigidbody.velocity = transform.TransformDirection(locVel);
        }

        if (bPickingUp == true) // picking up item
        {
            if (bGoDown == true)
            {
                m_pickingSpeed -= m_pickingSpeedFactor;

                if (m_maxPickingSpeed > m_pickingSpeed)
                {
                    m_pickingSpeed = m_pickingSpeed;
                }
            }
            else // bGoDown == false
            {
                m_pickingSpeed += m_pickingSpeedFactor;

                if (worldPos.y >= m_initialHeight)
                {
                    bGoDown = true;     // reset
                    bPickingUp = false; // reset

                    worldPos.y = m_initialHeight; // set height to starting value
                    m_pickingSpeed = 0.0f;

                    PickUpLight_left.SetActive(false);
                    PickUpLight_right.SetActive(false);
                }
            }

            locVel.y = m_pickingSpeed;
        }
        if (bReload == true)
        {
            reloadTorpedos();
        }

        GetComponent<Rigidbody>().position = new Vector3(worldPos.x, worldPos.y, worldPos.z);
        GetComponent<Rigidbody>().velocity = transform.TransformDirection(locVel);        
	}

    void ejectTorpedo()
    {
        if (m_currentTorpedo != TORPEDO_ORDER.T_NONE)
        {
            switch (m_currentTorpedo)
            {
                case TORPEDO_ORDER.T1_LEFT:
                    {
                        Instantiate((GameObject)torpedoLive, torpedo1_left.transform.position, torpedo1_left.transform.rotation);
                        Vector3 scale = torpedo1_left.transform.localScale;
                        scale.z = m_torpedoScale_z;
                        torpedo1_left.transform.localScale = scale;
                        break;
                    }
                case TORPEDO_ORDER.T1_RIGHT:
                    {
                        Instantiate((GameObject)torpedoLive, torpedo1_right.transform.position, torpedo1_right.transform.rotation);
                        Vector3 scale = torpedo1_right.transform.localScale;
                        scale.z = m_torpedoScale_z;
                        torpedo1_right.transform.localScale = scale;
                        break;
                    }
                case TORPEDO_ORDER.T2_LEFT:
                    {
                        Instantiate((GameObject)torpedoLive, torpedo2_left.transform.position, torpedo2_left.transform.rotation);
                        Vector3 scale = torpedo2_left.transform.localScale;
                        scale.z = m_torpedoScale_z;
                        torpedo2_left.transform.localScale = scale;
                        break;
                    }
                case TORPEDO_ORDER.T2_RIGHT:
                    {
                        Instantiate((GameObject)torpedoLive, torpedo2_right.transform.position, torpedo2_right.transform.rotation);
                        Vector3 scale = torpedo2_right.transform.localScale;
                        scale.z = m_torpedoScale_z;
                        torpedo2_right.transform.localScale = scale;
                        break;
                    }
            }

            --m_currentTorpedo;

            if (m_currentTorpedo == TORPEDO_ORDER.T_NONE)
            {
                bReloaded = false;
            }
        }
        
    }
    void reloadTorpedos()
    {
        m_torpedoScale_z *= m_torpedoScaleFactor;
        if (m_torpedoScale_z > m_torpedoScale.z)
        {
            bReload = false; // switch off reload button 
            bReloaded = true; // reloading done
            m_torpedoScale_z = m_torpedoScale.z;
        }

        Vector3 scale = m_torpedoScale;
                scale.z = m_torpedoScale_z;
 

        // left1
        torpedo1_left.transform.localScale = scale;

        // right1
        torpedo1_right.transform.localScale = scale;

        // left2
        torpedo2_left.transform.localScale = scale;

        // right2
        torpedo2_right.transform.localScale = scale;

        m_currentTorpedo = TORPEDO_ORDER.T2_RIGHT;

        if (bReloaded == true)
        {
            m_torpedoScale_z = 0.1f;
        }
    }
}

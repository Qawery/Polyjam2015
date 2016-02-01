using UnityEngine;
using System.Collections;

public class PlaneRay : MonoBehaviour 
{

    public static float m_distance = 0.0f; 

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000))
        {
            m_distance = hit.point.z;
        }
        
	}
}

using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour 
{
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        transform.localPosition = new Vector3(0, 0, PlaneRay.m_distance);
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WarningFlash : MonoBehaviour {
    public Image Warning;

    public enum State
    {
        Visible,
        Invisible
    }

    public State targetState;

    public int wait = 0;

    public float alpha;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (wait == 0)
        {

            if (targetState == State.Visible & alpha < 1.0f)
            {
                alpha += 0.05f;
            }

            if (targetState == State.Invisible & alpha > 0.0f)
            {
                alpha -= 0.05f;
            }

            if (alpha >= 1f)
            {
                alpha = 1f;
                targetState = State.Invisible;
                wait = 10;
            }

            if (alpha <= 0f)
            {
                alpha = 0f;
                targetState = State.Visible;
                wait = 8;
            }

            Warning.color = new Color(Warning.color.r, Warning.color.g, Warning.color.b, alpha);
        }
        else wait--;
	}
}

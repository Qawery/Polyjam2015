using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuTooltip : MonoBehaviour {
    public Image ButtonTooltip;

    public bool IsSettings;
    public bool Persistent;

    public Color HighlightedColor;

    bool IsMouseOver;

	// Use this for initialization
	void Start () {
        ButtonTooltip.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (IsSettings)
        {
            Persistent = GameVariables.isSettingsOpen;
            if (GameVariables.isSettingsOpen)
            {
                ButtonTooltip.color = HighlightedColor;
            }
            else
            {
                ButtonTooltip.color = Color.white;
            }
        }

        // keep it from hiding goddamit
        if (!IsMouseOver)
            ButtonTooltip.gameObject.SetActive(Persistent);
	}

    // stuff
    public void OnMouseEnter()
    {
        ButtonTooltip.gameObject.SetActive(true);
        IsMouseOver = true;
    }

    public void OnMouseExit()
    {
        ButtonTooltip.gameObject.SetActive(Persistent);
        IsMouseOver = false;
    }
}

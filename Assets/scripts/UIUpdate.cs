using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIUpdate : MonoBehaviour {
    public Text scoreText;
    public Image HPBar;
    public Image OxygenBar;
    public Image ThrustBar;
    public Image TorpedoBar;
    public Image TorpedoBarFill;

    public Image OxygenWarning;

    public Image PauseUI;

    public Color redBar;
    public Color yellowBar;
    public Color greenBar;

    bool OxygenWarningEnabled = false;

    public AudioSource O2Warning;

	// Use this for initialization
	void Start () {
        HPBar.fillAmount = 1;
        PauseUI.gameObject.SetActive(false);
        OxygenWarning.gameObject.SetActive(false);
	}

    void OnGUI()
    {
        // Update GUI
        scoreText.text = "Score: <b>" + GameVariables.score.ToString() + "</b>";


        // update HPBar fill
        float fillAmount = (float)GameVariables.life / (float)GameVariables.defaultLife;
        UpdateBar(HPBar, fillAmount);

        // update oxygen bar
        fillAmount = (float)GameVariables.oxygenState / GameVariables.oxygenMax;
        UpdateBar(OxygenBar, fillAmount);

        // update thrust bar
        fillAmount = (float)PlayerController.m_thrustFactor / 2.0f;
        UpdateBar(ThrustBar, fillAmount);

        // get torps bars sorted out
        TorpedoBarFill.rectTransform.sizeDelta = new Vector2(69 * (int)PlayerController.m_currentTorpedo, TorpedoBar.rectTransform.sizeDelta.y);
        TorpedoBar.rectTransform.sizeDelta = new Vector2(69 * GameVariables.maxTorps, TorpedoBar.rectTransform.sizeDelta.y);

        // Oxygen Warning
        if (!OxygenWarningEnabled && ((float)GameVariables.oxygenState / GameVariables.oxygenMax) < 0.3f) {
            OxygenWarningEnabled = true;
            O2Warning.Play();
            OxygenWarning.gameObject.SetActive(true);
        } 

        if (OxygenWarningEnabled && ((float)GameVariables.oxygenState / GameVariables.oxygenMax) >= 0.3f) {
            OxygenWarningEnabled = false;
            OxygenWarning.gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        //pausing
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                PauseUI.gameObject.SetActive(false);
            }
            else
            {
                PauseUI.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }

        // ignore this stuff if paused
        if (Time.timeScale != 0)
        {
            // detect death
            // not sure if the best place for this stuff but whatever
            if (GameVariables.life <= 0)
            {
                AutoFade.LoadLevel("DeathScene", 1, (float)0.2, Color.black);
                GameVariables.isAlive = false;
            }

            // flare cooldown
            if (GameVariables.cooldownProgress > 0)
            {
                GameVariables.cooldownProgress -= 1;
            }
            else
            {
                GameVariables.cooldownProgress = 0;
            }

            // flare reload
            if (GameVariables.flares < GameVariables.defaultFlares)
            {
                if (GameVariables.reloadProgress > 0)
                {
                    GameVariables.reloadProgress -= 1;
                }
                else
                {
                    GameVariables.reloadProgress = GameVariables.reloadTime;
                    GameVariables.flares += 1;
                }
            }

            // STOP BREATHING YOU MORON
            if (GameVariables.oxygenState > 0)
            {
                if (!GameVariables.disableDying)
                    GameVariables.oxygenState--;
            }
            else
            {
                GameVariables.isAlive = false;
            }
        }
	}

    void UpdateBar(Image modifiedObject, float fillAmount)
    {
        // do the transition
        if (modifiedObject.fillAmount > fillAmount)
        {
            modifiedObject.fillAmount -= (modifiedObject.fillAmount - fillAmount) / 20;
        }
        else
        {
            modifiedObject.fillAmount += (fillAmount - modifiedObject.fillAmount) / 20;
        }

        // update HPBar color
        if (modifiedObject.fillAmount < 0.3)
        {
            modifiedObject.color = Color.Lerp(modifiedObject.color, redBar, 0.05f);
        }
        else if (modifiedObject.fillAmount > 0.7)
        {
            modifiedObject.color = Color.Lerp(modifiedObject.color, greenBar, 0.05f);
        }
        else modifiedObject.color = Color.Lerp(modifiedObject.color, yellowBar, 0.05f);
    }
}

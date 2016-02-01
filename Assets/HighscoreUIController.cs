using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HighscoreUIController : MonoBehaviour
{
    public Text names;
    public Text scores;
    List<Scores> highscore;

    // Use this for initialization
    void Start()
    {
        highscore = HighScoreManager._instance.GetHighScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        // reset stuff
        names.text = "";
        scores.text = "";

        foreach (Scores _score in highscore)
        {
            names.text  += _score.name + "\n";
            scores.text += _score.score + "\n";
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathSceneUI : MonoBehaviour
{
    public Text UIText;

    // Use this for initialization
    void Start()
    {
        UIText.text = "WHAT DO WE DO NOW?!\n" +
                      "Your score is: " + GameVariables.score.ToString() + "\n" +
                      (GameVariables.gameFinished ? " You've finished the game." : "") + "\n" +
                      "Do you want to save this score?";
    }
}

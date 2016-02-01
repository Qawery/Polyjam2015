using UnityEngine;
using System.Collections;
 

public class GameVariables : MonoBehaviour {
    public static int score = 0;
    public static int flares = 5;
    public static int availableTorpedo = 4;
    public static int cooldownProgress = 250;
    public static int reloadProgress = 1200;
    public static int oxygenState = 6000;
    public static int life = 100;
    public static bool isAlive = true;

    public static bool disableDying = false;

    // default/max values
    public static int defaultLife = 100; // yolo
    public static int defaultFlares = 5;
    public static int maxTorps = 4;
    public static int cooldownTime = 250;
    public static int reloadTime = 1200;
    public static int oxygenMax = 8000;

    // UI variables
    public static bool isSettingsOpen = false;
    public static bool isCheatsOpen   = false;
    public static bool areCheatsEnabled = false;
    public static bool gameFinished = true;

    // volumez!!111
    public static float soundVolume = 1f;
    public static float musicVolume = 1f;
}

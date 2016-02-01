using UnityEngine;
using System.Collections;

public class InGameMusic : MonoBehaviour {
    public AudioSource music;

	// Use this for initialization
	void Start () {
        music.Play();
        music.volume = GameVariables.musicVolume;
	}
	
	// Update is called once per frame
	void Update () {
        AudioListener.volume = GameVariables.musicVolume;
        music.volume = GameVariables.musicVolume;
	}
}

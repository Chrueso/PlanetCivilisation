using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    [Header("UI")]
    public AudioClip general;
    public AudioClip moveShipButton;
    public AudioClip colonizeButton;
    public AudioClip attackButton;
    public AudioClip buildButton;
    public AudioClip teleporterButton;
    public AudioClip clickOnHex;
    public AudioClip clickOnPlanet;
    public AudioClip popUp;

    [Header("Ship Sound")]
    public AudioClip stationShip;
    public AudioClip laserSound;
    public AudioClip explosion;

    [Header("Music")]
    public AudioClip BGM;
    public AudioClip attackMusic;
    public AudioClip idleMusic;
    public AudioClip mainMenuMusic;
}

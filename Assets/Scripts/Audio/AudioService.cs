using UnityEngine;

public class AudioService : MonoBehaviour
{
    private static AudioSystem AudioInstance;
    public static AudioSystem CurrentAudioInstance => AudioInstance;

    public static void SetAudioInstance(AudioSystem system)
    {
        if (system == null)
        {
            AudioInstance = null;
        }
        AudioInstance = system;
    }
}

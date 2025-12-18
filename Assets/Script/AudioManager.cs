using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;

    private AudioClip currentClip;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (audioSource == null && clip == null) return;

        // “¯‚¶bgm‚ª—¬‚ê‚Ä‚¢‚½‚ç‰½‚à‚µ‚È‚¢
        if (currentClip == clip && audioSource.isPlaying) return;

        currentClip = clip;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
        currentClip = null;
    }
}

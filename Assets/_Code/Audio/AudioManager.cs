using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioClip gavelSmash;
    [SerializeField] private AudioClip quillPickup;
    [SerializeField] private AudioClip quillDraw;
    [SerializeField] private AudioClip paperSlide;
    [SerializeField] private AudioClip backgroundMusic;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    public void PlayGavelSmash()  => sfxSource.PlayOneShot(gavelSmash);
    public void PlayQuillPickup() => sfxSource.PlayOneShot(quillPickup);
    public void PlayQuillDraw()
    {
        if (!sfxSource.isPlaying)
            sfxSource.PlayOneShot(quillDraw);
    }
    public void PlayPaperSlide()
    {
        if (!sfxSource.isPlaying)
            sfxSource.PlayOneShot(paperSlide);
    }
    
    
}
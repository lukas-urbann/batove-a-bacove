using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }


    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private float minPitch = 0.5f;
    [SerializeField] private float maxPitch = 1.5f;


    [SerializeField] private AudioClip gavelSmash;
    [SerializeField] private AudioClip gavelSmashWeak;
    [SerializeField] private AudioClip quillPickup;
    [SerializeField] private AudioClip quillDraw;
    [SerializeField] private AudioClip paperSlide;
    [SerializeField] private AudioClip bribeTake;

    [SerializeField] private AudioClip silenceVoiceLine;
    
    [SerializeField] private AudioClip gavelDoubleFast;
    [SerializeField] private AudioClip gavelDoubleSlow;
    
    /*
    [SerializeField] private AudioClip poorAngerAlert;
    [SerializeField] private AudioClip richAngerAlert;
    [SerializeField] private AudioClip winGame;
    [SerializeField] private AudioClip loseGame;
    */
    
    [SerializeField] private AudioClip voiceLine1;
    [SerializeField] private AudioClip voiceLine2;
    [SerializeField] private AudioClip voiceLine3;
    [SerializeField] private AudioClip voiceLine4;
    [SerializeField] private AudioClip voiceLine5;
    [SerializeField] private AudioClip voiceLine6;
    [SerializeField] private AudioClip voiceLine7;
    
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
    public void PlayGavelSmashWeak()  => sfxSource.PlayOneShot(gavelSmashWeak);
    public void PlayQuillPickup() => sfxSource.PlayOneShot(quillPickup);
    public void PlayQuillDraw()
    {
        if (!sfxSource.isPlaying)
        {
            sfxSource.PlayOneShot(quillDraw);
        }
    }
    public void PlayPaperSlide()  => sfxSource.PlayOneShot(paperSlide);
    public void PlayBribeTake()   => sfxSource.PlayOneShot(bribeTake);
    
    public void PlaySilenceVoiceLine()   => sfxSource.PlayOneShot(silenceVoiceLine);
    
    public void PlayGavelDoubleFast()   => sfxSource.PlayOneShot(gavelDoubleFast);
    public void PlayGavelDoubleSlow()   => sfxSource.PlayOneShot(gavelDoubleSlow);
    
    /*
    public void PlayWinGame()     => sfxSource.PlayOneShot(poorAngerAlert);
    public void PlayWinGame()     => sfxSource.PlayOneShot(richAngerAlert);
    public void PlayWinGame()     => sfxSource.PlayOneShot(winGame);
    public void PlayLoseGame()    => sfxSource.PlayOneShot(loseGame);
    */
    
    public void PlayVoiceLine(int lineNumber)
    {
        int randomLineNumber = Random.Range(1, 8);
    
        AudioClip clipToPlay = randomLineNumber switch
        {
            1 => voiceLine1,
            2 => voiceLine2,
            3 => voiceLine3,
            4 => voiceLine4,
            5 => voiceLine5,
            6 => voiceLine6,
            7 => voiceLine7,
            _ => null
        };

        if (clipToPlay != null)
        {
            voiceSource.pitch = Random.Range(minPitch, maxPitch);
            voiceSource.PlayOneShot(clipToPlay);
        }
    }
    
    

}
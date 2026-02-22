using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }


    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;


    [SerializeField] private AudioClip gavelSmash;
    [SerializeField] private AudioClip gavelSmashWeak;
    [SerializeField] private AudioClip quillPickup;
    [SerializeField] private AudioClip quillDraw;
    [SerializeField] private AudioClip paperSlide;
    [SerializeField] private AudioClip bribeTake;
    
    /*
    [SerializeField] private AudioClip poorAngerAlert;
    [SerializeField] private AudioClip richAngerAlert;
    [SerializeField] private AudioClip winGame;
    [SerializeField] private AudioClip loseGame;
    
    [SerializeField] private AudioClip maleVoice1;
    [SerializeField] private AudioClip maleVoice2;
    [SerializeField] private AudioClip maleVoice3;
    [SerializeField] private AudioClip maleVoice4;
    [SerializeField] private AudioClip femaleVoice1;
    [SerializeField] private AudioClip femaleVoice2;
    [SerializeField] private AudioClip femaleVoice3;
    [SerializeField] private AudioClip femaleVoice4;
    
    */
    
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
    
    
    /*
    public void PlayWinGame()     => sfxSource.PlayOneShot(poorAngerAlert);
    public void PlayWinGame()     => sfxSource.PlayOneShot(richAngerAlert);
    public void PlayWinGame()     => sfxSource.PlayOneShot(winGame);
    public void PlayLoseGame()    => sfxSource.PlayOneShot(loseGame);
    
    public void PlayMaleVoice(int index)
    {
        AudioClip clip = index switch
        {
            1 => maleVoice1,
            2 => maleVoice2,
            3 => maleVoice3,
            4 => maleVoice4,
            _ => null
        };
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlayFemaleVoice(int index)
    {
        AudioClip clip = index switch
        {
            1 => femaleVoice1,
            2 => femaleVoice2,
            3 => femaleVoice3,
            4 => femaleVoice4,
            _ => null
        };
        if (clip != null) sfxSource.PlayOneShot(clip);
    }
    
    */

}
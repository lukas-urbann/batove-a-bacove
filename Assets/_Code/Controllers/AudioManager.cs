using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }


    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.3f;


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

    [SerializeField] private AudioClip voiceLineExec1;
    [SerializeField] private AudioClip voiceLineExec2;
    [SerializeField] private AudioClip voiceLineExec3;
    [SerializeField] private AudioClip voiceLineExec4;
<<<<<<< Updated upstream
    [SerializeField] private float execVoicePitch = 0.8f;
=======
    [SerializeField] private float execVoicePitch = 0.75f;
>>>>>>> Stashed changes

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
    
    private Queue<AudioClip> _voiceQueue = new Queue<AudioClip>();

    private AudioClip[] AllVoiceLines() => new[] { 
        voiceLine1, voiceLine2, voiceLine3, voiceLine4, voiceLine5, voiceLine6, voiceLine7 
    };

    public void PlayVoiceLine()
    {
        if (_voiceQueue.Count == 0)
        {
            foreach (var clip in AllVoiceLines().OrderBy(_ => Random.value))
            {
                _voiceQueue.Enqueue(clip);
            }
        }
        voiceSource.pitch = Random.Range(minPitch, maxPitch);
        voiceSource.PlayOneShot(_voiceQueue.Dequeue());
    }
    
    private Queue<AudioClip> _execVoiceQueue = new Queue<AudioClip>();

    private AudioClip[] AllExecVoiceLines() => new[] { 
        voiceLineExec1, voiceLineExec2, voiceLineExec3, voiceLineExec4
    };
    
    public void PlayExecVoiceLine()
    {
        if (_execVoiceQueue.Count == 0)
        {
            foreach (var clip in AllExecVoiceLines().OrderBy(_ => Random.value))
            {
                _execVoiceQueue.Enqueue(clip);
            }
        }

        voiceSource.pitch = execVoicePitch;
        voiceSource.PlayOneShot(_execVoiceQueue.Dequeue());
    }

}
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class SoundManager : AbstractSingleton<SoundManager>
{
    public AudioMixer SoundMixer;
    public static float MasterVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("master-volume", 1);
        }
        set
        {
            PlayerPrefs.SetFloat("master-volume", value);
        }
    }
    public static float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("music-volume", 1);
        }
        set
        {
            PlayerPrefs.SetFloat("music-volume", value);
        }
    }

    [SerializeField] AudioSource _gasSound;
    [SerializeField] AudioSource _genericSource;
    [SerializeField] AudioClip _moveSound;
    [SerializeField] AudioClip _crashSound;

    Tween _gasSoundTween;

    void Start()
    {
        UpdateMasterVolume();
        UpdateMusicVolume();
    }

    public static void SetMasterVolume(float volume)
    {
        MasterVolume = volume;
        Instance.UpdateMasterVolume();
    }

    public static void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        Instance.UpdateMusicVolume();
    }

    public void UpdateMasterVolume()
    {
        SoundMixer.SetFloat("Master", Mathf.Log10(MasterVolume) * 20);
    }

    public void UpdateMusicVolume()
    {
        SoundMixer.SetFloat("Music", Mathf.Log10(MusicVolume) * 20);
    }

    public void StartGasSound()
    {
        if(_gasSoundTween != null)
        {
            _gasSoundTween.Kill();
        }
        _gasSound.Play();
        _gasSoundTween = _gasSound.DOFade(0.1f, 0.5f);
        _gasSoundTween.SetUpdate(true);
    }

    public void StopGasSound()
    {
        if (_gasSoundTween != null)
        {
            _gasSoundTween.Kill();
        }
        _gasSoundTween = _gasSound.DOFade(0, 0.5f).OnComplete(()=> {
            _gasSound.Stop();
        });
        _gasSoundTween.SetUpdate(true);
    }

    public void PlayMoveSound()
    {
        _genericSource.PlayOneShot(_moveSound);
    }

    public void PlayCrashSound()
    {
        _genericSource.PlayOneShot(_crashSound);
    }
}

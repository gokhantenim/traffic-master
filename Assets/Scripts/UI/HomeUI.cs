using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : AbstractSingleton<HomeUI>
{
    [SerializeField] Image _soundButtonImage;
    [SerializeField] Sprite _soundOnSprite;
    [SerializeField] Sprite _soundOffSprite;
    float _masterVolumeTemp = 1;

    void Start()
    {
        UpdateSoundButton();
    }

    public void ToggleSound()
    {
        if(SoundManager.MasterVolume > 0.001f)
        {
            _masterVolumeTemp = SoundManager.MasterVolume;
            SoundManager.SetMasterVolume(0.001f);
        } else
        {
            SoundManager.SetMasterVolume(_masterVolumeTemp);
        }
        UpdateSoundButton();
    }

    public void UpdateSoundButton()
    {
        _soundButtonImage.sprite = SoundManager.MasterVolume > 0.001f ? _soundOnSprite : _soundOffSprite;
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
}

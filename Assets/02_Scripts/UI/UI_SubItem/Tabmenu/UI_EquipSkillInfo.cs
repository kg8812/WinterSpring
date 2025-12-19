using System.Collections;
using System.Collections.Generic;
using Apis;
using chamwhy;
using chamwhy.Managers;
using chamwhy.UI;
using Default;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class UI_EquipSkillInfo : UI_Base
{
    public VideoPlayer videoPlayer;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public UIAsset_Button playButton;
    
    public override void Init()
    {
        base.Init();
        videoPlayer.loopPointReached -= ActivatePlayButton;
        videoPlayer.loopPointReached += ActivatePlayButton;
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
        playButton.gameObject.SetActive(false);
    }

    void ActivatePlayButton(VideoPlayer vp)
    {
        playButton.gameObject.SetActive(true);
    }
    public void Set(Skill skill)
    {
        if (skill == null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        ActivatePlayButton(videoPlayer);
        videoPlayer.clip = ResourceUtil.Load<VideoClip>("Videos/Pv1");
        title.text = LanguageManager.Str(skill.SkillName);
        description.text = LanguageManager.Str(skill.Desc);
    }
}

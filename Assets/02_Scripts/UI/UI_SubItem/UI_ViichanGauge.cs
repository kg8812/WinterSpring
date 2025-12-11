using Apis;
using Default;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UI_ViichanGauge : UI_Base
{
    public enum Images
    {
        Gauge,LeafIcon,Frame
    }

    private Image gauge;
    private Image leafIcon;
    private Image gaugeBg;
    private Image frame;
    
    private ViichanPassiveSkill skill;

    public Sprite[] leafSprites;
    public Sprite[] barSprites;
    public Sprite[] frameSprites;
    
    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        gauge = Get<Image>((int)Images.Gauge);
        gameObject.SetActive(false);
        UI_MainHud.Instance.setEvent.AddListener(x =>
        {
            x.OnPassiveSkillChange += CheckSkill;
            CheckSkill(x.PassiveSkill);
        });
        UI_MainHud.Instance.afterSet.AddListener(x =>
        {
            SetSkill(x.PassiveSkill);
            x.OnPassiveSkillChange += SetSkill;
        });
    }

    void CheckSkill(PassiveSkill passive)
    {
        if (passive is ViichanPassiveSkill v)
        {
            skill = v;
            gameObject.SetActive(true);
        }
        else
        {
            skill = null;
            gameObject.SetActive(false);
        }
    }

    void SetSkill(PassiveSkill passive)
    {
        if (passive is ViichanPassiveSkill v)
        {
            v.OnGaugeChange -= UpdateGauge;
            v.OnGaugeChange += UpdateGauge;
            UpdateGauge(0);
        }
    }
    private void UpdateGauge(float amount)
    {
        if (ReferenceEquals(skill, null)) return;

        gauge.fillAmount = Mathf.Clamp01(amount / skill.maxGauge);
    }
    
    
}

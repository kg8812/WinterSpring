using Apis;
using Default;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UI_IneManaBar : UI_Base
{
    enum Images
    {
        Gauge,Circle2,Circle3,Circle4
    }

    private Image _gaugeImage;
    private Image circle2;
    private Image circle3;
    private Image circle4;
    private IneActiveSkill _active;
    
    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        _gaugeImage = Get<Image>((int)Images.Gauge);
        circle2 = Get<Image>((int)Images.Circle2);
        circle3 = Get<Image>((int)Images.Circle3);
        circle4 = Get<Image>((int)Images.Circle4);
        
        UI_MainHud.Instance.setEvent.AddListener(x =>
        {
            CheckSkill(x.ActiveSkill);
            x.OnActiveSkillChange += CheckSkill;
            x.OnActiveSkillChange += SetSkill;
        });
        
        UI_MainHud.Instance.afterSet.AddListener(x =>
        {
            if (_active != null)
            {
                _active.OnManaChange.AddListener(y =>
                {
                    _gaugeImage.fillAmount = Mathf.Clamp01(y / _active.MaxMana);
                });
                SetBarPositions();
                _active.OnMaxManaChange.AddListener(SetBarPositions);
            }
        });
        
        _gaugeImage.fillAmount = 0;
    }

    void CheckSkill(ActiveSkill skill)
    {
        _active = skill as IneActiveSkill;
        gameObject.SetActive(_active != null);
    }

    void SetSkill(ActiveSkill skill)
    {
        _active = skill as IneActiveSkill;
        
        if (_active != null)
        {
            _active.OnManaChange.RemoveListener(UpdateGauge);
            _active.OnManaChange.AddListener(UpdateGauge);
            _active.OnMaxManaChange.RemoveListener(SetBarPositions);
            _active.OnMaxManaChange.AddListener(SetBarPositions);
            SetBarPositions();
            UpdateGauge(_active.mana);
        }
    }

    void UpdateGauge(float y)
    {
        _gaugeImage.fillAmount = Mathf.Clamp01(y / _active.MaxMana);
    }
    void SetBarPositions()
    {
        float boundaryWidth = _gaugeImage.rectTransform.sizeDelta.x;
        circle2.rectTransform.anchoredPosition = new Vector2(boundaryWidth * _active.Circle2Mana / _active.MaxMana, 0);
        circle3.rectTransform.anchoredPosition = new Vector2(boundaryWidth * _active.Circle3Mana / _active.MaxMana, 0);
        
        circle4.gameObject.SetActive(_active.maxCircle >= 4);
        circle4.rectTransform.anchoredPosition = new Vector2(boundaryWidth * _active.circle4Mana / _active.MaxMana, 0);
    }
}

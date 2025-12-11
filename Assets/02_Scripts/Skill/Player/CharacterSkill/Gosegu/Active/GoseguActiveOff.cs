using System;
using Apis;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GoseguActiveOff", menuName = "Scriptable/Skill/GoseguActiveOff")]
public class GoseguActiveOff : ActiveSkill
{
    [HideInInspector] public SeguMecha mecha;
    protected override ActiveEnums _activeType => ActiveEnums.Instant;

    [HideInInspector] public GoseguPassive passive;
    public override UI_AtkItemIcon Icon => UI_MainHud.Instance.mainSkillIcon;

    private GoseguActiveSkill _skill;
    
    public void Init(GoseguActiveSkill skill)
    {
        _skill = skill;
    }
    public override void Active()
    {
        base.Active();

        if (mecha != null && !mecha.IsDead)
        {
            mecha.Die();
        }

        GameManager.instance.Player.IdleFixOff();
        GameManager.PlayerController = GameManager.instance.Player.Controller;
        GameManager.instance.Player.StateEvent.RemoveEvent(EventType.OnCutScene,_skill.MountDown);

        passive.TurnOnDrones();
    }
    
}

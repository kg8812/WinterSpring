using Apis;

public class ViichanShieldColorController : ShieldColorController
{
    private bool isNull;
    private ViichanActiveSkill skill;
    public override void Start()
    {
        base.Start();
        skill = GameManager.instance.Player.ActiveSkill as ViichanActiveSkill;
        isNull = skill == null;
    }

    public override void Update()
    {
        base.Update();
        if (isNull) return;

        range = skill.CurGauge / skill.MaxGauge;
    }
}

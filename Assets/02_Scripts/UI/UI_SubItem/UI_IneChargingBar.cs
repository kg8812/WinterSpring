using UI;
using Apis;

public class UI_IneChargingBar : UI_ChargingBar
{
    public override void Init()
    {
        base.Init();
        UI_MainHud.Instance.setEvent.AddListener(x =>
        {
            active = x?.ActiveSkill as IneActiveSkill;
            gameObject.SetActive(active != null);
        });
    }
}

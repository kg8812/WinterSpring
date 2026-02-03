using System.Collections;
using System.Collections.Generic;
using Managers;
using Save.Schema;
using UnityEngine;

public class SkillActivator : MonoBehaviour
{
    public void ActivateActiveSkill()
    {
        if(DataAccess.TaskData.IsDone(102))
        {
            return;
        }
        DataAccess.TaskData.ActivateTask(102);
        GameManager.instance.Player.ResetActiveSkill();
        SystemManager.SystemAlert("액티브스킬이 활성화 되었습니다.",null);
    }

    public void ActivatePassiveSkill()
    {
        if(DataAccess.TaskData.IsDone(103))
        {
            return;
        }
        DataAccess.TaskData.ActivateTask(103);
        GameManager.instance.Player.ResetPassiveSkill();
        SystemManager.SystemAlert("패시브스킬이 활성화 되었습니다.",null);
    }

    public bool IsActiveSkillActivated()
    {
        return DataAccess.TaskData.IsDone(102);
    }

    public bool IsActiveSkillDeActivated()
    {
        return !DataAccess.TaskData.IsDone(102);
    }
    public bool IsPassiveSkillActivated()
    {
        return !DataAccess.TaskData.IsDone(103);
    }
}

using System.Collections;
using System.Collections.Generic;
using Save.Schema;
using UnityEngine;

public class TaskData
{
    public TaskSaveData Data => GameManager.Save.currentSlotData.TaskSaveData;

    public void ActivateTask(int id)
    {
        if (id <= 0) return;
        Data.Tasks.Add(id);
        GameManager.instance.WhenUnlock.Invoke();
    }

    public void DeActivateTask(int id)
    {
        Data.Tasks.Remove(id);
        GameManager.instance.WhenUnlock.Invoke();
    }

    public bool IsDone(int id)
    {
        return Data.Tasks.Contains(id);
    }
}

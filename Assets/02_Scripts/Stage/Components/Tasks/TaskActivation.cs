using System.Collections;
using System.Collections.Generic;
using Save.Schema;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class TaskActivation : MonoBehaviour
{
    public int index;
    public UnityEvent OnTaskActivate;
    void Activate()
    {
        DataAccess.TaskData.ActivateTask(index);
    }

    public void DeActivate()
    {
        DataAccess.TaskData.DeActivateTask(index);
    }

    public void DoTask()
    {
        if (DataAccess.TaskData.IsDone(index))
        {
            return;
        }
        
        Activate();
        Task();
        OnTaskActivate?.Invoke();
    }

    protected virtual void Task()
    {
        
    }

    public bool CheckInteractable()
    {
        return !DataAccess.TaskData.IsDone(index);
    }
}

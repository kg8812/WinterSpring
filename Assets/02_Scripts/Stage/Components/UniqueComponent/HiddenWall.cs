using System;
using Default;
using DG.Tweening;
using Save.Schema;
using UnityEngine;

public class HiddenWall : MonoBehaviour
{
    public int idx;
    private SpriteRenderer _renderer;
    bool isUnlocked = false;
    public float fadeTime;
    public string sfxName;
    public string audioSetting;
    
    void Start()
    {
        GameManager.instance.WhenUnlock.RemoveListener(CheckUnlock);
        GameManager.instance.WhenUnlock.AddListener(CheckUnlock);
        CheckUnlock();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void CheckUnlock()
    {
        isUnlocked = DataAccess.TaskData.IsDone(idx);
        gameObject.SetActive(!isUnlocked);
    }

    void UnLock()
    {
        if (isUnlocked) return;
        Color color = _renderer.color;
        _renderer.DOColor(new Color(color.r, color.g, color.b, 0), fadeTime).onComplete += () =>
        {
            DataAccess.TaskData.ActivateTask(idx);
            _renderer.color = color;
        };
        isUnlocked = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Sound.PlayInPosition(sfxName, audioSetting, transform.position,Define.Sound.SFX);
            UnLock();
        }
    }
}

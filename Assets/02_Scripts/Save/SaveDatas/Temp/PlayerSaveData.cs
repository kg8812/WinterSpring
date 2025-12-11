using System;
using System.Collections;
using System.Collections.Generic;
using Apis;
using Apis.SkillTree;
using chamwhy;
using chamwhy.Components;
using Sirenix.Utilities;
using UnityEngine;

namespace Save.Schema
{
    [Serializable]
    public class PlayerSaveData : ISaveData
    {
        public PlayerType playerType = PlayerType.Ine;
        public float curHp = 0;
        public int level = 1;
        public int exp;
        
        public void BeforeSave()
        {
            curHp = GameManager.instance.Player.CurHp;
            playerType = GameManager.instance.Player.playerType;
            level = GameManager.instance.Level;
            exp = GameManager.instance.Exp;
        }

        public void OnLoaded()
        {
            Player.CreatePlayerByType(playerType, true);
            if (curHp > 0 && !Mathf.Approximately(curHp, 0))
            {
                GameManager.instance.Player.CurHp = curHp;
            }

            GameManager.instance.Level = level;
            GameManager.instance.Exp = exp;
        }

        public void Initialize()
        {
            curHp = 0;
            SkillTreeDatas.DeActiveAll();
            level = 1;
            exp = 0;

            GameManager.instance.Level = level;
            GameManager.instance.Exp = exp;
        }
    }
}
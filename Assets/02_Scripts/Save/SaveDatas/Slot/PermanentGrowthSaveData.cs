using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Apis;
using Save.Schema;
using UnityEngine;

namespace Save.Schema
{
    public class PermanentGrowthSaveData : SlotSaveData
    {
        public PlayerData Player = new();
        public GrowthData Growth = new();
        public StageObjectData ObjectData = new();

        [System.Serializable]
        public class PlayerData
        {
            [System.Serializable]
            public class PlayerStat // 여기 값들 저장
            {
                [LabelText("회복 추가횟수")] public int potionCount; // 수리 추가 횟수
                [LabelText("점프 추가횟수")] public int jumpMax; // 점프 추가 횟수

                public static PlayerStat operator +(PlayerStat a, PlayerStat b)
                {
                    PlayerStat c = new()
                    {
                        potionCount = a.potionCount + b.potionCount,
                        jumpMax = a.jumpMax + b.jumpMax
                    };
                    return c;
                }

                public static PlayerStat operator -(PlayerStat a, PlayerStat b)
                {
                    PlayerStat c = new()
                    {
                        potionCount = a.potionCount - b.potionCount,
                        jumpMax = a.jumpMax - b.jumpMax
                    };
                    return c;
                }
            }

            public BaseStat baseStat = new();

            public PlayerStat playerStat = new();
        }

        [InfoBox("로비 영구 성장 진행 데이터입니다")]
        public class GrowthData
        {
            public Dictionary<ActorStatType, int> CurLvl = new Dictionary<ActorStatType, int>
            {
                { ActorStatType.Atk, 0 },
                { ActorStatType.MaxHp, 0 },
                { ActorStatType.Def, 0 },
                { ActorStatType.MoveSpeed, 0 },
                { ActorStatType.CritProb, 0 }
            };
        }

        [InfoBox("오브젝트의 성장 변수들")]
        public class StageObjectData
        {
            public int wellOfWarmthCount;
        }

        protected override void OnBeforeSave()
        {
        }

        protected override void BeforeLoaded()
        {
        }

        protected override void OnReset()
        {
            Player = new();
            Growth = new();
            ObjectData = new();
            
        }
    }

}
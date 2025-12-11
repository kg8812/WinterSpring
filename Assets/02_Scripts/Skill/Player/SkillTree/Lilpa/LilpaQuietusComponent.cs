using System.Collections;
using System.Collections.Generic;
using Apis.SkillTree;
using Default;
using UnityEngine;

namespace Apis.SkillTree
{
    public class LilpaQuietusComponent : MonoBehaviour
    {
        private Player _player;
        private LilpaTree1A _tree;
        public void Init(LilpaTree1A tree,Player player)
        {
            _tree = tree;
            _player = player;
        }

        public void SpawnFirstEffect()
        {
            if (_tree.target == null) return;

            GameObject effect = _player.EffectSpawner.Spawn(Define.PlayerEffect.LilpaQuietus01, _tree.target.Position,false).gameObject;
            effect.SetRadius(_tree.effectRadius);
        }

        public void SpawnSecondEffect()
        {
            if (_tree.target == null) return;

            GameObject effect = _player.EffectSpawner.Spawn(Define.PlayerEffect.LilpaQuietus02, _tree.target.Position,false).gameObject;
            effect.SetRadius(_tree.effectRadius);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Apis;
using Default;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_JingGauge : UI_Base
    {
        public enum SubItems
        {
            DogGauge,
            BirdGauge,
            WhaleGauge
        }

        private Dictionary<JingburgerPassiveSkill.AnimalPaints, UI_Base> _gauges;
        private Dictionary<JingburgerPassiveSkill.AnimalPaints, List<Image>> _stacks;

        private JingburgerPassiveSkill _passive;

        private Dictionary<JingburgerPassiveSkill.AnimalPaints, JingburgerPassiveSkill.IFireEvent> _animals;

        public override void Init()
        {
            base.Init();

            Bind<UI_Base>(typeof(SubItems));
            _animals = new()
            {
                { JingburgerPassiveSkill.AnimalPaints.Dog, null },
                { JingburgerPassiveSkill.AnimalPaints.Bird, null },
                { JingburgerPassiveSkill.AnimalPaints.Whale, null },
            };
            
            _gauges = new();
            _stacks = new();

            _gauges.Add(JingburgerPassiveSkill.AnimalPaints.Dog, Get<UI_Base>((int)SubItems.DogGauge));
            _gauges.Add(JingburgerPassiveSkill.AnimalPaints.Bird, Get<UI_Base>((int)SubItems.BirdGauge));
            _gauges.Add(JingburgerPassiveSkill.AnimalPaints.Whale, Get<UI_Base>((int)SubItems.WhaleGauge));

            _stacks.Add(JingburgerPassiveSkill.AnimalPaints.Dog,null);
            _stacks.Add(JingburgerPassiveSkill.AnimalPaints.Bird, null);
            _stacks.Add(JingburgerPassiveSkill.AnimalPaints.Whale, null);
          
            UI_MainHud.Instance.setEvent.AddListener(x =>
            {
               CheckSkill(x.PassiveSkill);
               x.OnPassiveSkillChange += CheckSkill;
            });

            UI_MainHud.Instance.afterSet.AddListener(x =>
            {
                SetSkill(x.PassiveSkill);
                x.OnPassiveSkillChange += SetSkill;
            });
        }

        void CheckSkill(PassiveSkill passive)
        {
            _passive = passive as JingburgerPassiveSkill;
            gameObject.SetActive(_passive != null);
        }

        void SetSkill(PassiveSkill passive)
        {
            _passive = passive as JingburgerPassiveSkill;
            if (_passive == null) return;
            
            _passive.OnAnimalStackChange -= UpdateStack;
            _passive.OnAnimalStackChange += UpdateStack;
            _passive.OnAnimalChange -= UpdateAnimal;
            _passive.OnAnimalChange += UpdateAnimal;
            UpdateStack();
            UpdateAnimal();
        }
        
        void UpdateStack()
        {
            _animals.Keys.ForEach(key =>
            {
                var animal = _animals[key];
                
                if (animal != null)
                {
                    for (int i = 0; i < animal.CurStack; i++)
                    {
                        _stacks[key][i].color = Color.white;
                    }

                    for (int i = animal.CurStack; i < animal.MaxStack; i++)
                    {
                        _stacks[key][i].color = Color.grey;
                    }
                }
            });
        }

        void UpdateAnimal()
        {
            if ((object)_passive == null) return;
            
            var list = _animals.Keys.ToList();
            
            list.ForEach(animal =>
            {
                _animals[animal] = _passive.GetAnimalPaint(animal);
                if (_animals[animal] != null)
                {
                    _gauges[animal].gameObject.SetActive(true);
                    if (_stacks[animal] != null) return;
                    _stacks[animal] = new();
                    
                    for (int i = 0; i < _animals[animal].MaxStack; i++)
                    {
                        _stacks[animal].Add(GameManager.UI.MakeSubItem("JingStack", _gauges[animal].transform).GetComponent<Image>());
                    }
                }
                else
                {
                    _gauges[animal].gameObject.SetActive(false);
                }
            });
            
            UpdateStack();
        }
    }
}
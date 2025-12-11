using System.Collections.Generic;
using DG.Tweening;
using Save.Schema;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasterEggBasket
{
    public class Egg03 : SerializedMonoBehaviour
    {
        [SerializeField] private GameObject snowMan;
        [SerializeField] private Dictionary<PlayerType, Sprite> snowmanSprites;
        private const float EggDelay = 5f;
        private readonly Vector3 _offset = new Vector3(0, 1.4f, 0);

        private bool _isCheck = false;
        private float _startTime;
        private bool _isShowed;

        private ParticleSystem _particleSystem;
        private GameObject _showedSnowMan = null;
        private SpriteRenderer _renderer = null;


        private void Start()
        {
            if (GameManager.instance.Player != null)
            {
                GameManager.instance.Player.AddEvent(EventType.OnHit, _ => ResetTime());
                GameManager.instance.Player.AddEvent(EventType.OnMove, _ => ResetTime());
                _isCheck = true;
            }
            GameManager.instance.playerRegistered.AddListener(p =>
            {
                if (p == null) return;
                p.AddEvent(EventType.OnHit, _ => ResetTime());
                p.AddEvent(EventType.OnMove, _ => ResetTime());
                _isCheck = true;
            });
        }

        private void Update()
        {
            if (_isCheck && !_isShowed)
            {
                if (_startTime + EggDelay <= Time.time)
                {
                    ShowSnowMan();
                }
            }
        }

        private void ShowSnowMan()
        {
            _isCheck = false;
            _isShowed = true;
            _showedSnowMan = Instantiate(snowMan,
                GameManager.instance.PlayerTrans.position + _offset, Quaternion.identity);
            _renderer = _showedSnowMan.GetComponent<SpriteRenderer>();
            _renderer.sprite = snowmanSprites[GameManager.instance.Player.playerType];
            _particleSystem = _showedSnowMan.GetComponent<ParticleSystem>();
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 0f);
            _renderer.DOFade(1f, 1f).SetEase(Ease.OutQuad);
        }

        private void ResetTime()
        {
            _startTime = Time.time;
            DestroySnowMan();
        }


        private void DestroySnowMan()
        {
            if (_showedSnowMan != null && _renderer != null && _isShowed)
            {
                _isShowed = false;
                _particleSystem.Play();
                _renderer.DOFade(0, 0.4f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    Destroy(_showedSnowMan);
                    _showedSnowMan = null;
                    _renderer = null;
                    _isCheck = true;
                });
                
            }
        }
        
    }
}
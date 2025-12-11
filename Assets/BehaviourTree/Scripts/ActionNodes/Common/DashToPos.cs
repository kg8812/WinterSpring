using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.BehaviourTreeTool
{
    public class DashToPos : CommonActionNode
    {
        public string objectName;
        public float speed;
        public float distanceCondition;

        Tweener tweener;
        GameObject pos;

        float distance;
        bool isFinished;

        private IMovable mover;
        [LabelText("End모션 스킵여부")] public bool isSkip = false;

        private bool requestKill;      // ★ Kill 예약 플래그
        private bool tweenKilled;      // ★ Kill 중복 방지
        
        [Tooltip("플레이어가 반대쪽으로 가면 멈추는지 여부")] public bool stopIfOp = true;
        
        public override void OnStart()
        {
            base.OnStart();
            mover = _actor as IMovable;
            
            _actor.animator.ResetTrigger("DashEnd");
            OnAlert.RemoveAllListeners();
            pos = GameObject.Find(objectName);

            mover?.ActorMovement?.Stop();

            _actor.animator.SetBool("IsDashEnd",!isSkip);
            _actor.animator.SetTrigger("Dash");
            OnAlert.AddListener(Alert);
            isFinished = false;
            requestKill = false;
            tweenKilled = false;
            
        }
        
        /// <summary>
        /// Kill은 여기서만 이뤄짐
        /// Update에서는 Kill 예약만 한다.
        /// </summary>
        private void KillTween()
        {
            if (tweenKilled) return;           
            tweenKilled = true;
            requestKill = false;

            if (tweener != null && tweener.IsActive())
            {
                tweener.Kill();
            }
            
            mover?.ActorMovement?.Stop();
            mover?.ActorMovement?.ResetGravity();
            
            tweener = null;
            blackBoard.tweener = null;
        }

        void Dash()
        {
            blackBoard.tweener?.Kill();
            tweener?.Kill();
            
            if (pos != null)
            {
                float posX = pos.TryGetComponent(out Actor act) ? act.Position.x : pos.transform.position.x;

                mover?.ActorMovement?.SetGravityToZero();

                Transform trans = _actor.transform;

                float d = posX - trans.position.x;
                _actor.SetDirection(d < 0 ? EActorDirection.Left : EActorDirection.Right);

                tweener = _actor.Rb.DOMoveX(_actor.Position.x + (int)_actor.Direction, 1 / speed)
                    .SetUpdate(UpdateType.Fixed)
                    .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).OnKill(() =>
                    {
                        _actor.animator.SetTrigger("DashEnd");
                        
                        if (isSkip) isFinished = true;
                    });
                blackBoard.tweener = tweener;

                tweener.onUpdate += () =>
                {
                    var direction = new Vector2((int)_actor.Direction, 0);
                    if (Physics2D.Raycast(_actor.Position, direction, 0.75f, LayerMasks.Wall))
                    {
                        requestKill = true;
                    }
                };
            }
        }
        public override void OnStop()
        {
            base.OnStop();

            KillTween();
            
            OnAlert.RemoveAllListeners();
        }

        public override State OnUpdate()
        {
            float x = pos.TryGetComponent(out Actor act) ? act.Position.x : pos.transform.position.x;

            distance = Mathf.Abs(x - _actor.Position.x);
            
            if (distance < distanceCondition || distance < 0.05f && tweener.IsActive())
            {
                requestKill = true;
            }     
            if((_actor.Position.x < x && _actor.Direction == EActorDirection.Left
                || _actor.Position.x > x && _actor.Direction == EActorDirection.Right) && tweener.IsActive())
            {
                requestKill = true;
            }

            if (requestKill)
            {
                KillTween();
                isFinished = true;
            }
            
            return isFinished ? State.Success : State.Running;
            
        }
        public override void OnSkip()
        {
            base.OnSkip();
            KillTween();
        }
        void Alert(string alert)
        {
            if (!isStarted) return;
            
            if (alert == "DashStart")
            {
                Dash();
            }
            if(alert == "DashEnd")
            {
                isFinished = true;
            }
        }
    }
}
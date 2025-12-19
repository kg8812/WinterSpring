using chamwhy;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Apis.BehaviourTreeTool
{
    public class DashNode : CommonActionNode
    {
        public float time;
        public float distance;
        public bool isBackDash;

        [LabelText("End모션 스킵여부")]  public bool isSkip = false;
        bool success;

        Tweener tweener;

        bool isFinished;
        private IMovable mover;

        private bool requestKill;      // ★ Kill 예약 플래그
        private bool tweenKilled;      // ★ Kill 중복 방지
        
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
        
        public override void OnStart()
        {
            base.OnStart();
            mover = _actor as IMovable;
            success = false;
            isFinished = false;
            requestKill = false;
            tweenKilled = false;
            OnAlert.AddListener(Alert);
           
            _actor.animator.SetBool("IsDashEnd",!isSkip);
            _actor.animator.ResetTrigger("DashEnd");
            string trigger = isBackDash ? "BackDash" : "Dash";
            _actor.animator.SetTrigger(trigger);
        }

        void Dash()
        {
            if (mover == null) return;
            
            mover.ActorMovement.Stop();
            
            tweener = mover.ActorMovement.DashTemp(time, distance, isBackDash).SetAutoKill(true).SetEase(Ease.Linear);
            _actor.ExecuteEvent(EventType.OnDash, new EventParameters(_actor));

            mover.Rb.gravityScale = 0;
            blackBoard.tweener = tweener;
            tweener.onKill += () =>
            {
                success = true;
                _actor.animator.SetTrigger("DashEnd");
                
                if (isSkip) isFinished = true;
            };
            tweener.onUpdate += () =>
            {
                var direction = new Vector2((int)_actor.Direction * (isBackDash ? -1 : 1), 0);
                if (Physics2D.Raycast(_actor.Position, direction, 0.75f, LayerMasks.Wall))
                {
                    requestKill = true;
                }
            };
        }

        public override void OnStop()
        {
            base.OnStop();

            KillTween();

            OnAlert.RemoveAllListeners();
        }

        public override State OnUpdate()
        {
            if (requestKill)
            {
                KillTween();
                isFinished = true;
            }
            
            if (!isFinished)
            {
                return State.Running;
            }

            return success ? State.Success : State.Failure;
        }

        void Alert(string alert)
        {
            if (!isStarted) return;
            
            if (alert == "DashEnd")
            {
                isFinished = true;
            }

            if (alert == "DashStart")
            {
                Dash();
            }
        }
    }
}
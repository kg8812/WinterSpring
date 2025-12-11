using UnityEngine;

namespace OutsiderSpace
{
    public class MonsterMaker: MonoBehaviour
    {
        public Animator targetAnim;

        public Animation idle;
        public Animation move;

        public bool isGroggyHasStartEnd;
        public Animation groggyStart;
        public Animation groggyLoop;
        public Animation groggyEnd;

        public Animation attackStart;
        public Animation attackLoop;
        public Animation attackEnd;

        public Animation dead;

    }
}
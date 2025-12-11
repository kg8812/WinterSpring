using Sirenix.OdinInspector;

namespace Apis
{
    public class KingguSwordSkill : MagicSkill
    {
        [Title("킹구 스킬")] 
        protected override ActiveEnums _activeType => ActiveEnums.Instant;
    }
}
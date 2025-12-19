using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Define
{
    #region Enums

    public enum GameKey
    {
        LeftMove,
        RightMove,
        Down,
        Up,
        Heal,
        Dash,
        Jump,
        ActiveSkill,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        Interact,
        Tab,
        Escape,
        CameraLeft,
        CameraRight,
        CameraUp,
        CameraDown,
        ChainAction,
    }

    public enum UIKey
    {
        LeftHeader,
        RightHeader,
        Left,Right,Up,Down,
        Select,
        Cancel,
        Equip,
        Skip,
    }
    public enum UIEvent
    {
        Click,
        BeginDrag,
        Drag,
        EndDrag,
        Drop,
        PointDown,
        PointUp,
        PointEnter,
        PointExit,
        PointStay
    }

    public enum Sound
    {
        BGM,
        SFX,
        UI,
        Ambience,
        Master,
        MaxCOUNT
    }
    public enum StageConcept
    {
        Gosegu,
        Jururu,
        Jingburger,
        Lilpa,
        Ine,
        Viichan
    }

    public enum WeaponType
    {
        Default = 0,
        OneHanded,
        TwoHanded,
        Scythe,
        Polearm,
        Magic,
        Gun,
        Rifle
    }

    public enum AttackType
    {
        Extra,
        BasicAttack,
        DotDmg,
        PlayerSkillAttack,
        WeaponSkillAttack,
        FrozenDrillAttack,
    }
    #endregion
    

    #region Address 모음
    #region  사운드
    
    public struct BGMList
    {
        public const string MainThemeTitle = "MainThemeTitle";
        public const string MainTheme = "MainTheme";
        
        public const string TutorialAmbience = "TutorialAmbience";
        public const string TutorialFieldIntro = "TutorialFieldIntro";
        public const string TutorialFieldLoop = "TutorialFieldLoop";
        public const string TutorialBattleIntro = "TutorialBattleIntro";
        public const string TutorialBattleLoop = "TutorialBattleLoop";
        
        public const string LobbyAmbience = "LobbyAmbience";
        public const string LobbyField = "LobbyField";
        
        public const string GoseguAmbience = "GoseguAmbience";
        public const string GoseguField = "GoseguField";
        public const string GoseguBattleLoop = "GoseguBattleLoop";
        public const string GoseguPhase1Loop = "GoseguPhase1Loop";
        public const string GoseguPhase2Intro = "GoseguPhase2Intro";
        public const string GoseguPhase2Loop = "GoseguPhase2Loop";
        
        public const string JururuAmbience = "JururuAmbience";
        public const string JururuField = "JururuField";
        public const string JururuBattleLoop = "JururuBattleLoop";
        public const string JururuPhase1Intro = "JururuPhase1Intro";
        public const string JururuPhase1Loop = "JururuPhase1Loop";
        public const string JururuPhase2Intro = "JururuPhase2Intro";
        public const string JururuPhase2Loop = "JururuPhase2Loop";
        
        public const string JingburgerAmbience = "JingburgerAmbience";
        public const string JingburgerField = "JingburgerField";
        public const string JingburgerBattleLoop = "JingburgerBattleLoop";
        public const string JingburgerPhase1Loop = "JingburgerPhase1Loop";
        public const string JingburgerPhase2Loop = "JingburgerPhase2Loop";
        
        public const string LilpaAmbience = "LilpaAmbience";
        public const string LilpaField = "LilpaField";
        public const string LilpaBattleLoop = "LilpaBattleLoop";
        public const string LilpaPhase1Loop = "LilpaPhase1Loop";
        public const string LilpaPhase2Intro = "LilpaPhase2Intro";
        public const string LilpaPhase2Loop = "LilpaPhase2Loop";
        
        public const string IneAmbience = "IneAmbience";
        public const string IneField = "IneField";
        public const string IneBattleLoop = "IneBattleLoop";
        public const string InePhase1Loop = "InePhase1Loop";
        public const string InePhase2Loop = "InePhase2Loop";
        
        public const string VIichanAmbience = "VIichanAmbience";
        public const string VIichanField = "VIichanField";
        public const string VIichanBattleLoop = "VIichanBattleLoop";
        public const string VIichanPhase1Loop = "VIichanPhase1Loop";
        public const string VIichanPhase2Loop = "VIichanPhase2Loop";
        
        public const string EndingScene = "EndingScene";

        public const string ArenaSample = "MainTheme";
    }
    
    public struct SFXList
    {
        public const string LilpaGunFire = "lilpaSkill_gunfire";
        public const string LilpaGunHit = "lilpaSkill_gunhit";
        public const string LilpaGunPick = "lilpaSkill_pickgun";
        public const string LilpaGunDrop = "lilpaSkill_dropgun";

        public static readonly string[] GrassFootSteps = new string []
        {
            "2_1_footstep_grass1","2_1_footstep_grass2","2_1_footstep_grass3"
        };

        public static readonly string[] SnowFootSteps = new string[]
        {
            "2_2_footstep_snow1",
            "2_2_footstep_snow2",
            "2_2_footstep_snow3",
        };

        public static readonly string[] RockFootSteps =
        {
            "2_3_footstep_rock1",
            "2_3_footstep_rock2",
        };

        public static readonly string[] WaterFootSteps =
        {
            "2_4_footstep_water1",
            "2_4_footstep_water2",
            "2_4_footstep_water3",
        };
    }
    #endregion
    
    #region 이펙트
    public struct PlayerEffect // 플레이어 이펙트
    {
        public const string GoseguWalkLeaf = "Move_Leaf_Gosegu";
        public const string IneWalkLeaf = "Move_Leaf_Ine";
        public const string JingBurgerWalkLeaf = "Move_Leaf_Jingburger";
        public const string LilpaWalkLeaf = "Move_Leaf_Lilpa";
        public const string JururuWalkLeaf = "Move_Leaf_Jururu";
        public const string ViichanWalkLeaf = "Move_Leaf_Viichan";
        public const string IceDrillLoop = "IceDrillLoop";
        public const string IceDrill = "IceDrill";
        public const string Chain = "PlayerChain";
        
        public const string Repair = "FXP_Player_Repair";
        public const string RepairLoop = "FXP_Player_Repair_Loop";
        public const string Hit = "CommonHit";
        public const string GoseguDashStart = "FXP_Gosegu_Start_Glow";
        public const string GoseguDashTrail = "FXP_Gosegu_Trail";
        public const string GoesguDashEnd = "FXP_Gosegu_End_Glow";
        public const string IneDashStart = "FXP_Ine_Start_Glow"; 
        public const string IneDashTrail = "FXP_Ine_Trail";
        public const string IneDashEnd = "FXP_Ine_End_Glow";
        public const string LilpaDashStart = "FXP_Lilpa_Start_Glow";
        public const string LilpaDashTrail = "FXP_Lilpa_Trail";
        public const string LilpaDashEnd = "FXP_Lilpa_End_Glow";
        public const string JingBurgerDashStart = "FXP_JingBurger_Start_Glow";
        public const string JingBurgerDashTrail = "FXP_JingBurger_Trail";
        public const string JingBurgerDashEnd = "FXP_JingBurger_End_Glow";
        public const string JururuDashStart = "FXP_Jururu_Start_Glow";
        public const string JururuDashTrail = "FXP_Jururu_Trail";
        public const string JururuDashEnd = "FXP_Jururu_End_Glow";
        public const string ViichanDashStart = "FXP_Viichan_Start_Glow";
        public const string ViichanDashTrail = "FXP_Viichan_Trail";
        public const string ViichanDashEnd = "FXP_Viichan_End_Glow";
        public const string ViichanSwordEffect = "ViichanSwordEffect";

        public const string IneFeather = "FXP_Feather";
        public const string IneFeatherAppear = "FXP_Feather_Appear";
        public const string IneFeatherHit = "FXP_Feather_Hit";
        public const string IneActiveEnd = "FXP_Ine_Active_End";
        public const string IneAura= "FXP_Ine_Aura";
        public const string Ine_Magic_CircleEnter = "Ine_Magic_CircleEnter";
        public const string Ine_Magic_1Circle = "Ine_Magic_1Circle";
        public const string Ine_Magic_1Circle_Shard = "Ine_Magic_1Circle_Shard";
        public const string Ine_Magic_1Circle_Shoot = "Ine_Magic_1Circle_Shoot";
        public const string Ine_Magic_2Circle_Start = "FXP_Ine_Magic_2Circle_Field_Start";
        public const string Ine_Magic_2Circle_Loop = "FXP_Ine_Magic_2Circle_Field_Start_Loop";
        public const string Ine_Magic_2Circle_End = "FXP_Ine_Magic_2Circle_Field_End";
        public const string Ine_Magic_2Circle = "Ine_Magic_2Circle";
        public const string Ine_Magic_3Circle = "Ine_Magic_3Circle";
        public const string Ine_Magic_3Circle_GodRay = "Ine_Magic_3Circle_GodRay";
        public const string Ine_Magic_3Circle_Moon = "Ine_Magic_3Circle_Moon";
        public const string Ine_Magic_3Circle_MoonExplode = "Ine_Magic_3Circle_MoonExplode";
        public const string Ine_Magic_3Circle_MoonShard = "Ine_Magic_3Circle_MoonShard";
        public const string Ine_Magic_3Circle_MoonTotem = "Ine_Magic_3Circle_MoonTotem";
        public const string Ine_Magic_3Circle_Shoot = "Ine_Magic_3Circle_Shoot";
        public const string Ine_Magic_4Circle = "Ine_Magic_4Circle";
        public const string Ine_4CircleForm = "Ine_4CircleForm";
        public const string Ine_CircleChange = "Ine_CircleChange";
        public const string Ine_LightSpear = "Ine_LightSpear";
        public const string Ine_Magic_Charge = "Ine_Magic_Charge";
        public const string Ine_Magic_Circle_dissolution = "Ine_Magic_Circle_dissolution";
        public const string Ine_MoonCircle01_Change = "Ine_MoonCircle01_Change";
        public const string Ine_MoonCircle02 = "Ine_MoonCircle02";
        public const string Ine_WingCircle01_Change = "Ine_WingCircle01_Change";
        public const string Ine_WingCircle02 = "Ine_WingCircle02";
        public const string Ine_MoonSlash = "Ine_MoonSlash";
        public const string Ine_Book_Appear = "FXP_Ine_Book_Summon";
        public const string Ine_Book_Disappear = "FXP_Ine_Book_Release";
        public const string Ine_Magic_2Circle_Hit = "FXP_Ine_Magic_2Circle_Hit";
        
        public const string LilpaDrain = "FXP_Blood_Drain";
        public const string LilpaSuperDrain = "FXP_Blood_SuperDrain";
        public const string LilpaBullet = "FXP_Lilpa_Bullet_Shot";
        public const string LilpaBulletCrit = "FXP_Lilpa_Bullet_Crit";
        public const string LilpaBulletHit = "FXP_Lilpa_Bullet_Shot_Hit";
        public const string LilpaShockWave = "FXP_Lilpa_Bullet_Shot_Shockwave";
        public const string LilpaDaggerThrow = "FXP_Lilpa_Dagger_Throw";
        public const string LilpaDashSlash = "FXP_Lilpa_Dash_Slash";
        public const string LilpaBlood_Appear = "FXP_Lilpa_Floating_Blood_Appear";
        public const string LilpaBlood_Loop = "FXP_Lilpa_Floating_Blood_Loop";
        public const string LilpaBlood_Dissolve = "FXP_Lilpa_Floating_Blood_Dissolve";
        public const string LilpaStar = "LilpaStar";
        public const string LilpaQuietus01 = "FXP_Lilpa_Quietus01";
        public const string LilpaQuietus02 = "FXP_Lilpa_Quietus02";
        public const string LilpaHighSpiritAppear = "FXP_Lilpa_HighSpirit_Appear";
        public const string LilpaHighSpiritLoop = "FXP_Lilpa_HighSpirit_Loop";
        public const string LilpaHighSpiritDissolve = "FXP_Lilpa_HighSpirit_Dissolve";
        public const string LilpaBloodSlash = "FXP_Lilpa_HighSpirit_BloodSlash";

        public const string JingRasengan = "FXP_JingBurger_Nasengan";
        public const string JingRasenganShoot = "FXP_JingBurger_Nasengan_Shoot";
        public const string JingRasenganExplosion = "FXP_JingBurger_Nasengan_Explosion";
        public const string JingRasenganCharge = "FXP_JingBurger_Nasengan_Charge";
        public const string JingRasenganRush = "FXP_JingBurger_Nasengan_Rush";
        public const string JingShuriken = "FXP_JingBurger_Shuriken";
        public const string JingPaint = "FXP_JingBurger_Kunai";
        public const string JingPaintHit = "FXP_JingBurger_KunaiHit";
        public const string JingAnimalBird = "FXP_JingBurger_Animal_Bird";

        public const string JururuFoxGunnerBullet = "FXP_Jururu_FoxGunner_Bullet";
        public const string JururuFoxGunnerShoot = "FXP_Jururu_FoxGunner_Shoot";
        public const string JururuFoxMagicianReady = "FXP_Jururu_FoxMagician_Ready";
        public const string JururuFoxMagicianShoot = "FXP_Jururu_FoxMagician_Shoot";
        public const string JururuFoxSoldierHit = "FXP_Jururu_FoxSoldier_Hit";
        public const string JururuFoxSoldierRelease = "FXP_Jururu_FoxSoldier_Release";
        public const string JururuFoxSoldierSummon = "FXP_Jururu_FoxSoldier_Summon";
        public const string JururuFoxSoldierSummonLoop = "FXP_Jururu_FoxSoldier_Summon_Loop";
        public const string JururuSoulKnightFireball = "FXP_Jururu_SoulKnight_Fireball";
        public const string JururuSoulKnightRelease = "FXP_Jururu_SoulKnight_Release";
        public const string JururuSoulKnightScratch = "FXP_Jururu_SoulKnight_Scratch";
        public const string JururuSoulKnightScratchFin = "FXP_Jururu_SoulKnight_ScratchFin";
        public const string JururuSoulKnightSummon = "FXP_Jururu_SoulKnight_Summon";
        public const string JururuSoulKnightTeleport = "FXP_Jururu_SoulKnight_Teleport";
        
        public const string ViichanGuard = "FXP_Shield_Guard";
        public const string ViichanFlowerForm = "FXP_Flower_Form";
        public const string ViichanShieldOn = "FXP_Shield_On";
        public const string ViichanShieldLoop = "FXP_Shield_Loop";
        public const string ViichanShieldBroken = "FXP_Shield_Broken";
        public const string ViichanShieldGaugeFull = "FXP_Shield_Gauge_Full";
        public const string ViichanShieldGaugeLoop = "FXP_Shield_Gauge_Loop";
        public const string ViichanThornForm = "FXP_Thorn_Form";
        public const string ViichanShieldParrying = "FXP_Shield_Parrying";
        public const string ViichanCounter = "FXP_Viichan_Shield_CounterLance";
        public const string ViichanBeastClaw = "GrudgeClaws";
        public const string ViichanBeastAttack1 = "FXP_Viichan_Beast_Attack1";
        public const string ViichanBeastAttack2 = "FXP_Viichan_Beast_Attack2";
        public const string ViichanBeastAura = "FXP_Viichan_Beast_Aura";
        public const string ViichanBeastHealing = "FXP_Viichan_Beast_Healing";
        public const string ViichanBeastShield = "FXP_Viichan_Beast_Shield";
        public const string ViichanBeastShieldHorn = "FXP_Viichan_Beast_Shield_Horn";
        public const string ViichanBeastShieldHornFull = "FXP_Viichan_Beast_Shield_Horn_Full";
        public const string ViichanBeastTransform = "FXP_Viichan_Beast_Transform";
        public const string ViichanBeastTransformHowling = "FXP_Viichan_Beast_Transform_Howling";
        public const string ViichanShieldCounterLanceHit = "FXP_Viichan_Shield_CounterLance_Hit";
        public const string ViichanShieldDashHit = "FXP_Viichan_Shield_Dash_Hit";
        public const string ViichanShieldSword = "FXP_Viichan_Shield_Sword";

        public const string GoseguLockOnStart = "FXP_Gosegu_D_Missile_LockOn_Start";
        public const string GoseguLockOn = "FXP_Gosegu_D_Missile_LockOn";
        public const string GoseguLockOnEnd = "FXP_Gosegu_D_Missile_LockOn_End";
        public const string GoseguDroneIcicle = "FXP_Gosegu_D_Main_Icicle";
        public const string GoseguDroneIcicleExplosion = "FXP_Gosegu_D_Main_Icicle_explosion";
        public const string GoseguDroneIcicleHit = "FXP_Gosegu_D_Main_Icicle_Hit";
        public const string GoseguDroneIcicleShoot = "FXP_Gosegu_D_Main_Icicle_Shoot";
        public const string GoseguMechaBullet = "FXP_Gosegu_Mechanic_Projectile";
        public const string GoseguMechaCharge = "FXP_Gosegu_Mechanic_Ray_Charge";
        public const string GoseguMechaRide = "FXP_Gosegu_Mechanic_Ride";
        public const string GoseguMechaDash = "FXP_Gosegu_Mechanic_Dash";
        public const string GoseguMechaMove = "FXP_Gosegu_Mechanic_Move";
        public const string GoseguMechaAtkEffect = "FXP_Gosegu_Mechanic_Hit";
        public const string GoseguMechaCannonHit = "FXP_Gosegu_Mechanic_Cannon_Hit";
    }
    public static class JururuBossEffect
    {
        public enum EffectType
        {
            JururuAttack1,JururuAttack1_2,JururuAttack1Hit,JururuAttack1Hit2,JururuAttack3Appear,JururuAttack3AppearBlue,
            JururuAttack3Dis,JururuAttack3DisBlue,JururuAttack3Hit,JururuAttack3HitBlue,JururuAttack3Throw,JururuAttack3ThrowBlue,
            JururuAttack4Appear,JururuAttack4Dissolve,JururuAttack4Loop,JururuAttack4Ready,
            JururuAttack5Aura,JururuAttack5AuraAppear,JururuAttack5Slash,JururuAttack5Teleport,JururuAttack5Sun,
            JururuShieldAppear,JururuShieldLoop,JururuShieldDis,
            JururuTransformScreen,JururuTransformStart,JururuTransLoopStart,JururuTransLoop,JururuTransLoopEnd,
            JururuTransformEndStart,JururuTransformEnd,JururuDash ,
            JururuFireArrow,
            JururuDropAttack,
        }
        
        public static string Get(EffectType type)
        {
            return type switch
            {
                EffectType.JururuAttack1 => "FXP_Boss_Jururu_Attack01_01",
                EffectType.JururuAttack1_2 => "FXP_Boss_Jururu_Attack01_02",
                EffectType.JururuAttack1Hit => "FXP_Boss_Jururu_Attack01_01_Hit",
                EffectType.JururuAttack1Hit2 => "FXP_Boss_Jururu_Attack01_02_Hit",
                EffectType.JururuAttack3Appear => "FXP_Boss_Jururu_Attack03_Appear",
                EffectType.JururuAttack3AppearBlue => "FXP_Boss_Jururu_Attack03_Appear_Blue",
                EffectType.JururuAttack3Dis => "FXP_Boss_Jururu_Attack03_Diss",
                EffectType.JururuAttack3DisBlue => "FXP_Boss_Jururu_Attack03_Diss_Blue",
                EffectType.JururuAttack3Hit => "FXP_Boss_Jururu_Attack03_Hit",
                EffectType.JururuAttack3HitBlue => "FXP_Boss_Jururu_Attack03_Hit_Blue",
                EffectType.JururuAttack3Throw => "FXP_Boss_Jururu_Attack03_Throw",
                EffectType.JururuAttack3ThrowBlue => "FXP_Boss_Jururu_Attack03_Throw_Blue",
                EffectType.JururuAttack4Appear => "FXP_Boss_Jururu_Attack04_Appear",
                EffectType.JururuAttack4Dissolve => "FXP_Boss_Jururu_Attack04_Dissolve",
                EffectType.JururuAttack4Loop => "FXP_Boss_Jururu_Attack04_Loop",
                EffectType.JururuAttack4Ready => "FXP_Boss_Jururu_Attack04_Ready",
                EffectType.JururuAttack5Aura => "FXP_Boss_Jururu_Attack05_Fire_Aura",
                EffectType.JururuAttack5AuraAppear => "FXP_Boss_Jururu_Attack05_Fire_Aura_Appear",
                EffectType.JururuAttack5Slash => "FXP_Boss_Jururu_Attack05_Slash",
                EffectType.JururuAttack5Teleport => "FXP_Boss_Jururu_Attack05_Teleport",
                EffectType.JururuAttack5Sun => "JururuBossSun",
                EffectType.JururuShieldAppear => "FXP_Boss_Jururu_Shield_Appear",
                EffectType.JururuShieldLoop => "FXP_Boss_Jururu_Shield_Loop",
                EffectType.JururuShieldDis => "FXP_Boss_Jururu_Shield_Diss",
                EffectType.JururuTransformScreen => "FXP_Boss_Jururu_PostProcess_Fire",
                EffectType.JururuTransformStart => "FXP_Boss_Jururu_Transform_Start",
                EffectType.JururuTransLoopStart => "FXP_Boss_Jururu_Transform_Loop_Start",
                EffectType.JururuTransLoop => "FXP_Boss_Jururu_Transform_Loop",
                EffectType.JururuTransLoopEnd => "FXP_Boss_Jururu_Transform_Loop_End",
                EffectType.JururuTransformEndStart => "FXP_Boss_Jururu_Transform_End",
                EffectType.JururuTransformEnd => "FXP_Boss_Jururu_Transform_End_Explosion",
                EffectType.JururuDash => "FXP_Boss_Jururu_Dash",
                EffectType.JururuFireArrow => "JururuFireArrow",
                EffectType.JururuDropAttack => "FXP_Jururu_DropAttack_dummy",
                _ => ""
            };
        }
    }
    
    public struct DummyEffects
    {
        public const string Explosion = "BaseExplosion";
        public const string ExplosionNoAttack = "BaseExplosionNoAttack";
        public const string KnifeProjectile = "KnifeProjectile";
        public const string MagicSwordProjectile = "MagicSwordProjectile";
        public const string MagicScytheProjectile = "MagicScytheProjectile";
    }

    public struct EtcEffects
    {
        public const string BlackOut = "FXP_Black_Out";
        public const string Bleeding = "FXP_Bleeding";
        public const string Burn = "FXP_Burn";
        public const string ColdAir = "FXP_Cold_Air";
        public const string AccDropStart = "FXP_Item_Drop_Acc_Start";
        public const string AccDropLoop = "FXP_Item_Drop_Acc_Loop";
        public const string WeaponDropStart = "FXP_Item_Drop_Weapon_Start";
        public const string WeaponDropLoop = "FXP_Item_Drop_Weapon_Loop";
        public const string CollectionDropStart = "FXP_Item_Drop_Collection_Start";
        public const string CollectionDropLoop = "FXP_Item_Drop_Collection_Loop";
        public const string Poison = "FXP_Poison";
        public const string Recovery = "FXP_Recovery";
    }

    public struct WeaponEffects
    {
        public const string MagicSlash = "MagicSlash";
    }

    public struct Animations
    {
        public const string GoseguOverrider = "GoseguOverrider";
        public const string IneOverrider = "IneOverrider";
        public const string JururuOverrider = "JururuOverrider";
        public const string JingburgerOverrider = "JingburgerOverrider";
        public const string LilpaOverrider = "LilpaOverrider";
        public const string ViichanOverrider = "ViichanOverrider";
    }

    public static class BossEffects
    {
        public const string GiantSpiritProjectile = "GiantSpiritProjectile";
        public const string GiantSpiritBlackHole = "BlackHole";
    }
    #endregion
    
    #region 오브젝트
    
    public static class PlayerSkillObjects
    {
        public const string JingPuppyPaint = "PuppyPaint";
        public const string JingPuppyMine = "JingPuppyMine";
        public const string JingWhalePaint = "WhalePaint";
        
        public const string GoseguMainDrone = "GoseguMainDrone";
        public const string SeguMecha = "SeguMecha";
        public const string SeguMechaCannon = "FXP_Gosegu_Mechanic_Projectile";
        public const string GoseguMissile = "DroneMissile";
        public const string GoseguMissileDrone = "GoseguMissileDrone";
        public const string GoseguLaser = "DroneLaser";
        public const string GoseguLaserDrone = "GoseguLaserDrone";
        public const string GoseguHealDrone = "GoseguHealDrone";
        public const string GoseguFreezeBullet = "DroneFreezeBullet";
        public const string GoseguFreezeExplosion = "DroneFreezeExplosion";
        public const string GoseguFreezeDrone = "GoseguFreezeDrone";
        
        
        public const string IneCircle4Beam = "circle4Beam";
        public const string IneSmallFeather = "IneSmallFeather";
        public const string IneBook = "IneBook";
        
        public const string LilpaDagger = "LilpaDagger";
        public const string LilpaBulletFragment = "BulletFragment";

        public const string JururuFoxSoldier = "NormalFoxSoldier";
        public const string JururuGunFoxSoldier = "GunFoxSoldier";
        public const string JururuMagicFoxSoldier = "MagicFoxSoldier";
        public const string JururuShieldFoxSoldier = "ShieldFoxSoldier";
        public const string JururuPokdoFlameWall = "PokdoFireWall";
        public const string JururuPokdoGrab = "PokdoGrab";

        public const string ViichanGrab = "ViichanGrab";
    }

    public static class CommonObjects
    {
        public const string TriggerEventCollider = "TriggerCollider";
        public const string PlayerAtkCollider = "PlayerAtkEffect";
    }
    public static class AccessoryObjects
    {
        public const string Icicle = "Icicle";
        public const string ButterflyCompassArrow = "ButterflyCompassArrow";
        public const string BlackChainEffect = "BlackChainEffect";
        public const string BloodArrowEffect = "BleedArrowDummy";
        public const string FancyDualBladeEffect = "FancyBladeAttack";
    }

    public static class BossObjects
    {
        public const string JururuFlameGround = "FlameGround";
        public const string TutorialBossProjectile = "TutorialBossProjectile";
    }

    public static class StageObjects
    {
        public const string Laser = "Laser";
    }
    #endregion
    
    #region 데이터

    public class PlayerData
    {
        public const string IneData = "IneData";
        public const string JingburgerData = "JingburgerData";
        public const string LilpaData = "LilpaData";
        public const string JururuData = "JururuData";
        public const string GoseguData = "GoseguData";
        public const string ViichanData = "ViichanData";
    }
    #endregion

    #region 씬이름

    public class SceneNames
    {
        public const string TitleSceneName = "Title";
        public const string LobbySceneName = "Lobby";
        public const string MainWorldSceneName = "MainWorld";
    }

    #endregion

    #region 비디오
    

    #endregion
    #endregion
}


#region 효과 및 스탯관련 Enums
public enum ValueType
{
    Value, Ratio
}
public enum ActorStatType // 스탯 종류
{
    Atk = 0,
    Def,
    AtkSpeed,
    MoveSpeed,
    CritProb,
    CritDmg,
    CDReduction,
    Mental,
    MaxHp,
    DmgReduce,
    ExtraDmg,
    GoldRate,
    HealRate,
    ShieldRate,
    CritHit,
    Body,
    Spirit,
    Finesse,
    CastingSpeed,
    CastingDmg
}

public enum ItemType // 아이템 타입
{
    Accessory,
    Weapon,
    HpPotion,
    AtkItem,
    Magic,
    StatBonus,
}

public enum EventType // 효과 적용 조건
{
    OnEnable, // 켜질 때
    OnDisable, // 꺼질 때
    OnColliderAttack, // 근거리 공격 시 (콜라이더로 공격할 때)
    OnWeaponSlash, // 무기 휘두를 때
    OnAttack, // 공격 휘두를 때 (안맞아도됨)
    OnAttackSuccess, // 공격 성공 시
    OnBasicAttack, // 기본 공격 성공시
    OnAfterAtk, // 공격 후 (입힌 데미지 확인해야될 때)
    OnBackAttack, // 백어택
    OnAttackEnd, // 공격 모션 끝날 때
    OnJump, // 점프 시
    OnBeforeHit, // 피격 직전 (무적, 방어 등)
    OnHit, // 피격시
    OnHitReaction, // 피격 효과 발동시 (플레이어)
    OnAfterHit, // 피격 시 (데미지 증감이 적용된 후 피격)
    OnHpDown, // 체력 감소 시
    OnKill, // 적 처치 시
    OnSkill, // 고유 스킬 사용 시
    OnSkillEnd, // 고유 스킬 종료 시
    OnSubBuffTaken, // 서브버프 받을 시
    OnSubBuffRemove, // 서브버프 제거될 시
    OnDash, // 회피 사용 시
    OnRepair, // 수리 사용 시
    OnCrit, // 크리티컬 데미지 시
    OnCritHit, // 크리티컬 피격 시
    OnDeath, // 사망시
    OnTriggerEnter, // 충돌시
    OnTriggerExit, // 충돌 해제시 
    OnSubBuffApply, // 적에게 서브버프 적용시
    OnUpdate, // 업데이트
    OnFixedUpdate, // 물리 업데이트
    OnWeaponSkillUse, // 무기 스킬 사용시
    OnDestroy, // 파괴시
    OnRecognitionEnter, // 인식 상태 돌입 (몬스터)
    OnRecognitionExit, // 인식 상태 탈출 (몬스터)
    OnStateChanged, // 상태 변환 (몬스터)
    OnBuffGroupAdd, // 효과 장착시
    OnBuffGroupRemove, // 효과 해제시 
    OnHpHeal, // 체력 회복시
    OnCollide, // 충돌시
    OnRepairCharge,
    OnMove,
    OnBeforeHpDown, // 체력 감소 직전
    OnBarrierChange, // 쉴드값 변경시
    OnAttackStateEnter, // 공격상태 진입시
    OnAttackStateExit, // 공격상태 해제시
    OnInit, // 초기화시
    OnCutScene,
    OnTargetFirstAttack, // 각 타겟을 첫 공격시 (타겟마다 호출됨)
    OnFirstAttack, // 첫 타격시 (공격당 한번만 적용)
    OnChargeEnd, // 차징 완료 시
    OnCastingEnd, // 캐스팅 완료 시
    OnChargeCancel, // 차징 캔슬 시
    OnCastingCancel, // 캐스팅 캔슬 시
    OnLanding, // 바닥 착지 시
    OnAirEnter, // 공중 진입 시
    OnIdle,     // idle state 진입 시
    OnStop,     // 이동 멈출 시
    OnEventState, // EventState 전환 시
    OnIdleMotion,
    OnTurn,
    OnKnockbackComplete,
    OnClimbStart,
    OnClimbMotionStart,
    OnClimbEnd,
    OnAnyState,
    OnCutSceneEnd,
    OnDrillComplete,
}

#region 버프,디버프 관련
public enum SubBuffType // 버프 종류, 클래스랑 이름 똑같이 해야함
{
    Buff_DoubleJump, Debuff_SequenceStack, Buff_Warmth,MarkStack,StarEye,Celerity,HunterStack,RefinedAnger,DisCharge,ExpansionBullet,

    Buff_Atk,Buff_Def, Buff_AtkSpeed, Buff_MoveSpeed, Buff_CritProb, Buff_CritDmg, Buff_CD, Buff_Mentality, Buff_MaxHp, Buff_DmgReduce, Buff_ExtraDmg, Buff_GoldRate,Buff_HealRate,Buff_ShieldRate, Buff_AllStat,

    Debuff_Atk,Debuff_Def, Debuff_AtkSpeed, Debuff_MoveSpeed, Debuff_CritProb, Debuff_CritDmg, Debuff_CD, Debuff_Mentality, Debuff_MaxHp, Debuff_DmgReduce, Debuff_ExtraDmg, Debuff_GoldRate, Debuff_HealRate,Debuff_ShieldRate,

    Buff_BasicAtkEnhance,
    
    Debuff_Stun, Debuff_Chill, Debuff_Frozen, Debuff_Grab, Debuff_KnockBack, Debuff_BonusDmg, Debuff_Execution, Debuff_Tied,

    Buff_Immune, Buff_Remove,

    Debuff_Burn, Debuff_Poison, Debuff_Bleed,Debuff_RasenganResidue,

    Buff_MaxHpRegen, Buff_LostHpRegen, Buff_HpRegen, Buff_LifeSteal, Buff_ShieldHpRegen,

    Buff_Barrier, Buff_DefBarrier,

    Debuff_MaxHpLose, Debuff_LostHpLose,Debuff_HpLose, Debuff_AtkHpLose, Debuff_CurHpLose,Debuff_GroggyLose,
    
    Buff_Sight,Buff_AirDashCount,Buff_RepairValue, Buff_RepairCount,Buff_WeaponCD,Buff_SkillDmg,Buff_DashRange,
    
    Buff_ActiveDuration, Buff_ActiveCD,Buff_ActiveCurCD,
    
    None
}

#endregion

#region 플레이어 스테이트 관련
public static class NextState
{
    public static EPlayerState[] Get(EPlayerState currentState)
    {
        return currentState switch 
        {
            EPlayerState.Idle => new[] { EPlayerState.AirIdle, EPlayerState.Move, EPlayerState.Jump, EPlayerState.Attack, EPlayerState.Dash, EPlayerState.Heal,EPlayerState.Skill,EPlayerState.Crouch, EPlayerState.Drop, EPlayerState.Heal },
            EPlayerState.AirIdle => new[] { EPlayerState.Idle, EPlayerState.AirMove, EPlayerState.Jump, EPlayerState.Attack, EPlayerState.Dash, EPlayerState.Skill, EPlayerState.Climb },
            EPlayerState.Move => new[] { EPlayerState.AirIdle, EPlayerState.AirMove, EPlayerState.Jump, EPlayerState.Attack, EPlayerState.Dash, EPlayerState.Heal, EPlayerState.Skill, EPlayerState.Stop, EPlayerState.Drop, EPlayerState.Heal },
            EPlayerState.AirMove => new[] { EPlayerState.AirIdle, EPlayerState.Move, EPlayerState.Jump, EPlayerState.Attack, EPlayerState.Dash, EPlayerState.Skill,EPlayerState.Stop, EPlayerState.Climb },
            EPlayerState.Crouch => new[] { EPlayerState.Idle, EPlayerState.Drop, EPlayerState.Move, EPlayerState.Heal, EPlayerState.Jump },
            EPlayerState.Dash => new[] { EPlayerState.Jump, EPlayerState.Climb },
            EPlayerState.DashLanding => new [] { EPlayerState.Jump },
            EPlayerState.Stop => new[] { EPlayerState.Attack },
            EPlayerState.AttackWaiting => new[] { EPlayerState.Dash },
            EPlayerState.AirAttackWaiting => new[] { EPlayerState.Dash },
            EPlayerState.Run => new[] { EPlayerState.Run, EPlayerState.AirIdle, EPlayerState.AirMove, EPlayerState.Jump, EPlayerState.Attack, EPlayerState.Dash, EPlayerState.Heal, EPlayerState.Skill, EPlayerState.Stop, EPlayerState.Drop, EPlayerState.Heal, EPlayerState.Move },
            EPlayerState.AirRun => new[] { EPlayerState.Run, EPlayerState.AirIdle, EPlayerState.Move, EPlayerState.Jump, EPlayerState.Attack, EPlayerState.Dash,EPlayerState.Skill,EPlayerState.Stop, EPlayerState.Climb },
            EPlayerState.IceDrillCharge => new[] { EPlayerState.IceDrillCharge, EPlayerState.AirIdle, EPlayerState.AirMove, EPlayerState.Jump, EPlayerState.Attack, EPlayerState.Heal, EPlayerState.Skill, EPlayerState.Stop, EPlayerState.Drop, EPlayerState.Heal, EPlayerState.Move },
            // EPlayerState.Damaged => new[] { EPlayerState.Attack, EPlayerState.Skill },
            _ => null
        };
    }
}
#endregion

#region 플레이어 애니메이터
public enum EAnimationBool
{
    Idle,
    IsMove,
    Turn,
    OnAir,
    IsCrouch,
    IsDash,
    SkillAir,
    IsSummon,
    IsBeast,
    IsShield,
    IsFrontKnockback,
    IsRun,
    IsIceDrill,
}
public enum EAnimationTrigger
{
    Climb,
    ClimbLow,
    Turn,
    Jump,
    Attack,
    Heal,
    Damaged,
    Dead,
    Interact,
    IdleOn,
    CancelMotion,
    AttackEnd,
    IdleFix,
    WeaponSkill,
    WeaponSkillEnd,
    WeaponSkillIndex,
    Charge,
    ChargeEnd,
    RushEnd,
    PlayerSkill,
    PlayerSkillEnd,
    Casting,
    CounterAtk,
    Stop,
    Dash,
    AttackInit,
    PreAttack,
    StepAttack,
    KnockbackEnter,
    KnockbackEnd,
    IdleFixOff,
    InteractEnd,
    CutsceneOn,
    CutsceneEnd,
    IceDrillOn,
    Drop,
}
public enum EAnimationInt
{
    Direction,
    MaxGroundAtk,
    MaxAirAtk,
    AttackType,
    WeaponSkillType,
    DashType,
    ChargingType,
}
public enum EAnimationFloat
{
    AtkSpeed,
    MoveMultiplier,
    PotionTime,
    MoveSpeed,
}
#endregion
#endregion
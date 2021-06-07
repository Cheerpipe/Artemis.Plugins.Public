using ProcessMemory;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SRTPluginProviderRE8.Structs.GameStructs
{
    public class PlayerStatus
    {
        public bool IsEthan { get; internal set; }
        public bool IsChris { get; internal set; }
        public bool IsEnableUpdate { get; private set; }
        public bool IsIdleUpperBody { get; private set; }
        public bool IsGameOver { get; private set; }
        public bool IsBlowDamage { get; private set; }
        public bool IsBlowLand { get; private set; }
        public bool IsHandsGuard { get; private set; }
        public bool IsMeleeGuard { get; private set; }
        public bool IsGunGuard { get; private set; }
        public bool IsExternalGuard { get; private set; }
        public bool IsFrontForbidAttackTarget { get; private set; }
        public bool IsSlipDamage { get; private set; }
        public bool IsNotifyAcidDamage { get; private set; }
        public bool IsInfiniteBulletByFsmAction { get; private set; }
        public bool IsHideShelf { get; private set; }
        public bool IsHollow { get; private set; }
        public bool IsDisableUpperRotate { get; private set; }

        public long BaseAcionID { get; private set; }
        public long UpperActionID { get; private set; }
        public long LArmUpperActionID { get; private set; }
        public long EventActionID { get; private set; }

        public bool IsMeleeAction { get; private set; }
        public bool IsChrisPunch { get; private set; }
        public bool IsGunAttack { get; private set; }
        public bool IsGunAttackLoop { get; private set; }
        public bool IsEnableChangeFovByAim { get; private set; }
        public bool IsSprintForbiddenByOrder { get; private set; }
        public bool IsCrouchForbidden { get; private set; }
        public bool IsForceDisableProgramMovement { get; private set; }
        public bool IsAttackForbiddenByOrder { get; private set; }
        public bool IsGuardForbiddenByOrder { get; private set; }
        public bool IsUpperBodyActionForbiddenByOrder { get; private set; }
        public bool IsInputForbiddenByGUI { get; private set; }
        public bool IsInShop { get; private set; }
        public bool IsInInventoryMenu { get; private set; }
        public bool IsInSelectMenu { get; private set; }
        public bool IsEnableColdBreath { get; private set; }
        public bool IsInSlipArea { get; private set; }
        public bool IsInEscapeSlipArea { get; private set; }
        public bool IsOnBridge { get; private set; }
        public bool IsOnWaterSurface { get; private set; }
        public bool IsExecuteThreeGunMatch { get; private set; }
        public bool IsUseLeftHandWeapon { get; private set; }
        public bool IsUseLeftHandSequence { get; private set; }
        public bool IsDisableUseMine { get; private set; }
        public bool IsFixedAimMode { get; private set; }
        public bool IsEnableUseLaserIrradiation { get; private set; }

        public long PlayerReference { get; private set; }

        public bool IsInWaterAreaCam { get; private set; }
        public bool IsNoReduceBullet { get; private set; }
        public bool IsLoadingNumDouble { get; private set; }
        public bool IsForbidReloadCommand { get; private set; }
        public bool IsForbidAim { get; private set; }

        public PlayerStatus()
        {
        }

        public void Update(GamePlayerStatus gs)
        {
            IsEthan = gs.IsEthan;
            IsChris = gs.IsChris;
            IsEnableUpdate = gs.IsEnableUpdate;
            IsIdleUpperBody = gs.IsIdleUpperBody;
            IsGameOver = gs.IsGameOver;
            IsBlowDamage = gs.IsBlowDamage;
            IsBlowLand = gs.IsBlowLand;
            IsHandsGuard = gs.IsHandsGuard;
            IsMeleeGuard = gs.IsMeleeGuard;
            IsGunGuard = gs.IsGunGuard;
            IsExternalGuard = gs.IsExternalGuard;
            IsFrontForbidAttackTarget = gs.IsFrontForbidAttackTarget;
            IsSlipDamage = gs.IsSlipDamage;
            IsNotifyAcidDamage = gs.IsNotifyAcidDamage;
            IsInfiniteBulletByFsmAction = gs.IsInfiniteBulletByFsmAction;
            IsHideShelf = gs.IsHideShelf;
            IsHollow = gs.IsHollow;
            IsDisableUpperRotate = gs.IsDisableUpperRotate;

            BaseAcionID = gs.BaseAcionID;
            UpperActionID = gs.UpperActionID;
            LArmUpperActionID = gs.LArmUpperActionID;
            EventActionID = gs.EventActionID;

            IsMeleeAction = gs.IsMeleeAction;
            IsChrisPunch = gs.IsChrisPunch;
            IsGunAttack = gs.IsGunAttack;
            IsGunAttackLoop = gs.IsGunAttackLoop;
            IsEnableChangeFovByAim = gs.IsEnableChangeFovByAim;
            IsSprintForbiddenByOrder = gs.IsSprintForbiddenByOrder;
            IsCrouchForbidden = gs.IsCrouchForbidden;
            IsForceDisableProgramMovement = gs.IsForceDisableProgramMovement;
            IsAttackForbiddenByOrder = gs.IsAttackForbiddenByOrder;
            IsGuardForbiddenByOrder = gs.IsGuardForbiddenByOrder;
            IsUpperBodyActionForbiddenByOrder = gs.IsUpperBodyActionForbiddenByOrder;
            IsInputForbiddenByGUI = gs.IsInputForbiddenByGUI;
            IsInShop = gs.IsInShop;
            IsInInventoryMenu = gs.IsInInventoryMenu;
            IsInSelectMenu = gs.IsInSelectMenu;
            IsEnableColdBreath = gs.IsEnableColdBreath;
            IsInSlipArea = gs.IsInSlipArea;
            IsInEscapeSlipArea = gs.IsInEscapeSlipArea;
            IsOnBridge = gs.IsOnBridge;
            IsOnWaterSurface = gs.IsOnWaterSurface;
            IsExecuteThreeGunMatch = gs.IsExecuteThreeGunMatch;
            IsUseLeftHandWeapon = gs.IsUseLeftHandWeapon;
            IsUseLeftHandSequence = gs.IsUseLeftHandSequence;
            IsDisableUseMine = gs.IsDisableUseMine;
            IsFixedAimMode = gs.IsFixedAimMode;
            IsEnableUseLaserIrradiation = gs.IsEnableUseLaserIrradiation;

            PlayerReference = gs.PlayerReference;

            IsInWaterAreaCam = gs.IsInWaterAreaCam;
            IsNoReduceBullet = gs.IsNoReduceBullet;
            IsLoadingNumDouble = gs.IsLoadingNumDouble;
            IsForbidReloadCommand = gs.IsForbidReloadCommand;
            IsForbidAim = gs.IsForbidAim;
        }

    }
}
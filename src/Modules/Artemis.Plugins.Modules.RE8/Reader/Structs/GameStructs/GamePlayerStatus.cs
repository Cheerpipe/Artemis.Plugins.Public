using System.Runtime.InteropServices;

namespace SRTPluginProviderRE8.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]

    public unsafe struct GamePlayerStatus
    {
        [FieldOffset(0x78)] public bool IsEthan;
        [FieldOffset(0x79)] public bool IsChris;
        [FieldOffset(0x7A)] public bool IsEnableUpdate;
        [FieldOffset(0x7B)] public bool IsIdleUpperBody;
        [FieldOffset(0x7C)] public bool IsGameOver;
        [FieldOffset(0x7D)] public bool IsBlowDamage;
        [FieldOffset(0x7E)] public bool IsBlowLand;
        [FieldOffset(0x7F)] public bool IsHandsGuard;
        [FieldOffset(0x80)] public bool IsMeleeGuard;
        [FieldOffset(0x81)] public bool IsGunGuard;
        [FieldOffset(0x82)] public bool IsExternalGuard;
        [FieldOffset(0x83)] public bool IsFrontForbidAttackTarget;
        [FieldOffset(0x84)] public bool IsSlipDamage;
        [FieldOffset(0x85)] public bool IsNotifyAcidDamage;
        [FieldOffset(0x86)] public bool IsInfiniteBulletByFsmAction;
        [FieldOffset(0x87)] public bool IsHideShelf;
        [FieldOffset(0x88)] public bool IsHollow;
        [FieldOffset(0x89)] public bool IsDisableUpperRotate;
        [FieldOffset(0x90)] public long BaseAcionID;
        [FieldOffset(0x98)] public long UpperActionID;
        [FieldOffset(0xA0)] public long LArmUpperActionID;
        [FieldOffset(0xA8)] public long EventActionID;
        [FieldOffset(0xB0)] public bool IsMeleeAction;
        [FieldOffset(0xB1)] public bool IsChrisPunch;
        [FieldOffset(0xB2)] public bool IsGunAttack;
        [FieldOffset(0xB3)] public bool IsGunAttackLoop;
        [FieldOffset(0xB4)] public bool IsEnableChangeFovByAim;
        [FieldOffset(0xB5)] public bool IsSprintForbiddenByOrder;
        [FieldOffset(0xB6)] public bool IsCrouchForbidden;
        [FieldOffset(0xB7)] public bool IsForceDisableProgramMovement;
        [FieldOffset(0xB8)] public bool IsAttackForbiddenByOrder;
        [FieldOffset(0xB9)] public bool IsGuardForbiddenByOrder;
        [FieldOffset(0xBA)] public bool IsUpperBodyActionForbiddenByOrder;
        [FieldOffset(0xBB)] public bool IsInputForbiddenByGUI;
        [FieldOffset(0xBC)] public bool IsInShop;
        [FieldOffset(0xBD)] public bool IsInInventoryMenu;
        [FieldOffset(0xBE)] public bool IsInSelectMenu;
        [FieldOffset(0xBF)] public bool IsEnableColdBreath;
        [FieldOffset(0xC0)] public bool IsInSlipArea;
        [FieldOffset(0xC1)] public bool IsInEscapeSlipArea;
        [FieldOffset(0xC2)] public bool IsOnBridge;
        [FieldOffset(0xC3)] public bool IsOnWaterSurface;
        [FieldOffset(0xC4)] public bool IsExecuteThreeGunMatch;
        [FieldOffset(0xC5)] public bool IsUseLeftHandWeapon;
        [FieldOffset(0xC6)] public bool IsUseLeftHandSequence;
        [FieldOffset(0xC7)] public bool IsDisableUseMine;
        [FieldOffset(0xC8)] public bool IsFixedAimMode;
        [FieldOffset(0xC9)] public bool IsEnableUseLaserIrradiation;
        [FieldOffset(0xD0)] public long PlayerReference;
        [FieldOffset(0xD8)] public bool IsInWaterAreaCam;
        [FieldOffset(0xD9)] public bool IsNoReduceBullet;
        [FieldOffset(0xDA)] public bool IsLoadingNumDouble;
        [FieldOffset(0xDB)] public bool IsForbidReloadCommand;
        [FieldOffset(0xDC)] public bool IsForbidAim;

        public static GamePlayerStatus AsStruct(byte[] data)
        {
            fixed (byte* pb = &data[0])
            {
                return *(GamePlayerStatus*)pb;
            }
        }
    }
}
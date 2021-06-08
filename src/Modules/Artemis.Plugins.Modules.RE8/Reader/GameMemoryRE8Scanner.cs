using ProcessMemory;
using static ProcessMemory.Extensions;
using System;
using System.Diagnostics;
using SRTPluginProviderRE8.Structs;
using System.Text;
using SRTPluginProviderRE8.Structs.GameStructs;
using Serilog;

namespace SRTPluginProviderRE8
{
    internal class GameMemoryRE8Scanner : IDisposable
    {
        private static readonly int MAX_ENTITIES = 64;
        private static readonly int MAX_ITEMS = 256;

        // Variables
        private ProcessMemoryHandler memoryAccess;
        private GameMemoryRE8 gameMemoryValues;
        public bool HasScanned;
        private ILogger logger;
        public bool ProcessRunning => memoryAccess != null && memoryAccess.ProcessRunning;
        public int ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;
        private int EnemyTableCount;
        private int InventoryTableCount;

        // Pointer Address Variables
        private int pointerEventActionTask;

        private int pointerPropsManager;

        private int pointerRankManager;
        private int pointerInventory;

        private int pointerAddressEnemies;
        private int pointerAddressItems;

        // Pointer Classes
        private IntPtr BaseAddress { get; set; }
        //private MultilevelPointer PointerCurrentView { get; set; }
        private MultilevelPointer PointerEventActionTask { get; set; }
        private MultilevelPointer PointerEventActionType { get; set; }
        private MultilevelPointer PointerIsMotionPlay { get; set; }
        private MultilevelPointer PointerPlayerStatus { get; set; }
        private MultilevelPointer PointerPlayerHP { get; set; }
        private MultilevelPointer PointerPlayerPosition { get; set; }
        private MultilevelPointer PointerRankManager { get; set; }
        private MultilevelPointer PointerInventory { get; set; }

        // Enemy Pointers
        private MultilevelPointer PointerEnemyCount { get; set; }
        private MultilevelPointer PointerEnemyEntryList { get; set; }
        private MultilevelPointer[] PointerEnemyEntries { get; set; }

        // Inventory Pointers
        private MultilevelPointer PointerInventoryCount { get; set; }
        private MultilevelPointer PointerInventoryEntryList { get; set; }
        private MultilevelPointer[] PointerInventoryEntries { get; set; }
        private MultilevelPointer[] PointerInventoryEntriesCustom { get; set; }

        internal GameMemoryRE8Scanner(Process process = null, ILogger logger = null)
        {
            gameMemoryValues = new GameMemoryRE8();
            if (process != null)
                Initialize(process, logger);
        }

        internal unsafe void Initialize(Process process, ILogger logger)
        {
            this.logger = logger;
            if (process == null)
                return; // Do not continue if this is null.

            if (!SelectPointerAddresses(GameHashes.DetectVersion(process.MainModule.FileName)))
                return; // Unknown version.

            int pid = GetProcessId(process).Value;
            memoryAccess = new ProcessMemoryHandler(pid);
            if (ProcessRunning)
            {
                BaseAddress = NativeWrappers.GetProcessBaseAddress(pid, PInvoke.ListModules.LIST_MODULES_64BIT); // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.

                // Setup the pointers.
                //PointerPlayerHP = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerAddressHP), 0x58L, 0x18L, 0x18L, 0x78L, 0x68L, 0x48L);
                //PointerCurrentView = new MultilevelPointer(memoryAccess, IntPtr.Add(BaseAddress, pointerCurrentView), 0x58L);

                PointerEventActionType = new MultilevelPointer(
                    memoryAccess,
                    IntPtr.Add(BaseAddress, pointerPropsManager),
                    0x58L,
                    0x68L,
                    0x68L,
                    0xD0L,
                    0x38L,
                    0x50L
                );

                PointerIsMotionPlay = new MultilevelPointer(
                    memoryAccess,
                    IntPtr.Add(BaseAddress, pointerPropsManager),
                    0x58L,
                    0x68L,
                    0x68L,
                    0xD0L,
                    0x38L
                );

                PointerEventActionTask = new MultilevelPointer(
                    memoryAccess,
                    IntPtr.Add(BaseAddress, pointerEventActionTask),
                    0x58L,
                    0x60L,
                    0x40L,
                    0x80L,
                    0x10L,
                    0x10L,
                    0x20L,
                    0x28L
                );

                PointerPlayerStatus = new MultilevelPointer(
                    memoryAccess,
                    IntPtr.Add(BaseAddress, pointerPropsManager),
                    0x58L,
                    0x68L,
                    0x68L
                );

                PointerPlayerHP = new MultilevelPointer(
                    memoryAccess,
                    IntPtr.Add(BaseAddress, pointerPropsManager),
                    0x58L,
                    0x68L,
                    0x68L,
                    0xD0L,
                    0x68L,
                    0x48L
                );

                PointerPlayerPosition = new MultilevelPointer(
                    memoryAccess,
                    IntPtr.Add(BaseAddress, pointerPropsManager),
                    0x58L,
                    0x68L,
                    0x68L,
                    0xD0L,
                    0x78L,
                    0x50L
                );

                PointerRankManager = new MultilevelPointer(
                    memoryAccess,
                    IntPtr.Add(BaseAddress, pointerRankManager)
                );

                try
                {
                    PointerInventory = new MultilevelPointer(
                        memoryAccess,
                        IntPtr.Add(BaseAddress, pointerInventory),
                        0x60L,
                        0x18L,
                        0x10L
                    );

                    //Enemies
                    PointerEnemyEntryList = new MultilevelPointer(
                        memoryAccess,
                        IntPtr.Add(BaseAddress, pointerAddressEnemies),
                        0x58L,
                        0x10L
                    );

                    PointerEnemyCount = new MultilevelPointer(
                        memoryAccess,
                        IntPtr.Add(BaseAddress, pointerAddressEnemies),
                        0x58L,
                        0x10L
                    );

                    GenerateEnemyEntries();

                    //Items
                    PointerInventoryCount = new MultilevelPointer(
                        memoryAccess,
                        IntPtr.Add(BaseAddress, pointerInventory),
                        0x60L,
                        0x18L,
                        0x10L
                    );

                    PointerInventoryEntryList = new MultilevelPointer(
                        memoryAccess,
                        IntPtr.Add(BaseAddress, pointerAddressItems),
                        0x78L,
                        0x70L
                    );

                    GenerateItemEntries();
                }
                catch

                {
                    // May fail in main menu
                }
            }
        }

        private bool SelectPointerAddresses(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.RE8_PROMO_01_20210426_1:
                    {
                        pointerRankManager = 0x0A1A50C0 + 0x1030;
                        pointerAddressItems = 0x0A1A5880 + 0x1030;
                        pointerInventory = 0x0A1B29F0 + 0x1030;
                        pointerAddressEnemies = 0x0A1B1D00 + 0x1030;
                        pointerPropsManager = 0x0A18D990 + 0x1030;
                        pointerEventActionTask = 0x0A182B38 + 0x1030;
                        return true;
                    }
                case GameVersion.RE8_WW_20210506_1:
                    {
                        pointerRankManager = 0x0A1A50C0;
                        pointerAddressItems = 0x0A1A5880;
                        pointerInventory = 0x0A1B29F0;
                        pointerAddressEnemies = 0x0A1B1D00;
                        pointerPropsManager = 0x0A18D990;
                        pointerEventActionTask = 0x0A182B38;
                        return true;
                    }
                case GameVersion.RE8_CEROD_20210506_1:
                    {
                        pointerRankManager = 0x0A1A50C0 + 0x2000;
                        pointerAddressItems = 0x0A1A5880 + 0x2000;
                        pointerInventory = 0x0A1B29F0 + 0x2000;
                        pointerAddressEnemies = 0x0A1B1D00 + 0x2000;
                        pointerPropsManager = 0x0A18D990 + 0x2000;
                        pointerEventActionTask = 0x0A182B38 + 0x2000;
                        return true;
                    }
                case GameVersion.RE8_CEROZ_20210508_1:
                    {
                        pointerRankManager = 0x0A1A50C0 + 0x1000;
                        pointerAddressItems = 0x0A1A5880 + 0x1000;
                        pointerInventory = 0x0A1B1C70 + 0x1000;
                        pointerAddressEnemies = 0x0A1B1D00 + 0x1000;
                        pointerPropsManager = 0x0A18D990 + 0x1000;
                        pointerEventActionTask = 0x0A182B38 + 0x1000;
                        return true;
                    }
            }

            // If we made it this far... rest in pepperonis. We have failed to detect any of the correct versions we support and have no idea what pointer addresses to use. Bail out.
            return false;
        }

        /// <summary>
        /// Dereferences a 4-byte signed integer via the PointerEnemyEntryCount pointer to detect how large the enemy pointer table is and then create the pointer table entries if required.
        /// </summary>
        private unsafe void GenerateEnemyEntries()
        {
            bool success;
            fixed (int* p = &EnemyTableCount)
                success = PointerEnemyCount.TryDerefInt(0x1C, p);

            PointerEnemyEntries = new MultilevelPointer[MAX_ENTITIES]; // Create a new enemy pointer table array with the detected size.

            // Skip the first 28 bytes and read the rest as a byte array
            // This can be done because the pointers are stored sequentially in an array
            byte[] entityPtrByteArr = PointerEnemyEntryList.DerefByteArray(0x28, MAX_ENTITIES * sizeof(IntPtr));

            // Do a block copy to convert the byte array to an IntPtr array
            IntPtr[] entityPtrArr = new IntPtr[MAX_ENTITIES];
            Buffer.BlockCopy(entityPtrByteArr, 0, entityPtrArr, 0, entityPtrByteArr.Length);

            // The pointers we read are already the address of the entity, so make sure we add the first offset here
            for (int i = 0; i < PointerEnemyEntries.Length; ++i) // Loop through and create all of the pointers for the table.
            {
                PointerEnemyEntries[i] = new MultilevelPointer(memoryAccess, IntPtr.Add(entityPtrArr[i], 0x228), 0x18L, 0x48L, 0x48L);
            }
        }
        private unsafe void GenerateItemEntries()
        {
            bool success;
            fixed (int* p = &InventoryTableCount)
                success = PointerInventoryCount.TryDerefInt(0x1C, p);

            PointerInventoryEntries = new MultilevelPointer[MAX_ITEMS];
            PointerInventoryEntriesCustom = new MultilevelPointer[MAX_ITEMS];

            // Skip the first 28 bytes and read the rest as a byte array
            // This can be done because the pointers are stored sequentially in an array
            byte[] inventoryEntriesPtrByteArr = PointerInventoryEntryList.DerefByteArray(0x20, MAX_ITEMS * sizeof(IntPtr));

            // Do a block copy to convert the byte array to an IntPtr array
            IntPtr[] inventoryEntriesPtrArr = new IntPtr[MAX_ITEMS];
            Buffer.BlockCopy(inventoryEntriesPtrByteArr, 0, inventoryEntriesPtrArr, 0, inventoryEntriesPtrByteArr.Length);

            for (int i = 0; i < PointerInventoryEntries.Length; ++i)
            {
                PointerInventoryEntries[i] = new MultilevelPointer(memoryAccess, IntPtr.Add(inventoryEntriesPtrArr[i], 0x58));
                PointerInventoryEntriesCustom[i] = new MultilevelPointer(memoryAccess, IntPtr.Add(inventoryEntriesPtrArr[i], 0x58), 0x90);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        internal void UpdatePointers()
        {
            PointerEventActionType.UpdatePointers();
            PointerIsMotionPlay.UpdatePointers();
            PointerEventActionTask.UpdatePointers();

            PointerPlayerStatus.UpdatePointers();
            PointerPlayerHP.UpdatePointers();
            PointerPlayerPosition.UpdatePointers();

            PointerRankManager.UpdatePointers();
            PointerInventory.UpdatePointers();

            try
            {
                PointerEnemyCount.UpdatePointers();
                PointerEnemyEntryList.UpdatePointers();

                GenerateEnemyEntries();

                PointerInventoryCount.UpdatePointers();
                PointerInventoryEntryList.UpdatePointers();
                GenerateItemEntries();
            }
            catch
            {
                // May fail in main menu
            }

        }

        private void GetCurrentEvent(int? length)
        {
            if (length == null)
            {
                gameMemoryValues._currentevent = "Null";
            }
            else
            {
                if (length < 64)
                {
                    byte[] eventName = PointerEventActionTask.DerefByteArray(0x14, (int)length * 2);
                    gameMemoryValues._currentevent = GetString(eventName);
                }
                else
                {
                    gameMemoryValues._currentevent = "None";
                }
            }
        }

        internal unsafe IGameMemoryRE8 Refresh()
        {
            bool success;

            // Map Data
            //int size1 = PointerCurrentView.DerefInt(0x10);
            //byte[] view = PointerCurrentView.DerefByteArray(0x14, size1 * 2);
            //gameMemoryValues._currentview = GetString(view);

            // Map Data
            int? size2 = PointerEventActionTask.DerefInt(0x10);
            GetCurrentEvent(size2);

            // Init Bosses On First Scan
            if (gameMemoryValues.EnemyHealth == null && gameMemoryValues.PlayerInventory == null)
            {
                gameMemoryValues.EnemyHealth = new EnemyHP[MAX_ENTITIES];
                gameMemoryValues.LastKeyItem = new InventoryEntry();
                gameMemoryValues.PlayerInventory = new InventoryEntry[MAX_ITEMS];
                gameMemoryValues.PlayerStatus = new PlayerStatus();
            }

            // Player Status
            if (SafeReadByteArray(PointerPlayerStatus.Address, sizeof(GamePlayerStatus), out byte[] gamePlayerStatus))
            {
                var playerStatus = GamePlayerStatus.AsStruct(gamePlayerStatus);
                gameMemoryValues._playerstatus.Update(playerStatus);
            }
            else
            {
                gameMemoryValues._playerstatus.IsChris = false;
                gameMemoryValues._playerstatus.IsEthan = false;
            }

            //Player HP
            if (SafeReadByteArray(PointerPlayerHP.Address, sizeof(GamePlayerHP), out byte[] gamePlayerHpBytes))
            {
                var playerHp = GamePlayerHP.AsStruct(gamePlayerHpBytes);
                gameMemoryValues._playerMaxHealth = playerHp.Max;
                gameMemoryValues._playerCurrentHealth = playerHp.Current;
            }

            // Player Position
            if (SafeReadByteArray(PointerPlayerPosition.Address, sizeof(GamePlayerPosition), out byte[] gamePlayerPosBytes))
            {
                var playerPosition = GamePlayerPosition.AsStruct(gamePlayerPosBytes);
                gameMemoryValues._playerPositionX = playerPosition.X;
                gameMemoryValues._playerPositionY = playerPosition.Y;
                gameMemoryValues._playerPositionZ = playerPosition.Z;
            }

            // DA
            if (SafeReadByteArray(PointerRankManager.Address, sizeof(GameRank), out byte[] gameRankBytes))
            {
                var gameRank = GameRank.AsStruct(gameRankBytes);
                gameMemoryValues._rankScore = gameRank.Score;
                gameMemoryValues._rank = gameRank.Rank;
            }

            // Lei
            fixed (int* p = &gameMemoryValues._lei)
                success = PointerInventory.TryDerefInt(0x48, p);

            // EventType
            fixed (int* p = &gameMemoryValues._eventType)
                success = PointerEventActionType.TryDerefInt(0x158, p);
            if (!success) { gameMemoryValues._eventType = 0; }

            // IsMotionPlay
            fixed (byte* p = &gameMemoryValues._isMotionPlay)
                success = PointerIsMotionPlay.TryDerefByte(0x1D0, p);

            // Lei
            fixed (int* p = &gameMemoryValues._lei)
                success = PointerInventory.TryDerefInt(0x48, p);

            // Enemy HP
            if (gameMemoryValues._enemyHealth == null)
            {
                gameMemoryValues._enemyHealth = new EnemyHP[MAX_ENTITIES];
                for (int i = 0; i < gameMemoryValues._enemyHealth.Length; ++i)
                    gameMemoryValues._enemyHealth[i] = new EnemyHP();
            }
            for (int i = 0; i < gameMemoryValues._enemyHealth.Length; ++i)
            {
                try
                {
                    // Check to see if the pointer is currently valid. It can become invalid when rooms are changed.
                    if (PointerEnemyEntries[i].Address != IntPtr.Zero && i < EnemyTableCount && SafeReadByteArray(PointerEnemyEntries[i].Address, sizeof(GamePlayerHP), out byte[] enemyHpBytes))
                    {
                        // Note, this is using the same structure as the player HP.
                        // This may not always be the case, but the structures match for now.
                        var enemyHp = GamePlayerHP.AsStruct(enemyHpBytes);
                        gameMemoryValues.EnemyHealth[i]._maximumHP = enemyHp.Max;
                        gameMemoryValues.EnemyHealth[i]._currentHP = enemyHp.Current;
                    }
                    else
                    {
                        // Clear these values out so stale data isn't left behind when the pointer address is no longer value and nothing valid gets read.
                        // This happens when the game removes pointers from the table (map/room change).
                        gameMemoryValues.EnemyHealth[i]._maximumHP = 0;
                        gameMemoryValues.EnemyHealth[i]._currentHP = 0;
                    }
                }
                catch
                {
                    gameMemoryValues.EnemyHealth[i]._maximumHP = 0;
                    gameMemoryValues.EnemyHealth[i]._currentHP = 0;
                }
            }

            // Last Key Item
            fixed (uint* p = &gameMemoryValues._lastKeyItem._itemid)
                success = PointerInventoryEntries[0].TryDerefUInt(0x3C, p);

            // Inventory
            if (gameMemoryValues._playerInventory == null)
            {
                gameMemoryValues._playerInventory = new InventoryEntry[MAX_ITEMS];
                for (int i = 0; i < gameMemoryValues._playerInventory.Length; ++i)
                {
                    gameMemoryValues._playerInventory[i] = new InventoryEntry();
                    gameMemoryValues._playerInventory[i].CustomParameter = new InventoryEntryCustomParams();
                }
            }
            for (int i = 0; i < gameMemoryValues._playerInventory.Length; ++i)
            {
                try
                {
                    // Check to see if the pointer is currently valid. It can become invalid when rooms are changed.
                    if (PointerInventoryEntries[i].Address != IntPtr.Zero && i < InventoryTableCount && SafeReadByteArray(PointerInventoryEntries[i].Address, sizeof(GameInventoryItem), out byte[] inventoryItemBytes))
                    {
                        var inventoryItem = GameInventoryItem.AsStruct(inventoryItemBytes);

                        // This hook is a little poorly done... but good enough for now
                        gameMemoryValues.PlayerInventory[i]._isTemporary = 255;
                        gameMemoryValues.PlayerInventory[i]._sortOrder = 255;
                        gameMemoryValues.PlayerInventory[i]._isUsing = 255;
                        gameMemoryValues.PlayerInventory[i]._itemid = 0xFFFFFFFF;
                        gameMemoryValues.PlayerInventory[i]._itemCategoryHash = 0xFFFFFFFF;
                        gameMemoryValues.PlayerInventory[i]._slotNo = -1;
                        gameMemoryValues.PlayerInventory[i]._quickSlotHash = 0xFFFFFFFF;
                        gameMemoryValues.PlayerInventory[i]._stackSize = -1;
                        gameMemoryValues.PlayerInventory[i]._ishorizontal = 255;
                        gameMemoryValues.PlayerInventory[i]._includeItemID = 0xFFFFFFFF;
                        gameMemoryValues.PlayerInventory[i]._includeStackSize = -1;
                        gameMemoryValues.PlayerInventory[i]._includeItemIDSub = 0xFFFFFFFF;
                        gameMemoryValues.PlayerInventory[i]._includeStackSizeSub = -1;
                        gameMemoryValues.PlayerInventory[i]._isHidden = 255;
                        gameMemoryValues.PlayerInventory[i]._customParameter._power = 0;
                        gameMemoryValues.PlayerInventory[i]._customParameter._rate = 0;
                        gameMemoryValues.PlayerInventory[i]._customParameter._reload = 0;
                        gameMemoryValues.PlayerInventory[i]._customParameter._stackSize = -1;

                        if (gameMemoryValues.PlayerInventory[i].IsItem || gameMemoryValues.PlayerInventory[i].IsWeapon)
                        {
                            gameMemoryValues.PlayerInventory[i]._isTemporary = inventoryItem.IsTemporary;
                            gameMemoryValues.PlayerInventory[i]._sortOrder = inventoryItem.SortOrder;
                            gameMemoryValues.PlayerInventory[i]._isUsing = inventoryItem.IsUsing;
                            gameMemoryValues.PlayerInventory[i]._slotNo = inventoryItem.SlotNo;
                            gameMemoryValues.PlayerInventory[i]._quickSlotHash = inventoryItem.QuickSlotHash;
                            gameMemoryValues.PlayerInventory[i]._stackSize = inventoryItem.StackSize;
                            gameMemoryValues.PlayerInventory[i]._ishorizontal = inventoryItem.IsHorizontal;
                            gameMemoryValues.PlayerInventory[i]._includeItemID = inventoryItem.IncludeItemID;
                            gameMemoryValues.PlayerInventory[i]._includeStackSize = inventoryItem.IncludeStackSize;
                            gameMemoryValues.PlayerInventory[i]._includeItemIDSub = inventoryItem.IncludeItemIDSub;
                            gameMemoryValues.PlayerInventory[i]._includeStackSizeSub = inventoryItem.IncludeStackSizeSub;
                            gameMemoryValues.PlayerInventory[i]._isHidden = inventoryItem.IsHidden;
                            if (SafeReadByteArray(PointerInventoryEntriesCustom[i].Address, sizeof(GameItemCustomParams), out byte[] itemCustomBytes))
                            {
                                var customParams = GameItemCustomParams.AsStruct(itemCustomBytes);
                                gameMemoryValues.PlayerInventory[i]._customParameter._power = customParams.Power;
                                gameMemoryValues.PlayerInventory[i]._customParameter._rate = customParams.Rate;
                                gameMemoryValues.PlayerInventory[i]._customParameter._reload = customParams.Reload;
                                gameMemoryValues.PlayerInventory[i]._customParameter._stackSize = customParams.StackSize;
                            }
                        }
                        else if (gameMemoryValues.PlayerInventory[i].IsKeyItem || gameMemoryValues.PlayerInventory[i].IsCraftable || gameMemoryValues.PlayerInventory[i].IsTreasure)
                        {
                            gameMemoryValues.PlayerInventory[i]._stackSize = inventoryItem.StackSize;
                        }
                    }
                    else
                    {
                        // Clear these values out so stale data isn't left behind when the pointer address is no longer value and nothing valid gets read.
                        // This happens when the game removes pointers from the table (map/room change).
                        EmptySlot(i);
                    }
                }
                catch
                {
                    EmptySlot(i);
                }
            }

            HasScanned = true;
            return gameMemoryValues;
        }

        private void EmptySlot(int i)
        {
            gameMemoryValues.PlayerInventory[i]._isTemporary = 255;
            gameMemoryValues.PlayerInventory[i]._sortOrder = 255;
            gameMemoryValues.PlayerInventory[i]._isUsing = 255;
            gameMemoryValues.PlayerInventory[i]._itemid = 0xFFFFFFFF;
            gameMemoryValues.PlayerInventory[i]._itemCategoryHash = 0xFFFFFFFF;
            gameMemoryValues.PlayerInventory[i]._slotNo = -1;
            gameMemoryValues.PlayerInventory[i]._quickSlotHash = 0xFFFFFFFF;
            gameMemoryValues.PlayerInventory[i]._stackSize = -1;
            gameMemoryValues.PlayerInventory[i]._ishorizontal = 255;
            gameMemoryValues.PlayerInventory[i]._includeItemID = 0xFFFFFFFF;
            gameMemoryValues.PlayerInventory[i]._includeStackSize = -1;
            gameMemoryValues.PlayerInventory[i]._includeItemIDSub = 0xFFFFFFFF;
            gameMemoryValues.PlayerInventory[i]._includeStackSizeSub = -1;
            gameMemoryValues.PlayerInventory[i]._isHidden = 255;
            gameMemoryValues.PlayerInventory[i]._customParameter._power = 0;
            gameMemoryValues.PlayerInventory[i]._customParameter._rate = 0;
            gameMemoryValues.PlayerInventory[i]._customParameter._reload = 0;
            gameMemoryValues.PlayerInventory[i]._customParameter._stackSize = -1;
        }

        private string GetString(byte[] value)
        {
            if (value != null)
            {
                return Encoding.Unicode.GetString(value);
            }
            return "";
        }

        private int? GetProcessId(Process process) => process?.Id;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private unsafe bool SafeReadByteArray(IntPtr address, int size, out byte[] readBytes)
        {
            readBytes = new byte[size];
            fixed (byte* p = readBytes)
            {
                return memoryAccess.TryGetByteArrayAt(address, size, p);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (memoryAccess != null)
                        memoryAccess.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~REmake1Memory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
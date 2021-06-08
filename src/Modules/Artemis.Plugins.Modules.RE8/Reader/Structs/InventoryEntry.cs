using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace SRTPluginProviderRE8.Structs
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct InventoryEntry // : IEquatable<InventoryEntry>, IEqualityComparer<InventoryEntry>
    {
        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsItem && IsAmmoClip) 
                    return string.Format("Weapon Clip {0} Quantity {1}", (ItemEnumeration)ItemID, StackSize);
                else if (IsItem && !IsAmmoClip)
                    return string.Format("[#{0}] Item {1} Quantity {2}", SlotNo, (ItemEnumeration)ItemID, StackSize);
                else if (IsWeapon)
                    return string.Format("[#{0}] Weapon {1} Ammo {2}", SlotNo, (WeaponEnumeration)ItemID, IncludeStackSize);
                else if (IsKeyItem)
                    return string.Format("Key Item {0}", (KeyItemEnumeration)ItemID);
                else if (IsTreasure)
                    return string.Format("Treasure {0} Quantity {1}", (TreasureEnumeration)ItemID, StackSize);
                else if (IsCraftable)
                    return string.Format("Craftable {0} Quantity {1}", (CraftableEnumeration)ItemID, StackSize);
                else if (IsMap)
                    return string.Format("Map {0}", (CraftableEnumeration)ItemID);
                else if (ItemID > 0)
                    return string.Format("Uknown Item 0x{0}", ItemID.ToString("X4"));
                else
                    return string.Format("Empty Slot");
            }
        }

        public string ItemName 
        { 
            get
            {
                if (IsItem)
                    return string.Format("{0}", (ItemEnumeration)ItemID);
                else if (IsWeapon)
                    return string.Format("{0}", (WeaponEnumeration)ItemID);
                else if (IsKeyItem)
                    return string.Format("{0}", (KeyItemEnumeration)ItemID);
                else if (IsTreasure)
                    return string.Format("{0}", (TreasureEnumeration)ItemID);
                else if (IsCraftable)
                    return string.Format("{0}", (CraftableEnumeration)ItemID);
                else if (IsMap)
                    return string.Format("{0}", (MapEnumeration)ItemID);
                else
                    return "None";
            }
        }

        //public static readonly int[] EMPTY_INVENTORY_ITEM = new int[5] { 0x00000000, unchecked((int)0xFFFFFFFF), 0x00000000, 0x00000000, 0x01000000 };

        // Storage variable.
        // Accessor properties.
        public byte IsTemporary { get => _isTemporary; set => _isTemporary = value; }
        internal byte _isTemporary;
        public int SortOrder { get => _sortOrder; set => _sortOrder = value; }
        internal int _sortOrder;
        public byte IsUsing { get => _isUsing; set => _isUsing = value; }
        internal byte _isUsing;
        public uint ItemID { get => _itemid; set => _itemid = value; }
        internal uint _itemid;
        public uint ItemCategoryHash { get => _itemCategoryHash; set => _itemCategoryHash = value; }
        internal uint _itemCategoryHash;
        public int SlotNo { get => _slotNo; set => _slotNo = value; }
        internal int _slotNo;
        public uint QuickSlotHash { get => _quickSlotHash; set => _quickSlotHash = value; }
        internal uint _quickSlotHash;
        public bool IsItem => Enum.IsDefined(typeof(ItemEnumeration), _itemid);
        public bool IsWeapon => Enum.IsDefined(typeof(WeaponEnumeration), _itemid);
        public bool IsKeyItem => Enum.IsDefined(typeof(KeyItemEnumeration), _itemid);
        public bool IsTreasure => Enum.IsDefined(typeof(TreasureEnumeration), _itemid);
        public bool IsCraftable => Enum.IsDefined(typeof(CraftableEnumeration), _itemid);
        public bool IsMap => Enum.IsDefined(typeof(MapEnumeration), _itemid);
        public bool IsAmmoClip => _slotNo > 256;
        public bool IsItemSlot => IsItem && !IsAmmoClip || IsWeapon;
        public int StackSize { get => _stackSize; set => _stackSize = value; }
        internal int _stackSize;
        public byte IsHorizontal { get => _ishorizontal; set => _ishorizontal = value; }
        internal byte _ishorizontal;
        public uint IncludeItemID { get => _includeItemID; set => _includeItemID = value; }
        internal uint _includeItemID;
        public int IncludeStackSize { get => _includeStackSize; set => _includeStackSize = value; }
        internal int _includeStackSize;
        public uint IncludeItemIDSub { get => _includeItemIDSub; set => _includeItemIDSub = value; }
        internal uint _includeItemIDSub;
        public int IncludeStackSizeSub { get => _includeStackSizeSub; set => _includeStackSizeSub = value; }
        internal int _includeStackSizeSub;
        public byte IsHidden { get => _isHidden; set => _isHidden = value; }
        internal byte _isHidden;
        public InventoryEntryCustomParams CustomParameter { get => _customParameter; set => _customParameter = value; }
        internal InventoryEntryCustomParams _customParameter;

        //public bool Equals(InventoryEntry other) => this == other;
        //public bool Equals(InventoryEntry x, InventoryEntry y) => x == y;
        //public override bool Equals(object obj)
        //{
        //    if (obj is InventoryEntry)
        //        return this == (InventoryEntry)obj;
        //    else
        //        return base.Equals(obj);
        //}
        //public static bool operator ==(InventoryEntry obj1, InventoryEntry obj2)
        //{
        //    if (ReferenceEquals(obj1, obj2))
        //        return true;
        //
        //    if (ReferenceEquals(obj1, null) || ReferenceEquals(obj1._data, null))
        //        return false;
        //
        //    if (ReferenceEquals(obj2, null) || ReferenceEquals(obj2._data, null))
        //        return false;
        //
        //    return obj1.SlotPosition == obj2.SlotPosition && obj1._data.SequenceEqual(obj2._data);
        //}
        //public static bool operator !=(InventoryEntry obj1, InventoryEntry obj2) => !(obj1 == obj2);
        //
        //public override int GetHashCode() => SlotPosition ^ _data.Aggregate((int p, int c) => p ^ c);
        //public int GetHashCode(InventoryEntry obj) => obj.GetHashCode();
        //
        //public override string ToString() => _DebuggerDisplay;

        //public InventoryEntry Clone()
        //{
        //    InventoryEntry clone = new InventoryEntry() { _data = new int[this._data.Length] };
        //    clone._slotPosition = this._slotPosition;
        //    for (int i = 0; i < this._data.Length; ++i)
        //        clone._data[i] = this._data[i];
        //    clone._invDataOffset = this._invDataOffset;
        //    return clone;
        //}

        //public static InventoryEntry Clone(InventoryEntry subject) => subject.Clone();
    }
}
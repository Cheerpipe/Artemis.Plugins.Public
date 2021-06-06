using System.Diagnostics;

namespace SRTPluginProviderRE7.Structs
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class InventoryEntry
    {
        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsItem)
                    return string.Format("[#{0}] Item {1} Quantity {2}", SlotPosition, ItemName, Quantity);
                else
                    return string.Format("[#{0}] Empty Slot", SlotPosition);
            }
        }

        public int SlotPosition { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public bool IsItem => ItemName != null;

        public InventoryEntry()
        {
            this.SlotPosition = -1;
            this.ItemName = "";
            this.Quantity = -1;
        }

        public void SetValues(int slotPosition, string name, int quantity)
        {
            this.SlotPosition = slotPosition;
            this.ItemName = name;
            this.Quantity = quantity;
        }

    }
}

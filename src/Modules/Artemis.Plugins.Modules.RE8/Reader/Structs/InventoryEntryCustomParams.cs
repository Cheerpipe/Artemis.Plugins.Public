using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace SRTPluginProviderRE8.Structs
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct InventoryEntryCustomParams // : IEquatable<InventoryEntry>, IEqualityComparer<InventoryEntry>
    {
        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                return string.Format(
                    "Damage {0} - Rate of Fire {1} - Reload Speed {2} - Max Stack Size {3}",
                    Power,
                    Rate,
                    Reload,
                    StackSize
                );
            }
        }

        // Storage variable.
        // Accessor properties.
        public float Power { get => _power; set => _power = value; }
        internal float _power;
        public float Rate { get => _rate; set => _rate = value; }
        internal float _rate;
        public float Reload { get => _reload; set => _reload = value; }
        internal float _reload;
        public int StackSize { get => _stackSize; set => _stackSize = value; }
        internal int _stackSize;
        
    }
}
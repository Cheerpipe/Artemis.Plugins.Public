using System.Diagnostics;

namespace SRTPluginProviderRE7.Structs
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class EnemyHP
    {
        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsAlive)
                    return string.Format("ID: {0} {1} / {2} ({3:P1})", ID, CurrentHP, MaximumHP, Percentage);
                else
                    return "DEAD / DEAD (0%)";
            }
        }

        public ushort ID { get; set; }
        public float MaximumHP { get; set; }
        public float CurrentHP { get; set; }
        public bool IsAlive => MaximumHP > 0 && CurrentHP > 0 && CurrentHP <= MaximumHP;
        public float Percentage => ((IsAlive) ? CurrentHP / MaximumHP : 0f);

        public EnemyHP()
        {
            this.ID = 0;
            this.MaximumHP = 0;
            this.CurrentHP = 0;
        }
    }
}

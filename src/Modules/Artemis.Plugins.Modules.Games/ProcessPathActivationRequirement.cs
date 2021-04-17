using Artemis.Core.DataModelExpansions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Artemis.Core.Modules
{
    /// <summary>
    ///     Evaluates to true or false by checking if the specified process is running
    /// </summary>
    public class ProcessPathContainsActivationRequirement : IModuleActivationRequirement
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="ProcessActivationRequirement" /> class
        /// </summary>
        /// <param name="processName">The name of the process that must run</param>
        /// <param name="location">The location of where the process must be running from (optional)</param>
        public ProcessPathContainsActivationRequirement(string locationWildcard)
        {
            LocationWildCard = locationWildcard;
        }

        /// <summary>
        ///     The location of where the process must be running from
        /// </summary>
        public string LocationWildCard { get; set; }

        /// <inheritdoc />
        public bool Evaluate()
        {
            if (LocationWildCard == "*") return true;

            IntPtr handle = GetForegroundWindow();
            uint pID;

            GetWindowThreadProcessId(handle, out pID);
            try
            {
                return Process.GetProcessById((Int32)pID).MainModule.FileName.ToLower().Contains(LocationWildCard.ToLower());
            }
            catch
            {
                return false;
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        /// <inheritdoc />
        public string GetUserFriendlyDescription()
        {
            string description = string.Format("Requirement met when a proccess is running from a \"{0}\" folder", LocationWildCard);

            return description;
        }
    }
}
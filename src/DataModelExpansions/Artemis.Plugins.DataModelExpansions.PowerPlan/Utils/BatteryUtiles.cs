namespace Artemis.Plugins.DataModelExpansions.PowerPlan.Utils
{
    public class BatteryUtiles
    {

        public void Test()
        {
            // https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-battery?redirectedfrom=MSDN
            // EstimatedChargeRemaining

            /*
            System.Management.ObjectQuery query = new ObjectQuery("Select * FROM Win32_Battery");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            ManagementObjectCollection collection = searcher.Get();

            foreach (ManagementObject mo in collection)
            {
                foreach (PropertyData property in mo.Properties)
                {
                    MessageBox.Show(string.Format("Property {0}: Value is {1}", property.Name, property.Value));
                }
            }
            */
        }
    }
}

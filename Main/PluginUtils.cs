using UnityEngine;

namespace Terminals
{
    public class PluginUtils
    {
        public string[] GetRandomErrorParameters()
        {
            string chars = "0123456789abcdef";

            var parameters = new string[3];

            for (byte index = 0; index < 3; index++)
            {
                for (byte symbol = 0; symbol < 32; symbol++)
                    parameters[index] += chars[Random.Range(0, 15)];
            }

            return parameters;
        }
    }
}

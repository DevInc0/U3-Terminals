using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminals
{
    public struct Error
    {

        public string warningMessage;

        public string debugCommand;

        public float reloadingTime;

        public Error(string _warningMessage = null, string _debugCommand = null, float _reloadingTime = 0f)
        {            
            warningMessage = _warningMessage;
            debugCommand = _debugCommand;
            reloadingTime = _reloadingTime;
        }        

        public static Error GetRandomError()
        {
            int random = new Random().Next(0, Plugin.Instance.Configuration.Instance.errors.Count - 1);
            return Plugin.Instance.Configuration.Instance.errors[random];
        }
    }
}

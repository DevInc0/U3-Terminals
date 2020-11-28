using System;
using System.Linq;

namespace Terminals
{
    public struct Error
    {
        public string warningMessage;

        public string debugCommand;

        public float reloadingTime;

        public string[] parameters;

        public Error(string _warningMessage = null, string _debugCommand = null, float _reloadingTime = 0f, string[] _parameters = null)
        {
            warningMessage = _warningMessage;
            debugCommand = _debugCommand;
            reloadingTime = _reloadingTime;
            parameters = _parameters;
        }

        public static Error GetRandomError()
        {
            Error newError = Plugin.Instance.Configuration.Instance.errors[UnityEngine.Random.Range(0, 2)];
            newError.parameters = new PluginUtils().GetRandomErrorParameters();

            return newError;
        }

        public bool FixError(string input)
        {
            var answer = string.Empty;

            if (warningMessage.Contains("ESRCH"))
            {
                foreach (string parameter in parameters)
                    parameter.Where(x => x >= 'a' && x <= 'f').ToList().ForEach(x => answer += x);

                answer = Convert.ToString((Convert.ToInt64(answer, 16) / 2), 16);
            }
            else if (warningMessage.Contains("EINTR"))
            {
                foreach (string parameter in parameters)
                    answer += parameter[parameter.Length - 1];

                answer = Convert.ToString(Convert.ToInt64(answer, 16) - 1);
            }
            else if (warningMessage.Contains("EIO"))
            {
                long number = 0;

                foreach (string parameter in parameters)
                    number += Convert.ToInt64(parameter.Substring(0, 3), 16);

                // число - последний символ последней строки(переводя из 16 в 10). и в ответ пишим разность в 16 системе
                answer = Convert.ToString((number - Convert.ToInt64(parameters[2][31].ToString(), 16)), 16);
            }

            return input == answer;
        }
    }
}

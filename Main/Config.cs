using Rocket.API;
using System.Collections.Generic;

namespace Terminals
{
    public class Config : IRocketPluginConfiguration
    {
        public List<Error> errors;

        public void LoadDefaults()
        {
            errors = new List<Error>
            {
                new Error("#//$% ERROR **!@   ESRCH | No such process", "crt /n prc/ *|Main| -res -rel -d", 300f),
                new Error("#()% ERROR ||!@   EINTR | Interrupted system call", "gen_ser connect *|Main| -res -rel -d", 600f),
                new Error("#_001 ERROR |!!*!@   EIO | Input/output error", "*|Main| -res -rel -d", 900f)
            };
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;

namespace MadsKristensen.FlattenPackages
{
    class ProcessHelper
    {
        public static bool RunProcessSync(string arguments, string directory)
        {
            ProcessStartInfo start = new ProcessStartInfo("cmd", arguments)
            {
                WorkingDirectory = directory,
            };

            using (Process p = new Process())
            {
                p.StartInfo = start;
                p.Start();
                p.WaitForExit();

                return true;
            }
        }

        public static bool IsNodeModuleInstalled()
        {
            string env = Environment.GetEnvironmentVariable("PATH");
            string[] paths = env.Split(';');

            foreach (string path in paths)
            {
                string cmd = Path.Combine(path, "flatten-packages.cmd");

                if (File.Exists(cmd))
                    return true;
            }

            return false;
        }
    }
}
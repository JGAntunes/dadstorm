using DADStormCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeService
{
    class Program
    {

        public static string ExecPath()
        {
            return Environment.CurrentDirectory + "\\NodeService.exe";
        }

        static void Main(string[] args)
        {
            var uri = args[0];
            var operatorType = Type.GetType("DADStormCore." + args[1]);

            var instance = (Operator)Activator.CreateInstance(operatorType, );

            new Node(uri,)

            ProcessStartInfo info1 = new ProcessStartInfo(exe, "1");
            info1.CreateNoWindow = false;

            Process.Start(info1);

            ProcessStartInfo info2 = new ProcessStartInfo(exe, "2");
            info2.CreateNoWindow = false;

            Process.Start(info2);
        }
    }
}

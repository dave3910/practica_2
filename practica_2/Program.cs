using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practica_2
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Task.Factory.StartNew(() => new Main());
            Application.Run();
        }
    }
}
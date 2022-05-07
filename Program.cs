using System;

namespace AstraLostInSpace
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new AstraLostInSpace())
            {
                game.Run();
            }
        }
    }
}

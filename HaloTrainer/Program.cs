namespace HaloTrainer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Nouzuru;

    /// <summary>
    /// The main program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of this program.
        /// </summary>
        /// <param name="args">The command line arguments passed to this program.</param>
        private static void Main(string[] args)
        {
            HaloTrainer ht = new HaloTrainer();
            ht.Open();

            ht.EnableUnlimitedHumanAmmo();
            ht.EnableUnlimitedGrenades();
            ht.EnableUnlimitedFlashlightPower();
            ht.FreezeShields();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}

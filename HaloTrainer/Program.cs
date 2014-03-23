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

            Options options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                // Disable or enable unlimited ammo.
                if (options.DisableUnlimitedAmmo || options.ResetEverything)
                {
                    ht.DisableUnlimitedHummanAmmo();
                }
                else
                {
                    ht.EnableUnlimitedHumanAmmo();
                }

                // Disable or enable unlimited grenades.
                if (options.DisableUnlimitedGrenades || options.ResetEverything)
                {
                    ht.DisableUnlimitedGrenades();
                }
                else
                {
                    ht.EnableUnlimitedGrenades();
                }

                // Disable or enable unlimited flashlight power.
                if (options.DisableUnlimitedFlashlightPower || options.ResetEverything)
                {
                    ht.DisableUnlimitedFlashlightPower();
                }
                else
                {
                    ht.EnableUnlimitedFlashlightPower();
                }

                // Disable invisibility when specified. Otherwise enable them.
                if (options.ResetInvisibility || options.ResetEverything)
                {
                    ht.EnableInvisibility = false;
                    ht.ResetInvisibility();
                }
                else
                {
                    ht.EnableInvisibility = true;
                    Console.WriteLine("[+] Invisibility enabled.");
                }

                // Disable massive shields when specified. Otherwise enable them.
                if (options.ResetShields || options.ResetEverything)
                {
                    ht.EnableMassiveShields = false;
                    ht.ResetShields();
                }
                else
                {
                    ht.EnableMassiveShields = true;
                    Console.WriteLine("[+] Massive shields enabled.");
                }

                // If either invisibility or massive shields are enabled, start the thread to freeze their values.
                if (ht.EnableInvisibility || ht.EnableMassiveShields)
                {
                    ht.StartFreezeThread();

                    // Wait for user input to exit the freeze thread.
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey(true);
                }
            }
        }
    }
}

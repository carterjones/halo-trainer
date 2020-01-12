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
            Options options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                HaloTrainer ht = new HaloTrainer();
                if (!ht.Open())
                {
                    return;
                }

                // Disable or enable unlimited ammo.
                if (options.DisableUnlimitedAmmo || options.ResetAll)
                {
                    ht.DisableUnlimitedHummanAmmo();
                }
                else
                {
                    ht.EnableUnlimitedHumanAmmo();
                }

                // Disable or enable unlimited grenades.
                if (options.DisableUnlimitedGrenades || options.ResetAll)
                {
                    ht.DisableUnlimitedGrenades();
                }
                else
                {
                    ht.EnableUnlimitedGrenades();
                }

                // Disable or enable unlimited flashlight power.
                if (options.DisableUnlimitedFlashlightPower || options.ResetAll)
                {
                    ht.DisableUnlimitedFlashlightPower();
                }
                else
                {
                    ht.EnableUnlimitedFlashlightPower();
                }

                // Disable or enable unlimited health.
                if (options.DisableUnlimitedHealth || options.ResetAll)
                {
                    ht.EnableUnlimitedHealth = false;
                    ht.ResetHealth();
                }
                else
                {
                    ht.EnableUnlimitedHealth = true;
                    Console.WriteLine("[+] Unlimited health enabled.");
                }

                // Disable invisibility when specified. Otherwise enable them.
                if (options.ResetInvisibility || options.ResetAll)
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
                if (options.ResetShields || options.ResetAll)
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
                if (ht.EnableUnlimitedHealth || ht.EnableInvisibility || ht.EnableMassiveShields)
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

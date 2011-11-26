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
        /// Gets the base address of the current player's class object.
        /// </summary>
        /// <param name="p">The patcher that has opened the halo.exe process.</param>
        /// <returns>
        /// Returns the base address of the player's class object, if a level is being played.
        /// Returns 0 if the player is in the home screen UI (ie: not in any level).
        /// </returns>
        private static IntPtr GetPlayerBaseAddress(Patcher p)
        {
            if (!p.IsOpen())
            {
                return IntPtr.Zero;
            }

            Dictionary<short, int> levelAddresses = new Dictionary<short, int>();
            levelAddresses.Add(0x6975, 0x00000000); // ui (not in a level)           // levels\ui\ui
            levelAddresses.Add(0x3161, 0x4005220C); // the pillar of autumn          // levels\a10\a10
            levelAddresses.Add(0x3361, 0x4005220C); // halo                          // levels\a30\a30
            levelAddresses.Add(0x3561, 0x400534C0); // the truth and reconciliation  // levels\a50\a50
            levelAddresses.Add(0x3362, 0x4005313C); // the silent cartogropher       // levels\b30\b30
            levelAddresses.Add(0x3462, 0x40051DA4); // assault on the control room   // levels\b40\b40
            levelAddresses.Add(0x3163, 0x40050700); // 343 guilty spark              // levels\c10\c10
            levelAddresses.Add(0x3263, 0x4005214C); // the library                   // levels\c20\c20
            levelAddresses.Add(0x3463, 0x40051E10); // two betrayals                 // levels\c40\c40
            levelAddresses.Add(0x3264, 0x40051D2C); // keyes                         // levels\d20\d20
            levelAddresses.Add(0x3464, 0x40051BF4); // the maw                       // levels\d40\d40

            byte[] levelIdentifier = new byte[2];
            byte[] baseAddressPivot = new byte[4];
            p.Read(IntPtr.Add(IntPtr.Zero, 0x4000000b), levelIdentifier);
            int levelAddress = levelAddresses[BitConverter.ToInt16(levelIdentifier, 0)];
            if (levelAddress == 0)
            {
                return IntPtr.Zero;
            }
            else
            {
                p.Read(IntPtr.Add(IntPtr.Zero, levelAddress), baseAddressPivot);
                return IntPtr.Add(IntPtr.Add(IntPtr.Zero, BitConverter.ToInt32(baseAddressPivot, 0)), 0x5C);
            }
        }

        /// <summary>
        /// Freeze the player's shields at 15 (normal is 1, overshield is 3).
        /// </summary>
        /// <param name="p">The patcher object that has opened halo's process.</param>
        /// <param name="enableInvisibility">If true, the player will also receive active cammo.</param>
        private static void FreezeShields(Patcher p, bool enableInvisibility = true)
        {
            IntPtr baseAddress = GetPlayerBaseAddress(p);
            const int ShieldsOffset = 0x88;
            const int InvisibilityOffset = 0x1a8;
            float newShieldValue = 15;
            byte[] newInvisibilityValue = new byte[] { 0x51 };
            byte[] velocities = new byte[12];

            Console.WriteLine("Player address base: 0x" + baseAddress.ToString("X"));
            Console.WriteLine("Beginning freeze shields loop...");

            uint printCount = 0;

            while (true)
            {
                // Find the address again, in case something was re-loaded.
                baseAddress = GetPlayerBaseAddress(p);
                if (!baseAddress.Equals(IntPtr.Zero))
                {
                    p.Write(IntPtr.Add(baseAddress, ShieldsOffset), BitConverter.GetBytes(newShieldValue));

                    if (enableInvisibility)
                    {
                        p.Write(IntPtr.Add(baseAddress, InvisibilityOffset), newInvisibilityValue);
                    }
                }

                printCount++;
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// The entry point of this program.
        /// </summary>
        /// <param name="args">The command line arguments passed to this program.</param>
        private static void Main(string[] args)
        {
            SysInteractor.Init();

            Patcher p = new Patcher();
            p.Open("halo");

            // Unlimited Human Ammo
            Console.WriteLine("Getting unlimited human ammo...");
            byte[] ammoNop = new byte[] { 0x90, 0x90, 0x90, 0x90 };
            p.Write(IntPtr.Add(IntPtr.Zero, 0x004c2696), ammoNop);

            // Unlimited Grenades
            Console.WriteLine("Getting unlimited grenades...");
            byte[] nadesNop = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
            p.Write(IntPtr.Add(IntPtr.Zero, 0x00569ce4), nadesNop);

            // Unlimited Flashlight Power
            Console.WriteLine("Getting unlimited flashlight power...");
            byte[] flashlightNop = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
            p.Write(IntPtr.Add(IntPtr.Zero, 0x0055f0ad), flashlightNop);

            FreezeShields(p);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}

namespace HaloTrainer
{
    using System;
    using System.Collections.Generic;
    using Nouzuru;

    /// <summary>
    /// The trainer used to modify the halo.exe process.
    /// </summary>
    public class HaloTrainer : Patcher
    {
        /// <summary>
        /// The list of levels and their memory addresses.
        /// </summary>
        private Dictionary<short, int> levelAddresses;

        /// <summary>
        /// Opens the halo.exe process.
        /// </summary>
        /// <returns>Returns true if the process was successfully opened.</returns>
        public bool Open()
        {
            if (!base.Open("halo"))
            {
                return false;
            }

            // Initialize the list of 1st person campaign levels.
            this.levelAddresses = new Dictionary<short, int>();
            this.levelAddresses.Add(0x6975, 0x00000000); // ui (not in a level)           // levels\ui\ui
            this.levelAddresses.Add(0x3161, 0x4005220C); // the pillar of autumn          // levels\a10\a10
            this.levelAddresses.Add(0x3361, 0x4005220C); // halo                          // levels\a30\a30
            this.levelAddresses.Add(0x3561, 0x400534C0); // the truth and reconciliation  // levels\a50\a50
            this.levelAddresses.Add(0x3362, 0x4005313C); // the silent cartogropher       // levels\b30\b30
            this.levelAddresses.Add(0x3462, 0x40051DA4); // assault on the control room   // levels\b40\b40
            this.levelAddresses.Add(0x3163, 0x40050700); // 343 guilty spark              // levels\c10\c10
            this.levelAddresses.Add(0x3263, 0x4005214C); // the library                   // levels\c20\c20
            this.levelAddresses.Add(0x3463, 0x40051E10); // two betrayals                 // levels\c40\c40
            this.levelAddresses.Add(0x3264, 0x40051D2C); // keyes                         // levels\d20\d20
            this.levelAddresses.Add(0x3464, 0x40051BF4); // the maw                       // levels\d40\d40

            return true;
        }

        /// <summary>
        /// Enables unlimited human weapon ammunition.
        /// </summary>
        /// <returns>Returns true if the enabling process was successful.</returns>
        public bool EnableUnlimitedHumanAmmo()
        {
            return this.EnableWithNOPInstruction(0x004c2696, "unlimited human weapon ammunition");
        }

        /// <summary>
        /// Enables unlimited grenades.
        /// </summary>
        /// <returns>Returns true if the enabling process was successful.</returns>
        public bool EnableUnlimitedGrenades()
        {
            return this.EnableWithNOPInstruction(0x00569ce4, "unlimited grenades");
        }

        /// <summary>
        /// Enables unlimited flashlight power.
        /// </summary>
        /// <returns>Returns true if the enabling process was successful.</returns>
        public bool EnableUnlimitedFlashlightPower()
        {
            return this.EnableWithNOPInstruction(0x0055f0ad, "unlimited flashlight power");
        }

        /// <summary>
        /// Freeze the player's shields at 15 (normal is 1, overshield is 3).
        /// </summary>
        /// <param name="enableInvisibility">If true, the player will also receive active cammo.</param>
        public void FreezeShields(bool enableInvisibility = true)
        {
            IntPtr baseAddress = this.GetPlayerBaseAddress();
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
                baseAddress = this.GetPlayerBaseAddress();
                if (!baseAddress.Equals(IntPtr.Zero))
                {
                    this.Write(IntPtr.Add(baseAddress, ShieldsOffset), BitConverter.GetBytes(newShieldValue));

                    if (enableInvisibility)
                    {
                        this.Write(IntPtr.Add(baseAddress, InvisibilityOffset), newInvisibilityValue);
                    }
                }

                printCount++;
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Gets the base address of the current player's class object.
        /// </summary>
        /// <returns>
        /// Returns the base address of the player's class object, if a level is being played.
        /// Returns 0 if the player is in the home screen UI (ie: not in any level).
        /// </returns>
        private IntPtr GetPlayerBaseAddress()
        {
            if (!this.IsOpen())
            {
                return IntPtr.Zero;
            }

            byte[] levelIdentifier = new byte[2];
            byte[] baseAddressPivot = new byte[4];

            // Find out which level is currently loaded.
            this.Read(IntPtr.Add(IntPtr.Zero, 0x4000000b), levelIdentifier);
            int levelAddress = this.levelAddresses[BitConverter.ToInt16(levelIdentifier, 0)];
            if (levelAddress == 0)
            {
                return IntPtr.Zero;
            }
            else
            {
                // Obtain the pivot address that is close to the address of the character.
                this.Read(IntPtr.Add(IntPtr.Zero, levelAddress), baseAddressPivot);
                IntPtr pivotAddress = IntPtr.Add(IntPtr.Zero, BitConverter.ToInt32(baseAddressPivot, 0));

                // Return the address at a constant offset from the pivot address of the character.
                return IntPtr.Add(pivotAddress, 0x5C);
            }
        }

        /// <summary>
        /// Enables some advantage in halo, by modifying its memory.
        /// </summary>
        /// <param name="address">
        /// The address of the thing that restricts the player's awesomeness. The instruction at this address will be
        /// replaced with NOP instructions.
        /// </param>
        /// <param name="thingEnabled">The thing that makes the player more awesome.</param>
        /// <returns>Returns true if the enabling process was successful.</returns>
        private bool EnableWithNOPInstruction(ulong address, string thingEnabled)
        {
            Console.Write("Enabling " + thingEnabled + "...");
            if (this.NOPInstruction(address))
            {
                Console.WriteLine("Success.");
                return true;
            }
            else
            {
                Console.WriteLine("Failure.");
                return false;
            }
        }
    }
}

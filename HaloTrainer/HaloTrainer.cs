namespace HaloTrainer
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Nouzuru;

    /// <summary>
    /// The trainer used to modify the halo.exe process.
    /// </summary>
    public class HaloTrainer : Patcher
    {
        #region Fields

        /// <summary>
        /// The list of levels and their memory addresses, when played on easy or normal difficulty.
        /// </summary>
        private Dictionary<short, int> levelAddressesEasyNormal;

        /// <summary>
        /// The list of levels and their memory addresses, when played on heroic difficulty.
        /// </summary>
        private Dictionary<short, int> levelAddressesHeroic;

        /// <summary>
        /// The list of levels and their memory addresses, when played on legendary difficulty.
        /// </summary>
        private Dictionary<short, int> levelAddressesLegendary;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the HaloTrainer class.
        /// </summary>
        public HaloTrainer()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not this trainer should grant the player massive shields.
        /// </summary>
        public bool EnableMassiveShields { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this trainer should enable invisibility for the player.
        /// </summary>
        public bool EnableInvisibility { get; set; }

        #endregion

        #region Methods

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

            // Disable logging.
            this.Status.PermittedLevels = Logger.Logger.Level.NONE;

            // Initialize the list of 1st person campaign levels for easy and normal.
            this.levelAddressesEasyNormal = new Dictionary<short, int>();
            this.levelAddressesEasyNormal.Add(0x6975, 0x00000000); // ui (not in a level)
            this.levelAddressesEasyNormal.Add(0x3161, 0x4005193C); // the pillar of autumn
            this.levelAddressesEasyNormal.Add(0x3361, 0x4005220C); // halo
            this.levelAddressesEasyNormal.Add(0x3561, 0x400534C0); // the truth and reconciliation
            this.levelAddressesEasyNormal.Add(0x3362, 0x4005313C); // the silent cartogropher
            this.levelAddressesEasyNormal.Add(0x3462, 0x40051DA4); // assault on the control room
            this.levelAddressesEasyNormal.Add(0x3163, 0x40050700); // 343 guilty spark
            this.levelAddressesEasyNormal.Add(0x3263, 0x4005214C); // the library
            this.levelAddressesEasyNormal.Add(0x3463, 0x40051E10); // two betrayals
            this.levelAddressesEasyNormal.Add(0x3264, 0x40051D2C); // keyes
            this.levelAddressesEasyNormal.Add(0x3464, 0x40051BF4); // the maw

            // Initialize the list of 1st person campaign levels for heroic.
            this.levelAddressesHeroic = new Dictionary<short, int>();
            this.levelAddressesHeroic.Add(0x6975, 0x00000000); // ui (not in a level)
            this.levelAddressesHeroic.Add(0x3161, 0x4005193C); // the pillar of autumn
            this.levelAddressesHeroic.Add(0x3361, 0x400521C4); // halo
            this.levelAddressesHeroic.Add(0x3561, 0x400534C0); // the truth and reconciliation
            this.levelAddressesHeroic.Add(0x3362, 0x4005313C); // the silent cartogropher
            this.levelAddressesHeroic.Add(0x3462, 0x40051DA4); // assault on the control room
            this.levelAddressesHeroic.Add(0x3163, 0x40050700); // 343 guilty spark
            this.levelAddressesHeroic.Add(0x3263, 0x400521AC); // the library
            this.levelAddressesHeroic.Add(0x3463, 0x40051E10); // two betrayals
            this.levelAddressesHeroic.Add(0x3264, 0x40051D2C); // keyes
            this.levelAddressesHeroic.Add(0x3464, 0x40051BF4); // the maw

            // Initialize the list of 1st person campaign levels for legendary.
            this.levelAddressesLegendary = new Dictionary<short, int>();
            this.levelAddressesLegendary.Add(0x6975, 0x00000000); // ui (not in a level)
            this.levelAddressesLegendary.Add(0x3161, 0x4005193C); // the pillar of autumn
            this.levelAddressesLegendary.Add(0x3361, 0x400521A0); // halo
            this.levelAddressesLegendary.Add(0x3561, 0x400534C0); // the truth and reconciliation
            this.levelAddressesLegendary.Add(0x3362, 0x4005313C); // the silent cartogropher
            this.levelAddressesLegendary.Add(0x3462, 0x40051DA4); // assault on the control room
            this.levelAddressesLegendary.Add(0x3163, 0x40050700); // 343 guilty spark
            this.levelAddressesLegendary.Add(0x3263, 0x40052248); // the library
            this.levelAddressesLegendary.Add(0x3463, 0x40051E34); // two betrayals
            this.levelAddressesLegendary.Add(0x3264, 0x40051D2C); // keyes
            this.levelAddressesLegendary.Add(0x3464, 0x40051BF4); // the maw

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
        /// Disables unlimited human weapon ammunition.
        /// </summary>
        /// <returns>Returns true if the disabling process was successful.</returns>
        public bool DisableUnlimitedHummanAmmo()
        {
            return this.RestoreOriginalBytes(
                0x004c2696,
                "unlimited human weapon ammunition",
                new byte[] { 0x66, 0x89, 0x46, 0x08 });
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
        /// Disables unlimited grenades.
        /// </summary>
        /// <returns>Returns true if the disabling process was successful.</returns>
        public bool DisableUnlimitedGrenades()
        {
            return this.RestoreOriginalBytes(
                0x00569ce4,
                "unlimited grenades",
                new byte[] { 0xfe, 0x8c, 0x38, 0x1e, 0x03, 0x00, 0x00 });
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
        /// Disables unlimited flashlight power.
        /// </summary>
        /// <returns>Returns true if the disabling process was successful.</returns>
        public bool DisableUnlimitedFlashlightPower()
        {
            return this.RestoreOriginalBytes(
                0x0055f0ad,
                "unlimited flashlight power",
                new byte[] { 0xd9, 0x9b, 0x44, 0x03, 0x00, 0x00 });
        }

        /// <summary>
        /// Sets the player's invisibility value to 0x41.
        /// </summary>
        /// <returns>Returns true if the invisibility value was successfully set.</returns>
        public bool ResetInvisibility()
        {
            int invisibilityOffset = Marshal.OffsetOf(typeof(MasterChief), "Invisibility").ToInt32();
            bool result = false;
            PlayerBaseAddress ba = this.GetPlayerBaseAddress();

            Console.WriteLine("[*] Disabling invisibility...");

            // Create byte restoration utility.
            Action<IntPtr> restoreInvisibility = (baseAddress) =>
            {
                IntPtr invisibilityAddress = IntPtr.Add(baseAddress, invisibilityOffset);
                result |= this.Write(invisibilityAddress, new byte[] { 0x41 }, WriteOptions.None);
            };

            // Restore original shield values.
            restoreInvisibility(ba.EasyNormal);
            restoreInvisibility(ba.Heroic);
            restoreInvisibility(ba.Legendary);

            // Print the result.
            Console.WriteLine(result ? "[+] Success." : "[-] Failure.");

            // Return true if any of the writes succeeded.
            return result;
        }

        /// <summary>
        /// Sets the player's shields value to 1.
        /// </summary>
        /// <returns>Returns true if the shields value was successfully set.</returns>
        public bool ResetShields()
        {
            int shieldsOffset = Marshal.OffsetOf(typeof(MasterChief), "Shields").ToInt32();
            bool result = false;
            PlayerBaseAddress ba = this.GetPlayerBaseAddress();

            Console.WriteLine("[*] Disabling massive shields...");

            // Create byte restoration utility.
            Action<IntPtr> restoreShields = (baseAddress) =>
            {
                IntPtr invisibilityAddress = IntPtr.Add(baseAddress, shieldsOffset);
                result |= this.Write(invisibilityAddress, BitConverter.GetBytes(1.0f), WriteOptions.None);
            };

            // Restore original shield values.
            restoreShields(ba.EasyNormal);
            restoreShields(ba.Heroic);
            restoreShields(ba.Legendary);

            // Print the result.
            Console.WriteLine(result ? "[+] Success." : "[-] Failure.");

            // Return true if any of the writes succeeded.
            return result;
        }

        /// <summary>
        /// Freeze the player's shields at 15 (normal is 1, overshield is 3).
        /// </summary>
        protected override void FreezeThread()
        {
            // Initialize player base address.
            PlayerBaseAddress baseAddress = new PlayerBaseAddress();
            baseAddress.EasyNormal = IntPtr.Zero;
            baseAddress.Heroic = IntPtr.Zero;
            baseAddress.Legendary = IntPtr.Zero;

            int shieldsOffset = Marshal.OffsetOf(typeof(MasterChief), "Shields").ToInt32();
            int invisibilityOffset = Marshal.OffsetOf(typeof(MasterChief), "Invisibility").ToInt32();
            byte[] newShieldValue = BitConverter.GetBytes((float)15);
            byte[] newInvisibilityValue = new byte[] { 0x51 };

            while (true)
            {
                // Find the address again, in case a new level or level area was loaded.
                baseAddress = this.GetPlayerBaseAddress();

                if (!baseAddress.Equals(IntPtr.Zero))
                {
                    if (this.EnableMassiveShields)
                    {
                        this.Write(IntPtr.Add(baseAddress.EasyNormal, shieldsOffset), newShieldValue);
                        this.Write(IntPtr.Add(baseAddress.Heroic, shieldsOffset), newShieldValue);
                        this.Write(IntPtr.Add(baseAddress.Legendary, shieldsOffset), newShieldValue);
                    }

                    if (this.EnableInvisibility)
                    {
                        this.Write(IntPtr.Add(baseAddress.EasyNormal, invisibilityOffset), newInvisibilityValue);
                        this.Write(IntPtr.Add(baseAddress.Heroic, invisibilityOffset), newInvisibilityValue);
                        this.Write(IntPtr.Add(baseAddress.Legendary, invisibilityOffset), newInvisibilityValue);
                    }
                }

                Thread.Sleep(this.FreezeFrequency);
            }
        }

        /// <summary>
        /// Gets the base address of the current player's class object.
        /// </summary>
        /// <returns>
        /// Returns the base address of the player's class object, if a level is being played.
        /// Returns 0 if the player is in the home screen UI (i.e.: not in any level).
        /// </returns>
        private PlayerBaseAddress GetPlayerBaseAddress()
        {
            PlayerBaseAddress ba = new PlayerBaseAddress();
            ba.EasyNormal = IntPtr.Zero;
            ba.Heroic = IntPtr.Zero;
            ba.Legendary = IntPtr.Zero;

            if (!this.IsOpen)
            {
                return ba;
            }

            byte[] levelIdentifier = new byte[2];
            byte[] baseAddressPivot = new byte[4];

            // Find out which level is currently loaded.
            this.Read(IntPtr.Add(IntPtr.Zero, 0x4000000b), levelIdentifier);
            int levelAddressEasyNormal = this.levelAddressesEasyNormal[BitConverter.ToInt16(levelIdentifier, 0)];
            int levelAddressHeroic = this.levelAddressesHeroic[BitConverter.ToInt16(levelIdentifier, 0)];
            int levelAddressLegendary = this.levelAddressesLegendary[BitConverter.ToInt16(levelIdentifier, 0)];
            if (levelAddressEasyNormal == 0 || levelAddressHeroic == 0 || levelAddressLegendary == 0)
            {
                return ba;
            }
            else
            {
                // Obtain the pivot address that is close to the address of the character for easy or normal.
                this.Read(IntPtr.Add(IntPtr.Zero, levelAddressEasyNormal), baseAddressPivot);
                IntPtr pivotAddressEasyNormal = IntPtr.Add(IntPtr.Zero, BitConverter.ToInt32(baseAddressPivot, 0));

                // Obtain the pivot address that is close to the address of the character for heroic.
                this.Read(IntPtr.Add(IntPtr.Zero, levelAddressHeroic), baseAddressPivot);
                IntPtr pivotAddressHeroic = IntPtr.Add(IntPtr.Zero, BitConverter.ToInt32(baseAddressPivot, 0));

                // Obtain the pivot address that is close to the address of the character for heroic.
                this.Read(IntPtr.Add(IntPtr.Zero, levelAddressLegendary), baseAddressPivot);
                IntPtr pivotAddressLegendary = IntPtr.Add(IntPtr.Zero, BitConverter.ToInt32(baseAddressPivot, 0));

                // Calculate the offset for the character base addresses.
                ba.EasyNormal = IntPtr.Add(pivotAddressEasyNormal, 0x5C);
                ba.Heroic = IntPtr.Add(pivotAddressHeroic, 0x5C);
                ba.Legendary = IntPtr.Add(pivotAddressLegendary, 0x5C);

                // Return the base address for both easy/normal and heroic/legendary.
                return ba;
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
            Console.WriteLine("[*] Enabling " + thingEnabled + "...");
            if (this.NOPInstruction(address))
            {
                Console.WriteLine("[+] Success.");
                return true;
            }
            else
            {
                Console.WriteLine("[-] Failure.");
                return false;
            }
        }

        /// <summary>
        /// Restores some functionality in halo by resetting an instruction to its original bytes.
        /// </summary>
        /// <param name="address">
        /// The address of the thing that restricts the player's awesomeness. The instruction at this address will be
        /// replaced with the supplied bytes.
        /// </param>
        /// <param name="thingDisabled">The thing that makes the player more awesome.</param>
        /// <param name="originalBytes">The original bytes of the binary at the specified address.</param>
        /// <returns>Returns true if the enabling process was successful.</returns>
        private bool RestoreOriginalBytes(ulong address, string thingDisabled, byte[] originalBytes)
        {
            Console.WriteLine("[*] Disabling " + thingDisabled + "...");
            if (this.Write(IntPtr.Add(IntPtr.Zero, (int)address), originalBytes))
            {
                Console.WriteLine("[+] Success.");
                return true;
            }
            else
            {
                Console.WriteLine("[-] Failure.");
                return false;
            }
        }

        #endregion

        #region Structs

        /// <summary>
        /// Contains data about the current player, as it is represented in the game's memory.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct MasterChief
        {
            /// <summary>
            /// The X axis of the character's in-game position.
            /// </summary>
            [FieldOffset(0x0)]
            public float XAxisPosition;

            /// <summary>
            /// The Y axis of the character's in-game position.
            /// </summary>
            [FieldOffset(0x4)]
            public float YAxisPosition;

            /// <summary>
            /// The Z axis of the character's in-game position.
            /// </summary>
            [FieldOffset(0x8)]
            public float ZAxisPosition;

            /// <summary>
            /// The velocity the player is traveling along the X axis.
            /// </summary>
            [FieldOffset(0xc)]
            public float XAxisVelocity;

            /// <summary>
            /// The velocity the player is traveling along the Y axis.
            /// </summary>
            [FieldOffset(0x10)]
            public float YAxisVelocity;

            /// <summary>
            /// The velocity the player is traveling along the Z axis.
            /// </summary>
            [FieldOffset(0x14)]
            public float ZAxisVelocity;

            /// <summary>
            /// The direction that the player is facing.
            /// </summary>
            [FieldOffset(0x1c)]
            public float DirectionFacing;

            /// <summary>
            /// The amount of shields the player has.
            /// </summary>
            [FieldOffset(0x88)]
            public float Shields;

            /// <summary>
            /// A byte indicating the state of invisibility for the player.
            /// 0x51 indicates that the player is currently invisible.
            /// 0x41 indicates that the player is currently visible.
            /// </summary>
            [FieldOffset(0x1a8)]
            public byte Invisibility;

            /// <summary>
            /// The number of frag grenades that the player currently has.
            /// </summary>
            [FieldOffset(0x2c2)]
            public byte GrenadeFrags;

            /// <summary>
            /// The number of plasma grenades that the player currently has.
            /// </summary>
            [FieldOffset(0x2c3)]
            public byte GrenadePlasmas;
        }

        /// <summary>
        /// Represents the base address of the player, when on various difficulties.
        /// </summary>
        private struct PlayerBaseAddress
        {
            /// <summary>
            /// Gets or sets the base address of the player on easy or normal difficulty.
            /// </summary>
            public IntPtr EasyNormal { get; set; }

            /// <summary>
            /// Gets or sets the base address of the player on heroic difficulty.
            /// </summary>
            public IntPtr Heroic { get; set; }

            /// <summary>
            /// Gets or sets the base address of the player on legendary difficulty.
            /// </summary>
            public IntPtr Legendary { get; set; }
        }

        #endregion
    }
}

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
        /// The list of levels and their memory addresses.
        /// </summary>
        private Dictionary<short, int> levelAddresses;

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

            // Initialize the list of 1st person campaign levels.
            this.levelAddresses = new Dictionary<short, int>();
            this.levelAddresses.Add(0x6975, 0x00000000); // ui (not in a level)
            this.levelAddresses.Add(0x3161, 0x4005193C); // the pillar of autumn
            this.levelAddresses.Add(0x3361, 0x4005220C); // halo
            this.levelAddresses.Add(0x3561, 0x400534C0); // the truth and reconciliation
            this.levelAddresses.Add(0x3362, 0x4005313C); // the silent cartogropher
            this.levelAddresses.Add(0x3462, 0x40051DA4); // assault on the control room
            this.levelAddresses.Add(0x3163, 0x40050700); // 343 guilty spark
            this.levelAddresses.Add(0x3263, 0x4005214C); // the library
            this.levelAddresses.Add(0x3463, 0x40051E10); // two betrayals
            this.levelAddresses.Add(0x3264, 0x40051D2C); // keyes
            this.levelAddresses.Add(0x3464, 0x40051BF4); // the maw

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
            ulong invisibilityAddress = (ulong)IntPtr.Add(this.GetPlayerBaseAddress(), invisibilityOffset).ToInt64();
            return this.RestoreOriginalBytes(invisibilityAddress, "invisibility", new byte[] { 0x41 });
        }

        /// <summary>
        /// Sets the player's shields value to 1.
        /// </summary>
        /// <returns>Returns true if the shields value was successfully set.</returns>
        public bool ResetShields()
        {
            int shieldsOffset = Marshal.OffsetOf(typeof(MasterChief), "Shields").ToInt32();
            ulong shieldsAddress = (ulong)IntPtr.Add(this.GetPlayerBaseAddress(), shieldsOffset).ToInt64();
            return this.RestoreOriginalBytes(shieldsAddress, "massive shields", BitConverter.GetBytes(1.0f));
        }

        /// <summary>
        /// Read the master chief data from memory into a MasterChief structure.
        /// </summary>
        /// <returns>Returns a MasterChief structure populated with data from the game.</returns>
        public MasterChief ReadMasterChiefData()
        {
            return this.ReadStructure<MasterChief>(this.GetPlayerBaseAddress());
        }

        /// <summary>
        /// Freeze the player's shields at 15 (normal is 1, overshield is 3).
        /// </summary>
        protected override void FreezeThread()
        {
            IntPtr baseAddress = this.GetPlayerBaseAddress();
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
                        this.Write(IntPtr.Add(baseAddress, shieldsOffset), newShieldValue);
                    }

                    if (this.EnableInvisibility)
                    {
                        this.Write(IntPtr.Add(baseAddress, invisibilityOffset), newInvisibilityValue);
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
        private IntPtr GetPlayerBaseAddress()
        {
            if (!this.IsOpen)
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

        #endregion
    }
}

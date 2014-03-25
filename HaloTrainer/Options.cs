namespace HaloTrainer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// Command line options for the Halo trainer.
    /// </summary>
    class Options
    {
        [Option("ammo", HelpText = "Enable unlimited ammo. (enabled by default)", MutuallyExclusiveSet = "ammo")]
        public bool EnableUnlimitedAmmo { get; set; }

        [Option("no-ammo", HelpText = "Disable unlimited ammo.", MutuallyExclusiveSet = "ammo")]
        public bool DisableUnlimitedAmmo { get; set; }

        [Option("grenades", HelpText = "Enable unlimited grenades. (enabled by default)", MutuallyExclusiveSet = "grenades")]
        public bool EnableUnlimitedGrenades { get; set; }

        [Option("no-grenades", HelpText = "Disable unlimited grenades.", MutuallyExclusiveSet = "grenades")]
        public bool DisableUnlimitedGrenades { get; set; }

        [Option("flashlight", HelpText = "Enable unlimited flashlight power. (enabled by default)", MutuallyExclusiveSet = "flashlight")]
        public bool EnableUnlimitedFlashlightPower { get; set; }

        [Option("no-flashlight", HelpText = "Disable unlimited flashlight power.", MutuallyExclusiveSet = "flashlight")]
        public bool DisableUnlimitedFlashlightPower { get; set; }

        [Option("health", HelpText = "Enable unlimited health. (enabled by default)", MutuallyExclusiveSet = "health")]
        public bool EnableUnlimitedHealth { get; set; }

        [Option("reset-health", HelpText = "Disable unlimited health.", MutuallyExclusiveSet = "health")]
        public bool DisableUnlimitedHealth { get; set; }

        [Option("invisibility", HelpText = "Enable invisibility. (enabled by default)", MutuallyExclusiveSet = "invisibility")]
        public bool EnableInvisibility { get; set; }

        [Option("reset-invisibility", HelpText = "Resets the player's invisibility.", MutuallyExclusiveSet = "invisibility")]
        public bool ResetInvisibility { get; set; }

        [Option("massive-shields", HelpText = "Enable massive shields. (enabled by default)", MutuallyExclusiveSet = "shields")]
        public bool EnableMaximumShields { get; set; }

        [Option("reset-shields", HelpText = "Resets the player's shields.", MutuallyExclusiveSet = "shields")]
        public bool ResetShields { get; set; }

        [Option("reset-all", HelpText = "Restores original functionality and values to: ammo, grenades, flashlight power, invisibility, shields, and health")]
        public bool ResetAll { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            HelpText help = new HelpText
            {
                Heading = new HeadingInfo("halo-trainer"),
                Copyright = new CopyrightInfo("Carter Jones", 2014),
                AddDashesToOption = true
            };

            help.AddPreOptionsLine(" ");
            help.AddPreOptionsLine("Note: Run this program with administrator permissions to ensure that you can ");
            help.AddPreOptionsLine("      modify the halo.exe process. Running as a non-administrator can have ");
            help.AddPreOptionsLine("      unpredictable effects.");
            help.AddOptions(this);
            return help;
        }
    }
}

# halo-trainer

This project is used as an example of some of the capabilities of the
[nouzuru](https://github.com/carterjones/nouzuru) memory manipulation
framework. It employs memory patching techniques, continuous freezing of
values (re-writing memory every few milliseconds), as well as value tracking
of in-game structures, such as the Master Chief data structure.

## usage

    C:\halo-trainer>HaloTrainer.exe -h
    halo-trainer
    Copyright (C) 2014 Carter Jones

    Note: Run this program with administrator permissions to ensure that you can
          modify the halo.exe process. Running as a non-administrator can have
          unpredictable effects.

      --ammo                  Enable unlimited ammo. (enabled by default)
      --no-ammo               Disable unlimited ammo.
      --grenades              Enable unlimited grenades. (enabled by default)
      --no-grenades           Disable unlimited grenades.
      --flashlight            Enable unlimited flashlight power. (enabled by
                              default)
      --no-flashlight         Disable unlimited flashlight power.
      --invisibility          Enable invisibility. (enabled by default)
      --reset-invisibility    Resets the player's invisibility.
      --massive-shields       Enable massive shields. (enabled by default)
      --reset-shields         Resets the player's shields.
      --reset-all             Restores original functionality and values to: ammo,
                              grenades, flashlight power, invisibility, and shields
      --help                  Display this help screen.

    C:\halo-trainer>

Run a command prompt as administrator. Browse to the compiled directory
containing HaloTrainer.exe and run it. Leave the trainer running as long as
the game is open. This will allow infinite shields and invisibility.
Successful output looks like the following:

    C:\halo-trainer>HaloTrainer.exe
    [*] Enabling unlimited human weapon ammunition...
    [+] Success.
    [*] Enabling unlimited grenades...
    [+] Success.
    [*] Enabling unlimited flashlight power...
    [+] Success.
    [+] Invisibility enabled.
    [+] Massive shields enabled.
    Press any key to exit.

    C:\halo-trainer>

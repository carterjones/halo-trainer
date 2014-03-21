# halo-trainer

This project is used as an example of some of the capabilities of the
[nouzuru](https://github.com/carterjones/nouzuru) memory manipulation
framework. It employs memory patching techniques, continuous freezing of
values (re-writing memory every few milliseconds), as well as value tracking
of in-game structures, such as the Master Chief data structure.

## usage

Run a command prompt as administrator. Browse to the compiled directory
containing HaloTrainer.exe and run it. Leave the trainer running as long as
the game is open. This will allow infinite shields and invisibility.
Successful output looks like the following:

    C:> HaloTrainer.exe
    [*] Enabling unlimited human weapon ammunition...
    [+] Success.
    [*] Enabling unlimited grenades...
    disasm engine is not allowed to read more memory.
    [+] Success.
    [*] Enabling unlimited flashlight power...
    [+] Success.
    Press any key to exit.
    
    C:>

# LibConfig

A general purpose virtual configuration file processor.

## Brief

This library was developed with one goal.  To allow for a better and easier configuration management
system within the popular game 'Kerbal Space Program'.  Due to the limitations given to plugins that
run within this game, a normal file handling settings configuration manager would not work.  Thus this virtual
configuration file processor has been made.  If you have any way of saving and loading string arrays
then this library will work.  And so it is not just limited to the realm of 'Kerbal Space Program'.

## Features

 * Reads and writes to a virtual configuration file.
 * Uses a robust sectioned structure.
 * Does not have to use a sectioned structure if you do not want/need it to.
 * Has no hooks into the System.IO assemblies.
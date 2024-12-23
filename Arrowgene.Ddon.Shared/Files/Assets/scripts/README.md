# DDON Scripting

The DDON scripting is intended to expose certain server internal details of the game server that a server admin wish to be configure.
While initially implemented as JSON file, as more complex features come into the picture, a more complex configuration archirecture
is required.

The scripting root is `scripts` directory inside the assets directory. Internally the functions which perform reverse lookups for modules will
terminate once reaching the root.

Each directory inside scripts defines the scripting module. A module name should be all lowercase. Inside each module, there should be a `README.md`
file which describes the purpose and usage of the module. It should also describe any guidelines required. When implementing a module, be aware if
you want the module to be hotloadable. If you do, make sure to program in such a way that the settings can reflect as such after an update.

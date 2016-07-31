I have never used GitHub before so this may be a bit disorganized till I figure things out.

This is a project as an idea for a GUI for the Pokemon Go NecroBot or PokeMobBot. It is purely the GUI look/feel. It isn't integrated in any way with the other projects. Rather than submitting it to either project, I am offering this up as an idea that can be compiled and run on its own.

Since the config.json files have diverged between NecroBot and PokeMobBot, this project will need to be either one or the other. It is currently written to PokeMobBot as I liked the new options in the 1.0 release. 

The intention is that when compiling the project, the gui would compile just like the CLI and the GUI use the main classes to access the settings and listen to events. Currently neither project is setup for MVVM but it looks like creating the model is isolated to wherever something is logged to the console. 

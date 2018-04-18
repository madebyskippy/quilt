# quilt
This is a quilting grid-based game made by Tim Sun.

In each level of the game, you must touch all the spaces with your mouse.
However, you must avoid the intersections of the lines (generally where the corner of the spaces are).

The grid is morphing slightly the entire time, meant to simulate how fabric will fold and adjust as you are working on it. 
The levels are inspired by different quilt piecing patterns.
Gameplay requires precision, a little foresight, and some quick reflexes.

## tech details
The project was started in Unity and ported to Processing, with the intention to eventually put it on the web using something like p5.js.

The game reads text files containing a list of indicies for each space, based on a 20x20 grid.
There is also a level editor where you can use the the keyboard to set spaces and generate a text file for the game to read.

## misc
This game was started in Bennett Foddy's Prototype Studio class at NYU, with the personalized week-long prompt of "make a game a robot cannot lose", or something with set rules.

You can play the very first version, submitted in Prototype Studio, [here](https://madebyskippy.itch.io/sewing).

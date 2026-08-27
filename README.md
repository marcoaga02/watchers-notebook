# The Watcher's Notebook

A 2D top down game about learning by watching. You play as Man, moving through a world that follows its own rules, and
the goal is simply to understand it. There's no fight to win, just creatures to observe and obstacles that only make
sense once you've paid enough attention to them.

## How it plays

An old man stops you once near the start and explains the controls: how to check your notebook, how to interact with
things, how to run. After that he's just an NPC you can talk to again if you want.

Further along, other old men point you toward a nearby creature and comment on what they notice about it, without ever
naming what's actually going on mechanically. You have to work that part out yourself by watching.

You collect two kinds of things scattered across the map: sigils, which are abilities a creature can have, and
behaviors, which are the different ways a creature can use that ability. Some creatures swim fast, some swim slow, some
just float there and go nowhere. Watching them is the only way to tell which is which.

When you hit an obstacle, like a stretch of water or a chasm, you can open a panel and compose a creature from what
you've collected so far: pick the sigil, pick a behavior for it, and evoke it. Control passes to that creature while you
cross. Pick wrong and you can't get through, so you turn back and try something else.

The notebook keeps track of everything you've learned: which creatures you've observed, which sigils you know about, and
which behaviors you've actually seen for each one. Question marks fill in as you explore.

## Controls

- `Arrow keys` or `WASD` to move
- `Shift` to run
- `E` to interact, talk, collect, or open the evocation panel near an obstacle
- `Tab` to open your notebook
- `Esc` to close panels

## Built with

Unity, URP 2D, and the Unity Localization package (English and Italian).

## Credits

Character sprites (Man, Elf, Lizard, Old Man, Dragon) are generated with the Universal
LPC Spritesheet Character Generator
(https://liberatedpixelcup.github.io/Universal-LPC-Spritesheet-Character-Generator/),
combined piece by piece from many different contributors. Full per-piece attribution and
license terms (OGA-BY 3.0, CC-BY-SA 3.0, CC-BY 3.0/4.0, GPL 2.0/3.0, CC0 depending on the
piece) are listed in each character's own `credits.txt`, under
`Assets/Sprites/Characters/*/credits.txt`.

World sprites are from the Free Pixel Art Asset Pack, Topdown Tileset RPG 16x16
by Anokolisa (https://anokolisa.itch.io/free-pixel-art-asset-pack-topdown-tileset-rpg-16x16-sprites),
see `Assets/Sprites/PixelCrawler/Terms.txt`.

Icons are from Otsoga's Free GUI, Attribute and Element Icons
(https://otsoga.itch.io/free-gui-attr-and-element-icons), see
`Assets/Sprites/Otsoga_Icons/Icons/License.txt`.

UI frames, buttons, and panels are from bdragon1727's Free Pixel Frames, Buttons & Panels
Part 2 (https://bdragon1727.itch.io/free-pixel-frames-buttons-panels-part-2).

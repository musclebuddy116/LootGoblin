# LootGoblin

That about sums it up for now.

Loot Goblin Progress Report

I have implemented the following for Loot Goblin: A basic dungeon manager that randomly generates rooms from a list of room prefabs, rotating new rooms to ensure the entrance matches the cardinal of the room exited; a character sprite with 8 degree motion and 360 degree attack freedom, along with a dodge roll and room exiting; basic monster functionality, with a simple state machine AI and sword attack based when the monster nears the player; a simple loot system with each killed monster providing some amount of gold; a simple win condition when the player reaches 10 gold and exits a room; a lose condition when the player gets hit twice by a Goblin; a basic inventory system keeping track of what the player is carrying (in the prototype, this is automatically just a sword); a main menu with a controls explanation, a start button and a quit button.
I have still yet to implement the following: Boss fight as a dungeon floor completion; more monsters and better AI (the combat is quite unbalanced at the moment); loot in terms of more than just gold; health bar, inventory UI; shop/town phase; magical companion with room hints.
I think I am on schedule. I have most of my basic mechanics in place, I just need to tweak them to feel better and add the rest of my desired features, which should definitely keep me busy up to the due date.

Loot Goblin’s current gameplay loop: Enter a room, fight monsters, leave room, repeat until you have enough gold to win, or you die.

Youtube Link: https://youtu.be/AnEzfyx2Ssg

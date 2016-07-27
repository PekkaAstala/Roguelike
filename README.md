# Roguelike
Playing around with Unity 5.3

I decided to blow some dust off my Unity installation and followed this guide https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial. The original contents of this repo are more or less what you get by just following that guide with very little changes in functionality or structure. The assets used are the ones from Unity Technologies, etc.

Following the guide is fun as heck but the resulting code is a total mess. For example the Player class:
- Takes care of the board and collision detection (inherited from MovableObject)
- Stores player’s properties like food and wallDamage (these seem like they belong here)
- Stores a reference to an Animator object so it can change player’s animation between idle, chop and hurt (Getting iffy)
- Stores how much points every collectible in game provides in separate members like pointsPerFood and pointsPerSoda (Wait what?)
- Stores references to 7 AudioClips like “moveSound1”, “eatSound2” and “gameOverSound” so it can tell the SoundManager singleton to play them at appropriate times (Noooooo….)
- Stores references to UI elements like foodText so it can update them with hard-coded strings like foodText.text = "-" + loss + " Food: " + food; (Please stop)
- Stores game settings like “public float restartLevelDelay” (Someone somewhere actually thought "What would be the right place for this? Ah, the Player class surely!")

In other words the first enemy that the Player defeated seems to have been the Single Responsibility Principle.

So I decided to continue cleaning up the code a little (I still wouldn't call it good but reading it no longer makes me want to cry). I also have some plans for making the level generation and enemy AI more sophisticated etc. but we'll see if I'll ever get to that point.

## Setup

Just clone the repo and you should be able to open the project in Unity.

# Power Scaling Mod

This mod changes how the internal scaling of stats work.  
Whenever you level up, normally the game will add any percentages together, so `135% + 35% = 170%`.  
This results in a fairly low power cap, so I thought it would be fun if the stats were multiplicative as the games implies.  
This means that `135% + 35%` actually becomes `135% * (100% + 35%) = 182.25%`, as you can see this already results in significantly more power.

This mod is intended to be used with the Endless game mode and only works in that mode, other modes are unaffected by the scaling changes, nerfs, and other changes.

This mod requires [20MinutesTillDawn.Fixes](https://github.com/DragoonX6/20MinutesTillDawn.Fixes/releases), make sure it's in your BepInEx plugins folder.

# Balance

With the scaling changed I have made some balance changes to keep the game engaging and fun.

## Spawn sessions

The game has a different spawn session after the initial 20 minutes, after some testing this is extremely boring so I increased the amount of enemies and length of the session.  
For specifics look at `ModifyEndlessSpawnSessions.cs`.

## Changes to stats

Caps:
* Dodge has been capped to 70%  
  With movespeed going high very quickly dodge would always be at 100% making you immortal.
* Projectile speed is capped to 100  
  You would shoot over enemies otherwise, may need tweaking.
* Movespeed multiplier is capped to 10 for actual movement.  
  This only applies to moving your character (so you don't become faster than Sonic), the actual stat is still uncapped and has no effect on `Windborne`.

Nerfs:
* Take Aim has been limited to 1  
  From play testing there has never been a situation where I wanted this more than once because of the negative spread.
* Assassin has been limited to 1  
  Having more than one of these is useless.
* Focal Point has been limited to 1  
  Taking this more than once causes your lenses to be extremely small.
* Light Weapony has been limited to 1  
  See Dual Wield buff.
* Kill clip has been limited to 1  
  Having it more than once is too OP, now you need to work for it (a little bit).
* Frostbite has been limited to 1, and a single proc per enemy  
  Even by limiting it to just once, it would proc multiple times per enemy and essentially make the game a snoozefest after 30 minutes because everything dies instantly.
* Sharpen damage has been lowered to 15%
* Ritual growth percentage has been changed to 0.1%
* Electro Mastery's size growth has been limited to 5%
* Shatter's damage has been nerfed to 1%
* Intense Burn's damage has been nerfed to 10%  
  The damage would explode really quickly, now it can still do a lot of damage but requires more work.
* Electro Mastery's damage has been nerfed to 10%  
  The same as with intense burn, this one has a head start, but burn damage has way more procs.
* The entire dodge tree has been limit to 1 of each  
  Since you will reach the cap with 1 of each having more of it is wasting level ups.
* Windborne now only transforms your movespeed into extra damage once, subsequent uses give a 10% increase to Gale damage.  
  Way too OP otherwise, damage would be in the millions after 5 minutes of gameplay.
* Holy Arts has been limited to 1  
  See the Holy Arts buff.

Buffs:
* Initial experience growth has been increased by 50%  
  This is so you can keep up with the scaling of the enemies.
* Every 20 minutes your experience growth will increase by an additional 10%  
  See above.
* Sniper can be taken an unlimited amount of times
* Reaper Rounds can be taken an unlimited amount of times
* Aged Dragon can be taken an unlimited amount of times
* Rapid Fire can be taken an unlimited amount of times
* Sharpen can be taken an unlimited amount of times
* Vitality can be taken an unlimited amount of times
* Electro Mastery can be taken an unlimited amount of times
* Intense Burn can be taken an unlimited amount of times
* Armed and Ready can be taken an unlimited amount of times
* Dual Wield can be taken an unlimited amount of times
* Aero Mastery now does does 15% extra Gale damage, up from 15 flat Gale damage.
* Eye of the Storm now does 5% extra Gale damage, up from 5 flat Gale damage.
* Holy Arts now does % max health damage, 10% to normal enemies, 1% to boss enemies.
* Holy Arts range has been doubled.
* Holy Might increases Smite damage by 2.5% for each current HP you have.  
  Note "current HP", getting hit will lower your Smite damage.

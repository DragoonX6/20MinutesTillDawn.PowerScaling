# Power Scaling Mod

This mod changes how the internal scaling of stats work.  
Whenever you level up, normally the game will add any percentages together, so `135% + 35% = 170%`.  
This results in a fairly low power cap, so I thought it would be fun if the stats were multiplicative as the games implies.  
This means that `135% + 35%` actually becomes `135% * (100% + 35%) = 182.25%`, as you can see this already results in significantly more power.

This mod is intended to be used with the Endless game mode, and has only been tested in that mode. Currently it does not turn itself off when playing in a different mode.

# Balance

With the scaling changed I have made some balance changes to keep the game engaging and fun.

## Spawn sessions

The game has a different spawn session after the initial 20 minutes, after some testing this is extremely boring so I increased the amount of enemies and length of the session.  
For specifics look at `ModifyEndlessSpawnSessions.cs`.

## Nerfs and buffs

Nerfs:
* Take Aim has been limited to 1  
  From play testing there has never been a situation where I wanted this more than once because of the negative spread.
* Assassin has been limited to 1
* Focal Point has been limited to 1  
  Taking this more than once causes your lenses to be extremely small.
* Light Weapony has been limited to 1  
  See Dual Wield buff
* Kill clip has been limited to 1  
  Having it more than once is too OP, now you need to work for it (a little bit).
* Frostbite has been limited to 1, and a single proc per enemy  
  Even by limiting it to just once, it would proc multiple times per enemy and essentially make the game a snoozefest after 30 minutes because everything dies instantly.
* Sharpen damage has been lowered to 15%
* Ritual growth percentage has been changed to 0.1%
* Electro Mastery's size growth has been limited to 5%
* Shatter's damage has been nerfed to 1%

Buffs:
* Sniper can be taken an unlimited amount of times
* Reaper Rounds can be taken an unlimited amount of times
* Aged Dragon can be taken an unlimited amount of times
* Rapid Fire can be taken an unlimited amount of times
* Sharpen can be taken an unlimited amount of times
* Vitality can be taken an unlimited amount of times
* Electro Mastery can be taken an unlimited amount of times
* Intense Burn can be taken an unlimited amount of times
* Armed and Ready can be taken an unlimited amount of times
* Holy Might can be taken an unlimited amount of times
* Dual Wield can be taken an unlimited amount of times

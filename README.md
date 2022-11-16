### FunGuy Fury is a mod that adds hallucinogenic mushrooms (Fly Agaric or Amanita Muscaria) to the game. When eaten, you will go into a primal viking rage doubling damage to creatures, player, and trees.

<div style="text-align: center;"><a href="https://i.imgur.com/nABnseC.png"><img src="https://i.imgur.com/nABnseC.png" href="https://i.imgur.com/nABnseC.png" width="300" height="300"></a></div>

`This mod uses ServerSync internally. Settings can change live through the BepInEx Configuration manager (if you are in game) or by directly changing the file on the server. Can be installed on both the client (all clients) and the server to enforce configuration. Installation on the server is not needed, but will cause the synced values to be changeable. Note, not all configuration values are synced. Only the ones that matter.`

`Feel free to reach out to me on discord if you need manual download assistance.`

# This mod was made at the request of the PlainsWalkers. Check out their [discord](https://discord.gg/cy4bSvNVX7)!

<div style="text-align: center;"><a href="https://discord.gg/cy4bSvNVX7"><img src="https://i.imgur.com/MlJ2G2j.png" href="https://i.imgur.com/MlJ2G2j.png" width="256" height="256"></a></div>

## A Bit of History

---

<details>
<summary><b>Articles</b></summary>

These articles debate that the mushroom was even the cause of the trancelike berserker state. Though, some still say it
was the hallucinogenic properties of the mushroom that caused the berserker state.

* [Crazed Viking warriors may have been high on henbane](https://sciencenorway.no/drugs-history-plants/crazed-viking-warriors-may-have-been-high-on-henbane/1571431)
* [Viking berserkers may have used henbane to induce trance-like state](https://arstechnica.com/science/2019/09/viking-berserkers-may-have-used-henbane-to-induce-trance-like-state/)

</details>

<details>
<summary><b>About the Mushroom</b></summary> 
<a href="https://upload.wikimedia.org/wikipedia/commons/thumb/3/32/Amanita_muscaria_3_vliegenzwammen_op_rij.jpg/1280px-Amanita_muscaria_3_vliegenzwammen_op_rij.jpg"><img src="https://upload.wikimedia.org/wikipedia/commons/thumb/3/32/Amanita_muscaria_3_vliegenzwammen_op_rij.jpg/1280px-Amanita_muscaria_3_vliegenzwammen_op_rij.jpg" href="https://upload.wikimedia.org/wikipedia/commons/thumb/3/32/Amanita_muscaria_3_vliegenzwammen_op_rij.jpg/1280px-Amanita_muscaria_3_vliegenzwammen_op_rij.jpg" width="350" height="300"></a>

* [Amanita muscaria](https://en.wikipedia.org/wiki/Amanita_muscaria)

</details>

<details>
<summary><b>About the Plant</b></summary> 
<a href="http://t2.gstatic.com/licensed-image?q=tbn:ANd9GcSSp1um5pxyQn6uXL98nVC1iZuSGScHJJSYmFhX6Ix9Qf_2Q0hOOJRIo3mMFU38H4dUEWnSKjDy-HSggzA"><img src="http://t2.gstatic.com/licensed-image?q=tbn:ANd9GcSSp1um5pxyQn6uXL98nVC1iZuSGScHJJSYmFhX6Ix9Qf_2Q0hOOJRIo3mMFU38H4dUEWnSKjDy-HSggzA" href="http://t2.gstatic.com/licensed-image?q=tbn:ANd9GcSSp1um5pxyQn6uXL98nVC1iZuSGScHJJSYmFhX6Ix9Qf_2Q0hOOJRIo3mMFU38H4dUEWnSKjDy-HSggzA" width="350" height="300"></a>

* [Hyoscyamus niger](https://en.wikipedia.org/wiki/Hyoscyamus_niger)

</details>

## Additional Information and Available Configuration Options

---
<details>
<summary><b>Additonal Information/FAQs</b></summary>

> > `How long does it take for the mushroom to grow back?`
>  * 8 days (in-game) approx 14400 seconds
> > `Why doesn't all of my armor turn red?`
>  * Technically, the portion of your armor that is not turning red, is a material placed over the player skin. This
     will be fixed in a future update.
> > `Where can I find the mushroom?`
>  * The mushroom can be found in the swamp biome. It is a rare spawn. No, this is not configurable.
</details>

<details>
<summary><b>Available Configuration Options</b></summary>

Please note that not all configuration options are listed, only the ones that matter the most and are not from the
ItemManager (which allows basic configuration of the Agaric Mushroom Hat Item.

`[1 - General]`

* Lock Configuration [Synced with Server]
    * If on, the configuration is locked and can be changed by server admins only.
        * Default value:  On
* Damage Boost [Synced with Server]
    * Damage multiplier gained from eating the mushroom. Applies to creatures, players, and trees.
        * Default value:  2

`[2 - Berserk Effect]`

* Cooldown [Synced with Server]
    * Cooldown in seconds between each use of the effect. Prevents eating the food if the cooldown is not over. Displays
      the cooldown when failed to eat the food. Tweak the Cooldown Message to change the message displayed.
        * Default value:  15
* Duration [Synced with Server]
    * Duration in seconds of the berserker effect.
        * Default value:  15
* Start Message [Not Synced with Server]
    * Message displayed when the berserk effect starts.
        * Default value:  Damage increased at the cost of health!
* Stop Message [Not Synced with Server]
    * Message displayed when the berserk effect ends.
        * Default value:  Empty
* Cooldown Message [Not Synced with Server]
    * Message displayed when the cooldown is not over. {0} is replaced by the remaining time in seconds.
        * Default value:  You are still recovering from your last berserk rage! {0} remaining.
* Effect Tooltip [Not Synced with Server]
    * Tooltip shown when hovering over the Fly Agaric mushroom to describe the effect.
        * Default value:  <color=red>Increase damage x2, but at the cost of health loss over time</color>
* Damage Per Hit [Synced with Server]
    * Damage taken per hit while berserk. Set to 0 to disable damage.
        * Default value:  5
* Damage Interval [Synced with Server]
    * Interval in seconds between each damage taken while berserk.
        * Default value:  1

`[Fly Agaric Mushroom]`

* Weight [Synced with Server]
    * Weight of the Fly Agaric Mushroom.
        * Default value:  0.1
* Trader Value [Synced with Server]
    * Trader value of the Fly Agaric Mushroom.
        * Default value:  0

</details>

## Author Information

---
<details>
<summary><b>DETAILS</b></summary>

### Azumatt

`DISCORD:` Azumatt#2625

`STEAM:` https://steamcommunity.com/id/azumatt/

For Questions or Comments, find me in the Odin Plus Team Discord or in mine:

[![https://i.imgur.com/XXP6HCU.png](https://i.imgur.com/XXP6HCU.png)](https://discord.gg/Pb6bVMnFb2)
<a href="https://discord.gg/pdHgy6Bsng"><img src="https://i.imgur.com/Xlcbmm9.png" href="https://discord.gg/pdHgy6Bsng" width="175" height="175"></a>


</details>

## Special thanks!

> A massive thanks to the following people for their help and support:
> > `Gravebear`, thank you for tweaking the audio!
>
> > `Coemt`, thank you for helping test the mod!

### Changelog (Latest Listed First)

---
<details>
<summary><b>v1.0.1</b></summary>

> - README fix
</details>

<details>
<summary><b>v1.0.0</b></summary>

> - Initial Release
</details>
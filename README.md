# GDIM33 Vertical Slice
## Milestone 1 Devlog

1，This visual scripting graph use the state machine, which has two main states: Unarmed and PistolEquipped. The player starts in Unarmed, the pistol is hidden, and use normal movement animation,. When the player presses Tab, a transition checks the weapon-switch input and moves the system into PistolEquipped. In that state, the pistol becomes visible, the animator switches to pistol animation, and the graph keeps checking the right mouse button every frame. If the player is holding right click, the graph sets the aiming animation on and disables movement. if the player releases right click, it turns aiming off and allows movement again. Pressing Tab again will triggers the opposite transition and sends the player back to the Unarmed state.

2, [Break-down](https://docs.google.com/drawings/d/1NqhMj7zMddQUyuUmfBft3l2naNNAFNeYzsN6Gl916Xo/edit?usp=sharing)

I updated my break-down by making the inventory and weapon system more specific. In the new version, the inventory is connected to a state machine that controls which weapon state the player is in. When the player opens the inventory and clicks on a weapon, the state machine switches, and the player equips that weapon. Sate mashine really helped me make the system clearer. 

In addition, I added an aiming system to the break-down. When the player is already in a weapon state, they can right click to enter aiming mode. In this mode, the camera zooms in, the player cannot move, and left click can be used to shoot. The state machine is related to this aiming systems in my game, also animation , movement, and camera. 


## Milestone 2 Devlog
Milestone 2 Devlog goes here.
## Milestone 3 Devlog
Milestone 3 Devlog goes here.
## Milestone 4 Devlog
Milestone 4 Devlog goes here.
## Final Devlog
Final Devlog goes here.
## Open-source assets
- Cite any external assets used here!

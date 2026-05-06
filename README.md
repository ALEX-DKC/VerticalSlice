# GDIM33 Vertical Slice
## Milestone 1 Devlog

1，This visual scripting graph use the state machine, which has two main states: Unarmed and PistolEquipped. The player starts in Unarmed, the pistol is hidden, and use normal movement animation,. When the player presses Tab, a transition checks the weapon-switch input and moves the system into PistolEquipped. In that state, the pistol becomes visible, the animator switches to pistol animation, and the graph keeps checking the right mouse button every frame. If the player is holding right click, the graph sets the aiming animation on and disables movement. if the player releases right click, it turns aiming off and allows movement again. Pressing Tab again will triggers the opposite transition and sends the player back to the Unarmed state.

2, [Break-down](https://docs.google.com/drawings/d/1NqhMj7zMddQUyuUmfBft3l2naNNAFNeYzsN6Gl916Xo/edit?usp=sharing)

I updated my break-down by making the inventory and weapon system more specific. In the new version, the inventory is connected to a state machine that controls which weapon state the player is in. When the player opens the inventory and clicks on a weapon, the state machine switches, and the player equips that weapon. Sate mashine really helped me make the system clearer. 

In addition, I added an aiming system to the break-down. When the player is already in a weapon state, they can right click to enter aiming mode. In this mode, the camera zooms in, the player cannot move, and left click can be used to shoot. The state machine is related to this aiming systems in my game, also animation , movement, and camera. 


## Milestone 2 Devlog
1，

Step 1: Build the guard patrol system

1.	Add guard model in the scene, setting up the CharacterController in the inspector

2.	Setting up variable in the animator

3.	Create several waypoint objects in the scene and place them in the order where the guard are going to move. 

4.	Make the guard rotate toward the direction point and move toward it 

5.	When the guard reaches one waypoint, make it switch to the next waypoint. 

6.	After the guard reaches the last waypoint, make it move backward through the list so it patrols back and forth. 


Step 2: Add player detection and chase behavior

1.	Add variables for vision radius and shooting radius in the Guard script. 

2.	Add a bool variable like to control whether the guard is in patrol mode or alert mode

3.	If the guard is not alerted, make it continue walking between waypoints. 

4.	If the guard is alerted when player is inside the vision radius but not yet inside the shooting radius, make the guard chase the player. 

5.	While chasing, make the guard face the player and switch from walking animation to running animation. 


Step 3: Add shooting behavior when the player is close

1.	Check whether the player is inside the shooting radius. 

2.	If the player is inside the shooting radius, stop the guard’s movement. 

3.	Make the guard face the player 

4.	Switch the guard animation from run or walk to shoot. 

5.	If the player moves away, make the guard chase the player again

(I have not finished the alert trigger system yet, so for testing in the scene I placed two guards manually. One guard is set to stay in the normal patrol state, and the other guard is already set to the alerted state so I can test the chase and shooting behavior.)


2，

Yes, the task breakdown activity helped me build this milestone feature. At first, I thought the guard AI logic was a very complex system. But after breaking it down into steps, I could focus on one small part at a time. This made the whole problem much easier to solve. I first made the guard patrol, then I added chase and shooting behavior. Breaking the work into smaller steps helped me organized the task and easier to find mistakes.

If I did the breakdown again, I would include the Animator setup and animation transitions more clearly in the plan. This time, I was writing the code and building the Animator at the same time, and that caused a lot of strange problems. Next time, I would plan the animation logic earlier, such as which states I need, which parameters I need, and how the transitions should work. I think that would make the whole process smoother.


3，

I combined Visual Scripting and C# in my gun draw and aiming system. Visual Graph controlled the state logic, and the C# scripts handled input, movement, and animation functions, letting the Visual Scripting State Machine call methods from my C# scripts

The main C# scripts I used were InputManager, AnimatorManager, PlayerMovement, and CameraManager. In InputManager, I stored the player’s input states, such as whether the player is aiming or switching weapons. I also added methods like IsAiming(), IsChangeWeaponPressed(), and SetCanMove() so that the Graph could read input and control movement. In AnimatorManager, I added methods like SetAiming() and TriggerReload() so that the Graph could control animation changes without directly writing all animation logic inside the Graph.

In architecture, the Visual Scripting State Machine controlled the two main weapon states: Unarmed and PistolEquipped. The Graph handled when the player switched between these states, for example when pressing Tab. Inside the equipped state, the Graph checked whether the player was aiming, and then called methods from AnimatorManager and InputManager to change the animation and lock movement. While the C# scripts handled function of input, camera, movement, and Animator.

<img width="901" height="752" alt="截屏2026-05-05 下午9 13 39" src="https://github.com/user-attachments/assets/d5bc631e-be49-4d35-84f2-4b8feadc2492" />

<img width="1057" height="415" alt="截屏2026-05-05 下午9 13 50" src="https://github.com/user-attachments/assets/8252e3f5-2bcc-4883-837e-f58dffb0657a" />

<img width="411" height="488" alt="截屏2026-05-05 下午9 13 20" src="https://github.com/user-attachments/assets/eac73ee5-c085-4762-bb48-d0a720da3ba4" />





## Milestone 3 Devlog
Milestone 3 Devlog goes here.
## Milestone 4 Devlog
Milestone 4 Devlog goes here.
## Final Devlog
Final Devlog goes here.
## Open-source assets
- Cite any external assets used here!

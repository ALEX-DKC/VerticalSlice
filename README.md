# GDIM33 Vertical Slice
## Milestone 1 Devlog

### 1
This visual scripting graph use the state machine, which has two main states: Unarmed and PistolEquipped. The player starts in Unarmed, the pistol is hidden, and use normal movement animation,. When the player presses Tab, a transition checks the weapon-switch input and moves the system into PistolEquipped. In that state, the pistol becomes visible, the animator switches to pistol animation, and the graph keeps checking the right mouse button every frame. If the player is holding right click, the graph sets the aiming animation on and disables movement. if the player releases right click, it turns aiming off and allows movement again. Pressing Tab again will triggers the opposite transition and sends the player back to the Unarmed state.

### 2 
[Break-down](https://docs.google.com/drawings/d/1NqhMj7zMddQUyuUmfBft3l2naNNAFNeYzsN6Gl916Xo/edit?usp=sharing)

I updated my break-down by making the inventory and weapon system more specific. In the new version, the inventory is connected to a state machine that controls which weapon state the player is in. When the player opens the inventory and clicks on a weapon, the state machine switches, and the player equips that weapon. Sate mashine really helped me make the system clearer. 

In addition, I added an aiming system to the break-down. When the player is already in a weapon state, they can right click to enter aiming mode. In this mode, the camera zooms in, the player cannot move, and left click can be used to shoot. The state machine is related to this aiming systems in my game, also animation , movement, and camera. 


## Milestone 2 Devlog
### 1

Step 1: Build the guard patrol system

1.	Add guard model in the scene, setting up the CharacterController in the inspector

2.	Setting up variable in the animator

3.	Create several waypoint objects in the scene and place them in the order where the guard are going to move. 

4.	in the C# script, make the guard face the first waypoint, and move toward to it

5.	When the guard reaches one waypoint, make it rotate toward the next waypoint point and move toward to it 

6.	Make it move backward through the list so it patrols back and forth. 


Step 2: Add bool varible and chase behavior

1.	Add a bool variable to control whether the guard is in patrol mode or alert mode

2.	If the guard is not alerted, make it continue walking between waypoints. 

3.	Add variables for vision radius and shooting radius in the Guard script. 

4.	If the guard is alerted when player is inside the vision radius but not yet inside the shooting radius, make the guard chase the player. 

5.  In the animator, build different state of running, add animation, and switch from different state


Step 3: Add shooting behavior when the player is close

1.	Check whether the player is inside the shooting radius. 

2.	If the player is inside the shooting radius, stop the guard’s movement. 

3.	Make the guard face the player 

4.	Switch the guard animation from run or walk to shoot

5.	If the player is hit by a guard’s raycast, reduce the player’s health.

6.	If the player moves away, make the guard chase the player again


Step 4: alert trigger system

1. Add a front vision check so the guard only detect if the player is in front of the guard

2. If the player is seen by the guard, set to Alerted

3. If the guard is attacked by the player, set to true Alerted

### 2

Yes, the task breakdown activity helped me build this milestone feature. At first, I thought the guard AI logic was a very complex system. But after breaking it down into steps, I could focus on one small part at a time. This made the whole problem much easier to solve. I first made the guard patrol, then I added chase and shooting behavior, and alert switch. 

If I did the breakdown again, I would include the Animator setup and animation transitions more clearly in the plan. This time, I was writing the code and building the Animator at the same time, and that caused a lot of bugs. Next time, I would plan the animation logic earlier, such as which states I need, which variable I need, and how the transition connected. I think that would make the whole process smoother.




### 3

I combined Visual Scripting and C# in my gun draw and aiming system. Visual Graph controlled the state logic, and the C# scripts handled input, movement, and animation functions, letting the Visual Scripting State Machine call methods from my C# scripts

The main C# scripts I used were InputManager, AnimatorManager, PlayerMovement, and Guard. In InputManager, I stored the player’s input states, such as whether the player is aiming or switching weapons. I also added methods like IsAiming(), IsChangeWeaponPressed(), and SetCanMove() so that the Graph could read input and control movement. In AnimatorManager, I added methods like SetAiming() and TriggerReload() so that the Graph could control animation changes, but not directly writing all animation logic inside the Graph.

In architecture, the Visual Scripting State Machine controlled the two 3 weapon states: Unarmed rifleEquipped and PistolEquipped. The Graph handled when the player switched between these states. And inside the each state, for example, the Graph can checked whether press the botton, and then it will start aiming, and then called methods from AnimatorManager and InputManager to change the animation and lock movement. While the C# scripts handled function of input, camera, movement, and Animator.


<img width="845" height="231" alt="截屏2026-05-15 上午1 55 45" src="https://github.com/user-attachments/assets/66e5ab57-7354-4257-8313-1d28d8fa4e34" />

<img width="1183" height="855" alt="截屏2026-05-15 上午1 56 41" src="https://github.com/user-attachments/assets/a0048039-a548-4f1a-b6b3-06c651494494" />

<img width="687" height="496" alt="截屏2026-05-15 上午1 55 23" src="https://github.com/user-attachments/assets/cfa3c111-696c-494d-868f-1d60b7000fd5" />


### 4
Guard AI system and weapon system for Feature (3). 

The guards have two main states: patrol and alert. In patrol mode, they move between waypoint points. The alert state is triggered when the player enters the guard’s front 60-degree vision cone or when the guard is attacked. Once alerted, the guard will chase the player and shoot when the player is close enough. (I Add a raycast vision check. If a wall is between the guard and the player, the guard will not become alerted. )

You can also test the weapon system. The player can switch between unarmed, pistol, and rifle using number keys. The pistol and rifle have different animations and different damage values. The player can attack and kill guards, and the player can also be damaged and killed by guards.
 


## Milestone 3 Devlog
### 1
For my Vertical Slice project, I made a ShaderGraph called SG_Damage, and the material is Mat_DamageVignette
This shader is used as a full-screen post-processing effect. It appears when the player takes damage. 
The shader starts by using the Screen Position node. I split the screen position into X and Y values, then combine them into a Vector2. I compare this screen position to the center of the screen using a Distance node. This lets the shader know how far each pixel is from the center. Then I use a Smoothstep node with the Radius and Softness values. This creates a soft circle mask. The center of the screen stays mostly clear, while the edges of the screen become affected more strongly. I also use a URP Sample Buffer node with BlitSource. This is the original camera image. Then I use a Lerp node to blend between the original screen image and Red color. The Intensity value controls how strong the red damage effect is.

In gameplay, when the player is hit, a script increases and decrease the shader’s Intensity value. This makes the screen briefly flash red around the edges, so the player clearly understands that they took damage.

### 2
Based on feedback from the last playtest, I improved the player death and ending states. During the playtest, testers noticed that the player could still move after dead. To fix this, I added a Game Over system: when the player dead, player input is disabled, the game pauses, the screen becomes gray, and a “YOU DIED” message appears , and a Restart Game button appear. I also added a mission completion ending. After the player defeats the boss and reaches the exit room, input is disabled again, the screen gray, and a MISSION COMPLETE message appears. 

### 3
Since the last milestone, I finished designing the boss and placed the guards around the map. The boss normally patrols like other enemies, but once the player attacks it or he find the player, it becomes alerted and starts chasing the player in a faster speed. The boss when it gets close to the player, it punches the player in high damage. If the player runs away, the boss continues chasing. I also arranged the guard patrol routes in the level, so the player has to observe enemy movement, avoid detection, and plan a assassination route. This makes the gameplay loop clearer which is explore the level, avoid or kill guards, defeat the boss, and then escape to complete the mission.


## Final Devlog
### 1
The core gameplay loop of my game is to explore the space fortress, observe enemy patrols, choose a safe route, fight or avoid guards, kill the main target, and escape. In the current Vertical Slice, the player can move through 3 levels, switch between unarmed, pistol, and rifle states, perform stealth assassination. The game also includes several enemy types: normal guards, rifle guards, melee guards, and final boss. The player must manage health, weapon and positioning in the mission.

This content connects closely to my original plan. My original idea was a space gunfight & assassination game where a rebel enters a noble corporate fortress to kill a powerful CEO. The Vertical Slice does not include every future feature, such as disguises, a more advanced suspicion meter. My Vertical Slice is an early level of the game. It demonstrates the core gameplay loop: which are sneaking into a hostile area, reading enemy behavior, choosing between stealth and direct combat, and escaping after killing the target. The carefully arranged guard patrol positions, boss behavior design, multiple weapon choices, assassination system, HUD, sound effects, mission flow, and space-themed level design all help communicate what the full game would feel like.

### 2
<img width="771" height="606" alt="截屏2026-06-11 下午8 57 23" src="https://github.com/user-attachments/assets/19d358fc-db6e-472f-b323-2d1d7d4664da" />

C#: Assets/scripts/Player/ PlayerMovement.cs & Assets/shader/ DamagePostEffectController.cs

My rendering effect is a damage post-processing effect that appears when the player is hit. The relevant C# files are PlayerMovement.cs and DamagePostEffectController.cs. In PlayerMovement.cs, the function characterHitDamage(float takeDamage) is called when the player receives damage from an enemy. Inside this function, the player’s current health is reduced, and then the script calls damagePostEffectController.TriggerDamageEffect(). This connects the gameplay event of taking damage to the visual feedback on screen.

The rendering effect uses a fullscreen damage vignette Shader Graph. The shader is assigned to a material, Mat_DamageVignette, and used through a URP Full Screen Pass Renderer Feature. The shader samples the camera color buffer and blends the original screen color with a red damage color near the edges of the screen. The strength of the effect is controlled by an intensity value. When the player is hit, DamagePostEffectController.cs increases the intensity, and then gradually fades it back down over time. 

### 3
For my planning process, I did use bubble diagram and breakdowns and task step breakdowns. First, I made a mood board to decide the visual style, atmosphere, and inspiration for the game, which is relate to cyberpunk, space, high-tech interior. Then, I made a pitch that listed the main mechanics, theme, technical systems, and story/background of the game.
After that, I used a bubble diagram to break the game into main objects and systems, such as the player, guards, boss, weapons, assassination system, health system... I used lines to show how these systems connect. After making the diagram, I broke the project into smaller tasks or function. Then, each function was divided into smaller steps. For example, for the health HUD, I first created the UI images, place it on the screen, and then connected them to the player health value through c# script. Then, I tested damage, and finally fixed problems. This process made the project easier to build because I knew what to work on first and next, and how each part connected to the whole game.

At first, a game idea can sound very complex, and I do not know where to start. However, when I draw diagrams and break the game into smaller systems, I can see how many small parts are actually needed. During the project, I tried to complete one or two systems each week. By following this plan step by step, I was able to slowly build the whole Vertical Slice.

This method also helped me understand the scope of the project. It helped me focus on the core gameplay loop and decide what was necessary for the Vertical Slice and what should be cut or saved for future development. For example, I originally thought about adding a disguise system and a more advanced alert system. However, after breaking down the project, I realized those features were too large for the current scope. I should focuse on the core experience and unique features of my game, which is different weapon choices, different enemy behaviors, and assassinate route design. These systems allow the player to understand the main fun of the game from the Vertical Slice.

This planning process relates directly to how I created my Vertical Slice. Some parts went well because I separated them into clear systems. For example, after I built the health HUD, I could test it directly. If I found a problem, such as the health not decreasing correctly, I could look at the health script the change instead of searching randomly through all of the code and settings. This helped me find and fix problems more easily

However, some parts also showed me that I should plan more carefully next time. Especially the animation system, before this project, I had not made animation logic this complicated. I had to connect animation states with C# scripts and Visual Scripting, which became very complex. Because I did not break down the animation steps clearly enough at the beginning, I did not set up all the needed parameters and variables in advance. This caused many errors, and I had to repeatedly change the code, Visual Script graphs, and Animator settings. From this experience, I learned that for complex systems, I should first list all required animation states, parameters, transitions, scripts, and triggers before building the system. In the future, I will make a more detailed task breakdown for animation, and set all the variable at first, which can make my mind clear, and easy for testing and fixing bugs.



## Open-source assets
- Cite any external assets used here!

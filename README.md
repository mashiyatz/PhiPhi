# PhiPhi's Quantum Tuna Cans
Code and documentation for "PhiPhi's Quantum Cans" by Yuke Ding and Mashi Zaman. Rotate a quantum can of tuna to increase the odds of feeding PhiPhi the cat, and learn a little bit about quantum mechanics along the way.

### Introduction

Welcome to the quantum world! An extraordinary place where flowers are pink and blue and red and yellow at the same time before you see them, and the weather is both sunny and rainy before you look out the window. What’s inside this tuna can for kittens? Fish? A bone? Both, of course! 

This is PhiPhi, a kitten eager for some delicious canned tuna. But as this is the quantum world, there’s always a chance of there being no trace of fresh tuna inside. However, you (yes, you!) could become PhiPhi’s favorite human and give them all the tuna they could ever want today!  

Having lived in the quantum world for quite a while, PhiPhi can tell the probability of tuna inside the quantum can. PhiPhi also knows that you can change this probability by rotating the can, and that there is even a special direction that holds the greatest probability of getting tuna!  

When you’re ready, press the can to start! Rotate the can while watching how PhiPhi reacts. Then, press the can again to see if there’s tuna inside. Good luck!

If you want to learn more about all the quantum behind the scenes, long press the tuna can. PhiPhi will share a bit about what they’ve learned from the quantum world!

### Technical Description
A tuna can attached to a rotary encoder (i.e., a button that rotates) behaves as the controller for the game. An Arduino Nano IoT 33 sends rotations and button presses from the rotary encoder to Unity3D through serial communication. A digital interface made in Unity3D responds to both button presses and rotations from the rotary encoder. Short and long button presses are used to change between different sections of the project, i.e., instructions, the game, and background information. Encoder rotations influence the probabilities of positive or negative outcomes, which are reflected through animations of the cat PhiPhi. 

Instructions onscreen, presented using both animations and simple text, outline the interaction’s objective — to feed PhiPhi. They introduce the main loop of player actions using the larger tuna can-turned-button in front: a short press to start the game, followed by rotation to increase the probability of reward, and then another short press to calculate the result based on this probability.

Players who choose to participate will rely on PhiPhi’s reactions to the tuna can’s orientation to achieve the most optimal outcome. Visual cues will signal to the player whether they will achieve the best possible outcome. After seeing the result, and returning to the first screen, the player will have the option of repeating the interaction. They may also choose to learn about the physics background of the interaction by long pressing the tuna can.

Players who choose to explore the physics background are able to read and reread a set of informational graphics on quantum mechanics and how it relates to the interaction. Players can interact with most of these graphics by rotating the can. They can choose to return to the interaction with PhiPhi through another long press.

### Takeaways and Ideas for Future Projects
* Not everyone reads directions! Delivering information in visually and in bite-sized pieces proved effective in guiding players. 
* Not everyone hates physics! Many players were curious about how this game has anything to do with quantum physics.
* A good, responsive button goes a long way. Players enjoyed just rotating and pressing the tuna can, sometimes a bit too much.
* A rotary encoder is incredibly versatile, we should consider more interactive experiences that just use a rotation and button. 

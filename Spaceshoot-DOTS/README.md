# spaceshooterDOTS

A Spaceshooter made with ECS in Unity as an assignment for Futuregames

Movement
>Orientiation - A and D key >Shooting - Space Button

NOTE:
In the build, it takes a while for asteroids to start spawning

To meet the requirements, I used Unity DOTS which by default requires you to seperate data, system and entities. I utilized DOTS jobs and the BurstCompiler a lot, which lets me spread my work into multiple processor threads, and is more effective, leaving your main thread with more time to do other things. This makes DOTS more useful if you're doing something with a lot of jobs, like thousands of asteroids spawning. The BurstCompiler is really useful when using multiple jobs because it translates the code into more optimized native code, which is way faster and improves performance greatly.

DOTS turns Object Oriented Programming into Data Oriented Design which, for me, was a steep learning curve. Learning about ISystems, SystemBase, IComponentData, Bakers, Authoring and how everything works together took a quite a while and at some point I was really stuck. I wouldn't say I prefer DOD to OOP but it was a great learning experience about the fundementals of programming and optimizing code, and how caching and reallocating really works.

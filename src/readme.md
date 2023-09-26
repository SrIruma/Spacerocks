The version i used for build this project on Unity is 2022.3.7f1, the project has compatibility for Windows, Mac, Linux, WebGL HTML. I did not test on Android but it might work fine on it.

You will need to make some configs to the Layers on Unity, to work fine. 
The game need this Order on Layers:

0. Default
1. TransparentFX
2. Ignore Raycast
3. Invensible
4. Water
5. UI
6. Player
7. Asteroids
8. Bullet
9. Walls

You can download and incorporate the Assets in this Directory or go to [[Package](https://github.com/SrIruma/Spacerocks/tree/main/builds/spacerocks.unitypackage)] on the build repository and use that one.

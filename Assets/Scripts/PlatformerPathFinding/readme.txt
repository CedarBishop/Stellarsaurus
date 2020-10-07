Thanks for downloading my asset.
Steps to make it work:
1) Create empty game object call it something like PathFindingGrid.
2) Add PathFindingGrid script to it.
3) Adjust the parameters, make sure draw grid is true.
4) Press Build, make sure the grid is correct.
5) Adjust the position of the game object you created earlier.

6) Add PathFindingAgent script to all agents you want.
Set height, width, jump strength and fall limit for each one of them.
This values are in node units, so if height = 2, it means that 
agent takes two nodes height.

7) Add AiController script or write your own. Adjust the fields.

If there are lots of agents you can create script which will handle 
them all or use ECS (better option).


There is also a demo scene called OneAgent. You can just copy 
everything from there.
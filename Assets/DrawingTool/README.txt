Thanks for buying Drawing Tool !!

********************************
HELP :

If you need help, have a look at this video to understand how to customize this drawing system : https://www.youtube.com/watch?v=2hC7KBe0Rpo

If you wanna change the size of the drawing frame, you just have to change the size of the GameObject Image placed in : Canvas > DrawingTab > DrawingCanvas > Image.

********************************

********************************
RECOMMENDATION : 

If you wanna use this it's recommended to create a layer named Drawing and to change the property CameraLayer of the component Drawer to the layer you just created. Then you change the GameObject DrawSpace and all it's children to the layer you just created too ! Finally, you go to the camera child of DrawSpace and you change his culling mask property to only show the layer you created.

********************************

********************************
CREATE YOUR OWN BRUSH :

If you wanna create your own drawing tool, you have to create a new class extending the class Brush. You can look at this class and you'll see that you can override a few methods like.
The concept of the basic brush is that it create a copy of himself at every frame when you keep the mouse pressed. 
The concept of the line brush is that it create a lineRenderer when you click on the frame, on each frame, you move the second point to correspond with the position of the mouse and finally, when you release your mouse, you just let the lineRenderer as it is.
If you wanna create cool tool like a square tool or a triangle tool, it's very easy ! You just have to reproduce the lineBrush and add the creation of multiple lineRenderer that you will place at every frame.

********************************

This package also include a fast dropdown tool I made, it's a gift :p

********************************
CONTACT: 
If you need any assistance, don't hesitate to contact me at quentin.mallen.virtualmind@gmail.com

********************************
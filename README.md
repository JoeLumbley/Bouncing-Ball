# Bouncing Ball

This application displays a single ball that moves smoothly across the screen using delta‑time‑based animation. When the program starts, the ball is placed at the exact center of the window. A random direction is chosen, and the ball begins traveling across the client area at a constant speed.








<img width="1147" height="796" alt="003" src="https://github.com/user-attachments/assets/cfac7e83-bc26-4b42-8a80-2fcbc57d6848" />


The movement is updated inside a high‑frequency timer, which calculates how much real time has passed between frames. This ensures the animation stays smooth and consistent even if the frame rate changes.

As the ball travels, it checks for collisions with the edges of the window. When the ball reaches any boundary — left, right, top, or bottom — it “bounces” by reversing the appropriate velocity component. This creates natural, physics‑style motion where the ball ricochets around the screen indefinitely.

The app also includes a simple FPS counter drawn directly onto the form. It updates once per second and shows how many frames were rendered, giving a clear visual indicator of performance.

Overall, the program demonstrates:

Delta‑time animation

Randomized initial motion

Boundary collision detection

Velocity reflection for bouncing

Smooth rendering with double buffering

Real‑time FPS measurement

It’s a compact, beginner‑friendly example of game‑loop logic inside a Windows Forms environment.

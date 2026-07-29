# 🔵 Bouncing Ball

This application displays a single ball that moves smoothly across the screen using delta‑time‑based animation. 

<img width="1920" height="1080" alt="005" src="https://github.com/user-attachments/assets/f1ec88e1-21b1-4f84-9afb-eabb017ae2a2" />

When the program starts, the ball is placed at the exact center of the window. A random direction is chosen, and the ball begins traveling across the client area at a constant speed.

The movement is updated inside a high‑frequency timer, which calculates how much real time has passed between frames. This ensures the animation stays smooth and consistent even if the frame rate changes.

As the ball travels, it checks for collisions with the edges of the window. When the ball reaches any boundary — left, right, top, or bottom — it “bounces” by reversing the appropriate velocity component. This creates natural, physics‑style motion where the ball ricochets around the screen indefinitely.

The app also includes a simple FPS counter drawn directly onto the form. It updates once per second and shows how many frames were rendered, giving a clear visual indicator of performance.

--- 

# 🎓 Learning Objectives

This project is designed to teach:

- How animation works  
- How physics loops work  
- How rendering loops work  
- How to avoid flicker  
- How to optimize without complexity  
- How to structure real-time code cleanly  

It’s a introduction to game programming concepts using Windows Forms and VB.NET.






---


# 🔵 Bouncing Ball - Code Walkthrough  
*A line‑by‑line explanation of how this animation works and why we built it this way.*

This project shows how to animate a bouncing ball in Windows Forms using smooth motion, trails, and an FPS counter.  
The goal is to teach you **how real-time graphics work**, using simple, readable code.

---

```vb
Imports System.Drawing.Drawing2D
```

- This line brings in the **Drawing2D** namespace from .NET’s GDI+ drawing library.
- It gives you access to advanced rendering features such as:
  - `SmoothingMode`
  - `InterpolationMode`
  - `PixelOffsetMode`
  - `CompositingMode`
- Without this import, you’d have to fully qualify those types (e.g., `System.Drawing.Drawing2D.SmoothingMode.AntiAlias`).

In short: **This import enables high‑quality graphics rendering options for your form.**

---

```vb
Public Class Form1
```

- This begins the definition of your Windows Forms class.
- `Form1` inherits from `System.Windows.Forms.Form` (implicitly).
- Everything that follows—fields, methods, event handlers—belongs to this form.
- When the program runs, this class becomes the window the user sees.

In short: **This is the main window of your application, and all your animation logic lives inside it.**

---

```vb
' -------------------------------
'  Engine State
' -------------------------------
Private ballPos As PointF
Private ballDiameter As Integer = 80

Private velX As Double
Private velY As Double
Private speed As Double = 450

Private physicsTimer As New Timer()
Private sw As New Stopwatch()
```

- **Comment block (Engine State):** Just a visual separator in the code, labeling the next section as “Engine State”.
- **`Private ballPos As PointF`**: Declares a private field that stores the ball’s current position as a 2D point (`X`, `Y`).
- **`Private ballDiameter As Integer = 80`**: Declares the ball’s diameter in pixels and initializes it to `80`.
- **`Private velX As Double`**: Declares the horizontal velocity component of the ball (pixels per second).
- **`Private velY As Double`**: Declares the vertical velocity component of the ball (pixels per second).
- **`Private speed As Double = 450`**: Declares a base speed value (magnitude) for the ball’s movement and initializes it to `450`.
- **`Private physicsTimer As New Timer()`**: Creates a `Timer` that will drive the physics updates (the fixed‑timestep loop).
- **`Private sw As New Stopwatch()`**: Creates a `Stopwatch` used to measure elapsed time between physics ticks (for `dt`).

---

```vb
' -------------------------------
'  FPS Tracking
' -------------------------------
Private frameCount As Integer = 0
Private fps As Integer = 0
Private fpsTimer As New Stopwatch()
```

- **Comment block (FPS Tracking):** Labels this section as related to frames‑per‑second tracking.
- **`Private frameCount As Integer = 0`**: Counts how many frames have been rendered in the current one‑second window.
- **`Private fps As Integer = 0`**: Stores the calculated FPS value that will be displayed on screen.
- **`Private fpsTimer As New Stopwatch()`**: Measures time to know when one second has passed so FPS can be updated.

---

```vb
' -------------------------------
'  GDI Resources
' -------------------------------
Private ballBrush As SolidBrush
Private fpsBrush As SolidBrush
Private fpsFont As Font
Private trailBrushes As SolidBrush()
```

- **Comment block (GDI Resources):** Marks the section for drawing resources.
- **`Private ballBrush As SolidBrush`**: Brush used to fill the ball when drawing.
- **`Private fpsBrush As SolidBrush`**: Brush used to draw the FPS text.
- **`Private fpsFont As Font`**: Font used for the FPS text rendering.
- **`Private trailBrushes As SolidBrush()`**: An array of brushes used to draw each segment of the trail with varying alpha.

---

```vb
' -------------------------------
'  Trail System
' -------------------------------
Private trail As New List(Of PointF)
Private trailLength As Integer = 25
Private trailSizes As Integer()
Private trailOffsets As Single()
```

- **Comment block (Trail System):** Labels the section for the motion trail behind the ball.
- **`Private trail As New List(Of PointF)`**: A list storing past positions of the ball to render the trail.
- **`Private trailLength As Integer = 25`**: Maximum number of trail points (segments) to keep and draw.
- **`Private trailSizes As Integer()`**: An array holding precomputed sizes (diameters) for each trail segment.
- **`Private trailOffsets As Single()`**: An array holding precomputed offsets so each trail ellipse is centered relative to the main ball.

---



```vb
Public Sub New()
```
This is the form’s constructor. It runs **once**, the moment the form is created.

---

```vb
InitializeComponent()
```

Loads everything designed in the Windows Forms Designer: controls, properties, layout, etc.  
Every WinForms form calls this first.

---

```vb
Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer, True)
```
This line enables three important rendering flags:

- **AllPaintingInWmPaint**  
  Prevents background erasing, reducing flicker.

- **UserPaint**  
  Tells WinForms that *you* will handle all painting manually (via `OnPaint`).

- **OptimizedDoubleBuffer**  
  Enables double‑buffering at the control level, eliminating flicker during animation.

Setting them to `True` applies the combined flags.

---

```vb
Me.DoubleBuffered = True
```

A second layer of double‑buffering.  
Even though you already set `OptimizedDoubleBuffer`, this property ensures the form itself uses a back‑buffer.

---

```vb
Me.BackColor = Color.Black
```

Sets the form’s background color to black — the canvas for your animation.

---

## 🎯 **Center ball**

```vb
ballPos = New PointF((ClientSize.Width - ballDiameter) / 2, (ClientSize.Height - ballDiameter) / 2)
```

This computes the centered position:

- `ClientSize.Width - ballDiameter` → remaining horizontal space  
- Divide by 2 → center horizontally  
- Same logic vertically  

Creates a `PointF` representing the ball’s starting position.

---

## 🎲 **Random direction**

```vb
Dim rnd As New Random()
```

Creates a random number generator.

```vb
Dim angle As Double = rnd.NextDouble() * Math.PI * 2
```

Generates a random angle between **0 and 2π** (full circle).

```vb
velX = Math.Cos(angle) * speed
```

Computes the horizontal velocity based on the angle.

```vb
velY = Math.Sin(angle) * speed
```
Computes the vertical velocity based on the angle.

Together, these give the ball a random direction with the same speed magnitude.

---

## ⚙️ **Physics at ~60 FPS**

```vb
physicsTimer.Interval = 15
```

Sets the timer to tick every **15 ms**, which is ~66 updates per second.

```vb
AddHandler physicsTimer.Tick, AddressOf PhysicsTick
```
Connects the timer’s `Tick` event to your physics update method.

Every tick → `PhysicsTick()` runs.

---

## ⏱️ **Start timing systems**

```vb
sw.Start()
```

Starts the stopwatch used to measure delta‑time (`dt`) between physics updates.

```vb
fpsTimer.Start()
```

Starts the stopwatch used for FPS measurement.

---

```vb
End Sub
```

Marks the end of the constructor.

---




# *OnLoad — Preparing all graphics resources before the animation begins*

`OnLoad` runs **once**, right after the form is created and just before it becomes visible.  
This is the perfect place to initialize brushes, fonts, trail arrays, and anything that depends on the form’s size.

---

```vb
Protected Overrides Sub OnLoad(e As EventArgs)
```

This overrides the form’s built‑in `OnLoad` method.  
It fires when the form is fully constructed and ready to initialize runtime resources.

---

```vb
MyBase.OnLoad(e)
```

Calls the base class version of `OnLoad`.  
This ensures WinForms performs its normal setup before your custom logic runs.

---

## 🎨 Core GDI Resources

```vb
ballBrush = New SolidBrush(Color.DeepSkyBlue)
fpsBrush = New SolidBrush(Color.White)
fpsFont = New Font("Segoe UI", 14, FontStyle.Bold)
```

These are the core drawing resources:

- **ballBrush** — fills the ball with a bright blue color  
- **fpsBrush** — draws the FPS text in white  
- **fpsFont** — sets the font used for the FPS counter  

In short: **These are the main tools used to draw your scene.**

---

## 🟦 Preallocate Trail Brushes

```vb
trailBrushes = New SolidBrush(trailLength - 1) {}
```

Creates an array of `SolidBrush` objects with `trailLength` entries.

This avoids allocating brushes during animation, which keeps performance smooth.

---

```vb
For i As Integer = 0 To trailLength - 1
    trailBrushes(i) = New SolidBrush(Color.FromArgb(0, 0, 191, 255))
Next
```

Initializes each brush with a fully transparent blue color:

- Alpha = **0**  
- RGB = **(0, 191, 255)**  

The alpha will be changed dynamically during drawing to create the fading trail effect.

In short: **Every trail segment gets its own brush, ready to have its alpha adjusted.**

---

## 📏 Precompute Trail Sizes and Offsets

```vb
trailSizes = New Integer(trailLength - 1) {}
trailOffsets = New Single(trailLength - 1) {}
```

Creates two arrays:

- **trailSizes** — the diameter of each trail circle  
- **trailOffsets** — how much each circle must be shifted to stay centered  

Precomputing these values avoids doing math inside the render loop.

---

```vb
For i As Integer = 0 To trailLength - 1
    Dim size As Integer = ballDiameter - (trailLength - i) * 2
    If size < 10 Then size = 10
```

This loop calculates the size of each trail segment:

- Older trail segments (lower `i`) get **smaller circles**  
- Newer trail segments (higher `i`) get **larger circles**  
- Minimum size is clamped to **10 pixels**

This creates a tapered, comet‑like trail behind the ball.

---

```vb
    trailSizes(i) = size
    trailOffsets(i) = CSng((ballDiameter - size) / 2)
Next
```

Stores:

- the computed size  
- the offset needed to center the smaller circle inside the ball’s position  

The offset is `(ballDiameter - size) / 2`, which centers the ellipse perfectly.

In short: **Each trail circle is smaller and centered relative to the ball.**

---

## ▶️ Start the Physics Engine

```vb
physicsTimer.Start()
```

Begins the fixed‑timestep physics loop.

From this moment on:

- the ball moves  
- collisions are checked  
- the trail updates  
- the form invalidates and redraws  

This is the moment the animation officially starts.

---

```vb
End Sub
```

Marks the end of the `OnLoad` method.

---





































---
 
# *PhysicsTick — The heartbeat of the animation*

This method runs every 15 ms (≈66 times per second).  
It updates the ball’s position, handles collisions, updates the trail, and triggers a redraw.

---

```vb
Private Sub PhysicsTick(sender As Object, e As EventArgs)
```

This is the event handler for the timer’s `Tick` event.  
Every time the timer fires, this method executes one physics update.

---

## ⏱️ Measure Delta‑Time (dt)

```vb
Dim dt As Double = sw.Elapsed.TotalSeconds
sw.Restart()
```

- `sw.Elapsed.TotalSeconds`  
  Reads how much time has passed since the last physics tick.
- `sw.Restart()`  
  Resets and starts the stopwatch again for the next tick.

This gives you a **precise delta‑time**, which makes movement smooth even if the timer drifts slightly.

In short: **dt = time since last update.**

---

# 🛑 Clamp dt (Safety Against Lag Spikes)

```vb
dt = Math.Min(dt, 0.05)
```

If the app freezes or lags for a moment, `dt` could become very large.  
Large dt → ball teleports across the screen → breaks physics.

Clamping dt to **0.05 seconds** (50 ms) prevents runaway movement.

In short: **Never allow dt to exceed a safe maximum.**

---

# 🎯 Update Ball Position

```vb
ballPos.X += CSng(velX * dt)
ballPos.Y += CSng(velY * dt)
```

This applies velocity to position:

- `velX * dt` → how far the ball moves horizontally this frame  
- `velY * dt` → how far the ball moves vertically this frame  

`CSng()` converts the result to `Single` because `PointF` uses `Single` values.

In short: **Move the ball based on velocity and elapsed time.**

---

# 🧱 Handle Collisions

```vb
HandleCollisions()
```

Checks whether the ball hit any of the window edges:

- left  
- right  
- top  
- bottom  

If so, it adjusts position and reverses velocity.

In short: **Bounce the ball off the walls.**

---

# 🟦 Update Trail

```vb
UpdateTrail()
```

Adds the ball’s current position to the trail list and removes old entries.

This creates the fading motion trail behind the ball.

In short: **Record the ball’s movement history.**

---

# 🎨 Request a Redraw

```vb
Invalidate()
```

Tells WinForms:

> “The screen is out of date — please repaint the form.”

This triggers `OnPaint`, which draws:

- the trail  
- the ball  
- the FPS counter  

In short: **PhysicsTick updates the world; OnPaint draws it.**

---

```vb
End Sub
```

Marks the end of the physics update loop.

---


















































































---
---
---













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

























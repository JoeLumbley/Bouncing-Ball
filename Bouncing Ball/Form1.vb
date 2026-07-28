' Bouncing Ball with FPS counter
' 
' MIT License
' Copyright (c) 2026 Joseph W. Lumbley
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:

' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.

' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.

Imports System.Drawing.Drawing2D

Public Class Form1

    ' -------------------------------
    '  Engine State
    ' -------------------------------
    Private ballX As Double
    Private ballY As Double
    Private ballDiameter As Integer = 80

    Private velX As Double
    Private velY As Double
    Private speed As Double = 450

    Private physicsTimer As New Timer()
    Private sw As New Stopwatch()

    ' -------------------------------
    '  FPS Tracking
    ' -------------------------------
    Private frameCount As Integer = 0
    Private fps As Integer = 0
    Private fpsTimer As New Stopwatch()

    ' -------------------------------
    '  GDI Resources
    ' -------------------------------
    Private ballBrush As SolidBrush
    Private fpsBrush As SolidBrush
    Private fpsFont As Font


    ' Trail history (stores last N positions)
    Private trail As New List(Of PointF)
    Private trailLength As Integer = 25   ' Number of trail segments

    Public Sub New()
        InitializeComponent()

        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or
                    ControlStyles.UserPaint Or
                    ControlStyles.OptimizedDoubleBuffer, True)

        Me.DoubleBuffered = True
        Me.BackColor = Color.Black

        ' Center ball
        ballX = (ClientSize.Width - ballDiameter) / 2
        ballY = (ClientSize.Height - ballDiameter) / 2

        ' Random direction
        Dim rnd As New Random()
        Dim angle As Double = rnd.NextDouble() * Math.PI * 2
        velX = Math.Cos(angle) * speed
        velY = Math.Sin(angle) * speed

        ' Physics at ~60 FPS
        physicsTimer.Interval = 15
        AddHandler physicsTimer.Tick, AddressOf PhysicsTick

        sw.Start()
        fpsTimer.Start()

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        ' Create GDI resources here (cleaner than field-level)
        ballBrush = New SolidBrush(Color.DeepSkyBlue)
        fpsBrush = New SolidBrush(Color.White)
        fpsFont = New Font("Segoe UI", 14, FontStyle.Bold)

        physicsTimer.Start()

    End Sub

    ' -------------------------------
    '  Physics Loop (Fixed Timestep)
    ' -------------------------------
    Private Sub PhysicsTick(sender As Object, e As EventArgs)

        Dim dt As Double = sw.Elapsed.TotalSeconds
        sw.Restart()

        ' Clamp dt to avoid physics explosions on lag spikes
        dt = Math.Min(dt, 0.05)

        ballX += velX * dt
        ballY += velY * dt

        ' Horizontal bounce
        If ballX <= 0 Then
            ballX = 0
            velX = Math.Abs(velX)
        ElseIf ballX >= ClientSize.Width - ballDiameter Then
            ballX = ClientSize.Width - ballDiameter
            velX = -Math.Abs(velX)
        End If

        ' Vertical bounce
        If ballY <= 0 Then
            ballY = 0
            velY = Math.Abs(velY)
        ElseIf ballY >= ClientSize.Height - ballDiameter Then
            ballY = ClientSize.Height - ballDiameter
            velY = -Math.Abs(velY)
        End If

        ' Record current position for trail
        trail.Add(New PointF(CSng(ballX), CSng(ballY)))

        ' Keep trail length fixed
        If trail.Count > trailLength Then
            trail.RemoveAt(0)
        End If

        Invalidate()

    End Sub

    ' -------------------------------
    '  Rendering
    ' -------------------------------
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.CompositingMode = CompositingMode.SourceOver
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality

        ' Draw fading trail
        For i As Integer = 0 To trail.Count - 1

            Dim t As Double = i / trailLength
            Dim alpha As Integer = CInt(32 * Math.Pow(t, 2))
            If alpha > 255 Then alpha = 255

            Using trailBrush As New SolidBrush(Color.FromArgb(alpha, 0, 191, 255))
                Dim p As PointF = trail(i)
                Dim size As Integer = ballDiameter - (trailLength - i) * 2
                If size < 10 Then size = 10

                e.Graphics.FillEllipse(trailBrush,
                           CSng(p.X + (ballDiameter - size) / 2),
                           CSng(p.Y + (ballDiameter - size) / 2),
                           CSng(size),
                           CSng(size))
            End Using

        Next

        ' Ball
        e.Graphics.FillEllipse(ballBrush,
                               CSng(ballX),
                               CSng(ballY),
                               CSng(ballDiameter),
                               CSng(ballDiameter))

        UpdateFPS()
        e.Graphics.DrawString($"FPS: {fps}", fpsFont, fpsBrush, 10, 10)

    End Sub

    Protected Overrides Sub OnPaintBackground(pevent As PaintEventArgs)

        ' Suppress background flicker
        ' We paint everything manually

    End Sub

    ' -------------------------------
    '  FPS Counter
    ' -------------------------------
    Private Sub UpdateFPS()

        frameCount += 1

        If fpsTimer.ElapsedMilliseconds >= 1000 Then
            fps = frameCount
            frameCount = 0
            fpsTimer.Restart()
        End If

    End Sub

    ' -------------------------------
    '  Cleanup
    ' -------------------------------
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        ballBrush?.Dispose()
        fpsBrush?.Dispose()
        fpsFont?.Dispose()
        physicsTimer?.Dispose()

    End Sub

End Class

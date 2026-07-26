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

    Private ballX As Double
    Private ballY As Double
    Private ballDiameter As Integer = 80

    Private velX As Double
    Private velY As Double
    Private speed As Double = 450 ' pixels per second

    Private lastUpdate As DateTime = DateTime.Now

    Private frameCount As Integer = 0
    Private fps As Integer = 0
    Private lastFpsTime As DateTime = DateTime.Now

    Private ballBrush As New SolidBrush(Color.DeepSkyBlue)
    Private fpsBrush As New SolidBrush(Color.White)
    Private fpsFont As New Font("Segoe UI", 14, FontStyle.Bold)

    Private WithEvents GameTimer As New Timer()

    Public Sub New()
        InitializeComponent()

        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or
                    ControlStyles.UserPaint Or
                    ControlStyles.OptimizedDoubleBuffer, True)

        Me.BackColor = Color.Black

        ' Center ball
        ballX = (Me.ClientSize.Width - ballDiameter) / 2
        ballY = (Me.ClientSize.Height - ballDiameter) / 2

        ' Pick random direction
        Dim rnd As New Random()
        Dim angle As Double = rnd.NextDouble() * Math.PI * 2

        velX = Math.Cos(angle) * speed
        velY = Math.Sin(angle) * speed

        GameTimer.Interval = 1
        GameTimer.Start()
    End Sub

    Private Sub GameTimer_Tick(sender As Object, e As EventArgs) Handles GameTimer.Tick
        Dim now As DateTime = DateTime.Now
        Dim dt As Double = (now - lastUpdate).TotalSeconds
        lastUpdate = now

        ' Move ball
        ballX += velX * dt
        ballY += velY * dt

        ' Bounce horizontally
        If ballX <= 0 Then
            ballX = 0
            velX = Math.Abs(velX)
        ElseIf ballX >= Me.ClientSize.Width - ballDiameter Then
            ballX = Me.ClientSize.Width - ballDiameter
            velX = -Math.Abs(velX)
        End If

        ' Bounce vertically
        If ballY <= 0 Then
            ballY = 0
            velY = Math.Abs(velY)
        ElseIf ballY >= Me.ClientSize.Height - ballDiameter Then
            ballY = Me.ClientSize.Height - ballDiameter
            velY = -Math.Abs(velY)
        End If

        Me.Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality

        ' Draw ball
        e.Graphics.FillEllipse(ballBrush, CInt(ballX), CInt(ballY), ballDiameter, ballDiameter)

        UpdateFPS()
        e.Graphics.DrawString($"FPS: {fps}", fpsFont, fpsBrush, 10, 10)
    End Sub

    Private Sub UpdateFPS()
        frameCount += 1

        Dim now As DateTime = DateTime.Now
        If (now - lastFpsTime).TotalSeconds >= 1 Then
            fps = frameCount
            frameCount = 0
            lastFpsTime = now
        End If
    End Sub

End Class

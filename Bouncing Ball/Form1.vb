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

Public Class Form1

    Private model As BallModel

    Private physics As PhysicsEngine
    Private trail As BallTrail
    Private fps As FpsCounter

    Private graphicsResources As GraphicsResources

    Private renderer As RenderingEngine

    Private loopClock As GameLoop

    Private physicsTimer As Timer


    Private windowManager As WindowManager


    Private fullscreenKeyDown As Boolean

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)

        MyBase.OnKeyDown(e)

        If e.KeyCode <> Keys.F11 AndAlso
       e.KeyCode <> Keys.F Then
            Return
        End If

        If fullscreenKeyDown Then
            Return
        End If

        fullscreenKeyDown = True

        windowManager.ToggleFullscreen()

    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)

        MyBase.OnKeyUp(e)

        If e.KeyCode = Keys.F11 OrElse
       e.KeyCode = Keys.F Then

            fullscreenKeyDown = False

        End If

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        MyBase.OnLoad(e)

        windowManager = New WindowManager(Me)
        windowManager.Initialize()

        InitializeSystems()


    End Sub

    Public Sub New()

        InitializeComponent()

        SetStyle(
            ControlStyles.AllPaintingInWmPaint Or
            ControlStyles.UserPaint Or
            ControlStyles.OptimizedDoubleBuffer,
            True)

        DoubleBuffered = True
        BackColor = Color.Black
        WindowState = FormWindowState.Maximized

    End Sub

    Private Sub InitializeSystems()

        model = New BallModel()

        'model.Position =
        '    New PointF(
        '        (ClientSize.Width - model.Diameter) / 2,
        '        (ClientSize.Height - model.Diameter) / 2)

        'Dim rnd As New Random()

        'Dim angle =
        '    rnd.NextDouble() * Math.PI * 2

        'model.Velocity =
        '    New PointF(
        '        CSng(Math.Cos(angle) * model.Speed),
        '        CSng(Math.Sin(angle) * model.Speed))



        Dim trailLength As Integer = 15

        trail = New BallTrail(trailLength)

        fps = New FpsCounter()

        physics = New PhysicsEngine(model)

        graphicsResources = New GraphicsResources()

        renderer =
            New RenderingEngine(
                New BallRenderer(graphicsResources),
                New BallTrailRenderer(trailLength),
                New FpsRenderer(graphicsResources))

        loopClock = New GameLoop()

        physicsTimer = New Timer With {
            .Interval = 15
        }

        AddHandler physicsTimer.Tick,
            AddressOf PhysicsTick

        loopClock?.Start()

        physicsTimer?.Start()


        physics.CenterBallAndLaunch(Me.Size)


    End Sub

    Private Sub PhysicsTick(
        sender As Object,
        e As EventArgs)

        Dim dt = loopClock?.GetDeltaTime()

        physics?.Update(dt, ClientSize)
        trail.Update(model.Position)


        Invalidate()

    End Sub

    Protected Overrides Sub OnPaint(
        e As PaintEventArgs)

        MyBase.OnPaint(e)

        fps.FrameRendered()

        renderer.Render(
            e.Graphics,
            model,
            trail,
            fps)

    End Sub

    Protected Overrides Sub OnPaintBackground(
        pevent As PaintEventArgs)

    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)

        MyBase.OnResize(e)

        If physics Is Nothing Then Return

        If WindowState <> FormWindowState.Minimized Then
            physics?.CenterBallAndLaunch(ClientSize)
            trail.Clear()
        End If

    End Sub

    ' ============================================================
    '  SYSTEM THEME CHANGE DETECTION
    '
    '  Windows sends WM_SETTINGCHANGE whenever a system setting
    '  changes. We use this notification to detect Light/Dark
    '  theme changes and update the title bar appearance without
    '  requiring the application to restart.
    ' ============================================================
    Protected Overrides Sub WndProc(ByRef m As Message)

        Const WM_SETTINGCHANGE As Integer = &H1A

        If m.Msg = WM_SETTINGCHANGE Then
            windowManager?.ApplySystemTheme()
        End If

        MyBase.WndProc(m)


    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)

        physicsTimer?.Stop()
        physicsTimer?.Dispose()

        graphicsResources?.Dispose()

        MyBase.OnFormClosing(e)

    End Sub

End Class
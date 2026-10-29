Public Class FpsRenderer

    Private ReadOnly resources As GraphicsResources

    Public Sub New(resources As GraphicsResources)

        Me.resources = resources

    End Sub

    Public Sub Draw(
        g As Graphics,
        fpsCounter As FpsCounter)

        g.DrawString(
            $"FPS: {fpsCounter.FPS}",
            resources.FpsFont,
            resources.FpsBrush,
            10,
            10)

    End Sub

End Class
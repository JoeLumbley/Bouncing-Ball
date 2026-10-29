Imports System.Drawing.Drawing2D

Public Class RenderingEngine

    Private ReadOnly ballRenderer As BallRenderer
    Private ReadOnly trailRenderer As BallTrailRenderer
    Private ReadOnly fpsRenderer As FpsRenderer

    Public Sub New(
        ballRenderer As BallRenderer,
        trailRenderer As BallTrailRenderer,
        fpsRenderer As FpsRenderer)

        Me.ballRenderer = ballRenderer
        Me.trailRenderer = trailRenderer
        Me.fpsRenderer = fpsRenderer

    End Sub

    Public Sub Render(
        g As Graphics,
        model As BallModel,
        trail As BallTrail,
        fps As FpsCounter)

        g.CompositingMode = CompositingMode.SourceOver
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.PixelOffsetMode = PixelOffsetMode.HighQuality
        g.InterpolationMode = InterpolationMode.HighQualityBicubic

        trailRenderer.Draw(g, trail, model.Diameter)
        ballRenderer.Draw(g, model)
        fpsRenderer.Draw(g, fps)

    End Sub

End Class



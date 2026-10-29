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


        ConfigureGraphics(g)


        DrawBackground(g)

        trailRenderer.Draw(g, trail, model.Diameter)
        ballRenderer.Draw(g, model)
        fpsRenderer.Draw(g, fps)

    End Sub

    Private Shared Sub ConfigureGraphics(
        g As Graphics)

        g.CompositingMode = CompositingMode.SourceOver
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        g.InterpolationMode = InterpolationMode.HighQualityBicubic
        g.CompositingQuality = CompositingQuality.GammaCorrected
        g.PixelOffsetMode = PixelOffsetMode.HighQuality

    End Sub


    Private Shared Sub DrawBackground(
        g As Graphics)

        Using backgroundBrush As New LinearGradientBrush(
            New RectangleF(g.VisibleClipBounds),
            Color.FromArgb(25, 25, 25),
            Color.Black,
            LinearGradientMode.Vertical)

            g.FillRectangle(
                backgroundBrush,
                g.VisibleClipBounds)

        End Using

    End Sub

End Class



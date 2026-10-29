'Imports System.Drawing.Drawing2D

'Public Class BallRenderer

'    Private ReadOnly resources As GraphicsResources

'    Public Sub New(resources As GraphicsResources)

'        Me.resources = resources

'    End Sub

'    'Public Sub Draw(g As Graphics, model As BallModel)

'    '    g.FillEllipse(
'    '        resources.BallBrush,
'    '        model.Position.X,
'    '        model.Position.Y,
'    '        model.Diameter,
'    '        model.Diameter)

'    'End Sub





'    Public Sub Draw(
'        g As Graphics,
'        model As BallModel)

'        ' Ball
'        g.FillEllipse(
'            resources.BallBrush,
'            model.Position.X,
'            model.Position.Y,
'            model.Diameter,
'            model.Diameter)

'        ' Shadow
'        Using shadowPath As New GraphicsPath()

'            shadowPath.AddEllipse(
'                model.Position.X + model.Diameter * 0.1F,
'                model.Position.Y + model.Diameter * 0.1F,
'                model.Diameter,
'                model.Diameter)

'            Using shadowBrush As New PathGradientBrush(shadowPath)

'                shadowBrush.CenterColor = Color.FromArgb(173, Color.Black)
'                shadowBrush.SurroundColors = {Color.FromArgb(0, Color.Black)}

'                g.FillPath(shadowBrush, shadowPath)

'            End Using

'        End Using

'        ' Highlight
'        Dim highlightSize As Single = model.Diameter * 0.24F
'        Dim highlightX As Single = model.Position.X + model.Diameter * 0.2F
'        Dim highlightY As Single = model.Position.Y + model.Diameter * 0.16F

'        Using highlightBrush As New SolidBrush(
'            Color.FromArgb(64, Color.White))

'            g.FillEllipse(
'                highlightBrush,
'                highlightX,
'                highlightY,
'                highlightSize,
'                highlightSize)

'        End Using

'    End Sub


'End Class


Imports System.Drawing.Drawing2D

Public Class BallRenderer

    Private ReadOnly resources As GraphicsResources

    Public Sub New(resources As GraphicsResources)

        Me.resources = resources

    End Sub

    Public Sub Draw(
        g As Graphics,
        model As BallModel)

        Dim x As Single = model.Position.X
        Dim y As Single = model.Position.Y
        Dim diameter As Single = model.Diameter


        ' Ball
        g.FillEllipse(
            resources.BallBrush,
            x,
            y,
            diameter,
            diameter)


        '    Using ballBrush As New LinearGradientBrush(
        'New RectangleF(x, y, diameter, diameter),
        'Color.DeepSkyBlue,
        'Color.Black,
        'LinearGradientMode.ForwardDiagonal)

        '        g.FillEllipse(
        '    ballBrush,
        '    x,
        '    y,
        '    diameter,
        '    diameter)

        '    End Using









        ' Shadow
        Using shadowPath As New GraphicsPath()

            shadowPath.AddEllipse(
                x + diameter * 0.1F,
                y + diameter * 0.1F,
                diameter,
                diameter)

            Using shadowBrush As New PathGradientBrush(shadowPath)

                shadowBrush.CenterColor =
                    Color.FromArgb(175, Color.Black)

                shadowBrush.SurroundColors =
                    {Color.Transparent}

                g.FillPath(
                    shadowBrush,
                    shadowPath)

            End Using

        End Using


        ' Highlight
        Dim highlightSize As Single =
            diameter * 0.24F

        g.FillEllipse(
            resources.HighlightBrush,
            x + diameter * 0.2F,
            y + diameter * 0.16F,
            highlightSize,
            highlightSize)

    End Sub

End Class
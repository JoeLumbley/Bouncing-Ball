Public Class BallTrailRenderer
    Implements IDisposable

    Private ReadOnly brushes As SolidBrush()

    Public Sub New(length As Integer)

        brushes = New SolidBrush(length - 1) {}

        For i As Integer = 0 To length - 1

            Dim alpha As Integer =
                CInt(32 * Math.Pow(i / CDbl(length), 2))

            brushes(i) =
                New SolidBrush(
                    Color.FromArgb(alpha, 0, 191, 255))

        Next

    End Sub

    Public Sub Draw(
        g As Graphics,
        trail As BallTrail,
        diameter As Integer)

        Dim count = trail.TrailPoints.Count

        For i As Integer = 0 To count - 1

            Dim p = trail.TrailPoints(i)

            Dim size As Integer =
                Math.Max(10, diameter - (count - i) * 2)

            Dim offset As Single =
                CSng((diameter - size) / 2)

            g.FillEllipse(
                brushes(i),
                p.X + offset,
                p.Y + offset,
                size,
                size)

        Next

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose

        For Each brush In brushes
            brush.Dispose()
        Next

    End Sub

End Class
Public Class BallTrail

    Private ReadOnly trailLength As Integer
    Private ReadOnly points As New List(Of PointF)

    Public Sub New(length As Integer)

        trailLength = length

    End Sub

    Public Sub Update(position As PointF)

        points.Add(position)

        If points.Count > trailLength Then
            points.RemoveAt(0)
        End If

    End Sub

    Public ReadOnly Property TrailPoints As IReadOnlyList(Of PointF)
        Get
            Return points
        End Get
    End Property


    Public Sub Clear()

        points.Clear()

    End Sub

End Class
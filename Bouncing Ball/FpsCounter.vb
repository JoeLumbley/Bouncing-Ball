Public Class FpsCounter

    Private frameCount As Integer
    Private framesPs As Integer

    Private ReadOnly stopwatch As New Stopwatch()

    Public Sub New()

        stopwatch.Start()

    End Sub

    Public Sub FrameRendered()

        frameCount += 1

        If stopwatch.ElapsedMilliseconds >= 1000 Then

            framesPs = frameCount
            frameCount = 0

            stopwatch.Restart()

        End If

    End Sub

    Public ReadOnly Property FPS As Integer
        Get
            Return framesPs
        End Get
    End Property

End Class
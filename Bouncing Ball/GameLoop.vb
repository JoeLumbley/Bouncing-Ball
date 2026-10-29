Public Class GameLoop

    Private ReadOnly stopwatch As New Stopwatch()

    Public Sub Start()

        stopwatch.Start()

    End Sub

    Public Function GetDeltaTime() As Double

        Dim dt = stopwatch.Elapsed.TotalSeconds

        stopwatch.Restart()

        Return Math.Min(dt, 0.05)

    End Function

End Class
Public Class GraphicsResources
    Implements IDisposable

    Public ReadOnly Property BallBrush As SolidBrush
    Public ReadOnly Property FpsBrush As SolidBrush
    Public ReadOnly Property FpsFont As Font

    Public Sub New()

        BallBrush = New SolidBrush(Color.DeepSkyBlue)
        FpsBrush = New SolidBrush(Color.White)
        FpsFont = New Font("Segoe UI", 14, FontStyle.Bold)

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose

        BallBrush.Dispose()
        FpsBrush.Dispose()
        FpsFont.Dispose()

    End Sub

End Class
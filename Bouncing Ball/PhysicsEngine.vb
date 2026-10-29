Public Class PhysicsEngine

    Private ReadOnly model As BallModel


    Private ReadOnly rng As New Random()


    Public Sub New(model As BallModel)

        Me.model = model

    End Sub

    Public Sub Update(dt As Double, clientSize As Size)

        model.Position = New PointF(
            CSng(model.Position.X + model.Velocity.X * dt),
            CSng(model.Position.Y + model.Velocity.Y * dt))

        HandleCollisions(clientSize)

    End Sub

    Private Sub HandleCollisions(clientSize As Size)

        If model.Position.X <= 0 Then

            model.Position = New PointF(
                0,
                model.Position.Y)

            model.Velocity = New PointF(
                Math.Abs(model.Velocity.X),
                model.Velocity.Y)

        ElseIf model.Position.X >= clientSize.Width - model.Diameter Then

            model.Position = New PointF(
                clientSize.Width - model.Diameter,
                model.Position.Y)

            model.Velocity = New PointF(
                -Math.Abs(model.Velocity.X),
                model.Velocity.Y)

        End If

        If model.Position.Y <= 0 Then

            model.Position = New PointF(
                model.Position.X,
                0)

            model.Velocity = New PointF(
                model.Velocity.X,
                Math.Abs(model.Velocity.Y))

        ElseIf model.Position.Y >= clientSize.Height - model.Diameter Then

            model.Position = New PointF(
                model.Position.X,
                clientSize.Height - model.Diameter)

            model.Velocity = New PointF(
                model.Velocity.X,
                -Math.Abs(model.Velocity.Y))

        End If

    End Sub




    Public Sub CenterBallAndLaunch(clientSize As Size)

        model.Position = New PointF(
            (clientSize.Width - model.Diameter) / 2.0F,
            (clientSize.Height - model.Diameter) / 2.0F)

        Dim angle As Double = rng.NextDouble() * Math.PI * 2

        model.Velocity = New PointF(
            CSng(Math.Cos(angle) * model.Speed),
            CSng(Math.Sin(angle) * model.Speed))

    End Sub

End Class





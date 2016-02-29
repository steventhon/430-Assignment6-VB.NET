Imports System


Public Class OWQQ3
    Private MustInherit Class ExprC
        Public MustOverride Function interp() As Integer

    End Class

    Private Class numC : Inherits ExprC
        Private n As Integer

        Public Sub New(ByVal newN As Integer)
            MyBase.New()
            n = newN
        End Sub

        Public Overrides Function interp() As Integer
            Return n
        End Function

    End Class

    Private Class boolC : Inherits ExprC
        Private b As Boolean

        Public Sub New(ByVal newB As Boolean)
            MyBase.New()
            b = newB
        End Sub

        Public Overrides Function interp() As Integer
            Return b
        End Function
    End Class

    Private Class binopC : Inherits ExprC
        Private s As Char
        Private lhs As ExprC
        Private rhs As ExprC

        Public Sub New(ByVal newS As Char, ByVal newL As ExprC, ByVal newR As ExprC)
            MyBase.New()
            s = newS
            lhs = newL
            rhs = newR
        End Sub

        Public Overrides Function interp() As Integer
            If s = "+"c Then
                Return lhs.interp() + rhs.interp()
            ElseIf s = "-"c Then
                Return lhs.interp() - rhs.interp()
            ElseIf s = "*"c Then
                Return lhs.interp() * rhs.interp()
            ElseIf s = "/"c Then
                Return lhs.interp() / rhs.interp()
            Else
                Throw New Exception("unknown operation")
            End If
        End Function
    End Class

    Public Shared Sub Main()
        Dim testExpr As binopC
        Dim testExpr2 As binopC
        testExpr = New binopC("+", New numC(3), New numC(4))
        testExpr2 = New binopC("*", New numC(4),
                               New binopC("-", New numC(3),
                                          New binopC("*", New numC(2),
                                                     New numC(7))))

        Console.WriteLine("3 + 4: " & testExpr.interp())
        Console.WriteLine("4 * (3 - (2 * 7)): " & testExpr2.interp())

        Console.ReadKey()
    End Sub

End Class



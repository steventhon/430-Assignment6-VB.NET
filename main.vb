Imports System

Public Class OWQQ3
    Private MustInherit Class ExprC
        Public MustOverride Function interp() As Value
    End Class

    Private MustInherit Class Value
        Public MustOverride Function getVal()
    End Class

    Private Class numV : Inherits Value
        Public n As Integer

        Public Sub New(ByVal newN As Integer)
            MyBase.New()
            n = newN
        End Sub

        Public Overrides Function getVal()
            Return n
        End Function
    End Class

    Private Class boolV : Inherits Value
        Public b As Boolean

        Public Sub New(ByVal newB As Boolean)
            MyBase.New()
            b = newB
        End Sub

        Public Overrides Function getVal()
            Return b
        End Function
    End Class


    Private Class numC : Inherits ExprC
        Private n As Integer

        Public Sub New(ByVal newN As Integer)
            MyBase.New()
            n = newN
        End Sub

        Public Overrides Function interp() As Value
            Return New numV(n)
        End Function

    End Class

    Private Class boolC : Inherits ExprC
        Private b As Boolean

        Public Sub New(ByVal newB As Boolean)
            MyBase.New()
            b = newB
        End Sub

        Public Overrides Function interp() As Value
            Return New boolV(b)
        End Function
    End Class

    Private Class ifC : Inherits ExprC
        Private test As ExprC
        Private trueExpr As ExprC
        Private falseExpr As ExprC

        Public Sub New(ByVal newTest As ExprC, ByVal t As ExprC, ByVal f As ExprC)
            MyBase.New()
            test = newTest
            trueExpr = t
            falseExpr = f
        End Sub

        Public Overrides Function interp() As Value
            Dim testval As Value

            testval = test.interp()

            If testval.GetType().Equals(GetType(boolV)) Then
                If testval.getVal() Then
                    Return trueExpr.interp()
                Else
                    Return falseExpr.interp()
                End If
            Else
                Throw New Exception("non-boolean value in if statement")
            End If

        End Function

    End Class

    Private Class binopC : Inherits ExprC
        Private s As String
        Private lhs As ExprC
        Private rhs As ExprC

        Public Sub New(ByVal newS As String, ByVal newL As ExprC, ByVal newR As ExprC)
            MyBase.New()
            s = newS
            lhs = newL
            rhs = newR
        End Sub

        Public Overrides Function interp() As Value
            Dim lval As Value
            Dim rval As Value

            lval = lhs.interp()
            rval = rhs.interp()

            If lval.GetType().Equals(GetType(numV)) _
                   And rval.GetType().Equals(GetType(numV)) Then
                If s = "+" Then
                    Return New numV(lval.getVal() + rval.getVal())
                ElseIf s = "-" Then
                    Return New numV(lval.getVal() - rval.getVal())
                ElseIf s = "*" Then
                    Return New numV(lval.getVal() * rval.getVal())
                ElseIf s = "/" Then
                    Return New numV(lval.getVal() / rval.getVal())
                ElseIf s.Equals("eq?") Then
                    Return New boolV(lval.getVal() = rval.getVal())
                ElseIf s.Equals("<=") Then
                    Return New boolV(lval.getVal() <= rval.getVal())
                Else
                    Throw New Exception("unknown operation")
                End If
            ElseIf s = "eq?" Then
                If lval.GetType().Equals(GetType(boolV)) _
                        And rval.GetType().Equals(GetType(boolV)) Then
                    Return New boolV(lval.getVal() = rval.getVal())
                Else
                    Return New boolV(False)
                End If
            Else
                Throw New Exception("conflicting value types")
            End If

        End Function
    End Class

    Public Shared Sub Main()
        Dim testExpr As ExprC
        Dim testExpr2 As ExprC
        Dim testExpr3 As ExprC
        Dim testExpr4 As ExprC
        Dim testExpr5 As ExprC
        Dim testExpr6 As ExprC
        testExpr = New binopC("+", New numC(3), New numC(4))
        testExpr2 = New binopC("*", New numC(4),
                                   New binopC("-", New numC(3),
                                              New binopC("*", New numC(2),
                                                         New numC(7))))
        testExpr3 = New binopC("eq?", New binopC("*", New numC(10),
                                                     New numC(20)),
                                   New binopC("-", New numC(200), New numC(0)))
        testExpr4 = New binopC("<=", New binopC("+", New numC(0), New numC(0)),
                                   New binopC("-", New numC(50), New numC(300)))
        testExpr5 = New ifC(New binopC("<=", New numC(9), New numC(100)),
                            New binopC("*", New numC(100), New numC(2)),
                            New boolC(False))
        testExpr6 = New ifC(New boolC(False), New boolC(True), New numC(-1))

        Console.WriteLine("(+ 3 4): " & testExpr.interp().getVal())
        Console.WriteLine("(* 4 (- 3 (* 2 7))): " _
                              & testExpr2.interp().getVal())
        Console.WriteLine("(eq? (* 10 20) (- 200 0)): " _
                              & testExpr3.interp().getVal())
        Console.WriteLine("(<= (+ 0 0) (-50 300): " _
                              & testExpr4.interp().getVal())
        Console.WriteLine("(if (<= 9 100) (* 200 2) false): " _
                          & testExpr5.interp().getVal())
        Console.WriteLine("(if false true -1): " & testExpr6.interp().getVal())

        Console.ReadKey()
    End Sub
End Class


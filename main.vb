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
    
    Private Class TopEval
    
        Public Sub New()
            MyBase.New()
        End Sub
        
        Public Function serialize(ByVal exp As ExprC) As String
            Return exp.interp().getVal().toString()
        End Function  
    
    End Class
    
    Private Class Parse
        
        Public Sub New()
            MyBase.New()
        End Sub
        
        Public Function parse(ByVal sexp As String) As ExprC
            Dim binops As String() = {"+", "-", "*", "/", "eq?", "<="}
            Dim subSexp As String = sexp
            If sexp.chars(0) = "(" And sexp.chars(sexp.Length - 1) = ")" Then
            	subSexp = sexp.Substring(1, sexp.Length - 2)
            End If
            Dim tokens As String() = subSexp.Split(new Char() {" "c})
	    Dim tempExpr as ExprC = New numC(0)
            Dim binop As String = tokens(0)
            Dim flag As Boolean
            
            If IsNumeric(subSexp) Then
            	tempExpr = New numC(Convert.toInt32(subSexp))
            ElseIf Boolean.TryParse(subSexp, flag) Then
            	tempExpr = New boolC(flag)
            If Array.IndexOf(binops, binop) > -1 Then
                
                Dim left = Convert.toInt32(tokens(1))
                Dim right = Convert.toInt32(tokens(2))
                
                tempExpr = New binopC(binop, New numC(left), New numC(right))
                
                Return tempExpr
                
            End If
	    Return tempExpr
        End Function
        
    End Class

    Public Shared Sub Main()
        Dim testExpr As ExprC
        Dim testExpr2 As ExprC
        Dim testExpr3 As ExprC
        Dim testExpr4 As ExprC
        Dim testExpr5 As ExprC
        Dim testExpr6 As ExprC
        Dim testExpr7 As ExprC
        Dim testExpr8 As ExprC
        Dim testExpr9 As ExprC
        Dim topEval = New TopEval
        Dim parse = New Parse
        Dim testString = "(+ 3 3)"
        
        'Parse and Interp and Serialize
        Console.WriteLine(testString & ": " & topEval.serialize(parse.parse(testString)))
        Console.WriteLine("true: " & topEval.serialize(parse.parse("true")))
        Console.WriteLine("3: " & topEval.serialize(parse.parse("3")))
        Console.WriteLine("(5): " & topEval.serialize(parse.parse("(5)")))
        Console.WriteLine("(* 3 3): " & topEval.serialize(parse.parse("(* 3 3)")))
        Console.WriteLine("(eq? 1 2): " & topEval.serialize(parse.parse("(eq? 1 2)")))
        Console.WriteLine("(<= 1 2): " & topEval.serialize(parse.parse("(<= 1 2)")))
        
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

        
        Console.WriteLine("(+ 3 4): " & topEval.serialize(testExpr))
        Console.WriteLine("(* 4 (- 3 (* 2 7))): " & topEval.serialize(testExpr2))
        Console.WriteLine("(eq? (* 10 20) (- 200 0)): " & topEval.serialize(testExpr3))
        Console.WriteLine("(<= (+ 0 0) (-50 300): " & topEval.serialize(testExpr4))
        Console.WriteLine("(if (<= 9 100) (* 200 2) false): " & topEval.serialize(testExpr5))
        Console.WriteLine("(if false true -1): " & topEval.serialize(testExpr6))
        
        Try
            Dim result As String = topEval.serialize(testExpr7)
        Catch ex As Exception
            Console.WriteLine("(+ -1 false): " & ex.Message)
        End Try

        Try
            Dim result As String = topEval.serialize(testExpr8)
        Catch ex As Exception
            Console.WriteLine("(% 3 4): " & ex.Message)
        End Try

        Try
            Dim result As String = topEval.serialize(testExpr9)
        Catch ex As Exception
            Console.WriteLine("(if 1 2 3): " & ex.Message)
        End Try
        
        Console.ReadKey()
    End Sub
End Class

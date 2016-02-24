Imports System

Public Class OWQQ3
  Private Class ExprC
    Private name as String
    
    Public Sub New(byval newName as String)
      name = newname
    End Sub
    
  End Class
  
  Private Class numC : inherits ExprC
    Public n as Integer
    
    Public Sub New(byval newN as Integer)
      MyBase.new("numC")
      n = newN
    End Sub
  End Class
  
  Private Class boolC : inherits ExprC
    Public b as Boolean
    
    Public Sub New(byval newB as Boolean)
      MyBase.new("boolC")
      b = newB
    End Sub
  End Class
  
  Private Class binopC : inherits ExprC
    Public s as Char
    Public lhs as ExprC
    Public rhs as ExprC
    
    Public Sub New(byval newS as Char, byval newL as ExprC, byval newR as ExprC)
      MyBase.new("binopC")
      s = newS
      lhs = newL
      rhs = newR
    End Sub
  End Class

  Public Shared Sub Main()
    Console.WriteLine("OWQQ3")
    Console.ReadKey()
  End Sub

End Class

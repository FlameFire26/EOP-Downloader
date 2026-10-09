Imports System.Net

Public Enum ScoreKind
    Staff = 0
    Numbered = 1
End Enum

Public Class ScoreSheet
    Public Property Kind As ScoreKind
    Public Property Title As String = String.Empty
    Public Property PreviewUrl As Uri
    Public Property PageUrls As New List(Of Uri)()
End Class

Public Class ScoreInfo
    Public Property Title As String = String.Empty
    Public Property SourceUrl As Uri
    Public Property Sheets As New List(Of ScoreSheet)()
End Class

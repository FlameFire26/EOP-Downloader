Imports System.Net
Imports System.Net.Http
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports HtmlAgilityPack

Public Class EveryonePianoClient
    Implements IDisposable

    Private ReadOnly _http As HttpClient

    Public Sub New()
        Dim handler = New HttpClientHandler With {
            .AutomaticDecompression = DecompressionMethods.All
        }
        _http = New HttpClient(handler) With {
            .Timeout = TimeSpan.FromSeconds(30)
        }
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("EOPDownloader/1.0 (Windows; .NET)")
    End Sub

    Public Async Function GetScoreInfoAsync(sourceUrl As Uri, cancellationToken As Threading.CancellationToken) As Task(Of ScoreInfo)
        If sourceUrl Is Nothing OrElse Not sourceUrl.Scheme.StartsWith("http", StringComparison.OrdinalIgnoreCase) Then
            Throw New ArgumentException("请输入有效的 HTTP/HTTPS 曲谱地址。", NameOf(sourceUrl))
        End If

        _http.DefaultRequestHeaders.Referrer = sourceUrl
        Dim html = Await _http.GetStringAsync(sourceUrl, cancellationToken)
        Dim doc = New HtmlDocument()
        doc.LoadHtml(html)

        Dim titleNode = doc.DocumentNode.SelectSingleNode("//div[@id='PianoHead']//h1")
        If titleNode Is Nothing Then titleNode = doc.DocumentNode.SelectSingleNode("//title")
        Dim title = HtmlEntity.DeEntitize(If(titleNode?.InnerText, "人人钢琴曲谱")).Trim()

        Dim result = New ScoreInfo With {.Title = title, .SourceUrl = sourceUrl}
        Dim iframeNodes = doc.DocumentNode.SelectNodes("//iframe[@dataSrc or @datasrc]")
        If iframeNodes IsNot Nothing Then
            For Each iframe In iframeNodes
                Dim dataSrc = iframe.GetAttributeValue("dataSrc", iframe.GetAttributeValue("datasrc", String.Empty))
                Dim kind As ScoreKind
                If dataSrc.StartsWith("S-", StringComparison.OrdinalIgnoreCase) Then
                    kind = ScoreKind.Staff
                ElseIf dataSrc.StartsWith("N-", StringComparison.OrdinalIgnoreCase) Then
                    kind = ScoreKind.Numbered
                Else
                    Continue For
                End If

                Dim detailUrl = New Uri(sourceUrl, dataSrc)
                Dim sheet = Await ReadSheetAsync(detailUrl, title, kind, cancellationToken)
                If sheet.PageUrls.Count > 0 Then result.Sheets.Add(sheet)
            Next
        End If

        ' Some pages omit the iframe list; the first preview image still gives us a useful fallback.
        If result.Sheets.Count = 0 Then
            Dim image = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'MusicDiv')]//img[@src]")
            If image IsNot Nothing Then
                Dim imageUrl = New Uri(sourceUrl, image.GetAttributeValue("src", String.Empty))
                result.Sheets.Add(New ScoreSheet With {
                    .Kind = ScoreKind.Staff,
                    .Title = title,
                    .PreviewUrl = imageUrl,
                    .PageUrls = New List(Of Uri) From {imageUrl}
                })
            End If
        End If

        Return result
    End Function

    Private Async Function ReadSheetAsync(detailUrl As Uri, title As String, kind As ScoreKind, cancellationToken As Threading.CancellationToken) As Task(Of ScoreSheet)
        Dim html = Await _http.GetStringAsync(detailUrl, cancellationToken)
        Dim doc = New HtmlDocument()
        doc.LoadHtml(html)
        Dim sheet = New ScoreSheet With {.Kind = kind, .Title = title}
        Dim nodes = doc.DocumentNode.SelectNodes("//ul[@id='ul1']//img[@src]")
        If nodes Is Nothing Then Return sheet
        For Each node In nodes
            Dim value = node.GetAttributeValue("src", String.Empty)
            If String.IsNullOrWhiteSpace(value) Then Continue For
            Dim pageUrl = New Uri(detailUrl, value)
            If sheet.PageUrls.Count = 0 Then sheet.PreviewUrl = pageUrl
            sheet.PageUrls.Add(pageUrl)
        Next
        Return sheet
    End Function

    Public Async Function DownloadFileAsync(fileUrl As Uri, targetPath As String, cancellationToken As Threading.CancellationToken) As Task
        Dim data = Await _http.GetByteArrayAsync(fileUrl, cancellationToken)
        If data.Length = 0 Then Throw New InvalidDataException("服务器返回了空文件。")
        Await File.WriteAllBytesAsync(targetPath, data, cancellationToken)
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        _http.Dispose()
    End Sub
End Class

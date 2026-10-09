Imports System.IO
Imports System.Linq
Imports System.Threading

Public Class DownloaderForm
    Private _cancelSource As CancellationTokenSource

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub DownloaderForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler BrowseButton.Click, AddressOf BrowseButton_Click
        AddHandler StartButton.Click, AddressOf StartButton_Click
        AddHandler CancelTaskButton.Click, AddressOf CancelButton_Click
    End Sub

    Private Sub BrowseButton_Click(sender As Object, e As EventArgs)
        If Directory.Exists(OutputTextBox.Text) Then folderDialog.SelectedPath = OutputTextBox.Text
        If folderDialog.ShowDialog(Me) = DialogResult.OK Then OutputTextBox.Text = folderDialog.SelectedPath
    End Sub

    Private Async Sub StartButton_Click(sender As Object, e As EventArgs)
        Dim source As Uri = Nothing
        If Not Uri.TryCreate(UrlTextBox.Text.Trim(), UriKind.Absolute, source) OrElse Not source.Scheme.StartsWith("http", StringComparison.OrdinalIgnoreCase) Then
            MessageBox.Show(Me, "请输入有效的曲谱网址。", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Try
            Directory.CreateDirectory(OutputTextBox.Text)
        Catch ex As Exception
            MessageBox.Show(Me, "无法创建输出目录：" & ex.Message, "目录错误", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        SetRunning(True)
        _cancelSource = New CancellationTokenSource()
        ProgressBar1.Value = 0
        LogTextBox.Clear()
        Try
            Using client = New EveryonePianoClient()
                Log("正在读取曲谱页面...")
                Dim info = Await client.GetScoreInfoAsync(source, _cancelSource.Token)
                Log($"标题：{info.Title}")
                Dim selected = info.Sheets.Where(Function(s) KindComboBox.SelectedIndex = 2 OrElse CInt(s.Kind) = KindComboBox.SelectedIndex).ToList()
                If selected.Count = 0 Then Throw New InvalidDataException("页面中没有找到所选类型的曲谱图片。")
                Dim total = selected.Sum(Function(s) s.PageUrls.Count)
                Dim done = 0
                For Each sheet In selected
                    Dim kindName = If(sheet.Kind = ScoreKind.Staff, "五线谱", "简谱")
                    Dim safeTitle = MakeSafeFileName(info.Title)
                    Dim imageDir = Path.Combine(OutputTextBox.Text, safeTitle & "-" & kindName)
                    Directory.CreateDirectory(imageDir)
                    Dim localFiles As New List(Of String)()
                    For i = 0 To sheet.PageUrls.Count - 1
                        _cancelSource.Token.ThrowIfCancellationRequested()
                        Dim localPath As String = IO.Path.Combine(imageDir, $"{i + 1:000}.png")
                        If Not File.Exists(localPath) OrElse New FileInfo(localPath).Length = 0 Then Await client.DownloadFileAsync(sheet.PageUrls(i), localPath, _cancelSource.Token)
                        localFiles.Add(localPath)
                        done += 1
                        ProgressBar1.Value = Math.Min(100, CInt(done * 100.0 / total))
                        StatusLabel.Text = $"{kindName}：{done}/{total}"
                    Next
                    Dim pdfPath = Path.Combine(OutputTextBox.Text, safeTitle & "-" & kindName & ".pdf")
                    PdfBuilder.CreatePdf(localFiles, pdfPath)
                    Log($"已生成：{pdfPath}")
                Next
                ProgressBar1.Value = 100
                StatusLabel.Text = "完成"
                Log("全部任务已完成。")
            End Using
        Catch ex As OperationCanceledException
            StatusLabel.Text = "已取消"
            Log("任务已取消。")
        Catch ex As Exception
            StatusLabel.Text = "失败"
            Log("错误：" & ex.Message)
            MessageBox.Show(Me, ex.Message, "下载失败", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _cancelSource?.Dispose()
            _cancelSource = Nothing
            SetRunning(False)
        End Try
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs)
        _cancelSource?.Cancel()
    End Sub

    Private Sub SetRunning(running As Boolean)
        UrlTextBox.Enabled = Not running
        KindComboBox.Enabled = Not running
        OutputTextBox.Enabled = Not running
        BrowseButton.Enabled = Not running
        StartButton.Enabled = Not running
        CancelTaskButton.Enabled = running
    End Sub

    Private Sub Log(message As String)
        LogTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}")
    End Sub

    Private Shared Function MakeSafeFileName(value As String) As String
        Dim result = value
        For Each c In Path.GetInvalidFileNameChars()
            result = result.Replace(c, "_"c)
        Next
        Return If(String.IsNullOrWhiteSpace(result), "score", result.Trim())
    End Function
End Class

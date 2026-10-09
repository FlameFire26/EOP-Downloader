<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DownloaderForm
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer
    Friend WithEvents UrlLabel As Label
    Friend WithEvents UrlTextBox As TextBox
    Friend WithEvents KindLabel As Label
    Friend WithEvents KindComboBox As ComboBox
    Friend WithEvents OutputLabel As Label
    Friend WithEvents OutputTextBox As TextBox
    Friend WithEvents BrowseButton As Button
    Friend WithEvents StartButton As Button
    Friend WithEvents CancelTaskButton As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents StatusLabel As Label
    Friend WithEvents LogTextBox As TextBox
    Private folderDialog As FolderBrowserDialog

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then components.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        UrlLabel = New Label()
        UrlTextBox = New TextBox()
        KindLabel = New Label()
        KindComboBox = New ComboBox()
        OutputLabel = New Label()
        OutputTextBox = New TextBox()
        BrowseButton = New Button()
        StartButton = New Button()
        CancelTaskButton = New Button()
        ProgressBar1 = New ProgressBar()
        StatusLabel = New Label()
        LogTextBox = New TextBox()
        folderDialog = New FolderBrowserDialog()
        ProgressLabel = New Label()
        SuspendLayout()
        ' 
        ' UrlLabel
        ' 
        UrlLabel.AutoSize = True
        UrlLabel.Location = New Point(18, 20)
        UrlLabel.Name = "UrlLabel"
        UrlLabel.Size = New Size(82, 24)
        UrlLabel.TabIndex = 0
        UrlLabel.Text = "曲谱网址"
        ' 
        ' UrlTextBox
        ' 
        UrlTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        UrlTextBox.Location = New Point(106, 16)
        UrlTextBox.Name = "UrlTextBox"
        UrlTextBox.Size = New Size(815, 30)
        UrlTextBox.TabIndex = 1
        UrlTextBox.Text = "https://everyonepiano.cn/Piano-14337.html#"
        ' 
        ' KindLabel
        ' 
        KindLabel.AutoSize = True
        KindLabel.Location = New Point(18, 57)
        KindLabel.Name = "KindLabel"
        KindLabel.Size = New Size(82, 24)
        KindLabel.TabIndex = 2
        KindLabel.Text = "曲谱类型"
        ' 
        ' KindComboBox
        ' 
        KindComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        KindComboBox.Items.AddRange(New Object() {"全部", "五线谱", "简谱"})
        KindComboBox.Location = New Point(106, 52)
        KindComboBox.Name = "KindComboBox"
        KindComboBox.Size = New Size(120, 32)
        KindComboBox.TabIndex = 3
        ' 
        ' OutputLabel
        ' 
        OutputLabel.AutoSize = True
        OutputLabel.Location = New Point(18, 94)
        OutputLabel.Name = "OutputLabel"
        OutputLabel.Size = New Size(82, 24)
        OutputLabel.TabIndex = 4
        OutputLabel.Text = "输出目录"
        ' 
        ' OutputTextBox
        ' 
        OutputTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        OutputTextBox.Location = New Point(106, 90)
        OutputTextBox.Name = "OutputTextBox"
        OutputTextBox.Size = New Size(709, 30)
        OutputTextBox.TabIndex = 5
        OutputTextBox.Text = "C:\Users\26748\Documents\EOP Downloads"
        ' 
        ' BrowseButton
        ' 
        BrowseButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        BrowseButton.Location = New Point(821, 89)
        BrowseButton.Name = "BrowseButton"
        BrowseButton.Size = New Size(100, 32)
        BrowseButton.TabIndex = 6
        BrowseButton.Text = "浏览..."
        ' 
        ' StartButton
        ' 
        StartButton.Location = New Point(695, 130)
        StartButton.Name = "StartButton"
        StartButton.Size = New Size(120, 32)
        StartButton.TabIndex = 7
        StartButton.Text = "开始下载"
        ' 
        ' CancelTaskButton
        ' 
        CancelTaskButton.Enabled = False
        CancelTaskButton.Location = New Point(821, 130)
        CancelTaskButton.Name = "CancelTaskButton"
        CancelTaskButton.Size = New Size(100, 32)
        CancelTaskButton.TabIndex = 8
        CancelTaskButton.Text = "取消"
        ' 
        ' ProgressBar1
        ' 
        ProgressBar1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ProgressBar1.Location = New Point(106, 132)
        ProgressBar1.Name = "ProgressBar1"
        ProgressBar1.Size = New Size(571, 30)
        ProgressBar1.TabIndex = 9
        ' 
        ' StatusLabel
        ' 
        StatusLabel.AutoSize = True
        StatusLabel.Location = New Point(18, 751)
        StatusLabel.Name = "StatusLabel"
        StatusLabel.Size = New Size(46, 24)
        StatusLabel.TabIndex = 10
        StatusLabel.Text = "就绪"
        ' 
        ' LogTextBox
        ' 
        LogTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        LogTextBox.Location = New Point(18, 179)
        LogTextBox.Multiline = True
        LogTextBox.Name = "LogTextBox"
        LogTextBox.ReadOnly = True
        LogTextBox.ScrollBars = ScrollBars.Vertical
        LogTextBox.Size = New Size(903, 559)
        LogTextBox.TabIndex = 11
        ' 
        ' ProgressLabel
        ' 
        ProgressLabel.AutoSize = True
        ProgressLabel.Location = New Point(54, 134)
        ProgressLabel.Name = "ProgressLabel"
        ProgressLabel.Size = New Size(46, 24)
        ProgressLabel.TabIndex = 12
        ProgressLabel.Text = "进度"
        ' 
        ' DownloaderForm
        ' 
        ClientSize = New Size(938, 784)
        Controls.Add(ProgressLabel)
        Controls.Add(UrlLabel)
        Controls.Add(UrlTextBox)
        Controls.Add(KindLabel)
        Controls.Add(KindComboBox)
        Controls.Add(OutputLabel)
        Controls.Add(OutputTextBox)
        Controls.Add(BrowseButton)
        Controls.Add(StartButton)
        Controls.Add(CancelTaskButton)
        Controls.Add(ProgressBar1)
        Controls.Add(StatusLabel)
        Controls.Add(LogTextBox)
        MinimumSize = New Size(650, 360)
        Name = "DownloaderForm"
        Text = "EOP 曲谱下载器"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents ProgressLabel As Label
End Class

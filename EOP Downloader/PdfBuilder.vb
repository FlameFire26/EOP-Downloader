Imports PdfSharpCore.Drawing
Imports PdfSharpCore.Pdf

Public Module PdfBuilder
    Public Sub CreatePdf(imagePaths As IEnumerable(Of String), outputPath As String)
        Dim files = imagePaths.ToList()
        If files.Count = 0 Then Throw New ArgumentException("没有可写入 PDF 的图片。", NameOf(imagePaths))

        Using document = New PdfDocument()
            For Each file In files
                Using image = XImage.FromFile(file)
                    Dim page = document.AddPage()
                    page.Width = XUnit.FromPoint(Math.Max(1.0, image.PixelWidth * 72.0 / Math.Max(1.0, image.HorizontalResolution)))
                    page.Height = XUnit.FromPoint(Math.Max(1.0, image.PixelHeight * 72.0 / Math.Max(1.0, image.VerticalResolution)))
                    Using gfx = XGraphics.FromPdfPage(page)
                        gfx.DrawImage(image, 0, 0, page.Width.Point, page.Height.Point)
                    End Using
                End Using
            Next
            document.Save(outputPath)
        End Using
    End Sub
End Module

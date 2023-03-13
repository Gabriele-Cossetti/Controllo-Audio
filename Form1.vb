Imports System.IO
Imports System.Media
Imports System.Xml

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.Cyan
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim XA = Val(TextBox1.Text)
        Dim XB = Val(TextBox2.Text)
        Dim XC = Val(TextBox3.Text)
        If XA < 0 Or XA > 1000 Then XA = 500
        FrequencyBeep(XA, XB, XC)       'Ampiezza, Frequenza, Durata 
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        End
    End Sub
    Public Shared Sub FrequencyBeep(ByVal Amplitude As Integer, ByVal Frequency As Integer, ByVal Duration As Integer)
        Dim A As Double = ((Amplitude * (System.Math.Pow(2, 15))) / 1000) - 1
        Dim DeltaFT As Double = 2 * Math.PI * Frequency / 44100.0
        Dim Samples As Integer = 441 * Duration / 10
        Dim Bytes As Integer = Samples * 4

        Dim Hdr As Integer() = {1179011410, 36 + Bytes, 1163280727, 544501094, 16, 131073, 44100, 176400, 1048580, 1635017060, Bytes}

        Using MS As MemoryStream = New MemoryStream(44 + Bytes)

            Using BW As BinaryWriter = New BinaryWriter(MS)

                For I As Integer = 0 To Hdr.Length - 1
                    BW.Write(Hdr(I))
                Next

                For T As Integer = 0 To Samples - 1
                    Dim Sample As Short = System.Convert.ToInt16(A * Math.Sin(DeltaFT * T))
                    BW.Write(Sample)
                    BW.Write(Sample)
                Next

                BW.Flush()
                MS.Seek(0, SeekOrigin.Begin)

                Using SP As SoundPlayer = New SoundPlayer(MS)
                    SP.PlaySync()
                End Using
            End Using
        End Using
    End Sub

End Class
'Riferimenti Bibliografici
'1) https://stackoverflow.com/questions/61578580/generate-sound-frequency-in-windows-vb-net
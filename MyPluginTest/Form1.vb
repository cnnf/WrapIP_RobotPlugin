Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

Public Class Form1
    Friend Shared MyInstance As Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MyInstance = Me
        If File.Exists(IniFilePath) Then
            Dim Sections = GetAllSections(IniFilePath)
            If Sections.Count = 0 Then Return
            For Each Section In Sections
                If Section = "WebAddress" Then
                    Dim KeysList = GetSectionKeyNames(IniFilePath, Section)
                    For Each Key In KeysList
                        If Key.ToString.StartsWith("url=") Then
                            TextBox1.Text = Key.ToString.Replace("url=", "")
                            Exit For
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim path = Environment.CurrentDirectory & "\main\data\config\WrapIP.ini"
        If Not System.IO.Directory.Exists(Environment.CurrentDirectory + "\main\data\config\") Then
            System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "\main\data\config\")
            WritePrivateProfileString("WebAddress", "url", TextBox1.Text + vbNewLine, IniFilePath)
        Else
            If Not File.Exists(IniFilePath) Then
                WritePrivateProfileString("WebAddress", "url", TextBox1.Text + vbNewLine, IniFilePath)
            Else
                Dim Sections = GetAllSections(IniFilePath)
                For Each Section In Sections
                    If Section = "WebAddress" Then
                        If TextBox1.Text.Substring(TextBox1.Text.Length - 1) <> "/" Then TextBox1.Text = TextBox1.Text + "/"
                        WritePrivateProfileString("WebAddress", "url", TextBox1.Text + vbNewLine, IniFilePath)
                        MessageBox.Show("设置成功.")
                        Exit For
                    End If
                Next
            End If
        End If
        url = TextBox1.Text
    End Sub
End Class
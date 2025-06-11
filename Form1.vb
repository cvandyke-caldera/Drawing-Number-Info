Imports System.CodeDom
Imports System.ComponentModel
Imports System.ComponentModel.Design.ObjectSelectorEditor
Imports System.ComponentModel.Design.Serialization
Imports System.Data.OleDb
Imports System.Drawing.Configuration
Imports System.IO
Imports System.Net.WebRequestMethods
Imports System.Reflection.Emit
Imports System.Reflection.Metadata
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices
Imports EPDM.Interop.epdm
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Logging
Imports VBScript_RegExp_55

Public Class Form1
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        ' EXTRACT EPDM VAULT FILES  ======================================================================
        Dim vault As IEdmVault5 = EPDM_Connect()
        Dim files = EPDM_Pull(vault, txtDrawingNo.Text)
        Dim descriptions(0) As String
        Dim i As Integer = 0
        Dim same As Boolean = True

        ' Iterate through the files found by the EPDM_Pull function, add them to the listview, and compare descriptions to ensure they are all equivalent
        lstEPDMFiles.Items.Clear()
        lstChildren.Items.Clear()
        lstEPDMFiles.SelectedItems.Clear()
        For Each file As IEdmFile5 In files
            Dim varEnum As IEdmEnumeratorVariable5 = file.GetEnumeratorVariable()
            Dim newitem As New ListViewItem(file.Name)
            Dim state As IEdmState5 = file.CurrentState
            newitem.SubItems.Add(state.Name)
            If file.IsLocked = True Then
                Dim user As IEdmUser5 = file.LockedByUser
                newitem.SubItems.Add(user.Name)
            Else
                newitem.SubItems.Add("Not Checked Out.")
            End If
            lstEPDMFiles.Items.Add(newitem)
            Dim val As String = ""
            varEnum.GetVar("Description", "@", val)
            ReDim Preserve descriptions(i)
            descriptions(i) = val
            i += 1
        Next

        If lstEPDMFiles.Items.Count = 0 Then
            MessageBox.Show("Unable to find a matching file in the EPDM Vault. Please check the part number entered.", "Unable to Find Part", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'Call ClearAll()
            Exit Sub
        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        lstEPDMFiles.Columns.Add("File Name", 150, HorizontalAlignment.Left)
        lstEPDMFiles.Columns.Add("Current State", 150, HorizontalAlignment.Left)
        lstEPDMFiles.Columns.Add("Checked Out By", 150, HorizontalAlignment.Left)
        lstChildren.Columns.Add("Drawing Number", 150, HorizontalAlignment.Left)
        lstChildren.Columns.Add("Current State", 150, HorizontalAlignment.Left)
        lstChildren.Columns.Add("Description", 400, HorizontalAlignment.Left)
    End Sub

    Private Sub lstEPDMFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstEPDMFiles.SelectedIndexChanged
        If lstEPDMFiles.SelectedItems.Count > 0 Then
            Dim selectedrow As ListViewItem = lstEPDMFiles.SelectedItems(0)
            If selectedrow.SubItems.Count > 1 Then
                If String.Equals(selectedrow.SubItems(1).Text.Trim(), "Production Release", StringComparison.OrdinalIgnoreCase) Then
                    btnFindPDF.Visible = True
                End If
            End If
        End If

    End Sub

    Private Sub lstEPDMFiles_Leave(sender As Object, e As EventArgs) Handles lstEPDMFiles.Leave
        'btnFindPDF.Visible = False
    End Sub

    Private Sub btnFindPDF_Click(sender As Object, e As EventArgs) Handles btnFindPDF.Click
        Dim filepath As String
        filepath = FindFolder(txtDrawingNo.Text)
        filepath = filepath & "\" & txtDrawingNo.Text & ".pdf"
        MessageBox.Show(filepath)
        If My.Computer.FileSystem.FileExists(filepath) Then
            MessageBox.Show("PDF Exists")
            'System.Diagnostics.Process.Start(filepath)
            Process.Start(New ProcessStartInfo(filepath) With {.UseShellExecute = True})
        Else
            MessageBox.Show("PDF Exists NOT")
        End If

    End Sub

    Public Function EPDM_Connect() As IEdmVault5

        ' Create the vault object and connect to the vault if not already
        Dim vault As IEdmVault5 = New EdmVault5
        vault.LoginAuto("EPDM_Vault", Me.Handle.ToInt32())
        Return vault

    End Function
    Public Function EPDM_Pull(vault As IEdmVault5, Partno As String) As List(Of IEdmFile5)
        Try
            ' Use the IEdmSearch5 interface to find files with the given part number in the vault
            Dim search As IEdmSearch5 = vault.CreateSearch()
            search.FileName = Partno & "%"
            search.Recursive = True
            Dim file1 As IEdmSearchResult5 = search.GetFirstResult()
            Dim sldext As String() = {".slddrw", ".sldasm", ".sldprt"}
            Dim files As New List(Of IEdmFile5)

            While file1 IsNot Nothing
                If sldext.Contains(Path.GetExtension(file1.Path).ToLower()) Then
                    Dim parentFolder As IEdmFolder5 = Nothing
                    Dim file2 As IEdmFile5 = vault.GetFileFromPath(file1.Path, parentFolder)
                    files.Add(file2)
                End If
                file1 = search.GetNextResult()
            End While

            Return files

        Catch ex As Exception
            MessageBox.Show("Unable to pull data from EPDM Vault.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try

    End Function
    Public Function FindFolder(DrawingNo As String) As String
        ' Check if the part number is old (before alphanumerical numbers were used)
        Dim AN_num As Boolean = False
        Dim Check As New Regex("^[a-zA-Z]")
        AN_num = Check.IsMatch(DrawingNo)

        ' Replace last three characters with corresponding folder characters
        Dim FolderName As String
        If AN_num = True Then
            FolderName = DrawingNo.Remove(2, 3)
            FolderName = FolderName & "000"
        Else
            FolderName = DrawingNo.Remove(2, DrawingNo.Length - 3)
            FolderName = FolderName & "xxx"
        End If

        Dim FolderPath As String
        FolderPath = "F:\Departments\Engineering\Production Drawings\" & FolderName
        Return FolderPath
    End Function

    Public Function FindChildren(Vault As IEdmVault5, Reference As IEdmReference10, FilePath As String, Parent As String, ProjectName As String, ByRef RefFilesDict As Dictionary(Of IEdmFile5, String)) As Boolean

        Dim currentReference As IEdmReference10

        If Reference Is Nothing Then
            Dim ParentFolder As IEdmFolder5 = Nothing
            Dim file As IEdmFile5 = Vault.GetFileFromPath(FilePath, ParentFolder)
            Dim Filename As String = RemoveSuffix(file.Name)

            'Add initial file to dictionary
            Parent = ""
            RefFilesDict.Add(file, Parent)
            currentReference = file.GetReferenceTree(ParentFolder.ID)
            FindChildren(Vault, currentReference, "", file.Name, ProjectName, RefFilesDict)
        Else
            'This is a recursive call, 'Reference' is a child reference
            currentReference = Reference
            Dim pos As IEdmPos5 = currentReference.GetFirstChildPosition4(ProjectName, False, True, False, EdmRefFlags.EdmRef_File, "", 0)
            Dim ref As IEdmReference10
            While Not pos.IsNull
                ref = currentReference.GetNextChild(pos)
                If ref.File IsNot Nothing Then
                    Dim Name As String = RemoveSuffix(ref.Name)
                    Dim reg1 As New Regex("^[a-zA-Z0-9]{6}$")
                    Dim reg2 As New Regex("^[a-zA-Z0-9]{6}\.[a-zA-Z0-9]{3}\.[a-zA-Z0-9]{3}$")
                    Dim currentfile As IEdmFile5 = ref.File
                    If reg1.IsMatch(Name) Then
                        RefFilesDict.TryAdd(currentfile, Parent)
                        'If Not RefFilesDict.ContainsKey(currentfile) Then
                        '    RefFilesDict.Add(currentfile, Level)
                        'End If
                    ElseIf reg2.IsMatch(Name) Then
                        RefFilesDict.TryAdd(currentfile, Parent)
                    End If
                    FindChildren(Vault, ref, "", currentfile.Name, ProjectName, RefFilesDict)

                End If
            End While

        End If
        Return True
    End Function

    Public Function RemoveSuffix(Name As String) As String
        Dim reg1 As New Regex(".SLDPRT")
        Dim reg2 As New Regex(".SLDASM")
        Name = reg1.Replace(Name, "")
        Name = reg2.Replace(Name, "")
        Return Name
    End Function

    Private Sub btnFindChildren_Click(sender As Object, e As EventArgs) Handles btnFindChildren.Click

        If lstEPDMFiles.SelectedItems.Count > 0 Then
            Dim vault As IEdmVault5 = EPDM_Connect()
            Dim selectedrow As ListViewItem = lstEPDMFiles.SelectedItems(0)
            Dim search As IEdmSearch5 = vault.CreateSearch()
            search.FileName = selectedrow.SubItems(0).Text.Trim()
            search.Recursive = True
            Dim result As IEdmSearchResult5 = search.GetFirstResult()
            Dim parentFolder As IEdmFolder5 = Nothing
            MessageBox.Show(result.Path)
            Dim file As IEdmFile5 = vault.GetFileFromPath(result.Path, parentFolder)
            Dim Reference As IEdmReference10 = Nothing
            Dim FilePath As String = file.GetLocalPath(parentFolder.ID)
            Dim ProjectName As String = "A"
            Dim RefFilesDict As New Dictionary(Of IEdmFile5, String)

            FindChildren(vault, Reference, FilePath, "", ProjectName, RefFilesDict)

            For Each kvp As KeyValuePair(Of IEdmFile5, String) In RefFilesDict
                Dim currentfile As IEdmFile5 = kvp.Key
                Dim currentlevel As String = kvp.Value
                Dim newitem As New ListViewItem(currentfile.Name)
                Dim state As IEdmState5 = currentfile.CurrentState
                newitem.SubItems.Add(state.Name)
                Dim varEnum As IEdmEnumeratorVariable5 = currentfile.GetEnumeratorVariable()
                Dim description As String = ""
                varEnum.GetVar("Description", "@", description)
                newitem.SubItems.Add(description)
                lstChildren.Items.Add(newitem)

            Next
        End If


    End Sub
End Class

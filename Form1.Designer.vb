<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        btnSearch = New Button()
        txtDrawingNo = New TextBox()
        lblDrawingNo = New Label()
        lstEPDMFiles = New ListView()
        btnFindPDF = New Button()
        lstChildren = New ListView()
        lblChildren = New Label()
        btnFindChildren = New Button()
        SuspendLayout()
        ' 
        ' btnSearch
        ' 
        btnSearch.Location = New Point(610, 23)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(140, 38)
        btnSearch.TabIndex = 0
        btnSearch.Text = "Search"
        btnSearch.UseVisualStyleBackColor = True
        ' 
        ' txtDrawingNo
        ' 
        txtDrawingNo.Location = New Point(228, 36)
        txtDrawingNo.Name = "txtDrawingNo"
        txtDrawingNo.Size = New Size(352, 23)
        txtDrawingNo.TabIndex = 1
        ' 
        ' lblDrawingNo
        ' 
        lblDrawingNo.AutoSize = True
        lblDrawingNo.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblDrawingNo.Location = New Point(55, 36)
        lblDrawingNo.Name = "lblDrawingNo"
        lblDrawingNo.Size = New Size(167, 25)
        lblDrawingNo.TabIndex = 2
        lblDrawingNo.Text = "Drawing Number"
        ' 
        ' lstEPDMFiles
        ' 
        lstEPDMFiles.FullRowSelect = True
        lstEPDMFiles.HeaderStyle = ColumnHeaderStyle.Nonclickable
        lstEPDMFiles.Location = New Point(55, 91)
        lstEPDMFiles.MultiSelect = False
        lstEPDMFiles.Name = "lstEPDMFiles"
        lstEPDMFiles.Size = New Size(450, 100)
        lstEPDMFiles.TabIndex = 3
        lstEPDMFiles.UseCompatibleStateImageBehavior = False
        lstEPDMFiles.View = View.Details
        ' 
        ' btnFindPDF
        ' 
        btnFindPDF.Location = New Point(570, 156)
        btnFindPDF.Name = "btnFindPDF"
        btnFindPDF.Size = New Size(180, 35)
        btnFindPDF.TabIndex = 4
        btnFindPDF.Text = "Open PDF Drawing"
        btnFindPDF.UseVisualStyleBackColor = True
        btnFindPDF.Visible = False
        ' 
        ' lstChildren
        ' 
        lstChildren.FullRowSelect = True
        lstChildren.Location = New Point(55, 243)
        lstChildren.MultiSelect = False
        lstChildren.Name = "lstChildren"
        lstChildren.Size = New Size(900, 227)
        lstChildren.TabIndex = 5
        lstChildren.UseCompatibleStateImageBehavior = False
        lstChildren.View = View.Details
        ' 
        ' lblChildren
        ' 
        lblChildren.AutoSize = True
        lblChildren.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblChildren.Location = New Point(55, 206)
        lblChildren.Name = "lblChildren"
        lblChildren.Size = New Size(87, 25)
        lblChildren.TabIndex = 6
        lblChildren.Text = "Children"
        ' 
        ' btnFindChildren
        ' 
        btnFindChildren.Location = New Point(570, 103)
        btnFindChildren.Name = "btnFindChildren"
        btnFindChildren.Size = New Size(180, 33)
        btnFindChildren.TabIndex = 7
        btnFindChildren.Text = "Find Children"
        btnFindChildren.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AcceptButton = btnSearch
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1012, 649)
        Controls.Add(btnFindChildren)
        Controls.Add(lblChildren)
        Controls.Add(lstChildren)
        Controls.Add(btnFindPDF)
        Controls.Add(lstEPDMFiles)
        Controls.Add(lblDrawingNo)
        Controls.Add(txtDrawingNo)
        Controls.Add(btnSearch)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnSearch As Button
    Friend WithEvents txtDrawingNo As TextBox
    Friend WithEvents lblDrawingNo As Label
    Friend WithEvents lstEPDMFiles As ListView
    Friend WithEvents btnFindPDF As Button
    Friend WithEvents lstChildren As ListView
    Friend WithEvents lblChildren As Label
    Friend WithEvents btnFindChildren As Button

End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class PortSettingsDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.gbxPort = New System.Windows.Forms.GroupBox
        Me.cmbPort = New System.Windows.Forms.ComboBox
        Me.gbxBitRate = New System.Windows.Forms.GroupBox
        Me.cmbBitRate = New System.Windows.Forms.ComboBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.gbxHandshaking = New System.Windows.Forms.GroupBox
        Me.cmbHandshaking = New System.Windows.Forms.ComboBox
        Me.chkOpenComPortOnStartup = New System.Windows.Forms.CheckBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.gbxPort.SuspendLayout()
        Me.gbxBitRate.SuspendLayout()
        Me.gbxHandshaking.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbxPort
        '
        Me.gbxPort.Controls.Add(Me.cmbPort)
        Me.gbxPort.Location = New System.Drawing.Point(15, 25)
        Me.gbxPort.Name = "gbxPort"
        Me.gbxPort.Size = New System.Drawing.Size(123, 65)
        Me.gbxPort.TabIndex = 0
        Me.gbxPort.TabStop = False
        Me.gbxPort.Text = "Port"
        '
        'cmbPort
        '
        Me.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPort.FormattingEnabled = True
        Me.cmbPort.Location = New System.Drawing.Point(10, 23)
        Me.cmbPort.Name = "cmbPort"
        Me.cmbPort.Size = New System.Drawing.Size(101, 21)
        Me.cmbPort.TabIndex = 0
        '
        'gbxBitRate
        '
        Me.gbxBitRate.Controls.Add(Me.cmbBitRate)
        Me.gbxBitRate.Location = New System.Drawing.Point(160, 25)
        Me.gbxBitRate.Name = "gbxBitRate"
        Me.gbxBitRate.Size = New System.Drawing.Size(124, 65)
        Me.gbxBitRate.TabIndex = 1
        Me.gbxBitRate.TabStop = False
        Me.gbxBitRate.Text = "Bit Rate"
        '
        'cmbBitRate
        '
        Me.cmbBitRate.FormattingEnabled = True
        Me.cmbBitRate.Location = New System.Drawing.Point(8, 27)
        Me.cmbBitRate.Name = "cmbBitRate"
        Me.cmbBitRate.Size = New System.Drawing.Size(102, 21)
        Me.cmbBitRate.TabIndex = 0
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(168, 224)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(114, 50)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "OK"
        '
        'gbxHandshaking
        '
        Me.gbxHandshaking.Controls.Add(Me.cmbHandshaking)
        Me.gbxHandshaking.Location = New System.Drawing.Point(12, 106)
        Me.gbxHandshaking.Name = "gbxHandshaking"
        Me.gbxHandshaking.Size = New System.Drawing.Size(272, 65)
        Me.gbxHandshaking.TabIndex = 5
        Me.gbxHandshaking.TabStop = False
        Me.gbxHandshaking.Text = "Handshaking"
        '
        'cmbHandshaking
        '
        Me.cmbHandshaking.FormattingEnabled = True
        Me.cmbHandshaking.Location = New System.Drawing.Point(13, 28)
        Me.cmbHandshaking.Name = "cmbHandshaking"
        Me.cmbHandshaking.Size = New System.Drawing.Size(248, 21)
        Me.cmbHandshaking.TabIndex = 0
        '
        'chkOpenComPortOnStartup
        '
        Me.chkOpenComPortOnStartup.AutoSize = True
        Me.chkOpenComPortOnStartup.Location = New System.Drawing.Point(15, 187)
        Me.chkOpenComPortOnStartup.Name = "chkOpenComPortOnStartup"
        Me.chkOpenComPortOnStartup.Size = New System.Drawing.Size(150, 17)
        Me.chkOpenComPortOnStartup.TabIndex = 6
        Me.chkOpenComPortOnStartup.Text = "Open COM port on startup"
        Me.chkOpenComPortOnStartup.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(12, 224)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(114, 50)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'PortSettingsDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(306, 286)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.chkOpenComPortOnStartup)
        Me.Controls.Add(Me.gbxHandshaking)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.gbxBitRate)
        Me.Controls.Add(Me.gbxPort)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PortSettingsDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "PortSettings"
        Me.gbxPort.ResumeLayout(False)
        Me.gbxBitRate.ResumeLayout(False)
        Me.gbxHandshaking.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbxPort As System.Windows.Forms.GroupBox
    Friend WithEvents gbxBitRate As System.Windows.Forms.GroupBox
    Friend WithEvents cmbPort As System.Windows.Forms.ComboBox
    Friend WithEvents cmbBitRate As System.Windows.Forms.ComboBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents gbxHandshaking As System.Windows.Forms.GroupBox
    Friend WithEvents cmbHandshaking As System.Windows.Forms.ComboBox
    Friend WithEvents chkOpenComPortOnStartup As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class

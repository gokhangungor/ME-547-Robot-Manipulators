<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MainForm
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
        Me.components = New System.ComponentModel.Container
        Me.rtbMonitor = New System.Windows.Forms.RichTextBox
        Me.rtbStatus = New System.Windows.Forms.RichTextBox
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.btnPort = New System.Windows.Forms.Button
        Me.btnOpenOrClosePort = New System.Windows.Forms.Button
        Me.tmrLookForPortChanges = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'rtbMonitor
        '
        Me.rtbMonitor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbMonitor.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbMonitor.Location = New System.Drawing.Point(12, 74)
        Me.rtbMonitor.Name = "rtbMonitor"
        Me.rtbMonitor.Size = New System.Drawing.Size(455, 327)
        Me.rtbMonitor.TabIndex = 7
        Me.rtbMonitor.Text = ""
        '
        'rtbStatus
        '
        Me.rtbStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbStatus.BackColor = System.Drawing.SystemColors.Control
        Me.rtbStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbStatus.Location = New System.Drawing.Point(14, 422)
        Me.rtbStatus.Margin = New System.Windows.Forms.Padding(5)
        Me.rtbStatus.Name = "rtbStatus"
        Me.rtbStatus.ReadOnly = True
        Me.rtbStatus.Size = New System.Drawing.Size(453, 50)
        Me.rtbStatus.TabIndex = 8
        Me.rtbStatus.Text = ""
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 486)
        Me.StatusStrip1.MinimumSize = New System.Drawing.Size(26, 0)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(487, 37)
        Me.StatusStrip1.TabIndex = 9
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.ToolStripStatusLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ToolStripStatusLabel1.Margin = New System.Windows.Forms.Padding(5)
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Padding = New System.Windows.Forms.Padding(5)
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(462, 27)
        Me.ToolStripStatusLabel1.Spring = True
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPort
        '
        Me.btnPort.AutoSize = True
        Me.btnPort.Location = New System.Drawing.Point(14, 12)
        Me.btnPort.Name = "btnPort"
        Me.btnPort.Size = New System.Drawing.Size(167, 47)
        Me.btnPort.TabIndex = 10
        Me.btnPort.Text = "Port Settings"
        Me.btnPort.UseVisualStyleBackColor = True
        '
        'btnOpenOrClosePort
        '
        Me.btnOpenOrClosePort.AutoSize = True
        Me.btnOpenOrClosePort.Location = New System.Drawing.Point(207, 12)
        Me.btnOpenOrClosePort.Name = "btnOpenOrClosePort"
        Me.btnOpenOrClosePort.Size = New System.Drawing.Size(163, 47)
        Me.btnOpenOrClosePort.TabIndex = 11
        Me.btnOpenOrClosePort.Text = "Open COM Port"
        Me.btnOpenOrClosePort.UseVisualStyleBackColor = True
        '
        'tmrLookForPortChanges
        '
        Me.tmrLookForPortChanges.Interval = 1000
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(487, 523)
        Me.Controls.Add(Me.btnOpenOrClosePort)
        Me.Controls.Add(Me.btnPort)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.rtbStatus)
        Me.Controls.Add(Me.rtbMonitor)
        Me.MaximumSize = New System.Drawing.Size(2000, 2000)
        Me.Name = "MainForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "COM Port Terminal"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rtbMonitor As System.Windows.Forms.RichTextBox
    Friend WithEvents rtbStatus As System.Windows.Forms.RichTextBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnPort As System.Windows.Forms.Button
    Friend WithEvents btnOpenOrClosePort As System.Windows.Forms.Button
    Friend WithEvents tmrLookForPortChanges As System.Windows.Forms.Timer


End Class

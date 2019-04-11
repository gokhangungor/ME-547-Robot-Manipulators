Option Explicit On
Option Strict On

Imports System.IO.Ports

''' <summary>
''' Provides a dialog box for viewing and selecting COM ports and parameters.
''' </summary>

Public Class PortSettingsDialog

    Private bitRates(10) As Integer
    Private oldBitRateIndex As Integer
    Private oldHandshakeIndex As Integer
    Friend oldPortName As String
    Private settingsInitialized As Boolean
    
    ' These events enable other modules to detect events and receive data.

    Friend Shared Event UserInterfacePortSettings(ByVal selectedPort As String, ByVal selectedBitRate As Integer, ByVal selectedHandshake As Handshake)
    Friend Shared Event UserInterfaceData(ByVal action As String, ByVal formText As String, ByVal textColor As Color)

    ''' <summary>
    ''' Display available COM ports in a combo box.
    ''' Assumes ComPorts.FindComPorts has been run to fill the myPorts array.
    ''' </summary>

    Friend Sub DisplayComPorts()

        ' Clear the combo box and repopulate (in case ports have been added or removed).

        cmbPort.DataSource = ComPorts.myPortNames
       
    End Sub

    ''' <summary>
    ''' Set initial port parameters. 
    ''' </summary>

    Friend Sub InitializePortSettings()

        If Not settingsInitialized Then

            'Bit rates to select from.

            bitRates(0) = 300
            bitRates(1) = 600
            bitRates(2) = 1200
            bitRates(3) = 2400
            bitRates(4) = 9600
            bitRates(5) = 14400
            bitRates(6) = 19200
            bitRates(7) = 38400
            bitRates(8) = 57600
            bitRates(9) = 115200
            bitRates(10) = 128000

            'Place the bit rates and handshaking options in the combo boxes.

            cmbBitRate.DataSource = bitRates
            cmbBitRate.DropDownStyle = ComboBoxStyle.DropDownList

            ' Handshaking options.

            cmbHandshaking.Items.Add(Handshake.None)
            cmbHandshaking.Items.Add(Handshake.XOnXOff)
            cmbHandshaking.Items.Add(Handshake.RequestToSend)
            cmbHandshaking.Items.Add(Handshake.RequestToSendXOnXOff)

            cmbHandshaking.DropDownStyle = ComboBoxStyle.DropDownList

            'Find and display available COM ports.

            ComPorts.FindComPorts()

            cmbPort.DataSource = ComPorts.myPortNames

            cmbPort.DropDownStyle = ComboBoxStyle.DropDownList

            settingsInitialized = True
        End If
    End Sub

    ''' <summary>
    ''' Compares stored parameters with the current parameters.
    ''' </summary>
    ''' 
    ''' <returns>
    ''' True if any parameter has changed.
    ''' </returns>

    Friend Function ParameterChanged() As Boolean

        Return (oldBitRateIndex <> cmbBitRate.SelectedIndex) Or _
                  (oldHandshakeIndex <> cmbHandshaking.SelectedIndex) Or _
                  ((String.Compare(oldPortName, CStr(cmbPort.SelectedItem), True) <> 0))
    End Function

    ''' <summary>
    ''' Save the current port parameters.
    ''' Enables learning if a parameter has changed.
    ''' </summary>

    Friend Sub SavePortParameters()

        oldBitRateIndex = cmbBitRate.SelectedIndex
        oldHandshakeIndex = cmbHandshaking.SelectedIndex
        oldPortName = CStr(cmbPort.SelectedItem)

    End Sub

    ''' <summary>
    ''' Select a bit rate in the combo box.
    ''' Does not set the bit rate for a COM port.
    ''' </summary>
    ''' 
    ''' <param name="bitRate"> The requested bit rate </param>

    Friend Sub SelectBitRate(ByVal bitRate As Integer)

        cmbBitRate.SelectedItem = bitRate

    End Sub

    ''' <summary>
    ''' Select a COM port in the combo box.
    ''' Does not set the selectedPort variable.
    ''' </summary>
    ''' 
    ''' <param name="comPortName"> A COM port name </param>
    ''' 
    ''' <returns > The index of the selected port in the combo box </returns>

    Friend Function SelectComPort(ByVal comPortName As String) As Integer

        ' If comPortName doesn't exist in the combo box, SelectedItem remains the same.

        cmbPort.SelectedItem = comPortName

        If (cmbPort.SelectedIndex > -1) Then

            ' At least one COM port exists.

            If (Not (String.Compare(cmbPort.SelectedItem.ToString, comPortName, True) = 0)) Then

                ' The requested port isn't available. Select the first port.

                cmbPort.SelectedIndex = 0

            End If
        Else

            ' No COM ports exist.

            RaiseEvent UserInterfaceData("DisplayStatus", ComPorts.noComPortsMessage, Color.Red)
            ComPorts.comPortExists = False
        End If

        Return cmbPort.SelectedIndex
    End Function

    ''' <summary>
    ''' Sets handshaking in the combo box.
    ''' Does not set handshaking for a COM port.
    ''' </summary>
    ''' 
    ''' <param name="requestedHandshake"> the requested handshaking as a System.IO.Ports.Handshake value. </param> 

    Friend Sub SelectHandshaking(ByVal requestedHandshake As Handshake)

        cmbHandshaking.SelectedItem = requestedHandshake

    End Sub

    ''' <summary>
    ''' Initialize port settings.
    ''' InitializeComponent is required by the Windows Form Designer.
    ''' </summary>
    Sub New()

        InitializeComponent()

        InitializePortSettings()

    End Sub

    ''' <summary>
    ''' The port parameters may have changed.
    ''' Make the parameters available to other modules. 
    ''' </summary>

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Dim statusMessage As String

        ' Set the port parameters.

        If ComPorts.comPortExists Then
            RaiseEvent UserInterfacePortSettings(cmbPort.SelectedItem.ToString, CInt(cmbBitRate.SelectedItem), DirectCast(cmbHandshaking.SelectedItem, Handshake))
        End If

        RaiseEvent UserInterfaceData("DisplayCurrentSettings", "", Color.Black)

        If cmbPort.SelectedIndex = -1 Then
            statusMessage = ComPorts.noComPortsMessage
        Else
            statusMessage = ""
        End If

        RaiseEvent UserInterfaceData("DisplayStatus", statusMessage, Color.Black)

    End Sub

    ''' <summary>
    ''' Configure components in the dialog box.
    ''' </summary>

    Friend Sub PortSettingsDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        btnOK.DialogResult = Windows.Forms.DialogResult.OK
        Me.AcceptButton = btnOK
        btnOK.Focus()

    End Sub

    ''' <summary>
    ''' Don't save any changes to the form.
    ''' </summary>

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Hide()
    End Sub
End Class

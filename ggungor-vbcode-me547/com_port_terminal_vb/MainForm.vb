Option Explicit On
Option Strict On

Imports System.IO.Ports
Imports Microsoft.Win32

''' <summary>
''' com_port_terminal 
''' Visual Basic .NET application. Created with Visual Studio 2008.
'''
''' Demonstrates communications using .NET 2.0's SerialPort class
''' Displays a text box where users can enter text to send to a COM port 
''' and view text received from a COM port.
''' 
''' The user can select a COM port, bit rate, and handshaking.
''' A ToolStripStatusLabel displays the current port settings.
''' A second text box displays error messages and other information.
''' 
''' Created using Visual Basic 2005 Express (free download from Microsoft).
'''
''' By Jan Axelson
''' Version 1.2
''' This application and more information about serial ports are available at Lvr.com
''' </summary>
''' 
''' <remarks>
''' The COM port can be any RS-232 port, including USB/RS-232 converters
''' or another device (such as FTDI FT245BM) that uses COM-port drivers on the PC.
'''
''' The ComPorts class stores information about the system's COM ports
''' and enables accessing them.
'''
''' The PortSettingsDialog dialog box displays the system's ports and updates 
''' as needed to display the currently attached ports.
'''
''' If there are no COM ports, the application searches for new ports and selects
''' one when attached.
'''
''' User preferences are stored in a .settings file and retrieved when the application runs.
'''
''' To connect two RS-232 ports on the same PC or different PCs, 
''' use a null-modem cable. (Cross-connect RXD to TXD, RTS to CTS, DTR to DSR.)
'''
''' Each COM port can be controlled by an instance of this application, or one of the ports 
''' can be controlled by Windows Hyperterminal or another application.
'''
''' Known issues:
'''
''' 1. Users should avoid removing virtual COM ports that are in use.
''' Physically removing a USB virtual COM port with an open handle 
''' can terminate the application with the following error:
'''
''' System.ObjectDisposedException was unhandled
''' Message="Safe handle has been closed"
''' at Microsoft.Win32.UnsafeNativeMethods.WaitCommEvent.
'''
''' Application.ThreadException doesn't catch this exception.
''' AppDomain.CurrentDomain.UnhandledException can detect the exception but terminates the application. 
'''
''' 2. The ToolStripStatusLabel at the bottom of the form
''' occasionally truncates to a few characters wide or less and you need to
''' restart the application to restore it. Advice welcome on this issue.
'''
''' Send any comments or trouble reports to me at jan@Lvr.com 
''' </remarks>

Public Class MainForm

    Const ButtonTextOpenPort As String = "Open COM Port"
    Const ButtonTextClosePort As String = "Close COM Port"
    Const ModuleName As String = "COM Port Terminal"

    Friend MyMainForm As MainForm
    Friend MyPortSettingsDialog As PortSettingsDialog
    Friend UserPort1 As ComPorts

    Private Delegate Sub AccessFormMarshalDelegate(ByVal action As String, ByVal textToAdd As String, ByVal textColor As Color)

    Private AccessFormMarshalDelegate1 As AccessFormMarshalDelegate
    Private colorReceive As Color = Color.Green
    Private colorTransmit As Color = Color.Red
    Private maximumTextBoxLength As Integer
    Private receiveBuffer As String
    Private savedOpenPortOnStartup As Boolean
    Private userInputIndex As Integer

    ''' <summary> 
    ''' Perform functions on the application's form.
    ''' Used to access the form from a different thread.
    ''' See AccessFormMarshal().
    ''' </summary>
    ''' 
    ''' <param name="action"> a string that names the action to perform on the form </param>  
    ''' <param name="formText"> text that the form displays </param> 
    ''' <param name="textColor"> a system color for displaying text </param>

    Private Sub AccessForm(ByVal action As String, ByVal formText As String, ByVal textColor As Color)

        Select Case action

            ' Select an action to perform on the form.
            ' (Can add more actions as needed.)

            Case "AppendToMonitorTextBox"

                ' Append text to the rtbMonitor textbox using the color for received data.

                rtbMonitor.SelectionColor = colorReceive
                rtbMonitor.AppendText(formText)

                'Return to the default color.

                rtbMonitor.SelectionColor = colorTransmit

                ' Trim the textbox's contents if needed.

                If rtbMonitor.TextLength > maximumTextBoxLength Then

                    TrimTextBoxContents()

                End If

            Case "DisplayStatus"

                ' Add text to the rtbStatus textbox using the specified color.

                DisplayStatus(formText, textColor)

            Case "DisplayCurrentSettings"

                ' Display the current port settings in the ToolStripStatusLabel.

                DisplayCurrentSettings()

            Case Else

        End Select

    End Sub

    ''' <summary>
    ''' Enables accessing the form from another thread.
    ''' The parameters match those of AccessForm() 
    ''' </summary>
    ''' 
    ''' <param name="action"> a string that names the action to perform on the form </param>  
    ''' <param name="formText"> text that the form displays </param> 
    ''' <param name="textColor"> a system color for displaying text </param>

    Private Sub AccessFormMarshal(ByVal action As String, ByVal formText As String, ByVal textColor As Color)

        AccessFormMarshalDelegate1 = New AccessFormMarshalDelegate(AddressOf AccessForm)
        Dim args() As Object = {action, formText, textColor}

        ' Call AccessForm, passing the parameters in args.

        MyBase.Invoke(AccessFormMarshalDelegate1, args)

    End Sub

    ''' <summary>
    ''' Display the current port parameters on the form.
    ''' </summary>

    Private Sub DisplayCurrentSettings()

        Dim selectedPortState As String = ""

        If ComPorts.comPortExists Then

            If (Not (UserPort1.SelectedPort Is Nothing)) Then

                If UserPort1.SelectedPort.IsOpen Then
                    selectedPortState = "OPEN"
                    btnOpenOrClosePort.Text = ButtonTextClosePort
                Else
                    selectedPortState = "CLOSED"
                    btnOpenOrClosePort.Text = ButtonTextOpenPort
                End If
            End If

            UpdateStatusLabel _
            (CStr(MyPortSettingsDialog.cmbPort.SelectedItem) + "   " + _
            CStr(MyPortSettingsDialog.cmbBitRate.SelectedItem) + _
            "   N 8 1   Handshake: " + _
            MyPortSettingsDialog.cmbHandshaking.SelectedItem.ToString + _
            "   " + selectedPortState)

        Else
            DisplayStatus(ComPorts.noComPortsMessage, Color.Red)
            UpdateStatusLabel("")

        End If
    End Sub

    ''' <summary>
    ''' Provide a central mechanism for displaying exception information.
    ''' Display a message that describes the exception.
    ''' </summary>
    ''' 
    ''' <param name="moduleName"> the module where the exception occurred.</param>
    ''' <param name="ex"> the exception </param>

    Private Sub DisplayException(ByVal moduleName As String, ByVal ex As Exception)

        Dim errorMessage As String

        errorMessage = "Exception: " & ex.Message & _
        " Module: " & moduleName & _
        ". Method: " & ex.TargetSite.Name

        DisplayStatus(errorMessage, Color.Red)

        ' To display errors in a message box, uncomment this line:
        'MessageBox.Show(errorMessage)

    End Sub

    ''' <summary>
    ''' Displays text in a richtextbox.
    ''' </summary>
    ''' 
    ''' <param name="status"> the text to display.</param>
    ''' <param name="textColor"> the text color. </param>

    Private Sub DisplayStatus(ByVal status As String, ByVal textColor As Color)

        ' Purpose    : Displays text in a richtextbox.

        ' Accepts    : status - the text to display.
        '            : textcolor - the text color.

        rtbStatus.ForeColor = textColor
        rtbStatus.Text = status

    End Sub

    ''' <summary>
    ''' Get user preferences for the COM port and parameters.
    ''' See SetPreferences for more information.
    ''' </summary>

    Private Sub GetPreferences()

        UserPort1.SavedPortName = Settings.Default.ComPort
        UserPort1.SavedBitRate = Settings.Default.BitRate
        UserPort1.SavedHandshake = Settings.Default.Handshaking
        savedOpenPortOnStartup = Settings.Default.OpenComPortOnStartup

    End Sub

    ''' <summary>
    ''' Initialize elements on the main form.
    ''' </summary>

    Private Sub InitializeDisplayElements()

        ' The TrimTextboxContents routine trims a richtextbox with more data than this:

        maximumTextBoxLength = 10000
        rtbMonitor.SelectionColor = colorTransmit

    End Sub

    ''' <summary>
    '''  Determine if the textbox's TextChanged event occurred due to new user input.
    ''' If yes, get the input and write it to the COM port.
    ''' </summary>

    Private Sub ProcessTextboxInput()

        Dim ar As IAsyncResult
        Dim msg As String
        Dim textLength As Integer
        Dim userInput As String

        ' Find out if the textbox contains new user input.
        ' If the new data is data received on the COM port or if no COM port exists, do nothing.

        If ((rtbMonitor.Text.Length > userInputIndex + UserPort1.receivedDataLength) And _
        ComPorts.comPortExists) Then

            ' Retrieve the contents of the textbox.

            userInput = rtbMonitor.Text

            ' Get the length of the new text.

            textLength = userInput.Length - userInputIndex

            ' Extract the unread input.

            userInput = rtbMonitor.Text.Substring(userInputIndex, textLength)

            ' Create a message to pass to the Write operation (optional). 
            ' The callback routine can retrieve the message when the write completes.

            msg = DateTime.Now.ToString

            ' Send the input to the COM port.
            ' Use a different thread so the main application doesn't have to wait
            ' for the write operation to complete.

            ar = UserPort1.WriteToComPortDelegate1.BeginInvoke _
            (userInput, New AsyncCallback(AddressOf UserPort1.WriteCompleted), msg)

            ' To use the same thread for writes to the port,
            ' comment out the statement above and uncomment the statement below.
            'UserPort1.WriteToComPort(userInput)

            AccessForm("UpdateStatusLabel", "", Color.Black)

        Else
            ' Received bytes displayed in the text box are ignored,
            ' but we need to reset the value that indicates
            ' the number of received but not processed bytes.

            UserPort1.receivedDataLength = 0

        End If

        If rtbMonitor.TextLength > maximumTextBoxLength Then

            TrimTextBoxContents()

        End If

        ' Update the value that indicates the last character processed.

        userInputIndex = rtbMonitor.Text.Length

    End Sub

    ''' <summary> 
    ''' Save user preferences for the COM port and parameters.
    ''' </summary>

    Private Sub SavePreferences()

        ' To define additional settings, in the Visual Studio IDE go to
        ' Solution Explorer > right click on project name > Properties > Settings.

        If (MyPortSettingsDialog.cmbPort.SelectedIndex > -1) Then

            ' The system has at least one COM port.

            Settings.Default.ComPort = MyPortSettingsDialog.cmbPort.SelectedItem.ToString
            Settings.Default.BitRate = CInt(MyPortSettingsDialog.cmbBitRate.SelectedItem)
            Settings.Default.Handshaking = DirectCast(MyPortSettingsDialog.cmbHandshaking.SelectedItem, Handshake)
            Settings.Default.OpenComPortOnStartup = MyPortSettingsDialog.chkOpenComPortOnStartup.Checked

            Settings.Default.Save()
        End If
    End Sub

    ''' <summary>
    ''' Use stored preferences or defaults to set the initial port parameters.
    ''' </summary>

    Private Sub SetInitialPortParameters()

        ' Get preferences or default values.

        GetPreferences()

        If ComPorts.comPortExists Then

            ' Select a COM port and bit rate using stored preferences if available.

            UsePreferencesToSelectParameters()

            ' Save the selected indexes of the combo boxes.

            MyPortSettingsDialog.SavePortParameters()

        Else

            ' No COM ports have been detected. Watch for one to be attached.

            tmrLookForPortChanges.Start()
            DisplayStatus(ComPorts.noComPortsMessage, Color.Red)

        End If

        UserPort1.parameterChanged = False

    End Sub

    ''' <summary>
    ''' Saves the passed port parameters.
    ''' Called when the user clicks OK on PortSettingsDialog.
    ''' </summary>

    Private Sub SetPortParameters(ByVal userPort As String, ByVal userBitRate As Integer, ByVal userHandshake As Handshake)

        Try

            ' Don't do anything if the system has no COM ports.

            If ComPorts.comPortExists Then

                If MyPortSettingsDialog.ParameterChanged Then

                    ' One or more port parameters has changed.

                    If (String.Compare(MyPortSettingsDialog.oldPortName, CStr(userPort), True) <> 0) Then

                        ' The port has changed.
                        ' Close the previously selected port.

                        UserPort1.PreviousPort = UserPort1.SelectedPort
                        UserPort1.CloseComPort(UserPort1.SelectedPort)

                        ' Set SelectedPort to the current port.

                        UserPort1.SelectedPort.PortName = userPort
                        UserPort1.PortChanged = True

                    End If

                    ' Set other port parameters.

                    UserPort1.SelectedPort.BaudRate = userBitRate
                    UserPort1.SelectedPort.Handshake = userHandshake

                    MyPortSettingsDialog.SavePortParameters()

                    UserPort1.ParameterChanged = True

                Else
                    UserPort1.ParameterChanged = False

                End If
            End If

        Catch ex As InvalidOperationException

            UserPort1.parameterChanged = True
            DisplayException(ModuleName, ex)

        Catch ex As UnauthorizedAccessException

            UserPort1.parameterChanged = True
            DisplayException(ModuleName, ex)

            ' This exception can occur if the port was removed. 
            ' If the port was open, close it.

            UserPort1.CloseComPort(UserPort1.selectedPort)

        Catch ex As System.IO.IOException

            UserPort1.parameterChanged = True
            DisplayException(ModuleName, ex)

        End Try

    End Sub

    ''' <summary>
    ''' Trim a richtextbox by removing the oldest contents.
    ''' </summary>
    ''' 
    ''' <remarks >
    ''' To trim the box while retaining any formatting applied to the retained contents,
    ''' create a temporary richtextbox, copy the contents to be preserved to the 
    ''' temporary richtextbox,and copy the temporary richtextbox back to the original richtextbox.
    ''' </remarks>

    Private Sub TrimTextBoxContents()

        Dim rtbTemp As New RichTextBox
        Dim textboxTrimSize As Integer

        ' When the contents are too large, remove half.

        textboxTrimSize = maximumTextBoxLength \ 2

        rtbMonitor.Select(rtbMonitor.TextLength - textboxTrimSize + 1, textboxTrimSize)
        rtbTemp.Rtf = rtbMonitor.SelectedRtf
        rtbMonitor.Clear()
        rtbMonitor.Rtf = rtbTemp.Rtf
        rtbTemp = Nothing
        rtbMonitor.SelectionStart = rtbMonitor.TextLength

    End Sub

    ''' <summary>
    ''' Set the text in the ToolStripStatusLabel.
    ''' </summary>
    ''' 
    ''' <param name="status"> the text to display </param>

    Private Sub UpdateStatusLabel(ByVal status As String)

        ToolStripStatusLabel1.Text = status
        StatusStrip1.Update()

    End Sub

    ''' <summary>
    ''' Set the user preferences or default values in the combo boxes and ports array
    ''' using stored preferences or default values.
    ''' </summary>

    Private Sub UsePreferencesToSelectParameters()

        MyPortSettingsDialog.SelectComPort(UserPort1.SavedPortName)

        MyPortSettingsDialog.SelectBitRate(UserPort1.SavedBitRate)

        UserPort1.SelectedPort.BaudRate = UserPort1.SavedBitRate

        MyPortSettingsDialog.SelectHandshaking(UserPort1.SavedHandshake)

        UserPort1.SelectedPort.Handshake = UserPort1.SavedHandshake

        MyPortSettingsDialog.chkOpenComPortOnStartup.Checked = savedOpenPortOnStartup

    End Sub

    ''' <summary>
    ''' Depending on the text displayed on the button, open or close the selected port
    ''' and change the button text to the opposite action.
    ''' </summary>

    Private Sub btnOpenOrClosePort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpenOrClosePort.Click

        If (btnOpenOrClosePort.Text Is buttonTextOpenPort) Then
            UserPort1.OpenComPort()
            If UserPort1.selectedPort.IsOpen Then
                btnOpenOrClosePort.Text = ButtonTextClosePort
            End If

        Else
            UserPort1.CloseComPort(UserPort1.selectedPort)

            If Not UserPort1.selectedPort.IsOpen Then
                btnOpenOrClosePort.Text = ButtonTextOpenPort
            End If

        End If

    End Sub

    ''' <summary>
    ''' Look for COM ports and display them in the combo box.
    ''' </summary>

    Private Sub btnPort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPort.Click

        ComPorts.FindComPorts()

        MyPortSettingsDialog.DisplayComPorts()
        MyPortSettingsDialog.SelectComPort(UserPort1.SelectedPort.PortName)
        MyPortSettingsDialog.SelectBitRate(UserPort1.SelectedPort.BaudRate)
        MyPortSettingsDialog.SelectHandshaking(UserPort1.SelectedPort.Handshake)

        UserPort1.ParameterChanged = False

        ' Display the combo boxes for setting port parameters.

        MyPortSettingsDialog.ShowDialog()
    End Sub

    ''' <summary>
    ''' Create an instance of the ComPorts class.
    ''' Initialize port settings and other parameters. 
    ''' specify behavior on events.
    ''' </summary>

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Show()

        ' Create an instance of the ComPorts class for accessing a specific port.

        UserPort1 = New ComPorts

        MyPortSettingsDialog = New PortSettingsDialog

        tmrLookForPortChanges.Interval = 1000
        tmrLookForPortChanges.Stop()

        InitializeDisplayElements()

        SetInitialPortParameters()

        If ComPorts.comPortExists Then
            UserPort1.SelectedPort.PortName = ComPorts.myPortNames(MyPortSettingsDialog.cmbPort.SelectedIndex)

            ' A check box enables requesting to open the selected COM port on start up.
            ' Otherwise the application opens the port when the user clicks the Open Port
            ' button or types text to send. 

            If MyPortSettingsDialog.chkOpenComPortOnStartup.Checked Then

                UserPort1.portOpen = UserPort1.OpenComPort()
                AccessForm("DisplayCurrentSettings", "", Color.Black)
                AccessForm("DisplayStatus", "", Color.Black)

            Else
                DisplayCurrentSettings()
            End If
        End If

        ' Specify the routines that execute on events in other modules.
        ' The routines can receive data from other modules. 

        AddHandler ComPorts.UserInterfaceData, AddressOf AccessFormMarshal
        AddHandler PortSettingsDialog.UserInterfaceData, AddressOf AccessFormMarshal
        AddHandler PortSettingsDialog.UserInterfacePortSettings, AddressOf SetPortParameters
    End Sub

    ''' <summary>
    ''' Close the port if needed and save preferences.
    ''' </summary>

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        UserPort1.CloseComPort(UserPort1.selectedPort)
        SavePreferences()
    End Sub

    ''' <summary>
    ''' Do whatever is needed with new characters in the textbox.
    ''' </summary>

    Private Sub rtbMonitor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rtbMonitor.TextChanged

        ProcessTextboxInput()

    End Sub

    ''' <summary>
    ''' Look for ports. If at least one is found, stop the timer and
    ''' select the saved port if possible or the first port.
    ''' This timer is enabled only when no COM ports are present.
    ''' </summary>

    Private Sub tmrLookForPortChanges_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrLookForPortChanges.Tick

        ComPorts.FindComPorts()

        If ComPorts.comPortExists Then

            tmrLookForPortChanges.Stop()
            DisplayStatus("COM port(s) found.", Color.Black)

            MyPortSettingsDialog.DisplayComPorts()
            MyPortSettingsDialog.SelectComPort(UserPort1.savedPortName)
            MyPortSettingsDialog.SelectBitRate(UserPort1.SavedBitRate)
            MyPortSettingsDialog.SelectHandshaking(UserPort1.SavedHandshake)

            ' Set selectedPort.

            SetPortParameters(UserPort1.savedPortName, CInt(UserPort1.savedBitRate), DirectCast(UserPort1.savedHandshake, Handshake))

            DisplayCurrentSettings()
            UserPort1.parameterChanged = True
        End If
    End Sub

End Class

option explicit on
Option Strict On

Imports System.IO.Ports
Imports System.Runtime.Remoting.Messaging

''' <summary>
''' Routines for finding and accessing COM ports.
''' </summary>

Public Class ComPorts

    Const ModuleName As String = "ComPorts"

    ' Shared members - do not belong to a specific instance of the class.

    Friend Shared comPortExists As Boolean
    Friend Shared myPortNames() As String
    Friend Shared noComPortsMessage As String = "No COM ports found. Please attach a COM-port device."

    Friend Shared Event UserInterfaceData _
           (ByVal action As String, ByVal formText As String, ByVal textColor As Color)

	' Non-shared members - belong to a specific instance of the class.
	
    Friend Delegate Function WriteToComPortDelegate(ByVal textToWrite As String) As Boolean

    Friend WriteToComPortDelegate1 As New WriteToComPortDelegate(AddressOf WriteToComPort)

    Private SerialDataReceivedEventHandler1 _
      As New SerialDataReceivedEventHandler(AddressOf DataReceived)
    Private SerialErrorReceivedEventHandler1 _
          As New SerialErrorReceivedEventHandler(AddressOf ErrorReceived)

    ' Local variables available as Properties.

    Private m_ParameterChanged As Boolean
    Private m_PortChanged As Boolean
    Private m_PortOpen As Boolean
    Private m_PreviousPort As New SerialPort
    Private m_ReceivedDataLength As Integer
    Private m_SavedBitRate As Integer = 9600
    Private m_SavedHandshake As Handshake = Handshake.None
    Private m_SavedPortName As String = ""
    Private m_SelectedPort As New SerialPort

    Friend Property ParameterChanged() As Boolean
        Get
            Return m_ParameterChanged
        End Get
        Set(ByVal value As Boolean)
            m_ParameterChanged = value
        End Set
    End Property

    Friend Property PortChanged() As Boolean
        Get
            Return m_PortChanged
        End Get
        Set(ByVal value As Boolean)
            m_PortChanged = value
        End Set
    End Property

    Friend Property PortOpen() As Boolean
        Get
            Return m_PortOpen
        End Get
        Set(ByVal value As Boolean)
            m_PortOpen = value
        End Set
    End Property

    Friend Property PreviousPort() As SerialPort
        Get
            Return m_PreviousPort
        End Get
        Set(ByVal value As SerialPort)
            m_PreviousPort = value
        End Set
    End Property
   
    Friend Property ReceivedDataLength() As Integer
        Get
            Return m_ReceivedDataLength
        End Get
        Set(ByVal value As Integer)
            m_ReceivedDataLength = value
        End Set
    End Property

    Friend Property SavedBitRate() As Integer
        Get
            Return m_SavedBitRate
        End Get
        Set(ByVal value As Integer)
            m_SavedBitRate = value
        End Set
    End Property

    Friend Property SavedHandshake() As Handshake
        Get
            Return m_SavedHandshake
        End Get
        Set(ByVal value As Handshake)
            m_SavedHandshake = value
        End Set
    End Property

    Friend Property SavedPortName() As String
        Get
            Return m_SavedPortName
        End Get
        Set(ByVal value As String)
            m_SavedPortName = value
        End Set
    End Property

    Friend Property SelectedPort() As SerialPort
        Get
            Return m_SelectedPort
        End Get
        Set(ByVal value As SerialPort)
            m_SelectedPort = value
        End Set
    End Property

    ''' <summary>
    ''' If the COM port is open, close it.
    ''' </summary>
    ''' 
    ''' <param name="portToClose"> the SerialPort object to close </param>  

    Friend Sub CloseComPort(ByVal portToClose As SerialPort)

        Try
            RaiseEvent UserInterfaceData("DisplayStatus", "", Color.Black)

            If (Not IsNothing(portToClose)) Then

                If portToClose.IsOpen Then

                    portToClose.Close()
                    RaiseEvent UserInterfaceData("DisplayCurrentSettings", "", Color.Black)

                End If
            End If

        Catch ex As InvalidOperationException

            parameterChanged = True
            portChanged = True
            DisplayException(ModuleName, ex)

        Catch ex As UnauthorizedAccessException

            parameterChanged = True
            portChanged = True
            DisplayException(ModuleName, ex)

        Catch ex As System.IO.IOException

            parameterChanged = True
            portChanged = True
            DisplayException(ModuleName, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Called when data is received on the COM port.
    ''' Reads and displays the data.
    ''' See FindPorts for the AddHandler statement for this routine.
    ''' </summary>

    Friend Sub DataReceived(ByVal sender As Object, ByVal e As SerialDataReceivedEventArgs)

        Dim newReceivedData As String

        Try
            ' Get data from the COM port.

            newReceivedData = selectedPort.ReadExisting

            ' Save the number of characters received.

            receivedDataLength += newReceivedData.Length

            RaiseEvent UserInterfaceData("AppendToMonitorTextBox", newReceivedData, Color.Black)

        Catch ex As Exception
            DisplayException(ModuleName, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Provide a central mechanism for displaying exception information.
    ''' Display a message that describes the exception.
    ''' </summary>
    ''' 
    ''' <param name="ex"> The exception </param> 
    ''' <param name="moduleName" > the module where the exception was raised. </param>

    Private Sub DisplayException(ByVal moduleName As String, ByVal ex As Exception)

        Dim errorMessage As String

        errorMessage = "Exception: " & ex.Message & _
        " Module: " & moduleName & _
        ". Method: " & ex.TargetSite.Name

        RaiseEvent UserInterfaceData("DisplayStatus", errorMessage, Color.Red)

        ' To display errors in a message box, uncomment this line:
        ' MessageBox.Show(errorMessage)
    End Sub

    ''' <summary>
    ''' Respond to error events.
    ''' </summary>

    Private Sub ErrorReceived(ByVal sender As Object, ByVal e As SerialErrorReceivedEventArgs)

        Dim SerialErrorReceived1 As SerialError

        SerialErrorReceived1 = e.EventType

        Select Case SerialErrorReceived1

            Case SerialError.Frame
                Console.WriteLine("Framing error.")

            Case SerialError.Overrun
                Console.WriteLine("Character buffer overrun.")

            Case SerialError.RXOver
                Console.WriteLine("Input buffer overflow.")

            Case SerialError.RXParity
                Console.WriteLine("Parity error.")

            Case SerialError.TXFull
                Console.WriteLine("Output buffer full.")
        End Select
    End Sub

    ''' <summary>
    ''' Find the PC's COM ports and store parameters for each port.
    ''' Use saved parameters if possible, otherwise use default values.  
    ''' </summary>
    ''' 
    ''' <remarks> 
    ''' The ports can change if a USB/COM-port converter is attached or removed,
    ''' so this routine may need to run multiple times.
    ''' </remarks>

    Friend Shared Sub FindComPorts()

        ' Place the names of all COM ports in an array and sort.

        myPortNames = SerialPort.GetPortNames

        ' Is there at least one COM port?

        If myPortNames.Length > 0 Then

            comPortExists = True
            Array.Sort(myPortNames)

        Else
            ' No COM ports found.

            comPortExists = False
        End If

    End Sub

    ''' <summary>
    ''' Open the SerialPort object selectedPort.
    ''' If open, close the SerialPort object previousPort.
    ''' </summary>

    Friend Function OpenComPort() As Boolean

        Dim success As Boolean = False

        Try
            If comPortExists Then

                ' The system has at least one COM port.
                ' If the previously selected port is still open, close it.

                If PreviousPort.IsOpen Then
                    CloseComPort(PreviousPort)
                End If

                If (Not (SelectedPort.IsOpen) Or PortChanged) Then

                    SelectedPort.Open()

                    If SelectedPort.IsOpen Then

                        ' The port is open. Set additional parameters.
                        ' Timeouts are in milliseconds.

                        SelectedPort.ReadTimeout = 5000
                        SelectedPort.WriteTimeout = 5000

                        ' Specify the routine that runs when a DataReceived event occurs.

                        AddHandler SelectedPort.DataReceived, SerialDataReceivedEventHandler1
                        AddHandler SelectedPort.ErrorReceived, SerialErrorReceivedEventHandler1

                        ' Send data to other modules.

                        RaiseEvent UserInterfaceData("DisplayCurrentSettings", "", Color.Black)
                        RaiseEvent UserInterfaceData("DisplayStatus", "", Color.Black)

                        success = True

                        ' The port is open with the current parameters.

                        PortChanged = False

                    End If
                End If

            End If

        Catch ex As InvalidOperationException

            parameterChanged = True
            portChanged = True
            DisplayException(ModuleName, ex)

        Catch ex As UnauthorizedAccessException

            parameterChanged = True
            portChanged = True
            DisplayException(ModuleName, ex)

        Catch ex As System.IO.IOException

            parameterChanged = True
            portChanged = True
            DisplayException(ModuleName, ex)
        End Try

        Return success

    End Function

    ''' <summary>
    '''  Executes when WriteToComPortDelegate1 completes.
    ''' </summary>
    ''' <param name="ar"> the value returned by the delegate's BeginInvoke method </param> 

    Friend Sub WriteCompleted(ByVal ar As IAsyncResult)

        Dim deleg As WriteToComPortDelegate
        Dim msg As String
        Dim success As Boolean

        ' Extract the value returned by BeginInvoke (optional).

        msg = DirectCast(ar.AsyncState, String)

        ' Get the value returned by the delegate.

        deleg = DirectCast(DirectCast(ar, AsyncResult).AsyncDelegate, WriteToComPortDelegate)

        success = deleg.EndInvoke(ar)

        If success Then
            RaiseEvent UserInterfaceData("UpdateStatusLabel", "", Color.Black)
        End If

        ' Add any actions that need to be performed after a write to the COM port completes.
        ' This example displays the value passed to the BeginInvoke method
        ' and the value returned by EndInvoke.

        Console.WriteLine("Write operation began: " & msg)
        Console.WriteLine("Write operation succeeded: " & success)

    End Sub

    ''' <summary>
    ''' Write a string to the SerialPort object selectedPort.
    ''' </summary>
    ''' 
    ''' <param name="textToWrite"> A string to write </param> 

    Friend Function WriteToComPort(ByVal textToWrite As String) As Boolean

        Dim success As Boolean

        Try
            ' Open the COM port if necessary.

            If (Not (selectedPort Is Nothing)) Then
                If ((Not selectedPort.IsOpen) Or portChanged) Then

                    ' Close the port if needed and open the selected port.

                    portOpen = OpenComPort()

                End If
            End If

            If selectedPort.IsOpen Then
                selectedPort.Write(textToWrite)
                success = True
            End If

        Catch ex As TimeoutException
            DisplayException(ModuleName, ex)

        Catch ex As InvalidOperationException
            DisplayException(ModuleName, ex)
            parameterChanged = True
            RaiseEvent UserInterfaceData("DisplayCurrentSettings", "", Color.Black)

        Catch ex As UnauthorizedAccessException
            DisplayException(ModuleName, ex)

            ' This exception can occur if the port was removed. 
            ' If the port was open, close it.

            CloseComPort(selectedPort)
            parameterChanged = True
            RaiseEvent UserInterfaceData("DisplayCurrentSettings", "", Color.Black)

        End Try

        Return success

    End Function

End Class
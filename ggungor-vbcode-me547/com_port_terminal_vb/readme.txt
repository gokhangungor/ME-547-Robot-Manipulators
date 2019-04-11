    ' com_port_terminal 
    ' Visual Basic .NET application. Created with Visual Studio 2008.

    ' Demonstrates communications using .NET 2.0's SerialPort class
    ' Displays a text box where users can enter text to send to a COM port 
    ' and view text received from a COM port.
    ' The user can select a COM port, bit rate, and handshaking.
    ' A ToolStripStatusLabel displays the current port settings.
    ' A second text box displays error messages and other information.

    ' The COM port can be any RS-232 port, including USB/RS-232 converters
    ' or another device (such as FTDI FT245BM) that uses COM-port drivers on the PC.

    ' The ComPorts class stores information about the system's COM ports
    ' and enables accessing them.

    ' The PortSettingsDialog dialog box displays the system's ports and updates 
    ' as needed to display the currently attached ports.

    ' If there are no COM ports, the application searches for new ports and selects
    ' one when attached.

    ' User preferences are stored in the registry and retrieved when the application runs.

    ' To connect two RS-232 ports on the same PC or different PCs, 
    ' use a null-modem cable. (Cross-connect RXD to TXD, RTS to CTS, DTR to DSR.)

    ' Each COM port can be controlled by an instance of this application, or one of the ports 
    ' can be controlled by Windows Hyperterminal or another application.

    ' Created using Visual Basic 2005 Express (free download from Microsoft).

    ' By Jan Axelson
    ' Version 1.1
    ' This application and more inforamation about serial ports are available at Lvr.com

    ' Known issues:

    ' For unknown reasons, the ToolStripStatusLabel at the bottom of the form
    ' occasionally truncates to a few characters wide or less and you need to
    ' restart the application to restore it. Advice welcome on this issue.

    ' Physically removing a USB virtual COM port with an open handle 
    ' can terminate the application with the following error:

    ' System.ObjectDisposedException was unhandled
    ' Message="Safe handle has been closed"
    ' at Microsoft.Win32.UnsafeNativeMethods.WaitCommEvent.

    ' Application.ThreadException doesn't catch this exception.
    ' AppDomain.CurrentDomain.UnhandledException can detect the exception but terminates the application. 
    ' Thus, users should avoid removing virtual COM ports that are in use.

    ' Send any comments or trouble reports to me at jan@Lvr.com 
'Attribute VB_Name = "JSendAcad"
Option Explicit

Imports System
Imports System.Data
Imports System.Text
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports System.Windows.Forms.Application

Module JSendAcad
    Private Delegate Function D_EnumWindowsProc(ByVal hWnd As Integer, ByVal lParam As Integer) As Long
    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Long, ByVal dwExtraInfo As Long)
    Private Const KEYEVENTF_KEYUP As Long = &H2
    Private Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As Long
    Private Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" (ByVal hwnd As Long, ByVal lpString As String, ByVal cch As Long) As Long
    Private Declare Function EnumWindows Lib "user32" (ByVal lpEnumFunc As D_EnumWindowsProc, ByVal lParam As Long) As Long
    Private Declare Function GetParent Lib "user32" (ByVal hwnd As Long) As Long
    Private Declare Function SetActiveWindow Lib "user32" (ByVal hwnd As Long) As Long
    Private FindWinTitle As String  '���o����Window�̃^�C�g��
    Private FindWinWnd As Long      '���o����Window�̃n���h��
    Private Const MAX_CHAR = 256    '�ő啶����
    Private nSendKeys As Long '=0:���������A=1:SendKeys���g�p�A=2:keybd_event���g�p�i�f�t�H���g�j
    '��0,2�ȊO��1�Ɠ���
    Private nSleepTime As Long 'SendMsg�ɂ�����Sleep����(ms)
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" _
        ( _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpApplicationName As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpKeyName As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpDefault As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpReturnedString As StringBuilder, _
            ByVal nSize As Integer, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpFileName As String _
        ) As Long

    '   ���X�y�b�N��PC�ȂǂŁA���b�Z�[�W���M������ɓ��삵�Ȃ����̑Ή�
    '   AppActivate��SendKeys���\�b�h�͎g�p�����ɁAAPI�֐��ɂ�Window��Active����L�[���M���s��

    '---------------------------------------------------------------------------------------------------
    '�@�\
    '   Active��Window�Ɏw�肳�ꂽ�R�[�h�̃L�[�𑗐M����
    '����
    '   KeyCode(I)�F�L�[�R�[�h
    '�߂�l
    '   ����
    '---------------------------------------------------------------------------------------------------
    Private Sub SendOneKey(ByVal KeyCode As Long, ByVal blnShift As Boolean)

        If blnShift Then Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, 0, 0)
        Call keybd_event(KeyCode, 0, 0, 0)
        Call keybd_event(KeyCode, 0, KEYEVENTF_KEYUP, 0)
        If blnShift Then Call keybd_event(System.Windows.Forms.Keys.ShiftKey, 0, KEYEVENTF_KEYUP, 0)

    End Sub

    '---------------------------------------------------------------------------------------------------
    '�@�\
    '   �w�肳�ꂽ�^�C�g����Window�Ƀ��b�Z�[�W�𑗐M����
    '����
    '   WinTitle(I)�FWindow�^�C�g��
    '   Message(I)�F���M���郁�b�Z�[�W
    '�߂�l
    '   ����
    '---------------------------------------------------------------------------------------------------
    Public Sub SendMsg(ByVal WinTitle As String, ByVal Message As String)
        Dim i As Integer
        Dim strMsg As String
        Dim strKey As String
        Dim lngKeyCode As Long
        Dim blnKeyOK As Boolean
        Dim strFileName As String

        strFileName = FunGetLanguage()

        FindWindowByTitle(WinTitle)
        SendOneKey(System.Windows.Forms.Keys.Escape, False)
        SendOneKey(System.Windows.Forms.Keys.Escape, False)
        SendOneKey(System.Windows.Forms.Keys.Escape, False)

        If Len(Message) = 0 Then Exit Sub '--ins  '11.05.27 mitsu

        FindWindowByTitle(WinTitle)
        Sleep(100)
        DoEvents()
        strMsg = UCase(Message)
        For i = 1 To Len(strMsg)
NextKey:
            strKey = Mid(strMsg, i, 1)
            Dim blnShift As Boolean
            blnShift = False
            If strKey = " " Or strKey = Chr(13) Then
                lngKeyCode = System.Windows.Forms.Keys.Return
            ElseIf strKey = "." Then
                lngKeyCode = System.Windows.Forms.Keys.Decimal
            ElseIf strKey = "+" Then
                lngKeyCode = System.Windows.Forms.Keys.Add
            ElseIf strKey = "\" Or strKey = "/" Then
                lngKeyCode = System.Windows.Forms.Keys.Divide
            ElseIf strKey = "*" Then
                lngKeyCode = System.Windows.Forms.Keys.Multiply
            ElseIf strKey = "-" Then
                lngKeyCode = System.Windows.Forms.Keys.Subtract
            ElseIf strKey = ":" Then
                lngKeyCode = 186

            ElseIf strKey = "!" Then
                lngKeyCode = System.Windows.Forms.Keys.D1
                blnShift = True
            ElseIf strKey = "#" Then
                lngKeyCode = System.Windows.Forms.Keys.D3
                blnShift = True
            ElseIf strKey = "$" Then
                lngKeyCode = System.Windows.Forms.Keys.D4
                blnShift = True
            ElseIf strKey = "%" Then
                lngKeyCode = System.Windows.Forms.Keys.D5
                blnShift = True
            ElseIf strKey = "&" Then
                lngKeyCode = System.Windows.Forms.Keys.D6
                blnShift = True
            ElseIf strKey = "'" Then
                lngKeyCode = System.Windows.Forms.Keys.D7
                blnShift = True
            ElseIf strKey = "(" Then
                lngKeyCode = System.Windows.Forms.Keys.D8
                blnShift = True
            ElseIf strKey = ")" Then
                lngKeyCode = System.Windows.Forms.Keys.D8
                blnShift = True
            ElseIf strKey = "=" Then
                lngKeyCode = 189
                blnShift = True
            ElseIf strKey = "^" Then
                lngKeyCode = 222
            ElseIf strKey = "~" Then
                lngKeyCode = 222
                blnShift = True
            ElseIf strKey = "@" Then
                lngKeyCode = 192
            ElseIf strKey = "`" Then
                lngKeyCode = 192
                blnShift = True
            ElseIf strKey = "[" Then
                lngKeyCode = 219
            ElseIf strKey = "{" Then
                lngKeyCode = 219
                blnShift = True
            ElseIf strKey = ";" Then
                lngKeyCode = 187
            ElseIf strKey = "]" Then
                lngKeyCode = 221
            ElseIf strKey = "}" Then
                lngKeyCode = 221
                blnShift = True
            ElseIf strKey = "," Then
                lngKeyCode = 188
            ElseIf strKey = "_" Then
                lngKeyCode = 226
                blnShift = True
            Else
                lngKeyCode = AscW(strKey)
                blnKeyOK = False
                If System.Windows.Forms.Keys.D0 <= lngKeyCode And lngKeyCode <= System.Windows.Forms.Keys.D9 Then blnKeyOK = True '�����Ȃ�OK
                If System.Windows.Forms.Keys.A <= lngKeyCode And lngKeyCode <= System.Windows.Forms.Keys.Z Then blnKeyOK = True '�A���t�@�x�b�g�Ȃ�OK
                If blnKeyOK = False Then
                    ' ����Ȃ��L�[������ꍇ�͏������I������
                    If Not strFileName Like "*ENG.INI" Then
                        MsgBox("�L�[�i" & strKey & "�j�𑗐M�ł��܂���B" & vbCr & "�t�@�C�����ɃX�y�[�X��Q�o�C�g�������g�p����Ă��Ȃ����m�F���Ă��������B", vbExclamation)
                    Else
                        MsgBox("A key�i" & strKey & "�jcannot be transmitted." & vbCr & "Please check whether neither the space nor the double byte character is used for the file name.", vbExclamation)
                    End If
                    Exit Sub
                End If
            End If
            SendOneKey(lngKeyCode, blnShift)
            Sleep(nSleepTime)
            DoEvents()
        Next i
        If lngKeyCode <> System.Windows.Forms.Keys.Return Then
            '�Ō�̃L�[��[Enter]�ł͂Ȃ��ꍇ
            SendOneKey(System.Windows.Forms.Keys.Return, False)
            Sleep(nSleepTime)
            DoEvents()
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '�@�\
    '   �w�肳�ꂽ�^�C�g����Window���������AActive�ɂ���
    '����
    '   WinTitle(I)�FWindow�^�C�g��
    '�߂�l
    '   ����
    '---------------------------------------------------------------------------------------------------
    Private Sub FindWindowByTitle(ByVal WinTitle As String)
        FindWinTitle = WinTitle
        Call EnumWindows(AddressOf EnumWinProc, 0&)

        If (FindWinWnd <> 0&) Then
            If SetForegroundWindow(FindWinWnd) > 0 Then
                SetActiveWindow(FindWinWnd)
            Else
                Debug.Print("SetForegroundWindow NG!")
            End If
            Sleep(nSleepTime)
        End If

    End Sub

    '---------------------------------------------------------------------------------------------------
    '�@�\
    '   Windows�̑S�n���h�����擾����
    '����
    '   API�֐� EnumWindows �ɓn�����
    '   hWnd(O) �F�R�[���o�b�N�֐��ւ̃|�C���^
    '   Param(O)�F�A�v���P�[�V������`�̒l
    '�߂�l
    '   True/False
    '---------------------------------------------------------------------------------------------------
    Public Function EnumWinProc(ByVal hwnd As Long, Param As Long) As Boolean
        'Dim lngTrd As Long  '�X���b�h
        'Dim lngPrs As Long  '�v���Z�X

        'True�̊Ԃ́AWindows�ɑ��݂���n���h�����Ō�܂Ŏ擾���悤�Ƃ���
        EnumWinProc = True

        '�q�E�B���h�E�͖�����
        If Not (GetParent(hwnd) = 0) Then GoTo PGMEND

        '�E�B���h�E�̃^�C�g�����擾
        Dim strBuf As String = ""
        Dim strTitle As String = ""
        Dim lngLen As Long
        strBuf = ""
        lngLen = GetWindowText(hwnd, strBuf, 256)
        If lngLen > 0 Then
            strTitle = Left(strBuf, lngLen)
        End If

        '�����v���Z�X���Ƃ�����
        If strTitle Like FindWinTitle & "*" Then
            '�擾���Ă����n���h�����L��
            FindWinWnd = hwnd
            '����ȏ�̃n���h���͎擾���Ȃ��ł������̂ŁAFalse���Z�b�g
            EnumWinProc = False
        End If

PGMEND:
    End Function

    '---------------------------------------------------------------------------------------------------
    '�@�\
    '   �w�肵��Window�Ƀ��b�Z�[�W�𑗐M����
    '����
    '   WinTitle(I) �F���b�Z�[�W�𑗐M����Window�̃^�C�g��
    '   Message(I)�F���M���郁�b�Z�[�W
    '�߂�l
    '   True/False
    '---------------------------------------------------------------------------------------------------
    Function Message���M(WinTitle As String, Message As String) As Boolean
        Message���M = False
        On Error GoTo ErrHandle
        SendMsg(WinTitle, Message)

        Message���M = True
        Exit Function

ErrHandle:

    End Function

    Function AutoCAD�֑��M(ScrFile As String) As Boolean
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   AutoCAD�ɽ�����̧�ق𑗐M����B
        '����
        '   ScrFile(I)�F������̧�ٖ�
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim i As Integer
        Dim cmd1 As String
        Dim buf As String, msg As String
        Dim cadWindowTitle As String
        Dim strScrSec_def As String
        Dim strScrSec As String = ""
        Dim intScrSec As Integer
        Dim PauseTime As New Timer()
        Dim strDelay As String
        Dim strFileName As String

        strFileName = FunGetLanguage()

        cadWindowTitle = FunGetCadWindowTitle()
        AutoCAD�֑��M = False

        On Error GoTo ACAD_ENABLED

        ' ������
        If nSendKeys = 0 Then
            nSendKeys = GetSendKeysMethod()
            nSleepTime = GetSleepTime()
        End If

        If nSendKeys = 2 Then
            ' keybd_event���g�p���ă��b�Z�[�W���M
            '************************************************************************************************************************
            '   SendKeys���\�b�h�́A���X�y�b�N��PC���Ɛ��������M�ł��Ȃ��ꍇ�����邽�߁A�g�p���Ȃ�
            '   API�֐� keybd_event ���g�p���āA�L�[��1�����M����
            '   ���̊֐��ł� ":"��"\"�����M�ł��Ȃ����߁A�X�N���v�g�t�@�C�����́A�p�X��g���q����菜�����t�@�C�����݂̂Ŏw�肷��
            '   �p�X�́A���O�ɃJ�����g�t�H���_��
            '************************************************************************************************************************
            '    Dim strScrPath As String
            '    Dim strScrName As String
            '    strScrPath = VBA.Left(ScrFile, InStrRev(ScrFile, "\") - 1)
            '    strScrName = VBA.Right(ScrFile, Len(ScrFile) - InStrRev(ScrFile, "\"))
            '    If StrComp(Right(strScrName, 4), ".SCR", vbTextCompare) = 0 Then
            '        strScrName = VBA.Left(strScrName, Len(strScrName) - 4)  '�g���q����菜��
            '    End If
            '    VBA.ChDir strScrPath               '�J�����g�t�H���_���X�N���v�g�t�@�C���̃p�X�ɐݒ肷��

            cmd1 = "FILEDIA 0"
            Call SendMsg(cadWindowTitle, cmd1)

            '    cmd1 = "SCRIPT " & strScrName
            cmd1 = "SCRIPT " & ScrFile
            Call SendMsg(cadWindowTitle, cmd1)

        Else
            ' SendKeys���g�p���ă��b�Z�[�W���M
            If cadWindowTitle Like "I*" Then
                'AppActivate(cadWindowTitle, False)
                'SendKeys("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}", True)
                AppActivate(cadWindowTitle)
                SendKeys.SendWait("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}")

                cmd1 = "FILEDIA " & "0" & Chr(13) & Chr(10)
                'AppActivate(cadWindowTitle, False)
                'SendKeys(cmd1, True)
                AppActivate(cadWindowTitle)
                SendKeys.SendWait(cmd1)

                'AppActivate(cadWindowTitle, False)
                'SendKeys("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}", True)
                AppActivate(cadWindowTitle)
                SendKeys.SendWait("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}")

                PauseTime.Interval = 0.5                ' ���f���Ԃ�ݒ肵�܂��B
                PauseTime.Enabled = True
                'StartTime = Timer              ' ���f�̊J�n������ݒ肵�܂��B
                'Do While Timer < StartTime + PauseTime
                'Loop

                cmd1 = "FILEDIA " & "0" & Chr(13) & Chr(10)
                'AppActivate(cadWindowTitle, False)
                'SendKeys(cmd1, True)
                AppActivate(cadWindowTitle)
                SendKeys.SendWait(cmd1)

                'AppActivate(cadWindowTitle, False)
                'SendKeys("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}", True)
                AppActivate(cadWindowTitle)
                SendKeys.SendWait("{ESC}{ESC}{ESC}{ESC}{ESC}{ESC}")

                cmd1 = "SCRIPT " & ScrFile & Chr(13) & Chr(10)
                SendKeys.SendWait(cmd1)
            Else
                strScrSec_def = "0"
                Call subGetOther("WaitingTime", "SCR", strScrSec_def, strScrSec)
                intScrSec = Int(CDbl(strScrSec) * 1000)
                strDelay = "DELAY " & intScrSec & " "

                If intScrSec > 0 Then
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys("{ESC}{ESC}{ESC}", True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait("{ESC}{ESC}{ESC}")

                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(strDelay, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(strDelay)

                    cmd1 = "FILEDIA "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    '            PauseTime = dblScrSec          ' ���f���Ԃ�ݒ肵�܂��B
                    '            StartTime = Timer              ' ���f�̊J�n������ݒ肵�܂��B
                    '            Do While Timer < StartTime + PauseTime
                    '            Loop

                    cmd1 = "0 "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    '            PauseTime = dblScrSec          ' ���f���Ԃ�ݒ肵�܂��B
                    '            StartTime = Timer              ' ���f�̊J�n������ݒ肵�܂��B
                    '            Do While Timer < StartTime + PauseTime
                    '            Loop

                    cmd1 = "FILEDIA "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    '            PauseTime = dblScrSec          ' ���f���Ԃ�ݒ肵�܂��B
                    '            StartTime = Timer              ' ���f�̊J�n������ݒ肵�܂��B
                    '            Do While Timer < StartTime + PauseTime
                    '            Loop

                    cmd1 = "0 "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    '            PauseTime = dblScrSec          ' ���f���Ԃ�ݒ肵�܂��B
                    '            StartTime = Timer              ' ���f�̊J�n������ݒ肵�܂��B
                    '            Do While Timer < StartTime + PauseTime
                    '            Loop
                Else
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys("{ESC}{ESC}{ESC}", True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait("{ESC}{ESC}{ESC}")

                    cmd1 = "FILEDIA " & "0 "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    'AppActivate(cadWindowTitle, False)
                    'SendKeys("{ESC}{ESC}{ESC}", True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait("{ESC}{ESC}{ESC}")

                    cmd1 = "FILEDIA " & "0 "
                    'AppActivate(cadWindowTitle, False)
                    'SendKeys(cmd1, True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait(cmd1)

                    'AppActivate(cadWindowTitle, False)
                    'SendKeys("{ESC}{ESC}{ESC}", True)
                    AppActivate(cadWindowTitle)
                    SendKeys.SendWait("{ESC}{ESC}{ESC}")
                End If

                cmd1 = "SCRIPT " & ScrFile & Chr(13) & Chr(10)
                'SendKeys(cmd1, True)
                SendKeys.SendWait(cmd1)
        End If
        End If

        On Error GoTo ACAD_ENABLED
        DoEvents()

        AutoCAD�֑��M = True

        Exit Function

ACAD_ENABLED:
        Beep()
        If Not strFileName Like "*ENG.INI" Then
            msg = cadWindowTitle & "��" & ScrFile & "�̓��e�𑗐M�o���܂���B" & Chr(10)
            msg = msg & cadWindowTitle & "�����s����Ă��邩�m�F���Ă��������B"
        Else
            msg = "The contents of " & ScrFile & "cannot be transmitted to " & cadWindowTitle & Chr(10)
            msg = msg & "Please check whether cad is performed."
        End If
        MsgBox(msg, vbOKOnly + vbCritical)
        Resume ERR_END

ERR_END:

    End Function

    Function SCRIPT���M(ScrFile As String, Options As String) As Boolean
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   CAD�܂���JCONSOLE�ֽ�����̧�ق𑗐M����B
        '����
        '   ScrFile(I)�F������̧�ٖ�
        '   AppName(I)�F���ع���ݖ�
        '   Lock   (I)�F���d�N���֎~�@True�F�֎~,False�F����
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim pos As Integer
        Dim msg As String
        Dim jconsole As String
        Dim strKoji As String = ""
        Dim strFileName As String

        strFileName = FunGetLanguage()

        jconsole = FunGetJconsole()

        SCRIPT���M = False
        On Error GoTo SCRIPT_ENABLED

        If jconsole = "ON" Then
            SCRIPT���M = True
            Dim sHnd As Long
            Dim AppName As String

            pos = InStrRev(ScrFile, "\")
            If pos <> 0 Then
                strKoji = Left(ScrFile, pos)
            End If
            AppName = strKoji & "JCONSOLE.EXE"
            If Dir(AppName, vbNormal) = "" Then
                AppName = FunGetBin() & "JCONSOLE.EXE"
            End If
            sHnd = Shell(AppName & " " & ScrFile & " " & Options, vbNormalFocus)
        Else
            SCRIPT���M = AutoCAD�֑��M(ScrFile)
        End If
        Exit Function

SCRIPT_ENABLED:
        Beep()
        If Not strFileName Like "*ENG.INI" Then
            msg = ScrFile & "�̓��e�𑗐M�o���܂���B" & Chr(10)
        Else
            msg = "The contents cannot be transmitted." & Chr(10)
        End If
        MsgBox(msg, vbOKOnly + vbCritical)
        Resume ERR_END

ERR_END:

    End Function

    '---------------------------------------------------------------------------------------------------
    '�@�\
    '   AutoCAD/IJCAD�Ƀ��b�Z�[�W�𑗐M����
    '����
    '   cmd�F���M���郁�b�Z�[�W
    '�߂�l
    '   �Ȃ�
    '---------------------------------------------------------------------------------------------------
    Public Sub COMMAND���M(cmd As String)
        Dim cadWindowTitle As String

        ' ������
        If nSendKeys = 0 Then
            nSendKeys = GetSendKeysMethod()
            nSleepTime = GetSleepTime()
        End If

        If nSendKeys = 2 Then
            cadWindowTitle = FunGetCadWindowTitle()
            Call SendMsg(cadWindowTitle, cmd)
        Else
            'SendKeys(cmd, True)
            SendKeys.SendWait(cmd)
        End If
    End Sub

    '---------------------------------------------------------------------------------------------------
    '�@�\
    '   AutoCAD/IJCAD�Ƀ��b�Z�[�W�𑗐M������@�����肷��
    '����
    '   �Ȃ�
    '�߂�l
    '   =1:SendKeys���g�p�A=2:keybd_event���g�p
    '---------------------------------------------------------------------------------------------------
    Private Function GetSendKeysMethod() As Long
        On Error GoTo labError
        GetSendKeysMethod = 2

        ' ���b�Z�[�W���M�̕��@�����肷��
        Dim IniFile As String
        Dim ans As New StringBuilder(MAX_CHAR)
        Dim strSendKeys As String
        Dim ret As Long

        ' Jmanage.ini����ǂݏo��
        IniFile = FunGetEnviron() & "JMANAGE.INI"

        ' �f�t�H���g��2:keybd_event���g�p
        ret = GetPrivateProfileString("Option", "SendKeys", "2", ans, MAX_CHAR, IniFile)
        'strSendKeys = Left(ans, InStr(ans, Chr(0)) - 1)
        strSendKeys = ans.ToString.Trim
        If strSendKeys = "1" Then
            GetSendKeysMethod = 1
        Else
            GetSendKeysMethod = 2
        End If

labError:
    End Function

    '---------------------------------------------------------------------------------------------------
    '�@�\
    '   AutoCAD/IJCAD�Ƀ��b�Z�[�W�𑗐M����Ƃ��̃X���[�v���Ԃ����肷��
    '����
    '   �Ȃ�
    '�߂�l
    '   �X���[�v����(ms)�i�f�t�H���g50�j
    '---------------------------------------------------------------------------------------------------
    Private Function GetSleepTime() As Long
        On Error GoTo labError
        GetSleepTime = 50

        ' ���b�Z�[�W���M�̕��@�����肷��
        Dim IniFile As String
        Dim ans As New StringBuilder(MAX_CHAR)
        Dim strSleepTime As String
        Dim ret As Long

        ' Jmanage.ini����ǂݏo��
        IniFile = FunGetEnviron() & "JMANAGE.INI"

        ' �f�t�H���g��2:keybd_event���g�p
        ret = GetPrivateProfileString("Option", "SendKeysSleepTime", "50", ans, MAX_CHAR, IniFile)
        'strSleepTime = Left(ans, InStr(ans, Chr(0)) - 1)
        strSleepTime = ans.ToString.Trim
        If Val(strSleepTime) > 0 Then
            GetSleepTime = Val(strSleepTime)
        Else
            GetSleepTime = 50
        End If
labError:
    End Function
End Module
Option Strict Off
Option Explicit On
Module WebNinsyo

    '***************************************************************************************************
    '�g�p���Ă���WindowsAPI�̐錾
    '***************************************************************************************************
    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer

    Private Const MAX_CHAR As Short = 256 '�ő啶����
	
	'////////////////////////////////////////////////////////////////////////////////
	'UPGRADE_WARNING: Structure CAS_LICENSE_HEAD may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
	Public Declare Function casGetLicenseHead Lib "casProtectLib_Jupiter.dll" (ByRef LicHead As CAS_LICENSE_HEAD) As Integer
	Public Declare Function casInitLicense Lib "casProtectLib_Jupiter.dll" (ByVal Mode As Byte, ByVal LicenseFile As String) As Integer
	
	Public Declare Function casInitLicense_PmfEdit Lib "casProtectLib_Jupiter.dll" (ByVal Mode As Byte, ByVal LicenseFile As String) As Integer
	Public Declare Function casInitLicense_General Lib "casProtectLib_Jupiter.dll" (ByVal Mode As Byte, ByVal LicenseFile As String, ByVal LicenseFileGen As String) As Integer
	
	
	
	
	''���C�Z���X�w�b�_�[
	Public Structure CAS_LICENSE_HEAD
		Dim flg As Byte ''�t���O�i=0�j
		Dim LicType As Byte ''���C�Z���X�^�C�v�i0:���[�J���A1:�l�b�g���[�N�A+100�ŕ]���Łj
		Dim SerialNo As Short ''�ʂ��ԍ�
		Dim SysCnt As Byte ''�Ǘ����C�Z���X��
		Dim Authorize As Byte ''�F�؍σt���O�i0:���F�؁A1:�F�؍ρj�� ���s���ɊǗ�APP�N������MAC�A�h���X���`�F�b�N���A�����ȏꍇ�ɔF�؍ςƂ���
		''                                �� USB�̃V���A���ԍ����`�F�b�N����ɕύX
		<VBFixedArray(1)> Dim dum() As Byte ''�\��
		'UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
		'UPGRADE_NOTE: Date was upgraded to Date_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
		<VBFixedString(8),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray,SizeConst:=8)> Public Date_Renamed() As Char ''�ŏI�X�V���iyyyymmdd�j�i�̌��ł̏ꍇ�A�L�������j
		Dim LicCode As Integer ''���C�Z���X�R�[�h�iUSB�t���b�V���������̃V���A���ԍ��j
		'UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
		<VBFixedString(28),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray,SizeConst:=28)> Public userName() As Char ''���[�U�[��
		
		'UPGRADE_TODO: "Initialize" must be called to initialize instances of this structure. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="B4BFF9E0-8631-45CF-910E-62AB3970F27B"'
		Public Sub Initialize()
			ReDim dum(1)
		End Sub
	End Structure
	
	
	
	Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer
	Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Integer, ByRef lpExitCode As Integer) As Integer
	Private Const STILL_ACTIVE As Integer = &H103s
	
	' �V�����v���Z�X�Ƃ��̃v���C�}���X���b�h���쐬����֐��̐錾
	'UPGRADE_WARNING: Structure PROCESS_INFORMATION may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
	'UPGRADE_WARNING: Structure STARTUPINFO may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
	'UPGRADE_WARNING: Structure SECURITY_ATTRIBUTES may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
	'UPGRADE_WARNING: Structure SECURITY_ATTRIBUTES may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
    Declare Function CreateProcess Lib "kernel32" Alias "CreateProcessA" (ByVal lpApplicationName As String, ByVal lpCommandLine As String, ByRef lpProcessAttributes As SECURITY_ATTRIBUTES, ByRef lpThreadAttributes As SECURITY_ATTRIBUTES, ByVal bInheritHandles As Integer, ByVal dwCreationFlags As Integer, ByRef lpEnvironment As String, ByVal lpCurrentDriectory As String, ByRef lpStartupInfo As STARTUPINFO, ByRef lpProcessInformation As PROCESS_INFORMATION) As Integer
	
	
	
	' �V�����v���Z�X�Ɋւ��鎯�ʏ����`����\����
	Structure PROCESS_INFORMATION
		Dim hProcess As Integer
		Dim hThread As Integer
		Dim dwProcessId As Integer
		Dim dwThreadId As Integer
	End Structure
	
	'   sub KickAndWait�ŃR�[������API�֐��̂��߂̐錾
	'       CreateProcess(),WaitForSingleObject()
	'
	'   VisualBasicMagazene Vol.11�����p
	'
	'============================================================���p�J�n
	' �Z�L�����e�B�����Ɋւ�������`����\����
	Structure SECURITY_ATTRIBUTES
		Dim nLength As Integer
		Dim lpSecurityDescriptor As Integer
		Dim bInheritHandle As Integer
	End Structure
	
	' �V�����v���Z�X�̃��C���E�B���h�E�̕\����Ԃ��`����\����
	Structure STARTUPINFO
		Dim cb As Integer
		Dim lpReserved As Integer
		Dim lpDesktop As Integer
		Dim lpTitle As Integer
		Dim dwX As Integer
		Dim dwY As Integer
		Dim dwXSize As Integer
		Dim dwYSize As Integer
		Dim dwXCountChars As Integer
		Dim dwYCountChars As Integer
		Dim dwFillAttribute As Integer
		Dim dwFlags As Integer
		Dim wShowWindow As Short
		Dim cbReserved2 As Short
		Dim lpReserved2 As Integer
		Dim hStdInput As Integer
		Dim hStdOutput As Integer
		Dim hStdError As Integer
	End Structure
	'
	'
	'
	'���W�X�g���̃n���h�����������
	Public Declare Function RegCloseKey Lib "ADVAPI32" (ByVal hKey As Integer) As Integer
	'���W�X�g���̃L�[���J����(�n���h���̊m�ہj
	Public Declare Function RegOpenKeyEx Lib "ADVAPI32"  Alias "RegOpenKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
	'���W�X�g���̒l���擾����
	Public Declare Function RegQueryValueExStr Lib "ADVAPI32"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByVal lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
	
	Public Const HKEY_CLASSES_ROOT As Integer = &H80000000
	Public Const HKEY_CURRENT_USER As Integer = &H80000001
	Public Const HKEY_CURRENT_CONFIG As Integer = &H80000005
	Public Const HKEY_DYN_DATA As Integer = &H80000006
	Public Const HKEY_LOCAL_MACHINE As Integer = &H80000002
	Public Const HKEY_USERS As Integer = &H80000003
	Public Const ERROR_SUCCESS As Short = 0
	
	Private Declare Function GetDesktopWindow Lib "user32.dll" () As Integer
	Private Declare Function GetWindow Lib "user32.dll" (ByVal hwnd As Integer, ByVal uCmd As Integer) As Integer
	Private Declare Function GetWindowLong Lib "user32.dll"  Alias "GetWindowLongA"(ByVal hwnd As Integer, ByVal nIndex As Integer) As Integer
	Private Declare Function IsWindow Lib "user32.dll" (ByVal hwnd As Integer) As Integer
	Private Declare Function GetWindowText Lib "user32.dll"  Alias "GetWindowTextA"(ByVal hwnd As Integer, ByVal lpString As String, ByVal nMaxCount As Integer) As Integer
	Private Declare Function IsWindowVisible Lib "user32" (ByVal hwnd As Integer) As Integer
	
	Private Const GWL_STYLE As Integer = -16
	Private Const WS_CAPTION As Integer = &HC00000
	Private Const GW_CHILD As Integer = 5
	Private Const GW_HWNDNEXT As Integer = 2
	
	
	
	Sub WebNunsyoJupiterKidou(ByRef IOFlg As Short)
		'IOFlg :0 �F��
		'      :1����
		'
		'JFE��p���[�`��
		Dim ID1 As String
		Dim ID2 As String
		Dim ID3 As String
		
		Dim userName As String
		Dim ret As Object
		'UPGRADE_WARNING: Arrays in structure LicHead may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        ' 2015/07/22 Nakagawa Edit Start
        'Dim LicHead As CAS_LICENSE_HEAD
        Dim LicHead As New CAS_LICENSE_HEAD
        ' 2015/07/22 Nakagawa Edit End
        Dim dum As New VB6.FixedLengthString(256)
		Dim Str3Moji As String
		Dim WebNinsyoFlg As String
		
		' Dim UserID As String
		
		ID1 = "00000" 'Jupiter
		ID2 = "00000" 'Jupiter�N����
		'    ID3 = "00001" 'JFE�t�B���s��
		
        ' 2015/07/22 Nakagawa Edit Start
        userName = ""
        ' 2015/07/22 Nakagawa Edit End
        Call subGetUserName(userName) '���[�U�[�� Jupiter\Environ\JManage.ini�̃��[�U�[���@NKK YBG YTI�Ȃ�
		
		WebNinsyoFlg = FuncGetWebNinsyo
		
		
		'    If UserName = "NKK" Or UserName = "NKK-WEB" Then
		'    Else
		'        Exit Sub 'JFE�o�[�W�����Ƃ��Ă͋N�����Ȃ��̂ŁA�������Ȃ��B
		'    End If
		
		'�v���e�N�g�`�b�v�̏���ǂݍ���
		
		'UPGRADE_WARNING: Couldn't resolve default property of object ret. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		ret = casInitLicense(2, dum.Value)
		'UPGRADE_WARNING: Couldn't resolve default property of object ret. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		ret = casGetLicenseHead(LicHead)
		Str3Moji = Left(LicHead.userName, 3) '�ŏ���3�����@������WEB�Ƃ����WEB�F�؂���
		ID3 = Right(Left(LicHead.userName, 8), 5) '4�`8�J�����ɂ́A���[�U�[ID������B
		
		Dim Wid As String
		'UPGRADE_NOTE: WebNinsyo was upgraded to WebNinsyo_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
		Dim WebNinsyo_Renamed As String
		Dim RootKey As Integer
		Dim Valuedate As String
		Dim length As Integer
		Dim Name As String
		Dim SubKey As String
		Dim Hensu As Integer
		Dim bPro As Boolean
		
		
		If IOFlg = 0 Then
			'�F�؂���
			bPro = True
			'<<<<2013/04/03
			'If userName = "NKK" And WebNinsyoFlg = "1" Then
			If UserNameWEB(userName) And WebNinsyoFlg = "1" Then
				'>>>>2013/04/03
				If Str3Moji = "WEB" Then
					'  "NKK-WEB" �́@WEB�F�؂���
				Else
					bPro = False '�`�b�v���_��
				End If
			ElseIf Str3Moji = "WEB" Then 
				bPro = False '�`�b�v���_��
			End If
			
			
			
			
			If bPro = False Then
				MsgBox("�v���e�N�g�`�b�v���Ⴂ�܂��B���K�̃v���e�N�g��ݒu���ĉ������B�v���O�������I�����܂��B", MsgBoxStyle.Critical)
				End
			End If
			
			'<<<<2013/04/03
			'If userName = "NKK" And WebNinsyoFlg = "1" And Str3Moji = "WEB" Then
			If UserNameWEB(userName) And WebNinsyoFlg = "1" And Str3Moji = "WEB" Then
				'>>>>2013/04/03
				On Error GoTo LA1
				WebNinsyo_Renamed = FunGetBin & "Package.exe"
				' �t�@�C�������݂��Ă��邩�ǂ����m�F����
				'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
				If Dir(WebNinsyo_Renamed) <> "" Then
					
					If Jman_Jupiter_KidouCheck = False Then
						MsgBox("Jupiter�̂Q�d�N���͂ł��܂���B", MsgBoxStyle.Critical)
						End
					End If
					'Call KickAndObserve(WebNinsyo & " 0 " & ID1 & "-" & ID2 & "-" & ID3)
					Call KickAndObserve(WebNinsyo_Renamed & " 0 " & ID1 & "-" & ID2)
					RootKey = HKEY_CURRENT_USER
					'SubKey = "Software\Newtone\NinshoRescue\NR-100VB"
					SubKey = "Software\YTI\WEBNinsyo\" & ID1 & "\" & ID2
					Name = "NinsyoOK"
					'�L�[���I�[�v�����ăn���h���𓾂�
					'UPGRADE_WARNING: Couldn't resolve default property of object ret. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					ret = RegOpenKeyEx(RootKey, SubKey, 0, 1, Hensu)
					'�o�b�t�@���m�ۂ���
					Valuedate = New String(Chr(0), 250)
					length = Len(Valuedate) '����
					'Hensu��RegOpenKeyEx�ŊJ�����L�[�̃n���h��
					'UPGRADE_WARNING: Couldn't resolve default property of object ret. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					ret = RegQueryValueExStr(Hensu, Name, 0, 0, Valuedate, length)
					'�n���h�������
					Call RegCloseKey(Hensu)
					Valuedate = Left(Valuedate, 1)
					If Trim(Valuedate) = "1" Then
					Else
						MsgBox("Web�F�؂Ɏ��s���܂����B�v���O�������I�����܂��B", MsgBoxStyle.Critical)
						End
					End If
					Exit Sub
				End If
LA1: 
				MsgBox("�Ȃ�炩�̌����Ńv���O����������ɏI�����܂���B�v���O�������I�����܂��B", MsgBoxStyle.Critical)
				End
			End If
		Else
			'�F�؉�������B
			
			
			'<<<<2013/04/03
			'If userName = "NKK" And WebNinsyoFlg = "1" And Str3Moji = "WEB" Then
			If UserNameWEB(userName) And WebNinsyoFlg = "1" And Str3Moji = "WEB" Then
				'>>>>2013/04/03
				WebNinsyo_Renamed = FunGetBin & "Package.exe"
				'Call KickAndObserve(WebNinsyo & " 1 " & ID1 & "-" & ID2 & "-" & ID3)
				Call KickAndObserve(WebNinsyo_Renamed & " 1 " & ID1 & "-" & ID2)
			End If
		End If
	End Sub
	
	Function WebNunsyoJupiterKidou_Nini(ByRef IOFlg As Short, ByRef PID2 As String) As Boolean
		'IOFlg :0 �F��
		'      :1����
		'
		'PID2:�܃P�^�̐����i�����j
		'
		'
		'JFE��p���[�`��
		WebNunsyoJupiterKidou_Nini = True
		Dim ID1 As String
		Dim ID2 As String
		Dim ID3 As String
		
		Dim userName As String
		Dim ret As Object
		'UPGRADE_WARNING: Arrays in structure LicHead may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        ' 2015/07/22 Nakagawa Edit Start
        'Dim LicHead As CAS_LICENSE_HEAD
        Dim LicHead As New CAS_LICENSE_HEAD
        ' 2015/07/22 Nakagawa Edit End
        Dim dum As New VB6.FixedLengthString(256)
		Dim dumGen As String
		Dim Str3Moji As String
		Dim WebNinsyoFlg As String
		
		' Dim UserID As String
		
		ID1 = "00000" 'Jupiter
		ID2 = PID2 'Jupiter�N����
		'    ID3 = "00001" 'JFE�t�B���s��
		
        ' 2015/07/22 Nakagawa Edit Start
        userName = ""
        ' 2015/07/22 Nakagawa Edit End
        Call subGetUserName(userName) '���[�U�[�� Jupiter\Environ\JManage.ini�̃��[�U�[���@NKK YBG YTI�Ȃ�
		
		WebNinsyoFlg = FuncGetWebNinsyo
		
		
		'    If UserName = "NKK" Or UserName = "NKK-WEB" Then
		'    Else
		'        Exit Sub 'JFE�o�[�W�����Ƃ��Ă͋N�����Ȃ��̂ŁA�������Ȃ��B
		'    End If
		
		'�v���e�N�g�`�b�v�̏���ǂݍ���
		dum.Value = ""
		dumGen = "FLGkegaki.LIC"
		'UPGRADE_WARNING: Couldn't resolve default property of object ret. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		ret = casInitLicense_General(2, dum.Value, dumGen)
		'UPGRADE_WARNING: Couldn't resolve default property of object ret. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		ret = casGetLicenseHead(LicHead)
		Str3Moji = Left(LicHead.userName, 3) '�ŏ���3�����@������WEB�Ƃ����WEB�F�؂���
		ID3 = Right(Left(LicHead.userName, 8), 5) '4�`8�J�����ɂ́A���[�U�[ID������B
		
		Dim Wid As String
		'UPGRADE_NOTE: WebNinsyo was upgraded to WebNinsyo_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
		Dim WebNinsyo_Renamed As String
		Dim RootKey As Integer
		Dim Valuedate As String
		Dim length As Integer
		Dim Name As String
		Dim SubKey As String
		Dim Hensu As Integer
		Dim bPro As Boolean
		
		
		If IOFlg = 0 Then
			'�F�؂���
			bPro = True
			'<<<<<2013/04/03
			' If userName = "NKK" And WebNinsyoFlg = "1" Then
			If UserNameWEB(userName) And WebNinsyoFlg = "1" Then
				'>>>>>2013/04/03
				If Str3Moji = "WEB" Then
					'  "NKK-WEB" �́@WEB�F�؂���
				Else
					bPro = False '�`�b�v���_��
				End If
			ElseIf Str3Moji = "WEB" Then 
				bPro = False '�`�b�v���_��
			End If
			
			
			If bPro = False Then
				MsgBox("�v���e�N�g�`�b�v���Ⴂ�܂��B���K�̃v���e�N�g��ݒu���ĉ������B�v���O�������I�����܂��B", MsgBoxStyle.Critical)
				WebNunsyoJupiterKidou_Nini = False
				Exit Function
			End If
			
			'<<<<<2013/04/03
			'If userName = "NKK" And WebNinsyoFlg = "1" And Str3Moji = "WEB" Then
			If UserNameWEB(userName) And WebNinsyoFlg = "1" And Str3Moji = "WEB" Then
				'>>>>2013/04/03
				On Error GoTo LA1
				WebNinsyo_Renamed = FunGetBin & "Package.exe"
				' �t�@�C�������݂��Ă��邩�ǂ����m�F����
				'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
				If Dir(WebNinsyo_Renamed) <> "" Then
					
					' If Jman_Jupiter_KidouCheck = False Then
					'     MsgBox "Jupiter�̂Q�d�N���͂ł��܂���B", vbCritical
					'     End
					' End If
					
					
					'Call KickAndObserve(WebNinsyo & " 0 " & ID1 & "-" & ID2 & "-" & ID3)
					Call KickAndObserve(WebNinsyo_Renamed & " 0 " & ID1 & "-" & ID2)
					RootKey = HKEY_CURRENT_USER
					'SubKey = "Software\Newtone\NinshoRescue\NR-100VB"
					SubKey = "Software\YTI\WEBNinsyo\" & ID1 & "\" & ID2
					Name = "NinsyoOK"
					'�L�[���I�[�v�����ăn���h���𓾂�
					'UPGRADE_WARNING: Couldn't resolve default property of object ret. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					ret = RegOpenKeyEx(RootKey, SubKey, 0, 1, Hensu)
					'�o�b�t�@���m�ۂ���
					Valuedate = New String(Chr(0), 250)
					length = Len(Valuedate) '����
					'Hensu��RegOpenKeyEx�ŊJ�����L�[�̃n���h��
					'UPGRADE_WARNING: Couldn't resolve default property of object ret. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					ret = RegQueryValueExStr(Hensu, Name, 0, 0, Valuedate, length)
					'�n���h�������
					Call RegCloseKey(Hensu)
					Valuedate = Left(Valuedate, 1)
					If Trim(Valuedate) = "1" Then
					Else
						MsgBox("Web�F�؂Ɏ��s���܂����B�v���O�������I�����܂��B", MsgBoxStyle.Critical)
						WebNunsyoJupiterKidou_Nini = False
					End If
					Exit Function
				End If
LA1: 
				MsgBox("�Ȃ�炩�̌����Ńv���O����������ɏI�����܂���B�v���O�������I�����܂��B", MsgBoxStyle.Critical)
				WebNunsyoJupiterKidou_Nini = False
			End If
		Else
			'�F�؉�������B
			'<<<<2013/04/03
			'If userName = "NKK" And WebNinsyoFlg = "1" And Str3Moji = "WEB" Then
			If UserNameWEB(userName) And WebNinsyoFlg = "1" And Str3Moji = "WEB" Then
				'>>>>2013/04/03
				WebNinsyo_Renamed = FunGetBin & "Package.exe"
				'Call KickAndObserve(WebNinsyo & " 1 " & ID1 & "-" & ID2 & "-" & ID3)
				Call KickAndObserve(WebNinsyo_Renamed & " 1 " & ID1 & "-" & ID2)
			End If
		End If
	End Function
	
	Private Function UserNameWEB(ByRef pUserName As String) As Boolean
		'2013/04/03 �ǉ�
		'WEB�F�؂��郆�[�U�[�����ǂ�����Ԃ�
		
		'    UserNameWEB = False
		'    If pUserName = "NKK" Or pUserName = "YBG" Or pUserName = "YTI" Then
		'        UserNameWEB = True
		'    End If
		
		UserNameWEB = True
		'2013/04/03 ���[�U�[���ɂ�����炸�Ƃ���B
		'jManage.ini�@��WebAttestation=1�̂Ƃ��͕K��WEB�F�؂Ƃ���B
		
		
	End Function
	
	
	Private Function Jman_Jupiter_KidouCheck() As Boolean
		On Error GoTo LA1
		Jman_Jupiter_KidouCheck = True
		
		Dim PrcSet As WbemScripting.SWbemObjectSet '�Q�Ɛݒ�@Microsift WMI Scription V1.2 Library
		Dim Prc As WbemScripting.SWbemObject
		Dim Locator As WbemScripting.SWbemLocator
		Dim Service As WbemScripting.SWbemServices
		Dim MesStr As String
		
		'����� jMan.exe����N�������
		Dim jManKidouNum As Short
		jManKidouNum = 0
		
        ' 2015/07/22 Nakagawa Edit Start
        MesStr = ""
        ' 2015/07/22 Nakagawa Edit End

        Locator = New WbemScripting.SWbemLocator
		Service = Locator.ConnectServer
		
		PrcSet = Service.ExecQuery("Select * From Win32_Process")
		
		For	Each Prc In PrcSet
			
			'UPGRADE_WARNING: Couldn't resolve default property of object Prc.ProcessId. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			'UPGRADE_WARNING: Couldn't resolve default property of object Prc.Description. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			MesStr = MesStr & Prc.Description & ":" & CStr(Prc.ProcessId) & vbCrLf
			
			'        Debug.Print Prc.Description
			
			'UPGRADE_WARNING: Couldn't resolve default property of object Prc.Description. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If LCase(Prc.Description) = "jman.exe" Then
				jManKidouNum = jManKidouNum + 1
			End If
			If jManKidouNum >= 2 Then
				Jman_Jupiter_KidouCheck = False
				Exit Function
				'UPGRADE_WARNING: Couldn't resolve default property of object Prc.Description. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			ElseIf LCase(Prc.Description) = "jupiter.exe" Then 
				Jman_Jupiter_KidouCheck = False
				Exit Function
			End If
		Next Prc
		
		'MsgBox "���s���A�v���P�[�V�����ł�" & vbCrLf & vbCrLf & _
		''MesStr & vbCrLf & "�ł���B"
		
		'UPGRADE_NOTE: Object Prc may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		Prc = Nothing
		'UPGRADE_NOTE: Object Service may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		Service = Nothing
		'UPGRADE_NOTE: Object Locator may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		Locator = Nothing
		
		'
		'    Dim hWnd As Long
		'    Dim lngStyle As Long
		'    Dim strTitle As String
		'
		'    Jman_Jupiter_KidouCheck = True
		'    On Error GoTo LA1
		'    hWnd = GetWindow(GetDesktopWindow(), GW_CHILD)
		'    Do While hWnd <> 0
		'        If IsWindow(hWnd) <> 0 Then
		'            If IsWindowVisible(hWnd) Then
		'                lngStyle = GetWindowLong(hWnd, GWL_STYLE)
		'                If (lngStyle And WS_CAPTION) = WS_CAPTION Then
		'                    strTitle = String(250, Chr(0))
		'                    Call GetWindowText(hWnd, strTitle, 250)
		'                    strTitle = Left(strTitle, InStr(strTitle, Chr(0)) - 1)
		'                    'Debug.Print strTitle
		'                    If Left(strTitle, 11) = "CastarJupiter" Then
		'                        Jman_Jupiter_KidouCheck = False
		'                        Exit Function
		'                    End If
		'                End If
		'            End If
		'        End If
		'        hWnd = GetWindow(hWnd, GW_HWNDNEXT)
		'    Loop
LA1: 
	End Function
	
	
	
	Function FuncGetWebNinsyo() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���հ�ޖ����擾����B
		'����
		'   UserName(O)�Fհ�ޖ�
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'հ�ޖ�
        ret = GetPrivateProfileString("WebAttestation", "WebAttestation", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			FuncGetWebNinsyo = "0"
		Else
			FuncGetWebNinsyo = Data
		End If
		
	End Function
	
	
	'
	'=========================================================
	'�@�\
	'   �t���p�X���Ŏw�肳�ꂽ�v���O���������s����
	'   �v���O�����̎��s���I������܂���۸��т��Ď�����B
	'
	'����
	'   ProgCmdLine [i]:���s����v���O�������i�t���p�X�j
	'                   �R�}���h���C�����܂�
	'
	Sub KickAndObserve(ByRef ProgCmdLine As String)
		
		Dim udtProcessInfomation As PROCESS_INFORMATION

		' �v���O���������s
		Call KickProcess(ProgCmdLine, udtProcessInfomation)
		
		'�v���O�����̓�����Ď�����
		'�v���Z�X�����s���̊ԃ��[�v����
		While SearchProcess(udtProcessInfomation.hProcess)
			System.Windows.Forms.Application.DoEvents()
		End While
		
		'(2005/02/28 �����a�L)�ǉ�
		'�I�[�v�����Ă���I�u�W�F�N�g�n���h�����N���[�Y����
		Call CloseHandle(udtProcessInfomation.hProcess)
	End Sub
	
	'=========================================================
	'�@�\
	'   �v���Z�X�n���h����{��
	'
	'����
	'   hProcess    [i]:�v���Z�X�n���h��
	'
	'�ߒl�F     True :���������i���s���j
	'          False:������Ȃ��i���s�I���j
	Private Function SearchProcess(ByRef hProcess As Integer) As Boolean
		
		Dim lpExitCode As Integer
		Dim iRet As Integer
		
		iRet = GetExitCodeProcess(hProcess, lpExitCode)
		'lpExitCode:�v���Z�X�I���R�[�h�A�I�����Ă��Ȃ��Ƃ��ɂ�STILL_ACTIVE��Ԃ�
		
		Select Case lpExitCode
			Case STILL_ACTIVE '���s��
				SearchProcess = True
			Case Else '�I��
				SearchProcess = False
		End Select
		
	End Function
	
	'=========================================================
	'�@�\
	'   �t���p�X���Ŏw�肳�ꂽ�v���O���������s����
	'
	'����
	'   strExeFileName[i]:���s����v���O�������i�t���p�X�j�R�}���h���C�����܂�
	'   udtProcessInfomation[o]:���s����ăv���Z�X�̏��
	'
	'�⑫
	'   ������strExeFileName���g����strCommandLine���L���ɂȂ�Ȃ�����
	'   strExeFileName��NULL�ɂ��āA���s�v���O�������ƃR�}���h���C��
	'   ������strCommandLine�Ɏw�肵�Ă�
	'
	'   VisualBasicMagazene Vol.11�����p
	'
	'
	'
	Private Sub KickProcess(ByRef ProgCmdLine As String, ByRef udtProcessInfomation As PROCESS_INFORMATION)
		
		Dim strExeFileName As String
		Dim strCommandLine As String
		Dim udtProcessAttributes As SECURITY_ATTRIBUTES
		Dim udtThreadAttributes As SECURITY_ATTRIBUTES
		Dim strCurrentDriectory As String
		Dim udtStartupInfo As STARTUPINFO
		Dim lngWin32apiResultCode As Integer
		
		' �v���O�������w��
		strExeFileName = vbNullString
		' �R�}���h���C�����w��
		strCommandLine = ProgCmdLine & vbNullString
		' �Z�L�����e�B�\���̂�������
		udtProcessAttributes.nLength = Len(udtProcessAttributes)
		udtThreadAttributes.nLength = Len(udtThreadAttributes)
		' �J�����g�f�B���N�g�����w��
		strCurrentDriectory = vbNullString
		' �V�����v���Z�X�̃��C���E�B���h�E�̕\����Ԃ��w��
		udtStartupInfo.cb = Len(udtStartupInfo)
		' �V�����v���Z�X���쐬
		lngWin32apiResultCode = CreateProcess(strExeFileName, strCommandLine, udtProcessAttributes, udtThreadAttributes, False, 0, vbNullString, strCurrentDriectory, udtStartupInfo, udtProcessInfomation)
		
	End Sub
End Module
Option Strict Off
Option Explicit On
Module JJmIni
	
	'***************************************************************************************************
	'�g�p���Ă���WindowsAPI�̐錾
	'***************************************************************************************************
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
	
	'***************************************************************************************************
	'հ�ް��`�萔�̐錾
    '***************************************************************************************************
    'mod by xiaoyun.z 20150408 sta
    'Private Const MAX_CHAR As Short = 256 '�ő啶����
    Private Const MAX_CHAR As Integer = 256 '�ő啶����
    'mod by xiaoyun.z 20150408 end
	
	Function FunGetAcad() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���ACAD�����߽�����擾����B
		'����
		'   ����
		'�߂�l
		'   ACAD�����߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂��ͱ��ع���݂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("�O���A�v���P�[�V����", "ACAD", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetAcad = ""
		Else
			On Error GoTo NO_DRIVE
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Normal) = "" Then
				FunGetAcad = ""
			Else
				FunGetAcad = PathName
			End If
		End If
		
		Exit Function
		
NO_DRIVE: 
		FunGetAcad = ""
		
	End Function
	Function FunGetCadWindowTitle() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���CAD��WindiwTitle���擾����B
		'����
		'   ����
		'�߂�l
		'   CAD��WindowTitle���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂��ͱ��ع���݂������Ƃ��A"AutoCAD"��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim cadWindowTitle As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("�C���X�g�[���^�C�v", "CadWindowTitle", "AutoCAD", ans.Value, MAX_CHAR, IniFile)
		cadWindowTitle = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetCadWindowTitle = cadWindowTitle
		
		Exit Function
	End Function
	Function FunGetJconsole() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���Jconsole=ON/OFF(�ȗ���)���擾����B
		'����
		'   ����
		'�߂�l
		'   JCONSOLE�łŎ��s���邩�ۂ��B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A"OFF"��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim jconsole As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("�C���X�g�[���^�C�v", "JCONSOLE", "OFF", ans.Value, MAX_CHAR, IniFile)
		jconsole = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetJconsole = jconsole
		
		Exit Function
	End Function
	
	Function FunGetRegEditProfiles() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���RegEdit��Profiles�p�X���擾����B
		'����
		'   ����
		'�߂�l
		'   RegEdit��Profiles�p�X
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂��ͱ��ع���݂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim regEditProfiles As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("�C���X�g�[���^�C�v", "RegEditProfiles", "", ans.Value, MAX_CHAR, IniFile)
		regEditProfiles = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetRegEditProfiles = regEditProfiles
		
		Exit Function
	End Function
	
	
	Function FunGetAutoCadVersion() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���AutoCAD�̃o�[�W�������擾����B
		'����
		'   ����
		'�߂�l
		'   AutoCAD�̃o�[�W����
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂��ͱ��ع���݂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim autoCadVersion As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("�C���X�g�[���^�C�v", "AutoCAD", "", ans.Value, MAX_CHAR, IniFile)
		autoCadVersion = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetAutoCadVersion = autoCadVersion
		
		Exit Function
		
	End Function
	
	Function FunGetAccess() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���ACCESS�����߽�����擾����B
		'����
		'   ����
		'�߂�l
		'   ACCESS�����߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂��ͱ��ع���݂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("�O���A�v���P�[�V����", "ACCESS", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetAccess = ""
		Else
			On Error GoTo NO_DRIVE
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Normal) = "" Then
				FunGetAccess = ""
			Else
				FunGetAccess = PathName
			End If
		End If
		
		Exit Function
		
NO_DRIVE: 
		FunGetAccess = ""
		
	End Function
	
	Function FunGetActiveData() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���[ACTIVE_DATA]�̍H�������擾����B
		'����
		'   ����
		'�߂�l
		'   [ACTIVE_DATA]�̍H�����B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("ACTIVE_DATA", "KOGO", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetActiveData = ""
		Else
			FunGetActiveData = PathName
		End If
		
	End Function
	
	Function FunGetApp(ByRef AppName As String) As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���APP[*]�̓��AAppName�ɊY��������ع���݂����߽�����擾����B
		'����
		'   AppName(I)�F���ع���̷݂�ܰ��
		'�߂�l
		'   AppName�����߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂��ͱ��ع���݂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim i, pos As Short
		Dim AppCnt As Short
		Dim PathName, AppKey As String
		
		FunGetApp = ""
		
		AppCnt = FunGetAppCnt()
		
		For i = 0 To AppCnt - 1
			PathName = FunGetAppNo(i)
			
			pos = InStr(PathName, ":")
			If pos > 0 Then
				AppKey = Left(PathName, pos - 1)
				If UCase(AppKey) = UCase(AppName) Then
					If fun�����񒊏o(PathName, "[", "]", PathName) = True Then
						On Error GoTo NO_DRIVE
						
						'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
						If Dir(PathName, FileAttribute.Normal) = "" Then
							FunGetApp = ""
						Else
							FunGetApp = PathName
						End If
						Exit For
					End If
				End If
			End If
		Next i
		
		Exit Function
		
NO_DRIVE: 
		FunGetApp = ""
		
	End Function
	
	Function FunGetAppCnt() As Short
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă�����ع���݂̐ݒ萔���擾����B
		'����
		'   ����
		'�߂�l
		'   AppCount���ް��B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A0��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("�O���A�v���P�[�V����", "AppCount", "0", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetAppCnt = CShort(Data)
		
	End Function
	
	Function FunGetAppNo(ByRef No As Short) As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă�����ع���݂̐ݒ���擾����B
		'����
		'   No(I)�F���ع���݂̐ݒ�ԍ�
		'�߂�l
		'   ���ع���̷݂������߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile, AppNo As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		AppNo = "App[" & No & "]"
		
		ret = GetPrivateProfileString("�O���A�v���P�[�V����", AppNo, "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetAppNo = ""
		Else
			FunGetAppNo = PathName
		End If
		
	End Function
	
	Function FunGetBin() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ���ع���݂��i�[����Ă���BIN�ިڸ�؂�ݒ肷��B
		'����
		'   ����
		'�߂�l
		'   BIN�����߽���B
		'   �ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "BIN\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetBin = ""
		Else
			FunGetBin = PathName
		End If
		
	End Function
	
	Function FunGetCopyListFile() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���[CopyListFile]���ް����擾����B
		'   �����ް��́A�V�K�H�����쐬����Ƃ������ިڸ�؂ɺ�߰����̧�ٖ����L�q���Ă���B
		'����
		'   ����
		'�߂�l
		'   [CopyListFile]�����߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂���̧�ق������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data, PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("CopyListFile", "FileName", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			FunGetCopyListFile = ""
		Else
			PathName = FunGetGensun() & Data
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Normal) = "" Then
				FunGetCopyListFile = ""
			Else
				FunGetCopyListFile = PathName
			End If
		End If
		
	End Function
	
	Function FunGetDcl() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   AutoCAD���޲�۸�̧�ق��i�[����Ă���DCL�ިڸ�؂�ݒ肷��B
		'����
		'   ����
		'�߂�l
		'   DCL�����߽���B
		'   �ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "DCL\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetDcl = ""
		Else
			FunGetDcl = PathName
		End If
		
	End Function
	
	Function FunGetEnviron() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   �ݒ�̧�ق��i�[����Ă���ENVIRON�ިڸ�؂�ݒ肷��B
		'����
		'   ����
		'�߂�l
		'   ENVIRON�����߽���B
		'   �ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "ENVIRON\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetEnviron = ""
		Else
			FunGetEnviron = PathName
		End If
		
	End Function
	
	Function FunGetExcel() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���EXCEL�����߽�����擾����B
		'����
		'   ����
		'�߂�l
		'   EXCEL�����߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂��ͱ��ع���݂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("�O���A�v���P�[�V����", "EXCEL", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetExcel = ""
		Else
			On Error GoTo NO_DRIVE
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Normal) = "" Then
				FunGetExcel = ""
			Else
				FunGetExcel = PathName
			End If
		End If
		
		Exit Function
		
NO_DRIVE: 
		FunGetExcel = ""
		
	End Function
	
	Function FunGetForm() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ���[����̧�ق��i�[����Ă���FORM�ިڸ�؂�ݒ肷��B
		'����
		'   ����
		'�߂�l
		'   FORM�����߽���B
		'   �ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "FORM\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetForm = ""
		Else
			FunGetForm = PathName
		End If
		
	End Function
	
	Function FunGetGensun() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ���ϐ�GENSUN_DIR���擾����B�߽���̍Ō��\�������ꍇ�͒ǉ�����B
		'����
		'   ����
		'�߂�l
		'   GENSUN_DIR���߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂����ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = Environ("GENSUN_DIR")
		
		If PathName = "" Then
			FunGetGensun = ""
		Else
			If Mid(PathName, Len(PathName), 1) <> "\" Then
				PathName = PathName & "\"
			End If
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Directory) = "" Then
				FunGetGensun = ""
			Else
				FunGetGensun = PathName
			End If
		End If
		
	End Function
	
	Function FunGetIcon() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ����̧�ق��i�[����Ă���ICON�ިڸ�؂�ݒ肷��B
		'����
		'   ����
		'�߂�l
		'   ICON�����߽���B
		'   �ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "ICON\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetIcon = ""
		Else
			FunGetIcon = PathName
		End If
		
	End Function
	
	Function FunGetJlog() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ���엚��̧�ق��i�[����Ă���JLOG�ިڸ�؂�ݒ肷��B
		'����
		'   ����
		'�߂�l
		'   JLOG�����߽���B
		'   �ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "JLOG\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetJlog = ""
		Else
			FunGetJlog = PathName
		End If
		
	End Function
	
	Function FunGetKoji() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���H�������߽�����擾����B
		'   �H���ިڸ�؂́A[�V�K�H��]��[ACTIVE_DATA]�����킹���߽���ƂȂ�B
		'����
		'   ����
		'�߂�l
		'   �H�������߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂����ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName, KogoName As String
		
		PathName = FunGetNewKoji()
		If PathName = "" Then
			FunGetKoji = ""
			Exit Function
		End If
		
		KogoName = FunGetActiveData()
		If KogoName = "" Then
			FunGetKoji = ""
		Else
			PathName = PathName & KogoName & "\"
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Directory) = "" Then
				FunGetKoji = ""
			Else
				FunGetKoji = PathName
			End If
		End If
		
	End Function
	
	Function FunGetMenu() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   �ƭ�̧�ق��i�[����Ă���MENU�ިڸ�؂�ݒ肷��B
		'����
		'   ����
		'�߂�l
		'   MENU�����߽���B
		'   �ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "MENU\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetMenu = ""
		Else
			FunGetMenu = PathName
		End If
		
	End Function
	
	Function FunGetNewKoji() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���[�V�K�H��]���߽�����擾����B
		'����
		'   ����
		'�߂�l
		'   [�V�K�H��]���߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂����ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("�V�K�H��", "PATH", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If PathName = "NO_KEY" Then
			FunGetNewKoji = ""
		Else
			If Mid(PathName, Len(PathName), 1) <> "\" Then
				PathName = PathName & "\"
			End If
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Directory) = "" Then
				FunGetNewKoji = ""
			Else
				FunGetNewKoji = PathName
			End If
		End If
		
	End Function

    ' 2015/0/916 Nakagawa Add Start
    Function FunGetNewKoji2() As String
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   JMANAGE.INI�ɐݒ肳��Ă���[�V�K�H��]���߽�����擾����B
        '����
        '   ����
        '�߂�l
        '   [�V�K�H��]���߽���B
        '   �ݒ肳��Ă��Ȃ��Ƃ�""��Ԃ��B(�H���t�H���_���Ƀt�H���_������Ȃ��Ă��悢
        '---------------------------------------------------------------------------------------------------
        Dim IniFile As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim PathName As String
        Dim ret As Integer

        IniFile = FunGetEnviron() & "JMANAGE.INI"

        ret = GetPrivateProfileString("�V�K�H��", "PATH", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
        PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)

        If PathName = "NO_KEY" Then
            FunGetNewKoji2 = ""
        Else
            If Mid(PathName, Len(PathName), 1) <> "\" Then
                PathName = PathName & "\"
            End If

            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FunGetNewKoji2 = PathName
        End If

    End Function
    ' 2015/0/916 Nakagawa Add End

	Function FunGetSystemFile() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���[SystemFile]���ް����擾����B
		'   �����ް��́A̧�ق̗L���ɂ������ۯ��𔻒f���邩�����肷��B
		'����
		'   ����
		'�߂�l
		'   [SystemFile]�����߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂���̧�ق������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data, PathName As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("SystemFile", "FileName", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			FunGetSystemFile = ""
		Else
			PathName = FunGetGensun() & Data
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Normal) = "" Then
				FunGetSystemFile = ""
			Else
				FunGetSystemFile = PathName
			End If
		End If
		
	End Function
	
	
	Function FunGetWaitingTime() As Short
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���[WaitingTime]���ް����擾����B
		'   �����ް��́AJupiter�Ȱ�ެ��Ɠ�����AutoCAD�����s����Ƃ���AutoCAD�̑҂����Ԃ�ݒ肵�Ă���B
		'����
		'   ����
		'�߂�l
		'   [WaitingTime]���ް��B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A0��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString("WaitingTime", "SEC", "0", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		FunGetWaitingTime = CShort(Data)
		
	End Function
	
	Sub subGetFtp(ByRef SendOn As String, ByRef User As String, ByRef Pass As String)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���[FTP]�̓]������ڽ�Aհ�ް���A�߽ܰ�ނ��擾����B
		'����
		'   SendOn(O)�F�]������ڽ
		'   User(O)�Fհ�ް��
		'   Pass(O)�F�߽ܰ��
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'�]������ڽ
		ret = GetPrivateProfileString("FTP", "SendOn", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			SendOn = ""
		Else
			SendOn = Data
		End If
		
		'հ�ް��
		ret = GetPrivateProfileString("FTP", "User", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			User = ""
		Else
			User = Data
		End If
		
		'�߽ܰ��
		ret = GetPrivateProfileString("FTP", "Pass", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			Pass = ""
		Else
			Pass = Data
		End If
		
	End Sub
	
	Sub subSetActiveData(ByRef Kogo As String)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI��[ACTIVE_DATA]�ɍH������ݒ肷��B
		'����
		'   Kogo(I)�F�H����
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'�H����
		ret = WritePrivateProfileString("ACTIVE_DATA", "KOGO", Kogo, IniFile)
		
	End Sub
	
	Sub subSetFtp(ByRef User As String, ByRef Pass As String)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI��[FTP]��հ�ް���A�߽ܰ�ނ�ݒ肷��B
		'����
		'   User(I)�Fհ�ް��
		'   Pass(I)�F�߽ܰ��
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'հ�ް��
		ret = WritePrivateProfileString("FTP", "User", User, IniFile)
		
		'�߽ܰ��
		ret = WritePrivateProfileString("FTP", "Pass", Pass, IniFile)
		
	End Sub
	
	Sub subSetReportFile(ByRef fname As String)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI��[���[]�ɑI��̧�ق�ݒ肷��B
		'����
		'   Fname(I)�F�I��̧��
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'�I��̧��
		ret = WritePrivateProfileString("���[", "�I��̧��", fname, IniFile)
		
	End Sub
	
	Sub subSetReportKanri(ByRef Kanri As String)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI��[���[]�ɑI���Ǘ�����ݒ肷��B
		'����
		'   Kanri(I)�F�I���Ǘ���
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'�I���Ǘ���
		ret = WritePrivateProfileString("���[", "�I���Ǘ���", Kanri, IniFile)
		
	End Sub
	
	Sub subSetReportPaper(ByRef Size As String, ByRef Orientation As String)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI��[���[]���߰�߰���ށA�c��������ݒ肷��B
		'����
		'   Size(I)�F�߰�߰����
		'   Orientation(I)�F�c������
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'�߰�߰����
		ret = WritePrivateProfileString("���[", "PaperSize", Size, IniFile)
		
		'�c������
		ret = WritePrivateProfileString("���[", "PaperOrientation", Orientation, IniFile)
		
	End Sub
	
	Sub subSetReportType(ByRef Rtype As String)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI��[���[]�ɒ��[���߂�ݒ肷��B
		'����
		'   Rtype(I)�F���[����
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'���[����
		ret = WritePrivateProfileString("���[", "�I�𒠕[", Rtype, IniFile)
		
	End Sub
	
	Sub subSetPaperSize(ByRef Size As String)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI��[SystemPrinter]���߰�߰���ނ�ݒ肷��B
		'����
		'   Size(I)�F�߰�߰����
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'�߰�߰����
		ret = WritePrivateProfileString("SystemPrinter", "PaperSize", Size, IniFile)
		
	End Sub
	
	Sub subGetPaperSize(ByRef Size As String)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI��[SystemPrinter]���߰�߰���ނ��擾����B
		'����
		'   Size(O)�F�߰�߰����
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'�������̗p������
		ret = GetPrivateProfileString("SystemPrinter", "PaperSize", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			Size = "A4"
		Else
			Size = Data
		End If
		
	End Sub
	
	Sub subGetUserName(ByRef userName As String)
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
		ret = GetPrivateProfileString("���[�U��", "���[�U��", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			userName = ""
		Else
			userName = Data
		End If
		
	End Sub
	
	Function FunGetCasClient() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ���ϐ�CASTAR_CLIENT���擾����B�߽���̍Ō��\�������ꍇ�͒ǉ�����B
		'����
		'   ����
		'�߂�l
		'   CASTAR_CLIENT���߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂����ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = Environ("CASTAR_CLIENT")
		
		If PathName = "" Then
			FunGetCasClient = ""
		Else
			If Mid(PathName, Len(PathName), 1) <> "\" Then
				PathName = PathName & "\"
			End If
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Directory) = "" Then
				FunGetCasClient = ""
			Else
				FunGetCasClient = PathName
			End If
		End If
		
	End Function
	
	Function FunGetCasUser() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ���ϐ�CASTAR_USER���擾����B�߽���̍Ō��\�������ꍇ�͒ǉ�����B
		'����
		'   ����
		'�߂�l
		'   CASTAR_USER���߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂����ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = Environ("CASTAR_USER")
		
		If PathName = "" Then
			FunGetCasUser = ""
		Else
			If Mid(PathName, Len(PathName), 1) <> "\" Then
				PathName = PathName & "\"
			End If
			
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(PathName, FileAttribute.Directory) = "" Then
				FunGetCasUser = ""
			Else
				FunGetCasUser = PathName
			End If
		End If
		
	End Function
	
	Function FunGetCasBin() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   CASTAR���ع���݂��i�[����Ă���BIN�ިڸ�؂�ݒ肷��B
		'����
		'   ����
		'�߂�l
		'   BIN�����߽���B
		'   �ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetCasClient() & "BIN\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetCasBin = ""
		Else
			FunGetCasBin = PathName
		End If
		
	End Function
	
	Sub subGetOther(ByRef strAppName As String, ByRef strKeyName As String, ByRef strDefault As String, ByRef strOutData As String)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă����ް����擾����B
		'����
		'   strAppName(I)�F���ع���ݖ�
		'   strKeyName(I)�F����
		'   strDefault(I)�F���ع���ݖ��A�����������Ƃ��̏����l
		'   strOutData(O)�F�ް�
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		ret = GetPrivateProfileString(strAppName, strKeyName, "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			strOutData = strDefault
		Else
			strOutData = Data
		End If
		
	End Sub
	
	Function FunGetInputBin() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ���̓V�X�e�����i�[����Ă���BIN�ިڸ�؂�ݒ肷��B
		'����
		'   ����
		'�߂�l
		'   BIN�����߽���B
		'   �ިڸ�؂������Ƃ��A""��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim PathName As String
		
		PathName = FunGetGensun() & "JP_InputData\"
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(PathName, FileAttribute.Directory) = "" Then
			FunGetInputBin = ""
		Else
			FunGetInputBin = PathName
		End If
		
	End Function
	
	' 2012-4-17 add by c.wu:
	Function FunGetLanguage() As String
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���[Language]�̖����擾����B
		'����
		'   ����
		'�߂�l
		'   [Language]���߽���B
		'   �ݒ肳��Ă��Ȃ��Ƃ��A�܂����ިڸ�؂������Ƃ��A"JPN"��Ԃ��B
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim PathName, strEnviron As String
		Dim ret As Integer
		
		strEnviron = FunGetEnviron()
        IniFile = strEnviron & "JMANAGE.INI"

        ret = GetPrivateProfileString("Language", "Language", "JPN", ans.Value, MAX_CHAR, IniFile)

        PathName = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
        PathName = strEnviron & PathName & ".ini"
        FunGetLanguage = StrConv(PathName, VbStrConv.Uppercase)
		
	End Function
	
	Function FunGetTransLate(ByRef strAppName As String, ByRef strKeyName As String, ByRef strDefault As String) As String
		
		Dim ans As New VB6.FixedLengthString(256)
		Dim strFileName As String
		Dim ret As Integer
		
		strFileName = FunGetLanguage()
		
		If Not strFileName Like "*JPN.INI" Then
			ret = GetPrivateProfileString(strAppName, strKeyName, strDefault, ans.Value, 256, strFileName)
			FunGetTransLate = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		Else
			FunGetTransLate = strDefault
		End If
	End Function
	
	Function FunJudgeJpnFlg() As Integer
		Dim strFileName As String
		strFileName = FunGetLanguage()
		
		If Not strFileName Like "*JPN.INI" Then
			'�p��łƂ��Ă�ENG.INI���Ȃ���Γ��{��łƂ���
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(FunGetEnviron() & "ENG.INI", FileAttribute.Normal) = "" Then
				FunJudgeJpnFlg = 0
			Else
				FunJudgeJpnFlg = 1
			End If
		Else
			FunJudgeJpnFlg = 0
		End If
	End Function
	
	Function FuncGetOptionFunction() As String
		'2014/06/04 simo �ǉ�
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JMANAGE.INI�ɐݒ肳��Ă���OptionFunction���擾����B
		'����
		'   ����
		'�߂�l
		'   YBCFunction
		'---------------------------------------------------------------------------------------------------
		Dim IniFile As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim Data As String
		Dim ret As Integer
		
		IniFile = FunGetEnviron() & "JMANAGE.INI"
		
		'հ�ޖ�
		ret = GetPrivateProfileString("OptionFunction", "OptionFunction", "NO_KEY", ans.Value, MAX_CHAR, IniFile)
		Data = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If Data = "NO_KEY" Then
			FuncGetOptionFunction = "0"
		Else
			FuncGetOptionFunction = Trim(Data)
		End If
		
    End Function

    '2015/07/23 Nakagawa Add Start
    Sub subJmIniSetApp(kind As String, exeName As String)
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   JMANAGE.INI��[�O���A�v���P�[�V����]��ACAD�̃p�X��ݒ肷��B
        '����
        '   kind(I)�F���(ACAD,EXCEL,ACCESS)
        '   exeName(I)�F�p�X
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim IniFile As String
        Dim ret As Integer

        IniFile = FunGetEnviron() & "JMANAGE.INI"
        ret = WritePrivateProfileString("�O���A�v���P�[�V����", kind, exeName, IniFile)

    End Sub

    Sub subJmIniSetAppNo(no As Integer, exeName As String)
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   JMANAGE.INI��[�O���A�v���P�[�V����]��ACAD�̃p�X��ݒ肷��B
        '����
        '   no(I)�F�A�v���P�[�V�����ԍ�(0:Editor,1:FTP)
        '   exeName(I)�F�p�X
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim IniFile As String
        Dim ret As Integer

        IniFile = FunGetEnviron() & "JMANAGE.INI"
        Dim appNo As String = "App[" & no & "]"
        ret = WritePrivateProfileString("�O���A�v���P�[�V����", appNo, exeName, IniFile)

    End Sub

    Sub subJmIniSetSection(apName As String, keyName As String, param As String)
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   JMANAGE.INI�Ɏw�肵�����e����������
        '����
        '   apName(I)�F�Z�N�V������
        '   keyName(I)�F���ږ�
        '   param(I)�F�l
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim IniFile As String
        Dim ret As Integer

        IniFile = FunGetEnviron() & "JMANAGE.INI"
        ret = WritePrivateProfileString(apName, keyName, param, IniFile)

    End Sub
    '2015/07/23 Nakagawa Add End

End Module
Option Strict Off
Option Explicit On
Module JPmfBmf
	
	'***************************************************************************************************
	'�g�p���Ă���WindowsAPI�̐錾
	'***************************************************************************************************
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	
	'***************************************************************************************************
	'հ�ް��`�萔�̐錾
	'***************************************************************************************************
	Private Const MAX_CHAR As Short = 256 '�ő啶����
	
	'***************************************************************************************************
	'հ�ް��`�\���̂̐錾
	'***************************************************************************************************
	'---------------------------------------------------------------------------------------------------
	'2�������ށA2������ۯ���Ͻ��̧�ٖ������p�\����
	'---------------------------------------------------------------------------------------------------
	Structure MAST
		Dim fcnt As Short '������
		Dim fname() As String 'Ͻ��̧�ٖ�(�����Đ錾����)
		Dim fcode() As String '̧�ٕϊ���(�����Đ錾����)
	End Structure
	
	Function funINI���猟�������擾(ByRef IniFile As String) As Short
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   Ͻ��̧�ٖ�����̧�ق��猟�������擾����B
		'����
		'   IniFile(I)�FϽ��̧�ٖ�����̧�ٖ�(���߽��)
		'�߂�l
		'   �������FϽ��̧�ٖ�����̧�قɋL�q����Ă��錟�����B
		'           Ͻ��̧�ٖ�����̧�قɌ������̷�ܰ�ނ������Ƃ��A0�Ƃ���B
		'---------------------------------------------------------------------------------------------------
		Dim cnt As Short
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim buf As String
		Dim ret As Integer
		
		ret = GetPrivateProfileString("TransTable", "count", "0", ans.Value, MAX_CHAR, IniFile)
		buf = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		cnt = CShort(buf)
		
		funINI���猟�������擾 = cnt
		
	End Function
	
	Function funINI���猟���e�[�u�����擾(ByRef IniFile As String, ByRef id As Short, ByRef fname As String, ByRef fcode As String) As Boolean
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   Ͻ��̧�ٖ�����̧�ق��猟������Ͻ��̧�ٖ���̧�ٕϊ������擾����B
		'����
		'   IniFile(I)�FϽ��̧�ٖ�����̧�ٖ�(���߽��)
		'   id(I)�F�擾����ð��ٔԍ�
		'   fname(O)�FϽ��̧�ٖ�
		'   fcode(O)�F̧�ٕϊ���
		'�߂�l
		'   True�FϽ��̧�ٖ���̧�ٕϊ����𐳂����擾�ł����B
		'   False�FϽ��̧�ٖ���̧�ٕϊ������擾�ł��Ȃ������B
		'---------------------------------------------------------------------------------------------------
		Dim pos As Short
		Dim st As String
		Dim ans As New VB6.FixedLengthString(MAX_CHAR)
		Dim buf, msg As String
		Dim ret As Integer
		Dim strFileName As String
		
		strFileName = FunGetLanguage()
		
		st = "Trans" & CStr(id)
		ret = GetPrivateProfileString("TransTable", st, "*Undefined", ans.Value, MAX_CHAR, IniFile)
		buf = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
		
		If buf = "*Undefined" Then
			funINI���猟���e�[�u�����擾 = False
			Exit Function
		End If
		
		pos = InStr(buf, " ")
		If pos = 0 Then
			Beep()
			If Not strFileName Like "*ENG.INI" Then
				msg = "Ͻ��̧�ٖ�����̧��(" & IniFile & ")�̓��e�����������ł��B" & Chr(10)
				msg = msg & "Ͻ��̧�ٖ���̧�ٕϊ����̊Ԃ����ݸ��؂肪�����\��������܂��B" & Chr(10)
				msg = msg & "̧�ٓ��e���m�F���A�������C�����ĉ������B" & Chr(10)
				msg = msg & "���e �F " & buf
			Else
				msg = "The contents of a master file name search file(" & IniFile & ")are amusing." & Chr(10)
				msg = msg & "There may be no blank pause between a master file name and a file conversion name." & Chr(10)
				msg = msg & "Please check the contents of a file and correct correctly." & Chr(10)
				msg = msg & "Contents �F " & buf
			End If
			MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
			funINI���猟���e�[�u�����擾 = False
			Exit Function
		End If
		
		fname = Trim(Left(buf, pos - 1))
		fcode = Trim(Right(buf, Len(buf) - pos))
		
		funINI���猟���e�[�u�����擾 = True
		
	End Function
	
	Sub SubINI����f�[�^���擾(ByRef IniFile As String, ByRef Mast_name As MAST)
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   Ͻ��̧�ٖ�����̧�ق��猟�����A��������Ͻ��̧�ٖ���̧�ٕϊ�����Ͻ��̧�ٖ������p�\���̂Ɋi�[����B
		'����
		'   IniFile(I)�FϽ��̧�ٖ�����̧�ٖ�(���߽��)
		'   mast_name(O)�FϽ��̧�ٖ������p�\����
		'�߂�l
		'   ����
		'---------------------------------------------------------------------------------------------------
		Dim i, cnt As Short
		Dim fname, fcode As String
		
		'���������擾����B
		Mast_name.fcnt = funINI���猟�������擾(IniFile)

		'����������Ͻ��̧�ٖ���̧�ٕϊ����̔z����m�ۂ���B
		ReDim Mast_name.fname(Mast_name.fcnt)
		ReDim Mast_name.fcode(Mast_name.fcnt)
		
		'Ͻ��̧�ٖ���̧�ٕϊ������擾����B
		cnt = 0
		For i = 1 To Mast_name.fcnt
			If funINI���猟���e�[�u�����擾(IniFile, i, fname, fcode) = True Then
				cnt = cnt + 1
				Mast_name.fname(cnt) = fname
				Mast_name.fcode(cnt) = fcode
			End If
		Next i
		
		'Ͻ��̧�ٌ���̧�ق��琳�����ǂݍ��߂����ɂ���B
		Mast_name.fcnt = cnt
		
	End Sub
	
	Function funBMF�t�@�C���̌���(ByRef dname As String, ByRef obj As Object, ByRef bname As String) As Short
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JBMF.INI���猟������2������ۯ�Ͻ��̧�ٖ���̧�ٕϊ������擾���A
		'   �H���ިڸ�؂���擾����2������ۯ�Ͻ��̧��(BmfMast)���������A��ʂ̺����ޯ���ɒǉ�����B
		'����
		'   dname(I)�F�H���ިڸ�ؖ�
		'   obj(I)�F�����ޯ���̵�޼ު�Ė��i̫�і�!���ޖ��j
		'   bname(I)�F�����ޯ���̏����I������2������ۯ�Ͻ��̧�ٖ��B�K���啶���œn�����B
		'�߂�l
		'   ������
		'     0�F�H���ިڸ�؂�2������ۯ�Ͻ��̧�ق�1�����݂��Ȃ��B
		'     1�ȏ�F�H���ިڸ�؂�2������ۯ�Ͻ��̧�ق����݂���B���̐��l��̧�ِ���\���B
		'---------------------------------------------------------------------------------------------------
		Dim cnt_bmf, i, idx_bmf As Short
		Dim IniFile As String
		Dim msg As String
		'UPGRADE_WARNING: Arrays in structure BmfMast may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim BmfMast As MAST
		Dim strFileName As String
		
		strFileName = FunGetLanguage()
		
		'JBMF.INI���猟������2������ۯ�Ͻ��̧�ٖ���̧�ٕϊ������擾����B
		IniFile = FunGetEnviron() & "JBMF.INI"
		Call SubINI����f�[�^���擾(IniFile, BmfMast)
		
		cnt_bmf = 0
		idx_bmf = 0
		'UPGRADE_WARNING: Couldn't resolve default property of object obj.Clear. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        obj.items.Clear()
		
		For i = 1 To BmfMast.fcnt
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(dname & BmfMast.fname(i), FileAttribute.Normal) <> "" Then
				'UPGRADE_WARNING: Couldn't resolve default property of object obj.AddItem. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                obj.items.add(BmfMast.fname(i) & " <" & BmfMast.fcode(i) & ">")
				'�����I���Ƃ��邽�߁A�����ޯ���ɒǉ������ʒu���m�ۂ���B
				If UCase(BmfMast.fname(i)) = bname Then
					idx_bmf = cnt_bmf
				End If
				cnt_bmf = cnt_bmf + 1
			End If
		Next i
		
		If cnt_bmf = 0 Then
			If Not strFileName Like "*ENG.INI" Then
				msg = "�w�肵���H���ɂ́A2������ۯ�Ͻ��̧�ق�1������܂���B" & Chr(10)
			Else
				msg = "There is not a BMF master file, either." & Chr(10)
			End If
			MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
			funBMF�t�@�C���̌��� = cnt_bmf
		Else
			funBMF�t�@�C���̌��� = cnt_bmf
			'UPGRADE_WARNING: Couldn't resolve default property of object obj.ListIndex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            obj.selectedIndex = idx_bmf
		End If
		
	End Function
	
	Function funPMF�t�@�C���̌���(ByRef dname As String, ByRef obj As Object, ByRef pname As String) As Short
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   JPMF.INI���猟������2��������Ͻ��̧�ٖ���̧�ٕϊ������擾���A
		'   �H���ިڸ�؂���擾����2��������Ͻ��̧��(PmfMast)���������A��ʂ̺����ޯ���ɒǉ�����B
		'����
		'   dname(I)�F�H���ިڸ�ؖ�
		'   obj(I)�F�����ޯ���̵�޼ު�Ė��i̫�і�!���ޖ��j
		'   pname(I)�F�����ޯ���̏����I������2��������Ͻ��̧�ٖ��B�K���啶���œn�����B
		'�߂�l
		'   ������
		'     0�F�H���ިڸ�؂�2��������Ͻ��̧�ق�1�����݂��Ȃ��B
		'     1�ȏ�F�H���ިڸ�؂�2��������Ͻ��̧�ق����݂���B���̐��l��̧�ِ���\���B
		'---------------------------------------------------------------------------------------------------
		Dim cnt_pmf, i, idx_pmf As Short
		Dim IniFile As String
		Dim msg As String
		'UPGRADE_WARNING: Arrays in structure PmfMast may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim PmfMast As MAST
		Dim strFileName As String
		
		strFileName = FunGetLanguage()
		
		'JPMF.INI���猟������2��������Ͻ��̧�ٖ���̧�ٕϊ������擾����B
		IniFile = FunGetEnviron() & "JPMF.INI"
        Call SubINI����f�[�^���擾(IniFile, PmfMast)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object obj.ListCount. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        cnt_pmf = obj.items.count
		idx_pmf = 0
		
        For i = 1 To PmfMast.fcnt
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Dir(dname & PmfMast.fname(i), FileAttribute.Normal) <> "" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object obj.AddItem. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                obj.Items.Add(PmfMast.fname(i) & " <" & PmfMast.fcode(i) & ">")
                '�����I���Ƃ��邽�߁A�����ޯ���ɒǉ������ʒu���m�ۂ���B
                If UCase(PmfMast.fname(i)) = pname Then
                    idx_pmf = cnt_pmf
                End If
                cnt_pmf = cnt_pmf + 1
            End If
        Next i
		
		If cnt_pmf = 0 Then
			If Not strFileName Like "*ENG.INI" Then
				msg = "�w�肵���H���ɂ́A2��������Ͻ��̧�ق�1������܂���B" & Chr(10)
			Else
				msg = "There is not a PMF master file, either." & Chr(10)
			End If
			MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
			funPMF�t�@�C���̌��� = cnt_pmf
		Else
			funPMF�t�@�C���̌��� = cnt_pmf
			'UPGRADE_WARNING: Couldn't resolve default property of object obj.ListIndex. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            obj.selectedindex = idx_pmf
		End If
		
	End Function
	
	Function fun�g���q�w��t�@�C���̌���(ByRef dname As String, ByRef obj As Object, ByRef ext As String) As Short
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   �H���ިڸ�؂���g���q�w���̧�ق��������A��ʂ̺����ޯ���ɒǉ�����B
		'����
		'   dname(I)�F�H���ިڸ�ؖ�
		'   obj(I)�F�����ޯ���̵�޼ު�Ė��i̫�і�!���ޖ��j
		'   ext(I)�F��������g���q
		'�߂�l
		'   ������
		'     0�F�H���ިڸ�؂Ɋg���q��̧�ق�1�����݂��Ȃ��B
		'     1�ȏ�F�H���ިڸ�؂Ɋg���q��̧�ق����݂���B���̐��l��̧�ِ���\���B
		'---------------------------------------------------------------------------------------------------
		Dim cnt_ext As Short
		Dim fname As String
		Dim msg As String
		
		cnt_ext = 0
		
		'JPMF.INI���猟������2��������Ͻ��̧�ٖ���̧�ٕϊ������擾����B
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		fname = Dir(dname & "*." & ext, FileAttribute.Normal)
		Do While fname <> ""
			'UPGRADE_WARNING: Couldn't resolve default property of object obj.AddItem. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'obj.AddItem(fname)
            obj.Items.Add(fname)
			cnt_ext = cnt_ext + 1
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			fname = Dir()
		Loop 
		
		fun�g���q�w��t�@�C���̌��� = cnt_ext
		
	End Function
	Function funPMD�g���q�w��t�@�C���̌���(ByRef dname As String, ByRef obj As Object) As Short
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   �H���ިڸ�؂���PMD�g���q��̧�ق��������A��ʂ̺����ޯ���ɒǉ�����B
		'   �����ޯ���ɒǉ�����Ƃ��ɁA������PMS�t�@�C��������Ƃ��͒ǉ����Ȃ��B
		'����
		'   dname(I)�F�H���ިڸ�ؖ�
		'   obj(I)�F�����ޯ���̵�޼ު�Ė��i̫�і�!���ޖ��j
		'�߂�l
		'   ������
		'     0�F�H���ިڸ�؂Ɋg���q��̧�ق�1�����݂��Ȃ��B
		'     1�ȏ�F�H���ިڸ�؂Ɋg���q��̧�ق����݂���B���̐��l��̧�ِ���\���B
		'---------------------------------------------------------------------------------------------------
		Dim cnt_ext As Short
		Dim pos, i, flgUmu As Short
		Dim fname As String
		Dim pmd_name, pms_name As String
		Dim msg As String
		
		cnt_ext = 0
		flgUmu = 0
		
		'JPMF.INI���猟������2��������Ͻ��̧�ٖ���̧�ٕϊ������擾����B
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		fname = Dir(dname & "*.PMD", FileAttribute.Normal)
		Do While fname <> ""
			pos = InStr(UCase(fname), ".PMD")
			pmd_name = Left(fname, pos - 1)
			'UPGRADE_WARNING: Couldn't resolve default property of object obj.ListCount. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'For i = 0 To obj.ListCount - 1
            For i = 0 To obj.Items.Count() - 1
                'UPGRADE_WARNING: Couldn't resolve default property of object obj.List. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'pos = InStr(UCase(obj.List(i)), ".PMS")
                pos = InStr(UCase(obj.Items().Item(i)), ".PMS")
                If pos > 0 Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object obj.List. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'pms_name = Left(obj.List(i), pos - 1)
                    pms_name = Left(obj.Items().Item(i), pos - 1)
                    If UCase(pmd_name) = UCase(pms_name) Then
                        GoTo NEXT_PMD
                    End If
                End If
            Next i
            If flgUmu = 0 Then
                'UPGRADE_WARNING: Couldn't resolve default property of object obj.AddItem. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'obj.AddItem(fname)
                obj.Items.Add(fname)
                cnt_ext = cnt_ext + 1
            End If

NEXT_PMD:
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            fname = Dir()
        Loop
		
		funPMD�g���q�w��t�@�C���̌��� = cnt_ext
		
	End Function
End Module
Option Strict Off
Option Explicit On
Module JStandard
	
	Function �W���l�t�@�C���̓ǂݍ���(ByRef dname As String, ByRef keyword As String, ByRef nrec As Short, ByRef drec As String) As Short
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   �W���ļ�ق��緰ܰ�ނ�ں��ޔԍ����ް���ǂݍ��ށB
		'   �W���ļ�ق́A�����ިڸ�؁A����(GENSUN_DIR)\ENVIRON�̏��ɗL���𔻒f����B
		'����
		'   dname(I)�F�����ިڸ�ؖ�
		'   keyword(I)�F�������鷰ܰ��(FLG�AWEB��)�B�K���啶���œn�����B
		'   nrec(I)�F��ܰ�ޓ��̎擾�����ް���ں��ޔԍ�
		'   drec(O)�F�ړI��ں��ނ��ް�
		'�߂�l
		'   0�F��ܰ�ނ�ں��ޔԍ����ް���ǂݍ��߂��B
		'   1�F�W���ļ�ق����݂��Ȃ��B
		'   2�F�W���ļ�ٓ��ɷ�ܰ�ނ�������Ȃ��B
		'   3�F��ܰ�ޓ���ں��ޔԍ����ް���������Ȃ��B
		'---------------------------------------------------------------------------------------------------
		Dim flg, fnum, nr As Short
		Dim fname, kw As String
		Dim buf, msg As String
		Dim bo As Boolean
		Dim strFileName As String
		
		strFileName = FunGetLanguage()
		
		'�W���ļ�ٓ��ɷ�ܰ�ނ��L�邩�ǂ������׸�
		flg = 0

        ' 2015/07/22 Nakagawa Edit Start
        kw = ""
        ' 2015/07/22 Nakagawa Edit End

		'�����ިڸ�؂ɕW���ļ�ق��L��Ƃ��́A�����ǂݍ��ށB�����Ƃ��́AGENSUN_DIR��ENVIRON�̕W���ļ�ق�ǂݍ��ށB
		fname = dname & "STANDARD.DAT"
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(fname, FileAttribute.Normal) = "" Then
			fname = FunGetEnviron() & "STANDARD.DAT"
			'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			If Dir(fname, FileAttribute.Normal) = "" Then
				Beep()
				If Not strFileName Like "*ENG.INI" Then
					msg = "�W���ļ��(STANDARD.DAT)�������ިڸ�؂ɂ�ENVIRON�ިڸ�؂ɂ��L��܂���B" & Chr(10)
					msg = msg & "CastarJupiter�̎��s�����������ݽİق���Ă��܂���B"
				Else
					msg = "A standard value file (STANDARD.DAT) is not in a current directory and an ENVIRON directory." & Chr(10)
					msg = msg & "The execution environment of CastarJupiter is not installed correctly."
				End If
				MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
				�W���l�t�@�C���̓ǂݍ��� = 1
				Exit Function
			End If
		End If
		
		fnum = FreeFile
		FileOpen(fnum, fname, OpenMode.Input)
		Do While Not EOF(fnum)
			buf = LineInput(fnum)
			If Mid(buf, 1, 1) = ";" Then GoTo NEXT_L1
			
			'�擪���������ł��邩�ǂ������肵�A�ړI�̷�ܰ�ނ����肷��B
			If Mid(buf, 1, 1) = "\" Then
				bo = fun�����񒊏o(buf, "\", " ", kw)
				If keyword = UCase(kw) Then
					flg = 1
					nr = 0
					Do While Not EOF(fnum)
						buf = LineInput(fnum)
						If Mid(buf, 1, 1) = ";" Then GoTo NEXT_L2
						If buf = "" Then GoTo NEXT_L2
						
						'���̷�ܰ�ނ����������Ƃ��A�ǂݍ��݂��I������B
						If Mid(buf, 1, 1) = "\" Then GoTo EXIT_FILE
						
						'�ړI��ں��ޔԍ��ʒu�ł��邩�ǂ������肵�A�ް����i�[����B
						nr = nr + 1
						If nrec = nr Then
							flg = 2
							drec = buf
							FileClose(fnum)
							�W���l�t�@�C���̓ǂݍ��� = 0
							Exit Function
						End If
NEXT_L2: 
					Loop 
				End If
			End If
NEXT_L1: 
		Loop 
		
EXIT_FILE: 
		
		FileClose(fnum)
		
		If flg = 0 Then
			Beep()
			If Not strFileName Like "*ENG.INI" Then
				msg = "�W���ļ��(" & fname & ")���ɷ�ܰ��(" & keyword & ")�ƈ�v���鍀�ڂ��L��܂���B" & Chr(10)
				msg = msg & "�W���ļ�ق��Â��\�����L��܂��B�ŐV�̂��̂Ɠ���ւ��ĉ������B"
			Else
				msg = "The item which is in agreement with a keyword(" & keyword & ")is not in a standard value file(" & fname & ")." & Chr(10)
				msg = msg & "There is a possibility that a standard value file is old.Please change for the newest thing."
			End If
			MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
			�W���l�t�@�C���̓ǂݍ��� = 2
			Exit Function
		End If
		If flg = 1 Then
			Beep()
			If Not strFileName Like "*ENG.INI" Then
				msg = "�W���ļ��(" & fname & ")���ɷ�ܰ��(" & keyword & ")��" & nrec & "�s�ڂŎ擾�ł��鍀�ڂ��L��܂���B" & Chr(10)
				msg = msg & "�W���ļ�ق��Â��\�����L��܂��B�ŐV�̂��̂Ɠ���ւ��ĉ������B"
			Else
				msg = "An item acquirable by the " & nrec & " th line of keyword(" & keyword & ")is not in a standard value file(" & fname & ")." & Chr(10)
				msg = msg & "There is a possibility that a standard value file is old.Please change for the newest thing."
			End If
			MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
			�W���l�t�@�C���̓ǂݍ��� = 2
			Exit Function
		End If
		
		'�W���ļ�قɷ�ܰ�ނ͌����������Aں��ޔԍ����ް���������Ȃ��Ƃ��A�����܂ŏ���������B
		�W���l�t�@�C���̓ǂݍ��� = 3
		
	End Function
End Module
Option Strict Off
Option Explicit On
Module JString
	
	Function fun�����񒊏o(ByRef idata As String, ByRef titles As String, ByRef terminate As String, ByRef ans As String) As Boolean
		'-----------------------------------------------------------------
		' ������idata�̒�����title��terminate�ň͂܂ꂽ������𒊏o����B
		' terminate������idata���ɂȂ��ꍇ�́Atitle�ȍ~�̕�����S���o�͂���B
		'-----------------------------------------------------------------
		Dim pos As String
		Dim Rword As String

		fun�����񒊏o = False
		
		pos = CStr(InStr(idata, titles))
		If CDbl(pos) = 0 Then Exit Function
		
		pos = CStr(CDbl(pos) + Len(titles))
		Rword = Right(idata, Len(idata) - CDbl(pos) + 1)
		
		pos = CStr(InStr(Rword, terminate))
		If CDbl(pos) > 0 Then
			ans = Left(Rword, CDbl(pos) - 1)
		Else
			ans = Rword
		End If
		fun�����񒊏o = True
		
	End Function
	
	Function J_ChoiceString(ByRef st_data As String, ByRef nn As Short) As String
		'-----------------------------------------------------------------
		'�@�\
		'   �󔒂��ިЯ��Ƃ����ꍇ�Ann�Ԗڂ̕�����𒊏o����
		'-----------------------------------------------------------------
		Dim RRword, buf, LLword, buf2 As String
		Dim i, pos As Short

        ' 2015/07/22 Nakagawa Edit Start
        LLword = ""
        ' 2015/07/22 Nakagawa Edit End

		buf = Trim(st_data)
		buf2 = J_TransChar(buf, Chr(9), " ")
		buf = J_RemoveDoubleChar(buf2, " ")
		buf = Trim(buf)
		
		For i = 0 To nn
			pos = InStr(buf, " ")
			If pos = 0 Then pos = InStr(buf, Chr(9))
			
			If pos > 0 Then
				LLword = Left(buf, pos - 1)
				RRword = Right(buf, Len(buf) - pos)
			Else
				LLword = buf
				Exit For
			End If
			buf = RRword
		Next i
		J_ChoiceString = LLword
		
	End Function
	
	Function J_MidString(ByRef idata As String, ByRef delimit As String) As String
		'-----------------------------------------------------------------
		'�@�\
		'   ������idata�ŁA�ިЯ�����delimit�ŋ��܂ꂽ������𒊏o����B
		'-----------------------------------------------------------------
		Dim Rword As String
		Dim pos, pos2 As Short
		
		pos = InStr(idata, delimit)
		If pos > 0 Then
			Rword = Right(idata, Len(idata) - pos)
			pos2 = InStr(Rword, delimit)
			If pos2 > 0 Then
				J_MidString = Left(Rword, pos2 - 1)
				Exit Function
			End If
		End If
		J_MidString = ""
		
	End Function
	
	Function J_RemoveDoubleChar(ByRef src As String, ByRef cch As String) As String
		'-----------------------------------------------------------------
		'�@�\
		'   �A���d�������̍폜�����B������src���̕���cch��ׯ�߂��Ă����ꍇ�B
		'-----------------------------------------------------------------
		Dim i, nn As Short
		Dim dst As String
		Dim s1, s2 As String
		Dim j, k As Short
		
		dst = ""
		nn = Len(src)
		i = 1
		Do 
			If i > nn Then Exit Do
			
			s1 = Mid(src, i, 1)
			k = 0
			If s1 = cch Then
				For j = i + 1 To nn
					s2 = Mid(src, j, 1)
					If s1 <> s2 Then Exit For
					k = k + 1
				Next j
			End If
			dst = dst & s1
			i = i + 1 + k
		Loop 
		J_RemoveDoubleChar = dst
		
	End Function
	
	Function J_TransChar(ByRef src As String, ByRef srcC As String, ByRef dstC As String) As String
		'-----------------------------------------------------------------
		'�@�\
		'   ������̒u�����������B������src���̕���srcC��dstC�ɒu��������
		'-----------------------------------------------------------------
		Dim dst As String
		Dim i, nn As Short
		Dim cch As String
		
		dst = ""
		nn = Len(src)
		For i = 1 To nn
			cch = Mid(src, i, 1)
			If cch = srcC Then
				dst = dst & dstC
			Else
				dst = dst & cch
			End If
		Next i
		J_TransChar = dst
		
	End Function
	
	Function J_NWord(ByRef src As String) As Short
		'-----------------------------------------------------------------
		'�@�\
		'   ���ݸ��؂�ŕ����񂪉����邩
		'-----------------------------------------------------------------
		Dim nWord, pos As Short
		Dim buf, buf2 As String
		Dim LLword, RRword As String
		
		nWord = 0
		
		buf = Trim(src)
		buf2 = J_TransChar(buf, Chr(9), " ")
		buf = J_RemoveDoubleChar(buf2, " ")
		buf = Trim(buf)
		
		If buf = "" Then
			J_NWord = 0
			Exit Function
		End If
		
		Do 
			pos = InStr(buf, " ")
			If pos = 0 Then
				nWord = nWord + 1
				Exit Do
			End If
			
			nWord = nWord + 1
			LLword = Left(buf, pos - 1)
			RRword = Right(buf, Len(buf) - pos)
			buf = RRword
		Loop 
		
		J_NWord = nWord
		
	End Function
	
	'--------------------------------------------------------
	'   �֐���  : DoubleByteChk
	'   �p�r    : ���������2�޲ĕ������܂܂�Ă��邩�ǂ����𒲂ׂ�
	'   ����    : strCheckString �����Ώە�����
	'   �߂�l  : 2�޲ĕ����𔭌������ʒu
	'--------------------------------------------------------
	Function DoubleByteChk(ByRef strCheckString As String) As Integer
		Dim i As Integer
		'�����Ώە�����̒������i�[
		Dim lngCheckSize As Integer
		'ANSI�ւ̕ϊ���̕������i�[
		Dim lngANSIStr As Integer
		
		DoubleByteChk = 0
		lngCheckSize = Len(strCheckString)
		
		For i = 1 To lngCheckSize
			'StrConv��Unicode����ANSI�ւƕϊ�
			'UPGRADE_ISSUE: Constant vbFromUnicode was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
            'UPGRADE_ISSUE: LenB function is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="367764E5-F3F8-4E43-AC3E-7FE0B5E074E2"'
            'mod by xiaoyun.z 20150408 sta
            'lngANSIStr = Strings.LenB(StrConv(Mid(strCheckString, i, 1), vbFromUnicode))
            ' 2015/07/22 Nakagawa Edit Start
            'lngANSIStr = StrConv(Mid(strCheckString, i, 1), 128)
            lngANSIStr = LenB(Mid(strCheckString, i, 1))
            ' 2015/07/22 Nakagawa Edit End
            'mod by xiaoyun.z 20150408 end
			'�ϊ���̕�����2�޲Ă��ǂ������f
			If lngANSIStr = 2 Then
				DoubleByteChk = i
				Exit For
			End If
		Next i
		
	End Function

    Public Function LenB(ByVal str As String) As Long
        'Shift JIS�ɕϊ������Ƃ��ɕK�v�ȃo�C�g����Ԃ�
        Return System.Text.Encoding.GetEncoding(932).GetByteCount(str)
    End Function

End Module
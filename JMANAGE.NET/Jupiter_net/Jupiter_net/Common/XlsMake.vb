Option Strict Off
Option Explicit On
Module XlsMake
	
	Sub �V����Sheet�ɃR�s�[(ByRef InFilePath As String, ByRef InName As String, ByRef OutFilePath As String, ByRef OutName As String, ByRef SheetNo As Short, ByRef SheetName As String)
		Dim FilePathName As String
		Dim OutNameTemp As String
		Dim xlApp As Microsoft.Office.Interop.Excel.Application
		
		xlApp = New Microsoft.Office.Interop.Excel.Application
		
		OutNameTemp = OutName
		'20100913 whl �ǉ� start
		If CDbl(xlApp.Version) > 11 Then
			OutNameTemp = OutNameToXlsx(OutName)
		End If
		'20100913 whl �ǉ� end
		
		'�o��Workbook���J���B
		FilePathName = OutFilePath & OutNameTemp
		'MsgBox "OutNameTemp=" & FilePathName
		xlApp.Workbooks.Open(FileName:=FilePathName)
		'MsgBox "Open1 OK"
		
		'����Workbook���J���B
		FilePathName = InFilePath & InName
		'MsgBox "InName=" & FilePathName
		xlApp.Workbooks.Open(FileName:=FilePathName)
		'MsgBox "Open2 OK"
		'nira S 2014/4/10
		'�V�[�g���̕ύX��xls�ɃV�[�g�ړ��������Ƃɍs����OFFICE2013���ł̓G���[�ɂȂ�B
		'����āA�V�[�g����htm�t�@�C���̂Ƃ��ɕύX����B
		'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Workbooks().Sheets().Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		xlApp.Workbooks(InName).Sheets("Sheet1").Name = SheetName
		'nira E 2014/4/10
		'MsgBox "Workbooks(InName).Sheets(Sheet1).Name=" & SheetName
		
		'��Ĕԍ���ϲŽ�l�̂Ƃ��A��Đ����擾�B
		If SheetNo < 0 Then
			SheetNo = xlApp.Workbooks(OutNameTemp).Sheets.Count
		End If
		'MsgBox "SheetNo=" & SheetNo
		
		'��Ĕԍ��̌�Ɉړ��B
		'    xlApp.Sheets("Sheet1").Move After:=xlApp.Workbooks(OutName).Sheets(SheetNo) 20100913 whl �폜
		'MsgBox "Move START"
		'    If xlApp.Version <= 14 Then
		'        xlApp.Workbooks(InName).Sheets("Sheet1").Move After:=xlApp.Workbooks(OutNameTemp).Sheets(SheetNo) '20100913 whl�ǉ�
		'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Workbooks().Sheets().Move. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		xlApp.Workbooks(InName).Sheets(SheetName).Move(After:=xlApp.Workbooks(OutNameTemp).Sheets(SheetNo)) '20100913 whl�ǉ�
		'    Else
		'MsgBox "Move Activate"
		'        xlApp.Workbooks(InName).Activate
		'MsgBox "Move Select"
		'        xlApp.Workbooks(InName).Sheets(SheetName).Select
		'MsgBox "Move Move"
		'        xlApp.Workbooks(InName).Sheets(SheetName).Move After:=xlApp.Workbooks(OutNameTemp).Sheets(SheetNo) '20100913 whl�ǉ�
		'    End If
		SheetNo = SheetNo + 1
		
		'��Ė���ύX�B
		xlApp.Workbooks(OutNameTemp).Activate()
		'MsgBox "SheetName SET Select"
		'xlApp.Sheets(SheetNo).Select
		'MsgBox "SheetName SET SheetName�ύX�O=" & SheetName
		'xlApp.Sheets(SheetNo).Name = SheetName
		'xlApp.Workbooks(OutNameTemp).Sheets(SheetNo).Name = SheetName
		'MsgBox "SheetName SET SheetName�ύX��=" & xlApp.Sheets(SheetNo).Name
		
		'����Workbook��ۑ���������B
		xlApp.Workbooks(InName).Activate()
		xlApp.Workbooks(InName).Close(SaveChanges:=False)
		'MsgBox "InName Close"
		
		'�װ��ޯ�����\���B
		xlApp.DisplayAlerts = False
		'�o��Workbook��ۑ��B
		xlApp.Workbooks(OutNameTemp).Activate()
		FilePathName = OutFilePath & OutNameTemp
		'MsgBox "FilePathName SaveAs=" & FilePathName
		xlApp.Workbooks(OutNameTemp).SaveAs(FileName:=FilePathName)
		
		'Workbook�����B
		xlApp.Workbooks(OutNameTemp).Close()
		'MsgBox "OutNameTemp Close"
		'Quitҿ��ނ��g����Excel���I���B
		xlApp.Quit()
		
		'��޼ު�Ă�����B
		'UPGRADE_NOTE: Object xlApp may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		xlApp = Nothing
		
	End Sub
	
	Sub �V�KBook���쐬(ByRef FilePath As String, ByRef OutName As String)
		Dim FilePathName As String
		Dim xlApp As Microsoft.Office.Interop.Excel.Application
		Dim xlBook As Microsoft.Office.Interop.Excel.Workbook
		
		xlApp = New Microsoft.Office.Interop.Excel.Application
		xlBook = xlApp.Workbooks.Add
		
		'Workbook��ۑ��B
		'20110330 nakagawa �ǉ� start
		'    If xlApp.Version > 11 Then
		'        OutName = OutNameToXlsx(OutName)
		'    End If
		'20110330 nakagawa �ǉ� end
		FilePathName = FilePath & OutName
		
		If CDbl(xlApp.Version) < 12 Then
			xlBook.SaveAs(FileName:=FilePathName)
		Else
			Call ChengeFormatToXlsx(FilePathName, xlApp)
		End If
		
		'Workbook�����B
		xlBook.Close()
		'Quitҿ��ނ��g����Excel���I���B
		xlApp.Quit()
		
		'��޼ު�Ă�����B
		'UPGRADE_NOTE: Object xlApp may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		xlApp = Nothing
		'UPGRADE_NOTE: Object xlBook may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		xlBook = Nothing
		
	End Sub
	
	Sub �W��Sheet���폜(ByRef FilePath As String, ByRef OutName As String)
		Dim i As Short
		Dim FilePathName As String
		Dim xlApp As Microsoft.Office.Interop.Excel.Application
		
		xlApp = New Microsoft.Office.Interop.Excel.Application
		
		'Workbook���J���B
		If CDbl(xlApp.Version) < 12 Then
			FilePathName = FilePath & OutName
		Else
			FilePathName = FilePath & OutName & "x"
		End If
		xlApp.Workbooks.Open(FileName:=FilePathName)
		
		If CDbl(xlApp.Version) < 12 Then
			For i = xlApp.Workbooks(OutName).Sheets.Count To 1 Step -1
				'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If InStr(xlApp.Sheets(i).Name, "Sheet") Then
					'�װ��ޯ�����\���B
					xlApp.DisplayAlerts = False
					'��Ă�I�����폜�B
					'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Select. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					xlApp.Sheets(i).Select()
					'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Delete. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					xlApp.Sheets(i).Delete()
				End If
			Next i
		Else
			For i = xlApp.Worksheets.Count To 1 Step -1
				'MsgBox "i=" & i & "         xlApp.Sheets(i).Name=" & xlApp.Sheets(i).Name
				'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If InStr(xlApp.Sheets(i).Name, "Sheet") Then
					'�װ��ޯ�����\���B
					xlApp.DisplayAlerts = False
					'��Ă�I�����폜�B
					'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Select. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					xlApp.Sheets(i).Select()
					'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Delete. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					xlApp.Sheets(i).Delete()
				End If
			Next i
		End If
		
		'MsgBox "Select"
		'�ŏ��̼�Ă�I���B
		'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Select. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		xlApp.Sheets(1).Select()
		
		'�װ��ޯ�����\���B
		xlApp.DisplayAlerts = False
		
		'Workbook��ۑ��B
		'    xlApp.Workbooks(OutName).SaveAs FileName:=FilePathName 20100913 whl �폜
		
		'MsgBox "save"
		'20100913 whl�ǉ� start
		If CDbl(xlApp.Version) < 12 Then
			xlApp.Workbooks(OutName).SaveAs(FileName:=FilePathName)
		Else
			Call ChengeFormatToXls(FilePathName, xlApp)
		End If
		'20100913 whl�ǉ� end
		
		'MsgBox "Close"
		'Workbook�����B
		xlApp.Workbooks.Close()
		
		'20100913 whl�ǉ� start
		Dim Fsys As Object
		If CDbl(xlApp.Version) > 11 Then
			
			FilePathName = FilePathName & "x"
			Fsys = CreateObject("Scripting.FileSystemObject")
			'UPGRADE_WARNING: Couldn't resolve default property of object Fsys.DeleteFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			Fsys.DeleteFile(FilePathName, True)
		End If
		'20100913 whl�ǉ� end
		
		'Quitҿ��ނ��g����Excel���I���B
		xlApp.Quit()
		
		'��޼ު�Ă�����B
		'UPGRADE_NOTE: Object xlApp may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		xlApp = Nothing
		
	End Sub
	
	Sub ���O��t���ĕۑ�(ByRef InFilePath As String, ByRef InName As String, ByRef OutFilePath As String, ByRef OutName As String, ByRef SheetName As String)
		Dim FilePathName As String
		Dim xlApp As Microsoft.Office.Interop.Excel.Application
		
		xlApp = New Microsoft.Office.Interop.Excel.Application
		
		'Workbook���J���B
		FilePathName = InFilePath & InName
		xlApp.Workbooks.Open(FileName:=FilePathName)
		
		'��Ė���ύX�B
		'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Select. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		xlApp.Sheets(1).Select()
		'UPGRADE_WARNING: Couldn't resolve default property of object xlApp.Sheets().Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		xlApp.Sheets(1).Name = SheetName
		
		'�װ��ޯ�����\���B
		xlApp.DisplayAlerts = False
		'Workbook��ۑ��B
		FilePathName = OutFilePath & OutName
		
		'    xlApp.Workbooks(InName).SaveAs FileName:=FilePathName, FileFormat:=xlNormal 20100913 whl �폜
		
		'20100913 whl �ǉ� start
		If CDbl(xlApp.Version) < 12 Then
			xlApp.Workbooks(InName).SaveAs(FileName:=FilePathName, FileFormat:=Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal)
		Else
			Call ChengeFormatToXlsx(FilePathName, xlApp)
		End If
		'20100913 whl �ǉ� end
		
		'Quitҿ��ނ��g����Excel���I���B
		xlApp.Quit()
		
		'��޼ު�Ă�����B
		'UPGRADE_NOTE: Object xlApp may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		xlApp = Nothing
		
	End Sub
	
	'20100913 whl�ǉ� start
	Function OutNameToXlsx(ByRef OutName As String) As String
		
		Dim Postfix As String
		Postfix = Mid(OutName, InStr(OutName, ".") + 1)
		
		If UCase(Postfix) = UCase("xls") Then
			OutNameToXlsx = OutName & "x"
		Else
			OutNameToXlsx = OutName
		End If
		
	End Function
	
	Function FilePathNameToXls(ByRef OutName As String) As String
		
		OutName = Mid(OutName, 1, InStr(OutName, "."))
		
		OutName = OutName & "xls"
		FilePathNameToXls = OutName
		
	End Function
	
	Function ChengeFormatToXlsx(ByRef FilePathName As String, ByRef xlApp As Microsoft.Office.Interop.Excel.Application) As Object
		
		Trim(FilePathName)
		FilePathName = FilePathName & "x"
		
		xlApp.ActiveWorkbook.SaveAs(FileName:=FilePathName, FileFormat:=Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, CreateBackup:=False)
		
	End Function
	
	Function ChengeFormatToXls(ByRef FilePathName As String, ByRef xlApp As Microsoft.Office.Interop.Excel.Application) As Object
		
		FilePathName = FilePathNameToXls(FilePathName)
		
		xlApp.ActiveWorkbook.SaveAs(FileName:=FilePathName, FileFormat:=Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, Password:="", WriteResPassword:="", ReadOnlyRecommended:=False, CreateBackup:=False)
		
	End Function
	'20100913 whl�ǉ� end
End Module
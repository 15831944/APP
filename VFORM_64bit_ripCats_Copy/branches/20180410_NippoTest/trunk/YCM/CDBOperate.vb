Imports System.Data
Imports System.Data.OleDb
Imports ADODB
Public Class CDBOperate
    '�R�l�N�V�����I�u�W�F�N�g
    Private m_adoConn As ADODB.Connection
    '�e�[�u����
    Private m_strTableName As String
    Private m_nFields As Long
    Private m_strFields() As String

    '===============================================================================
    '�@  �\: �N���X�̏���������
    '===============================================================================
    Public Sub New()
        m_adoConn = Nothing
        m_strTableName = ""
        m_nFields = 0
        Erase m_strFields
    End Sub

    ''===============================================================================
    ''�@  �\: �N���X�̏I������
    ''===============================================================================
    'Private Sub Dispose()
    '    If Not m_adoConn Is Nothing Then
    '        Call DisConnectDB()
    '    End If
    '    m_adoConn = Nothing
    '    m_strTableName = ""
    '    m_nFields = 0
    '    Erase m_strFields()
    'End Sub

    '===============================================================================
    '�@  �\: �f�[�^�x�[�X�ɐڑ�����
    '�߂�l: True:����, False:���s
    '��  ��:
    '       strDataSource   [I/ ]   �f�[�^�\�[�X��(mdb��)
    '��  �l:
    '===============================================================================
    Public Function ConnectDB(ByVal strDataSource As String) As Boolean
        Dim strDBProvider As String = ""
        Dim strConnectionString As String

        '�v���o�C�_���̂��Z�b�g����
        If IntPtr.Size = 4 Then
            strDBProvider = "Microsoft.Jet.OLEDB.4.0"
        ElseIf IntPtr.Size = 8 Then
            strDBProvider = "Microsoft.Ace.OLEDB.12.0"
        End If

        'ConnectionString���쐬
        strConnectionString = "Provider=" & strDBProvider & ";" & _
                              "Data Source=" & strDataSource

        '�R�l�N�V�����I�u�W�F�N�g�𐶐�����
        m_adoConn = New ADODB.Connection
        ConnectDB = True

        On Error GoTo ErrTrap
        'DB�Ɛڑ�
        Call m_adoConn.Open(strConnectionString)
        On Error GoTo 0
        '�f�[�^�\�[�X�����Z�b�g����
        Exit Function
ErrTrap:
        ConnectDB = False
    End Function

    '===============================================================================
    '�@  �\: �ڑ����Ă���DB�I�u�W�F�N�g��ؒf�E�������
    '===============================================================================
    Public Sub DisConnectDB()
        On Error Resume Next
        m_adoConn.Close()
        m_adoConn = Nothing
        On Error GoTo 0
    End Sub

    '===============================================================================
    '�@  �\: �w���SQL�����s����
    '�߂�l: True:����, False:���s
    '��  ��:
    '       strSQL      [I/ ]   SQL��
    '��  �l:
    '===============================================================================
    Public Function ExcuteSQL( _
        ByVal strSQL As String _
    ) As Boolean
        On Error GoTo AAAAAAAA
        m_adoConn.Execute(strSQL)
        On Error GoTo 0
        ExcuteSQL = True
        Exit Function
AAAAAAAA:
        ExcuteSQL = False
        On Error GoTo 0
    End Function

    '===============================================================================
    '�@  �\: SQL�����w�肵�āA�f�[�^���擾����
    '�߂�l: ���ʃZ�b�g(ADODB.Recordset)
    '��  ��:
    '       strSQL      [I/ ]   SQL��
    '��  �l:
    '===============================================================================
    Public Function CreateRecordset( _
    Optional ByVal strSQL As String = "" _
    ) As ADODB.Recordset
        Dim adoRst As ADODB.Recordset
        On Error GoTo Err_CreateRecordset
        If Len(strSQL) = 0 Then
            strSQL = m_strTableName
            If m_nFields > 0 Then
            End If
        End If
        '���R�[�h�Z�b�g���쐬����
        adoRst = New ADODB.Recordset
        adoRst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        adoRst.Open(strSQL, m_adoConn, CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

        '���R�[�h�Z�b�g�I�u�W�F�N�g��SQL���ɏ]���Đ�������B
        CreateRecordset = adoRst
        adoRst = Nothing
        On Error GoTo 0
        Exit Function
Err_CreateRecordset:
        CreateRecordset = Nothing
    End Function

    '===============================================================================
    ' �@�@�\�F�w�肵���e�[�u���̎w�肵���t�B�[���h�̒l���擾����
    ' �߂�l�F�Ȃ�
    ' ���@���F
    '       strField    [I/ ]   �擾����t�B�[���h
    '       strTable    [I/ ]   �Ώۃe�[�u��
    '       strWhere    [I/ ]   ���o����(�ȗ���:�ȗ����͑S���R�[�h��Ώ�)
    '--------------------------------------------------------------------------------
    Public Function DoDLookup( _
        ByVal strSelect As String, _
        ByVal strTable As String, _
        Optional ByVal strWhere As String = "" _
    ) As Object
        '�t�B�[���h�̓��e���擾
        Dim strSQL As String
        Dim objRs As ADODB.Recordset
        strSQL = "SELECT " & strSelect & " FROM " & strTable
        If Len(strWhere) > 0 Then
            strSQL = "SELECT " & strSelect & " FROM " & strTable & " WHERE " & strWhere
        End If
        objRs = CreateRecordset(strSQL)
        On Error Resume Next
        objRs.MoveFirst()
        DoDLookup = objRs.Fields(0).Value
        objRs.Close()
        On Error GoTo 0
    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       �w�肵�����R�[�h���X�V����B
    '
    '   <�߂�l>
    '       (Long)
    '       �X�V�������R�[�h���i�G���[�̎�-1��Ԃ��j
    '
    '   <���@��>
    '       strFields   [I/ ]   �t�B�[���h��
    '       strTable    [I/ ]   �Ώۃe�[�u��
    '       vSetValues  [I/ ]   �X�V����l
    '       strWhere    [I/ ]   �X�V����(�ȗ���:�ȗ����͑S���R�[�h��Ώ�)
    '
    '   <���@�l>
    '       strFields() �� vSetValues() �̓Y��������
    '--------------------------------------------------------------------------------
    Public Function DoUpdate( _
    ByVal strFields() As String, _
    ByVal strTable As String, _
    ByVal vSetValues() As Object, _
    Optional ByVal strWhere As String = "" _
    ) As Long
        '--Call outputADOInfomation("CDBOperateADO.DoUpdate")
        Dim objRecordset As ADODB.Recordset
        Dim strSQL As String
        Dim nUpdateCount As Long
        Dim i As Long

        On Error GoTo err_IDBOperate_DoUpdate
        'SQL�𐶐�
        strSQL = "select * from " & strTable
        '������t��
        If Len(strWhere) > 0 Then
            strSQL = strSQL & " where " & strWhere
        End If
        '���R�[�h�Z�b�g�𐶐�
        objRecordset = CreateRecordset(strSQL)
        '-    Call outputADOInfomation("    Update�Ώۃ��R�[�h�\�[�X:" & strSQL)
        If Not objRecordset Is Nothing Then
            nUpdateCount = 0
            Do Until objRecordset.EOF
                For i = 0 To UBound(strFields)
                    '--Call outputADOInfomation("    " & strTable & "." & strFields(i) & " �� " & vSetValues(i))
                    '�X�V�f�[�^���Z�b�g
                    objRecordset(strFields(i)).Value = vSetValues(i)
                Next i
                '�f�[�^���X�V
                objRecordset.Update()
                '���̃��R�[�h�ֈړ�
                objRecordset.MoveNext()
                '�X�V���R�[�h��
                nUpdateCount = nUpdateCount + 1
            Loop
        Else
            GoTo err_IDBOperate_DoUpdate
        End If

        '���R�[�h�Z�b�g�����
        objRecordset = Nothing
        '�߂�l���Z�b�g
        DoUpdate = nUpdateCount
        Exit Function
err_IDBOperate_DoUpdate:
        DoUpdate = -1
        '���R�[�h�Z�b�g�����
        objRecordset = Nothing
        'Debug.Print "CDBOperateADO.DoUpdate:" & strTable & ":" & Err.Description
        '--    Call dispADOErrMsg("DoUpdate")
    End Function
    '--------------------------------------------------------------------------------
    '   <�@�@�\> �f�[�^�x�[�X�ւ̃R�l�N�V�����I�u�W�F�N�g��Ԃ�
    '   <�߂�l> (ADODB.Connection)
    '       �f�[�^�x�[�X�ւ̃R�l�N�V�����I�u�W�F�N�g�i�G���[�̎�Nothing��Ԃ��j
    '   <���@��> �Ȃ�
    '   <���@�l>
    '--------------------------------------------------------------------------------
    Public Function GetConnection( _
    ) As ADODB.Connection
        GetConnection = m_adoConn
    End Function
    '===============================================================================
    Public Function BeginTrans() As Integer
        BeginTrans = m_adoConn.BeginTrans()
    End Function
    Public Sub CommitTrans()
        m_adoConn.CommitTrans()
    End Sub
    Public Sub RollbackTrans()
        m_adoConn.RollbackTrans()
    End Sub


    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       ���R�[�h�̓o�^
    '
    '   <�߂�l>
    '       ���s���R�[�h��
    '
    '   <���@��>
    '       strFields() [I/ ]   �o�^�t�B�[���h��
    '       strTable    [I/ ]   �e�[�u����
    '       strValues() [I/ ]   �o�^�l
    '--------------------------------------------------------------------------------
    Function DoInsert( _
                    ByVal strFields() As String, _
                    ByVal strTable As String, _
                    ByVal vSetValues() As String _
    ) As Long

        Dim intFieldCnt As Integer
        Dim i As Integer
        Dim strInsert As String = ""
        Dim strValue As String = ""


        Try

            intFieldCnt = UBound(strFields)
            For i = 0 To intFieldCnt
                Select Case i
                    Case 0
                        strInsert = "INSERT INTO " & strTable & " ("
                        strValue = " VALUES ("
                    Case Else
                        strInsert = strInsert & ","
                        strValue = strValue & ","
                End Select

                strInsert = strInsert & strFields(i)
                If vSetValues(i).Length = 0 Then
                    strValue = strValue & "null"
                Else
                    strValue = strValue & vSetValues(i)
                End If
            Next



            Return Me.ExcuteSQL(strInsert & ")" & strValue & ")")

        Catch
            Return -1
        End Try

    End Function
End Class

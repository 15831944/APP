'=================================================(C)Copyright YTI Inc., 2007====
'   �p�b�P�[�W���@�@: DB�ڑ��p�b�P�[�W
'   �N���X���@�@�@�@: DB�ڑ��N���X(OLE)
'   �o�[�W�����@�@�@:
'--------------------------------------------------------------------------------
'     ��@���@�ҁ@�@: kawasaki        2007.05.14
'     ���@�@�@���@�@:
'================================================================================
'�yMicrosoft Access�z
'.NET Framework Data Provider for OLE DB�@ 
Imports System.Data
Imports System.Data.OleDb
'Imports YTI.Kikan.Library.DB




Public Class CDBOperateOLE

    Implements IDBOperate

    'OLEDB�ڑ�
    Dim m_cnnOLEDB As OleDbConnection
    Dim m_cnnString As String
    '---Ins.Start-----------------------------------------(2008.04.18) kawasaki
    Dim m_strErrMsg As String
    '---Ins. End -----------------------------------------(2008.04.18) kawasaki

    '�g�����U�N�V��������
    Dim m_Transaction As OleDbTransaction
    Dim m_bIsExecTransaction As Boolean

    Dim m_daOleDB As OleDbDataAdapter
    Dim m_dsOleDB As DataSet


    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       DB�Ƃ̐ڑ����s���iAccess�j
    '
    '   <�߂�l>
    '       True�F�ڑ�����
    '       False�F�ڑ����s
    '
    '   <���@��>
    '       strDataSource   [I/ ]   �f�[�^�\�[�X���i�K�{�j
    '       strUserID       [I/ ]   ���[�U��(�ȗ���)
    '       strPassword     [I/ ]   �p�X���[�h(�ȗ���)
    '       strCnnString    [I/ ]   ���̑��ɕK�v�Ȑڑ�������(�ȗ���)
    '--------------------------------------------------------------------------------
    Public Function Connect( _
            ByVal strDataSource As String, _
            Optional ByVal strUserID As String = "", _
            Optional ByVal strPassword As String = "", _
            Optional ByVal strCnnString As String = "" _
    ) As Boolean Implements IDBOperate.Connect


        Try

            '�ڑ�������쐬
            Call ConnectionString(strDataSource, strUserID, strPassword, strCnnString)

            '�R�l�N�V�����I�u�W�F�N�g�𐶐�
            m_cnnOLEDB = New OleDbConnection

            'DB�Ɛڑ�
            m_cnnOLEDB.ConnectionString = m_cnnString
            m_cnnOLEDB.Open()

            Return True

        Catch
            m_cnnOLEDB = Nothing
            Return False
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       AccessDB�Ƃ̐ڑ���ؒf����
    '
    '   <�߂�l>
    '       �Ȃ�
    '
    '   <���@��>
    '       �Ȃ�
    '--------------------------------------------------------------------------------
    Public Sub DisConnect() Implements IDBOperate.DisConnect
        On Error GoTo Disconnect_Err

        If m_cnnOLEDB.State = ConnectionState.Open Then
            m_cnnOLEDB.Close()
        End If

Disconnect_Err:
        m_cnnOLEDB = Nothing
    End Sub

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       DB�Ƃ̐ڑ���������쐬����
    '
    '   <�߂�l>
    '       �Ȃ�
    '
    '   <���@��>
    '       strDataSource   [I/ ]   �f�[�^�\�[�X��
    '       strUserID       [I/ ]   ���[�U��(�ȗ���)
    '       strPassword     [I/ ]   �p�X���[�h(�ȗ���)
    '       strCnnString    [I/ ]   ���̑��ɕK�v�Ȑڑ�������(�ȗ���)
    '--------------------------------------------------------------------------------
    Public Sub ConnectionString( _
                    ByVal strDataSource As String, _
                    Optional ByVal strUserID As String = "", _
                    Optional ByVal strPassword As String = "", _
                    Optional ByVal strCnnString As String = "" _
    ) Implements IDBOperate.ConnectionString

        On Error Resume Next

        'm_cnnString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
        '                      "Data Source=" & strDataSource & ";"
        If IntPtr.Size = 4 Then
            m_cnnString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                                  "Data Source=" & strDataSource & ";"
        ElseIf IntPtr.Size = 8 Then
            m_cnnString = "Provider=Microsoft.ACE.OLEDB.12.0;" & _
                              "Data Source=" & strDataSource & ";"
        End If

        If strUserID <> "" Then
            m_cnnString = m_cnnString & "User ID=" & strUserID & ";"
        End If

        If strPassword <> "" Then
            m_cnnString = m_cnnString & "Jet OLEDB:Database Password=" & strPassword & ";"
        End If

        If strCnnString <> "" Then
            m_cnnString = m_cnnString & strCnnString
        End If

    End Sub

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       SQL�������s����i���R�[�h��Ԃ��Ȃ��j
    '
    '   <�߂�l>
    '       �Ȃ�
    '
    '   <���@��>
    '       �Ȃ�
    '--------------------------------------------------------------------------------
    Public Function ExecuteSQL( _
                    ByVal strSQL As String _
    ) As Long Implements IDBOperate.ExecuteSQL

        Dim cmd As New OleDbCommand

        Try

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.Message)
            Return -1
        End Try

    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       SQL�������s����i�W�v���ʂ�Ԃ��j
    '
    '   <�߂�l>
    '       �W�v����
    '
    '   <���@��>
    '       strSQL  [I/ ]   SQL��
    '--------------------------------------------------------------------------------
    Function ExecuteSQL2( _
                    ByVal strSQL As String _
    ) As Object Implements IDBOperate.ExecuteSQL2

        Dim cmd As New OleDbCommand

        Try

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteScalar()

        Catch
            Return Nothing
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       �e�[�u�����A�t�B�[���h�����w�肵�Ă��̃��R�[�h����Ԃ��iAccess�j
    '
    '   <�߂�l>
    '       ���R�[�h��
    '
    '   <���@��>
    '       strSelect   [I/ ]   �t�B�[���h��
    '       strTable    [I/ ]   �Ώۃe�[�u��
    '       strWhere    [I/ ]   ���o����(�ȗ���:�ȗ����͑S���R�[�h��Ώ�)
    '--------------------------------------------------------------------------------
    Public Function DoDCount( _
                ByVal strSelect As String, _
                ByVal strTable As String, _
                Optional ByVal strWhere As String = "" _
    ) As Long Implements IDBOperate.DoDCount

        Dim cmd As New OleDbCommand
        Dim strSql As String

        Try

            strSql = "SELECT COUNT(" & strSelect & ") FROM " & strTable
            If Len(strWhere) > 0 Then
                strSql = strSql & " WHERE " & strWhere
            End If

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSql
            cmd.Transaction = m_Transaction

            Return CLng(cmd.ExecuteScalar)

        Catch
            Return -1
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       �g�����U�N�V�����J�n����
    '
    '   <�߂�l>
    '
    '   <���@��>
    '
    '--------------------------------------------------------------------------------
    Public Sub BeginTrans() Implements IDBOperate.BeginTrans
        On Error Resume Next

        If m_bIsExecTransaction = False Then
            m_Transaction = m_cnnOLEDB.BeginTransaction()
            m_bIsExecTransaction = True
        End If
    End Sub

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       �R�~�b�g����
    '
    '   <�߂�l>
    '
    '   <���@��>
    '
    '--------------------------------------------------------------------------------
    Public Sub CommitTrans() Implements IDBOperate.CommitTrans
        On Error Resume Next

        If m_bIsExecTransaction = True Then
            m_Transaction.Commit()
            m_bIsExecTransaction = False
        End If
    End Sub

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       ���[���o�b�N����
    '
    '   <�߂�l>
    '
    '   <���@��>
    '
    '--------------------------------------------------------------------------------
    Public Sub RollbackTrans() Implements IDBOperate.RollbackTrans
        On Error Resume Next

        If m_bIsExecTransaction = True Then
            m_Transaction.Rollback()
            m_bIsExecTransaction = False
        End If
    End Sub

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       ���R�[�h�̑I���iAccess�j
    '
    '   <�߂�l>
    '       DataReader�I�u�W�F�N�g
    '
    '   <���@��>
    '       strSQL      [I/ ]   ����SQL
    '--------------------------------------------------------------------------------
    Public Function DoSelect( _
                    ByVal strSQL As String _
    ) As IDataReader Implements IDBOperate.DoSelect

        Dim cmd As New OleDbCommand

        Try

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteReader

        Catch
            Return Nothing
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       ���R�[�h�̍폜�iAccess�j
    '
    '   <�߂�l>
    '       ���s���R�[�h��
    '
    '   <���@��>
    '       strTable    [I/ ]   �e�[�u����
    '       strWhere    [I/ ]   �폜�����i�ȗ����F�S�f�[�^�Ώہj
    '--------------------------------------------------------------------------------
    Public Function DoDelete( _
                ByVal strTable As String, _
                Optional ByVal strWhere As String = "" _
    ) As Long Implements IDBOperate.DoDelete

        Dim cmd As New OleDbCommand
        Dim strSQL As String

        Try

            'SQL�𐶐�
            strSQL = "DELETE FROM " & strTable
            If Len(strWhere) > 0 Then
                strSQL = strSQL & " WHERE " & strWhere
            End If

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.Message)
            Return -1
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       ����J�����̒l�擾
    '
    '   <�߂�l>
    '       �����l
    '
    '   <���@��>
    '       strSelect   [I/ ]   ��������
    '       strTable    [I/ ]   �����e�[�u����
    '       strWhere    [I/ ]   ���������i�ȗ��j
    '--------------------------------------------------------------------------------
    Public Function DoDLookup( _
                    ByVal strSelect As String, _
                    ByVal strTable As String, _
                    Optional ByVal strWhere As String = "" _
    ) As Object Implements IDBOperate.DoDLookup

        '�t�B�[���h�̓��e���擾
        Dim strSQL As String
        Dim cmd As New OleDbCommand
        Dim dr As OleDbDataReader

        Try

            '����SQL�̍쐬
            strSQL = "SELECT " & strSelect & " FROM " & strTable
            If Len(strWhere) > 0 Then
                strSQL = "SELECT " & strSelect & " FROM " & strTable & " WHERE " & strWhere
            End If

            '�l�̌���
            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction
            dr = cmd.ExecuteReader

            dr.Read()
            Return dr(0)

        Catch
            Return Nothing
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       ���R�[�h�̍X�V�iAccess�j
    '
    '   <�߂�l>
    '       ���s���R�[�h��
    '
    '   <���@��>
    '       strFields() [I/ ]   �X�V�t�B�[���h��
    '       strTable    [I/ ]   �e�[�u����
    '       strValues() [I/ ]   �X�V�l
    '       strWhere    [I/ ]   �X�V�����i�ȗ����F�S�f�[�^�Ώہj
    '--------------------------------------------------------------------------------
    Public Function DoUpdate( _
                ByVal strFields() As String, _
                ByVal strTable As String, _
                ByVal vSetValues() As String, _
                Optional ByVal strWhere As String = "" _
    ) As Long Implements IDBOperate.DoUpdate

        Dim cmd As New OleDbCommand
        Dim strSQL As String
        Dim i As Integer

        Try

            'SQL�𐶐�
            strSQL = "UPDATE " & strTable

            For i = 0 To UBound(strFields)
                If i = 0 Then
                    strSQL = strSQL & " SET "
                Else
                    strSQL = strSQL & ", "
                End If

                If vSetValues(i) = "" Then
                    strSQL = strSQL & strFields(i) & "=Null"
                Else
                    strSQL = strSQL & strFields(i) & "=" & vSetValues(i)
                End If
            Next

            If Len(strWhere) > 0 Then
                strSQL = strSQL & " WHERE " & strWhere
            End If

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteNonQuery()

        Catch
            Return -1
        End Try
    End Function

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
    ) As Long Implements IDBOperate.DoInsert

        Dim intFieldCnt As Integer
        Dim i As Integer
        Dim strInsert As String = ""
        Dim strValue As String = ""
        Dim cmd As New OleDbCommand

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

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strInsert & ")" & strValue & ")"
            cmd.Transaction = m_Transaction

            Return cmd.ExecuteNonQuery()

        Catch
            Return -1
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       DataSet�I�u�W�F�N�g���擾����
    '
    '   <�߂�l>
    '       DataSet�I�u�W�F�N�g�̎擾
    '
    '   <���@��>
    '       strSQL      [I/ ]   ����SQL
    '       strTable    [I/ ]   �Ώۃe�[�u����
    '       bPropertyFlg[I/ ]   �����擾Flg
    '--------------------------------------------------------------------------------
    '---Rep.Start-----------------------------------------(2007.08.17) kawasaki
    'Public Function DoDataSet( _
    '                ByVal strSql As String, _
    '                ByVal strTable As String _
    ') As DataSet Implements IDBOperate.DoDataSet
    '-----------------------------------------------------
    Function DoDataSet( _
                    ByVal strSQL As String, _
                    Optional ByVal strTable As String = "", _
                    Optional ByVal bPropertyFlg As Boolean = False _
                    ) As DataSet Implements IDBOperate.DoDataSet
        '---Rep. End -----------------------------------------(2007.08.17) kawasaki
        Try

            m_daOleDB = Nothing
            m_dsOleDB = Nothing

            m_daOleDB = New OleDbDataAdapter(strSQL, m_cnnString)
            m_dsOleDB = New DataSet
            '---Ins.Start-----------------------------------------(2007.08.17) kawasaki
            '�������擾����
            If bPropertyFlg = True Then
                m_daOleDB.MissingSchemaAction = MissingSchemaAction.AddWithKey
            End If
            '---Ins. End -----------------------------------------(2007.08.17) kawasaki
            If strTable.Length <> 0 Then
                m_daOleDB.Fill(m_dsOleDB, strTable)
                Return m_dsOleDB
            Else
                m_daOleDB.Fill(m_dsOleDB)
                Return m_dsOleDB
            End If

        Catch
            Return Nothing
        End Try
    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       �f�[�^�^�ɂ�鎯�ʎq�ǉ�
    '
    '   <�߂�l>
    '       �f�[�^���ʎq�t��������
    '
    '   <���@��>
    '       strCellValue    [I/ ]   �f�[�^
    '       strCellType     [I/ ]   �f�[�^�^�C�v
    '--------------------------------------------------------------------------------
    Public Function checkDataType( _
                        ByVal strCellValue As String, _
                        ByVal strCellType As String _
    ) As String Implements IDBOperate.checkDataType

        If strCellValue.Length > 0 Then
            Select Case strCellType
                Case "String", "System.String"
                    Return "'" & strCellValue & "'"
                Case "DateTime", "System.DateTime"
                    Return "#" & strCellValue & "#"
                Case Else
                    Return strCellValue
            End Select
        Else
            Return ""
        End If

    End Function

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       �v���p�e�B�l(DB�R�l�N�V�����I�u�W�F�N�g)
    '
    '   <�߂�l>
    '       �Ȃ�
    '
    '   <���@��>
    '       �Ȃ�
    '--------------------------------------------------------------------------------
    Public Property DbConnection() As System.Data.IDbConnection Implements IDBOperate.DbConnection
        Get
            Return m_cnnOLEDB
        End Get
        Set(ByVal value As System.Data.IDbConnection)
            m_cnnOLEDB = CType(value, OleDbConnection)
        End Set
    End Property


    '---Ins.Start-----------------------------------------(2007.09.28) kawasaki
    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       �v���p�e�B�l(DB�g�����U�N�V�����I�u�W�F�N�g)
    '
    '   <�߂�l>
    '       �Ȃ�
    '
    '   <���@��>
    '       �Ȃ�
    '--------------------------------------------------------------------------------
    Public Property DbTansaction() As System.Data.IDbTransaction Implements IDBOperate.DbTansaction
        Get
            Return m_Transaction
        End Get
        Set(ByVal value As System.Data.IDbTransaction)
            m_Transaction = CType(value, OleDbTransaction)
        End Set
    End Property
    '---Ins. End -----------------------------------------(2007.09.28) kawasaki

    '<!--2007/12/13 T44
    Public Function getConnectionObject() As Object Implements IDBOperate.getConnectionObject
        Return Me.m_cnnOLEDB
    End Function
    '-->

    '---Ins.Start-----------------------------------------(2008.02.29) kawasaki
    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       ���R�[�h�̑I���i�p�����[�^�t�j
    '
    '   <�߂�l>
    '       DataReader�I�u�W�F�N�g
    '
    '   <���@��>
    '       strSQL      [I/ ]   ����SQL
    '       strParaName()   [I/ ] �p�����[�^��
    '       strParaVal()    [I/ ] �p�����[�^�l
    '--------------------------------------------------------------------------------
    Public Function DoSelectParameter(ByVal strSQL As String, ByVal strParaName() As String, ByVal objParaVal() As Object) As System.Data.IDataReader Implements IDBOperate.DoSelectParameter

        Dim cmd As New OleDbCommand

        Try

            cmd.Connection = m_cnnOLEDB
            cmd.CommandText = strSQL
            cmd.Transaction = m_Transaction
            For i As Integer = 0 To strParaName.Length - 1
                cmd.Parameters.Add(New OleDbParameter(strParaName(i), objParaVal(i)))
            Next

            Return cmd.ExecuteReader

        Catch
            Return Nothing
        End Try
    End Function
    '---Ins. End -----------------------------------------(2008.02.29) kawasaki

    '---Ins.Start-----------------------------------------(2008.04.18) kawasaki
    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       Blob�^�̃f�[�^Insert
    '
    '   <�߂�l>
    '       True    ����I��
    '       False   �ُ�I��
    '
    '   <���@��>
    '       strSQL          [I/ ]   ���sSQL
    '       strParaName     [I/ ]   �p�����[�^��
    '       strParaVal()    [I/ ]   �p�����[�^�l
    '--------------------------------------------------------------------------------
    Public Function DoInsert_Blob( _
                                ByVal strSQL As String, _
                                ByVal strParaName As String, _
                                ByVal bytParaVal As Byte() _
    ) As Long Implements IDBOperate.DoInsert_Blob

    End Function


    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       �v���p�e�B�i�ڑ�������j
    '
    '   <�߂�l>
    '       �Ȃ�
    '
    '   <���@��>
    '       �Ȃ�
    '--------------------------------------------------------------------------------
    Public Property DBConnectionString() As String Implements IDBOperate.DBConnectionString
        Get
            Return m_cnnString
        End Get
        Set(ByVal value As String)
            m_cnnString = value
        End Set
    End Property

    '--------------------------------------------------------------------------------
    '   <�@�@�\>
    '       �v���p�e�B�i�G���[���b�Z�[�W�j
    '
    '   <�߂�l>
    '       �Ȃ�
    '
    '   <���@��>
    '       �Ȃ�
    '--------------------------------------------------------------------------------
    Public ReadOnly Property ErrorMessage() As String Implements IDBOperate.ErrorMessage
        Get
            If m_strErrMsg.Chars(m_strErrMsg.Length - 1) = Chr(10) Then
                m_strErrMsg = Mid(m_strErrMsg, 1, m_strErrMsg.Length - 1)
            End If
            Return m_strErrMsg
        End Get
    End Property
    '---Ins. End -----------------------------------------(2008.04.18) kawasaki

    Public Function ExecuteSQL3(ByVal strSQL As String) As Long Implements IDBOperate.ExecuteSQL3

    End Function

    'Public Function ExecuteSQL4(ByVal strSQL As String) As Object Implements IDBOperate.ExecuteSQL4

    'End Function
End Class

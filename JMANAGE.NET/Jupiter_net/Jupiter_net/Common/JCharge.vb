'Attribute VB_Name = "JCharge"
Option Explicit On


Imports System
Imports System.Data
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.IO

Module JCharge

    '***************************************************************************************************
    '�g�p���Ă���WindowsAPI�̐錾
    '***************************************************************************************************
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" _
        ( _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpApplicationName As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpKeyName As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpDefault As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpReturnedString As StringBuilder, _
            ByVal nSize As Integer, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpFileName As String _
        ) As Long

    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" _
        ( _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpApplicationName As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpKeyName As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpString As String, _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpFileName As String _
        ) As Long

    Private Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" _
        ( _
            <MarshalAs(UnmanagedType.LPStr)> ByVal lpBuffer As String, _
            ByVal nSize As Long _
        ) As Long

    '***************************************************************************************************
    'հ�ް��`�萔�̐錾
    '***************************************************************************************************
    '***************************************************************************************************
    '�ϐ����� ���e���ނ�'\0'�}���p��1�傫���ݒ肷����̂Ƃ���B���̂Ƃ��A�ϐ����ނ���ƂȂ�B
    '�����VB�œǂݏ���������ް������������Ȃ邽��(�����s��)�A�����1�傫���ݒ肷��B
    '***************************************************************************************************
    Private Const CHARGE_SIZE = 10       '����޺��޻���
    Private Const KAISHA_SIZE = 52       '��Ж�����
    Private Const KJCODE_SIZE = 18       '�H�����޻���
    Private Const KJMEI_SIZE = 52        '�H��������
    Private Const HACCHU_SIZE = 52       '�����һ���
    Private Const TANTOU_SIZE = 22       '�����S���һ���
    Private Const DATE_MAX = 10          '���t,�d�ʌJ��Ԃ���
    Private Const DATE_SIZE = 12         '���t����

    Private Const MAX_CHAR = 256        '�ő啶����

    Private Const FILE_SIZE = 954     'CHARGE�\���̃T�C�Y

    '***************************************************************************************************
    'հ�ް��`�\���̂̐錾
    '***************************************************************************************************
    '---------------------------------------------------------------------------------------------------
    '���t�p�\����
    '---------------------------------------------------------------------------------------------------
    Public Structure sdate
        <VBFixedStringAttribute(DATE_SIZE)> Dim strdate As String
    End Structure

    '---------------------------------------------------------------------------------------------------
    'CHARGE.INI�ް��p�\����
    '---------------------------------------------------------------------------------------------------
    Public Structure CHARGE
        <VBFixedStringAttribute(CHARGE_SIZE)> Dim charge_code As String   '����޺���
        <VBFixedStringAttribute(KAISHA_SIZE)> Dim kaisha_mei As String    '��Ж�
        <VBFixedStringAttribute(KJCODE_SIZE)> Dim kouji_code As String    '�H������
        <VBFixedStringAttribute(KJMEI_SIZE)> Dim kouji_mei As String      '�H����
        <VBFixedStringAttribute(HACCHU_SIZE)> Dim hacchu_sha As String    '������
        Dim keisiki As Integer                                               '�����`��
        Dim gairyaku_juuryou As Double                                    '�T���d��(�P��:ton)
        Dim sikan_suu As Integer                                             '�x�Ԑ�
        Dim shuketa_daisuu As Integer                                        '�包�䐔
        Dim deck_daisuu As Integer                                           '�ޯ��䐔
        Dim yokoketa_daisuu As Integer                                       '�����䐔
        Dim brket_daisuu As Integer                                          '��׹�đ䐔
        Dim nakatate_daisuu As Integer                                       '���c���䐔
        Dim sokutate_daisuu As Integer                                       '���c���䐔
        Dim taikeikou_daisuu As Integer                                      '�΍��\�䐔
        Dim yokokou As Integer                                               '���\�L��(1:��,2:�L)
        <VBFixedStringAttribute(DATE_SIZE)> Dim shori_kaisi As String     '�����J�n�\���
        <VBFixedStringAttribute(DATE_SIZE)> Dim shori_shuuryou As String  '�����I���\���
        <VBFixedStringAttribute(TANTOU_SIZE)> Dim shori_tantou As String  '�����S����
        Dim skl_shori_flg As Integer                                         '���g�ꊇ�������s�׸�(1:�s��,2:��)
        Dim tenkai_flg As Integer                                            '�W�J�������s�׸�(1:�s��,2:��)
        <VBFixedStringAttribute(DATE_SIZE)> Dim sinki_touroku As String   '�V�K�o�^������
        Dim skl_shori_kaisuu As Integer                                      '���g�ꊇ�������s��
        Dim skl_shori_date() As sdate                                     '���g�ꊇ�������s��
        Dim buzai_shori_kaisuu As Integer                                    '���ވꊇ�������s��
        Dim buzai_shori_date() As sdate                                   '���ވꊇ�������s��
        Dim tenkai_shori_kaisuu As Integer                                   '�W�J�������s��
        Dim tenkai_shori_date() As sdate                                  '�W�J�������s��
        Dim tenkai_jyuuryou() As Double                                   '�W�J�������PMF�̏d��
        Dim bunrui_shori_kaisuu As Integer                                   '���ޏ������s��
        Dim bunrui_shori_date() As sdate                                  '���ޏ������s��
        Dim bunrui_jyuuryou() As Double                                   '���ޏ������PMF�̏d��
        Public Sub Initialize()
            ReDim skl_shori_date(DATE_MAX)           '���g�ꊇ�������s��
            ReDim buzai_shori_date(DATE_MAX)         '���ވꊇ�������s��
            ReDim tenkai_shori_date(DATE_MAX)        '�W�J�������s��
            ReDim tenkai_jyuuryou(DATE_MAX)          '�W�J�������PMF�̏d��
            ReDim bunrui_shori_date(DATE_MAX)        '���ޏ������s��
            ReDim bunrui_jyuuryou(DATE_MAX)          '���ޏ������PMF�̏d��
        End Sub
    End Structure

    '---------------------------------------------------------------------------------------------------
    'BLOCK.INF�ް��p�\����
    '---------------------------------------------------------------------------------------------------
    Structure KOZO
        Public shuketa_daisuu As Long          '�包�䐔
        Public deck_daisuu As Long             '�ޯ��䐔
        Public yokoketa_daisuu As Long         '�����䐔
        Public brket_daisuu As Long            '��׹�đ䐔
        Public tateketa_daisuu As Long         '�c��(���A��)�䐔
        Public taikeikou_daisuu As Long        '�΍��\�䐔
        Public yokokou As Long                 '���\�L��(1:��,2:�L)
    End Structure

    Public Sub subCHARGE�V�K���f�[�^�̊i�[(Kogo As String)
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ���H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��݁A
        '   ����޺��ޓo�^̧�ق̂��̍H���̐V�K�o�^���ɓ��t���i�[����B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, recno As Integer
        Dim ProFile As String, fname As String
        Dim today As String
        Dim ans As StringBuilder = New StringBuilder(MAX_CHAR)
        Dim msg As String
        Dim ret As Long
        Dim RecData As New CHARGE()
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans, MAX_CHAR, ProFile)
        'recno = Left(ans, InStr(ans, Chr(0)) - 1)
        recno = ans.ToString().Trim()

        If recno <= 0 Then
            Exit Sub
        End If

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A�V�K�o�^�����������ށB
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & vbCrLf
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI). " & vbCrLf
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Sub
        End If

        fnum = FreeFile()
        'Open fname For Random Access Read Write As #fnum Len = Len(RecData)
        'Get #fnum, recno, RecData
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , FILE_SIZE)
        FileGet(fnum, RecData, recno)

        '���݂̓��t���擾����B
        'today = VB6.Format(Now, "yyyy/mm/dd")
        today = DateTime.Now.ToString("yyyy/MM/dd")

        '�V�K�o�^�����i�[����B
        RecData.sinki_touroku = today

        'Put #fnum, recno, RecData
        'Close #fnum
        FilePut(recno, RecData)
        FileClose(fnum)

    End Sub

    Public Sub subBLOCKINF����e��\���̃f�[�^�Z�o(Kogo As String, KData As KOZO)
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ�����ۯ����̧�ق���\���Ǝ�ʂ�ǂݍ��݁A
        '   �e��\���̑䐔�i�包�A�ޯ��A�����A��׹�āA�c��(���A��)�j�ƗL���i���\�j���Z�o���AKOZO�\���̂Ɋi�[����B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '   KDate(O)�F�e��\���ް��i�\���́j
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim fname As String
        Dim kz As String, kind As String
        Dim buf As String, msg As String
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '�\���̂�����������B
        KData.shuketa_daisuu = 0
        KData.deck_daisuu = 0
        KData.yokoketa_daisuu = 0
        KData.brket_daisuu = 0
        KData.tateketa_daisuu = 0
        KData.taikeikou_daisuu = 0
        KData.yokokou = 1

        '��ۯ����̧�ق���\���Ǝ�ʂ�ǂݍ��݁A�e��\�����ް����Z�o����B
        fname = FunGetNewKoji() & Kogo & "\BLOCK.INF"
        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "��ۯ����̧��(BLOCK.INF)������܂���B" & Chr(10)
                msg = msg & "���ވꊇ����������ɏI�����Ă��܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no block information file (BLOCK.INF)." & Chr(10)
                msg = msg & "Component batch processing is not completed normally." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Sub
        End If

        'fnum = FreeFile()
        ''Open fname For Input As #fnum
        'FileOpen(fnum, fname, OpenMode.Input, OpenAccess.Read)

        'Do While Not EOF(fnum)
        'Line Input #fnum, buf

        '    kz = UCase(J_ChoiceString(buf, 2))      '�\��
        '    kind = UCase(J_ChoiceString(buf, 7))    '���

        '    If ((kz = "GRD") Or (kz = "KYAKU") Or (kz = "HARI")) And (kind = "BLK") Then        '�包�䐔
        '        KData.shuketa_daisuu = KData.shuketa_daisuu + 1
        '    ElseIf ((kz = "DECK") Or (kz = "GDK")) And (kind = "BLK") Then    '�ޯ��䐔�܂��͒�|�䐔
        '        KData.deck_daisuu = KData.deck_daisuu + 1
        '    ElseIf (kz = "CRS") And (kind = "BLK") Then     '�����䐔
        '        KData.yokoketa_daisuu = KData.yokoketa_daisuu + 1
        '    ElseIf (kz = "BRK") And (kind = "BLK") Then     '��׹�đ䐔
        '        KData.brket_daisuu = KData.brket_daisuu + 1
        '    ElseIf ((kz = "STR") Or (kz = "GST")) And (kind = "BLK") Then   '�c��(���A��)�䐔�܂��͑��|�䐔
        '        KData.tateketa_daisuu = KData.tateketa_daisuu + 1
        '    ElseIf (kz = "SWY") And (kind = "BLK") Then     '�΍��\�䐔
        '        KData.taikeikou_daisuu = KData.taikeikou_daisuu + 1
        '    ElseIf kz = "LATE" Then                         '���\�L��
        '        KData.yokokou = 2
        '    End If
        'Loop
        'Close #fnum
        Dim blockInf As New StreamReader(fname, Encoding.GetEncoding("Shift_JIS"))
        Dim blockNameList As New List(Of String)
        While (blockInf.Peek() >= 0)
            buf = blockInf.ReadLine()

            kz = UCase(J_ChoiceString(buf, 2))      '�\��
            kind = UCase(J_ChoiceString(buf, 7))    '���

            If ((kz = "GRD") Or (kz = "KYAKU") Or (kz = "HARI")) And (kind = "BLK") Then        '�包�䐔
                KData.shuketa_daisuu = KData.shuketa_daisuu + 1
            ElseIf ((kz = "DECK") Or (kz = "GDK")) And (kind = "BLK") Then    '�ޯ��䐔�܂��͒�|�䐔
                KData.deck_daisuu = KData.deck_daisuu + 1
            ElseIf (kz = "CRS") And (kind = "BLK") Then     '�����䐔
                KData.yokoketa_daisuu = KData.yokoketa_daisuu + 1
            ElseIf (kz = "BRK") And (kind = "BLK") Then     '��׹�đ䐔
                KData.brket_daisuu = KData.brket_daisuu + 1
            ElseIf ((kz = "STR") Or (kz = "GST")) And (kind = "BLK") Then   '�c��(���A��)�䐔�܂��͑��|�䐔
                KData.tateketa_daisuu = KData.tateketa_daisuu + 1
            ElseIf (kz = "SWY") And (kind = "BLK") Then     '�΍��\�䐔
                KData.taikeikou_daisuu = KData.taikeikou_daisuu + 1
            ElseIf kz = "LATE" Then                         '���\�L��
                KData.yokokou = 2
            End If
        End While
        blockInf.Close()

    End Sub

    '2007/10/09 Nakagawa Add Start
    Public Sub subBLOCKINF����e��\���̃f�[�^�Z�o_�t�@�C�����w��(Kogo As String, KData As KOZO, blockInfName As String)
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ�����ۯ����̧�ق���\���Ǝ�ʂ�ǂݍ��݁A
        '   �e��\���̑䐔�i�包�A�ޯ��A�����A��׹�āA�c��(���A��)�j�ƗL���i���\�j���Z�o���AKOZO�\���̂Ɋi�[����B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '   KDate(O)�F�e��\���ް��i�\���́j
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim fname As String
        Dim kz As String, kind As String
        Dim buf As String, msg As String
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '�\���̂�����������B
        KData.shuketa_daisuu = 0
        KData.deck_daisuu = 0
        KData.yokoketa_daisuu = 0
        KData.brket_daisuu = 0
        KData.tateketa_daisuu = 0
        KData.taikeikou_daisuu = 0
        KData.yokokou = 1

        '��ۯ����̧�ق���\���Ǝ�ʂ�ǂݍ��݁A�e��\�����ް����Z�o����B
        fname = FunGetNewKoji() & Kogo & "\" & blockInfName
        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "��ۯ����̧��(" & blockInfName & ")������܂���B" & Chr(10)
                msg = msg & "���ވꊇ����������ɏI�����Ă��܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no block information file (BLOCK.INF)." & Chr(10)
                msg = msg & "Component batch processing is not completed normally." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Sub
        End If

        'fnum = FreeFile()
        'Open fname For Input As #fnum
        'Do While Not EOF(fnum)
        'Line Input #fnum, buf

        '    kz = UCase(J_ChoiceString(buf, 2))      '�\��
        '    kind = UCase(J_ChoiceString(buf, 7))    '���

        '    If (kz = "GRD") And (kind = "BLK") Then         '�包�䐔
        '        KData.shuketa_daisuu = KData.shuketa_daisuu + 1
        '    ElseIf ((kz = "DECK") Or (kz = "GDK")) And (kind = "BLK") Then    '�ޯ��䐔�܂��͒�|�䐔
        '        KData.deck_daisuu = KData.deck_daisuu + 1
        '    ElseIf (kz = "CRS") And (kind = "BLK") Then     '�����䐔
        '        KData.yokoketa_daisuu = KData.yokoketa_daisuu + 1
        '    ElseIf (kz = "BRK") And (kind = "BLK") Then     '��׹�đ䐔
        '        KData.brket_daisuu = KData.brket_daisuu + 1
        '    ElseIf ((kz = "STR") Or (kz = "GST")) And (kind = "BLK") Then     '�c��(���A��)�䐔�܂��͑��|�䐔
        '        KData.tateketa_daisuu = KData.tateketa_daisuu + 1
        '    ElseIf (kz = "SWY") And (kind = "BLK") Then     '�΍��\�䐔
        '        KData.taikeikou_daisuu = KData.taikeikou_daisuu + 1
        '    ElseIf kz = "LATE" Then                         '���\�L��
        '        KData.yokokou = 2
        '    End If
        'Loop
        'Close #fnum
        Dim blockInf As New StreamReader(fname, Encoding.GetEncoding("Shift_JIS"))
        Dim blockNameList As New List(Of String)
        While (blockInf.Peek() >= 0)
            buf = blockInf.ReadLine()

            kz = UCase(J_ChoiceString(buf, 2))      '�\��
            kind = UCase(J_ChoiceString(buf, 7))    '���

            If (kz = "GRD") And (kind = "BLK") Then         '�包�䐔
                KData.shuketa_daisuu = KData.shuketa_daisuu + 1
            ElseIf ((kz = "DECK") Or (kz = "GDK")) And (kind = "BLK") Then    '�ޯ��䐔�܂��͒�|�䐔
                KData.deck_daisuu = KData.deck_daisuu + 1
            ElseIf (kz = "CRS") And (kind = "BLK") Then     '�����䐔
                KData.yokoketa_daisuu = KData.yokoketa_daisuu + 1
            ElseIf (kz = "BRK") And (kind = "BLK") Then     '��׹�đ䐔
                KData.brket_daisuu = KData.brket_daisuu + 1
            ElseIf ((kz = "STR") Or (kz = "GST")) And (kind = "BLK") Then     '�c��(���A��)�䐔�܂��͑��|�䐔
                KData.tateketa_daisuu = KData.tateketa_daisuu + 1
            ElseIf (kz = "SWY") And (kind = "BLK") Then     '�΍��\�䐔
                KData.taikeikou_daisuu = KData.taikeikou_daisuu + 1
            ElseIf kz = "LATE" Then                         '���\�L��
                KData.yokokou = 2
            End If
        End While
        blockInf.Close()

    End Sub
    '2007/10/09 Nakagawa Add End

    Public Function funBLOCKINF��DWG�t�@�C���̃`�F�b�N(Kogo As String) As Boolean
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ�����ۯ����̧�ق�����ۯ��O������ǂݍ��݁A
        '   ��ۯ��O�����ɑΉ�����DWĢ�ق����邩���肷��B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '�߂�l
        '   True�F��ۯ����̧�ٓ��̑S�Ă���ۯ��O�����ɑ΂���DWĢ�ق����݂���B
        '   False�F��ۯ����̧�ٓ�����ۯ��O�����ɑ΂���DWĢ�ق����݂��Ȃ����̂�����B
        '---------------------------------------------------------------------------------------------------
        Dim fname As String, kogodir As String
        Dim blkname As String, dwgname As String
        Dim buf As String, msg As String
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funBLOCKINF��DWG�t�@�C���̃`�F�b�N = False
        kogodir = FunGetNewKoji() & Kogo

        '��ۯ����̧�ق�����ۯ��O������ǂݍ��݁A������ۯ��O������DWĢ�ق���������B
        fname = kogodir & "\BLOCK.INF"
        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "��ۯ����̧��(BLOCK.INF)������܂���B" & Chr(10)
                msg = msg & "���ވꊇ����������ɏI�����Ă��܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no block information file (BLOCK.INF)." & Chr(10)
                msg = msg & "Component batch processing is not completed normally." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Function
        End If

        'fnum = FreeFile()
        'Open fname For Input As #fnum
        'Do While Not EOF(fnum)
        'Line Input #fnum, buf

        '    blkname = J_ChoiceString(buf, 1)   '��ۯ��O����
        '    dwgname = kogodir & "\" & blkname & ".DBA"
        '    If Dir(dwgname, vbNormal) = "" Then
        '        Beep()
        '        If Not strFileName Like "*ENG.INI" Then
        '            msg = "��ۯ����̧�قɋL�q����Ă���" & blkname & "��ۯ��ɑΉ�����DWĢ�ق�����܂���B"
        '        Else
        '            msg = "There is no DWG file corresponding to " & blkname & "Brock described by the Brock information file."
        '        End If
        '        MsgBox(msg, vbOKOnly + vbExclamation)
        '        Exit Function
        '    End If
        'Loop
        'Close #fnum
        Dim blockInf As New StreamReader(fname, Encoding.GetEncoding("Shift_JIS"))
        Dim blockNameList As New List(Of String)
        While (blockInf.Peek() >= 0)
            buf = blockInf.ReadLine()
            blkname = J_ChoiceString(buf, 1)   '��ۯ��O����
            dwgname = kogodir & "\" & blkname & ".DBA"
            If Dir(dwgname, vbNormal) = "" Then
                Beep()
                If Not strFileName Like "*ENG.INI" Then
                    msg = "��ۯ����̧�قɋL�q����Ă���" & blkname & "��ۯ��ɑΉ�����DWĢ�ق�����܂���B"
                Else
                    msg = "There is no DWG file corresponding to " & blkname & "Brock described by the Brock information file."
                End If
                MsgBox(msg, vbOKOnly + vbExclamation)
                Exit Function
            End If
        End While
        blockInf.Close()

        funBLOCKINF��DWG�t�@�C���̃`�F�b�N = True

    End Function

    Public Function funCHARGE�䐔�̃`�F�b�N(Kogo As String, intChgNum As Integer, intBlkNum As Integer) As Integer
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ�����ۯ����̧�ق���Z�o�����e��\���̑䐔�i���\�͗L���j�ƁA
        '   ����޺��ޓo�^̧�ق̊e��\���̑䐔�i���\�͗L���j�������Ă��邩���肷��B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '�߂�l
        '   0�F����޺��ނ�FREE-JUPITER�̂��߁A�䐔���������s��Ȃ��B�܂��́A�e��\���̑䐔�������Ă���B
        '   1�F����޺��ނ��o�^̧�قɓo�^����Ă��Ȃ��B
        '   2�F�包�̑䐔���Ⴄ�B
        '   3�F�ޯ��̑䐔���Ⴄ�B
        '   4�F�����̑䐔���Ⴄ�B
        '   5�F��׹�Ă̑䐔���Ⴄ�B
        '   6�F�c���̑䐔���Ⴄ�B
        '   7�F�ΌX�\�̑䐔���Ⴄ�B
        '   8�F���\�̗L�����Ⴄ�B
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, recno As Integer
        Dim ProFile As String, fname As String
        'Dim ans As String * MAX_CHAR, msg As String
        'Dim strWindowsDirectoryBuffer As String * 260
        Dim ans As StringBuilder = New StringBuilder(MAX_CHAR)
        Dim msg As String
        Dim ret As Long, tateketa As Long
        Dim RecData As New CHARGE()
        Dim KozoData As KOZO
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE�䐔�̃`�F�b�N = 1

        '��ۯ����̧�ق���e��\���̑䐔���Z�o����B
        Call subBLOCKINF����e��\���̃f�[�^�Z�o(Kogo, KozoData)

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans, MAX_CHAR, ProFile)
        'recno = Left(ans, InStr(ans, Chr(0)) - 1)
        recno = ans.ToString.Trim

        If recno < 0 Then       '�o�^̧�قɓo�^����Ă��Ȃ����ߴװ�I������B
            funCHARGE�䐔�̃`�F�b�N = 1
            Exit Function
        ElseIf recno = 0 Then   '����޺��ނ�FREE-JUPITER�̂��ߐ���I������B
            funCHARGE�䐔�̃`�F�b�N = 0
            Exit Function
        End If

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A�e��\���̑䐔����ۯ����̧�ق���Z�o�����䐔����������B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Function
        End If

        'fnum = FreeFile()
        'Open fname For Random Access Read As #fnum Len = Len(RecData)
        'Get #fnum, recno, RecData
        fnum = FreeFile()
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        Dim tmp As ValueType
        tmp = RecData
        FileGet(fnum, tmp, recno)
        RecData = CType(tmp, CHARGE)
        FileClose(fnum)

        'tateketa = RecData.nakatate_daisuu + RecData.sokutate_daisuu

        funCHARGE�䐔�̃`�F�b�N = 2

        If RecData.shuketa_daisuu = KozoData.shuketa_daisuu Then        '�包�䐔
            If RecData.deck_daisuu = KozoData.deck_daisuu Then              '�ޯ��䐔
                If RecData.yokoketa_daisuu = KozoData.yokoketa_daisuu Then      '�����䐔
                    If RecData.brket_daisuu = KozoData.brket_daisuu Then            '��׹�đ䐔
                        If tateketa = KozoData.tateketa_daisuu Then                     '�c��(���A��)�䐔
                            If RecData.taikeikou_daisuu = KozoData.taikeikou_daisuu Then    '�΍��\�䐔
                                If RecData.yokokou = KozoData.yokokou Then                      '���\�L��
                                    funCHARGE�䐔�̃`�F�b�N = 0
                                Else
                                    funCHARGE�䐔�̃`�F�b�N = 8
                                End If
                            Else
                                intChgNum = RecData.taikeikou_daisuu
                                intBlkNum = KozoData.taikeikou_daisuu
                                funCHARGE�䐔�̃`�F�b�N = 7
                            End If
                        Else
                            intChgNum = tateketa
                            intBlkNum = KozoData.tateketa_daisuu
                            funCHARGE�䐔�̃`�F�b�N = 6
                        End If
                    Else
                        intChgNum = RecData.brket_daisuu
                        intBlkNum = KozoData.brket_daisuu
                        funCHARGE�䐔�̃`�F�b�N = 5
                    End If
                Else
                    intChgNum = RecData.yokoketa_daisuu
                    intBlkNum = KozoData.yokoketa_daisuu
                    funCHARGE�䐔�̃`�F�b�N = 4
                End If
            Else
                intChgNum = RecData.deck_daisuu
                intBlkNum = KozoData.deck_daisuu
                funCHARGE�䐔�̃`�F�b�N = 3
            End If
        Else
            intChgNum = RecData.shuketa_daisuu
            intBlkNum = KozoData.shuketa_daisuu
            funCHARGE�䐔�̃`�F�b�N = 2
        End If

    End Function

    '2007/10/09 Nakagawa Add Start
    Public Function funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w��(Kogo As String, blockInfName As String) As Integer
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ�����ۯ����̧�ق���Z�o�����e��\���̑䐔�i���\�͗L���j�ƁA
        '   ����޺��ޓo�^̧�ق̊e��\���̑䐔�i���\�͗L���j�������Ă��邩���肷��B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '�߂�l
        '   0�F����޺��ނ�FREE-JUPITER�̂��߁A�䐔���������s��Ȃ��B�܂��́A�e��\���̑䐔�������Ă���B
        '   1�F����޺��ނ��o�^̧�قɓo�^����Ă��Ȃ��B
        '   2�F�包�̑䐔���Ⴄ�B
        '   3�F�ޯ��̑䐔���Ⴄ�B
        '   4�F�����̑䐔���Ⴄ�B
        '   5�F��׹�Ă̑䐔���Ⴄ�B
        '   6�F�c���̑䐔���Ⴄ�B
        '   7�F�ΌX�\�̑䐔���Ⴄ�B
        '   8�F���\�̗L�����Ⴄ�B
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, recno As Integer
        Dim ProFile As String, fname As String
        'Dim ans As String * MAX_CHAR, msg As String
        'Dim strWindowsDirectoryBuffer As String * 260
        Dim ans As StringBuilder = New StringBuilder(MAX_CHAR)
        Dim msg As String
        Dim ret As Long, tateketa As Long
        Dim RecData As New CHARGE()
        Dim KozoData As KOZO
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 1

        '��ۯ����̧�ق���e��\���̑䐔���Z�o����B
        Call subBLOCKINF����e��\���̃f�[�^�Z�o_�t�@�C�����w��(Kogo, KozoData, blockInfName)

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans, MAX_CHAR, ProFile)
        'recno = Left(ans, InStr(ans, Chr(0)) - 1)
        recno = ans.ToString.Trim

        If recno < 0 Then       '�o�^̧�قɓo�^����Ă��Ȃ����ߴװ�I������B
            funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 1
            Exit Function
        ElseIf recno = 0 Then   '����޺��ނ�FREE-JUPITER�̂��ߐ���I������B
            funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 0
            Exit Function
        End If

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A�e��\���̑䐔����ۯ����̧�ق���Z�o�����䐔����������B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Function
        End If

        'fnum = FreeFile()
        'Open fname For Random Access Read As #fnum Len = Len(RecData)
        'Get #fnum, recno, RecData
        fnum = FreeFile()
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        Dim tmp As ValueType
        tmp = RecData
        FileGet(fnum, tmp, recno)
        RecData = CType(tmp, CHARGE)
        FileClose(fnum)

        tateketa = RecData.nakatate_daisuu + RecData.sokutate_daisuu

        funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 2

        If RecData.shuketa_daisuu = KozoData.shuketa_daisuu Then        '�包�䐔
            If RecData.deck_daisuu = KozoData.deck_daisuu Then              '�ޯ��䐔
                If RecData.yokoketa_daisuu = KozoData.yokoketa_daisuu Then      '�����䐔
                    If RecData.brket_daisuu = KozoData.brket_daisuu Then            '��׹�đ䐔
                        If tateketa = KozoData.tateketa_daisuu Then                     '�c��(���A��)�䐔
                            If RecData.taikeikou_daisuu = KozoData.taikeikou_daisuu Then    '�΍��\�䐔
                                If RecData.yokokou = KozoData.yokokou Then                      '���\�L��
                                    funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 0
                                Else
                                    funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 8
                                End If
                            Else
                                funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 7
                            End If
                        Else
                            funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 6
                        End If
                    Else
                        funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 5
                    End If
                Else
                    funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 4
                End If
            Else
                funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 3
            End If
        Else
            funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 2
        End If

    End Function
    '2007/10/09 Nakagawa Add End

    Public Sub subCHARGE���s�f�[�^�̊i�[(Kogo As String, ftyp As Integer)
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ���H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��݁A
        '   ����޺��ޓo�^̧�ق̂��̍H���̍��g�A���ށA�W�J�A���ނ̊e�����̎��s�ް����X�V����B
        '   �W�J�A���ނ̂Ƃ��́A�d��۸�̧�ق��瑍�d�ʂ�ǂݍ��ށB
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '   ftyp(I)�F���s�ް��i���s�񐔁A���s���APMF�d�ʁj���i�[���鍀������
        '            1�F���g�ꊇ����
        '            2�F���ވꊇ����
        '            3�F�W�J����
        '            4�F���ޏ���
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, recno As Integer, cnt As Integer
        Dim ProFile As String, fname As String, WinDir As String
        Dim today As String, pmfweight As String
        'Dim ans As String * MAX_CHAR, msg As String
        'Dim strWindowsDirectoryBuffer As String * 260
        Dim ans As StringBuilder = New StringBuilder(MAX_CHAR)
        Dim msg As String
        Dim ret As Long
        Dim RecData As CHARGE
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans, MAX_CHAR, ProFile)
        'recno = Left(ans, InStr(ans, Chr(0)) - 1)
        recno = ans.ToString.Trim

        If recno <= 0 Then
            Exit Sub
        End If

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A�e�����̎��s�ް����X�V���A�������ށB
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Sub
        End If

        'fnum = FreeFile()
        'Open fname For Random Access Read Write As #fnum Len = Len(RecData)
        'Get #fnum, recno, RecData

        ''�e�����̎��s�񐔂��X�V����B
        'If ftyp = 1 Then        '���g�ꊇ����
        '    If RecData.skl_shori_kaisuu < 0 Then RecData.skl_shori_kaisuu = 0
        '    RecData.skl_shori_kaisuu = RecData.skl_shori_kaisuu + 1
        '    cnt = RecData.skl_shori_kaisuu
        'ElseIf ftyp = 2 Then    '���ވꊇ����
        '    If RecData.buzai_shori_kaisuu < 0 Then RecData.buzai_shori_kaisuu = 0
        '    RecData.buzai_shori_kaisuu = RecData.buzai_shori_kaisuu + 1
        '    cnt = RecData.buzai_shori_kaisuu
        'ElseIf ftyp = 3 Then    '�W�J����
        '    If RecData.tenkai_shori_kaisuu < 0 Then RecData.tenkai_shori_kaisuu = 0
        '    RecData.tenkai_shori_kaisuu = RecData.tenkai_shori_kaisuu + 1
        '    cnt = RecData.tenkai_shori_kaisuu
        'ElseIf ftyp = 4 Then    '���ޏ���
        '    If RecData.bunrui_shori_kaisuu < 0 Then RecData.bunrui_shori_kaisuu = 0
        '    RecData.bunrui_shori_kaisuu = RecData.bunrui_shori_kaisuu + 1
        '    cnt = RecData.bunrui_shori_kaisuu
        'End If

        ''���t�Əd�ʂ��i�[����z��ԍ����Z�o����B
        'cnt = cnt Mod DATE_MAX
        'If cnt = 0 Then
        '    cnt = 10
        'End If

        ''���݂̓��t���擾����B
        'today = Format(Now, "yyyy/mm/dd")

        ''�W�J�����܂��͕��ޏ����̂Ƃ��A�H�������̧�ق��瑍�d�ʂ�ǂݍ��ށB
        'If (ftyp = 3) Or (ftyp = 4) Then
        '    ret = GetPrivateProfileString("��������", "���d��(Kg)", "NO-WEIGHT", ans, MAX_CHAR, ProFile)
        '    pmfweight = Left(ans, InStr(ans, Chr(0)) - 1)
        '    If pmfweight = "NO-WEIGHT" Then
        '        Beep()
        '        If Not strFileName Like "*ENG.INI" Then
        '            msg = "�d�ʂ̎Z�o������Ă��Ȃ����߁A���d�ʂ�������܂���B" & Chr(10)
        '            msg = msg & "���d�ʂ��ۂƂ��܂��B"
        '        Else
        '            msg = "Since calculation of weight is not carried out, gross weight is not known." & Chr(10)
        '            msg = msg & "Let gross weight be zero."
        '        End If
        '        MsgBox(msg, vbOKOnly + vbExclamation)
        '        pmfweight = "0.0"
        '    End If
        'End If

        ''�e�����̎��s���APMF�d�ʂ��i�[����B
        'If ftyp = 1 Then        '���g�ꊇ����
        '    RecData.skl_shori_date(cnt).strdate = today
        'ElseIf ftyp = 2 Then    '���ވꊇ����
        '    RecData.buzai_shori_date(cnt).strdate = today
        'ElseIf ftyp = 3 Then    '�W�J����
        '    RecData.tenkai_shori_date(cnt).strdate = today
        '    RecData.tenkai_jyuuryou(cnt) = CDbl(pmfweight)
        'ElseIf ftyp = 4 Then    '���ޏ���
        '    RecData.bunrui_shori_date(cnt).strdate = today
        '    RecData.bunrui_jyuuryou(cnt) = CDbl(pmfweight)
        'End If

        'Put #fnum, recno, RecData
        'Close #fnum

    End Sub

    Public Function funCHARGE�����`���̎擾(Kogo As String) As Long
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ���H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��݁A
        '   ����޺��ޓo�^̧�ق̋����`�����擾����B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '�߂�l
        '   �����׸�
        '      -1�F����޺��ޓo�^̧�قɓo�^����Ă��Ȃ��B
        '       0�F�H�������̧�ق�����޺��ނ�"FREE-JUPITER"�ŁA�S�`�������\
        '       1�FRC����
        '       2�F�|���Ŕ���
        '       4�FRC�包
        '      16�F�|�����r
        '      32�F��������
        '      64�F�g�`�E�F�u
        '     128�F�|���Z�O�����g
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, recno As Integer
        Dim ProFile As String, fname As String
        'Dim ans As String * MAX_CHAR, msg As String
        'Dim strWindowsDirectoryBuffer As String * 260
        Dim ans As StringBuilder = New StringBuilder(MAX_CHAR)
        Dim msg As String
        Dim ret As Long
        Dim RecData As New CHARGE()
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE�����`���̎擾 = 1

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans, MAX_CHAR, ProFile)
        'recno = Left(ans, InStr(ans, Chr(0)) - 1)
        recno = ans.ToString.Trim

        If recno < 0 Then       '�o�^̧�قɓo�^����Ă��Ȃ����ߕs�Ƃ���B
            funCHARGE�����`���̎擾 = -1
            Exit Function
        ElseIf recno = 0 Then   '����޺��ނ�FREE-JUPITER�̂��ߑS�`���Ƃ���B
            funCHARGE�����`���̎擾 = 0
            Exit Function
        End If

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A���g�܂��͓W�J�����׸ނ��擾����B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Function
        End If

        'fnum = FreeFile()
        'Open fname For Random Access Read As #fnum Len = Len(RecData)
        'Get #fnum, recno, RecData
        fnum = FreeFile()
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        FileGet(fnum, RecData, recno)
        FileClose(fnum)

        funCHARGE�����`���̎擾 = RecData.keisiki

        '2015/09/15 Nakagawa Add Start Charge.ini�ƃ��C�Z���X�ԍ�������Ȃ��̂Ŗ�����荇�킹��B
        If RecData.keisiki = 8 Then         '��������
            funCHARGE�����`���̎擾 = 32
        ElseIf RecData.keisiki = 32 Then    '�g�`�E�F�u
            funCHARGE�����`���̎擾 = 64
        ElseIf RecData.keisiki = 64 Then    '�|���Z�O�����g
            funCHARGE�����`���̎擾 = 128
        End If
        '2015/09/15 Nakagawa Add End

    End Function

    Public Function funCHARGE�����t���O�̎擾(Kogo As String, ftyp As Integer) As Long
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ���H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��݁A
        '   ����޺��ޓo�^̧�ق̂��̍H���̍��g�܂��͓W�J�̏����׸ނ��擾����B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '   ftyp(I)�F�����׸ނ��擾���鍀������
        '            1�F���g�ꊇ����
        '            2�F�W�J����
        '�߂�l
        '   �����׸�
        '       1�F�s��
        '       2�F��
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, recno As Integer
        Dim ProFile As String, fname As String
        'Dim ans As String * MAX_CHAR, msg As String
        'Dim strWindowsDirectoryBuffer As String * 260
        Dim ans As StringBuilder = New StringBuilder(MAX_CHAR)
        Dim msg As String
        Dim ret As Long
        Dim RecData As New CHARGE()
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE�����t���O�̎擾 = 1

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans, MAX_CHAR, ProFile)
        'recno = Left(ans, InStr(ans, Chr(0)) - 1)
        recno = ans.ToString.Trim

        If recno < 0 Then       '�o�^̧�قɓo�^����Ă��Ȃ����ߕs�Ƃ���B
            funCHARGE�����t���O�̎擾 = 1
            Exit Function
        ElseIf recno = 0 Then   '����޺��ނ�FREE-JUPITER�̂��߉Ƃ���B
            funCHARGE�����t���O�̎擾 = 2
            Exit Function
        End If

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A���g�܂��͓W�J�����׸ނ��擾����B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Function
        End If

        'fnum = FreeFile()
        'Open fname For Random Access Read As #fnum Len = Len(RecData)
        'Get #fnum, recno, RecData
        fnum = FreeFile()
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        Dim tmp As ValueType
        tmp = RecData
        FileGet(fnum, tmp, recno)
        RecData = CType(tmp, CHARGE)
        FileClose(fnum)

        If ftyp = 1 Then        '���g�ꊇ����
            funCHARGE�����t���O�̎擾 = RecData.skl_shori_flg
        ElseIf ftyp = 2 Then    '�W�J����
            funCHARGE�����t���O�̎擾 = RecData.tenkai_flg
        End If

    End Function

    Public Sub subCHARGE�����t���O�̕ύX(Kogo As String, ftyp As Integer, flg As Integer)
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ���H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��݁A
        '   ����޺��ޓo�^̧�ق̂��̍H���̍��g�܂��͓W�J�̏����׸ނ�ύX����B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '   ftyp(I)�F�����׸ނ�ύX���鍀������
        '            1�F���g�ꊇ����
        '            2�F�W�J����
        '   flg(I) �F�����׸�
        '            1�F�s��
        '            2�F��
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, recno As Integer
        Dim ProFile As String, fname As String
        'Dim ans As String * MAX_CHAR, msg As String
        'Dim strWindowsDirectoryBuffer As String * 260
        Dim ans As StringBuilder = New StringBuilder(MAX_CHAR)
        Dim msg As String
        Dim ret As Long
        Dim RecData As CHARGE
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans, MAX_CHAR, ProFile)
        'recno = Left(ans, InStr(ans, Chr(0)) - 1)
        recno = ans.ToString.Trim

        If recno <= 0 Then
            Exit Sub
        End If

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A���g�܂��͓W�J�����׸ނ�ύX���A�������ށB

        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������


        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Sub
        End If

        'fnum = FreeFile()
        'Open fname For Random Access Read Write As #fnum Len = Len(RecData)
        'Get #fnum, recno, RecData

        'If ftyp = 1 Then        '���g�ꊇ����
        '    RecData.skl_shori_flg = flg
        'ElseIf ftyp = 2 Then    '�W�J����
        '    RecData.tenkai_flg = flg
        'End If

        'Put #fnum, recno, RecData
        'Close #fnum

    End Sub

    Public Function fun�`���[�W�R�[�h�̗L��(Kogo As String, ccode As String) As Integer
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ���H�������̧�ق�������޺��ނ�ǂݍ��݁A
        '   ����޺��ޓo�^̧�قɓo�^����Ă��邩���肷��B
        '   �H�������̧�قɓo�^̧��ں��ވʒu���������ށB
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '   ccode(IO)�F�H�������̧�ق���ǂݍ�������޺���
        '�߂�l
        '   -999�F����޺��ޓo�^̧�قɓo�^����Ă��Ȃ��B
        '   0�F�H�������̧�ق�����޺��ނ�"FREE-JUPITER"�ŁA����޺��ނ��������Ȃ��i���ٌ_���ЁA�Г��AYBC�̂Ƃ��j
        '   1�ȏ�F����޺��ޓo�^̧�قɓo�^����Ă��āA�����̐��l�͓o�^�ʒu��ں��ޔԍ�
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, reccnt As Integer
        Dim ProFile As String, fname As String, rcode As String
        'Dim ans As String * MAX_CHAR, msg As String
        'Dim strWindowsDirectoryBuffer As String * 260
        Dim ans As StringBuilder = New StringBuilder(MAX_CHAR)
        Dim msg As String
        Dim userName As String, SysFile As String
        Dim ret As Long
        Dim RecData As New CHARGE()
        Dim strFileName As String

        strFileName = FunGetLanguage()

        fun�`���[�W�R�[�h�̗L�� = -999

        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        Dim isNewKoji As Boolean = False
        '����޺��ނ��擾����B
        If ccode = "" Then
            '�H�������̧�ق�������޺��ނ�ǂݍ��ށB
            ret = GetPrivateProfileString("�`���[�W�R�[�h", "�`���[�W�R�[�h", "NO-CHARGE", ans, MAX_CHAR, ProFile)
            'ccode = Trim(UCase(Left(ans, InStr(ans, Chr(0)) - 1)))
            ccode = ans.ToString.Trim
            '�H�������̧�ق�����޺��ނ��L�q����Ă��Ȃ��ꍇ�B
            If ccode = "NO-CHARGE" Then
                '�H�������̧�قɓo�^̧��ں��ވʒu���������ށB
                ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��), ProFile)
                Exit Function
            End If
            isNewKoji = True
        End If

        reccnt = 0

        'SYSTEM.TXŢ�ق������Ƃ��A����޺��ޓo�^̧�ق������͍s��Ȃ��B
        SysFile = FunGetSystemFile()

        If SysFile = "" Then
            '       fun�`���[�W�R�[�h�̗L�� = reccnt
            '       '�H�������̧�قɓo�^̧��ں��ވʒu���������ށB
            '       ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��), ProFile)
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "CastarJupiter���ø��݂ɖ�肪����܂��B" & Chr(10)
                msg = msg & "���ø��݂̑������m���߂Ă��������B" & Chr(10)
                msg = msg & "��������������Ă���ꍇ�́A�������ɘA�����ĉ������B" & Chr(10)
                msg = msg & "��۸��т��I�����܂��B" & Chr(10)
            Else
                msg = "There is a problem in CastarJupiter protection." & Chr(10)
                msg = msg & "Please confirm wearing of protection." & Chr(10)
                msg = msg & "Please inform a distributor, when equipped correctly." & Chr(10)
                msg = msg & "A program is ended." & Chr(10)
            End If
            MsgBox(msg, vbOKOnly + vbCritical)
            End
            Exit Function
        End If
        '����޺��ނ�FREE-JUPITER��NKK�AIHI�̂Ƃ��A����޺��ޓo�^̧�ق������͍s��Ȃ��B
        Call subGetUserName(userName)

        '<<<<2014/06/04 YBG�ł��`���[�W��ǂދ@�\��ǉ�
        '   ���[�U�[��YBG�@�@��FuncGetYBCFunction = "0"�@�̏ꍇ�́A�`���[�W�Ȃ�"
        '    If (ccode = "FREE-JUPITER") And _
        '       ((UCase(userName) = "NKK") Or (UCase(userName) = "IHI") Or _
        '        (UCase(userName) = "YBG")) Then
        If (ccode = "FREE-JUPITER") And _
           ((UCase(userName) = "NKK" And FuncGetOptionFunction() = "0") Or (UCase(userName) = "IHI") Or _
            (UCase(userName) = "YBG" And FuncGetOptionFunction() = "0")) Then
            '>>>>2014/06/04 YBG�ł��`���[�W��ǂދ@�\��ǉ�
            fun�`���[�W�R�[�h�̗L�� = reccnt
            '�H�������̧�قɓo�^̧��ں��ވʒu���������ށB
            If isNewKoji = False Then
                ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��), ProFile)
            End If
            Exit Function
        End If

        '����޺��ޓo�^̧�فiCHARGE.INI�j��ǂݍ��݂Ȃ���A���̍H��������޺��ނ��o�^����Ă��邩����������B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Function
        End If

        'fnum = FreeFile()
        'Open fname For Random Access Read As #fnum Len = Len(RecData)
        'Do While Not EOF(fnum)
        'Get #fnum, , RecData
        '    reccnt = reccnt + 1

        '    If InStr(RecData.charge_code, Chr(0)) > 0 Then '2014/04/10 simo �����Ȃ��悤�ɏC�������B
        '        rcode = Trim(UCase(Left(RecData.charge_code, InStr(RecData.charge_code, Chr(0)) - 1)))
        '    Else
        '        '2014/04/10 simo �����Ȃ��悤�ɏC�������B
        '        rcode = Trim(UCase(RecData.charge_code))
        '    End If


        '    If ccode = rcode Then
        '        fun�`���[�W�R�[�h�̗L�� = reccnt
        '        Exit Do
        '    End If
        'Loop
        'Close #fnum
        Dim iLen As Integer = Len(RecData)
        fnum = FreeFile()
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        Do While Not EOF(fnum)
            'Dim tmp As ValueType
            'tmp = RecData
            'FileGet(fnum, tmp)
            'RecData = CType(tmp, CHARGE)
            reccnt = reccnt + 1
            Try
                FileGet(fnum, RecData, reccnt)
            Catch ex As Exception
                Exit Do
            End Try

            If InStr(RecData.charge_code, Chr(0)) > 0 Then '2014/04/10 simo �����Ȃ��悤�ɏC�������B
                rcode = Trim(UCase(Left(RecData.charge_code, InStr(RecData.charge_code, Chr(0)) - 1)))
            Else
                '2014/04/10 simo �����Ȃ��悤�ɏC�������B
                rcode = Trim(UCase(RecData.charge_code))
            End If

            If ccode = rcode Then
                fun�`���[�W�R�[�h�̗L�� = reccnt
                Exit Do
            End If
        Loop
        FileClose(fnum)

        '�H�������̧�قɓo�^̧��ں��ވʒu���������ށB
        If isNewKoji = False Then
            ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��), ProFile)
        End If

    End Function

    Public Function fun�`���[�W�R�[�h�̗L���V�K�H��(ccode As String, ByRef kogo As String, ByRef kozo As String) As Integer
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   ����޺��ޓo�^̧�ق���H���ƍ\���`�����擾����B
        '����
        '   ccode(I)�F�H�������̧�ق���ǂݍ�������޺���
        '   kogo(O)�F�����ΏۍH���i�ިڸ�ؖ��j
        '   kozo(O)�F�����ΏۍH���i�ިڸ�ؖ��j
        '�߂�l
        '   -999�F����޺��ޓo�^̧�قɓo�^����Ă��Ȃ��B
        '   0�F�H�������̧�ق�����޺��ނ�"FREE-JUPITER"�ŁA����޺��ނ��������Ȃ��i���ٌ_���ЁA�Г��AYBC�̂Ƃ��j
        '   1�ȏ�F����޺��ޓo�^̧�قɓo�^����Ă��āA�����̐��l�͓o�^�ʒu��ں��ޔԍ�
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, reccnt As Integer
        Dim fname As String, rcode As String
        Dim ans As StringBuilder = New StringBuilder(MAX_CHAR)
        Dim msg As String
        Dim userName As String = ""
        Dim SysFile As String
        Dim RecData As New CHARGE()
        Dim strFileName As String

        strFileName = FunGetLanguage()

        fun�`���[�W�R�[�h�̗L���V�K�H�� = -999

        reccnt = 0

        'SYSTEM.TXŢ�ق������Ƃ��A����޺��ޓo�^̧�ق������͍s��Ȃ��B
        SysFile = FunGetSystemFile()

        If SysFile = "" Then
            '       fun�`���[�W�R�[�h�̗L�� = reccnt
            '       '�H�������̧�قɓo�^̧��ں��ވʒu���������ށB
            '       ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��), ProFile)
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "CastarJupiter���ø��݂ɖ�肪����܂��B" & Chr(10)
                msg = msg & "���ø��݂̑������m���߂Ă��������B" & Chr(10)
                msg = msg & "��������������Ă���ꍇ�́A�������ɘA�����ĉ������B" & Chr(10)
                msg = msg & "��۸��т��I�����܂��B" & Chr(10)
            Else
                msg = "There is a problem in CastarJupiter protection." & Chr(10)
                msg = msg & "Please confirm wearing of protection." & Chr(10)
                msg = msg & "Please inform a distributor, when equipped correctly." & Chr(10)
                msg = msg & "A program is ended." & Chr(10)
            End If
            MsgBox(msg, vbOKOnly + vbCritical)
            End
            Exit Function
        End If
        '����޺��ނ�FREE-JUPITER��NKK�AIHI�̂Ƃ��A����޺��ޓo�^̧�ق������͍s��Ȃ��B
        Call subGetUserName(userName)

        '<<<<2014/06/04 YBG�ł��`���[�W��ǂދ@�\��ǉ�
        '   ���[�U�[��YBG�@�@��FuncGetYBCFunction = "0"�@�̏ꍇ�́A�`���[�W�Ȃ�"
        '    If (ccode = "FREE-JUPITER") And _
        '       ((UCase(userName) = "NKK") Or (UCase(userName) = "IHI") Or _
        '        (UCase(userName) = "YBG")) Then
        If (ccode = "FREE-JUPITER") And _
           ((UCase(userName) = "NKK" And FuncGetOptionFunction() = "0") Or (UCase(userName) = "IHI") Or _
            (UCase(userName) = "YBG" And FuncGetOptionFunction() = "0")) Then
            '>>>>2014/06/04 YBG�ł��`���[�W��ǂދ@�\��ǉ�
            fun�`���[�W�R�[�h�̗L���V�K�H�� = reccnt
            Exit Function
        End If

        '����޺��ޓo�^̧�فiCHARGE.INI�j��ǂݍ��݂Ȃ���A���̍H��������޺��ނ��o�^����Ă��邩����������B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Function
        End If

        'fnum = FreeFile()
        'Open fname For Random Access Read As #fnum Len = Len(RecData)
        'Do While Not EOF(fnum)
        'Get #fnum, , RecData
        '    reccnt = reccnt + 1

        '    If InStr(RecData.charge_code, Chr(0)) > 0 Then '2014/04/10 simo �����Ȃ��悤�ɏC�������B
        '        rcode = Trim(UCase(Left(RecData.charge_code, InStr(RecData.charge_code, Chr(0)) - 1)))
        '    Else
        '        '2014/04/10 simo �����Ȃ��悤�ɏC�������B
        '        rcode = Trim(UCase(RecData.charge_code))
        '    End If


        '    If ccode = rcode Then
        '        fun�`���[�W�R�[�h�̗L�� = reccnt
        '        Exit Do
        '    End If
        'Loop
        'Close #fnum
        fnum = FreeFile()
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        Do While Not EOF(fnum)
            reccnt = reccnt + 1
            Try
                FileGet(fnum, RecData, reccnt)
            Catch ex As Exception
                Exit Do
            End Try

            If InStr(RecData.charge_code, Chr(0)) > 0 Then '2014/04/10 simo �����Ȃ��悤�ɏC�������B
                rcode = Trim(UCase(Left(RecData.charge_code, InStr(RecData.charge_code, Chr(0)) - 1)))
            Else
                '2014/04/10 simo �����Ȃ��悤�ɏC�������B
                rcode = Trim(UCase(RecData.charge_code))
            End If

            If ccode = rcode Then
                kogo = RecData.kouji_code.Trim()
                If RecData.keisiki = 1 Then
                    kozo = "RC����"
                ElseIf RecData.keisiki = 2 Then
                    kozo = "�|���Ŕ���"
                ElseIf RecData.keisiki = 4 Then
                    kozo = "RC�包"
                ElseIf RecData.keisiki = 8 Then
                    kozo = "��������"
                ElseIf RecData.keisiki = 16 Then
                    kozo = "�|�����r"
                ElseIf RecData.keisiki = 64 Then
                    kozo = "�|���Z�O�����g"
                End If
                fun�`���[�W�R�[�h�̗L���V�K�H�� = reccnt
                Exit Do
            End If
        Loop
        FileClose(fnum)

    End Function

    Public Function get_CHARGE_FileName() As String
        '2014/06/09 �`���[�W�t�@�C����Ԃ��֐�
        '���D���c:\Jupiter\CHARGE\CHARGE.INI"
        '���D���C:\Windows\charge\CHARGE.INI"�Ƃ���
        '
        Dim WinDir As String
        Dim CHARGE_FileName As String
        CHARGE_FileName = FunGetGensun() & "CHARGE\CHARGE.INI"


        If Dir(CHARGE_FileName, vbNormal) = "" Then
            '���D���C:\Windows\charge\CHARGE.INI"�Ƃ��� ���܂܂łǂ���
            'Windows�ިڸ�؁i�� D:\WINNT�j���߽�����擾����B
            'lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer, Len(strWindowsDirectoryBuffer))
            'WinDir = Left(strWindowsDirectoryBuffer, InStr(strWindowsDirectoryBuffer, vbNullChar) - 1)
            WinDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows)
            get_CHARGE_FileName = WinDir & "\CHARGE\CHARGE.INI"
        Else
            '���D�� c:\Jupiter\CHARGE\CHARGE.INI"���������ꍇ
            get_CHARGE_FileName = CHARGE_FileName

        End If
    End Function

    Public Function fun�`���[�W�R�[�h�̗L��_�W�J�m�F(Kogo As String, ccode As String) As Integer
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ���H�������̧�ق�������޺��ނ�ǂݍ��݁A
        '   ����޺��ޓo�^̧�قɓo�^����Ă��邩���肷��B
        '   ���݂̓��t�ƃ`���[�W�t�@�C���̏I�������r���A���� > �I�����̏ꍇ��-999��Ԃ��B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '   ccode(IO)�F�H�������̧�ق���ǂݍ�������޺���
        '�߂�l
        '   -999�F���� > �I����
        '   0�F�H�������̧�ق�����޺��ނ�"FREE-JUPITER"�ŁA����޺��ނ��������Ȃ��i���ٌ_���ЁA�Г��AYBC�̂Ƃ��j
        '   1�ȏ�F����޺��ޓo�^̧�قɓo�^����Ă��āA�����̐��l�͓o�^�ʒu��ں��ޔԍ�
        '---------------------------------------------------------------------------------------------------
        Dim fnum As Integer, reccnt As Integer
        Dim ProFile As String, fname As String, rcode As String
        Dim ans As StringBuilder = New StringBuilder(MAX_CHAR)
        Dim msg As String
        Dim userName As String, SysFile As String
        Dim ret As Long
        Dim RecData As New CHARGE()
        Dim strFileName As String

        strFileName = FunGetLanguage()

        fun�`���[�W�R�[�h�̗L��_�W�J�m�F = -999

        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        Dim isNewKoji As Boolean = False
        '����޺��ނ��擾����B
        If ccode = "" Then
            '�H�������̧�ق�������޺��ނ�ǂݍ��ށB
            ret = GetPrivateProfileString("�`���[�W�R�[�h", "�`���[�W�R�[�h", "NO-CHARGE", ans, MAX_CHAR, ProFile)
            'ccode = Trim(UCase(Left(ans, InStr(ans, Chr(0)) - 1)))
            ccode = ans.ToString.Trim
            '�H�������̧�ق�����޺��ނ��L�q����Ă��Ȃ��ꍇ�B
            If ccode = "NO-CHARGE" Then
                '�H�������̧�قɓo�^̧��ں��ވʒu���������ށB
                ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��_�W�J�m�F), ProFile)
                Exit Function
            End If
            isNewKoji = True
        End If

        reccnt = 0

        'SYSTEM.TXŢ�ق������Ƃ��A����޺��ޓo�^̧�ق������͍s��Ȃ��B
        SysFile = FunGetSystemFile()

        If SysFile = "" Then
            '       fun�`���[�W�R�[�h�̗L�� = reccnt
            '       '�H�������̧�قɓo�^̧��ں��ވʒu���������ށB
            '       ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��), ProFile)
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "CastarJupiter���ø��݂ɖ�肪����܂��B" & Chr(10)
                msg = msg & "���ø��݂̑������m���߂Ă��������B" & Chr(10)
                msg = msg & "��������������Ă���ꍇ�́A�������ɘA�����ĉ������B" & Chr(10)
                msg = msg & "��۸��т��I�����܂��B" & Chr(10)
            Else
                msg = "There is a problem in CastarJupiter protection." & Chr(10)
                msg = msg & "Please confirm wearing of protection." & Chr(10)
                msg = msg & "Please inform a distributor, when equipped correctly." & Chr(10)
                msg = msg & "A program is ended." & Chr(10)
            End If
            MsgBox(msg, vbOKOnly + vbCritical)
            End
            Exit Function
        End If
        '����޺��ނ�FREE-JUPITER��NKK�AIHI�̂Ƃ��A����޺��ޓo�^̧�ق������͍s��Ȃ��B
        Call subGetUserName(userName)

        '<<<<2014/06/04 YBG�ł��`���[�W��ǂދ@�\��ǉ�
        '   ���[�U�[��YBG�@�@��FuncGetYBCFunction = "0"�@�̏ꍇ�́A�`���[�W�Ȃ�"
        '    If (ccode = "FREE-JUPITER") And _
        '       ((UCase(userName) = "NKK") Or (UCase(userName) = "IHI") Or _
        '        (UCase(userName) = "YBG")) Then
        If (ccode = "FREE-JUPITER") And _
           ((UCase(userName) = "NKK" And FuncGetOptionFunction() = "0") Or (UCase(userName) = "IHI") Or _
            (UCase(userName) = "YBG" And FuncGetOptionFunction() = "0")) Then
            '>>>>2014/06/04 YBG�ł��`���[�W��ǂދ@�\��ǉ�
            fun�`���[�W�R�[�h�̗L��_�W�J�m�F = reccnt
            '�H�������̧�قɓo�^̧��ں��ވʒu���������ށB
            If isNewKoji = False Then
                ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��_�W�J�m�F), ProFile)
            End If
            Exit Function
        End If

        '����޺��ޓo�^̧�فiCHARGE.INI�j��ǂݍ��݂Ȃ���A���̍H��������޺��ނ��o�^����Ă��邩����������B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        If Dir(fname, vbNormal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, vbOKOnly + vbExclamation)
            Exit Function
        End If

        Dim iLen As Integer = Len(RecData)
        fnum = FreeFile()
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        Do While Not EOF(fnum)
            reccnt = reccnt + 1
            Try
                FileGet(fnum, RecData, reccnt)
            Catch ex As Exception
                Exit Do
            End Try

            If InStr(RecData.charge_code, Chr(0)) > 0 Then '2014/04/10 simo �����Ȃ��悤�ɏC�������B
                rcode = Trim(UCase(Left(RecData.charge_code, InStr(RecData.charge_code, Chr(0)) - 1)))
            Else
                '2014/04/10 simo �����Ȃ��悤�ɏC�������B
                rcode = Trim(UCase(RecData.charge_code))
            End If

            If ccode = rcode Then
                fun�`���[�W�R�[�h�̗L��_�W�J�m�F = reccnt
                '���݂̓��t�ƃ`���[�W�t�@�C���̏I�������r���A���� > �I�����̏ꍇ��-999��Ԃ��B
                Dim dtNow As DateTime = DateTime.Now
                Dim stNow As String = dtNow.ToString("yyyy/MM/dd")
                Dim result As Integer = Date.Compare(stNow, RecData.shori_shuuryou)
                If result = 1 Then
                    fun�`���[�W�R�[�h�̗L��_�W�J�m�F = -999
                End If
                Exit Do
            End If
        Loop
        FileClose(fnum)

    End Function

End Module


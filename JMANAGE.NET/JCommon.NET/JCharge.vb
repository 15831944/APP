Option Strict Off
Option Explicit On
Module JCharge

    '***************************************************************************************************
    '�g�p���Ă���WindowsAPI�̐錾
    '***************************************************************************************************
    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer

    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer

    Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Integer) As Integer

    '***************************************************************************************************
    'հ�ް��`�萔�̐錾
    '***************************************************************************************************
    '***************************************************************************************************
    '�ϐ����� ���e���ނ�'\0'�}���p��1�傫���ݒ肷����̂Ƃ���B���̂Ƃ��A�ϐ����ނ���ƂȂ�B
    '�����VB�œǂݏ���������ް������������Ȃ邽��(�����s��)�A�����1�傫���ݒ肷��B
    '***************************************************************************************************
    '2017/07/28 Nakagawa Edit Start
    'Public Const CHARGE_SIZE As Short = 10 '����޺��޻���
    'Public Const KAISHA_SIZE As Short = 52 '��Ж�����
    'Public Const KJCODE_SIZE As Short = 18 '�H�����޻���
    'Public Const KJMEI_SIZE As Short = 52 '�H��������
    'Public Const HACCHU_SIZE As Short = 52 '�����һ���
    'Public Const TANTOU_SIZE As Short = 22 '�����S���һ���
    'Public Const DATE_MAX As Short = 10 '���t,�d�ʌJ��Ԃ���
    'Public Const DATE_SIZE As Short = 12 '���t����

    'Private Const MAX_CHAR As Short = 256 '�ő啶����
    Public Const CHARGE_SIZE As Integer = 10 '����޺��޻���
    Public Const KAISHA_SIZE As Integer = 52 '��Ж�����
    Public Const KJCODE_SIZE As Integer = 18 '�H�����޻���
    Public Const KJMEI_SIZE As Integer = 52 '�H��������
    Public Const HACCHU_SIZE As Integer = 52 '�����һ���
    Public Const TANTOU_SIZE As Integer = 22 '�����S���һ���
    Public Const DATE_MAX As Integer = 10 '���t,�d�ʌJ��Ԃ���
    Public Const DATE_SIZE As Integer = 12 '���t����

    Private Const MAX_CHAR As Integer = 256 '�ő啶����
    Private Const FILE_SIZE As Integer = 954     'CHARGE�\���̃T�C�Y
    '2017/07/28 Nakagawa Edit End

    '***************************************************************************************************
    'հ�ް��`�\���̂̐錾
    '***************************************************************************************************
    '---------------------------------------------------------------------------------------------------
    '���t�p�\����
    '---------------------------------------------------------------------------------------------------
    Structure sdate
        'UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '2017/07/28 Nakagawa Edit Start
        '<VBFixedString(DATE_SIZE),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray,SizeConst:=DATE_SIZE)> Public strdate() As Char
        <VBFixedStringAttribute(DATE_SIZE)> Dim strdate As String
        '2017/07/28 Nakagawa Edit End
    End Structure

    '---------------------------------------------------------------------------------------------------
    'CHARGE.INI�ް��p�\����
    '---------------------------------------------------------------------------------------------------
    Structure CHARGE
        '2017/07/28 Nakagawa Edit Start
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(CHARGE_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=CHARGE_SIZE)> Public charge_code() As Char '����޺���
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(KAISHA_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=KAISHA_SIZE)> Public kaisha_mei() As Char '��Ж�
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(KJCODE_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=KJCODE_SIZE)> Public kouji_code() As Char '�H������
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(KJMEI_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=KJMEI_SIZE)> Public kouji_mei() As Char '�H����
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(HACCHU_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=HACCHU_SIZE)> Public hacchu_sha() As Char '������
        <VBFixedStringAttribute(CHARGE_SIZE)> Dim charge_code As String   '����޺���
        <VBFixedStringAttribute(KAISHA_SIZE)> Dim kaisha_mei As String    '��Ж�
        <VBFixedStringAttribute(KJCODE_SIZE)> Dim kouji_code As String    '�H������
        <VBFixedStringAttribute(KJMEI_SIZE)> Dim kouji_mei As String      '�H����
        <VBFixedStringAttribute(HACCHU_SIZE)> Dim hacchu_sha As String    '������
        '2017/07/28 Nakagawa Edit End
        Dim keisiki As Integer '�����`��
        Dim gairyaku_juuryou As Double '�T���d��(�P��:ton)
        Dim sikan_suu As Integer '�x�Ԑ�
        Dim shuketa_daisuu As Integer '�包�䐔
        Dim deck_daisuu As Integer '�ޯ��䐔
        Dim yokoketa_daisuu As Integer '�����䐔
        Dim brket_daisuu As Integer '��׹�đ䐔
        Dim nakatate_daisuu As Integer '���c���䐔
        Dim sokutate_daisuu As Integer '���c���䐔
        Dim taikeikou_daisuu As Integer '�΍��\�䐔
        Dim yokokou As Integer '���\�L��(1:��,2:�L)
        '2017/07/28 Nakagawa Edit Start
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(DATE_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=DATE_SIZE)> Public shori_kaisi() As Char '�����J�n�\���
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(DATE_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=DATE_SIZE)> Public shori_shuuryou() As Char '�����I���\���
        ''UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(TANTOU_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=TANTOU_SIZE)> Public shori_tantou() As Char '�����S����
        <VBFixedStringAttribute(DATE_SIZE)> Dim shori_kaisi As String     '�����J�n�\���
        <VBFixedStringAttribute(DATE_SIZE)> Dim shori_shuuryou As String  '�����I���\���
        <VBFixedStringAttribute(TANTOU_SIZE)> Dim shori_tantou As String  '�����S����
        '2017/07/28 Nakagawa Edit End
        Dim skl_shori_flg As Integer '���g�ꊇ�������s�׸�(1:�s��,2:��)
        Dim tenkai_flg As Integer '�W�J�������s�׸�(1:�s��,2:��)
        '2017/07/28 Nakagawa Edit Start
        'UPGRADE_WARNING: Fixed-length string size must fit in the buffer. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="3C1E4426-0B80-443E-B943-0627CD55D48B"'
        '<VBFixedString(DATE_SIZE), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=DATE_SIZE)> Public sinki_touroku() As Char '�V�K�o�^������
        <VBFixedStringAttribute(DATE_SIZE)> Dim sinki_touroku As String   '�V�K�o�^������
        '2017/07/28 Nakagawa Edit End
        Dim skl_shori_kaisuu As Integer '���g�ꊇ�������s��
        '2017/07/28 Nakagawa Edit Star
        '<VBFixedArray(DATE_MAX)> Dim skl_shori_date() As sdate '���g�ꊇ�������s��
        Dim skl_shori_date() As sdate                                     '���g�ꊇ�������s��
        '2017/07/28 Nakagawa Edit End
        Dim buzai_shori_kaisuu As Integer '���ވꊇ�������s��
        '2017/07/28 Nakagawa Edit Star
        '<VBFixedArray(DATE_MAX)> Dim buzai_shori_date() As sdate '���ވꊇ�������s��
        Dim buzai_shori_date() As sdate                                   '���ވꊇ�������s��
        '2017/07/28 Nakagawa Edit End
        Dim tenkai_shori_kaisuu As Integer '�W�J�������s��
        '2017/07/28 Nakagawa Edit Star
        '<VBFixedArray(DATE_MAX)> Dim tenkai_shori_date() As sdate '�W�J�������s��
        '<VBFixedArray(DATE_MAX)> Dim tenkai_jyuuryou() As Double '�W�J�������PMF�̏d��
        Dim tenkai_shori_date() As sdate                                  '�W�J�������s��
        Dim tenkai_jyuuryou() As Double                                   '�W�J�������PMF�̏d��
        '2017/07/28 Nakagawa Edit End
        Dim bunrui_shori_kaisuu As Integer '���ޏ������s��
        '2017/07/28 Nakagawa Edit Star
        '<VBFixedArray(DATE_MAX)> Dim bunrui_shori_date() As sdate '���ޏ������s��
        '<VBFixedArray(DATE_MAX)> Dim bunrui_jyuuryou() As Double '���ޏ������PMF�̏d��
        Dim bunrui_shori_date() As sdate                                  '���ޏ������s��
        Dim bunrui_jyuuryou() As Double                                   '���ޏ������PMF�̏d��
        '2017/07/28 Nakagawa Edit End

        'UPGRADE_TODO: "Initialize" must be called to initialize instances of this structure. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="B4BFF9E0-8631-45CF-910E-62AB3970F27B"'
        Public Sub Initialize()
            'UPGRADE_WARNING: Lower bound of array skl_shori_date was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim skl_shori_date(DATE_MAX)
            'UPGRADE_WARNING: Lower bound of array buzai_shori_date was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim buzai_shori_date(DATE_MAX)
            'UPGRADE_WARNING: Lower bound of array tenkai_shori_date was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim tenkai_shori_date(DATE_MAX)
            'UPGRADE_WARNING: Lower bound of array tenkai_jyuuryou was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim tenkai_jyuuryou(DATE_MAX)
            'UPGRADE_WARNING: Lower bound of array bunrui_shori_date was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim bunrui_shori_date(DATE_MAX)
            'UPGRADE_WARNING: Lower bound of array bunrui_jyuuryou was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim bunrui_jyuuryou(DATE_MAX)
        End Sub
    End Structure

    '---------------------------------------------------------------------------------------------------
    'BLOCK.INF�ް��p�\����
    '---------------------------------------------------------------------------------------------------
    Structure KOZO
        Dim shuketa_daisuu As Integer '�包�䐔
        Dim deck_daisuu As Integer '�ޯ��䐔
        Dim yokoketa_daisuu As Integer '�����䐔
        Dim brket_daisuu As Integer '��׹�đ䐔
        Dim tateketa_daisuu As Integer '�c��(���A��)�䐔
        Dim taikeikou_daisuu As Integer '�΍��\�䐔
        Dim yokokou As Integer '���\�L��(1:��,2:�L)
    End Structure

    Sub subCHARGE�V�K���f�[�^�̊i�[(ByRef Kogo As String)
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ���H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��݁A
        '   ����޺��ޓo�^̧�ق̂��̍H���̐V�K�o�^���ɓ��t���i�[����B
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '�߂�l
        '   ����
        '---------------------------------------------------------------------------------------------------
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        'UPGRADE_NOTE: today was upgraded to today_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim today_Renamed As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno <= 0 Then
            Exit Sub
        End If

        'Windows�ިڸ�؁i�� D:\WINNT�j���߽�����擾����B
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A�V�K�o�^�����������ށB
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������


        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI). " & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)

        '���݂̓��t���擾����B
        today_Renamed = VB6.Format(Now, "yyyy/mm/dd")

        '�V�K�o�^�����i�[����B
        RecData.sinki_touroku = today_Renamed

        'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FilePut(fnum, RecData, recno)
        FileClose(fnum)

    End Sub

    Sub subBLOCKINF����e��\���̃f�[�^�Z�o(ByRef Kogo As String, ByRef KData As KOZO)
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
        Dim fnum As Short
        Dim fname As String
        Dim kz, kind As String
        Dim buf, msg As String
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
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
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
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        fnum = FreeFile
        FileOpen(fnum, fname, OpenMode.Input)
        Do While Not EOF(fnum)
            buf = LineInput(fnum)

            kz = UCase(J_ChoiceString(buf, 2)) '�\��
            kind = UCase(J_ChoiceString(buf, 7)) '���

            If ((kz = "GRD") Or (kz = "KYAKU") Or (kz = "HARI")) And (kind = "BLK") Then '�包�䐔
                KData.shuketa_daisuu = KData.shuketa_daisuu + 1
            ElseIf ((kz = "DECK") Or (kz = "GDK")) And (kind = "BLK") Then  '�ޯ��䐔�܂��͒�|�䐔
                KData.deck_daisuu = KData.deck_daisuu + 1
            ElseIf (kz = "CRS") And (kind = "BLK") Then  '�����䐔
                KData.yokoketa_daisuu = KData.yokoketa_daisuu + 1
            ElseIf (kz = "BRK") And (kind = "BLK") Then  '��׹�đ䐔
                KData.brket_daisuu = KData.brket_daisuu + 1
            ElseIf ((kz = "STR") Or (kz = "GST")) And (kind = "BLK") Then  '�c��(���A��)�䐔�܂��͑��|�䐔
                KData.tateketa_daisuu = KData.tateketa_daisuu + 1
            ElseIf (kz = "SWY") And (kind = "BLK") Then  '�΍��\�䐔
                KData.taikeikou_daisuu = KData.taikeikou_daisuu + 1
            ElseIf kz = "LATE" Then  '���\�L��
                KData.yokokou = 2
            End If
        Loop
        FileClose(fnum)

    End Sub

    '2007/10/09 Nakagawa Add Start
    Sub subBLOCKINF����e��\���̃f�[�^�Z�o_�t�@�C�����w��(ByRef Kogo As String, ByRef KData As KOZO, ByRef blockInfName As String)
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
        Dim fnum As Short
        Dim fname As String
        Dim kz, kind As String
        Dim buf, msg As String
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
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
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
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        fnum = FreeFile
        FileOpen(fnum, fname, OpenMode.Input)
        Do While Not EOF(fnum)
            buf = LineInput(fnum)

            kz = UCase(J_ChoiceString(buf, 2)) '�\��
            kind = UCase(J_ChoiceString(buf, 7)) '���

            If (kz = "GRD") And (kind = "BLK") Then '�包�䐔
                KData.shuketa_daisuu = KData.shuketa_daisuu + 1
            ElseIf ((kz = "DECK") Or (kz = "GDK")) And (kind = "BLK") Then  '�ޯ��䐔�܂��͒�|�䐔
                KData.deck_daisuu = KData.deck_daisuu + 1
            ElseIf (kz = "CRS") And (kind = "BLK") Then  '�����䐔
                KData.yokoketa_daisuu = KData.yokoketa_daisuu + 1
            ElseIf (kz = "BRK") And (kind = "BLK") Then  '��׹�đ䐔
                KData.brket_daisuu = KData.brket_daisuu + 1
            ElseIf ((kz = "STR") Or (kz = "GST")) And (kind = "BLK") Then  '�c��(���A��)�䐔�܂��͑��|�䐔
                KData.tateketa_daisuu = KData.tateketa_daisuu + 1
            ElseIf (kz = "SWY") And (kind = "BLK") Then  '�΍��\�䐔
                KData.taikeikou_daisuu = KData.taikeikou_daisuu + 1
            ElseIf kz = "LATE" Then  '���\�L��
                KData.yokokou = 2
            End If
        Loop
        FileClose(fnum)

    End Sub
    '2007/10/09 Nakagawa Add End

    Function funBLOCKINF��DWG�t�@�C���̃`�F�b�N(ByRef Kogo As String) As Boolean
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
        Dim fnum, cnt As Short
        Dim fname, kogodir As String
        Dim blkname, dwgname As String
        Dim buf, msg As String
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funBLOCKINF��DWG�t�@�C���̃`�F�b�N = False
        kogodir = FunGetNewKoji() & Kogo

        '��ۯ����̧�ق�����ۯ��O������ǂݍ��݁A������ۯ��O������DWĢ�ق���������B
        fname = kogodir & "\BLOCK.INF"
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
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
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile
        FileOpen(fnum, fname, OpenMode.Input)
        Do While Not EOF(fnum)
            buf = LineInput(fnum)

            blkname = J_ChoiceString(buf, 1) '��ۯ��O����
            dwgname = kogodir & "\" & blkname & ".DBA"
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Dir(dwgname, FileAttribute.Normal) = "" Then
                Beep()
                If Not strFileName Like "*ENG.INI" Then
                    msg = "��ۯ����̧�قɋL�q����Ă���" & blkname & "��ۯ��ɑΉ�����DWĢ�ق�����܂���B"
                Else
                    msg = "There is no DWG file corresponding to " & blkname & "Brock described by the Brock information file."
                End If
                MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
                Exit Function
            End If
        Loop
        FileClose(fnum)

        funBLOCKINF��DWG�t�@�C���̃`�F�b�N = True

    End Function

    Function funCHARGE�䐔�̃`�F�b�N(ByRef Kogo As String, ByRef intChgNum As Short, ByRef intBlkNum As Short) As Short
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
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret, tateketa As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim KozoData As KOZO
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE�䐔�̃`�F�b�N = 1

        '��ۯ����̧�ق���e��\���̑䐔���Z�o����B
        Call subBLOCKINF����e��\���̃f�[�^�Z�o(Kogo, KozoData)

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno < 0 Then '�o�^̧�قɓo�^����Ă��Ȃ����ߴװ�I������B
            funCHARGE�䐔�̃`�F�b�N = 1
            Exit Function
        ElseIf recno = 0 Then  '����޺��ނ�FREE-JUPITER�̂��ߐ���I������B
            funCHARGE�䐔�̃`�F�b�N = 0
            Exit Function
        End If

        'Windows�ިڸ�؁i�� D:\WINNT�j���߽�����擾����B
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A�e��\���̑䐔����ۯ����̧�ق���Z�o�����䐔����������B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������


        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)
        tateketa = RecData.nakatate_daisuu + RecData.sokutate_daisuu

        funCHARGE�䐔�̃`�F�b�N = 2

        If RecData.shuketa_daisuu = KozoData.shuketa_daisuu Then '�包�䐔
            If RecData.deck_daisuu = KozoData.deck_daisuu Then '�ޯ��䐔
                If RecData.yokoketa_daisuu = KozoData.yokoketa_daisuu Then '�����䐔
                    If RecData.brket_daisuu = KozoData.brket_daisuu Then '��׹�đ䐔
                        If tateketa = KozoData.tateketa_daisuu Then '�c��(���A��)�䐔
                            If RecData.taikeikou_daisuu = KozoData.taikeikou_daisuu Then '�΍��\�䐔
                                If RecData.yokokou = KozoData.yokokou Then '���\�L��
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
        FileClose(fnum)

    End Function

    '2007/10/09 Nakagawa Add Start
    Function funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w��(ByRef Kogo As String, ByRef blockInfName As String) As Short
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
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret, tateketa As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim KozoData As KOZO
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 1

        '��ۯ����̧�ق���e��\���̑䐔���Z�o����B
        Call subBLOCKINF����e��\���̃f�[�^�Z�o_�t�@�C�����w��(Kogo, KozoData, blockInfName)

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno < 0 Then '�o�^̧�قɓo�^����Ă��Ȃ����ߴװ�I������B
            funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 1
            Exit Function
        ElseIf recno = 0 Then  '����޺��ނ�FREE-JUPITER�̂��ߐ���I������B
            funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 0
            Exit Function
        End If

        'Windows�ިڸ�؁i�� D:\WINNT�j���߽�����擾����B
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A�e��\���̑䐔����ۯ����̧�ق���Z�o�����䐔����������B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������



        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)
        tateketa = RecData.nakatate_daisuu + RecData.sokutate_daisuu

        funCHARGE�䐔�̃`�F�b�N_�t�@�C�����w�� = 2

        If RecData.shuketa_daisuu = KozoData.shuketa_daisuu Then '�包�䐔
            If RecData.deck_daisuu = KozoData.deck_daisuu Then '�ޯ��䐔
                If RecData.yokoketa_daisuu = KozoData.yokoketa_daisuu Then '�����䐔
                    If RecData.brket_daisuu = KozoData.brket_daisuu Then '��׹�đ䐔
                        If tateketa = KozoData.tateketa_daisuu Then '�c��(���A��)�䐔
                            If RecData.taikeikou_daisuu = KozoData.taikeikou_daisuu Then '�΍��\�䐔
                                If RecData.yokokou = KozoData.yokokou Then '���\�L��
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
        FileClose(fnum)

    End Function
    '2007/10/09 Nakagawa Add End

    Sub subCHARGE���s�f�[�^�̊i�[(ByRef Kogo As String, ByRef ftyp As Short)
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
        Dim recno, fnum, cnt As Short
        Dim fname, ProFile, WinDir As String
        'UPGRADE_NOTE: today was upgraded to today_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim today_Renamed, pmfweight As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno <= 0 Then
            Exit Sub
        End If

        'Windows�ިڸ�؁i�� D:\WINNT�j���߽�����擾����B
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A�e�����̎��s�ް����X�V���A�������ށB
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������



        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)

        '�e�����̎��s�񐔂��X�V����B
        If ftyp = 1 Then '���g�ꊇ����
            If RecData.skl_shori_kaisuu < 0 Then RecData.skl_shori_kaisuu = 0
            RecData.skl_shori_kaisuu = RecData.skl_shori_kaisuu + 1
            cnt = RecData.skl_shori_kaisuu
        ElseIf ftyp = 2 Then  '���ވꊇ����
            If RecData.buzai_shori_kaisuu < 0 Then RecData.buzai_shori_kaisuu = 0
            RecData.buzai_shori_kaisuu = RecData.buzai_shori_kaisuu + 1
            cnt = RecData.buzai_shori_kaisuu
        ElseIf ftyp = 3 Then  '�W�J����
            If RecData.tenkai_shori_kaisuu < 0 Then RecData.tenkai_shori_kaisuu = 0
            RecData.tenkai_shori_kaisuu = RecData.tenkai_shori_kaisuu + 1
            cnt = RecData.tenkai_shori_kaisuu
        ElseIf ftyp = 4 Then  '���ޏ���
            If RecData.bunrui_shori_kaisuu < 0 Then RecData.bunrui_shori_kaisuu = 0
            RecData.bunrui_shori_kaisuu = RecData.bunrui_shori_kaisuu + 1
            cnt = RecData.bunrui_shori_kaisuu
        End If

        '���t�Əd�ʂ��i�[����z��ԍ����Z�o����B
        cnt = cnt Mod DATE_MAX
        If cnt = 0 Then
            cnt = 10
        End If

        '���݂̓��t���擾����B
        today_Renamed = VB6.Format(Now, "yyyy/mm/dd")

        '�W�J�����܂��͕��ޏ����̂Ƃ��A�H�������̧�ق��瑍�d�ʂ�ǂݍ��ށB
        If (ftyp = 3) Or (ftyp = 4) Then
            ret = GetPrivateProfileString("��������", "���d��(Kg)", "NO-WEIGHT", ans.Value, MAX_CHAR, ProFile)
            pmfweight = Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)
            If pmfweight = "NO-WEIGHT" Then
                Beep()
                If Not strFileName Like "*ENG.INI" Then
                    msg = "�d�ʂ̎Z�o������Ă��Ȃ����߁A���d�ʂ�������܂���B" & Chr(10)
                    msg = msg & "���d�ʂ��ۂƂ��܂��B"
                Else
                    msg = "Since calculation of weight is not carried out, gross weight is not known." & Chr(10)
                    msg = msg & "Let gross weight be zero."
                End If
                MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
                pmfweight = "0.0"
            End If
        End If

        '�e�����̎��s���APMF�d�ʂ��i�[����B
        If ftyp = 1 Then '���g�ꊇ����
            RecData.skl_shori_date(cnt).strdate = today_Renamed
        ElseIf ftyp = 2 Then  '���ވꊇ����
            RecData.buzai_shori_date(cnt).strdate = today_Renamed
        ElseIf ftyp = 3 Then  '�W�J����
            RecData.tenkai_shori_date(cnt).strdate = today_Renamed
            RecData.tenkai_jyuuryou(cnt) = CDbl(pmfweight)
        ElseIf ftyp = 4 Then  '���ޏ���
            RecData.bunrui_shori_date(cnt).strdate = today_Renamed
            RecData.bunrui_jyuuryou(cnt) = CDbl(pmfweight)
        End If

        'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FilePut(fnum, RecData, recno)
        FileClose(fnum)

    End Sub

    Function funCHARGE�����`���̎擾(ByRef Kogo As String) As Integer
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
        '       3�FRC�包
        '       4�F�|�����包
        '---------------------------------------------------------------------------------------------------
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE�����`���̎擾 = 1

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno < 0 Then '�o�^̧�قɓo�^����Ă��Ȃ����ߕs�Ƃ���B
            funCHARGE�����`���̎擾 = -1
            Exit Function
        ElseIf recno = 0 Then  '����޺��ނ�FREE-JUPITER�̂��ߑS�`���Ƃ���B
            funCHARGE�����`���̎擾 = 0
            Exit Function
        End If

        'Windows�ިڸ�؁i�� D:\WINNT�j���߽�����擾����B
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A���g�܂��͓W�J�����׸ނ��擾����B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)

        funCHARGE�����`���̎擾 = RecData.keisiki

        FileClose(fnum)

    End Function

    Function funCHARGE�����t���O�̎擾(ByRef Kogo As String, ByRef ftyp As Short) As Integer
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
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        funCHARGE�����t���O�̎擾 = 1

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno < 0 Then '�o�^̧�قɓo�^����Ă��Ȃ����ߕs�Ƃ���B
            funCHARGE�����t���O�̎擾 = 1
            Exit Function
        ElseIf recno = 0 Then  '����޺��ނ�FREE-JUPITER�̂��߉Ƃ���B
            funCHARGE�����t���O�̎擾 = 2
            Exit Function
        End If

        'Windows�ިڸ�؁i�� D:\WINNT�j���߽�����擾����B
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A���g�܂��͓W�J�����׸ނ��擾����B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)

        If ftyp = 1 Then '���g�ꊇ����
            funCHARGE�����t���O�̎擾 = RecData.skl_shori_flg
        ElseIf ftyp = 2 Then  '�W�J����
            funCHARGE�����t���O�̎擾 = RecData.tenkai_flg
        End If

        FileClose(fnum)

    End Function

    Sub subCHARGE�����t���O�̕ύX(ByRef Kogo As String, ByRef ftyp As Short, ByRef flg As Short)
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
        Dim fnum, recno As Short
        Dim fname, ProFile, WinDir As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        '�H�������̧�ق���o�^̧��ں��ވʒu��ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"
        '�o�^̧��ں��ވʒu���擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", "-999", ans.Value, MAX_CHAR, ProFile)
        recno = CShort(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1))

        If recno <= 0 Then
            Exit Sub
        End If

        'Windows�ިڸ�؁i�� D:\WINNT�j���߽�����擾����B
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        '����޺��ޓo�^̧�فiCHARGE.INI�j�̓o�^̧��ں��ވʒu��ǂݍ��݁A���g�܂��͓W�J�����׸ނ�ύX���A�������ށB

        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������


        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.ReadWrite, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(fnum, RecData, recno)

        If ftyp = 1 Then '���g�ꊇ����
            RecData.skl_shori_flg = flg
        ElseIf ftyp = 2 Then  '�W�J����
            RecData.tenkai_flg = flg
        End If

        'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FilePut(fnum, RecData, recno)
        FileClose(fnum)

    End Sub

    Function fun�`���[�W�R�[�h�̗L��(ByRef Kogo As String, ByRef ccode As String) As Short
        '---------------------------------------------------------------------------------------------------
        '�@�\
        '   �H���̉��ɂ���H�������̧�ق�������޺��ނ�ǂݍ��݁A
        '   ����޺��ޓo�^̧�قɓo�^����Ă��邩���肷��B
        '   �H�������̧�قɓo�^̧��ں��ވʒu���������ށB
        '����
        '   kogo(I)�F�����ΏۍH���i�ިڸ�ؖ��j
        '   ccode(O)�F�H�������̧�ق���ǂݍ�������޺���
        '�߂�l
        '   -999�F����޺��ޓo�^̧�قɓo�^����Ă��Ȃ��B
        '   0�F�H�������̧�ق�����޺��ނ�"FREE-JUPITER"�ŁA����޺��ނ��������Ȃ��i���ٌ_���ЁA�Г��AYBC�̂Ƃ��j
        '   1�ȏ�F����޺��ޓo�^̧�قɓo�^����Ă��āA�����̐��l�͓o�^�ʒu��ں��ޔԍ�
        '---------------------------------------------------------------------------------------------------
        Dim fnum, reccnt As Short
        Dim WinDir, ProFile, fname, rcode As String
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
        Dim msg As String
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim userName, SysFile As String
        Dim ret As Integer
        Dim lngWindowsDirectoryLength As Integer
        'UPGRADE_WARNING: Arrays in structure RecData may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        '2017/07/28 Nakagawa Edit Start
        'Dim RecData As CHARGE
        Dim RecData As New CHARGE()
        '2017/07/28 Nakagawa Edit End
        Dim strFileName As String

        strFileName = FunGetLanguage()

        fun�`���[�W�R�[�h�̗L�� = -999

        '�H�������̧�ق�������޺��ނ�ǂݍ��ށB
        ProFile = FunGetNewKoji() & Kogo & "\PROFILE.INI"


        '����޺��ނ��擾����B
        ret = GetPrivateProfileString("�`���[�W�R�[�h", "�`���[�W�R�[�h", "NO-CHARGE", ans.Value, MAX_CHAR, ProFile)
        ccode = Trim(UCase(Left(ans.Value, InStr(ans.Value, Chr(0)) - 1)))

        '�H�������̧�ق�����޺��ނ��L�q����Ă��Ȃ��ꍇ�B
        If ccode = "NO-CHARGE" Then
            '�H�������̧�قɓo�^̧��ں��ވʒu���������ށB
            ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��), ProFile)
            Exit Function
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
            MsgBox(msg, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
            End
            Exit Function
        End If
        '����޺��ނ�FREE-JUPITER��NKK�AIHI�̂Ƃ��A����޺��ޓo�^̧�ق������͍s��Ȃ��B
        Call subGetUserName(userName)

        '<<<<2014/06/04 YBG�ł��`���[�W��ǂދ@�\��ǉ�
        '   ���[�U�[��YBG�@�@��FuncGetYBCFunction = "0"�@�̏ꍇ�́A�`���[�W�Ȃ�"
        '    If (ccode = "FREE-JUPITER") And _
        ''       ((UCase(userName) = "NKK") Or (UCase(userName) = "IHI") Or _
        ''        (UCase(userName) = "YBG")) Then
        If (ccode = "FREE-JUPITER") And ((UCase(userName) = "NKK" And FuncGetOptionFunction() = "0") Or (UCase(userName) = "IHI") Or (UCase(userName) = "YBG" And FuncGetOptionFunction() = "0")) Then
            '>>>>2014/06/04 YBG�ł��`���[�W��ǂދ@�\��ǉ�
            fun�`���[�W�R�[�h�̗L�� = reccnt
            '�H�������̧�قɓo�^̧��ں��ވʒu���������ށB
            ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��), ProFile)
            Exit Function
        End If

        'Windows�ިڸ�؁i�� D:\WINNT�j���߽�����擾����B
        lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
        WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)

        '����޺��ޓo�^̧�فiCHARGE.INI�j��ǂݍ��݂Ȃ���A���̍H��������޺��ނ��o�^����Ă��邩����������B
        '<<<<2014/06/05 �`���[�W�t�@�C����c:Jupiter ������
        'fname = WinDir & "\CHARGE\CHARGE.INI"
        fname = get_CHARGE_FileName()
        '>>>>2014/06/05 �`���[�W�t�@�C����c:Jupiter ������



        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(fname, FileAttribute.Normal) = "" Then
            Beep()
            If Not strFileName Like "*ENG.INI" Then
                msg = "����޺��ޓo�^̧��(CHARGE.INI)������܂���B" & Chr(10)
                msg = msg & "����̧�ق������Ə������p���o���܂���B"
            Else
                msg = "There is no charge code registration file (CHARGE.INI)." & Chr(10)
                msg = msg & "Processing is uncontinuable unless this file exists."
            End If
            MsgBox(msg, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
            Exit Function
        End If

        fnum = FreeFile()
        '2017/07/28 Nakagawa Edit Start
        'FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , FILE_SIZE)
        '2017/07/28 Nakagawa Edit End
        Do While Not EOF(fnum)
            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            '2017/07/28 Nakagawa Edit Start
            'FileGet(fnum, RecData)
            'reccnt = reccnt + 1
            reccnt = reccnt + 1
            Try
                FileGet(fnum, RecData, reccnt)
            Catch ex As Exception
                Exit Do
            End Try
            '2017/07/28 Nakagawa Edit End

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
        ret = WritePrivateProfileString("�`���[�W�R�[�h", "�o�^�t�@�C�����R�[�h�ʒu", CStr(fun�`���[�W�R�[�h�̗L��), ProFile)

    End Function

    '2017/04/04 Nakagawa Add Start
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
        Dim ans As New VB6.FixedLengthString(MAX_CHAR)
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
            ret = GetPrivateProfileString("�`���[�W�R�[�h", "�`���[�W�R�[�h", "NO-CHARGE", ans.Value, MAX_CHAR, ProFile)
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
        FileOpen(fnum, fname, OpenMode.Random, OpenAccess.Read, , Len(RecData))
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
                Dim stEnd As String = RecData.shori_shuuryou
                Dim result As Integer = Date.Compare(stNow, stEnd)
                If result = 1 Then
                    fun�`���[�W�R�[�h�̗L��_�W�J�m�F = -999
                End If
                Exit Do
            End If
        Loop
        FileClose(fnum)

    End Function
    '2017/04/04 Nakagawa Add End

    Function get_CHARGE_FileName() As String
        '2014/06/09 �`���[�W�t�@�C����Ԃ��֐�
        '���D���c:\Jupiter\CHARGE\CHARGE.INI"
        '���D���C:\Windows\charge\CHARGE.INI"�Ƃ���
        '
        Dim strWindowsDirectoryBuffer As New VB6.FixedLengthString(260)
        Dim lngWindowsDirectoryLength As Integer
        Dim WinDir As String
        Dim CHARGE_FileName As String
        CHARGE_FileName = FunGetGensun() & "CHARGE\CHARGE.INI"


        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(CHARGE_FileName, FileAttribute.Normal) = "" Then
            '���D���C:\Windows\charge\CHARGE.INI"�Ƃ��� ���܂܂łǂ���
            'Windows�ިڸ�؁i�� D:\WINNT�j���߽�����擾����B
            lngWindowsDirectoryLength = GetWindowsDirectory(strWindowsDirectoryBuffer.Value, Len(strWindowsDirectoryBuffer.Value))
            WinDir = Left(strWindowsDirectoryBuffer.Value, InStr(strWindowsDirectoryBuffer.Value, vbNullChar) - 1)
            get_CHARGE_FileName = WinDir & "\CHARGE\CHARGE.INI"
        Else
            '���D�� c:\Jupiter\CHARGE\CHARGE.INI"���������ꍇ
            get_CHARGE_FileName = CHARGE_FileName

        End If
    End Function
End Module
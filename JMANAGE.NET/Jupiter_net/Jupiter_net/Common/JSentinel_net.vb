Option Strict Off
Option Explicit On
Module JSentinel
	
	'***************************************************************************************************
	' �g�p���Ă���DLL�̊O����ۼ��ެ�̐錾
	'***************************************************************************************************
	'***************************************************************************************************
	' ����ق̏��ǂݍ���
	'***************************************************************************************************
	Declare Function ReadProtectData_YTI Lib "ytiPrtct.dll" (ByVal No As Short, ByVal subno As Short, ByRef limdate As Integer, ByRef limcnt As Integer, ByRef usr As Short, ByRef ver As Short) As Short
	'����No.��޼���No.��
	'�g�p������(�V���A���l)�A
	'�����J�E���^�l�A
	'���[�U�^�C�v�A
	'�o�[�W����No.��Ԃ�
	'(result:����I��==1/�ُ�I��==0/���ϲè��������==2)
	'���ϲè���������ꍇ�A�����Ƃ��Ă͉����Ԃ��Ȃ��B
	
	Declare Function EncodeDate_YTI Lib "ytiPrtct.dll" (ByVal yy As Short, ByVal mm As Short, ByVal dd As Short) As Integer
	'���t(YYMMDD)�ŁA�v���e�N�g�p���t�V���A���l�����߂�
	
	Declare Function SetLastAccessDate_YTI Lib "ytiPrtct.dll" (ByVal No As Short, ByVal subno As Short) As Short
	'����No.��޼���No.�ōŌ�ɃA�N�Z�X���ꂽ���t���`�F�b�N�i�߂���Ă��Ȃ����j
	'OK�ł���Ό��݂̓��t���Z�b�g����
	'result:�ُ�I��==0/����I��==1/���ϲè==2/���t���߂��ꂽ==-1
	'���ϲè���������ꍇ�A���t�͕ύX����Ȃ��B
	
	Declare Function Dec_Counter_YTI Lib "ytiPrtct.dll" (ByVal No As Short, ByVal subno As Short) As Short
	'����No.��޼���No.�Ŏw�肳�ꂽ���R�[�h�̃J�E���^���P���炷
	'result:�ُ�I��==0/����I��==1/���ϲè==2
	'���ϲè���������ꍇ�A�J�E���^�͕ύX����Ȃ��B
	
	Private Declare Function ReadProtectData_JPT2 Lib "JptPrtct.dll" (ByVal SysID As Integer, ByVal SubSysID As Integer, ByRef OPT As Integer, ByRef maxUse As Integer, ByRef inUse As Integer) As Integer
	Private Declare Function ReadProtectData_JPT2_PmfEdit Lib "JptPrtct.dll" (ByVal SysID As Integer, ByVal SubSysID As Integer, ByRef OPT As Integer, ByRef maxUse As Integer, ByRef inUse As Integer) As Integer
	'--ins '10.09.13 mimori st
	Private Declare Function ReadProtectData_JPT2_General Lib "JptPrtct.dll" (ByVal SysID As Integer, ByVal SubSysID As Integer, ByRef OPT As Integer, ByRef maxUse As Integer, ByRef inUse As Integer, ByVal LicenseFileGen As String) As Integer
	'--ins '10.09.13 mimori ed
	'�V�X�e���ԍ�(99)�A�T�u�V�X�e���ԍ����A�o�[�W�����ԍ��A�ő�g�p�\���A�g�p����Ԃ�
	'result:����I��==0/�ُ�I��==���̑�
	
	Private Declare Function RegOpenKeyEx Lib "ADVAPI32"  Alias "RegOpenKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
	'���W�X�g���̃L�[���J��
	'result:����I��==0/�ُ�I��==���̑�
	
	Private Declare Function RegQueryValueExStr Lib "ADVAPI32"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByVal lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
	'���W�X�g���̒l���擾����
	'result:����I��==0/�ُ�I��==���̑�
	
	Private Declare Function RegCloseKey Lib "ADVAPI32" (ByVal hKey As Integer) As Integer
	'���W�X�g���̃n���h�����J������
	'result:����I��==0/�ُ�I��==���̑�
	
	Private Declare Function InitializeLicense_JPT Lib "JptPrtct.dll" () As Integer
	'--ins 10.09.16 mimori st
	Private Declare Function InitializeLicense_JPT_General Lib "JptPrtct.dll" (ByVal LicenseFileGen As String) As Integer
	'--ins 10.09.16 mimori ed
	'�l�b�g���[�N���C�Z���X�̏��������s��
	'result:����I��==0/�ُ�I��==���̑�
	
	Private Declare Function BorrowLicense_JPT Lib "JptPrtct.dll" (ByVal SubSysID As Integer, ByVal OPT As Integer) As Integer
	'�l�b�g���[�N���C�Z���X�̎擾(�g�p��+1)���s��
	'result:����I��==0/�ُ�I��==���̑�
	
	Public Declare Function GetLicense_JPT Lib "JptPrtct.dll" (ByVal SubSysID As Integer, ByRef OPT As Integer) As Integer
	'�l�b�g���[�N���C�Z���X�̃I�v�V����(�f�ʃ^�C�v)���擾����
	'result:����I��==0/�ُ�I��==���̑�
	
	Private Declare Function ReturnLicense_JPT Lib "JptPrtct.dll" (ByVal SubSysID As Integer, ByVal OPT As Integer) As Integer
	'�l�b�g���[�N���C�Z���X�̕ԋp(�g�p��-1)���s��
	'result:����I��==0/�ُ�I��==���̑�
	
	Private Declare Function FinishLicense_JPT Lib "JptPrtct.dll" () As Integer
	'�l�b�g���[�N���C�Z���X�̏I���������s��
	'result:����I��==0/�ُ�I��==���̑�
	
	Private Declare Function GetUses_JPT Lib "JptPrtct.dll" (ByVal SubSysID As Integer, ByVal OPT As Integer, ByRef maxUse As Integer, ByRef inUse As Integer) As Integer
	'--ins 10.09.13 mimori st
	Private Declare Function GetUses_JPT_General Lib "JptPrtct.dll" (ByVal SubSysID As Integer, ByVal OPT As Integer, ByRef maxUse As Integer, ByRef inUse As Integer, ByVal LicenseFileName As String) As Integer
	'--ins 10.09.13 mimori ed
	'�l�b�g���[�N���C�Z���X�̍ő�g�p���A���݂̎g�p�����擾����
	'result:����I��==0/�ُ�I��==���̑�
	
	''2012/03/15
	'Private Declare Function casCatchOption Lib "casProtectLib.dll" _
	''                ( _
	''                    ByVal SysID As Integer, ByVal SubSysID As Integer _
	''                ) As Long
	''�g�p�\�Ȍ��̍\���`�����擾����B
	''result:�g�p�\�Ȍ��̍\���`��
	
	'***************************************************************************************************
	'հ�ް��`�萔�̐錾
	'***************************************************************************************************
	Public Const ProductSystemNo251 As Short = 251
	Public Const ProductSystemNo252 As Short = 252
	Public Const ProductSystemNo253 As Short = 253
	Public Const ProductSystemNo254 As Short = 254
	Public Const ProductSubSystemNo0 As Short = 0
	Public Const ProductSubSystemNo1 As Short = 1
	Public Const ProductSubSystemNo2 As Short = 2
	Public Const ProductSubSystemNo4 As Short = 4
	Public Const ProductSubSystemNo8 As Short = 8
	Public Const ProductSubSystemNo16 As Short = 16
	
	Public Const JupiterSystemNo As Short = 99
	
	Public Const TypRcB As Short = 1 'RC�����̾���ٔԍ�
	Public Const TypDkB As Short = 2 '�|���Ŕ����̾���ٔԍ�
	Public Const TypRcI As Short = 4 'RC�包�̾���ٔԍ�
	Public Const TypGen As Short = 8 '�ėp�̾���ٔԍ�
	Public Const TypPier As Short = 16 '�|���r�̾���ٔԍ�
	Public Const TypPSlb As Short = 32 '�������ł̾���ٔԍ��@2012/03/20 simo �ǉ�
    Public Const TypWWeb As Short = 64 '�g�`�E�F�u�̾���ٔԍ��@2015/09/15 Nakagawa �ǉ�
    Public Const TypSeg As Short = 128 '�|���Z�O�����g�̾���ٔԍ��@2015/09/15 Nakagawa �ǉ�

	Function �I�v�V�����g�p�̎擾(ByRef isys As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ����ق�CastarJupiter�̏��(���єԍ�=isys)���������܂�Ă���(�g�p�\)���m�F����B
		'����
		'   isys(I)�F�g�p�\�ł����߼�݂ɑΉ������ԍ�
		'           253�F��b�������g�p�\
		'�߂�l
		'   TRUE�F��񂪎擾�o�����B
		'   FALSE�F��񂪎擾�o���Ȃ������B
		'---------------------------------------------------------------------------------------------------
        Dim lim, limcnt As Integer
        Dim usr, ver As Short
        Dim maxUse, OPT, inUse As Integer

        'mod by xiaoyun.z 20140408 sta
        '�I�v�V�����g�p�̎擾 = True
		
        �I�v�V�����g�p�̎擾 = False

        If ���C�Z���X�^�C�v�̎擾() = 3 Then
            '���Z���`�l����
            'If ReadProtectData_YTI(isys, ProductSubSystemNo0, lim, limcnt, usr, ver) = 1 Then
            '    �I�v�V�����g�p�̎擾 = True
            '    Exit Function
            'End If

            If ReadProtectData_JPT2(JupiterSystemNo, isys, OPT, maxUse, inUse) = 0 Then
                If maxUse > 0 Then '2012/10/04 simo
                    �I�v�V�����g�p�̎擾 = True
                End If '2012/10/04 simo
            End If
        Else
            '�l�b�g���[�N�Ή���
            If GetUses_JPT(isys, OPT, maxUse, inUse) = 0 Then
                If maxUse <> 0 And maxUse >= inUse Then
                    �I�v�V�����g�p�̎擾 = True
                End If
            End If
        End If
        'mod by xiaoyun.z 20140408 end
		
	End Function
	
	Function �I�v�V�����@�\�̎擾(ByRef isys As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ����ق�CastarJupiter�̏��(���єԍ�=isys)���������܂�Ă���(�g�p�\)���m�F����B
		'����
		'   isys(I)�F�g�p�\�ł����߼�݂ɑΉ������ԍ�
		'           253�F��b�������g�p�\
		'�߂�l
		'   TRUE�F��񂪎擾�o�����B
		'   FALSE�F��񂪎擾�o���Ȃ������B
		'---------------------------------------------------------------------------------------------------
        'Dim lim As Long, limcnt As Long
        'Dim usr As Integer, ver As Integer
        Dim OPT As Long, maxUse As Long, inUse As Long

        'mod by xiaoyun.z 20150408 sta
        '�I�v�V�����@�\�̎擾 = True
		
        �I�v�V�����@�\�̎擾 = False

        Dim LicenseFileName As String = ""
        If isys = 42 Or isys = 102 Then LicenseFileName = "FlgKegaki.LIC"

        If ���C�Z���X�^�C�v�̎擾() = 3 Then
            If ReadProtectData_JPT2_General(JupiterSystemNo, isys, OPT, maxUse, inUse, LicenseFileName) = 0 Then
                �I�v�V�����@�\�̎擾 = True
            End If
        Else
            If GetUses_JPT_General(isys, OPT, maxUse, inUse, LicenseFileName) = 0 Then
                �I�v�V�����@�\�̎擾 = True
            End If
        End If
        'mod by xiaoyun.z 20150408 end
		
	End Function
	
	
	Function �f�ʎg�p�̊m�F(ByRef isys As Short, ByRef isub As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   �Ȱ�ެ����s(�V�K�H���A�����H��)���̒f��(RC�����A�|���Ŕ����ARC�包 ��)���A
		'   ����ق�CastarJupiter�̏��(���єԍ�=isys�A��޼��єԍ�=isub)�ɏ������܂�Ă���(�g�p�\)���m�F����B
		'����
		'   isys(I)�FJupiter�Ȱ�ެ���Jupiter���ͼ��т��̕�
		'            251�FJupiter�Ȱ�ެ�
		'            252�FJupiter���ͼ���
		'   isub(I)�F�I�������f�ʂɑΉ������ԍ��A�܂��́A���̑g�ݍ��킹
		'            1�FRC����
		'            2�F�|���Ŕ���
		'            4�FRC�包
		'            8�F�ėp
		'�߂�l
		'   TRUE�F��񂪏������܂�Ă���(�g�p�\)�B
		'   FALSE�F��񂪏������܂�Ă��Ȃ��A�܂��;���ق��ُ�(�g�p�s�\)�B
		'---------------------------------------------------------------------------------------------------
        Dim i As Integer, No As Integer
        Dim lim As Long, limcnt As Long
        Dim usr As Integer, ver As Integer
        Dim OPT As Long, maxUse As Long, inUse As Long

        '�f�ʎg�p�̊m�F = True
        �f�ʎg�p�̊m�F = False

        If ���C�Z���X�^�C�v�̎擾() = 3 Then
            '���Z���`�l����
            '��޼��єԍ��̍ő�l(1(RC����)+2(�|���Ŕ���)+4(RC�包)+8(�ėp))
            'No = 15

            'For i = 0 To No
            '    If ReadProtectData_YTI(isys, i, lim, limcnt, usr, ver) = 1 Then
            '        If i = 0 Then
            '            '�S�Ă̒f�ʂ��g�p�\�ł���B
            '            �f�ʎg�p�̊m�F = True
            '        Else
            '            '�g�p�\�Ȓf�ʔԍ������Z����Ċi�[����Ă���B
            '            'RC������RC�包���g�p�\�ł���΁A1(RC����)+4(RC�包)=5�ƂȂ�B
            '            If (i And isub) = isub Then
            '                �f�ʎg�p�̊m�F = True
            '            End If
            '        End If
            '        Exit Function
            '    End If
            'Next i

            If ReadProtectData_JPT2(JupiterSystemNo, isys, OPT, maxUse, inUse) = 0 Then
                If OPT = 0 Then
                    �f�ʎg�p�̊m�F = True
                Else
                    If (OPT And isub) = isub Then
                        �f�ʎg�p�̊m�F = True
                    End If
                End If
            End If
        Else
            '�l�b�g���[�N�Ή���
            OPT = isub
            �f�ʎg�p�̊m�F = False
            If GetLicense_JPT(isys, OPT) = 0 Then
                �f�ʎg�p�̊m�F = True
            End If

            '        If GetLicense_JPT(isys, opt) = 0 Then
            '            If opt = 0 Then
            '                '<<<<2012/03/15 simo
            '                '�f�ʎg�p�̊m�F = True
            '                �f�ʎg�p�̊m�F = False
            '                '>>>>2012/03/15 simo
            '            Else
            '                If (opt And isub) = isub Then
            '                    �f�ʎg�p�̊m�F = True
            '                End If
            '            End If
            '        End If
        End If
		
	End Function
	
	Function Jupiter���[�U�[�m�F(ByRef isys As Short, ByRef osub As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ����ق�CastarJupiter�̏��(���єԍ�=isys�A��޼��єԍ�=*)���������܂�Ă��邩�m�F����B
		'   ��޼��єԍ��́A0:�S�`���A1:RC�����A2:�|���Ŕ����A4:RC�包�A����т��̑g�ݍ��킹���������܂�Ă���B
		'   Jupiter�Ȱ�ެ��̏ꍇ�A�e���؂���ϰ���۰قŏ�Ɋm�F����B
		'����
		'   isys(I)�FJupiter�Ȱ�ެ���Jupiter���ͼ��т��̕�
		'            251�FJupiter�Ȱ�ެ�
		'            252�FJupiter���ͼ���
		'   osub(O)�F�o�^����Ă���f�ʂɑΉ������ԍ��A�܂��́A���̑g�ݍ��킹
		'            1�FRC����
		'            2�F�|���Ŕ���
		'            4�FRC�包
        '            8�F�ėp
        '           
		'�߂�l
		'   TRUE�F��񂪏������܂�Ă���(�g�p�\)�B
		'   FALSE�F��񂪏������܂�Ă��Ȃ��A�܂��;���ق��ُ�(�g�p�s�\)�B
		'---------------------------------------------------------------------------------------------------
        Dim i As Integer, No As Integer
        Dim lim As Long, limcnt As Long
        Dim usr As Integer, ver As Integer
        Dim OPT As Long, maxUse As Long, inUse As Long

        'Jupiter���[�U�[�m�F = True
        Jupiter���[�U�[�m�F = False

        If ���C�Z���X�^�C�v�̎擾() = 3 Then
            '���Z���`�l����
            '��޼��єԍ��̍ő�l(1(RC����)+2(�|���Ŕ���)+4(RC�包)+8(�ėp))
            'No = 15

            'For i = 0 To No
            '    If ReadProtectData_YTI(isys, i, lim, limcnt, usr, ver) = 1 Then
            '        osub = i
            '        Jupiter���[�U�[�m�F = True
            '        Exit Function
            '    End If
            'Next i

            If ReadProtectData_JPT2(JupiterSystemNo, isys, OPT, maxUse, inUse) = 0 Then
                osub = OPT
                Jupiter���[�U�[�m�F = True
            End If
        Else
            '�l�b�g���[�N�Ή���
            '<<<<2012/03/16 simo GetLicense_JPT �̎d�l�ύX�̂���
            If isys = 251 Or isys = 252 Then
                osub = 0
                If GetLicense_JPT(isys, 1) = 0 Then osub = osub + 1
                If GetLicense_JPT(isys, 2) = 0 Then osub = osub + 2
                If GetLicense_JPT(isys, 4) = 0 Then osub = osub + 4
                If GetLicense_JPT(isys, 8) = 0 Then osub = osub + 8
                If GetLicense_JPT(isys, 16) = 0 Then osub = osub + 16
                If GetLicense_JPT(isys, 32) = 0 Then osub = osub + 32
                If GetLicense_JPT(isys, 64) = 0 Then osub = osub + 64
                If osub <> 0 Then
                    Jupiter���[�U�[�m�F = True
                End If
                '<<<<2012/03/16 simo GetLicense_JPT �̎d�l�ύX�̂���
            ElseIf GetLicense_JPT(isys, OPT) = 0 Then
                osub = OPT
                Jupiter���[�U�[�m�F = True
            End If
        End If
		
	End Function
	
	Function ���C�Z���X�^�C�v�̎擾() As Integer
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   ���W�X�g�����烉�C�Z���X�̃^�C�v���擾����B
		'����
		'   �Ȃ�
		'�߂�l
		'   0:�X�^���h�A���[����
		'   1:�l�b�g���[�N�ŃT�[�o�[
		'   2:�l�b�g���[�N�ŃN���C�A���g
		'   3:���Z���`�l����
		'---------------------------------------------------------------------------------------------------
		Dim hKey As Integer
		Dim valuedata As String
		Dim length As Integer
		
		���C�Z���X�^�C�v�̎擾 = 3
		
		'    'USB�ׯ�����(����ޱ�ݔ�)�����݂���ꍇ�uײ�ݽ����=3�v�Ɠ�������������B
		'    If InitializeLicense_JPT <> 0 Then
		
		'HKEY_LOCAL_MACHINE��Software\Yti\Jupiter���烉�C�Z���X�����擾����
        If RegOpenKeyEx(&H80000002, "Software\Yti\Jupiter", 0, 1, hKey) <> 0 Then
            Exit Function
        End If
		
		valuedata = New String(Chr(0), 250)
		length = Len(valuedata)
		
		If RegQueryValueExStr(hKey, "LicenseType", 0, 0, valuedata, length) <> 0 Then
            RegCloseKey(hKey)
			Exit Function
		End If
		
		���C�Z���X�^�C�v�̎擾 = Val(valuedata)
		
		'    'USB�ׯ�����(����ޱ�ݔ�)�����݂���ꍇ�uײ�ݽ����=3�v�Ɠ�������������B
		'    End If
		
        RegCloseKey(hKey)
	End Function
	
	Function �l�b�g���[�N���C�Z���X�̏�����() As Boolean
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   �l�b�g���[�N���C�Z���X�̏��������s���B
		'   �l�b�g���[�N�ŃT�[�o�[�A�N���C�A���g�̓��C�Z���X���j�^�[�ōs�����߁A�X�^���h�A���[���ł̂ݍs���B
		'����
		'   �Ȃ�
		'�߂�l
		'   TRUE�F�������ɐ�������
		'   FALSE�F�������Ɏ��s����
		'---------------------------------------------------------------------------------------------------
		�l�b�g���[�N���C�Z���X�̏����� = False
		
		If ���C�Z���X�^�C�v�̎擾() = 0 Then
			If InitializeLicense_JPT() = 0 Then
				�l�b�g���[�N���C�Z���X�̏����� = True
			End If
			Call InitializeLicense_JPT_General("FlgKegaki.lic")
		Else
			�l�b�g���[�N���C�Z���X�̏����� = True
		End If
	End Function
	
	Function �l�b�g���[�N���C�Z���X�̏I��() As Boolean
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   �l�b�g���[�N���C�Z���X�̏I���������s���B
		'   �l�b�g���[�N�ŃT�[�o�[�A�N���C�A���g�̓��C�Z���X���j�^�[�ōs�����߁A�X�^���h�A���[���ł̂ݍs���B
		'����
		'   �Ȃ�
		'�߂�l
		'   TRUE�F�I�������ɐ�������
		'   FALSE�F�I�������Ɏ��s����
		'---------------------------------------------------------------------------------------------------
		�l�b�g���[�N���C�Z���X�̏I�� = False
		
		If ���C�Z���X�^�C�v�̎擾() = 0 Then
			If FinishLicense_JPT() = 0 Then
				�l�b�g���[�N���C�Z���X�̏I�� = True
			End If
		Else
			�l�b�g���[�N���C�Z���X�̏I�� = True
		End If
	End Function
	
	Function �l�b�g���[�N���C�Z���X�̎擾(ByRef isys As Short, ByRef isub As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   �l�b�g���[�N���C�Z���X�̎擾(�g�p��+1)���s���B
		'����
		'   isys(I)�F���C�Z���X�̃V�X�e���ԍ�
		'            251�FJupiter�}�l�[�W���[
		'            252�FJupiter���̓V�X�e��
		'            253�F��b����
		'   isub(I)�F�f�ʃ^�C�v
		'            1�FRC����
		'            2�F�|���Ŕ���
		'            4�FRC�包
		'            8�F�ėp
		'�߂�l
		'   TRUE�F�擾�ɐ�������
		'   FALSE�F�擾�Ɏ��s����
		'---------------------------------------------------------------------------------------------------
		�l�b�g���[�N���C�Z���X�̎擾 = False
		
		If BorrowLicense_JPT(isys, isub) = 0 Then
			�l�b�g���[�N���C�Z���X�̎擾 = True
		End If
	End Function
	
	Function �l�b�g���[�N���C�Z���X�̕ԋp(ByRef isys As Short, ByRef isub As Short) As Boolean
		'---------------------------------------------------------------------------------------------------
		'�@�\
		'   �l�b�g���[�N���C�Z���X�̕ԋp(�g�p��-1)���s���B
		'����
		'   isys(I)�F���C�Z���X�̃V�X�e���ԍ�
		'            251�FJupiter�}�l�[�W���[
		'            252�FJupiter���̓V�X�e��
		'            253�F��b����
		'   isub(I)�F�f�ʃ^�C�v
		'            1�FRC����
		'            2�F�|���Ŕ���
		'            4�FRC�包
		'            8�F�ėp
		'�߂�l
		'   TRUE�F�ԋp�ɐ�������
		'   FALSE�F�ԋp�Ɏ��s����
		'---------------------------------------------------------------------------------------------------
		�l�b�g���[�N���C�Z���X�̕ԋp = False
		
		If ReturnLicense_JPT(isys, isub) = 0 Then
			�l�b�g���[�N���C�Z���X�̕ԋp = True
		End If
	End Function
	
	'Function ���`��(isys As Integer, isub As Integer) As Boolean
	''---------------------------------------------------------------------------------------------------
	''�@�\
	''   ���`���̎g�p�ۂ𔻒肷��B
	''����
	''   isys(I)�F���C�Z���X�̃V�X�e���ԍ�
	''            251�FJupiter�}�l�[�W���[
	''            252�FJupiter���̓V�X�e��
	''            253�F��b����
	''   isub(I)�F�f�ʃ^�C�v
	''            1�FRC����
	''            2�F�|���Ŕ���
	''            4�FRC�包
	''            8�F�ėp
	''�߂�l
	''   TRUE �F�g�p��
	''   FALSE�F�g�p�s��
	''---------------------------------------------------------------------------------------------------
	'    ���`�� = False
	'
	'    If casCatchOption() = 0 Then
	'        ���`�� = True
	'    End If
	'End Function
End Module
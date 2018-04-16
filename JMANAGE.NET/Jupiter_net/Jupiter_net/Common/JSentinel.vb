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
	
	Private Declare Function ReadProtectData_JPT2 Lib "JptPrtct.dll" (ByVal SysId As Integer, ByVal SubSysId As Integer, ByRef opt As Integer, ByRef maxUse As Integer, ByRef inUse As Integer) As Integer
	'�V�X�e���ԍ�(99)�A�T�u�V�X�e���ԍ����A�o�[�W�����ԍ��A�ő�g�p�\���A�g�p����Ԃ�
	'result:����I��==0/�ُ�I��==���̑�
	
	'***************************************************************************************************
	'հ�ް��`�萔�̐錾
	'***************************************************************************************************
	Public Const ProductSystemNo251 As Short = 251
	Public Const ProductSystemNo252 As Short = 252
	Public Const ProductSystemNo253 As Short = 253
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
		Dim maxUse, opt, inUse As Integer
		
		�I�v�V�����g�p�̎擾 = False
		
		If ReadProtectData_YTI(isys, ProductSubSystemNo0, lim, limcnt, usr, ver) = 1 Then
			�I�v�V�����g�p�̎擾 = True
			Exit Function
		End If
		
		If ReadProtectData_JPT2(JupiterSystemNo, isys, opt, maxUse, inUse) = 0 Then
			�I�v�V�����g�p�̎擾 = True
		End If
		
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
		Dim i, No As Short
		Dim lim, limcnt As Integer
		Dim usr, ver As Short
		Dim maxUse, opt, inUse As Integer
		
		�f�ʎg�p�̊m�F = False
		
		'��޼��єԍ��̍ő�l(1(RC����)+2(�|���Ŕ���)+4(RC�包)+8(�ėp))
		No = 15
		
		For i = 0 To No
			If ReadProtectData_YTI(isys, i, lim, limcnt, usr, ver) = 1 Then
				If i = 0 Then
					'�S�Ă̒f�ʂ��g�p�\�ł���B
					�f�ʎg�p�̊m�F = True
				Else
					'�g�p�\�Ȓf�ʔԍ������Z����Ċi�[����Ă���B
					'RC������RC�包���g�p�\�ł���΁A1(RC����)+4(RC�包)=5�ƂȂ�B
					If (i And isub) = isub Then
						�f�ʎg�p�̊m�F = True
					End If
				End If
				Exit Function
			End If
		Next i
		
		If ReadProtectData_JPT2(JupiterSystemNo, isys, opt, maxUse, inUse) = 0 Then
			If opt = 0 Then
				�f�ʎg�p�̊m�F = True
			Else
				If (opt And isub) = isub Then
					�f�ʎg�p�̊m�F = True
				End If
			End If
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
		'�߂�l
		'   TRUE�F��񂪏������܂�Ă���(�g�p�\)�B
		'   FALSE�F��񂪏������܂�Ă��Ȃ��A�܂��;���ق��ُ�(�g�p�s�\)�B
		'---------------------------------------------------------------------------------------------------
		Dim i, No As Short
		Dim lim, limcnt As Integer
		Dim usr, ver As Short
		Dim maxUse, opt, inUse As Integer
		
		Jupiter���[�U�[�m�F = False
		
		'��޼��єԍ��̍ő�l(1(RC����)+2(�|���Ŕ���)+4(RC�包)+8(�ėp))
		No = 15
		
		For i = 0 To No
			If ReadProtectData_YTI(isys, i, lim, limcnt, usr, ver) = 1 Then
				osub = i
				Jupiter���[�U�[�m�F = True
				Exit Function
			End If
		Next i
		
		If ReadProtectData_JPT2(JupiterSystemNo, isys, opt, maxUse, inUse) = 0 Then
			osub = opt
			Jupiter���[�U�[�m�F = True
			Exit Function
		End If
		
	End Function
End Module
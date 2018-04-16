#pragma warning(disable : 4995)
/*---------------------------------------------------------------------------
 ExtCDaoDatabase.h
	MFC4.2 �� DAO �Ɋւ���@�\��⋭����֐��Q
---------------------------------------------------------------------------*/

#ifndef __JJG30211_EXTCDAODATABASE_H__
#define __JJG30211_EXTCDAODATABASE_H__

#include <afxdao.h> 
extern int UserASSERT(int f,LPCSTR lpszFileName, int nLine);
#ifdef ASSERT
#undef ASSERT
#endif
#define	ASSERT(f)	UserASSERT(f,__FILE__,__LINE__)

/*---------------------------------------------------------------------------
 ���R�[�h�Z�b�g�̃��R�[�h����Ԃ�
	�߂�l
		���R�[�h��
	rs
		���R�[�h�Z�b�g
	���
		CDaoRecordset.GetRecordCount() �����������R�[�h����Ԃ��Ȃ��ꍇ
		������o�O�ւ̑Ή����u�B
		��x�ŉ��i���R�[�h�܂ŃJ�[�\�����ړ�����ΐ��������R�[�h����Ԃ��B
	����
		���̊֐����g�p����ƃJ�[�\���͍ŏ�i���R�[�h�Ɉړ�����B
---------------------------------------------------------------------------*/
inline int GetCount(CDaoRecordset &rs)
{
	int nCount = 0;
	if (rs.IsOpen())
	{
		if (!rs.IsEOF())
			rs.MoveLast();
		nCount = rs.GetRecordCount();
		if (!rs.IsBOF())
			rs.MoveFirst();
	}

	return nCount;
}

/*---------------------------------------------------------------------------
 �J�[�\�����R�[�h�̒l�i�����j��Ԃ�
	�߂�l
		�J�[�\�����R�[�h�̒l
	rs
		���R�[�h�Z�b�g
	strFieldName
		�t�B�[���h��
---------------------------------------------------------------------------*/
inline int GetFieldValueInteger(CDaoRecordset &rs, const CString strFieldName)
{
	COleVariant vValue;
	rs.GetFieldValue(strFieldName, vValue);

#ifdef _DEBUG
	CDaoFieldInfo info;
	rs.GetFieldInfo(strFieldName, info);
	ASSERT(info.m_nType == dbLong || info.m_nType == dbInteger);
#endif

	return vValue.intVal;
}

//�}�C�i�X�̒l�����������Ȃ�ꍇ�Ɏg�p
inline int GetFieldValueInteger2(CDaoRecordset &rs, const CString strFieldName)
{
	COleVariant vValue;
	rs.GetFieldValue(strFieldName, vValue);

#ifdef _DEBUG
	CDaoFieldInfo info;
	rs.GetFieldInfo(strFieldName, info);
	ASSERT(info.m_nType == dbLong || info.m_nType == dbInteger);
#endif
	unsigned short valueUnsignedShort = vValue.intVal;
	short valueShort = valueUnsignedShort;
	int valueInt = (int)valueShort;
	return valueInt;
}

/*---------------------------------------------------------------------------
 �J�[�\�����R�[�h�̒l�i�����j��Ԃ�
	�߂�l
		�J�[�\�����R�[�h�̒l
	rs
		���R�[�h�Z�b�g
	strFieldName
		�t�B�[���h��
---------------------------------------------------------------------------*/
inline double GetFieldValueDouble(CDaoRecordset &rs, const CString strFieldName)
{
	COleVariant vValue;
	rs.GetFieldValue(strFieldName, vValue);

#ifdef _DEBUG
	CDaoFieldInfo info;
	rs.GetFieldInfo(strFieldName, info);
	ASSERT(info.m_nType == dbDouble);
#endif

	return vValue.dblVal;
}

/*---------------------------------------------------------------------------
 �J�[�\�����R�[�h�̒l�i������j��Ԃ�
	�߂�l
		�J�[�\�����R�[�h�̒l
	rs
		���R�[�h�Z�b�g
	strFieldName
		�t�B�[���h��
---------------------------------------------------------------------------*/
inline CString GetFieldValueString(CDaoRecordset &rs, const CString strFieldName)
{
	COleVariant vValue;
	rs.GetFieldValue(strFieldName, vValue);

#ifdef _DEBUG
	CDaoFieldInfo info;
	rs.GetFieldInfo(strFieldName, info);
	ASSERT(info.m_nType == dbText);
#endif

	return vValue.pcVal;
}

/*---------------------------------------------------------------------------
 �J�[�\�����R�[�h�̒l�i�����^�j��Ԃ�
	�߂�l
		�J�[�\�����R�[�h�̒l
	rs
		���R�[�h�Z�b�g
	strFieldName
		�t�B�[���h��
---------------------------------------------------------------------------*/
inline CString GetFieldValueMemo(CDaoRecordset &rs, const CString strFieldName)
{
	COleVariant vValue;
	rs.GetFieldValue(strFieldName, vValue);

#ifdef _DEBUG
	CDaoFieldInfo info;
	rs.GetFieldInfo(strFieldName, info);
	ASSERT(info.m_nType == dbMemo);
#endif

	return vValue.pcVal;
}

#endif	// __JJG30211_EXTCDAODATABASE_H__

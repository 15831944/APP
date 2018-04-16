#include "stdafx.h"
#include "AtdDbSecGrdLeng.h"

JptErrorStatus AtdDbSecGrdLeng::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�f�ʥ�包�̒f�ʒ�]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));		//����
		int nosec = GetFieldValueInteger(rs, _T("NOSEC"));		//�W���C���g��
		double rlsec = GetFieldValueDouble(rs, _T("RLSEC"));	//�u���b�N��
		AtdDbSecGrdLengItem atdDbSecGrdLengItem;
		atdDbSecGrdLengItem.setNogrd(nogrd);
		atdDbSecGrdLengItem.setNosec(nosec);
		atdDbSecGrdLengItem.setRlsec(rlsec);
		this->append(atdDbSecGrdLengItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecGrdLeng::getAt(int index, AtdDbSecGrdLengItem& atdDbSecGrdLengItem)
{
	if(_atdDbSecGrdLengItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecGrdLengItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecGrdLengItem = _atdDbSecGrdLengItemList[index];

	return JPT_OK;
}


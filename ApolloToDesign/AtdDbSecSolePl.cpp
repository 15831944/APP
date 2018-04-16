#include "stdafx.h"
#include "AtdDbSecSolePl.h"

JptErrorStatus AtdDbSecSolePl::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�f�ʥ�\�[���v���[�g]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));	//����
		int nos = GetFieldValueInteger(rs, _T("NOS"));	//
		double bs1 = GetFieldValueDouble(rs, _T("BS1"));	//SOLE-PL��
		double rls1 = GetFieldValueDouble(rs, _T("RLS1"));	//SOLE-PL����
		AtdDbSecSolePlItem atdDbSecSolePlItem;
		atdDbSecSolePlItem.setNogrd(nogrd);
		atdDbSecSolePlItem.setNos(nos);
		atdDbSecSolePlItem.setBs1(bs1);
		atdDbSecSolePlItem.setRls1(rls1);
		this->append(atdDbSecSolePlItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecSolePl::getAt(int index, AtdDbSecSolePlItem& atdDbSecSolePlItem)
{
	if(_atdDbSecSolePlItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecSolePlItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecSolePlItem = _atdDbSecSolePlItemList[index];

	return JPT_OK;
}


#include "stdafx.h"
#include "AtdDbSecGrdHeightConstant.h"

JptErrorStatus AtdDbSecGrdHeightConstant::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�f�ʥ�包����(���E�������)]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));	//
		double hweb = GetFieldValueDouble(rs, _T("HWEB"));	//�E�F�u��
		AtdDbSecGrdHeightConstantItem atdDbSecGrdHeightConstantItem;
		atdDbSecGrdHeightConstantItem.setNogrd(nogrd);
		atdDbSecGrdHeightConstantItem.setHweb(hweb);
		this->append(atdDbSecGrdHeightConstantItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecGrdHeightConstant::getAt(int index, AtdDbSecGrdHeightConstantItem& atdDbSecGrdHeightConstantItem)
{
	if(_atdDbSecGrdHeightConstantItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecGrdHeightConstantItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecGrdHeightConstantItem = _atdDbSecGrdHeightConstantItemList[index];

	return JPT_OK;
}


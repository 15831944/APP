#include "stdafx.h"
#include "AtdDbInputHeight.h"

JptErrorStatus AtdDbInputHeight::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [���ͥ������ω��_]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int inogrd = GetFieldValueInteger(rs, _T("NOGRD"));	//
		int inopt = GetFieldValueInteger(rs, _T("NOPT"));	//
		double hweb = GetFieldValueDouble(rs, _T("HWEB"));	//�E�F�u��
		int itplc = GetFieldValueInteger(rs, _T("ITPLC"));	//�E�F�u����ԕ��@
		AtdDbInputHeightItem atdDbInputHeightItem;
		atdDbInputHeightItem.setInogrd(inogrd);
		atdDbInputHeightItem.setInopt(inopt);
		atdDbInputHeightItem.setHweb(hweb);
		atdDbInputHeightItem.setItplc(itplc);
		this->append(atdDbInputHeightItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputHeight::getAt(int index, AtdDbInputHeightItem& atdDbInputHeightItem)
{
	if(_atdDbInputHeightItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputHeightItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputHeightItem = _atdDbInputHeightItemList[index];

	return JPT_OK;
}


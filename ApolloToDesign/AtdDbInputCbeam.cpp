#include "stdafx.h"
#include "AtdDbInputCbeam.h"

JptErrorStatus AtdDbInputCbeam::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [���ͥ����]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int iclcul = GetFieldValueInteger(rs, _T("ICLCUL"));	//�x�_UFLG_JC �i�_H_FLG_JC
		int iclcwl = GetFieldValueInteger(rs, _T("ICLCWL"));	//�x�_WEB_JC �i�_H_WEB_JC
		int iclcll = GetFieldValueInteger(rs, _T("ICLCLL"));	//�x�_LFLG_JC
		AtdDbInputCbeamItem atdDbInputCbeamItem;
		atdDbInputCbeamItem.setIclcul(iclcul);
		atdDbInputCbeamItem.setIclcwl(iclcwl);
		atdDbInputCbeamItem.seIclcllt(iclcll);
		this->append(atdDbInputCbeamItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputCbeam::getAt(int index, AtdDbInputCbeamItem& atdDbInputCbeamItem)
{
	if(_atdDbInputCbeamItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputCbeamItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputCbeamItem = _atdDbInputCbeamItemList[index];

	return JPT_OK;
}


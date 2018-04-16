#include "stdafx.h"
#include "AtdDbSecHstfHaichi.h"

JptErrorStatus AtdDbSecHstfHaichi::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [fΚ₯HSTFzu]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));		//Ό
		int nopnl = GetFieldValueInteger(rs, _T("NOPNL"));		//n€i_Ό I€i_Ό i_ΤΤ
		int nohsl1 = GetFieldValueInteger(rs, _T("NOHSL1"));	//zuiΤ
		int nohsl2 = GetFieldValueInteger(rs, _T("NOHSL2"));	//zuiΤ
		AtdDbSecHstfHaichiItem atdDbSecHstfHaichiItem;
		atdDbSecHstfHaichiItem.setNogrd(nogrd);
		atdDbSecHstfHaichiItem.setNopnl(nopnl);
		atdDbSecHstfHaichiItem.setNohsl1(nohsl1);
		atdDbSecHstfHaichiItem.setNohsl2(nohsl2);
		this->append(atdDbSecHstfHaichiItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecHstfHaichi::getAt(int index, AtdDbSecHstfHaichiItem& atdDbSecHstfHaichiItem)
{
	if(_atdDbSecHstfHaichiItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecHstfHaichiItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecHstfHaichiItem = _atdDbSecHstfHaichiItemList[index];

	return JPT_OK;
}

int AtdDbSecHstfHaichi::find(const int& ig, const int& ip)
{
	for(int i=0;i<this->size();i++) {
		AtdDbSecHstfHaichiItem shhItem;
		getAt(i, shhItem);
		int nogrd = shhItem.getNogrd();		//NOGRD Ό
		int nopnl = shhItem.getNopnl();		//NOPNL n€i_Ό I€i_Ό i_ΤΤ
		if(nogrd == ig && nopnl == ip) {
			return i;
		}
	}

	return -1;
}


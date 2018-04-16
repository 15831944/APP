#include "stdafx.h"
#include "AtdDbSecVstfHaichi.h"

JptErrorStatus AtdDbSecVstfHaichi::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [fΚ₯VSTFzu]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));	//Ό
		int nopnl = GetFieldValueInteger(rs, _T("NOPNL"));	//n€i_Ό I€i_Ό
		int nvst = GetFieldValueInteger(rs, _T("NVST"));	//Τ_Τ
		int novst = GetFieldValueInteger(rs, _T("NOVST"));	//―ΚID
		AtdDbSecVstfHaichiItem atdDbSecVstfHaichiItem;
		atdDbSecVstfHaichiItem.setNogrd(nogrd);
		atdDbSecVstfHaichiItem.setNopnl(nopnl);
		atdDbSecVstfHaichiItem.setNvst(nvst);
		atdDbSecVstfHaichiItem.setNovst(novst);
		this->append(atdDbSecVstfHaichiItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecVstfHaichi::getAt(int index, AtdDbSecVstfHaichiItem& atdDbSecVstfHaichiItem)
{
	if(_atdDbSecVstfHaichiItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecVstfHaichiItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecVstfHaichiItem = _atdDbSecVstfHaichiItemList[index];

	return JPT_OK;
}

int AtdDbSecVstfHaichi::findId(const int& ig, const int& ip)
{
	for(int i=0;i<this->size();i++) {
		AtdDbSecVstfHaichiItem svhItem;
		getAt(i, svhItem);
		int nogrd = svhItem.getNogrd();		//NOGRD Ό
		int nopnl = svhItem.getNopnl();		//NOPNL
		if(nogrd == ig && nopnl == ip) {
			int novst = svhItem.getNovst();	//NOVST ―ΚID
			return novst;
		}
	}

	return -1;
}


#include "stdafx.h"
#include "AtdDbSecGrdKaku.h"

JptErrorStatus AtdDbSecGrdKaku::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�f�ʥ�包�i�_��]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));		//STRCODE
		if(strcode == 3) {
			int nostr = GetFieldValueInteger(rs, _T("NOSTR"));		//NOSTR
			int nocrs = GetFieldValueInteger(rs, _T("NOCRS"));		//NOCRS
			CString odname = GetFieldValueString(rs, _T("ODNAME"));	//���f����
			odname.Trim();
			AtdDbSecGrdKakuItem atdDbSecGrdKakuItem;
			atdDbSecGrdKakuItem.setOdname(odname);
			atdDbSecGrdKakuItem.setNostr(nostr);
			atdDbSecGrdKakuItem.setNocrs(nocrs);
			this->append(atdDbSecGrdKakuItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecGrdKaku::getAt(int index, AtdDbSecGrdKakuItem& atdDbSecGrdKakuItem)
{
	if(_atdDbSecGrdKakuItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecGrdKakuItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecGrdKakuItem = _atdDbSecGrdKakuItemList[index];

	return JPT_OK;
}


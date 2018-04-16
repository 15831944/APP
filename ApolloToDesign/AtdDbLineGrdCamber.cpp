#include "stdafx.h"
#include "AtdDbLineGrdCamber.h"

JptErrorStatus AtdDbLineGrdCamber::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [���`��包(����/�u���P�b�g)��L�����o�[�l]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 3) {
			int nostr = GetFieldValueInteger(rs, _T("NOSTR"));	//����
			int nopnl = GetFieldValueInteger(rs, _T("NOPNL"));	//
			double zcamz = GetFieldValueDouble(rs, _T("ZCAMZ"));	//Z�L�����o�[
			AtdDbLineGrdCamberItem atdDbLineGrdCamberItem;
			atdDbLineGrdCamberItem.setNostr(nostr);
			atdDbLineGrdCamberItem.setNopnl(nopnl);
			atdDbLineGrdCamberItem.setZcamz(zcamz);
			this->append(atdDbLineGrdCamberItem);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbLineGrdCamber::getAt(int index, AtdDbLineGrdCamberItem& atdDbLineGrdCamberItem)
{
	if(_atdDbLineGrdCamberItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbLineGrdCamberItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbLineGrdCamberItem = _atdDbLineGrdCamberItemList[index];

	return JPT_OK;
}


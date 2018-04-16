#include "stdafx.h"
#include "AtdDbGrdVstfHaichi.h"

JptErrorStatus AtdDbGrdVstfHaichi::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�包���Ԑ����⍄�ޔz�u�f�[�^]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nostr = GetFieldValueInteger(rs, _T("���ސ��R�[�h"));		//���ސ��R�[�h
		int nopanel = GetFieldValueInteger(rs, _T("�p�l���R�[�h"));		//�p�l���R�[�h
		int numvstf = GetFieldValueInteger(rs, _T("�����⍄�ޖ{��"));	//�����⍄�ޖ{��
		AtdDbGrdVstfHaichiItem atdDbGrdVstfHaichiItem;
		atdDbGrdVstfHaichiItem.setNostr(nostr);
		atdDbGrdVstfHaichiItem.setNopanel(nopanel);
		atdDbGrdVstfHaichiItem.setNumvstf(numvstf);
		this->append(atdDbGrdVstfHaichiItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbGrdVstfHaichi::getAt(int index, AtdDbGrdVstfHaichiItem& atdDbGrdVstfHaichiItem)
{
	if(_atdDbGrdVstfHaichiItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbGrdVstfHaichiItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbGrdVstfHaichiItem = _atdDbGrdVstfHaichiItemList[index];

	return JPT_OK;
}


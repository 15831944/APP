#include "stdafx.h"
#include "AtdDbInputStfAki.h"

JptErrorStatus AtdDbInputStfAki::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [���ͥ�r�s�e�̂���]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int idgfvi = GetFieldValueInteger(rs, _T("IDGFVI"));	//���́ESTF�̂��� �^�C�v
		AtdDbInputStfAkiItem atdDbInputStfAkiItem;
		atdDbInputStfAkiItem.setIdgfvi(idgfvi);
		this->append(atdDbInputStfAkiItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbInputStfAki::getAt(int index, AtdDbInputStfAkiItem& atdDbInputStfAkiItem)
{
	if(_atdDbInputStfAkiItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbInputStfAkiItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbInputStfAkiItem = _atdDbInputStfAkiItemList[index];

	return JPT_OK;
}


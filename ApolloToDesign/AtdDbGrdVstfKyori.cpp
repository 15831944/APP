#include "stdafx.h"
#include "AtdDbGrdVstfKyori.h"

JptErrorStatus AtdDbGrdVstfKyori::load(CDaoDatabase& dbFile)
{
	CDaoTableDefInfo tabledef;
	int nTableCount = dbFile.GetTableDefCount();
	bool bResult = false;
	for(int nCnt = 0; nCnt < nTableCount; nCnt++) {
		dbFile.GetTableDefInfo(nCnt, tabledef);
		if(tabledef.m_strName == "�包���Ԑ����⍄�ފԊu�f�[�^") {
			bResult = true;
			break;
		}
	}
	if(bResult != true) {	//�e�[�u�������݂��Ȃ��ꍇ
		return JPT_OK;
	}
	//
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�包���Ԑ����⍄�ފԊu�f�[�^]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nostr = GetFieldValueInteger(rs, _T("���ސ��R�[�h"));			//���ސ��R�[�h
		int nopanel = GetFieldValueInteger(rs, _T("�p�l���R�[�h"));			//�p�l���R�[�h
		int vstfNum = GetFieldValueInteger(rs, _T("�����⍄�ފԊu�ԍ�"));	//�����⍄�ފԊu�ԍ�
		double vstfkyori = GetFieldValueDouble(rs, _T("�����⍄�ފԊu"));	//�����⍄�ފԊu
		AtdDbGrdVstfKyoriItem atdDbGrdVstfKyoriItem;
		atdDbGrdVstfKyoriItem.setNostr(nostr);
		atdDbGrdVstfKyoriItem.setNopanel(nopanel);
		atdDbGrdVstfKyoriItem.setVstfNum(vstfNum);
		atdDbGrdVstfKyoriItem.setVstfkyori(vstfkyori);
		this->append(atdDbGrdVstfKyoriItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbGrdVstfKyori::getAt(int index, AtdDbGrdVstfKyoriItem& atdDbGrdVstfKyoriItem)
{
	if(_atdDbGrdVstfKyoriItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbGrdVstfKyoriItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbGrdVstfKyoriItem = _atdDbGrdVstfKyoriItemList[index];

	return JPT_OK;
}


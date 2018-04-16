#include "stdafx.h"
#include "AtdDbGrdHstfLap.h"

JptErrorStatus AtdDbGrdHstfLap::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�包�����⍄�ރ��b�v�͈�]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int buzaiCode = GetFieldValueInteger(rs, _T("���ސ��R�[�h"));		//���ސ��R�[�h
		int hstfPos = GetFieldValueInteger2(rs, _T("�����⍄�ގ�t�ʒu"));		//�����⍄�ގ�t�ʒu
		int hstfCode = GetFieldValueInteger(rs, _T("�����⍄�ޒf�ʃR�[�h"));	//�����⍄�ޒf�ʃR�[�h
		int hstfNum = GetFieldValueInteger(rs, _T("�����⍄�ޒi��"));			//�����⍄�ޒi��
		AtdDbGrdHstfLapItem atdDbGrdHstfLapItem;
		atdDbGrdHstfLapItem.setBuzaiCode(buzaiCode);
		atdDbGrdHstfLapItem.setHstfPos(hstfPos);
		atdDbGrdHstfLapItem.setHstfCode(hstfCode);
		atdDbGrdHstfLapItem.setHstfNum(hstfNum);
		this->append(atdDbGrdHstfLapItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbGrdHstfLap::getAt(int index, AtdDbGrdHstfLapItem& atdDbGrdHstfLapItem)
{
	if(_atdDbGrdHstfLapItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbGrdHstfLapItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbGrdHstfLapItem = _atdDbGrdHstfLapItemList[index];

	return JPT_OK;
}


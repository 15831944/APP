#include "stdafx.h"
#include "AtdDbSecGrd.h"

JptErrorStatus AtdDbSecGrd::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�f�ʥ�包�f��]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nogrd = GetFieldValueInteger(rs, _T("NOGRD"));	//����
		int nosec = GetFieldValueInteger(rs, _T("NOSEC"));	//�f�ʔԍ�
		double buf = GetFieldValueDouble(rs, _T("BUF"));	//UFLG_��
		double tuf = GetFieldValueDouble(rs, _T("TUF"));	//UFLG_��
		int muf = GetFieldValueInteger(rs, _T("MUF"));		//UFLG_�ގ�
		double blw = GetFieldValueDouble(rs, _T("TLW"));	//WEB_��
		int mlw = GetFieldValueInteger(rs, _T("MLW"));		//WEB_�ގ�
		double blf = GetFieldValueDouble(rs, _T("BLF"));	//LFLG_��
		double tlf = GetFieldValueDouble(rs, _T("TLF"));	//LFLG_��
		int mlf = GetFieldValueInteger(rs, _T("MLF"));		//LFLG_�ގ�
		AtdDbSecGrdItem atdDbSecGrdItem;
		atdDbSecGrdItem.setNogrd(nogrd);
		atdDbSecGrdItem.setNosec(nosec);
		atdDbSecGrdItem.setBuf(buf);
		atdDbSecGrdItem.setTuf(tuf);
		atdDbSecGrdItem.setMuf(muf);
		atdDbSecGrdItem.setBlw(blw);
		atdDbSecGrdItem.setMlw(mlw);
		atdDbSecGrdItem.setBlf(blf);
		atdDbSecGrdItem.setTlf(tlf);
		atdDbSecGrdItem.setMlf(mlf);
		this->append(atdDbSecGrdItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecGrd::getAt(int index, AtdDbSecGrdItem& atdDbSecGrdItem)
{
	if(_atdDbSecGrdItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecGrdItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecGrdItem = _atdDbSecGrdItemList[index];

	return JPT_OK;
}


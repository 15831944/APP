#include "stdafx.h"
#include "AtdDbSecCbeamSec.h"

JptErrorStatus AtdDbSecCbeamSec::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�f�ʥ������f��]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int nocrs = GetFieldValueInteger(rs, _T("NOCRS"));	//NOCRS
		double bfcu = GetFieldValueDouble(rs, _T("BFCU"));	//UFLG_�� H�|�t�����W��
		double tfcu = GetFieldValueDouble(rs, _T("TFCU"));	//UFLG_�� H�|�t�����W��
		int mfcu = GetFieldValueInteger(rs, _T("MFCU"));	//UFLG_�ގ� H�|�ގ�
		double tfcw = GetFieldValueDouble(rs, _T("TFCW"));	//WEB_�� H�|�E�F�u��
		int mfcw = GetFieldValueInteger(rs, _T("MFCW"));	//WEB_�ގ�
		double bfcl = GetFieldValueDouble(rs, _T("BFCL"));	//LFLG_�� H�|�t�����W��
		double tfcl = GetFieldValueDouble(rs, _T("TFCL"));	//LFLG_�� H�|�t�����W��
		int mfcl = GetFieldValueInteger(rs, _T("MFCL"));	//LFLG_�ގ� H�|�ގ�
		AtdDbSecCbeamSecItem atdDbSecCbeamSecItem;
		atdDbSecCbeamSecItem.setNocrs(nocrs);
		atdDbSecCbeamSecItem.setBfcu(bfcu);
		atdDbSecCbeamSecItem.setTfcu(tfcu);
		atdDbSecCbeamSecItem.setMfcu(mfcu);
		atdDbSecCbeamSecItem.setTfcw(tfcw);
		atdDbSecCbeamSecItem.setMfcw(mfcw);
		atdDbSecCbeamSecItem.setBfcl(bfcl);
		atdDbSecCbeamSecItem.setTfcl(tfcl);
		atdDbSecCbeamSecItem.setMfcl(mfcl);
		this->append(atdDbSecCbeamSecItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecCbeamSec::getAt(int index, AtdDbSecCbeamSecItem& atdDbSecCbeamSecItem)
{
	if(_atdDbSecCbeamSecItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecCbeamSecItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecCbeamSecItem = _atdDbSecCbeamSecItemList[index];

	return JPT_OK;
}


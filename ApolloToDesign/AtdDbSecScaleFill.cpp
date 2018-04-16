#include "stdafx.h"
#include "AtdDbSecScaleFill.h"

JptErrorStatus AtdDbSecScaleFill::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��t�B���[�v���[�g]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int mtfill = GetFieldValueInteger(rs, _T("MTFILL"));	//Fill�ގ�
		double rfilfu = GetFieldValueDouble(rs, _T("RFILFU"));	//
		double rfilfl = GetFieldValueDouble(rs, _T("RFILFL"));	//
		double rfilwb = GetFieldValueDouble(rs, _T("RFILWB"));	//
		AtdDbSecScaleFillItem atdDbSecScaleFillItem;
		atdDbSecScaleFillItem.setMtfill(mtfill);
		atdDbSecScaleFillItem.setRfilfu(rfilfu);
		atdDbSecScaleFillItem.setRfilfl(rfilfl);
		atdDbSecScaleFillItem.setRfilwb(rfilwb);
		this->append(atdDbSecScaleFillItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSecScaleFill::getAt(int index, AtdDbSecScaleFillItem& atdDbSecScaleFillItem)
{
	if(_atdDbSecScaleFillItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSecScaleFillItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSecScaleFillItem = _atdDbSecScaleFillItemList[index];

	return JPT_OK;
}


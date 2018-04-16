// 2018/02/26 take Add Start
#include "stdafx.h"
#include "AtdDbBaseData.h"

JptErrorStatus AtdDbBaseData::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�����݌v�]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int brdgType = GetFieldValueInteger(rs, _T("�����`��"));
		int crossType = GetFieldValueInteger(rs, _T("���g�\���`��"));
		AtdDbBaseDataItem atdDbBaseDataItem;
		atdDbBaseDataItem.setBrdgType(brdgType);
		atdDbBaseDataItem.setCrossType(crossType);
		this->append(atdDbBaseDataItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbBaseData::getAt(int index, AtdDbBaseDataItem& atdDbBaseDataItem)
{
	if(_atdDbBaseDataItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbBaseDataItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbBaseDataItem = _atdDbBaseDataItemList[index];

	return JPT_OK;
}

JptErrorStatus AtdDbBaseData::judgeBrdgType( bool &brdgTypeFlg )
{
	AtdDbBaseDataItem atdDbBaseDataItem;
	if(this->getAt(0, atdDbBaseDataItem) != JPT_OK ){
		return JPT_ERROR;
	}

	int brdgType = atdDbBaseDataItem.getBrdgType();
	int crossType = atdDbBaseDataItem.getCrossType();
	if( brdgType == 1 && crossType == 1 ){
		brdgTypeFlg = true;
	}
	return JPT_OK;
}
// 2018/02/26 take Add End
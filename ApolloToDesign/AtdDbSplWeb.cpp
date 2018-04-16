#include "stdafx.h"
#include "AtdDbSplWeb.h"

JptErrorStatus AtdDbSplWeb::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�Y�ڥ�e�Y�ڥ�E�F�u]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 3 || strcode == 4) {
			int pos = GetFieldValueInteger(rs, _T("POS"));	//
			int nowj = GetFieldValueInteger(rs, _T("NOWJ"));	//NOWJ �W���C���g�ԍ�
			int itpwj = GetFieldValueInteger(rs, _T("ITPWJ"));	//
			double gwj = GetFieldValueDouble(rs, _T("GWJ"));	//P
			int ng2wj = GetFieldValueInteger(rs, _T("NG2WJ"));	//P�̐�
			double gowj = GetFieldValueDouble(rs, _T("GOWJ"));	//CP
			double p2wj = GetFieldValueDouble(rs, _T("P2WJ"));	//�㑤G G
			int nd2wj = GetFieldValueInteger(rs, _T("ND2WJ"));	//�㑤G�̐�
			double p3wj = GetFieldValueDouble(rs, _T("P3WJ"));	//����G
			int nd3wj = GetFieldValueInteger(rs, _T("ND3WJ"));	//����G�̐�
			double p0wj = GetFieldValueDouble(rs, _T("P0WJ"));	//�㑤�󂫗�
			double p00j = GetFieldValueDouble(rs, _T("P00J"));	//�����󂫗�
			double t2wj = GetFieldValueDouble(rs, _T("T2WJ"));	//SPL�� SPL_��
			int mtwj = GetFieldValueInteger(rs, _T("MTWJ"));	//SPL�ގ� SPL_�ގ�
			AtdDbSplWebItem atdDbSplWeb;
			atdDbSplWeb.setStrcode(strcode);
			atdDbSplWeb.setPos(pos);
			atdDbSplWeb.setNowj(nowj);
			atdDbSplWeb.setItpwj(itpwj);
			atdDbSplWeb.setGwj(gwj);
			atdDbSplWeb.setNg2wj(ng2wj);
			atdDbSplWeb.setGowj(gowj);
			atdDbSplWeb.setP2wj(p2wj);
			atdDbSplWeb.setNd2wj(nd2wj);
			atdDbSplWeb.setP3wj(p3wj);
			atdDbSplWeb.setNd3wj(nd3wj);
			atdDbSplWeb.setP0wj(p0wj);
			atdDbSplWeb.setP00j(p00j);
			atdDbSplWeb.setT2wj(t2wj);
			atdDbSplWeb.setMtwj(mtwj);
			this->append(atdDbSplWeb);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSplWeb::getAt(int index, AtdDbSplWebItem& atdDbSplWebItem)
{
	if(_atdDbSplWebItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSplWebItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSplWebItem = _atdDbSplWebItemList[index];

	return JPT_OK;
}

int AtdDbSplWeb::findItpwj(const int& nocj)
{
	int itpwj = -1;
	for(int i=0;i<this->size();i++) {
		AtdDbSplWebItem swbItem;
		getAt(i, swbItem);
		int strcode = swbItem.getStrcode();	//STRCODE
		if(strcode != 4) {
			continue;
		}
		int nowj = swbItem.getNowj();	//NOWJ �W���C���g�ԍ�
		if(nowj == nocj) {
			itpwj = swbItem.getItpwj();	//ITPWJ
			break;
		}
	}

	return itpwj;
}


#include "stdafx.h"
#include "AtdDbSplFlg.h"

JptErrorStatus AtdDbSplFlg::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�Y�ڥ�e�Y�ڥ�t�����W]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int strcode = GetFieldValueInteger(rs, _T("STRCODE"));	//
		if(strcode == 3 || strcode == 4) {
			int pos = GetFieldValueInteger(rs, _T("POS"));	//
			int iupdw = GetFieldValueInteger(rs, _T("IUPDW"));	//IUPDW �㉺
			int noj = GetFieldValueInteger(rs, _T("NOJ"));		//NOJ �W���C���g�ԍ�
			int ifmj = GetFieldValueInteger(rs, _T("IFMJ"));	//�E�^�C�v FLG�E_�^�C�v
			double poj = GetFieldValueDouble(rs, _T("POJ"));	//CP
			double pj = GetFieldValueDouble(rs, _T("PJ"));		//P1 P FLG�E_P
			int npj = GetFieldValueInteger(rs, _T("NPJ"));		//P1�̐�
			double p00j = GetFieldValueDouble(rs, _T("P00J"));	//P2
			int npoj = GetFieldValueInteger(rs, _T("NPOJ"));	//P2�̐�
			double goj = GetFieldValueDouble(rs, _T("GOJ"));	//CG FLG�E_CG
			double gj = GetFieldValueDouble(rs, _T("GJ"));		//G FLG�E_G
			// 2018/02/15 take Edit Start
			// �f�[�^�^��Double
			double ngj = GetFieldValueDouble(rs, _T("NGJ"));
//			int ngj = GetFieldValueInteger(rs, _T("NGJ"));		//G�̐�
			// 2018/02/15 take Edit End
			int b1j = GetFieldValueInteger(rs, _T("B1J"));		//SPL�^�C�v SPL_�^�C�v
			double t1j = GetFieldValueDouble(rs, _T("T1J"));	//NS�� SPL_���i�O���j SPL_���i�����j
			double t2j = GetFieldValueDouble(rs, _T("T2J"));	//FS�� CONN_��
			int mtj = GetFieldValueInteger(rs, _T("MTJ"));		//NS�ގ� FS�ގ� SPL_�ގ��i�O���j SPL_�ގ��i�����j
			double gjc = GetFieldValueDouble(rs, _T("GJC"));	//CONN�E_���@X
			int ngjc = GetFieldValueInteger(rs, _T("NGJC"));	//CONN�E_���@X�̐�
			double pjc = GetFieldValueDouble(rs, _T("PJC"));	//CONN�E_���@Y
			int npjc = GetFieldValueInteger(rs, _T("NPJC"));	//CONN�E_���@Y�̐�
			AtdDbSplFlgItem atdDbSplFlg;
			atdDbSplFlg.setStrcode(strcode);
			atdDbSplFlg.setPos(pos);
			atdDbSplFlg.setIupdw(iupdw);
			atdDbSplFlg.setNoj(noj);
			atdDbSplFlg.setIfmj(ifmj);
			atdDbSplFlg.setPoj(poj);
			atdDbSplFlg.setPj(pj);
			atdDbSplFlg.setNpj(npj);
			atdDbSplFlg.setP00j(p00j);
			atdDbSplFlg.setNpoj(npoj);
			atdDbSplFlg.setGoj(goj);
			atdDbSplFlg.setGj(gj);
			// 208/02/15 take Edit Start
			// Double �� int
			atdDbSplFlg.setNgj((int)ngj);
			//atdDbSplFlg.setNgj(ngj);
			// 208/02/15 take Edit End
			atdDbSplFlg.setB1j(b1j);
			atdDbSplFlg.setT1j(t1j);
			atdDbSplFlg.setT2j(t2j);
			atdDbSplFlg.setMtj(mtj);
			atdDbSplFlg.setGjc(gjc);
			atdDbSplFlg.setNgjc(ngjc);
			atdDbSplFlg.setPjc(pjc);
			atdDbSplFlg.setNpjc(npjc);
			this->append(atdDbSplFlg);
		}
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbSplFlg::getAt(int index, AtdDbSplFlgItem& atdDbSplFlgItem)
{
	if(_atdDbSplFlgItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbSplFlgItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbSplFlgItem = _atdDbSplFlgItemList[index];

	return JPT_OK;
}


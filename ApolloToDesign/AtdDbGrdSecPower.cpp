#include "stdafx.h"
#include "AtdDbGrdSecPower.h"

JptErrorStatus AtdDbGrdSecPower::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�包�f�ʗ̓f�[�^]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int buzaiCode = GetFieldValueInteger(rs, _T("���ސ��R�[�h"));	//���ސ��R�[�h
		int secCode = GetFieldValueInteger(rs, _T("�f�ʃR�[�h"));	//�f�ʃR�[�h
		int shosaPosNo = GetFieldValueInteger(rs, _T("�ƍ��ʒu�ԍ�"));	//�ƍ��ʒu�ԍ�
		int secCaseNo = GetFieldValueInteger(rs, _T("�f�ʗ̓P�[�X�ԍ�"));	//�f�ʗ̓P�[�X�ԍ�
		double mageMx = GetFieldValueDouble(rs, _T("�Ȃ����[�����g�i�ʓ��j�l��"));	//�Ȃ����[�����g�i�ʓ��j�l��
		AtdDbGrdSecPowerItem atdDbGrdSecPowerItem;
		atdDbGrdSecPowerItem.setBuzaiCode(buzaiCode);
		atdDbGrdSecPowerItem.setSecCode(secCode);
		atdDbGrdSecPowerItem.ShosaPosNo(shosaPosNo);
		atdDbGrdSecPowerItem.SecCaseNo(secCaseNo);
		atdDbGrdSecPowerItem.setMageMx(mageMx);
		this->append(atdDbGrdSecPowerItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbGrdSecPower::getAt(int index, AtdDbGrdSecPowerItem& atdDbGrdSecPowerItem)
{
	if(_atdDbGrdSecPowerItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbGrdSecPowerItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbGrdSecPowerItem = _atdDbGrdSecPowerItemList[index];

	return JPT_OK;
}

double AtdDbGrdSecPower::findMageMx(const int& shosaPosNo, const int& secCaseNo)
{
	double mageMx = 0.0;
	for(int i=0;i<this->size();i++) {
		AtdDbGrdSecPowerItem gspItem;
		getAt(i, gspItem);
		int shosaPosNo2 = gspItem.getShosaPosNo();	//�ƍ��ʒu�ԍ�
		int secCaseNo2 = gspItem.getSecCaseNo();	//�f�ʗ̓P�[�X�ԍ�
		if(shosaPosNo2 == shosaPosNo && secCaseNo2 == secCaseNo) {
			mageMx = gspItem.getMageMx();	//�Ȃ����[�����g�i�ʓ��j�l��
			break;
		}
	}

	return mageMx;
}


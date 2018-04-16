// 2018/03/01 take Add Start
#include "stdafx.h"
#include "AtdDbGrdPower.h"

JptErrorStatus AtdDbGrdPower::load(CDaoDatabase& dbFile)
{
	CDaoRecordset rs(&dbFile);
	CString strquery = "select * from [�包���͓x�f�[�^]";
	rs.Open(dbOpenSnapshot, strquery, dbReadOnly);
	while(!rs.IsEOF()) {
		int buzaiCode = GetFieldValueInteger(rs, _T("���ސ��R�[�h"));	//���ސ��R�[�h
		int secCode = GetFieldValueInteger(rs, _T("�f�ʃR�[�h"));		//�f�ʃR�[�h
		int shosaPosNo = GetFieldValueInteger(rs, _T("�ƍ��ʒu�ԍ�"));	//�ƍ��ʒu�ԍ�
		int secCaseNo = GetFieldValueInteger(rs, _T("�f�ʗ̓P�[�X�ԍ�"));//�f�ʗ̓P�[�X�ԍ�
		int shosaPoiNo = GetFieldValueInteger(rs, _T("�ƍ��_�ԍ�(�f�ʓ�)"));	//�ƍ��_�ԍ�
		double forceMx = GetFieldValueDouble(rs, _T("�l���ɑ΂��鉞�͓x"));//�l���ɑ΂��鉞�͓x

		AtdDbGrdPowerItem atdDbGrdPowerItem;
		atdDbGrdPowerItem.setBuzaiCode(buzaiCode);
		atdDbGrdPowerItem.setSecCode(secCode);
		atdDbGrdPowerItem.setShosaPosNo(shosaPosNo);
		atdDbGrdPowerItem.setSecCaseNo(secCaseNo);
		atdDbGrdPowerItem.setShosaPoiNo(shosaPoiNo);
		atdDbGrdPowerItem.setForceMx(forceMx);
		this->append(atdDbGrdPowerItem);
		rs.MoveNext();
	}
	rs.Close();

	return JPT_OK;
}

JptErrorStatus AtdDbGrdPower::getAt(int index, AtdDbGrdPowerItem& atdDbGrdPowerItem)
{
	if(_atdDbGrdPowerItemList.empty()){
		return JPT_ERROR;
	}
	int tempSize = _atdDbGrdPowerItemList.size();
	if(index < 0 || index > tempSize){
		return JPT_ERROR;
	}
	atdDbGrdPowerItem = _atdDbGrdPowerItemList[index];

	return JPT_OK;
}

double AtdDbGrdPower::findForceMx(const int& grdNo, const int& panelNo)
{
	double forceMx = 0.0;
	for(int i=0;i<this->size();i++) {
		AtdDbGrdPowerItem gpItem;
		getAt(i, gpItem);
		int buzaiCode = gpItem.getBuzaiCode();		//���ސ��R�[�h
		int secNo = gpItem.getSecCode() % 1000;		//�f�ʔԍ�
		if( grdNo == buzaiCode && panelNo == secNo ){	//���ԍ��ƃp�l������v���邩
			int shosaPosNo = gpItem.getShosaPosNo();	//�ƍ��ʒu�ԍ�
			int secCaseNo = gpItem.getSecCaseNo();		//�f�ʗ̓P�[�X�ԍ�
			int shosaPoiNo = gpItem.getShosaPoiNo();	//�ƍ��_�ԍ�
			if( shosaPosNo == 3 && secCaseNo == 1 && shosaPoiNo == 4 ){	//�ƍ��ʒu�ԍ�(=������t�_)�A�f�ʗ̓P�[�X�ԍ�(�ő厞)�A�ƍ��_�ԍ�(=���t�����W��)
				forceMx = gpItem.getForceMx();			//�l���ɑ΂��鉞�͓x
				break;
			}
		}
	}
	return forceMx;
}
// 2018/03/01 take Add End
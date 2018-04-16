#include "stdafx.h"
#include "AtdDbToCsv.h"
#include "AlnLinearCoordinateFile\\AlnLinearCoordinateFile.h"
#include <set>

//�Q�D��{�`��

//(�P)�i�_���W
JptErrorStatus AtdDbToCsv::dbToCsvSklKakuten(
	string& sekkeiFilePath,
	AtdDbSecGrdKaku& sgk,	//�f�ʥ�包�i�_��
	AtdDbLineGrdZahyo& lgz,	//���`��包���W(�c��/���c��)
	AtdDbInputGrdMen& igm,	//���ͥ�包���ʌ`��
	std::ofstream& ofOb)
{
	StringTokenizer strToken(sekkeiFilePath,"\\.");
	AlnLinearCoordinateFile alcf;
	if(strToken.size2() > 1) {
		string koji = strToken[strToken.size2()-2];
		string alignPath = "Align\\" + koji + ".sug";
		alcf.load(alignPath);
		if(alcf.size() < 1) {
			alignPath = "Align\\" + koji + "01.sug";
			alcf.load(alignPath);
		}
	}
	StringHandler sh;
	DiafSklKakuten dsk;
	_ngMax = 0;
	_npMax = 0;
	for(int i=0;i<lgz.size();i++) {
		AtdDbLineGrdZahyoItem lgzItem;
		lgz.getAt(i, lgzItem);
		int nostr = lgzItem.getNostr();		//NOSTR
		int nopt = lgzItem.getNopt();		//NOPT
		// 2018/02/15 take Edit Start
		// mm �� m
		double xu = lgzItem.getXu() / 1000;
		double yu = lgzItem.getYu() / 1000;
		double zu = lgzItem.getZu() / 1000;
		//double xu = lgzItem.getXu();		//XU
		//double yu = lgzItem.getYu();		//YU
		//double zu = lgzItem.getZu();		//ZU
		// 2018/02/15 take Edit End
		EnHokan heimenHokan = S_TYOKUSEN;			//���ʂ̕�ԕ��@
		EnHokan sokumenHokan = A_SANJIKYOKUSEN;		//���ʂ̕�ԕ��@
		for(int j=0;j<igm.size();j++) {
			AtdDbInputGrdMenItem igmItem;
			igm.getAt(j, igmItem);
			int strno = igmItem.getStrno();		//STRNO ����
			if(nostr != strno) {
				continue;
			}
			int itpuw = igmItem.getItpuw();		//ITPUW ���ʕ�ԕ��@
			if(itpuw != 3) {
				sokumenHokan = S_TYOKUSEN;
			}
		}
		if(nostr > _ngMax) {
			_ngMax = nostr;
		}
		if(nopt > _npMax) {
			_npMax = nopt;
		}
		string odname = "";
		if(nopt == 1) {
			odname = "GE1";
		} else {
			for(int j=0;j<sgk.size();j++) {
				AtdDbSecGrdKakuItem atdDbSecGrdKakuItem;
				sgk.getAt(j, atdDbSecGrdKakuItem);
				int nostr2 = atdDbSecGrdKakuItem.getNostr();
				int nocrs2 = atdDbSecGrdKakuItem.getNocrs();
				if(nostr == nostr2 && nopt-1 == nocrs2) {
					odname = atdDbSecGrdKakuItem.getOdname();
					break;
				}
			}
			if(odname == "") {
				odname = "GE2";
			}
		}
		string ketaName = "G" + sh.toString(nostr);	//����
		for(int k=0;k<alcf.size();k++) {
			AlnLinearCoordinateData lcd;
			alcf.getAt(k, lcd);
			string lineName = lcd.getLineName();
			string lineName2 = lineName;
			if(lineName2.size() == ketaName.size()+1) {
				lineName2.erase(lineName2.begin() + 1);
			}
			if(lineName == ketaName || lineName2 == ketaName) {
				for(int l=0;l<lcd.size();l++) {
					AlnPointData point;
					lcd.getAt(l, point);
					string pointName = point.getPointName();
					if(odname == pointName) {
						double radius = point.getRadius();
						if(fabs(radius) < 0.00001) {
							heimenHokan = S_TYOKUSEN;		//����
						} else {
							heimenHokan = A_SANJIKYOKUSEN;	//�O���Ȑ�(�X�v���C��)
						}
						break;
					}
				}
			}
		}
		DiafSklKakutenItem dskItem;
		dskItem.setKetaName(ketaName);
		dskItem.setKakutenNo(nopt);
		dskItem.setXZahyou(xu);
		dskItem.setYZahyou(yu);
		dskItem.setWebzZahyou(zu);
		dskItem.setHeimenHokan(heimenHokan);
		dskItem.setSokumenHokan(sokumenHokan);
		dsk.append(dskItem);
	}
	dsk.sort();
	dsk.toCsv(ofOb);
	//����
	for(int i=0;i<_ngMax;i++) {
		string ketaName = "G" + sh.toString(i+1);
		_ketaNameameList.push_back(ketaName);
	}

	return JPT_OK;
}

//(�Q)���f����`
// 2018/02/27 take Edit Start
//JptErrorStatus AtdDbToCsv::dbToCsvSklOudan(
//	AtdDbSecGrdKaku& sgk,	//�f�ʥ�包�i�_��
//	AtdDbLineGrdZahyo& lgz,	//���`��包���W(�c��/���c��)
//	std::ofstream& ofOb)
JptErrorStatus AtdDbToCsv::dbToCsvSklOudan(
	AtdDbStructAll& sta,	//�\���E�S��
	AtdDbSecGrdKaku& sgk,	//�f�ʥ�包�i�_��
	AtdDbLineGrdZahyo& lgz,	//���`��包���W(�c��/���c��)
	std::ofstream& ofOb)
// 2018/02/26 take Edit End
{
	StringHandler sh;
	DiafSklOudan dso;

	// 2018/02/27 take Add Start
	int nSpanCount = 0;
	AtdDbStructAllItem staItem;	//�a�Ԑ�
	sta.getAt(0, staItem);
	_nSpan = staItem.getNspan();
	// 2018/02/27 take Add End
	for(int i=0;i<_npMax;i++) {
		int itmbX = -1;
		vector<int> itmbMemberList;				//ITMB�\������
		for(int j=0;j<lgz.size();j++) {
			AtdDbLineGrdZahyoItem lgzItem;
			lgz.getAt(j, lgzItem);
			int nopt = lgzItem.getNopt();		//NOPT
			int itmb = lgzItem.getItmb();		//ITMB
			// 2018/02/28 ITMB����\�������擾
			devideItmb( itmb, itmbMemberList );
			if(nopt == i+1) {
				itmbX = itmb;
				// 2018/02/28 take Add Start
				if(existConfigurationItem( 2, itmbMemberList ) == true){
					nSpanCount += 1;	//�x�_�����J�E���g
				}
				// 2018/02/28 take Add End
				break;
			}
			// 2018/02/28 take Add Start
			itmbMemberList.clear();
			// 2018/02/28 take Add End
		}
		if(itmbX == -1) {
//err
		}
		CString odanName = "";
		if(i == 0) {
			odanName = "GE1";
		} else if(i == _npMax-1) {
			odanName = "GE2";
		} else {
			for(int j=0;j<sgk.size();j++) {
				AtdDbSecGrdKakuItem sgkItem;
				sgk.getAt(j, sgkItem);
				int nocrs = sgkItem.getNocrs();		//NOCRS
				CString odname = sgkItem.getOdname();	//ODNAME ���f����
				if(nocrs == i) {
					odanName = odname;
					break;
				}
			}
			if(odanName == "") {
				//err
			}
			// 2018/02/27 take Edit Start
			//if(itmbX == 6) {	//�[�x�_
			//	_oudanType.push_back(0);
			//} else if(itmbX == 2) {	//���Ԏx�_
			//	_oudanType.push_back(1);
			//} else if(itmbX == 4) {	//�i�_
			//	_oudanType.push_back(2);
			//}
			if(existConfigurationItem( 2, itmbMemberList ) == true){
				if( nSpanCount == 1 || nSpanCount - 1 == _nSpan ){	//�[�x�_
					_oudanType.push_back(0);
				}else{	//���Ԏx�_
					_oudanType.push_back(1);
				}
			// 2018/02/27 take Edit End
			} else if(existConfigurationItem( 4, itmbMemberList ) == true) {	//�i�_
				_oudanType.push_back(2);
			}
			_oudanNameList.push_back(odanName.GetBuffer());
		}
		_oudanNameWithKetatanList.push_back(odanName.GetBuffer());
		string oudanLineName = odanName.GetBuffer();	//���f����
		EnOudanLineType oudanLineType;		//���f�����
		// 2018/02/28 take Edit Start
		//if(itmbX == 128) {		//���[
		//	oudanLineType = KETATAN;
		//} else if(itmbX == 6) {	//�[�x�_
		//	oudanLineType = TANSHITEN;
		//	_shitenKakutenNo.push_back(i);
		//	_shitenNameList.push_back(odanName.GetBuffer());
		//} else if(itmbX == 2) {	//���Ԏx�_
		//	oudanLineType = TYUUKANSHITEN;
		//	_shitenKakutenNo.push_back(i);
		//	_shitenNameList.push_back(odanName.GetBuffer());
		//} else if(itmbX == 4) {	//�i�_
		//	oudanLineType = KAKUTEN;
		//} else {
		////err
		//}
		if(existConfigurationItem( 128, itmbMemberList ) == true) {		//���[
			oudanLineType = KETATAN;
		}else if(existConfigurationItem( 2, itmbMemberList ) == true){
			if( nSpanCount == 1 || nSpanCount - 1 == _nSpan ){			//�[�x�_
				oudanLineType = TANSHITEN;
			}else{														//���Ԏx�_
				oudanLineType = TYUUKANSHITEN;
			}
			_shitenKakutenNo.push_back(i);
			_shitenNameList.push_back(odanName.GetBuffer());
		
		} else if(existConfigurationItem( 4, itmbMemberList ) == true) {	//�i�_
			oudanLineType = KAKUTEN;
		} else {
//err
		}
		// 2018/02/28 take Edit End
		DiafSklOudanItem dsoItem;
		dsoItem.setOudanLineName(oudanLineName);
		dsoItem.setOudanLineType(oudanLineType);
		int kakutenNo = i+1;			//�i�_�ԍ�
		for(int n=0;n<_ngMax;n++) {
			string ketaName = _ketaNameameList[n];	//����
			DiafSklOudanData sklOudanData;
			sklOudanData.setKetaName(ketaName);
			sklOudanData.setKakutenNo(kakutenNo);
			dsoItem.append(sklOudanData);
		}
		dso.append(dsoItem);
	}
	dso.toCsv(ofOb);

	return JPT_OK;
}

//(�R)�L�����o�[
JptErrorStatus AtdDbToCsv::dbToCsvSklCamber(
	AtdDbSecGrdKaku& sgk,		//�f�ʥ�包�i�_��
	AtdDbLineGrdCamber& lgc,	//���`��包(����/�u���P�b�g)��L�����o�[�l
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafSklCamber dsc;
	for(int i=0;i<sgk.size();i++) {
		AtdDbSecGrdKakuItem sgkItem;
		sgk.getAt(i, sgkItem);
		int nostr = sgkItem.getNostr();		//NOSTR
		int nocrs = sgkItem.getNocrs();		//NOCRS
		CString getOdname = sgkItem.getOdname();	//ODNAME ���f����
		double zCamber = 0.0;			//�i�_��Z�L�����o�[
		for(int j=0;j<lgc.size();j++) {
			AtdDbLineGrdCamberItem lgcItem;
			lgc.getAt(j, lgcItem);
			int nostr2 = lgcItem.getNostr();		//NOSTR ����
			int nopnl2 = lgcItem.getNopnl();		//NOPNL
			if(nostr == nostr2 && nocrs == nopnl2) {
				zCamber = lgcItem.getZcamz();	//ZCAMZ Z�L�����o�[
				break;
			}
		}
		DiafSklCamberItem dscItem;
		string ketaName = _ketaNameameList[nostr-1];	//����
		string kakutenName = getOdname;			//�i�_��
		int kakutenNo = nocrs;					//�i�_�ԍ�
		dscItem.setKetaName(ketaName);
		dscItem.setKakutenName(kakutenName);
		dscItem.setKakutenNo(kakutenNo);
		dscItem.setZCamber(zCamber);
		dsc.append(dscItem);
	}
	dsc.sort();
	dsc.toCsv(ofOb);

	return JPT_OK;
}

//(�S)�����⍄�ވʒu
// 2018/02/28 take Edit Start
// �g�p����e�[�u����ύX�i�����̂݁j
//JptErrorStatus AtdDbToCsv::dbToCsvSklVstf(
//	AtdDbGrdVstfHaichi& gvh,	//�包���Ԑ����⍄�ޔz�u�f�[�^
//	AtdDbGrdVstfKyori& gvk,		//�包���Ԑ����⍄�ފԊu�f�[�^
//	std::ofstream& ofOb)
//{
//	StringHandler sh;
//	DiafSklVstf dsv;
//	for(int i=0;i<_ngMax;i++) {
//		for(int j=0;j<_npMax-1;j++) {
//			int numvstf = 0;
//			for(int k=0;k<gvh.size();k++) {
//				AtdDbGrdVstfHaichiItem gvhItem;
//				gvh.getAt(k, gvhItem);
//				int nostr = gvhItem.getNostr();		//���ސ��R�[�h
//				int nopanel = gvhItem.getNopanel();	//�p�l���R�[�h
//				if(nostr != i+1) {
//					continue;
//				}
//				int pno = nopanel % 1000;	//�p�l���ԍ�
//				if(pno != j+1) {
//					continue;
//				}
//				numvstf = gvhItem.getNumvstf();	//�����⍄�ޖ{��
//				break;
//			}
//			if(numvstf < 1) {
//				continue;
//			}
//			// 2018/02/26 take Add Start
//			bool equalIntervalsFlg = true;		// �i�_�Ԃ�VSTF�����Ԋu�z�u����邩�itrue�F���Ԋu,false�F�s���Ԋu�j
//			// 2018/02/26 take Add End
//			double interval[10];
//			for(int l=0;l<10;l++) {
//				interval[l] = 0.0;	//����1-10
//			}
//			for(int k=0;k<gvk.size();k++) {
//				AtdDbGrdVstfKyoriItem gvkItem;
//				gvk.getAt(k, gvkItem);
//				int nostr = gvkItem.getNostr();			//���ސ��R�[�h
//				int nopanel = gvkItem.getNopanel();		//�p�l���R�[�h
//				if(nostr != i+1) {
//					continue;
//				}
//				int pno = nopanel % 1000;	//�p�l���ԍ�
//				if(pno != j+1) {
//					continue;
//				}
//				int vstfNum = gvkItem.getVstfNum();		//�����⍄�ފԊu�ԍ�
//				double vstfkyori = gvkItem.getVstfkyori();	//�����⍄�ފԊu
//				interval[vstfNum-1] = vstfkyori;
//				// 2018/02/26 take Add Start
//				if( equalIntervalsFlg == true && vstfNum > 1){
//					if( interval[vstfNum-2] != interval[vstfNum-1] ){
//						equalIntervalsFlg = false;
//					}
//				}
//				// 2018/02/26 take Add End
//			}
//			string ketaName = _ketaNameameList[i];				//����
//			string startSideKakutenName = _oudanNameList[j];	//�n���i�_��
//			string endSideKakutenName = _oudanNameList[j+1];	//�I���i�_��
//			int startSideKakutenNo = j+1;			//�n���i�_�ԍ�
//			int endSideKakutenNo = j+2;				//�I���i�_�ԍ�
//			int panelNo = j+1;						//�p�l���ԍ�
//			int bunkatsuNum = numvstf + 1;		//������
//			// 2018/2/26 take Edit Start
//			/*for(int l=0;l<bunkatsuNum;l++) {
//				if(interval[l] > 1.0) {
//					bunkatsuNum = -1;
//					break;
//				}
//			}*/
//			if(equalIntervalsFlg == true){	// VSTF�̔z�u�����Ԋu�̏ꍇ
//				for(int l=0;l<10;l++) {
//					interval[l] = -1.0;		
//				}
//			}else{							// VSTF�̔z�u���s���Ԋu�̏ꍇ
//				bunkatsuNum = -1;
//			}
//			// 2018/2/26 take Edit End
//			DiafSklVstfItem dsvItem;
//			dsvItem.setKetaName(ketaName);
//			dsvItem.setStartSideKakutenName(startSideKakutenName);
//			dsvItem.setEndSideKakutenName(endSideKakutenName);
//			dsvItem.setStartSideKakutenNo(startSideKakutenNo);
//			dsvItem.setEndSideKakutenNo(endSideKakutenNo);
//			dsvItem.setPanelNo(panelNo);
//			dsvItem.setBunkatsuNum(bunkatsuNum);
//			dsvItem.setInterval1(interval[0]);
//			dsvItem.setInterval2(interval[1]);
//			dsvItem.setInterval3(interval[2]);
//			dsvItem.setInterval4(interval[3]);
//			dsvItem.setInterval5(interval[4]);
//			dsvItem.setInterval6(interval[5]);
//			dsvItem.setInterval7(interval[6]);
//			dsvItem.setInterval8(interval[7]);
//			dsvItem.setInterval9(interval[8]);
//			dsvItem.setInterval10(interval[9]);
//			dsv.append(dsvItem);
//		}
//	}
//	dsv.sort();
//	dsv.toCsv(ofOb);
//
//	return JPT_OK;
//}
JptErrorStatus AtdDbToCsv::dbToCsvSklVstf(
	AtdDbSecVstfHaichi& svh,	//�f�ʥVSTF�z�u
	AtdDbLineGrdPanel& lgp,		//���`��包(����/�u���P�b�g)��p�l����
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafSklVstf dsv;
	// 2018/02/28 take Add Start
	_midVstfLengAdd.resize( _ngMax );
	_hstfLengAdd.resize( _ngMax );
	_hstfPanelNumber.resize( _ngMax );
	// 2018/02/28 take Add End
	for(int i=0;i<_ngMax;i++) {
		// 2018/02/28 take Add Start
		double vstfLocation = 0.0;	//VSTF�̈ʒu
		// 2018/02/28 take Add End
		for(int j=0;j<_npMax-1;j++) {
			int numvstf = 0;
			for(int k=0;k<svh.size();k++) {
				AtdDbSecVstfHaichiItem svhItem;
				svh.getAt(k, svhItem);
				int nostr = svhItem.getNogrd();		//���ސ��R�[�h
				int pno = svhItem.getNopnl();		//�p�l���R�[�h
				if(nostr != i+1) {
					continue;
				}
				if(pno != j+1) {
					continue;
				}
				numvstf = svhItem.getNvst();	//�����⍄�ޖ{��
				break;
			}
			if(numvstf < 1) {
				continue;
			}
			// 2018/02/28 take Add Start
			for(int k=0;k<lgp.size();k++) {		//VSTF�̔z�u�ʒu���擾�i�����⍄�ވʒu�Ŏg�p�j
				AtdDbLineGrdPanelItem lgpItem;
				lgp.getAt(k, lgpItem);
				int strcode = lgpItem.getStrcode();
				if( strcode != 3 ){
					continue;
				}
				int nostr = lgpItem.getNostr();		//�包�ԍ�
				int pno = lgpItem.getNopnl();		//�i�_�ԍ�
				if(nostr != i+1){
					continue;
				}
				if(pno != j+1) {
					continue;
				}
				//
				double vstfInterval = lgpItem.getRlp() / (numvstf + 1);		//�p�l���Ԃ�VSTF�z�u�Ԋu
				double halfVstfInterval = vstfInterval / 2;					//�z�u�Ԋu�̒��Ԉʒu(�����⍄�ނ̒��Ԉʒu)

				for(int l=0;l<numvstf;l++){
					vstfLocation += vstfInterval;
					_midVstfLengAdd[i].push_back(vstfLocation);
					_hstfLengAdd[i].push_back(vstfLocation - halfVstfInterval);
					_hstfPanelNumber[i].push_back(pno);
				}
				_hstfLengAdd[i].push_back(vstfLocation + halfVstfInterval);
				_hstfPanelNumber[i].push_back(pno);
				vstfLocation += vstfInterval;
			}
			// 2018/02/28 take Add End

			double interval[10];
			for(int l=0;l<10;l++) {
				interval[l] = -1.0;		//�p�l���Ԃ�VSTF�{���̂ݒ�`����Ă��邽�߁A���Ԋu
			}
			
			string ketaName = _ketaNameameList[i];				//����
			string startSideKakutenName = _oudanNameList[j];	//�n���i�_��
			string endSideKakutenName = _oudanNameList[j+1];	//�I���i�_��
			int startSideKakutenNo = j+1;			//�n���i�_�ԍ�
			int endSideKakutenNo = j+2;				//�I���i�_�ԍ�
			int panelNo = j+1;						//�p�l���ԍ�
			int bunkatsuNum = numvstf + 1;		//������
			
			DiafSklVstfItem dsvItem;
			dsvItem.setKetaName(ketaName);
			dsvItem.setStartSideKakutenName(startSideKakutenName);
			dsvItem.setEndSideKakutenName(endSideKakutenName);
			dsvItem.setStartSideKakutenNo(startSideKakutenNo);
			dsvItem.setEndSideKakutenNo(endSideKakutenNo);
			dsvItem.setPanelNo(panelNo);
			dsvItem.setBunkatsuNum(bunkatsuNum);
			dsvItem.setInterval1(interval[0]);
			dsvItem.setInterval2(interval[1]);
			dsvItem.setInterval3(interval[2]);
			dsvItem.setInterval4(interval[3]);
			dsvItem.setInterval5(interval[4]);
			dsvItem.setInterval6(interval[5]);
			dsvItem.setInterval7(interval[6]);
			dsvItem.setInterval8(interval[7]);
			dsvItem.setInterval9(interval[8]);
			dsvItem.setInterval10(interval[9]);
			dsv.append(dsvItem);
		}
	}
	dsv.sort();
	dsv.toCsv(ofOb);

	return JPT_OK;
}
// 2018/02/28take Edit End

//(�T)�W���C���g�ʒu
JptErrorStatus AtdDbToCsv::dbToCsvSklJoint(
	AtdDbSecGrdLeng& sgl,	//�f�ʥ�包�̒f�ʒ�
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafSklJoint dsj;
	_jointMax = 0;
	for(int i=0;i<sgl.size();i++) {
		AtdDbSecGrdLengItem sglItem;
		sgl.getAt(i, sglItem);
		int nosec = sglItem.getNosec();		//NOSEC �W���C���g��
		if(nosec > _jointMax) {
			_jointMax = nosec;
		}
	}
	for(int i=0;i<sgl.size();i++) {
		AtdDbSecGrdLengItem sglItem;
		sgl.getAt(i, sglItem);
		int nogrd = sglItem.getNogrd();		//NOGRD ����
		int nosec = sglItem.getNosec();		//NOSEC �W���C���g��
		if(_jointMax == nosec) {
			continue;
		}
		double rlsec = sglItem.getRlsec();	//RLSEC �u���b�N��
		string ketaName = _ketaNameameList[nogrd-1];		//����
		string jointName = "J" + sh.toString(nosec);		//�W���C���g��
		DiafSklJointItem dsjItem;
		dsjItem.setKetaName(ketaName);
		dsjItem.setJointName(jointName);
		dsjItem.setJointNo(nosec);
		dsjItem.setBlockLength(rlsec);
		dsj.append(dsjItem);
	}
	_jointMax--;
	dsj.sort();
	dsj.toCsv(ofOb);

	return JPT_OK;
}

//(�U)�包�E�F�u���[��
JptErrorStatus AtdDbToCsv::dbToCsvSklWebHeight(
	AtdDbInputGrdMen& igm,				//���ͥ�包���ʌ`��
	AtdDbSecGrdHeightConstant& sghc,	//�f�ʥ�包����(���E�������)
	AtdDbSecGrdHeightVariable& sghv,	//�f�ʥ�包����(�������S��)
	AtdDbLineGrdPanel& lgp,				//���`��包(����/�u���P�b�g)��p�l����
	AtdDbInputKetatanLeng& ikl,			//���ͥ���[��
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafSklWebHeight dswh;
	_kakutenLengAdd.resize(_ngMax);
	for(int i=0;i<_ngMax;i++) {
		// 2018/02/27 take Edit Start
		//_kakutenLengAdd[i].resize(_npMax);
		_kakutenLengAdd[i].resize(_npMax - 1);
		// 2018/02/27 take Edit End
		int itpww = 3;
		for(int k=0;k<igm.size();k++) {
			AtdDbInputGrdMenItem igmItem;
			igm.getAt(k, igmItem);
			int strno = igmItem.getStrno();		//STRNO ����
			if(strno != i+1) {
				continue;
			}
			itpww = igmItem.getItpww();		//ITPWW ���ʕ�ԕ��@
		}
		string ketaName = _ketaNameameList[i];			//����
		for(int j=0;j<_npMax-1;j++) {
			double rlp = 0.0;
			if(j == 0 || j == _npMax-2) {
				int idx = ikl.find(i+1);
				if(idx > -1) {
					AtdDbInputKetatanLengItem atdDbInputKetatanLengItem;
					ikl.getAt(idx, atdDbInputKetatanLengItem);
					if(j == 0) {
						rlp = atdDbInputKetatanLengItem.getRls();
					} else {
						rlp = atdDbInputKetatanLengItem.getRle();
					}
				}
			} else {
				rlp = lgp.findRlp(3, i+1, j);
			}
			_kakutenLengAdd[i][j] = rlp;
		}
		_kakutenLengAdd[i][1] -= _kakutenLengAdd[i][0];
		_kakutenLengAdd[i][_npMax-3] -= _kakutenLengAdd[i][_npMax-2];
		for(int j=1;j<_npMax-1;j++) {
			_kakutenLengAdd[i][j] += _kakutenLengAdd[i][j-1];
		}

		// 2018/02/27 take Add Start
		double startLocation = 0.0;					//�E�F�u�����ω�����ꍇ�Ɏg�p�i�p�l���Ԃ̈ʒu�֌W�������j
		double endLocation = _kakutenLengAdd[i][1];
		// 2018/02/27 take Add end
		for(int j=0;j<_npMax;j++) {
			string kakutenName = _oudanNameWithKetatanList[j];	//�i�_��
			int kakutenNo = j+1;						//�i�_�ԍ�
			double teigiPositionDim = 0.0;				//��`�ʒu���@
			double webHeight = 0.0;						//�E�F�u��
			EnHokan webHeightHokanMethod = S_TYOKUSEN;	//�E�F�u����ԕ��@
			if(itpww == 0 || itpww == 3) {			//�������
				for(int k=0;k<sghc.size();k++) {
					AtdDbSecGrdHeightConstantItem sghcItem;
					sghc.getAt(k, sghcItem);
					int nogrd = sghcItem.getNogrd();	//NOGRD
					if(nogrd == i+1) {
						webHeight = sghcItem.getHweb();	//HWEB �E�F�u��
						break;
					}
				}
				// 2018/02/26 take Add Start
				DiafSklWebHeightItem dswhItem;
				dswhItem.setKetaName(ketaName);
				dswhItem.setKakutenName(kakutenName);
				dswhItem.setKakutenNo(kakutenNo);
				dswhItem.setTeigiPositionDim(teigiPositionDim);
				dswhItem.setWebHeight(webHeight);
				dswhItem.setWebHeightHokanMethod(webHeightHokanMethod);
				dswh.append(dswhItem);
				// 2018/02/26 take Add End
			} else {	//�����ω�
				double rcweb = 0.0;
				int itplc = 0;
				for(int k=0;k<sghv.size();k++) {	//�f�ʥ�包����(�������S��)
					AtdDbSecGrdHeightVariableItem sghvItem;
					sghv.getAt(k, sghvItem);
					int nogrd = sghvItem.getNogrd();		//NOGRD
					if(nogrd != i+1) {
						continue;
					}
					rcweb = sghvItem.getRcweb();	//RCWEB
					webHeight = sghvItem.getHcweb();	//HCWEB
					itplc = sghvItem.getItplc();		//ITPLC
//					int itphc = sghvItem.getItphc();	//ITPHC

					// 2018/02/27 take Add Start
					if( j == 0 ){								// �n�����[���i�n���x�_���̌����Ɠ����l�j
						sghv.getAt(0, sghvItem);
						teigiPositionDim = 0.0;
						webHeight = sghvItem.getHcweb();
					}else if( j == _npMax - 1 ){				// �I�����[���i�I���x�_���̌����Ɠ����l�j
						sghv.getAt((sghv.size() - 1), sghvItem);
						teigiPositionDim = 0.0;
						webHeight = sghvItem.getHcweb();
					}else if( rcweb == startLocation ){								//�i�_���̌���
						teigiPositionDim = 0.0;
					}else if( startLocation < rcweb && rcweb < endLocation ){		//�i�_�Ԃ̌���
						teigiPositionDim = rcweb - startLocation;
					}else if( rcweb > endLocation ){								//���̊i�_�Ɉړ�
						startLocation = _kakutenLengAdd[i][j];
						endLocation = _kakutenLengAdd[i][j+1];
						break;
					}else{
						continue;
					}

					if(itplc == 3) {
						EnHokan webHeightHokanMethod = A_SANJIKYOKUSEN;		//�E�F�u����ԕ��@
					}
					DiafSklWebHeightItem dswhItem;
					dswhItem.setKetaName(ketaName);
					dswhItem.setKakutenName(kakutenName);
					dswhItem.setKakutenNo(kakutenNo);
					dswhItem.setTeigiPositionDim(teigiPositionDim);
					dswhItem.setWebHeight(webHeight);
					dswhItem.setWebHeightHokanMethod(webHeightHokanMethod);
					dswh.append(dswhItem);
					if( j == 0 || j == _npMax - 1 ){						// ���[���̏ꍇ
						break;
					}
					// 2018/02/27 take Add End
					// 2018/02/27 take Delete Start
					//break;
					// 2018/02/27 take Delete End
				}
				// 2018/02/27 take Delete Start
				//if(itplc == 3) {
				//	EnHokan webHeightHokanMethod = A_SANJIKYOKUSEN;		//�E�F�u����ԕ��@
				//}
				////��`�ʒu���@
				//teigiPositionDim = _kakutenLengAdd[i][j] + rcweb;
				// 2018/02/27 take Delete End
			}
			// 2018/02/27 take Delete Start
			/*DiafSklWebHeightItem dswhItem;
			dswhItem.setKetaName(ketaName);
			dswhItem.setKakutenName(kakutenName);
			dswhItem.setKakutenNo(kakutenNo);
			dswhItem.setTeigiPositionDim(teigiPositionDim);
			dswhItem.setWebHeight(webHeight);
			dswhItem.setWebHeightHokanMethod(webHeightHokanMethod);
			dswh.append(dswhItem);*/
			// 2018/02/27 take Delete End
		}
	}
	dswh.sort();
	dswh.toCsv(ofOb);

	return JPT_OK;
}

// 2018/02/28 take Edit Start
//(�V)�����⍄�ލ���
//JptErrorStatus AtdDbToCsv::dbToCsvSklHstf(
//	AtdDbGrdHstfLap& ghl,	//�包�����⍄�ރ��b�v�͈�
//	std::ofstream& ofOb)
//{
//	StringHandler sh;
//	DiafSklHstf dsh;
//	for(int i=0;i<_ngMax;i++) {
//		int hstfPos = 0;
//		int hstfNum = 0;
//		// 2018/02/16 take Add Start
//		//�����⍄�ނ̎�t�ʒu�ƒi�����u���b�N���Ɏ擾
//		std::map<int,int> hstfTargetList;
//		// 2018/02/16 take Add End
//		for(int j=0;j<ghl.size();j++) {
//			AtdDbGrdHstfLapItem ghlItem;
//			ghl.getAt(j, ghlItem);
//			int buzaiCode = ghlItem.getBuzaiCode();	//���ސ��R�[�h
//			if(buzaiCode == i+1) {
//				hstfPos = ghlItem.getHstfPos();	//�����⍄�ގ�t�ʒu
//				hstfNum = ghlItem.getHstfNum();	//�����⍄�ޒi��
//				// 2018/02/16 take Add Start
//				hstfTargetList.insert(std::make_pair(hstfPos, hstfNum)); //[��t�ʒu,�i��]
//				// 2018/02/16 take Add End
//				// 2018/02/16 take Delete Start
//				//break;
//				// 2018/02/16 take Delete End
//			}
//		}
//		if(hstfNum < 1) {
//			continue;
//		}
//		string ketaName = _ketaNameameList[i];		//����
//		double hstfHeightRatio[6];	//�����⍄�ލ����䗦1-6
//		for(int k=0;k<6;k++) {
//			hstfHeightRatio[k] = 0.0;
//		}
//		// 2018/02/16 take Edit Start
//		std::set<double> hstfHeightRatioList;
//		for(std::map<int,int>::iterator hstfItr = hstfTargetList.begin(); hstfItr != hstfTargetList.end(); ++hstfItr) {
//			if(hstfItr->first == -1) {			//��
//				if(hstfItr->second == 1) {
//					hstfHeightRatioList.insert(0.2);
//				} else {
//					hstfHeightRatioList.insert(0.14);
//					hstfHeightRatioList.insert(0.36);
//				}
//			} else if(hstfItr->first == 1) {	//��
//				if(hstfItr->second == 1) {
//					hstfHeightRatioList.insert(-0.2);
//				} else {
//					hstfHeightRatioList.insert(-0.36);
//					hstfHeightRatioList.insert(-0.14);
//				}
//			} else if(hstfItr->first == 2) {	//�㉺
//				if(hstfItr->second == 1) {
//					hstfHeightRatioList.insert(0.2);
//					hstfHeightRatioList.insert(-0.2);
//				} else {
//					hstfHeightRatioList.insert(0.14);
//					hstfHeightRatioList.insert(0.36);
//					hstfHeightRatioList.insert(-0.36);
//					hstfHeightRatioList.insert(-0.14);
//				}
//			} else {
//				continue;
//			}
//		}
//		
//		//if(hstfPos == -1) {			//��
//		//	if(hstfNum == 1) {
//		//		hstfHeightRatio[0] = 0.2;
//		//	} else {
//		//		hstfHeightRatio[0] = 0.14;
//		//		hstfHeightRatio[1] = 0.36;
//		//	}
//		//} else if(hstfPos == 1) {	//��
//		//	if(hstfNum == 1) {
//		//		hstfHeightRatio[0] = -0.2;
//		//	} else {
//		//		hstfHeightRatio[0] = -0.36;
//		//		hstfHeightRatio[1] = -0.14;
//		//	}
//		//} else if(hstfPos == 2) {	//�㉺
//		//	if(hstfNum == 1) {
//		//		hstfHeightRatio[0] = 0.2;
//		//		hstfHeightRatio[1] = -0.2;
//		//	} else {
//		//		hstfHeightRatio[0] = 0.14;
//		//		hstfHeightRatio[1] = 0.36;
//		//		hstfHeightRatio[2] = -0.36;
//		//		hstfHeightRatio[3] = -0.14;
//		//	}
//		//} else {
//		//	continue;
//		//}
//		// 2018/02/16 take Edit End
//		DiafSklHstfItem dshItem;
//		dshItem.setKetaName(ketaName);
//		dshItem.setHstfHeightRatio1(hstfHeightRatio[0]);
//		dshItem.setHstfHeightRatio2(hstfHeightRatio[1]);
//		dshItem.setHstfHeightRatio3(hstfHeightRatio[2]);
//		dshItem.setHstfHeightRatio4(hstfHeightRatio[3]);
//		dshItem.setHstfHeightRatio5(hstfHeightRatio[4]);
//		dshItem.setHstfHeightRatio6(hstfHeightRatio[5]);
//		dsh.append(dshItem);
//	}
//	if(dsh.size() > 0) {
//		dsh.sort();
//		dsh.toCsv(ofOb);
//	}
//
//	return JPT_OK;
//}
JptErrorStatus AtdDbToCsv::dbToCsvSklHstf(
	AtdDbStatusHstf& sh,	//�f�ʥHSTF�̈ʒu�֌W(�i���ω�����i��)
	AtdDbRangeHstf &rh,		//�f�ʥ�����⍄�ނ̓���͈�(�ǉ�����)
	std::ofstream& ofOb)
{
	//StringHandler sh;
	DiafSklHstf dsh;

	// 2018/03/01 take Add Start
	_nKetaHstfUpperPrg.resize( _ngMax );
	_nKetaHstfLowerPrg.resize( _ngMax );
	_hstfPanelUpperPrg.resize( _ngMax );
	_hstfPanelLowerPrg.resize( _ngMax );
	// 2018/03/01 take Add End

	for(int i=0;i<_ngMax;i++) {
		// 2018/02/28 take Add Start
		set<int> nHstfUpperPrg;		//�����⍄�ޏ㑤�i��
		set<int> nHstfLowerPrg;		//�����⍄�މ����i��
		set<double> hstfHeightRatio;//�����⍄�ލ����䗦
		vector<int>	nHstfList;		//�����⍄�ޒi��
		vector<int>	rlHstfList;		//�i���ω���
		double currLocation = 0.0;	//�i�����ω�����ʒu
		for(int j=0;j<sh.size();j++){
			AtdDbStatusHstfItem shItem;
			sh.getAt(j, shItem);
			int noGrd = shItem.getNoGrd();
			int nStf= shItem.getNsfh();
			double rlsfh = shItem.getRlsfh();
			if(noGrd != i + 1){
				continue;
			}
			currLocation += rlsfh;
			nHstfList.push_back(nStf);
			rlHstfList.push_back(currLocation);
		}
		
		for(int j=0;j<_hstfLengAdd[i].size();j++){
			// 2018/03/01 take Add Start
			bool existUpperPrgFlg = false;	//��iHSTF�����݃t���O
			bool existLowerPrgFlg = false;	//���iHSTF�����݃t���O
			// 2018/03/01 take Add End
			for(int k=0;k<rh.size();k++){
				AtdDbRangeHstfItem rhItem;
				rh.getAt(k, rhItem);
				int noGrd =	rhItem.getNoGrd();
				int noHstf = rhItem.getNoHstf();
				double tLen1 = rhItem.getTLen1();
				double tLen2 = rhItem.getTLen2();
				if(noGrd != i + 1){	
					continue;
				}
				if(noHstf == -1){	
					continue;
				}
				if(tLen1 > _hstfLengAdd[i][j] || _hstfLengAdd[i][j] > tLen2){
					continue;
				}
				int uplo = rhItem.getUplo();
				if(uplo == 0){			//��i
					existUpperPrgFlg = true;
					for(int l=0;l<rlHstfList.size();l++){
						if(_hstfLengAdd[i][j] < rlHstfList[l]){	//�����⍄�ނ��ǂ̒f�ʕω����ɑ����Ă��邩
							if(nHstfList[l] == 1){				//1�i�̏ꍇ
								hstfHeightRatio.insert(0.2);	//0.2H
								_hstfPanelUpperPrg[i].push_back(1);
								nHstfUpperPrg.insert(1);
							}else if(nHstfList[l] == 2){		//2�i�̏ꍇ
								hstfHeightRatio.insert(0.14);	//0.14H
								hstfHeightRatio.insert(0.36);	//0.36H
								_hstfPanelUpperPrg[i].push_back(2);
								nHstfUpperPrg.insert(2);
							}
							break;
						}
					}
				}else if(uplo == 1){	//���i
					existLowerPrgFlg = true;
					for(int l=0;l<rlHstfList.size();l++){
						if(_hstfLengAdd[i][j] < rlHstfList[l]){	//�����⍄�ނ��ǂ̒f�ʕω����ɑ����Ă��邩
							if(nHstfList[l] == 1){				//1�i�̏ꍇ
								hstfHeightRatio.insert(-0.2);	//-0.2H
								_hstfPanelLowerPrg[i].push_back(1);
								nHstfLowerPrg.insert(1);
							}else if(nHstfList[l] == 2){		//2�i�̏ꍇ
								hstfHeightRatio.insert(-0.14);	//-0.14H
								hstfHeightRatio.insert(-0.36);	//-0.36H
								_hstfPanelLowerPrg[i].push_back(2);
								nHstfLowerPrg.insert(2);
							}
							break;
						}
					}
				}
			}
			if( existUpperPrgFlg == false ){	//��i�����݂��Ȃ������ꍇ
				_hstfPanelUpperPrg[i].push_back( 0 );
			}
			if( existLowerPrgFlg == false ){	//���i�����݂��Ȃ������ꍇ
				_hstfPanelLowerPrg[i].push_back( 0 );
			}
		}
		//��i�̐����擾
		int hstfPrg = 0;
		set<int>::iterator itr;
		for(itr=nHstfUpperPrg.begin(); itr != nHstfUpperPrg.end(); ++itr) {
			hstfPrg += *itr;
		}
		_nKetaHstfUpperPrg[i].push_back(hstfPrg);

		//���i�̐����擾
		hstfPrg = 0;
		for(itr=nHstfLowerPrg.begin(); itr != nHstfLowerPrg.end(); ++itr) {
			hstfPrg += *itr;
		}
		_nKetaHstfLowerPrg[i].push_back(hstfPrg);

		string ketaName = _ketaNameameList[i];		//����
		double tempHstfHeightRatio[6];	//�����⍄�ލ����䗦1-6
		for(int j=0;j<6;j++){			//�����⍄�ލ����̐�
			if(j < hstfHeightRatio.size()){
				set<double>::iterator temp = hstfHeightRatio.begin();
				for(int k=0;k<hstfHeightRatio.size()-(1+j);++k){	//�v�f�̈ړ�
					++temp;
				}
				tempHstfHeightRatio[j] = *temp;
			}else{
				tempHstfHeightRatio[j] = 0.0;
			}
		}
		// 2018/02/28 take Add End
		DiafSklHstfItem dshItem;
		dshItem.setKetaName(ketaName);
		dshItem.setHstfHeightRatio1(tempHstfHeightRatio[0]);
		dshItem.setHstfHeightRatio2(tempHstfHeightRatio[1]);
		dshItem.setHstfHeightRatio3(tempHstfHeightRatio[2]);
		dshItem.setHstfHeightRatio4(tempHstfHeightRatio[3]);
		dshItem.setHstfHeightRatio5(tempHstfHeightRatio[4]);
		dshItem.setHstfHeightRatio6(tempHstfHeightRatio[5]);
		dsh.append(dshItem);
	}
	if(dsh.size() > 0) {
		dsh.sort();
		dsh.toCsv(ofOb);
	}

	return JPT_OK;
}
// 2018/02/28 take Edit End

//�R�D�包
//(�P)���ʏڍ׃f�[�^
JptErrorStatus AtdDbToCsv::dbToCsvGirderCommon(
	AtdGirderCommon& agc,
	AtdDbSecScaleSpl& sss,	//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��包�Y�ڊ֌W
	AtdDbInputGrdSpl& igs,	//���ͥ�包�Y��	
	std::ofstream& ofOb)
{
	DiafGirderCommon dgc;
	int ibuuti = 0;
	int ibluti = 0;
	for(int i=0;i<sss.size();i++) {
		AtdDbSecScaleSplItem sssItem;
		sss.getAt(i, sssItem);
		ibuuti = sssItem.getIbuuti();	//IBUUTI UFLG����������
		ibluti = sssItem.getIbluti();	//IBLUTI LFLG����������
	}
	int iclegu = 0;
	int iclegw = 0;
	int iclegl = 0;
	for(int i=0;i<igs.size();i++) {
		AtdDbInputGrdSplItem igsItem;
		igs.getAt(i, igsItem);
		iclegu = igsItem.getIclegu();	//ICLEGU UFLG_JC
		iclegw = igsItem.getIclegw();	//ICLEGW WEB_JC
		iclegl = igsItem.getIclegl();	//ICLEGL LFLG_JC
		break;
	}
	EnPlateThicknessEscWay uflgPtEscWay;	//��t�����W����������
	EnPlateThicknessEscWay lflgPtEscWay;	//���t�����W����������
	if(ibuuti == 0) {	//�O����
		uflgPtEscWay = SOTONIGE;
	} else {			//������
		uflgPtEscWay = UCHINIGE;
	}
	if(ibluti == 0) {	//�O����
		lflgPtEscWay = SOTONIGE;
	} else {			//������
		lflgPtEscWay = UCHINIGE;
	}
//	double uflgJc = (double)(iclegu % 1000) / 100;			//��t�����W�̃W���C���g�N���A�����X
//	double webJc = (double)(iclegw % 1000) / 100;			//�E�F�u�̃W���C���g�N���A�����X
//	double lflgJc = (double)(iclegl % 1000) / 100;			//���t�����W�̃W���C���g�N���A�����X
	double uflgJc = agc.getUflgJc();						//��t�����W�̃W���C���g�N���A�����X
	double webJc = agc.getWebJc();							//�E�F�u�̃W���C���g�N���A�����X
	double lflgJc = agc.getLflgJc();						//���t�����W�̃W���C���g�N���A�����X
	double itatsugiZureRyo = agc.getItatsugiZureRyo();		//�p�Y����
	EnTypeNo uflgZaitanKeijo = agc.getUflgZaitanKeijo();								//��t�����W�ޒ[�`��
	EnTypeNo lflgZaitanKeijo = agc.getLflgZaitanKeijo();								//���t�����W�ޒ[�`��
	double updownFlgZaitanKeijoTachiageRyo = agc.getUpdownFlgZaitanKeijoTachiageRyo();	//�㉺�t�����W�ޒ[�`�󗧏グ��
	double lflgKakuhukubuTaper = agc.getLflgKakuhukubuTaper();							//���t�����W�g�����̃e�[�p�[���z
	double solePlateKyotyokuFreeSpace = agc.getSolePlateKyotyokuFreeSpace();	//�\�[���v���[�g�̋��������󂫗�
	double solePlateKyojikuFreeSpace = agc.getSolePlateKyojikuFreeSpace();		//�\�[���v���[�g�̋��������󂫗�
	double hsFreeSpaceVs = agc.getHsFreeSpaceVs();					//�����⍄�ނ̐����⍄�ޕ��A�����E�F�u���󂫗�
	double hsFreeSpaceSpl = agc.getHsFreeSpaceSpl();				//�����⍄�ނ̓Y�ڔ��󂫗�
	double hsFreeSpaceCbf = agc.getHsFreeSpaceCbf();				//�����⍄�ނ̉����t�����W���󂫗�
	double hsFreeSpaceCbfUlimit = agc.getHsFreeSpaceCbfUlimit();	//�����⍄�ނ̉����t�����W����̍������@���
	string hsSnipSizeVs = agc.getHsSnipSizeVs();					//�����⍄�ނ̐����⍄�ޕ��̃X�j�b�v�T�C�Y
	string hsSnipSizeSpl = agc.getHsSnipSizeSpl();					//�����⍄�ނ̓Y�ڕ��̃X�j�b�v�T�C�Y
	string hsSnipSizeCbf = agc.getHsSnipSizeCbf();					//�����⍄�ނ̉����t�����W���̃X�j�b�v�T�C�Y
	string shitenVsCutWu = agc.getShitenVsCutWu();					//�x�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
	string shitenVsCutWd = agc.getShitenVsCutWd();					//�x�_�㐂���⍄�ނ̗n�ڕӑ������،�
	string shitenVsCutFu = agc.getShitenVsCutFu();					//�x�_�㐂���⍄�ނ̏㑤�،�
	string kakutenVsCutWu = agc.getKakutenVsCutWu();				//�i�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
	string kakutenVsCutWd = agc.getKakutenVsCutWd();				//�i�_�㐂���⍄�ނ̗n�ڕӑ������،�
	string kakutenVsCutFu = agc.getKakutenVsCutFu();				//�i�_�㐂���⍄�ނ̏㑤�،�
	string middleVsCutWu = agc.getMiddleVsCutWu();					//���Ԑ����⍄�ނ̗n�ڕӑ��㑤�،�
	string middleVsCutWd = agc.getMiddleVsCutWd();					//���Ԑ����⍄�ށi���k�^�C�v�j�̗n�ڕӑ������،�
	double middleVsFreeSpace = agc.getMiddleVsFreeSpace();			//���Ԑ����⍄�ށi�����^�C�v�j�̉����󂫗�
	double uflgSplKyojikuZaitan = agc.getUflgSplKyojikuZaitan();					//��t�����W�Y�ڔ̋��������ޒ[
	double uflgOutsideSplKyotyokuZaitan = agc.getUflgOutsideSplKyotyokuZaitan();	//��t�����W�O���Y�ڔ̋��������ޒ[
	double uflgInsideSplKyotyokuZaitan = agc.getUflgInsideSplKyotyokuZaitan();		//��t�����W�����Y�ڔ̋��������ޒ[
	double webSplKyojikuZaitan = agc.getWebSplKyojikuZaitan();						//�E�F�u�Y�ڔ̍��������ޒ[
	double webSplHeightZaitan = agc.getWebSplHeightZaitan();						//�E�F�u�Y�ڔ̋��������ޒ[
	double lflgSplKyojikuZaitan = agc.getLflgSplKyojikuZaitan();					//���t�����W�Y�ڔ̋��������ޒ[
	double lflgSplKyotyokuZaitan = agc.getLflgSplKyotyokuZaitan();					//���t�����W�Y�ڔ̋��������ޒ[
	double webHoleSlopeLowerLimit = agc.getWebHoleSlopeLowerLimitGrd();				//�E�F�u�E�̍E���z�̉����l
	dgc.setUflgPtEscWay(uflgPtEscWay);	//��t�����W����������
	dgc.setLflgPtEscWay(lflgPtEscWay);	//���t�����W����������
	dgc.setUflgJc(uflgJc);				//��t�����W�̃W���C���g�N���A�����X
	dgc.setWebJc(webJc);				//�E�F�u�̃W���C���g�N���A�����X
	dgc.setLflgJc(lflgJc);				//���t�����W�̃W���C���g�N���A�����X
	dgc.setItatsugiZureRyo(itatsugiZureRyo);
	dgc.setUflgZaitanKeijo(uflgZaitanKeijo);
	dgc.setLflgZaitanKeijo(lflgZaitanKeijo);
	dgc.setUpdownFlgZaitanKeijoTachiageRyo(updownFlgZaitanKeijoTachiageRyo);
	dgc.setLflgKakuhukubuTaper(lflgKakuhukubuTaper);
	dgc.setSolePlateKyotyokuFreeSpace(solePlateKyotyokuFreeSpace);
	dgc.setSolePlateKyojikuFreeSpace(solePlateKyojikuFreeSpace);
	dgc.setHsFreeSpaceVs(hsFreeSpaceVs);
	dgc.setHsFreeSpaceSpl(hsFreeSpaceSpl);
	dgc.setHsFreeSpaceCbf(hsFreeSpaceCbf);
	dgc.setHsFreeSpaceCbfUlimit(hsFreeSpaceCbfUlimit);
	dgc.setHsSnipSizeVs(hsSnipSizeVs);
	dgc.setHsSnipSizeSpl(hsSnipSizeSpl);
	dgc.setHsSnipSizeCbf(hsSnipSizeCbf);
	dgc.setShitenVsCutWu(shitenVsCutWu);
	dgc.setShitenVsCutWd(shitenVsCutWd);
	dgc.setShitenVsCutFu(shitenVsCutFu);
	dgc.setKakutenVsCutWu(kakutenVsCutWu);
	dgc.setKakutenVsCutWd(kakutenVsCutWd);
	dgc.setKakutenVsCutFu(kakutenVsCutFu);
	dgc.setMiddleVsCutWu(middleVsCutWu);
	dgc.setMiddleVsCutWd(middleVsCutWd);
	dgc.setMiddleVsFreeSpace(middleVsFreeSpace);
	dgc.setUflgSplKyojikuZaitan(uflgSplKyojikuZaitan);
	dgc.setUflgOutsideSplKyotyokuZaitan(uflgOutsideSplKyotyokuZaitan);
	dgc.setUflgInsideSplKyotyokuZaitan(uflgInsideSplKyotyokuZaitan);
	dgc.setWebSplKyojikuZaitan(webSplKyojikuZaitan);
	dgc.setWebSplHeightZaitan(webSplHeightZaitan);
	dgc.setLflgSplKyojikuZaitan(lflgSplKyojikuZaitan);
	dgc.setLflgSplKyotyokuZaitan(lflgSplKyotyokuZaitan);
	dgc.setWebHoleSlopeLowerLimit(webHoleSlopeLowerLimit);
	dgc.toCsv(ofOb);

	return JPT_OK;
}

//(�Q)�包�f��
JptErrorStatus AtdDbToCsv::dbToCsvGirderSection(
AtdDbInputUseMaterial& ium,	//���́E�g�p�ޗ��f�[�^
	AtdDbSecGrd& sgd,		//�f�ʥ�包�f��
	AtdDbSecGrdLeng& sgl,	//�f�ʥ�包�̒f�ʒ�	
	std::ofstream& ofOb)
{
	DiafGirderSection dgs;

	_danmenLengAdd.resize(_ngMax);
	_danmenAtsuUflg.resize(_ngMax);
	_danmenHabaUflg.resize(_ngMax);
	_danmenAtsuWeb.resize(_ngMax);
	_danmenAtsuLflg.resize(_ngMax);
	for( int i=0; i<_ngMax; i++ ){
		_danmenLengAdd[i].resize(_jointMax+1);
		_danmenAtsuUflg[i].resize(_jointMax+1);
		_danmenHabaUflg[i].resize(_jointMax+1);
		_danmenAtsuWeb[i].resize(_jointMax+1);
		_danmenAtsuLflg[i].resize(_jointMax+1);
	}
	for(int i=0;i<sgd.size();i++) {//AtdDbSecGrd& sgd,		//�f�ʥ�包�f��
		AtdDbSecGrdItem sgdItem;
		sgd.getAt(i, sgdItem);
		int nogrd = sgdItem.getNogrd();				//NOGRD ����
		int nosec = sgdItem.getNosec();				//NOSEC �f�ʔԍ�
		string ketaName = _ketaNameameList[nogrd-1];		//����
		int sectionNo = nosec;						//�f�ʔԍ�
		double sectionLength = 0.0;					//�f�ʒ�
		double uflgWidth = sgdItem.getBuf();								//BUF UFLG_�� ��t�����W��
		double uflgPlateThickness = sgdItem.getTuf();						//TUF UFLG_�� ��t�����W��
		int muf = sgdItem.getMuf();											//MUF UFLG_�ގ�
		string uflgMaterial = ium.findMaterial(uflgPlateThickness, muf);	//��t�����W�ގ�
		double webPlateThickness = sgdItem.getBlw();						//BLW WEB_�� �E�F�u��
		int mlw = sgdItem.getMlw();											//MLW WEB_�ގ�
		string webMaterial = ium.findMaterial(webPlateThickness, mlw);		//�E�F�u�ގ�
		double lflgWidth = sgdItem.getBlf();								//BLF LFLG_�� ���t�����W��
		double lflgPlateThickness = sgdItem.getTlf();						//TLF LFLG_�� ���t�����W��
		int mlf = sgdItem.getMlf();											//MLF LFLG_�ގ�
		string lflgMaterial = ium.findMaterial(lflgPlateThickness, mlf);	//���t�����W�ގ�
		for(int j=0;j<sgl.size();j++) {//AtdDbSecGrdLeng& sgl,	//�f�ʥ�包�̒f�ʒ�
			AtdDbSecGrdLengItem sglItem;
			sgl.getAt(j, sglItem);
			int nogrd2 = sglItem.getNogrd();		//NOGRD ����
			int nosec2 = sglItem.getNosec();		//NOSEC �W���C���g��
			if(nogrd == nogrd2 && nosec == nosec2) {
				sectionLength = sglItem.getRlsec();	//RLSEC �u���b�N��
				break;
			}
		}
		if(sectionLength < 1.0) {
//err
		}
		_danmenLengAdd[nogrd-1][nosec-1] = sectionLength;		//����
		_danmenAtsuUflg[nogrd-1][nosec-1] = uflgPlateThickness;	//TUF UFLG_�� ��t�����W��
		_danmenHabaUflg[nogrd-1][nosec-1] = uflgWidth;			//BUF UFLG_�� ��t�����W��
		_danmenAtsuWeb[nogrd-1][nosec-1] = webPlateThickness;	//BLW WEB_�� �E�F�u��
		_danmenAtsuLflg[nogrd-1][nosec-1] = lflgPlateThickness;	//TLF LFLG_�� ���t�����W��
		DiafGirderSectionItem dgsItem;
		dgsItem.setKetaName(ketaName);
		dgsItem.setSectionNo(sectionNo);
		dgsItem.setSectionLength(sectionLength);
		dgsItem.setUflgWidth(uflgWidth);
		dgsItem.setUflgPlateThickness(uflgPlateThickness);
		dgsItem.setUflgMaterial(uflgMaterial);
		dgsItem.setWebPlateThickness(webPlateThickness);
		dgsItem.setWebMaterial(webMaterial);
		dgsItem.setLflgWidth(lflgWidth);
		dgsItem.setLflgPlateThickness(lflgPlateThickness);
		dgsItem.setLflgMaterial(lflgMaterial);
		dgs.append(dgsItem);
	}
	for( int i=0; i<_ngMax; i++ ){
		for( int j=1; j<_jointMax+1; j++ ){
			_danmenLengAdd[i][j] += _danmenLengAdd[i][j-1];
		}
	}
	dgs.sort();
	dgs.toCsv(ofOb);

	return JPT_OK;
}

//(�R)���t�����W�g�����`��
JptErrorStatus AtdDbToCsv::dbToCsvGirderLflgWidening(
//AtdDbSecGrdKaku& sgk,		//�f�ʥ�包�i�_��
	AtdDbSecSolePl& ssp,	//�f�ʥ�\�[���v���[�g
	std::ofstream& ofOb)
{
	DiafGirderLflgWidening dglw;

	for(int i=0;i<ssp.size();i++) {
		AtdDbSecSolePlItem sspItem;
		ssp.getAt(i, sspItem);
		int nogrd = sspItem.getNogrd();				//NOGRD ����
		string ketaName = _ketaNameameList[nogrd-1];		//����
		int nos = sspItem.getNos();					//NOS
		int kakutenNo = _shitenKakutenNo[nos-1];	//�i�_�ԍ�
		string shitenName = _shitenNameList[nos-1];	//�x�_��
		double solePlateWidth = sspItem.getBs1();	//BS1 SOLE-PL�� �\�[���v���[�g�̕�
		double solePlateLength = sspItem.getRls1();	//RLS1 SOLE-PL���� �\�[���v���[�g�̒���
		DiafGirderLflgWideningItem dglwItem;
		dglwItem.setKetaName(ketaName);
		dglwItem.setShitenName(shitenName);
		dglwItem.setKakutenNo(kakutenNo);
		dglwItem.setSolePlateWidth(solePlateWidth);
		dglwItem.setSolePlateLength(solePlateLength);
		dglw.append(dglwItem);
	}
	dglw.sort();
	dglw.toCsv(ofOb);

	return JPT_OK;
}

//(�S)���[�}���z�[���`��
JptErrorStatus AtdDbToCsv::dbToCsvGirderWebManhole(
	AtdDbSecManhole& smh,	//�f�ʥ���[���̃}���z�[���̃J�b�g
	AtdDbSecCutData& scd,	//�f�ʥ�J�b�g�f�[�^
	std::ofstream& ofOb)
{
	DiafGirderWebManhole dgwm;

	for(int i=0;i<smh.size();i++) {
		AtdDbSecManholeItem smhItem;
		smh.getAt(i, smhItem);
		int nogrd = smhItem.getNogrd();	//NOGRD ����
		int sepos = smhItem.getSepos();	//SEPOS �z�u��
		int iwcut = smhItem.getIwcut();	//IWCUT
		string ketaName = _ketaNameameList[nogrd-1];	//����
		EnSide setSide;		//���[�}���z�[���̔z�u��
		if(sepos == 0) {	//�n��
			setSide = STARTSIDE;
		} else {			//�I��
			setSide = ENDSIDE;
		}
		double mhPosition = 0.0;	//���[�}���z�[���̃E�F�u���[����̐��@
		double mhHeight = 0.0;		//���[�}���z�[���̍���
		double mhWidth = 0.0;		//���[�}���z�[���̕�
		double mhRsize = 0.0;		//���[�}���z�[����R�T�C�Y
		bool flag = false;
		for(int j=0;j<scd.size();j++) {
			AtdDbSecCutDataItem scdItem;
			scd.getAt(j, scdItem);
			int dtcut1 = scdItem.getDtcut1();	//DTCUT1
			if(iwcut == dtcut1) {
				mhPosition = scdItem.getDtcut2();	//DTCUT2 MH_�ʒu
				mhHeight = scdItem.getDtcut4();		//DTCUT4 MH_����
				mhWidth = scdItem.getDtcut3();		//DTCUT3 MH_��
				mhRsize = scdItem.getDtcut5();		//DTCUT5 MH_R�T�C�Y
				flag = true;
				break;
			}
		}
		if(flag != true) {
			continue;
		}
		DiafGirderWebManholeItem dgwmItem;
		dgwmItem.setKetaName(ketaName);
		dgwmItem.setSetSide(setSide);
		dgwmItem.setMhPosition(mhPosition);
		dgwmItem.setMhHeight(mhHeight);
		dgwmItem.setMhWidth(mhWidth);
		dgwmItem.setMhRsize(mhRsize);
		dgwm.append(dgwmItem);
	}
	if(dgwm.size() > 0) {
		dgwm.sort();
		dgwm.toCsv(ofOb);
	}

	return JPT_OK;
}

//(�T)�����⍄��ID�o�^
// 2018/03/01 take Edit Start
//JptErrorStatus AtdDbToCsv::dbToCsvGirderVstfId(
//	AtdDbInputUseMaterial& ium,	//���́E�g�p�ޗ��f�[�^
//	AtdDbSecVstf& svs,			//�f�ʥVSTF�f��
//	AtdDbSecGrdKakuVst& sgv,	//�f�ʥ�包�i�_��VSTF
//	AtdDbSecVstfHaichi& svh,	//�f�ʥVSTF�z�u
//	AtdDbInputStfAki& isa,		//���ͥ�r�s�e�̂���
//	AtdDbStructAll& sta,		//�\����S��
//	AtdDbGrdSecPower& gsp,		//�包�f�ʗ̓f�[�^
//	std::ofstream& ofOb)
JptErrorStatus AtdDbToCsv::dbToCsvGirderVstfId(
	AtdDbInputUseMaterial& ium,	//���́E�g�p�ޗ��f�[�^
	AtdDbSecVstf& svs,			//�f�ʥVSTF�f��
	AtdDbSecGrdKakuVst& sgv,	//�f�ʥ�包�i�_��VSTF
	AtdDbSecVstfHaichi& svh,	//�f�ʥVSTF�z�u
	AtdDbInputStfAki& isa,		//���ͥ�r�s�e�̂���
	AtdDbStructAll& sta,		//�\����S��
	AtdDbGrdPower& gp,			//�包���͓x�f�[�^
	std::ofstream& ofOb)
// 2018/03/01 take Edit End
{
	StringHandler sh;
	DiafGirderVstfId dgvi;

	for(int i=0;i<svs.size();i++) {
		AtdDbSecVstfItem svsItem;
		svs.getAt(i, svsItem);
		int novssc = svsItem.getNovssc();	//NOVSSC ����ID
		double vssc3 = svsItem.getVssc3();	//VSSC3 ��
		double vssc5 = svsItem.getVssc5();	//VSSC5 ��
		// 2018/02/27 take Edit Start
		//int vssc8 = svsItem.getVssc8();		//VSSC8 �ގ�
		int vssc8 = svsItem.getVssc8() % 1000;
		// 2018/02/27 take Edit End
		int idgfvi = -2;
		for(int j=0;j<isa.size();j++) {
			AtdDbInputStfAkiItem isaItem;
			isa.getAt(j, isaItem);
			idgfvi = isaItem.getIdgfvi();	//IDGFVI ���́ESTF�̂��� �^�C�v
		}
		int ig = -1;
		int ip = -1;
		// 2018/03/01 take Delete Start
		//double grdUflgHaba = 0.0;
		//double grdWebHaba = 0.0;
		// 2018/03/01 take Delete End
		// 2018/03/01 take Add Start
		bool OnVstfKakuten = false;		//VSTF���i�_��t���O�itrue�F�i�_��,false�F�i�_�ȊO�j
		// 2018/03/01 take Add End
		//�包��t�����W���擾
		for(int j=0;j<sgv.size();j++) {	//�f�ʥ�包�i�_��VSTF
			AtdDbSecGrdKakuVstItem sgvItem;
			sgv.getAt(j, sgvItem);
			int novstp = sgvItem.getNovstp();	//NOVSTP �z�u�� ����ID
			int novst = abs(novstp) % 100;
			if(novst == novssc) {
				ig = sgvItem.getNogrd();		//NOGRD ����
				ip = sgvItem.getNocrs();		//NOCRS
				// 2018/03/01 take Add Start
				OnVstfKakuten = true;
				// 2018/03/01 take Add End
				break;
			}
		}
		if(ig < 0) {
			for(int j=0;j<svh.size();j++) {	//�f�ʥVSTF�z�u
				AtdDbSecVstfHaichiItem svhItem;
				svh.getAt(j, svhItem);
				int novst = svhItem.getNovst();	//NOVST ����ID
				if(novst == novssc) {
					ig = svhItem.getNogrd();	//NOGRD ����
					ip = svhItem.getNopnl();	//NOPNL �n���i�_�� �I���i�_��
					break;
				}
			}
		}
		// 2018/03/01 take Delete Start
		/*if(ig > 0) {
			grdUflgHaba = _danmenHabaUflg[ig-1][0];
			grdWebHaba = _danmenAtsuWeb[ig-1][0];
		}*/
		// 2018/03/01 take Delete End

		EnTypeNo girderVstfType = TYPE1;	//�����⍄�ނ̃^�C�v
		// 2018/03/01 take Edit Start
		//if(idgfvi == -2) {	//�w-2�������Ȃ��x
		//	if((grdUflgHaba/2 - grdWebHaba/2) < vssc3) {
		//		girderVstfType = TYPE1;
		//	} else {
		//		girderVstfType = TYPE2;
		//	}
		//} else {	//�w0���������Ƒ������x�A�w1���������̂݁x
		//	int nspan = 1;
		//	if(sta.size() > 0) {
		//		AtdDbStructAllItem staItem;
		//		sta.getAt(0, staItem);
		//		nspan = staItem.getNspan();	//NSPAN
		//	}
		//	if(nspan == 1) {	//�w�\���E�S�́x�e�[�u���́wNSPAN�x�̒l��1�̂Ƃ��A���ׂĂ̐����⍄�ނ��^�C�v2
		//		girderVstfType = TYPE2;
		//	} else {
		//		double mageMx = gsp.findMageMx(1, 1);	//�包�f�ʗ̓f�[�^ �Ȃ����[�����g�i�ʓ��j�l��
		//		if(mageMx > 0.0) {
		//			girderVstfType = TYPE2;
		//		} else {
		//			if((grdUflgHaba/2 - grdWebHaba/2) < vssc3) {
		//				girderVstfType = TYPE1;
		//			} else {
		//				girderVstfType = TYPE2;
		//			}
		//		}
		//	}
		//}
		if( OnVstfKakuten == true && _oudanType[ip-1] == 0 || OnVstfKakuten == true && _oudanType[ip-1] == 1 ){	//�x�_���̏ꍇ
			girderVstfType = TYPE1;
		}else if( OnVstfKakuten == true ){	//�i�_���̏ꍇ
			girderVstfType = TYPE2;
		}else if( idgfvi == 0 ){	//���ԕ��̏ꍇ�i0�������Ȃ��j
			girderVstfType = TYPE2;
		}else{						//���ԕ��̏ꍇ�i-1���������̂�,-2���������Ƒ������j
			if(_nSpan == 1) {		//�P�����̂Ƃ��A���ׂĂ̐����⍄�ނ��^�C�v3
				girderVstfType = TYPE3;
			} else {
				// 2018/03/01 take Edit Start
				//double mageMx = gsp.findMageMx(1, 1);	//�包�f�ʗ̓f�[�^ �Ȃ����[�����g�i�ʓ��j�l��
				//if(mageMx > 0.0) {
				double forceMx = gp.findForceMx(ig, ip);//�包���͓x�f�[�^ �l���ɑ΂��鉞�͓x
				if(forceMx > 0.0) {
				// 2018/03/01 take Edit End
				
					girderVstfType = TYPE3;
				} else {
					girderVstfType = TYPE2;
				}
			}
		}
		// 2018/03/01 take Edit End
		string girderVstfId = "VS" + sh.toString(novssc);	//����ID
		double width = vssc3;				//��
		double plateThickness = vssc5;		//��
		string material = ium.findMaterial(plateThickness, vssc8);	//�ގ�
		DiafGirderVstfIdItem dgviItem;
		dgviItem.setGirderVstfId(girderVstfId);
		dgviItem.setGirderVstfType(girderVstfType);
		dgviItem.setWidth(width);
		dgviItem.setPlateThickness(plateThickness);
		dgviItem.setMaterial(material);
		dgvi.append(dgviItem);
		_grdVstfNovssc.push_back(novssc);		//�包�����⍄�ގ���ID
		_grdVstfItaatsu.push_back(plateThickness);	//�包�����⍄�ޔ�
	}
	dgvi.sort();
	dgvi.toCsv(ofOb);

	return JPT_OK;
}

//(�U)�x�_�E�i�_�����⍄�ޔz�u
JptErrorStatus AtdDbToCsv::dbToCsvGirderVstfSetSc(
	AtdDbSecGrdKakuVst& sgv,	//�f�ʥ�包�i�_��VSTF
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderVstfSetSc dgvss;

	for(int i=0;i<_ngMax;i++) {
		for(int j=0;j<(int)_oudanNameList.size();j++) {
			int novstp = sgv.findId(i+1, j+1);
			if(novstp == 0) {
				continue;
			}
			string ketaName = _ketaNameameList[i];			//����
			string kakutenName = _oudanNameList[j];		//�i�_��
			int kakutenNo = j+1;						//�i�_�ԍ�
			DiafGirderVstfSetScItem dgvssItem;
			dgvssItem.setKetaName(ketaName);
			dgvssItem.setKakutenName(kakutenName);
			dgvssItem.setKakutenNo(kakutenNo);
			string girderVstfId;
			if(novstp > 0) {		//����
				double vstfAtsu =findDouble(novstp, _grdVstfNovssc, _grdVstfItaatsu);
				_grdKakuVstfKetaNo.push_back(i+1);	//�x�_�E�i�_�包�����⍄�ތ��ԍ�
				_grdKakuVstfKakutenNo.push_back(j+1);	//�x�_�E�i�_�包�����⍄�ފi�_�ԍ�
				_grdKakuVstfKetaName.push_back(ketaName);	//�x�_�E�i�_�包�����⍄�ތ���
				_grdKakuVstfKakutenName.push_back(kakutenName);	//�x�_�E�i�_�包�����⍄�ފi�_��
				_grdKakuVstfFace.push_back(NS);	//�x�_�E�i�_�包�����⍄�ޔz�u��
				_grdKakuVstfitaastu.push_back(vstfAtsu);	//�x�_�E�i�_�包�����⍄�ޔ�
				_grdKakuVstfKetaNo.push_back(i+1);	//�x�_�E�i�_�包�����⍄�ތ��ԍ�
				_grdKakuVstfKakutenNo.push_back(j+1);	//�x�_�E�i�_�包�����⍄�ފi�_�ԍ�
				_grdKakuVstfKetaName.push_back(ketaName);	//�x�_�E�i�_�包�����⍄�ތ���
				_grdKakuVstfKakutenName.push_back(kakutenName);	//�x�_�E�i�_�包�����⍄�ފi�_��
				_grdKakuVstfFace.push_back(FS);	//�x�_�E�i�_�包�����⍄�ޔz�u��
				_grdKakuVstfitaastu.push_back(vstfAtsu);	//�x�_�E�i�_�包�����⍄�ޔ�
				girderVstfId = "VS" + sh.toString(novstp);
				dgvssItem.setGirderVstfId(girderVstfId);
				dgvssItem.setSetFace(NS);
				dgvss.append(dgvssItem);
				dgvssItem.setSetFace(FS);
				dgvss.append(dgvssItem);
			} else {
				EnFace setFace;					//�����⍄�ނ̔z�u��
				novstp = abs(novstp);
				if(novstp > 100) {	//�E��
					novstp = novstp % 100;
					setFace = NS;		//	�\��
				} else {			//����
					setFace = FS;		//	����
				}
				double vstfAtsu =findDouble(novstp, _grdVstfNovssc, _grdVstfItaatsu);
				_grdKakuVstfKetaNo.push_back(i+1);	//�x�_�E�i�_�包�����⍄�ތ��ԍ�
				_grdKakuVstfKakutenNo.push_back(j+1);	//�x�_�E�i�_�包�����⍄�ފi�_�ԍ�
				_grdKakuVstfKetaName.push_back(ketaName);	//�x�_�E�i�_�包�����⍄�ތ���
				_grdKakuVstfKakutenName.push_back(kakutenName);	//�x�_�E�i�_�包�����⍄�ފi�_��
				_grdKakuVstfFace.push_back(setFace);	//�x�_�E�i�_�包�����⍄�ޔz�u��
				_grdKakuVstfitaastu.push_back(vstfAtsu);	//�x�_�E�i�_�包�����⍄�ޔ�
				girderVstfId = "VS" + sh.toString(novstp);
				dgvssItem.setGirderVstfId(girderVstfId);
				dgvssItem.setSetFace(setFace);
				dgvss.append(dgvssItem);
			}
		}
	}
	dgvss.sort();
	dgvss.toCsv(ofOb);

	return JPT_OK;
}

//(�V)���Ԑ����⍄�ޔz�u
JptErrorStatus AtdDbToCsv::dbToCsvGirderVstfSetV(
	AtdDbSecVstfHaichi& svh,	//�f�ʥVSTF�z�u
	AtdDbSecStfDir& ssd,		//�f�ʥ�ڍץ�⍄�ނ̌���
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderVstfSetV dgvsv;

	for(int i=0;i<_ngMax;i++) {
		int idrv = ssd.findIdrv(i+1);
		if(idrv < 0) {
//err
		}
		for(int j=0;j<(int)_oudanNameList.size()-1;j++) {
			int novst = svh.findId(i+1, j+1);
			if(novst < 1) {
				continue;
			}
			string ketaName = _ketaNameameList[i];					//����
			string startSideKakutenName = _oudanNameList[j];	//�n���i�_��
			string endSideKakutenName = _oudanNameList[j+1];	//�I���i�_��
			int startSideKakutenNo = j+1;						//�n���i�_�ԍ�
			int endSideKakutenNo = j+2;							//�I���i�_�ԍ�
			int middlePointNo = 0;								//�i�_�Ԃ̒��ԓ_�ԍ�
			EnFace setFace;			//�����⍄�ނ̔z�u��
			if(idrv == 0) {
				setFace = FS;	//	����
			} else {			//����
				setFace = NS;	//	�\��
			}
			string girderVstfId = "VS" + sh.toString(novst);				//�z�u���鐂���⍄�ނ̎���ID
			DiafGirderVstfSetVItem dgvsvItem;
			dgvsvItem.setKetaName(ketaName);
			dgvsvItem.setStartSideKakutenName(startSideKakutenName);
			dgvsvItem.setEndSideKakutenName(endSideKakutenName);
			dgvsvItem.setStartSideKakutenNo(startSideKakutenNo);
			dgvsvItem.setEndSideKakutenNo(endSideKakutenNo);
			dgvsvItem.setMiddlePointNo(middlePointNo);
			dgvsvItem.setSetFace(setFace);
			dgvsvItem.setGirderVstfId(girderVstfId);
			dgvsv.append(dgvsvItem);
		}
	}
	dgvsv.sort();
	dgvsv.toCsv(ofOb);

	return JPT_OK;
}

//(�W)�����⍄��ID�o�^
JptErrorStatus AtdDbToCsv::dbToCsvGirderHstfId(
	AtdDbInputUseMaterial& ium,	//���́E�g�p�ޗ��f�[�^
	AtdDbSecHstf& shf,			//�f�ʥHSTF�f��	
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderHstfId dghi;

	for(int i=0;i<shf.size();i++) {
		AtdDbSecHstfItem shfItem ;
		shf.getAt(i, shfItem);
		int nohssc = shfItem.getNohssc();	//NOHSSC ����ID
		string girderHstfId = "HS" + sh.toString(nohssc);	//����ID
		double width = shfItem.getHssc3();	//HSSC3 ��
		double plateThickness = shfItem.getHssc5();	//HSSC5 ��
		int hssc8 = shfItem.getHssc8();		//HSSC8 �ގ�
		string material = ium.findMaterial(plateThickness, hssc8);		//�ގ�
		DiafGirderHstfIdItem dghiItem;
		dghiItem.setGirderHstfId(girderHstfId);
		dghiItem.setWidth(width);
		dghiItem.setPlateThickness(plateThickness);
		dghiItem.setMaterial(material);
		dghi.append(dghiItem);
	}
	dghi.sort();
	dghi.toCsv(ofOb);

	return JPT_OK;
}

//(�X)�����⍄�ޔz�u
JptErrorStatus AtdDbToCsv::dbToCsvGirderHstfSet(
	AtdDbSecHstfHaichi& shh,	//�f�ʥHSTF�z�u	
	AtdDbSecStfDir& ssd,		//�f�ʥ�ڍץ�⍄�ނ̌���
	AtdDbSecVstfHaichi& svh,	//�f�ʥVSTF�z�u
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderHstfSet dghs;

	for(int i=0;i<_ngMax;i++) {
		int idx = ssd.find(i+1);
		if(idx < 0) {
			continue;
		}
		AtdDbSecStfDirItem ssdItem;
		ssd.getAt(idx, ssdItem);
		int idrv = ssdItem.getIdrv();		//IDRV �z�u��
		int idrh = ssdItem.getIdrh();		//IDRH �z�u��
		if(idrh == 1) {	//1�������⍄�ނƋt
			if(idrv == 0) {
				idrv = 1;
			} else {
				idrv = 0;
			}
		}
		//for(int j=0;j<(int)_oudanNameList.size()-1;j++) {
		//	int idx = shh.find(i+1, j+1);
		//	if(idx < 0) {
		//		continue;
		//	}
		//	AtdDbSecHstfHaichiItem shhItem;
		//	shh.getAt(idx, shhItem);
		//	int nohsl1 = shhItem.getNohsl1();	//NOHSL1 �z�u�i�ԍ�
		//	int nohsl2 = shhItem.getNohsl2();	//NOHSL2 �z�u�i�ԍ�
		//	string ketaName = _ketaNameameList[i];					//����
		//	string startSideKakutenName = _oudanNameList[j];	//�n���i�_��
		//	string endSideKakutenName = _oudanNameList[j+1];	//�I���i�_��
		//	int startSideKakutenNo = j+1;		//�n���i�_�ԍ�
		//	int endSideKakutenNo = j+2;			//�I���i�_�ԍ�
		//	int kakutenIntervalNo = 0;			//�����⍄�ނ̊i�_�Ԃł̔z�u�ʒu
		//	EnFace setFace;						//�����⍄�ނ̔z�u��
		//	if(idrv == 0) {
		//		setFace = FS;		//	����
		//	} else {			//����
		//		setFace = NS;		//	�\��
		//	}
		//	DiafGirderHstfSetItem dghsItem;
		//	dghsItem.setKetaName(ketaName);
		//	dghsItem.setStartSideKakutenName(startSideKakutenName);
		//	dghsItem.setEndSideKakutenName(endSideKakutenName);
		//	dghsItem.setStartSideKakutenNo(startSideKakutenNo);
		//	dghsItem.setEndSideKakutenNo(endSideKakutenNo);
		//	dghsItem.setKakutenIntervalNo(kakutenIntervalNo);
		//	dghsItem.setSetFace(setFace);
		//	int setStepNo = 1;							//�����⍄�ނ�z�u����i�̔ԍ�
		//	string girderHstfId = "HS" + sh.toString(nohsl1);		//�z�u���鐅���⍄�ނ̎���ID
		//	dghsItem.setSetStepNo(setStepNo);
		//	dghsItem.setGirderHstfId(girderHstfId);
		//	dghs.append(dghsItem);
		//	if(nohsl2 > 0) {
		//		int setStepNo = 2;							//�����⍄�ނ�z�u����i�̔ԍ�
		//		string girderHstfId = "HS" + sh.toString(nohsl2);		//�z�u���鐅���⍄�ނ̎���ID
		//		dghsItem.setSetStepNo(setStepNo);
		//		dghsItem.setGirderHstfId(girderHstfId);
		//		dghs.append(dghsItem);
		//	}
		//}

		for(int j=0;j<(int)_oudanNameList.size()-1;j++) {
			int idx = shh.find(i+1, j+1);
			if(idx < 0) {
				continue;
			}
			// 2018/03/01 take Add Start
			int kakutenIntervalNo = 0;		//�����⍄�ނ̊i�_�Ԃł̔z�u�ʒu
			// 2018/03/01 take Add End
			AtdDbSecHstfHaichiItem shhItem;
			shh.getAt(idx, shhItem);
			for(int k = 0;k < _hstfPanelNumber.size();k++ ){
				int panel = shhItem.getNopnl();
				if( panel == _hstfPanelNumber[i][k] ){	//�Ώۂ�HSTF�z�u�p�l����
					kakutenIntervalNo += 1;
					int nohsl1 = shhItem.getNohsl1();	//NOHSL1 �z�u�i�ԍ�
					int nohsl2 = shhItem.getNohsl2();	//NOHSL2 �z�u�i�ԍ�
					string ketaName = _ketaNameameList[i];					//����
					string startSideKakutenName = _oudanNameList[j];	//�n���i�_��
					string endSideKakutenName = _oudanNameList[j+1];	//�I���i�_��
					int startSideKakutenNo = j+1;		//�n���i�_�ԍ�
					int endSideKakutenNo = j+2;			//�I���i�_�ԍ�
					EnFace setFace;						//�����⍄�ނ̔z�u��
					if(idrv == 0) {
						setFace = FS;		//	����
					} else {			//����
						setFace = NS;		//	�\��
					}
					//int prgs = //���݂̊i�_�Ԕԍ��Ɏ�t�������⍄�ނ̒i���̍��v
					for(int l=0;l<_hstfPanelUpperPrg[i][k];l++){ //��i
						
					}
					for(int l=0;l<_hstfPanelLowerPrg[i][k];l++){ //���i
						
					}
				}
				if( panel < _hstfPanelNumber[i][k] ){
					break;
				}
			}
			DiafGirderHstfSetItem dghsItem;
			//dghsItem.setKetaName(ketaName);
			//dghsItem.setStartSideKakutenName(startSideKakutenName);
			//dghsItem.setEndSideKakutenName(endSideKakutenName);
			//dghsItem.setStartSideKakutenNo(startSideKakutenNo);
			//dghsItem.setEndSideKakutenNo(endSideKakutenNo);
			//dghsItem.setKakutenIntervalNo(kakutenIntervalNo);
			//dghsItem.setSetFace(setFace);
			//int setStepNo = 1;							//�����⍄�ނ�z�u����i�̔ԍ�
			//string girderHstfId = "HS" + sh.toString(nohsl1);		//�z�u���鐅���⍄�ނ̎���ID
			//dghsItem.setSetStepNo(setStepNo);
			//dghsItem.setGirderHstfId(girderHstfId);
			//dghs.append(dghsItem);
			//if(nohsl2 > 0) {
			//	int setStepNo = 2;							//�����⍄�ނ�z�u����i�̔ԍ�
			//	string girderHstfId = "HS" + sh.toString(nohsl2);		//�z�u���鐅���⍄�ނ̎���ID
			//	dghsItem.setSetStepNo(setStepNo);
			//	dghsItem.setGirderHstfId(girderHstfId);
			//	dghs.append(dghsItem);
			//}
		}
	}
	dghs.sort();
	dghs.toCsv(ofOb);

	return JPT_OK;
}

//(�P�O)�t�����W�Y�ڌ`���`
JptErrorStatus AtdDbToCsv::dbToCsvGirderFlgSplId(
	AtdDbInputUseMaterial& ium,	//���́E�g�p�ޗ��f�[�^
	AtdDbSplCommon& scm,		//�Y�ڥ���`�Y�ڋ��ʃf�[�^
	AtdDbSplFlg& sfg,			//�Y�ڥ�e�Y�ڥ�t�����W
	AtdDbSecScaleFill& ssf,		//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��t�B���[�v���[�g
	AtdDbInputFillOption& ifo,	//���ͥ�t�B���[�v���[�g�I�v�V����
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderFlgSplId dgfsi;

	//�E�a
	double holeSize = scm.findHoleSize(3);

	//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��t�B���[�v���[�g
	int mtfill = 0;
	double rfilfu = 0.0;	//RFILFU
	double rfilfl = 0.0;	//RFILFL
	if(ssf.size() > 0) {
		AtdDbSecScaleFillItem ssfItem;
		ssf.getAt(0, ssfItem);
		mtfill = ssfItem.getMtfill();	//MTFILL Fill�ގ�
		rfilfu = ssfItem.getRfilfu();
		rfilfl = ssfItem.getRfilfl();
	}

	//���ͥ�t�B���[�v���[�g�I�v�V����
	int ifilfu = 0;	//IFILFU
	int ifilfl = 0;	//IFILFL
	if(ifo.size() > 0) {
		AtdDbInputFillOptionItem ifoItem;
		ifo.getAt(0, ifoItem);
		ifilfu = ifoItem.getIfilfu();
		ifilfl = ifoItem.getIfilfl();
	}

	//�Y�ڥ�e�Y�ڥ�t�����W
	int cnt = 0;
	for(int i=0;i<sfg.size();i++) {
		AtdDbSplFlgItem sfgItem;
		sfg.getAt(i, sfgItem);
		int strcode = sfgItem.getStrcode();	//STRCODE
		if(strcode != 3) {
			continue;
		}
		int iupdw = sfgItem.getIupdw();		//IUPDW �㉺
		int noj = sfgItem.getNoj();			//NOJ �W���C���g�ԍ�
		int ifmj = sfgItem.getIfmj();		//IFMJ �E�^�C�v FLG�E_�^�C�v
		double poj = sfgItem.getPoj();		//POJ CP
		double pj = sfgItem.getPj();		//PJ P1 P FLG�E_P
		int npj = sfgItem.getNpj();			//NPJ P1�̐�
		double p00j = sfgItem.getP00j();	//P00J P2
		int npoj = sfgItem.getNpoj();		//NPOJ P2�̐�
		double goj = sfgItem.getGoj();		//GOJ CG FLG�E_CG
		double gj = sfgItem.getGj();		//GJ G FLG�E_G
		int ngj = sfgItem.getNgj();			//NGJ G�̐�
		int b1j = sfgItem.getB1j();			//B1J SPL�^�C�v SPL_�^�C�v
		double t1j = sfgItem.getT1j();		//T1J NS�� SPL_���i�O���j SPL_���i�����j
		double t2j = sfgItem.getT2j();		//T2J FS�� CONN_��
		int mtj = sfgItem.getMtj();			//MTJ NS�ގ� FS�ގ� SPL_�ގ��i�O���j SPL_�ގ��i�����j
		cnt++;
		string girderFlgSplId = "GFS" + sh.toString(cnt);	//����ID
		EnHoleType holeType;			//�E�^�C�v
		if(ifmj == 1) {			//�i�q
			holeType = KOUSHI;			//	�i�q
		} else if(ifmj == 2) {	//�璹1
			holeType = CHIDORI1;		//	�璹1
		} else {				//�璹3
			holeType = CHIDORI3;		//	�璹3
		}
		double centerPitch = poj;					//�Z���^�[�s�b�`
		string pitch1 = makePitchGage(npj, pj);		//�s�b�`1
		string pitch2 = makePitchGage(npoj, p00j);	//�s�b�`2
		double centerGauge = goj;					//�Z���^�[�Q�[�W
		string gauge = makePitchGage(ngj, gj);		//�Q�[�W
		EnTypeNo splType;							//�Y�ڔ^�C�v
		if(b1j > 0) {	//�^�C�v1
			splType = TYPE1;
		} else {		//�^�C�v2
			splType = TYPE2;
		}
		double nsPlateThickness = t1j;		//�I���e���Y�ڔ̔�
		double fsPlateThickness = t2j;		//�E�����Y�ڔ̔�
		string nsMaterial = ium.findMaterial(nsPlateThickness, mtj);		//�I���e���Y�ڔ̍ގ�
		string fsMaterial = ium.findMaterial(fsPlateThickness, mtj);		//�E�����Y�ڔ̍ގ�
		// 2018/02/15 take Edit Start
		// �t�B���[���쐬���Ȃ��ꍇ�́A�w-1.0�x�Ƃ���
		double fillerPlateThickness = -1.0;
		//double fillerPlateThickness = 0.0;	//�t�B���[�̔�
		// 2018/02/15 take Edit End
		string fillerMaterial = "";			//�t�B���[�̍ގ�
		if((iupdw == 0 && ifilfu == 1) ||(iupdw == 1 && ifilfl == 1)) {	//�t�B���[�z�u
			int ig = (noj-1) / _jointMax;
			int ij = (noj-1) % _jointMax;
			double atsu1, atsu2;
			if(iupdw == 0) {	//��t�����W
				atsu1 = fabs(_danmenAtsuUflg[ig][ij]);
				atsu2 = fabs(_danmenAtsuUflg[ig][ij+1]);
			} else {			//���t�����W
				atsu1 = fabs(_danmenAtsuLflg[ig][ij]);
				atsu2 = fabs(_danmenAtsuLflg[ig][ij+1]);
			}
			double atsuSa = fabs(atsu1-atsu2);
			if((iupdw == 0 && rfilfu <= atsuSa) ||(iupdw == 1 && rfilfl <= atsuSa)) {	//��������
				fillerPlateThickness = getFillItaatsu(atsuSa);	//�t�B���[�̔�
				fillerMaterial = ium.findMaterial(fillerPlateThickness, mtfill);	//�t�B���[�̍ގ�
			}
		}
		DiafGirderFlgSplIdItem dgfsiItem;
		dgfsiItem.setGirderFlgSplId(girderFlgSplId);
		dgfsiItem.setHoleType(holeType);
		dgfsiItem.setHoleSize(holeSize);
		dgfsiItem.setCenterPitch(centerPitch);
		dgfsiItem.setPitch1(pitch1);
		dgfsiItem.setPitch2(pitch2);
		dgfsiItem.setCenterGauge(centerGauge);
		dgfsiItem.setGauge(gauge);
		dgfsiItem.setSplType(splType);
		dgfsiItem.setNsPlateThickness(nsPlateThickness);
		dgfsiItem.setNsMaterial(nsMaterial);
		dgfsiItem.setFsPlateThickness(fsPlateThickness);
		dgfsiItem.setFsMaterial(fsMaterial);
		dgfsiItem.setFillerPlateThickness(fillerPlateThickness);
		dgfsiItem.setFillerMaterial(fillerMaterial);
		dgfsi.append(dgfsiItem);
		if(iupdw == 0) {	//��t�����W
			_uflgIdName.push_back(girderFlgSplId);	//��t�����W���ʖ�
			_uflgJointNo.push_back(noj);	//��t�����W�W���C���g�ԍ�
		} else {			//���t�����W
			_lflgIdName.push_back(girderFlgSplId);	//���t�����W���ʖ�
			_lflgJointNo.push_back(noj);	//���t�����W�W���C���g�ԍ�
		}
	}
	dgfsi.sort();
	dgfsi.toCsv(ofOb);

	return JPT_OK;
}

//(�P�P)�E�F�u�Y�ڌ`��
JptErrorStatus AtdDbToCsv::dbToCsvGirderWebSplId(
AtdDbInputUseMaterial& ium,		//���́E�g�p�ޗ��f�[�^
	AtdDbSplCommon& scm,		//�Y�ڥ���`�Y�ڋ��ʃf�[�^
	AtdDbSplWeb& swb,			//�Y�ڥ�e�Y�ڥ�E�F�u
	AtdDbSecScaleFill& ssf,		//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��t�B���[�v���[�g
	AtdDbInputFillOption& ifo,	//���ͥ�t�B���[�v���[�g�I�v�V����
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderWebSplId dgwsi;

	//�E�a
	double holeSize = scm.findHoleSize(3);

	//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��t�B���[�v���[�g
	int mtfill = 0;
	double rfilwb = 0.0;	//RFILWB
	if(ssf.size() > 0) {
		AtdDbSecScaleFillItem ssfItem;
		ssf.getAt(0, ssfItem);
		mtfill = ssfItem.getMtfill();	//MTFILL Fill�ގ�
		rfilwb = ssfItem.getRfilwb();
	}

	//���ͥ�t�B���[�v���[�g�I�v�V����
	int ifilwb = 0;	//IFILWB
	if(ifo.size() > 0) {
		AtdDbInputFillOptionItem ifoItem;
		ifo.getAt(0, ifoItem);
		ifilwb = ifoItem.getIfilwb();
	}

	//�Y�ڥ�e�Y�ڥ�E�F�u
	int cnt = 0;
	for(int i=0;i<swb.size();i++) {
		AtdDbSplWebItem swbItem;
		swb.getAt(i, swbItem);
		int strcode = swbItem.getStrcode();	//STRCODE
		// 2018/02/15 take Add Start
		int itpwj = swbItem.getItpwj();		//ITPWJ
		// 2018/02/15 take Add End
		if(strcode != 3) {
			continue;
		}
		// 2018/02/15 take Add Start
		if(itpwj != 1) {
			continue;
		}
		// 2018/02/15 take Add End
		int nowj = swbItem.getNowj();		//NOWJ
		double gwj = swbItem.getGwj();		//GWJ P
		int ng2wj = swbItem.getNg2wj();		//NG2WJ P�̐�
		double gowj = swbItem.getGowj();	//GOWJ CP
		double p2wj = swbItem.getP2wj();	//P2WJ ����G G
		int nd2wj = swbItem.getNd2wj();		//ND2WJ ����G�̐�
		double p3wj = swbItem.getP3wj();	//P3WJ �㑤G
		int nd3wj = swbItem.getNd3wj();		//ND3WJ �㑤G�̐�
		double p0wj = swbItem.getP0wj();	//P0WJ �㑤�󂫗�
		double p00j = swbItem.getP00j();	//P00J �����󂫗�
		double t2wj = swbItem.getT2wj();	//T2WJ SPL�� SPL_��
		int mtwj = swbItem.getMtwj();		//MTWJ SPL�ގ� SPL_�ގ�
		cnt++;
		string girderWebSplId = "GWS" + sh.toString(cnt);	//����ID
		double centerPitch = gowj;			//�Z���^�[�s�b�`
		string pitch = makePitchGage(ng2wj, gwj);				//�s�b�`
		string upperGauge = makePitchGage(nd2wj, p2wj);		//�㑤�Q�[�W
		string centerGauge = makePitchGage(nd3wj, p3wj);		//�����Q�[�W
		if(centerGauge == "") {
			centerGauge = upperGauge;
			upperGauge = "";
		}
		string lowerGauge = upperGauge;		//�����Q�[�W
		double upperFreeSpace = p0wj;			//�E�F�u��[����ŏ�i�E�܂ł̋󂫗�
		double lowerFreeSpace = p00j;			//�E�F�u���[����ŉ��i�E�܂ł̋󂫗�
		double splPlateThickness = t2wj;		//�Y�ڔ̔�
		string splMaterial = ium.findMaterial(splPlateThickness, mtwj);		//�Y�ڔ̍ގ�
		// 2018/02/15 take Edit Start
		// �t�B���[���쐬���Ȃ��ꍇ�́A�w-1.0�x�Ƃ���
		double fillerPlateThickness = -1.0;
		//double fillerPlateThickness = 0.0;	//�t�B���[�̔�
		// 2018/02/15 take Edit End
		string fillerMaterial = "";			//�t�B���[�̍ގ�
		if(ifilwb == 1) {	//�t�B���[�z�u
			int ig = (nowj-1) / _jointMax;
			int ij = (nowj-1) % _jointMax;
			double atsu1 = fabs(_danmenAtsuUflg[ig][ij]);
			double atsu2 = fabs(_danmenAtsuUflg[ig][ij+1]);
			double atsuSa = fabs(atsu1-atsu2);
			if(rfilwb <= atsuSa) {	//��������
				fillerPlateThickness = getFillItaatsu(atsuSa);	//�t�B���[�̔�
				fillerMaterial = ium.findMaterial(fillerPlateThickness, mtfill);	//�t�B���[�̍ގ�
			}
		}
		DiafGirderWebSplIdItem dgwsiItem;
		dgwsiItem.setGirderWebSplId(girderWebSplId);
		dgwsiItem.setHoleSize(holeSize);
		dgwsiItem.setCenterPitch(centerPitch);
		dgwsiItem.setPitch(pitch);
		dgwsiItem.setUpperGauge(upperGauge);
		dgwsiItem.setCenterGauge(centerGauge);
		dgwsiItem.setLowerGauge(lowerGauge);
		dgwsiItem.setUpperFreeSpace(upperFreeSpace);
		dgwsiItem.setLowerFreeSpace(lowerFreeSpace);
		dgwsiItem.setSplPlateThickness(splPlateThickness);
		dgwsiItem.setSplMaterial(splMaterial);
		dgwsiItem.setFillerPlateThickness(fillerPlateThickness);
		dgwsiItem.setFillerMaterial(fillerMaterial);
		dgwsi.append(dgwsiItem);
		_webIdName.push_back(girderWebSplId);	//�E�G�u���ʖ�
		_webJointNo.push_back(nowj);			//�E�G�u�W���C���g�ԍ�
	}
	dgwsi.sort();
	dgwsi.toCsv(ofOb);

	return JPT_OK;
}

//(�P�Q)�Y�ڔz�u
JptErrorStatus AtdDbToCsv::dbToCsvGirderSplSet(
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafGirderSplSet dgss;

	for(int i=0;i<_ngMax;i++) {
		string ketaName = _ketaNameameList[i];		//����
		for(int j=0;j<_jointMax;j++) {
			string jointName = "J" + sh.toString(j+1);		//�W���C���g��
			int jointNo = j+1;				//�W���C���g�ԍ�
			int nogj = i*_jointMax + jointNo;
			string uflgSplId = findIdName(nogj, _uflgJointNo, _uflgIdName);;	//��t�����W�ɔz�u����Y�ڌ`��
			string webSplId = findIdName(nogj, _webJointNo, _webIdName);;		//�E�F�u�ɔz�u����Y�ڌ`��
			string lflgSplId = findIdName(nogj, _lflgJointNo, _lflgIdName);;	//���t�����W�ɔz�u����Y�ڌ`��
			DiafGirderSplSetItem dgssItem;
			dgssItem.setKetaName(ketaName);
			dgssItem.setJointName(jointName);
			dgssItem.setJointNo(jointNo);
			dgssItem.setUflgSplId(uflgSplId);
			dgssItem.setWebSplId(webSplId);
			dgssItem.setLflgSplId(lflgSplId);
			dgss.append(dgssItem);
		}
	}
	dgss.sort();
	dgss.toCsv(ofOb);

	return JPT_OK;
}

//�S�D����

//(�P)���ʏڍ׃f�[�^
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamCommon(
	AtdCrossBeamCommon& acc,
	AtdDbInputCbeam& icb,	//���ͥ����
	std::ofstream& ofOb)
{
	DiafCrossbeamCommon dcc;

	double shitenUflgJc = acc.getShitenUflgJc();	//�x�_�㉡���iBH�j��t�����W�̃W���C���g�N���A�����X
	double shitenWebJc = acc.getShitenWebJc();		//�x�_�㉡���iBH�j�E�F�u�̃W���C���g�N���A�����X
	double shitenLflgJc = acc.getShitenLflgJc();	//�x�_�㉡���iBH�j���t�����W�̃W���C���g�N���A�����X
	double kakutenHflgJc = acc.getKakutenHflgJc();	//�i�_�㉡���iH�|�j�t�����W�̃W���C���g�N���A�����X
	double kakutenHwebJc = acc.getKakutenHwebJc();	//�i�_�㉡���iH�|�j�E�F�u�̃W���C���g�N���A�����X
	string shitenConnCut = acc.getShitenConnCut();									//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӑ��،�
	double shitenConnFillet = acc.getShitenConnFillet();							//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̃t�B���b�g��R�T�C�Y
	double shitenConnTachiageryo = acc.getShitenConnTachiageryo();					//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӗ��グ��
	string kakutenConnCut = acc.getKakutenConnCut();								//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӑ��،����w��
	double kakutenConnFillet = acc.getKakutenConnFillet();							//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̃t�B���b�g��R�T�C�Y
	double kakutenConnTachiageryo = acc.getKakutenConnTachiageryo();				//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӗ��グ��
	string cvsCutWu = acc.getCvsCutWu();											//�����t�����⍄�ނ̗n�ڕӑ��㑤�،�
	string cvsCutWd = acc.getCvsCutWd();											//�����t�����⍄�ނ̗n�ڕӑ������،�
	double shitenUflgSplKyojikuZaitan = acc.getShitenUflgSplKyojikuZaitan();		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	double shitenUflgSplKyotyokuZaitan = acc.getShitenUflgSplKyotyokuZaitan();		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	double shitenWebSplKyotyokuZaitan = acc.getShitenWebSplKyotyokuZaitan();		//�x�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
	double shitenWebSplHeightZaitan = acc.getShitenWebSplHeightZaitan();			//�x�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
	double shitenLflgSplKyojikuZaitan = acc.getShitenLflgSplKyojikuZaitan();		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	double shitenLflgSplKyotyokuZaitan = acc.getShitenLflgSplKyotyokuZaitan();		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	double shitenConnKyojikuZaitan = acc.getShitenConnKyojikuZaitan();				//�x�_��R�l�N�V�����̋��������ޒ[
	double shitenConnKyoutyokuZaitan = acc.getShitenConnKyoutyokuZaitan();			//�x�_��R�l�N�V�����̋��������ޒ[
	double kakutenUflgSplKyojikuZaitan = acc.getKakutenUflgSplKyojikuZaitan();		//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	double kakutenUflgSplKyotyokuZaitan = acc.getKakutenUflgSplKyotyokuZaitan();	//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	double kakutenWebSplKyotyokuZaitan = acc.getKakutenWebSplKyotyokuZaitan();		//�i�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
	double kakutenWebSplHeightZaitan = acc.getKakutenWebSplHeightZaitan();			//�i�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
	double kakutenLflgSplKyojikuZaitan = acc.getKakutenLflgSplKyojikuZaitan();		//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	double kakutenLflgSplKyotyokuZaitan = acc.getKakutenLflgSplKyotyokuZaitan();	//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	double kakutenConnKyojikuZaitan = acc.getKakutenConnKyojikuZaitan();			//�i�_��R�l�N�V�����̋��������ޒ[
	double kakutenConnKyoutyokuZaitan = acc.getKakutenConnKyoutyokuZaitan();		//�i�_��R�l�N�V�����̋��������ޒ[
	double webHoleSlopeLowerLimit = acc.getWebHoleSlopeLowerLimitCrs();				//�E�F�u�E�̍E���z�̉����l
	EnTypeNo flgSectionType = acc.getFlgSectionType();								//�t�����W�،��̕���
	dcc.setShitenUflgJc(shitenUflgJc);
	dcc.setShitenWebJc(shitenWebJc);
	dcc.setShitenLflgJc(shitenLflgJc);
	dcc.setKakutenHflgJc(kakutenHflgJc);
	dcc.setKakutenHwebJc(kakutenHwebJc);
	dcc.setShitenConnCut(shitenConnCut);
	dcc.setShitenConnFillet(shitenConnFillet);
	dcc.setShitenConnTachiageryo(shitenConnTachiageryo);
	dcc.setKakutenConnCut(kakutenConnCut);
	dcc.setKakutenConnFillet(kakutenConnFillet);
	dcc.setKakutenConnTachiageryo(kakutenConnTachiageryo);
	dcc.setCvsCutWu(cvsCutWu);
	dcc.setCvsCutWd(cvsCutWd);
	dcc.setShitenUflgSplKyojikuZaitan(shitenUflgSplKyojikuZaitan);
	dcc.setShitenUflgSplKyotyokuZaitan(shitenUflgSplKyotyokuZaitan);
	dcc.setShitenWebSplKyotyokuZaitan(shitenWebSplKyotyokuZaitan);
	dcc.setShitenWebSplHeightZaitan(shitenWebSplHeightZaitan);
	dcc.setShitenLflgSplKyojikuZaitan(shitenLflgSplKyojikuZaitan);
	dcc.setShitenLflgSplKyotyokuZaitan(shitenLflgSplKyotyokuZaitan);
	dcc.setShitenConnKyojikuZaitan(shitenConnKyojikuZaitan);
	dcc.setShitenConnKyoutyokuZaitan(shitenConnKyoutyokuZaitan);
	dcc.setKakutenUflgSplKyojikuZaitan(kakutenUflgSplKyojikuZaitan);
	dcc.setKakutenUflgSplKyotyokuZaitan(kakutenUflgSplKyotyokuZaitan);
	dcc.setKakutenWebSplKyotyokuZaitan(kakutenWebSplKyotyokuZaitan);
	dcc.setKakutenWebSplHeightZaitan(kakutenWebSplHeightZaitan);
	dcc.setKakutenLflgSplKyojikuZaitan(kakutenLflgSplKyojikuZaitan);
	dcc.setKakutenLflgSplKyotyokuZaitan(kakutenLflgSplKyotyokuZaitan);
	dcc.setKakutenConnKyojikuZaitan(kakutenConnKyojikuZaitan);
	dcc.setKakutenConnKyoutyokuZaitan(kakutenConnKyoutyokuZaitan);
	dcc.setWebHoleSlopeLowerLimit(webHoleSlopeLowerLimit);
	dcc.setFlgSectionType(flgSectionType);

	dcc.toCsv(ofOb);

	return JPT_OK;
}

//(�Q)�x�_�㉡���f��ID�o�^
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamSectionId(
	AtdDbInputUseMaterial& ium,	//���́E�g�p�ޗ��f�[�^
	AtdDbSecCbeam& scb,			//�f�ʥ����
	AtdDbSecCbeamSec& scs,		//�f�ʥ������f��
	AtdDbSplWeb& swb,			//�Y�ڥ�e�Y�ڥ�E�F�u
	AtdDbLineCbeam& lcb,		//���`�����(�ΌX�\/�u���P�b�g)
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamSectionId dcsi;

	int cnt = 0;
	for(int i=0;i<scs.size();i++) {
		AtdDbSecCbeamSecItem scsItem;
		scs.getAt(i, scsItem);
		int nocrs = scsItem.getNocrs();	//NOCRS
		double bfcu = scsItem.getBfcu();	//BFCU UFLG_�� H�|�t�����W��
		double tfcu = scsItem.getTfcu();	//TFCU UFLG_�� H�|�t�����W��
		int mfcu = scsItem.getMfcu();		//MFCU UFLG_�ގ� H�|�ގ�
		double tfcw = scsItem.getTfcw();	//TFCW WEB_�� H�|�E�F�u��
		// 2018/02/15 take Edit Start
		// MFCW�̈ꌅ�ڂ�WEB�ގ�
		int mfcw = scsItem.getMfcw();
		mfcw = mfcw % 10;
		//int mfcw = scsItem.getMfcw();		//MFCW WEB_�ގ�
		// 2018/02/15 take Edit End
		double bfcl = scsItem.getBfcl();	//BFCL LFLG_�� H�|�t�����W��
		double tfcl = scsItem.getTfcl();	//TFCL LFLG_�� H�|�t�����W��
		int mfcl = scsItem.getMfcl();		//MFCL LFLG_�ގ� H�|�ގ�
		if(tfcu < 0.0 || tfcl < 0.0) {
			continue;
		}
		double idx = scb.find(nocrs);
		if(idx < 0) {
			continue;
		}

		AtdDbSecCbeamItem scbItem;
		scb.getAt(idx, scbItem);
		int nocjul = scbItem.getNocjul();	//NOCJUL:��t�����W�����̓Y�ڔԍ�
		int nocjll = scbItem.getNocjll();	//NOCJLL:���t�����W�����̓Y�ڔԍ�
		int nocjwl = scbItem.getNocjwl();	//NOCJWL:�E�F�u�����̓Y�ڔԍ�
		int nocjur = scbItem.getNocjur();	//NOCJUR:��t�����W�E���̓Y�ڔԍ�
		int nocjlr = scbItem.getNocjlr();	//NOCJLR:���t�����W�E���̓Y�ڔԍ�
		int nocjwr = scbItem.getNocjwr();	//NOCJWR:�E�F�u�E���̓Y�ڔԍ�
		int itpwj1 = swb.findItpwj(nocjwl);
		int itpwj2 = swb.findItpwj(nocjwr);
		if(itpwj1 == -1 || itpwj1 == 3 || itpwj2 == -1 || itpwj2 == 3) {
			continue;
		}
		_habaUflg.push_back(bfcu);	//UFLG_��
		_atsuUflg.push_back(tfcu);	//UFLG_��
		_zaiUflg.push_back(mfcu);	//UFLG_�ގ�
		_zaiLflg.push_back(mfcl);	//LFLG_�ގ�
		_atsuWeb.push_back(tfcw);	//WEB_��
		_atsuLflg.push_back(tfcl);	//LFLG_��
		_crsNocjul.push_back(nocjul);
		_crsNocjll.push_back(nocjll);
		_crsNocjwl.push_back(nocjwl);
		_crsNocjur.push_back(nocjur);
		_crsNocjlr.push_back(nocjlr);
		_crsNocjwr.push_back(nocjwr);
		//
		double atsuVstf = 0.0;
		for(int ii=0;ii<_ngMax-1;ii++) {
			string leftKetaName = _ketaNameameList[ii];		//��������
			string rightKetaName = _ketaNameameList[ii+1];	//�E������
			for(int jj=0;jj<(int)_oudanNameList.size();jj++) {
				int nosec = lcb.findNosec(ii+1, ii+2, jj+1);
				if(nocrs == nosec) {
					for(int k=0;k<(int)_grdKakuVstfKakutenNo.size();k++) {
						if(_grdKakuVstfKakutenNo[k] == jj+1 && ((_grdKakuVstfKetaNo[k] == ii+1 && _grdKakuVstfFace[k] == NS) || (_grdKakuVstfKetaNo[k] == ii+2 && _grdKakuVstfFace[k] == FS))) {
							atsuVstf = _grdKakuVstfitaastu[k];
							break;
						}
					}
				}
			}
		}
		_atsuTsukiVatf.push_back(atsuVstf);
		//
		cnt++;
		string crossbeamSectionId = "BH" + sh.toString(cnt);				//����ID
		double uflgWidth = bfcu;											//��t�����W��
		double uflgPlateThickness = tfcu;									//��t�����W��
		string uflgMaterial = ium.findMaterial(uflgPlateThickness, mfcu);	//��t�����W�ގ�
		double webPlateThickness = fabs(tfcw);								//�E�F�u��
		string webMaterial = ium.findMaterial(webPlateThickness, mfcw);		//�E�F�u�ގ�
		double lflgWidth = bfcl;											//���t�����W��
		double lflgPlateThickness = tfcl;									//���t�����W��
		string lflgMaterial = ium.findMaterial(lflgPlateThickness, mfcl);	//���t�����W�ގ�
		DiafCrossbeamSectionIdItem dcsiItem;
		dcsiItem.setCrossbeamSectionId(crossbeamSectionId);
		dcsiItem.setUflgWidth(uflgWidth);
		dcsiItem.setUflgPlateThickness(uflgPlateThickness);
		dcsiItem.setUflgMaterial(uflgMaterial);
		dcsiItem.setWebPlateThickness(webPlateThickness);
		dcsiItem.setWebMaterial(webMaterial);
		dcsiItem.setLflgWidth(lflgWidth);
		dcsiItem.setLflgPlateThickness(lflgPlateThickness);
		dcsiItem.setLflgMaterial(lflgMaterial);
		dcsi.append(dcsiItem);
		//�����r���h��
		int shiguchiType;
		if(tfcw > 0.0) {	//�d���^�C�v
			shiguchiType = 0;
		} else {			//CONN�^�C�v
			shiguchiType = 1;
		}
		_crsHontaiType.push_back(0);	//�����{�̃^�C�v(0:�r���h�� 1:�g�|)
		_crsShiguchiType.push_back(shiguchiType);	//�����d���^�C�v(0:�d�� 1:CONN)
		_crsbeamNocrs.push_back(nocrs);	//�����{�̎d���̔ԍ�
		_crsbeamIdname.push_back(crossbeamSectionId);	//�����{�̎d�����ʖ�
	}
	dcsi.sort();
	dcsi.toCsv(ofOb);

	return JPT_OK;
}

//(�R)�i�_�㉡���g�|ID�o�^
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamHbeamId(
AtdDbInputUseMaterial& ium,	//���́E�g�p�ޗ��f�[�^
	AtdDbSecCbeam& scb,		//�f�ʥ����
	AtdDbSecCbeamSec& scs,	//�f�ʥ������f��
	AtdDbSplWeb& swb,		//�Y�ڥ�e�Y�ڥ�E�F�u
	AtdDbLineCbeam& lcb,	//���`�����(�ΌX�\/�u���P�b�g)
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamHbeamId dchi;

	int cnt = 0;
	for(int i=0;i<scs.size();i++) {
		AtdDbSecCbeamSecItem scsItem;
		scs.getAt(i, scsItem);
		int nocrs = scsItem.getNocrs();		//NOCRS
		double bfcu = scsItem.getBfcu();	//BFCU UFLG_�� H�|�t�����W��
		double tfcu = scsItem.getTfcu();	//TFCU UFLG_�� H�|�t�����W��
		int mfcu = scsItem.getMfcu();		//MFCU UFLG_�ގ� H�|�ގ�
		double tfcw = scsItem.getTfcw();	//TFCW WEB_�� H�|�E�F�u��
		if(tfcu > 0.0) {
			continue;
		}
		double idx = scb.find(nocrs);
		if(idx < 0) {
			continue;
		}
		AtdDbSecCbeamItem scbItem;
		scb.getAt(idx, scbItem);
		int nocjul = scbItem.getNocjul();	//NOCJUL:��t�����W�����̓Y�ڔԍ�
		int nocjll = scbItem.getNocjll();	//NOCJLL:���t�����W�����̓Y�ڔԍ�
		int nocjwl = scbItem.getNocjwl();	//NOCJWL:�E�F�u�����̓Y�ڔԍ�
		int nocjur = scbItem.getNocjur();	//NOCJUR:��t�����W�E���̓Y�ڔԍ�
		int nocjlr = scbItem.getNocjlr();	//NOCJLR:���t�����W�E���̓Y�ڔԍ�
		int nocjwr = scbItem.getNocjwr();	//NOCJWR:�E�F�u�E���̓Y�ڔԍ�
		int itpwj1 = swb.findItpwj(nocjwl);
		int itpwj2 = swb.findItpwj(nocjwr);
		if(itpwj1 == -1 || itpwj1 == 3 || itpwj2 == -1 || itpwj2 == 3) {
			continue;
		}
		_habaUflg.push_back(bfcu);	//UFLG_��
		_atsuUflg.push_back(tfcu);	//UFLG_��
		_zaiUflg.push_back(mfcu);	//UFLG_�ގ�
		_zaiLflg.push_back(mfcu);	//LFLG_�ގ�
		_atsuWeb.push_back(tfcw);	//WEB_��
		_atsuLflg.push_back(tfcu);	//LFLG_��
		_crsNocjul.push_back(nocjul);
		_crsNocjll.push_back(nocjll);
		_crsNocjwl.push_back(nocjwl);
		_crsNocjur.push_back(nocjur);
		_crsNocjlr.push_back(nocjlr);
		_crsNocjwr.push_back(nocjwr);
		//
		double atsuVstf = 0.0;
		for(int ii=0;ii<_ngMax-1;ii++) {
			string leftKetaName = _ketaNameameList[ii];		//��������
			string rightKetaName = _ketaNameameList[ii+1];	//�E������
			for(int jj=0;jj<(int)_oudanNameList.size();jj++) {
				int nosec = lcb.findNosec(ii+1, ii+2, jj+1);
				if(nocrs == nosec) {
					for(int k=0;k<(int)_grdKakuVstfKakutenNo.size();k++) {
						if(_grdKakuVstfKakutenNo[k] == jj+1 && ((_grdKakuVstfKetaNo[k] == ii+1 && _grdKakuVstfFace[k] == NS) || (_grdKakuVstfKetaNo[k] == ii+2 && _grdKakuVstfFace[k] == FS))) {
							atsuVstf = _grdKakuVstfitaastu[k];
							break;
						}
					}
				}
			}
		}
		_atsuTsukiVatf.push_back(atsuVstf);
		//
		cnt++;
		string crossbeamHbeamId = "RH" + sh.toString(cnt);	//����ID
		double hbeamWebHeight = scb.findWebHeight(nocrs);;	//H�|�E�F�u��
		double hbeamFlgWidth = bfcu;						//H�|�t�����W��
		double hbeamWebPlateThickness = fabs(tfcw);			//H�|�E�F�u��
		double hbeamFlgPlateThickness = fabs(tfcu);			//H�|�t�����W��
		string hbeamMaterial = ium.findMaterial(hbeamWebPlateThickness, mfcu);			//H�|�ގ�
		DiafCrossbeamHbeamIdItem dchiItem;
		dchiItem.setCrossbeamHbeamId(crossbeamHbeamId);
		dchiItem.setHbeamWebHeight(hbeamWebHeight);
		dchiItem.setHbeamFlgWidth(hbeamFlgWidth);
		dchiItem.setHbeamWebPlateThickness(hbeamWebPlateThickness);
		dchiItem.setHbeamFlgPlateThickness(hbeamFlgPlateThickness);
		dchiItem.setHbeamMaterial(hbeamMaterial);
		dchi.append(dchiItem);
		//�����g�|
		int shiguchiType;
		if(tfcw > 0.0) {	//�d���^�C�v(�ʏ�͗L�蓾�Ȃ�)
			shiguchiType = 0;
		} else {			//CONN�^�C�v
			shiguchiType = 1;
		}_crsHontaiType.push_back(1);				//�����{�̃^�C�v(0:�r���h�� 1:�g�|)
		_crsShiguchiType.push_back(shiguchiType);	//�����d���^�C�v(0:�d�� 1:CONN)
		_crsbeamNocrs.push_back(nocrs);				//�����{�̎d���̔ԍ�
		_crsbeamIdname.push_back(crossbeamHbeamId);	//�����{�̎d�����ʖ�
	}
	dchi.sort();
	dchi.toCsv(ofOb);

	return JPT_OK;
}

//(�S)�x�_�㉡���t�����W�Y�ڌ`���`
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamFlgSplId(
AtdDbInputUseMaterial& ium,	//���́E�g�p�ޗ��f�[�^
	AtdDbSplCommon& scm,	//�Y�ڥ���`�Y�ڋ��ʃf�[�^
	AtdDbSplFlg& sfg,		//�Y�ڥ�e�Y�ڥ�t�����W
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamFlgSplId dcfsi;

	//�E�a
	double holeSize = scm.findHoleSize(4);

	//�Y�ڥ�e�Y�ڥ�t�����W
	int cnt = 0;
	for(int i=0;i<sfg.size();i++) {
		AtdDbSplFlgItem sfgItem;
		sfg.getAt(i, sfgItem);
		int strcode = sfgItem.getStrcode();	//STRCODE
		if(strcode != 4) {
			continue;
		}
		int pos = sfgItem.getPos();		//POS
		int iupdw = sfgItem.getIupdw();	//IUPDW �㉺
		int noj = sfgItem.getNoj();		//NOJ �W���C���g�ԍ�
		int ifmj = sfgItem.getIfmj();	//IFMJ �E�^�C�v FLG�E_�^�C�v
		double poj = sfgItem.getPoj();	//POJ CP
		double pj = sfgItem.getPj();	//PJ P1 P FLG�E_P
		int npj = sfgItem.getNpj();		//NPJ P1�̐�
		double goj = sfgItem.getGoj();	//GOJ CG FLG�E_CG
		double gj = sfgItem.getGj();	//GJ G FLG�E_G
		int ngj = sfgItem.getNgj();		//NGJ G�̐�
		int b1j = sfgItem.getB1j();		//B1J SPL�^�C�v SPL_�^�C�v
		double t1j = sfgItem.getT1j();	//T1J NS�� SPL_���i�O���j SPL_���i�����j
		double t2j = sfgItem.getT2j();	//T2J FS�� CONN_��
		int mtj = sfgItem.getMtj();		//MTJ NS�ގ� FS�ގ� SPL_�ގ��i�O���j SPL_�ގ��i�����j
		if(iupdw == 1) {	//�z�u�͏㉺���ʂȂ̂ŏ㑤�̂ݓo�^
			continue;
		}
		bool flag = false;
		for(int k=0;k<_crsNocjul.size();k++) {
			if(_crsShiguchiType[k] == 1) {
				continue;
			}
			if(iupdw == 0) {	//��
				if(_crsNocjul[k] == noj || _crsNocjur[k] == noj) {
					flag = true;
					break;
				}
			} else {			//��
				if(_crsNocjll[k] == noj || _crsNocjlr[k] == noj) {
					flag = true;
					break;
				}
			}
		}
		if(flag != true) {
			continue;
		}
		cnt++;
		string crossbeamFlgSplId = "CFS" + sh.toString(cnt);	//����ID
		EnHoleType holeType;			//�E�^�C�v
		if(ifmj == 1) {			//�i�q
			holeType = KOUSHI;			//	�i�q
		} else if(ifmj == 2) {	//�璹1
			holeType = CHIDORI1;		//	�璹1
		} else {				//�璹3
			holeType = CHIDORI3;		//	�璹3
		}
		double centerPitch = poj;				//�Z���^�[�s�b�`
		string pitch = makePitchGage(npj, pj);					//�s�b�`
		double centerGauge = goj;				//�Z���^�[�Q�[�W
		string gauge = makePitchGage(ngj, gj);					//�Q�[�W
		EnTypeNo splType;				//�Y�ڔ^�C�v
		if(b1j > 0) {	//�^�C�v1
			splType = TYPE1;
		} else {		//�^�C�v2
			splType = TYPE2;
		}
		double nsPlateThickness = t1j;			//�I���e���Y�ڔ̔�
		double fsPlateThickness = t2j;			//�E�����Y�ڔ̔�
		string nsMaterial = ium.findMaterial(nsPlateThickness, mtj);			//�I���e���Y�ڔ̍ގ�
		string fsMaterial = ium.findMaterial(fsPlateThickness, mtj);			//�E�����Y�ڔ̍ގ�
		double fillerPlateThickness = 0.0;	//�t�B���[�̔�(�{�̂Ǝd���������Ȃ̂Ńt�B���[�Ȃ�)
		string fillerMaterial = "";		//�t�B���[�̍ގ�(�{�̂Ǝd���������Ȃ̂Ńt�B���[�Ȃ�)
		DiafCrossbeamFlgSplIdItem dcfsiItem;
		dcfsiItem.setCrossbeamFlgSplId(crossbeamFlgSplId);
		dcfsiItem.setHoleType(holeType);
		dcfsiItem.setHoleSize(holeSize);
		dcfsiItem.setCenterPitch(centerPitch);
		dcfsiItem.setPitch(pitch);
		dcfsiItem.setCenterGauge(centerGauge);
		dcfsiItem.setGauge(gauge);
		dcfsiItem.setSplType(splType);
		dcfsiItem.setNsPlateThickness(nsPlateThickness);
		dcfsiItem.setNsMaterial(nsMaterial);
		dcfsiItem.setFsPlateThickness(fsPlateThickness);
		dcfsiItem.setFsMaterial(fsMaterial);
		dcfsiItem.setFillerPlateThickness(fillerPlateThickness);
		dcfsiItem.setFillerMaterial(fillerMaterial);
		dcfsi.append(dcfsiItem);
		_crsFsplNoj.push_back(noj);
		_crsFsplPos.push_back(pos);						//0:�[�x�_�A1:���Ԏx�_�A2:�i�_
		_crsFsplPupdw.push_back(iupdw);					//0:�� 1:��
		_crsFsplIdname.push_back(crossbeamFlgSplId);	//�����t�����W�Y�ڌ`�󎯕ʖ�
	}
	dcfsi.sort();
	dcfsi.toCsv(ofOb);

	return JPT_OK;
}

//(�T)�R�l�N�V�����E�t�����W�Y�ڌ`���`
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamConnFsplId(
AtdDbInputUseMaterial& ium,			//���́E�g�p�ޗ��f�[�^
	AtdDbSplCommon& scm,			//�Y�ڥ���`�Y�ڋ��ʃf�[�^
	AtdDbSplFlg& sfg,				//�Y�ڥ�e�Y�ڥ�t�����W
	AtdDbSecScaleFill& ssf,			//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��t�B���[�v���[�g
	AtdDbInputFillOption& ifo,		//���ͥ�t�B���[�v���[�g�I�v�V����
	AtdDbInputCbeamConnSpl& icc,	//���́E�����R�l�N�V�����Y��
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamConnFsplId dccfi;

	//�E�a
	double holeSize = scm.findHoleSize(4);

	//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��t�B���[�v���[�g
	int mtfill = 0;
	double rfilfu = 0.0;	//RFILFU
	double rfilfl = 0.0;	//RFILFL
	if(ssf.size() > 0) {
		AtdDbSecScaleFillItem ssfItem;
		ssf.getAt(0, ssfItem);
		mtfill = ssfItem.getMtfill();	//MTFILL Fill�ގ�
		rfilfu = ssfItem.getRfilfu();
		rfilfl = ssfItem.getRfilfl();
	}

	//���ͥ�t�B���[�v���[�g�I�v�V����
	int ifilfu = 0;	//IFILFU
	int ifilfl = 0;	//IFILFL
	if(ifo.size() > 0) {
		AtdDbInputFillOptionItem ifoItem;
		ifo.getAt(0, ifoItem);
		ifilfu = ifoItem.getIfilfu();
		ifilfl = ifoItem.getIfilfl();
	}

	//�Y�ڥ�e�Y�ڥ�t�����W
	int cnt = 0;
	for(int i=0;i<sfg.size();i++) {
		AtdDbSplFlgItem sfgItem;
		sfg.getAt(i, sfgItem);
		int strcode = sfgItem.getStrcode();	//STRCODE
		if(strcode != 4) {
			continue;
		}
		int pos = sfgItem.getPos();		//POS
		int iupdw = sfgItem.getIupdw();	//IUPDW �㉺
		int noj = sfgItem.getNoj();		//NOJ �W���C���g�ԍ�
		int ifmj = sfgItem.getIfmj();	//IFMJ �E�^�C�v FLG�E_�^�C�v
		double pj = sfgItem.getPj();	//PJ P1 P FLG�E_P
		int npj = sfgItem.getNpj();		//NPJ P1�̐�
		double goj = sfgItem.getGoj();	//GOJ CG FLG�E_CG
		double gj = sfgItem.getGj();	//GJ G FLG�E_G
		int ngj = sfgItem.getNgj();		//NGJ G�̐�
		int b1j = sfgItem.getB1j();		//B1J SPL�^�C�v SPL_�^�C�v
		double t1j = sfgItem.getT1j();	//T1J NS�� SPL_���i�O���j SPL_���i�����j
		double t2j = sfgItem.getT2j();	//T2J FS�� CONN_��
		int mtj = sfgItem.getMtj();		//MTJ NS�ގ� FS�ގ� SPL_�ގ��i�O���j SPL_�ގ��i�����j
		double gjc = sfgItem.getGjc();	//GJC CONN�E_���@X
		int ngjc = sfgItem.getNgjc();	//NGJC CONN�E_���@X�̐�
		double pjc = sfgItem.getPjc();	//PJC CONN�E_���@Y
		int npjc = sfgItem.getNpjc();	//NPJC CONN�E_���@Y�̐�
		if(iupdw == 1) {	//�z�u�͏㉺���ʂȂ̂ŏ㑤�̂ݓo�^
			continue;
		}
		int kk = 0;
		bool flag = false;
		for(int k=0;k<_crsNocjul.size();k++) {
			if(_crsShiguchiType[k] == 0) {
				continue;
			}
			if(iupdw == 0) {	//��
				if(_crsNocjul[k] == noj || _crsNocjur[k] == noj) {
					flag = true;
					kk = k;
					break;
				}
			} else {			//��
				if(_crsNocjll[k] == noj || _crsNocjlr[k] == noj) {
					flag = true;
					kk = k;
					break;
				}
			}
		}
		if(flag != true) {
			continue;
		}
		cnt++;
		string crossbeamConnFsplId = "CCS" + sh.toString(cnt);	//����ID
		double connPlateThickness = t2j;						//�R�l�N�V������
		double edgec = 0.0;
		double gagec = 0.0;
		double gagecenc = 0.0;
		int idx = icc.find(noj, iupdw);	//���́E�����R�l�N�V�����Y��
		if(idx > -1) {
			AtdDbInputCbeamConnSplItem iccItem;
			icc.getAt(idx, iccItem);
			edgec = iccItem.getEdgec();			//EDGEC ConnPL�A���O�����[
			gagec = iccItem.getGagec();			//GAGEC ConnPL�Q�[�W
			gagecenc = iccItem.getGagecenc();	//GAGECENC ConnPL���S�Q�[�W
		} else {
//err
		}
		double atsuVstf = fabs(_atsuTsukiVatf[kk]);	//�˂����킹VSTF��
		double connWidth = edgec + gagec + (gagecenc/2 + atsuVstf/2);				//�R�l�N�V������
//		string connMaterial = ium.findMaterial(connPlateThickness, _zaiUflg[kk]);	//�R�l�N�V�����ގ�
		string connMaterial = ium.findMaterial(_atsuUflg[kk], _zaiUflg[kk]);		//�R�l�N�V�����ގ�
		EnHoleType flgHoleType;			//�E�^�C�v
		// 2018/02/15 take Edit Start
		// �t�����W�̍E�^�C�v�́A�i�q�����璹
		if(ifmj == 1) {			//�i�q
			flgHoleType = KOUSHI;		//�i�q
		} else if(ifmj == 2) {	//���璹
			flgHoleType = UCHICHIDORI;	//���璹
		}
		//if(ifmj == 1) {			//�i�q
		//	flgHoleType = KOUSHI;		//�i�q
		//} else if(ifmj == 2) {	//�璹1
		//	flgHoleType = CHIDORI1;		//�璹1
		//} else {				//�璹3
		//	flgHoleType = CHIDORI3;		//�璹3
		//}
		// 2018/02/15 take Edit End
		string flgHolePitch = makePitchGage(npj, pj);		//�t�����W�E�̃s�b�`
		string flgHoleGauge = makePitchGage(ngj, gj);		//�t�����W�E�̃Q�[�W
		double flgHoleCenterGauge = goj;					//�t�����W�E�̃Z���^�[�Q�[�W
		string connHoleDimX = makePitchGage(ngjc, gjc);		//�R�l�N�V�����E�̐��@�iX�����j
		string connHoleDimY = makePitchGage(npjc, pjc);		//�R�l�N�V�����E�̐��@�iY�����j
		EnTypeNo splType;				//�Y�ڔ^�C�v
		if(b1j > 0) {	//�^�C�v1
			splType = TYPE1;
		} else {		//�^�C�v2
			splType = TYPE2;
		}
		double splPlateThicknessOutside = t1j;		//�Y�ڔ̔��i�O���j
		double splPlateThicknessInside = t1j;		//�Y�ڔ̔��i�����j
		string splMaterialOutside = ium.findMaterial(splPlateThicknessOutside, mtj);	//�Y�ڔ̍ގ��i�O���j
		string splMaterialInside = ium.findMaterial(splPlateThicknessInside, mtj);		//�Y�ڔ̍ގ��i�����j
		// 2018/02/15 take Edit Start
		// �t�B���[���쐬���Ȃ��ꍇ�́A�w-1.0�x�Ƃ���
		double fillerPlateThickness = -1.0;
		//double fillerPlateThickness = 0.0;	//�t�B���[�̔�
		// 2018/02/15 take Edit End
		string fillerMaterial = "";			//�t�B���[�̍ގ�
		if((iupdw == 0 && ifilfu == 1) ||(iupdw == 1 && ifilfl == 1)) {	//�t�B���[�z�u
			double atsu1 = fabs(_atsuUflg[kk]);			//UFLG_��
			double atsu2 = fabs(connPlateThickness); 	//�R�l�N�V������
			double atsuSa = fabs(atsu1-atsu2);
			if((iupdw == 0 && rfilfu <= atsuSa) ||(iupdw == 1 && rfilfl <= atsuSa)) {	//��������
				fillerPlateThickness = getFillItaatsu(atsuSa);						//�t�B���[�̔�
				fillerMaterial = ium.findMaterial(fillerPlateThickness, mtfill);	//�t�B���[�̍ގ�
			}
		}
		DiafCrossbeamConnFsplIdItem dccfiItem;
		dccfiItem.setCrossbeamConnFsplId(crossbeamConnFsplId);
		dccfiItem.setConnPlateThickness(connPlateThickness);
		dccfiItem.setConnWidth(connWidth);
		dccfiItem.setConnMaterial(connMaterial);
		dccfiItem.setHoleSize(holeSize);
		dccfiItem.setFlgHoleType(flgHoleType);
		dccfiItem.setFlgHolePitch(flgHolePitch);
		dccfiItem.setFlgHoleGauge(flgHoleGauge);
		dccfiItem.setFlgHoleCenterGauge(flgHoleCenterGauge);
		dccfiItem.setConnHoleDimX(connHoleDimX);
		dccfiItem.setConnHoleDimY(connHoleDimY);
		dccfiItem.setSplType(splType);
		dccfiItem.setSplPlateThicknessOutside(splPlateThicknessOutside);
		dccfiItem.setSplMaterialOutside(splMaterialOutside);
		dccfiItem.setSplPlateThicknessInside(splPlateThicknessInside);
		dccfiItem.setSplMaterialInside(splMaterialInside);
		dccfiItem.setFillerPlateThickness(fillerPlateThickness);
		dccfiItem.setFillerMaterial(fillerMaterial);
		dccfi.append(dccfiItem);
		_crsConnNoj.push_back(noj);
		_crsConnPos.push_back(pos);		//0:�[�x�_�A1:���Ԏx�_�A2:�i�_
		_crsConnPupdw.push_back(iupdw);		//0:�� 1:��
		_crsConnIdname.push_back(crossbeamConnFsplId);	//�����R�l�N�V�����E�t�����W�Y�ڌ`�󎯕ʖ�
	}
	dccfi.sort();
	dccfi.toCsv(ofOb);

	return JPT_OK;
}

//(�U)�E�F�u�Y�ڌ`���`
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamWsplId(
AtdDbInputUseMaterial& ium,		//���́E�g�p�ޗ��f�[�^
	AtdDbSplCommon& scm,		//�Y�ڥ���`�Y�ڋ��ʃf�[�^
	AtdDbSplWeb& swb,			//�Y�ڥ�e�Y�ڥ�E�F�u
	AtdDbSecScaleFill& ssf,		//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��t�B���[�v���[�g
	AtdDbInputFillOption& ifo,	//���ͥ�t�B���[�v���[�g�I�v�V����
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamWsplId dcwi;

	//�E�a
	double holeSize = scm.findHoleSize(4);

	//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��t�B���[�v���[�g
	int mtfill = 0;
	double rfilwb = 0.0;	//RFILWB
	if(ssf.size() > 0) {
		AtdDbSecScaleFillItem ssfItem;
		ssf.getAt(0, ssfItem);
		mtfill = ssfItem.getMtfill();	//MTFILL Fill�ގ�
		rfilwb = ssfItem.getRfilwb();
	}

	//���ͥ�t�B���[�v���[�g�I�v�V����
	int ifilwb = 0;	//IFILWB
	if(ifo.size() > 0) {
		AtdDbInputFillOptionItem ifoItem;
		ifo.getAt(0, ifoItem);
		ifilwb = ifoItem.getIfilwb();
	}

	//�Y�ڥ�e�Y�ڥ�E�F�u
	int cnt = 0;
	for(int i=0;i<swb.size();i++) {
		AtdDbSplWebItem swbItem;
		swb.getAt(i, swbItem);
		int strcode = swbItem.getStrcode();	//STRCODE
		if(strcode != 4) {
			continue;
		}
		int pos = swbItem.getPos();	//POS
		int nowj = swbItem.getNowj();	//NOWJ
		double gwj = swbItem.getGwj();	//GWJ P
		int ng2wj = swbItem.getNg2wj();		//NG2WJ P�̐�
		double gowj = swbItem.getGowj();	//GOWJ CP
		double p2wj = swbItem.getP2wj();	//P2WJ �㑤G G
		int nd2wj = swbItem.getNd2wj();		//ND2WJ �㑤G�̐�
		double p0wj = swbItem.getP0wj();	//P0WJ �㑤�󂫗�
		double p00j = swbItem.getP00j();	//P00J �����󂫗�
		double t2wj = swbItem.getT2wj();	//T2WJ SPL�� SPL_��
		int mtwj = swbItem.getMtwj();		//MTWJ SPL�ގ� SPL_�ގ�
		int kk;
		bool flag = false;
		for(int k=0;k<_crsNocjwl.size();k++) {
			if(_crsNocjwl[k] == nowj || _crsNocjwr[k] == nowj) {
				flag = true;
				kk = k;
				break;
			}
		}
		if(flag != true) {
			continue;
		}
		//
		cnt++;
		string crossbeamWsplId = "CWS" + sh.toString(cnt);	//����ID
		double centerPitch = gowj;			//�E�F�u�E�̃Z���^�[�s�b�`
		string pitch = makePitchGage(ng2wj, gwj);				//�E�F�u�E�̃s�b�`
		string gauge = makePitchGage(nd2wj, p2wj);				//�E�F�u�E�̃Q�[�W
		double upperFreeSpace = p0wj;			//�E�F�u��[����ŏ�i�E�܂ł̋󂫗�
		double lowerFreeSpace = p00j;			//�E�F�u���[����ŉ��i�E�܂ł̋󂫗�
		double splPlateThickness = t2wj;		//�Y�ڔ̔�
		string splMaterial = ium.findMaterial(splPlateThickness, mtwj);		//�Y�ڔ̍ގ�
		// 2018/02/16 take Edit Start
		// �t�B���[���쐬���Ȃ��ꍇ�́A�w-1.0�x�Ƃ���
		double fillerPlateThickness = -1.0;
		//double fillerPlateThickness = 0.0;	//�t�B���[�̔�
		// 2018/02/16 take Edit End
		string fillerMaterial = "";	//�t�B���[�̍ގ�
		if(ifilwb == 1) {	//�t�B���[�z�u
			double atsu1 = fabs(_atsuWeb[kk]);			//WEB_��
			double atsu2 = fabs(_atsuTsukiVatf[kk]);	//�˂����킹VSTF��
			double atsuSa = fabs(atsu1-atsu2);
			if(rfilwb <= atsuSa) {	//��������
				fillerPlateThickness = getFillItaatsu(atsuSa);						//�t�B���[�̔�
				fillerMaterial = ium.findMaterial(fillerPlateThickness, mtfill);	//�t�B���[�̍ގ�
			}
		}
		DiafCrossbeamWsplIdItem dcwiItem;
		dcwiItem.setCrossbeamWsplId(crossbeamWsplId);
		dcwiItem.setHoleSize(holeSize);
		dcwiItem.setCenterPitch(centerPitch);
		dcwiItem.setPitch(pitch);
		dcwiItem.setGauge(gauge);
		dcwiItem.setUpperFreeSpace(upperFreeSpace);
		dcwiItem.setLowerFreeSpace(lowerFreeSpace);
		dcwiItem.setSplPlateThickness(splPlateThickness);
		dcwiItem.setSplMaterial(splMaterial);
		dcwiItem.setFillerPlateThickness(fillerPlateThickness);
		dcwiItem.setFillerMaterial(fillerMaterial);
		dcwi.append(dcwiItem);
		_crsWsplNowj.push_back(nowj);
		_crsWsplPos.push_back(pos);		//0:�[�x�_�A1:���Ԏx�_�A2:�i�_
		_crsWsplIdname.push_back(crossbeamWsplId);	//�����E�F�u�Y�ڔ��ʖ�
	}
	dcwi.sort();
	dcwi.toCsv(ofOb);

	return JPT_OK;
}

//(�V)�����⍄��ID�o�^
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamVstfId(
	AtdDbInputUseMaterial& ium,	//���́E�g�p�ޗ��f�[�^
	AtdDbSecCbeamStf& sct	,	//�f�ʥ������⍄�ޒf�ʐ�
	AtdDbSecCbeamVstfNum& scv,	//�f�ʥ������p�l������VSTF�{��
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamVstfId dcvi;

	int cnt = 0;
	for(int i=0;i<sct.size();i++) {
		AtdDbSecCbeamStfItem sctItem;
		sct.getAt(i, sctItem);
		int nocrs = sctItem.getNocrs();		//NOCRS
		int posstf = sctItem.getPosstf();	//POSSTF
		double stcw = sctItem.getStcw();	//STCW ��
		double stct = sctItem.getStct();	//STCT ��
		int stcm = sctItem.getStcm();		//STCM �ގ�
		if(posstf != 1 && posstf != 2) {
			continue;
		}
		if(stcw < 1.0 || stct < 1.0) {
			continue;
		}
		string cBramId = findIdName(nocrs, _crsbeamNocrs, _crsbeamIdname);
		if(cBramId == "") {
			continue;
		}
		int nvstc = scv.findNvstc(nocrs);	//�f�ʥ������p�l������VSTF�{��
		if(nvstc < 1) {
			continue;
		}
		cnt++;
		string crossbeamVstfId = "CVS" + sh.toString(cnt);	//����ID
		EnTypeNo crossbeamVstfType = TYPE1;	//�����⍄�ނ̃^�C�v
		double uflgHaba = findDouble(nocrs, _crsbeamNocrs, _habaUflg);
		double webAtsu = findDouble(nocrs, _crsbeamNocrs, _atsuWeb);
		if((uflgHaba-webAtsu)/2 < stcw) {
			crossbeamVstfType = TYPE1;
		} else {
			crossbeamVstfType = TYPE2;
		}
		double width = stcw;			//��
		double plateThickness = stct;	//��
		string material = ium.findMaterial(plateThickness, stcm);	//�ގ�
		DiafCrossbeamVstfIdItem dcviItem;
		dcviItem.setCrossbeamVstfId(crossbeamVstfId);
		dcviItem.setCrossbeamVstfType(crossbeamVstfType);
		dcviItem.setWidth(width);
		dcviItem.setPlateThickness(plateThickness);
		dcviItem.setMaterial(material);
		dcvi.append(dcviItem);
		_crsVstfNvstc.push_back(nvstc);	//���������⍄�ނ̖{��
		_crsVstfNocrs.push_back(nocrs);	//���������⍄�ނ̔ԍ�
		_crsVstfIdname.push_back(crossbeamVstfId);	//���������⍄�ގ��ʖ�
	}
	dcvi.sort();
	dcvi.toCsv(ofOb);

	return JPT_OK;
}

//(�W)�����z�u
JptErrorStatus AtdDbToCsv::dbToCsvCrossbeamSet(
	AtdDbSecCbeam& scb,				//�f�ʥ����
	AtdDbLineCbeam& lcb			,	//���`�����(�ΌX�\/�u���P�b�g)
	AtdDbLineGrdPanel& lgp,			//���`��包(����/�u���P�b�g)��p�l����
	AtdDbInputHaraikomiHoko& ihh,	//���ͥ�������ݕ���
	std::ofstream& ofOb)
{
	StringHandler sh;
	DiafCrossbeamSet dcs;

	for(int i=0;i<_ngMax-1;i++) {
		string leftKetaName = _ketaNameameList[i];		//��������
		string rightKetaName = _ketaNameameList[i+1];	//�E������
		for(int j=0;j<(int)_oudanNameList.size();j++) {
			int nostr = lcb.findNostr(i+1, i+2, j+1);
			int nosec = lcb.findNosec(i+1, i+2, j+1);
			if(nosec < 1) {
				continue;
			}
			int idx = scb.find(nosec);
			if(idx < 0) {
				continue;
			}
			string kakutenName = _oudanNameList[j];		//�i�_��
			int kakutenNo = j+1;						//�i�_�ԍ�
			AtdDbSecCbeamItem scbItem;
			scb.getAt(idx, scbItem);
			double luLength = scbItem.getH1cl();	//�����包��[���牡���E�F�u��[�܂ł̋���
			double ldLength = scbItem.getH1cr();	//�����包���[���牡���E�F�u���[�܂ł̋���
			double ruLength = scbItem.getH2cl();	//�E���包��[���牡���E�F�u��[�܂ł̋���
			double rdLength = scbItem.getH2cr();	//�E���包���[���牡���E�F�u���[�܂ł̋���
			double ljLength = scbItem.getDspll();	//�����包�E�F�u�c���牡���W���C���g�ʒu�܂ł̋���
			double rjLength = scbItem.getDsplr();	//�E���包�E�F�u�c���牡���W���C���g�ʒu�܂ł̋���
			double lcLength = scbItem.getH1cl();	//�����包��[���牡���E�F�u�c�܂ł̋���
			double rcLength = scbItem.getH1cr();	//�E���包��[���牡���E�F�u�c�܂ł̋���
			string crossbeamId = "";		//�z�u���鉡��
			string leftShiguchiId = "";		//�����d��
			string rightShiguchiId = "";	//�E���d��
			string leftFsplId = "";			//�����t�����W�Y�ڌ`��
			string rightFsplId = "";		//�E���t�����W�Y�ڌ`��
			string leftConnId = "";			//�����R�l�N�V�����E�t�����W�Y�ڌ`��
			string rightConnId = "";		//�E���R�l�N�V�����E�t�����W�Y�ڌ`��
			string leftWsplId = "";			//�����E�F�u�Y�ڔ�
			string rightWsplId = "";		//�E���E�F�u�Y�ڔ�
			string vstfId = "";				//�����ɔz�u���鐂���⍄��
			//�������ʖ�����
			int shiguchiType = findType(nosec, _crsbeamNocrs, _crsShiguchiType);
			string cBramId = findIdName(nosec, _crsbeamNocrs, _crsbeamIdname);
			if(cBramId == "") {
				continue;
			}
			int nocjul = findType(nosec, _crsbeamNocrs, _crsNocjul);	//NOCJUL:��t�����W�����̓Y�ڔԍ�
			int nocjll = findType(nosec, _crsbeamNocrs, _crsNocjll);	//NOCJLL:���t�����W�����̓Y�ڔԍ�
			int nocjwl = findType(nosec, _crsbeamNocrs, _crsNocjwl);	//NOCJWL:�E�F�u�����̓Y�ڔԍ�
			int nocjur = findType(nosec, _crsbeamNocrs, _crsNocjur);	//NOCJUR:��t�����W�E���̓Y�ڔԍ�
			int nocjlr = findType(nosec, _crsbeamNocrs, _crsNocjlr);	//NOCJLR:���t�����W�E���̓Y�ڔԍ�
			int nocjwr = findType(nosec, _crsbeamNocrs, _crsNocjwr);	//NOCJWR:�E�F�u�E���̓Y�ڔԍ�
			crossbeamId = cBramId;		//�z�u���鉡��
			if(shiguchiType == 0) {	//�d���^�C�v
				// 2018/02/15 take Add Start
				// �d���^�C�v�ŕs�K�v�ȗv�f�̓u�����N�Ƃ���
				lcLength = -1.0;
				rcLength = -1.0;
				// 2018/02/15 take Add End
				leftShiguchiId = cBramId;	//�����d��
				rightShiguchiId = cBramId;	//�E���d��
				leftFsplId = findIdName(nocjul, _crsFsplNoj, _crsFsplIdname);	//�����t�����W�Y�ڌ`��
				rightFsplId = findIdName(nocjur, _crsFsplNoj, _crsFsplIdname);	//�E���t�����W�Y�ڌ`��
			} else {
				// 2018/02/15 take Add Start
				// �包��tVSTF�^�C�v�ŕs�K�v�ȗv�f�̓u�����N�Ƃ���
				luLength = -1.0;
				ldLength = -1.0;
				ruLength = -1.0;
				rdLength = -1.0;
				ljLength = -1.0;
				rjLength = -1.0;
				// 2018/02/15 take Add End
				leftConnId = findIdName(nocjul, _crsConnNoj, _crsConnIdname);	//�����R�l�N�V�����E�t�����W�Y�ڌ`��
				rightConnId = findIdName(nocjur, _crsConnNoj, _crsConnIdname);	//�E���R�l�N�V�����E�t�����W�Y�ڌ`��
			}
			leftWsplId = findIdName(nocjwl, _crsWsplNowj, _crsWsplIdname);		//�����E�F�u�Y�ڔ�
			rightWsplId = findIdName(nocjwr, _crsWsplNowj, _crsWsplIdname);		//�E���E�F�u�Y�ڔ�
			//VSTF
			// 2018/02/15 take Edit Start
			// �����l�ύX
			double leftVstfDim = -1.0;		
			string centerVstfPitch = "-";	
			double rightVstfDim = -1.0;	
			EnFace vstfSetFace = NON;
			if( cBramId.find("BH") != std::string::npos ){
				leftVstfDim = 0.0;
				rightVstfDim = 0.0;
			}
			//double leftVstfDim = 0.0;		//����VSTF�ʒu�̐��@
			//string centerVstfPitch = "";	//����VSTF�ʒu�̃s�b�`
			//double rightVstfDim = 0.0;	//�E��VSTF�ʒu�̐��@
			//EnFace vstfSetFace = NS;		//���������⍄�ނ̔z�u��
			// 2018/02/15 take Edit End
			vstfId = findIdName(nosec, _crsVstfNocrs, _crsVstfIdname);
			if(vstfId != "") {
				int nvstf = findType(nosec, _crsVstfNocrs, _crsVstfNvstc);	//���������⍄�ނ̖{��
				if(nvstf > 0) {
					double rlp = lgp.findRlp(4, nostr, 1);
					double vsKyori = rlp / (nvstf+1);
					leftVstfDim = vsKyori;
					if(nvstf > 1) {
						rightVstfDim = vsKyori;
					}
					if(nvstf > 2) {
						if(nvstf == 3) {
							centerVstfPitch = vsKyori;
						} else {
							centerVstfPitch = sh.toString(nvstf-2) + "@" + sh.toString(vsKyori);
						}
					}
					int itphr = ihh.findItphr(nostr);	//���ͥ�������ݕ���
					if(itphr % 10 == 0) {	//����
						vstfSetFace = FS;
					} else {				//�E��
						vstfSetFace = NS;
					}
				}
			}
			DiafCrossbeamSetItem dcsItem;
			dcsItem.setLeftKetaName(leftKetaName);
			dcsItem.setRightKetaName(rightKetaName);
			dcsItem.setKakutenName(kakutenName);
			dcsItem.setKakutenNo(kakutenNo);
			dcsItem.setLuLength(luLength);
			dcsItem.setLdLength(ldLength);
			dcsItem.setRuLength(ruLength);
			dcsItem.setRdLength(rdLength);
			dcsItem.setLjLength(ljLength);
			dcsItem.setRjLength(rjLength);
			dcsItem.setLcLength(lcLength);
			dcsItem.setRcLength(rcLength);
			dcsItem.setCrossbeamId(crossbeamId);
			dcsItem.setLeftShiguchiId(leftShiguchiId);
			dcsItem.setRightShiguchiId(rightShiguchiId);
			dcsItem.setLeftFsplId(leftFsplId);
			dcsItem.setRightFsplId(rightFsplId);
			dcsItem.setLeftConnId(leftConnId);
			dcsItem.setRightConnId(rightConnId);
			dcsItem.setLeftWsplId(leftWsplId);
			dcsItem.setRightWsplId(rightWsplId);
			dcsItem.setLeftVstfDim(leftVstfDim);
			dcsItem.setCenterVstfPitch(centerVstfPitch);
			dcsItem.setRightVstfDim(rightVstfDim);
			dcsItem.setVstfId(vstfId);
			dcsItem.setVstfSetFace(vstfSetFace);
			dcs.append(dcsItem);
		}
	}
	dcs.sort();
	dcs.toCsv(ofOb);

	return JPT_OK;
}

double AtdDbToCsv::findGrdKakutenVstfItaatsu(const string& ketaName, const string& kakutenName, const EnFace& face)
{
	for(int i=0;i<(int)_grdKakuVstfKetaName.size();i++) {
		if(_grdKakuVstfKetaName[i] == ketaName && _grdKakuVstfKakutenName[i] == kakutenName && _grdKakuVstfFace[i] == face) {
			return _grdKakuVstfitaastu[i];
		}
	}
	return 0.0;
}

// 2018/02/28 take Add Start
void AtdDbToCsv::devideItmb( const int& itmb, vector<int>& itmbMemberList )
{
	int currItmb = itmb;
	int targetIndex = 0;
	double min = 10000.0;
	double memberList[14] = { 2, 4, 8, 16, 32, 64, 128, 256, 512,		//�\������
							  1024, 2048, 4096, 8192, 16384 };

	int nYouso = sizeof(memberList) / sizeof(memberList[0]);
	while(1){
		for( int i = 0; i < nYouso; i++ ){					//�\�����ڂ���itmb�̒l�Ƌ߂����ڔԍ����擾
			if( currItmb >= memberList[i] ){
				int subtract = currItmb - memberList[i];
				if( subtract < min ){
					min = subtract;
					targetIndex = i;					//�C���f�b�N�X�ԍ����擾
				}
			}else{
				break;
			}
		}
		currItmb -= memberList[targetIndex];
		itmbMemberList.push_back( memberList[targetIndex] );
		if( currItmb == 0 ){			//ITMB�̍\���v�f��S�Ď擾�����ꍇ
			break;
		}
	}
}

bool AtdDbToCsv::existConfigurationItem( const int& targetItem, const vector<int>& itmbMemberList )
{
	for( int i = 0; i < itmbMemberList.size(); i++ ){
		if( itmbMemberList[i] == targetItem ){			//�\�����ڂɖړI�̍��ڂ����݂����ꍇ
			return true;
		}
	}
	return false;										//�\�����ڂɖړI�̍��ڂ����݂��Ȃ��ꍇ
}
// 2018/02/28 take Add End

int AtdDbToCsv::findType(int jointNo, vector<int>& jointNoList, vector<int>& idNameList)
{
	int type = -1;
	for(int i=0;i<(int)jointNoList.size();i++) {
		if(jointNo == jointNoList[i]) {
			type = idNameList[i];
			break;
		}
	}

	return type;
}

double AtdDbToCsv::findDouble(int jointNo, vector<int>& jointNoList, vector<double>& idNameList)
{
	double type = 0.0;
	for(int i=0;i<(int)jointNoList.size();i++) {
		if(jointNo == jointNoList[i]) {
			type = idNameList[i];
			break;
		}
	}

	return type;
}

string AtdDbToCsv::findIdName(int jointNo, vector<int>& jointNoList, vector<string>& idNameList)
{
	string splId = "";
	for(int i=0;i<(int)jointNoList.size();i++) {
		if(jointNo == jointNoList[i]) {
			splId = idNameList[i];
			break;
		}
	}

	return splId;
}

string AtdDbToCsv::makePitchGage(const int& npg, const double& pg)
{
	StringHandler sh;
	string pitchGage = "";
	if(npg == 2) {
		pitchGage = sh.toString(pg);
	} else if(npg > 2) {
		pitchGage = sh.toString(npg-1) + "@" + sh.toString(pg);
	}

	return pitchGage;
}

double AtdDbToCsv::getFillItaatsu(const double& atsuSa)
{
	double itaatsu = atsuSa;

	if(atsuSa > 6.0) {
		for(int d=100;d>=7;d--) {
			if(atsuSa <= (double)(d)) {
				itaatsu = (double)(d);
			}
		}
		return itaatsu;
	}
	if(atsuSa <= 6.0) {
		itaatsu = 6.0;
	}
	if(atsuSa <= 4.5) {
		itaatsu = 4.5;
	}
	if(atsuSa <= 2.3) {
		itaatsu = 2.3;
	}
	if(atsuSa <= 1.6) {
		itaatsu = 1.6;
	}

	return itaatsu;
}


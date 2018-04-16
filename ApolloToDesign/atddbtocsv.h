#pragma once

#include "DiafManager.h"
#include "AtdGirderCommon.h"
#include "AtdCrossBeamCommon.h"
#include "AtdDbInputStfAki.h"
#include "AtdDbInputGrdSpl.h"
#include "AtdDbInputHeight.h"
#include "AtdDbInputGrdMen.h"
#include "AtdDbInputCbeam.h"
#include "AtdDbInputKetatanLeng.h"
#include "AtdDbInputUseMaterial.h"
#include "AtdDbInputFillOption.h"
#include "AtdDbInputHaraikomiHoko.h"
#include "AtdDbInputCbeamConnSpl.h"
#include "AtdDbLineGrdZahyo.h"
#include "AtdDbLineCbeam.h"
#include "AtdDbLineGrdCamber.h"
#include "AtdDbSecGrdKaku.h"
#include "AtdDbSecGrdLeng.h"
#include "AtdDbSecScaleSpl.h"
#include "AtdDbSecGrd.h"
#include "AtdDbSecSolePl.h"
#include "AtdDbSecManhole.h"
#include "AtdDbSecCutData.h"
#include "AtdDbSecGrdKakuVst.h"
#include "AtdDbSecVstf.h"
#include "AtdDbSecVstfHaichi.h"
#include "AtdDbSecHstf.h"
#include "AtdDbSecHstfHaichi.h"
#include "AtdDbSecStfDir.h"
#include "AtdDbSecScaleFill.h"
#include "AtdDbSecCbeamSec.h"
#include "AtdDbSecCbeam.h"
#include "AtdDbSecCbeamStf.h"
#include "AtdDbSecCbeamKind.h"
#include "AtdDbSecCbeamVstfNum.h"
#include "AtdDbSplCommon.h"
#include "AtdDbSplFlg.h"
#include "AtdDbSplWeb.h"
#include "AtdDbGrdHstfLap.h"
#include "AtdDbGrdVstfHaichi.h"
#include "AtdDbGrdVstfKyori.h"
#include "AtdDbSecGrdHeightConstant.h"
#include "AtdDbSecGrdHeightVariable.h"
#include "AtdDbLineGrdPanel.h"
#include "AtdDbStructAll.h"
#include "AtdDbGrdSecPower.h"
// 2018/02/26 take Add Start
#include "AtdDbBaseData.h"
#include "AtdDbStatusHstf.h"
#include "AtdDbRangeHstf.h"
#include "AtdDbGrdPower.h"
// 2018/02/26 take Add End

class AtdDbToCsv
{
public:
	AtdDbToCsv(void) {}
	~AtdDbToCsv(void) {}

	JptErrorStatus dbToCsvSklKakuten(string& sekkeiFilePath, AtdDbSecGrdKaku& sgk, AtdDbLineGrdZahyo& lgz, AtdDbInputGrdMen& igm, std::ofstream& ofOb);
	// 2018/02/27 take Edit Start
	//JptErrorStatus dbToCsvSklOudan(AtdDbSecGrdKaku& sgk, AtdDbLineGrdZahyo& lgz, std::ofstream& ofOb);
	JptErrorStatus dbToCsvSklOudan(AtdDbStructAll& sta, AtdDbSecGrdKaku& sgk, AtdDbLineGrdZahyo& lgz, std::ofstream& ofOb);	//�u�\���E�S�́v�ǉ�
	// 2018/02/27 take Edit End
	JptErrorStatus dbToCsvSklCamber(AtdDbSecGrdKaku& sgk, AtdDbLineGrdCamber& lgc, std::ofstream& ofOb);
	// 2018/02/28 take Edit Start
	//JptErrorStatus dbToCsvSklVstf(AtdDbGrdVstfHaichi& gvh, AtdDbGrdVstfKyori& gvk, std::ofstream& ofOb);
	JptErrorStatus dbToCsvSklVstf(AtdDbSecVstfHaichi& svh, AtdDbLineGrdPanel& lgp, std::ofstream& ofOb);
	// 2018/02/28 take Edit End
	JptErrorStatus dbToCsvSklJoint(AtdDbSecGrdLeng& sgl, std::ofstream& ofOb);
	JptErrorStatus dbToCsvSklWebHeight(AtdDbInputGrdMen& igm, AtdDbSecGrdHeightConstant& sghc, AtdDbSecGrdHeightVariable& sghv, AtdDbLineGrdPanel& lgp, AtdDbInputKetatanLeng& ikl, std::ofstream& ofOb);
	// 2018/02/28 take Edit Start
	//JptErrorStatus dbToCsvSklHstf(AtdDbGrdHstfLap& ghl, std::ofstream& ofOb);
	JptErrorStatus dbToCsvSklHstf(AtdDbStatusHstf& sh, AtdDbRangeHstf &rh, std::ofstream& ofOb);
	// 2018/02/28 take Edit End
	JptErrorStatus dbToCsvGirderCommon(AtdGirderCommon& agc, AtdDbSecScaleSpl& sss, AtdDbInputGrdSpl& igs, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderSection(AtdDbInputUseMaterial& ium, AtdDbSecGrd& sgd, AtdDbSecGrdLeng& sgl, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderLflgWidening(AtdDbSecSolePl& ssp, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderWebManhole(AtdDbSecManhole& smh, AtdDbSecCutData& scd, std::ofstream& ofOb);
	//2018/03/01 take Edit Start
	//JptErrorStatus dbToCsvGirderVstfId(AtdDbInputUseMaterial& ium, AtdDbSecVstf& svs, AtdDbSecGrdKakuVst& sgv, AtdDbSecVstfHaichi& svh, AtdDbInputStfAki& isa, AtdDbStructAll& sta, AtdDbGrdSecPower& gsp, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderVstfId(AtdDbInputUseMaterial& ium, AtdDbSecVstf& svs, AtdDbSecGrdKakuVst& sgv, AtdDbSecVstfHaichi& svh, AtdDbInputStfAki& isa, AtdDbStructAll& sta, AtdDbGrdPower& gp, std::ofstream& ofOb);
	//2018/03/01 take Edit End
	JptErrorStatus dbToCsvGirderVstfSetSc(AtdDbSecGrdKakuVst& sgv, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderVstfSetV(AtdDbSecVstfHaichi& svh, AtdDbSecStfDir& ssd, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderHstfId(AtdDbInputUseMaterial& ium, AtdDbSecHstf& shf, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderHstfSet(AtdDbSecHstfHaichi& shh, AtdDbSecStfDir& ssd, AtdDbSecVstfHaichi& svh, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderFlgSplId(AtdDbInputUseMaterial& ium, AtdDbSplCommon& scm, AtdDbSplFlg& sfg, AtdDbSecScaleFill& ssf, AtdDbInputFillOption& ifo, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderWebSplId(AtdDbInputUseMaterial& ium, AtdDbSplCommon& scm, AtdDbSplWeb& swb, AtdDbSecScaleFill& ssf, AtdDbInputFillOption& ifo, std::ofstream& ofOb);
	JptErrorStatus dbToCsvGirderSplSet(std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamCommon(AtdCrossBeamCommon& acc, AtdDbInputCbeam& icb, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamSectionId(AtdDbInputUseMaterial& ium, AtdDbSecCbeam& scb, AtdDbSecCbeamSec& scs, AtdDbSplWeb& swb, AtdDbLineCbeam& lcb, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamHbeamId(AtdDbInputUseMaterial& ium, AtdDbSecCbeam& scb, AtdDbSecCbeamSec& scs, AtdDbSplWeb& swb, AtdDbLineCbeam& lcb, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamFlgSplId(AtdDbInputUseMaterial& ium, AtdDbSplCommon& scm, AtdDbSplFlg& sfg, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamConnFsplId(AtdDbInputUseMaterial& ium, AtdDbSplCommon& scm, AtdDbSplFlg& sfg, AtdDbSecScaleFill& ssf, AtdDbInputFillOption& ifo, AtdDbInputCbeamConnSpl& icc, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamWsplId(AtdDbInputUseMaterial& ium, AtdDbSplCommon& scm, AtdDbSplWeb& swb, AtdDbSecScaleFill& ssf, AtdDbInputFillOption& ifo, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamVstfId(AtdDbInputUseMaterial& ium, AtdDbSecCbeamStf& sct, AtdDbSecCbeamVstfNum& scv, std::ofstream& ofOb);
	JptErrorStatus dbToCsvCrossbeamSet(AtdDbSecCbeam& scb, AtdDbLineCbeam& lcb, AtdDbLineGrdPanel& lgp, AtdDbInputHaraikomiHoko& ihh, std::ofstream& ofOb);

private:
	int _ngMax;		//�包��
	int _npMax;		//�x�_�A�i�_�A���[��
	int _jointMax;	//�W���C���g��
	// 2018/02/27 take Add Start
	int _nSpan;		//�a�Ԑ�
	// 2018/02/27 take Add End

	vector<string> _ketaNameameList;	//�包��

	vector<string> _oudanNameWithKetatanList;	//�[�x�_�A���Ԏx�_�A�i�_�A���[�_	_npMax
	vector< vector<double> > _kakutenLengAdd;	//�x�_�i�_�ǉ�����	npMax-1;
	// 2018/02/27 take Add Start
	vector< vector<double> > _midVstfLengAdd;	//�包����VSTF�z�u�ǉ�����
	vector< vector<int> > _nKetaHstfUpperPrg;	//���̐����⍄�ޏ�i��
	vector< vector<int> > _nKetaHstfLowerPrg;	//���̐����⍄�މ��i��
	vector< vector<int> > _hstfPanelNumber;		//�包HSTF�z�u�p�l���ʒu
	vector< vector<int> > _hstfPanelUpperPrg;	//�包HSTF�z�u��i��
	vector< vector<int> > _hstfPanelLowerPrg;	//�包HSTF�z�u���i��
	vector< vector<double> > _hstfLengAdd;		//�包HSTF�z�u�ǉ�����
	// 2018/02/27 take Add End

	vector<int> _oudanType;		//0:�[�x�_�A1:���Ԏx�_�A2:�i�_�A(���[�_����)	_npMax-2
	vector<string> _oudanNameList;	//�[�x�_�A���Ԏx�_�A�i�_�A(���[�_����)	_npMax-2

	vector<int> _shitenKakutenNo;	//�x�_�i�_�ԍ�(�i�_�A���[�_����)
	vector<string> _shitenNameList;	//�[�x�_�A���Ԏx�_(�i�_�A���[�_����)

	//�包�f��
	vector< vector<double> > _danmenLengAdd;	//�f�ʒǉ�����		_jointMax+1
	vector< vector<double> > _danmenAtsuUflg;	//��t�����W��	_jointMax+1
	vector< vector<double> > _danmenHabaUflg;	//��t�����W��	_jointMax+1
	vector< vector<double> > _danmenAtsuWeb;	//�E�F�u��		_jointMax+1
	vector< vector<double> > _danmenAtsuLflg;	//���t�����W��	_jointMax+1
	//�包�t�����W�Y��
	vector<int> _uflgJointNo;	//��t�����W�W���C���g�ԍ�
	vector<string> _uflgIdName;	//��t�����W���ʖ�
	vector<int> _lflgJointNo;	//���t�����W�W���C���g�ԍ�
	vector<string> _lflgIdName;	//���t�����W���ʖ�
	//�包�E�F�u�Y��
	vector<int> _webJointNo;	//�E�G�u�W���C���g�ԍ�
	vector<string> _webIdName;	//�E�G�u���ʖ�

	//�包�����⍄��ID�o�^
	vector<int> _grdVstfNovssc;		//�包�����⍄�ގ���ID
	vector<double> _grdVstfItaatsu;	//�包�����⍄�ޔ�
	//�x�_�E�i�_�����⍄�ޔz�u
	vector<int> _grdKakuVstfKetaNo;			//�x�_�E�i�_�包�����⍄�ތ��ԍ�
	vector<int> _grdKakuVstfKakutenNo;		//�x�_�E�i�_�包�����⍄�ފi�_�ԍ�
	vector<string> _grdKakuVstfKetaName;	//�x�_�E�i�_�包�����⍄�ތ���
	vector<string> _grdKakuVstfKakutenName;	//�x�_�E�i�_�包�����⍄�ފi�_��
	vector<EnFace> _grdKakuVstfFace;		//�x�_�E�i�_�包�����⍄�ޔz�u��
	vector<double> _grdKakuVstfitaastu;		//�x�_�E�i�_�包�����⍄�ޔ�

	//�����{��
	vector<int> _crsHontaiType;		//�����{�̃^�C�v(0:�r���h�� 1:�g�|)
	vector<int> _crsShiguchiType;	//�����d���^�C�v(0:�d�� 1:CONN)
	vector<int> _crsbeamNocrs;		//�����{�̎d���̔ԍ�
	vector<string> _crsbeamIdname;	//�����{�̎d�����ʖ�
	vector<double> _habaUflg;	//UFLG_��
	vector<double> _atsuUflg;	//UFLG_��
	vector<int> _zaiUflg;		//UFLG_�ގ�
	vector<double> _atsuWeb;	//WEB_��
	vector<double> _atsuLflg;	//LFLG_��
	vector<double> _zaiLflg;	//LFLG_�ގ�
	vector<double> _atsuTsukiVatf;	//�˂����킹VSTF��
	vector<int> _crsNocjul;	//NOCJUL:��t�����W�����̓Y�ڔԍ�
	vector<int> _crsNocjll;	//NOCJLL:���t�����W�����̓Y�ڔԍ�
	vector<int> _crsNocjwl;	//NOCJWL:�E�F�u�����̓Y�ڔԍ�
	vector<int> _crsNocjur;	//NOCJUR:��t�����W�E���̓Y�ڔԍ�
	vector<int> _crsNocjlr;	//NOCJLR:���t�����W�E���̓Y�ڔԍ�
	vector<int> _crsNocjwr;	//NOCJWR:�E�F�u�E���̓Y�ڔԍ�
	//�x�_�㉡���t�����W�Y��
	vector<int> _crsFsplNoj;		//noj
	vector<int> _crsFsplPos;		//0:�[�x�_�A1:���Ԏx�_�A2:�i�_
	vector<int> _crsFsplPupdw;		//0:�� 1:��
	vector<string> _crsFsplIdname;	//�����t�����W�Y�ڌ`�󎯕ʖ�
	//�����R�l�N�V�����E�t�����W�Y��
	vector<int> _crsConnNoj;		//noj
	vector<int> _crsConnPos;		//0:�[�x�_�A1:���Ԏx�_�A2:�i�_
	vector<int> _crsConnPupdw;		//0:�� 1:��
	vector<string> _crsConnIdname;	//�����R�l�N�V�����E�t�����W�Y�ڌ`�󎯕ʖ�
	//�����E�F�u�Y��
	vector<int> _crsWsplNowj;		//nowj
	vector<int> _crsWsplPos;		//0:�[�x�_�A1:���Ԏx�_�A2:�i�_
	vector<string> _crsWsplIdname;	//�����E�F�u�Y�ڔ��ʖ�
	//���������⍄��
	vector<int> _crsVstfNvstc;		//���������⍄�ނ̖{��
	vector<int> _crsVstfNocrs;		//���������⍄�ނ̔ԍ�
	vector<string> _crsVstfIdname;	//���������⍄�ގ��ʖ�

	// 2018/02/28 take Add Start
	void devideItmb( const int& itmb, vector<int>& itmbMemberList );							//ITMB����\�����ڂ��擾
	bool existConfigurationItem( const int& targetItem, const vector<int>& itmbMemberList );	//�\�����ڂ����݂��邩�itrue:����, false:�Ȃ��j
	// 2018/02/28 take Add End
	int findType(int jointNo, vector<int>& jointNoList, vector<int>& idNameList);
	double findDouble(int jointNo, vector<int>& jointNoList, vector<double>& idNameList);
	string findIdName(int jointNo, vector<int>& jointNoList, vector<string>& idNameList);
	double findGrdKakutenVstfItaatsu(const string& ketaName, const string& kakutenName, const EnFace& face);
	string makePitchGage(const int& npg, const double& pg);
	double getFillItaatsu(const double& atsuSa);

};


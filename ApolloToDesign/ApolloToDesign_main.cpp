#pragma warning(disable : 4995)
/************************************************************************/
/*                                                                      */
/************************************************************************/
#include "stdafx.h"
#include "ApolloToDesign_main.h"
#include "AtdDbToCsv.h"

/*:**********************************************************************
�@�@�@�@�\�F
�߁@��@�l�F0	����I��
1	�ُ�I��
���@�@�@�l�F
************************************************************************/
int ApolloToDesign_Main(
	string& sekkeiFilePath,
	string& seizuFilePath,
	string& csvFileName,
	AtdGirderCommon& agc,
	AtdCrossBeamCommon& acc
)
/*:*/
{
	CDaoDatabase dbFile;
	CString seizuFileName = seizuFilePath.c_str();
	CString sekkeiFileName = sekkeiFilePath.c_str();

	//�f�[�^�x�[�X�ǂݍ���(���}MDB)
	dbFile.Open(seizuFileName,FALSE,FALSE);

	//���ͥ�r�s�e�̂���	
	AtdDbInputStfAki isa;
	isa.load(dbFile);

	//���ͥ�包�Y��
	AtdDbInputGrdSpl igs;
	igs.load(dbFile);

	//���ͥ������ω��_
	AtdDbInputHeight iht;
	iht.load(dbFile);

	//���ͥ�包���ʌ`��
	AtdDbInputGrdMen igm;
	igm.load(dbFile);

	//���ͥ����
	AtdDbInputCbeam icb;
	icb.load(dbFile);

	//���ͥ���[��
	AtdDbInputKetatanLeng ikl;
	ikl.load(dbFile);

	//���́E�g�p�ޗ��f�[�^
	AtdDbInputUseMaterial ium;
	ium.load(dbFile);

	//���ͥ�t�B���[�v���[�g�I�v�V����
	AtdDbInputFillOption ifo;
	ifo.load(dbFile);

	//���ͥ�������ݕ���
	AtdDbInputHaraikomiHoko ihh;
	ihh.load(dbFile);

	//���́E�����R�l�N�V�����Y��
	AtdDbInputCbeamConnSpl icc;
	icc.load(dbFile);

	//���`��包���W(�c��/���c��)
	AtdDbLineGrdZahyo lgz;
	lgz.load(dbFile);

	//���`�����(�ΌX�\/�u���P�b�g)
	AtdDbLineCbeam lcb;
	lcb.load(dbFile);

	//���`��包(����/�u���P�b�g)��L�����o�[�l
	AtdDbLineGrdCamber lgc;
	lgc.load(dbFile);

	//���`��包(����/�u���P�b�g)��p�l����
	AtdDbLineGrdPanel lgp;
	lgp.load(dbFile);

	//�f�ʥ�包�i�_��
	AtdDbSecGrdKaku sgk;
	sgk.load(dbFile);

	//�f�ʥ�包�̒f�ʒ�
	AtdDbSecGrdLeng sgl;
	sgl.load(dbFile);

	//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��包�Y�ڊ֌W
	AtdDbSecScaleSpl sss;
	sss.load(dbFile);

	//�f�ʥ�包�f��
	AtdDbSecGrd sgd;
	sgd.load(dbFile);

	//�f�ʥ�\�[���v���[�g
	AtdDbSecSolePl ssp;
	ssp.load(dbFile);

	//�f�ʥ���[���̃}���z�[���̃J�b�g
	AtdDbSecManhole smh;
	smh.load(dbFile);

	//�f�ʥ�J�b�g�f�[�^
	AtdDbSecCutData scd;
	scd.load(dbFile);

	//�f�ʥ�包�i�_��VSTF
	AtdDbSecGrdKakuVst sgv;
	sgv.load(dbFile);

	//�f�ʥVSTF�f��
	AtdDbSecVstf svs;
	svs.load(dbFile);

	//�f�ʥVSTF�z�u
	AtdDbSecVstfHaichi svh;
	svh.load(dbFile);

	//�f�ʥHSTF�f��
	AtdDbSecHstf shf;
	shf.load(dbFile);

	//�f�ʥHSTF�z�u
	AtdDbSecHstfHaichi shh;
	shh.load(dbFile);

	// 2018/02/28 take Add Start
	//�f�ʥHSTF�̈ʒu�֌W(�i���ω�������i��)
	AtdDbStatusHstf sh;
	sh.load(dbFile);
	//�f�ʥ�����⍄�ނ̓���͈�(�ǉ�����)
	AtdDbRangeHstf rh;
	rh.load(dbFile);
	// 2018/02/28 take Add End

	//�f�ʥ�ڍץ�⍄�ނ̌���
	AtdDbSecStfDir ssd;
	ssd.load(dbFile);

	//�f�ʥ�X�P�[���y�ѕ��������ƍގ��d�l��t�B���[�v���[�g
	AtdDbSecScaleFill ssf;
	ssf.load(dbFile);

	//�f�ʥ�包����(���E�������)
	AtdDbSecGrdHeightConstant sghc;
	sghc.load(dbFile);

	//�f�ʥ�包����(�������S��)
	AtdDbSecGrdHeightVariable sghv;
	sghv.load(dbFile);

	//�f�ʥ������f��
	AtdDbSecCbeamSec scs;
	scs.load(dbFile);

	//�f�ʥ����
	AtdDbSecCbeam scb;
	scb.load(dbFile);

	//�f�ʥ������⍄�ޒf�ʐ�
	AtdDbSecCbeamStf sct;
	sct.load(dbFile);

	//�f�ʥ������ސ�
	AtdDbSecCbeamKind sck;
	sck.load(dbFile);

	//�f�ʥ������p�l������VSTF�{��
	AtdDbSecCbeamVstfNum scv;
	scv.load(dbFile);

	//�Y�ڥ���`�Y�ڋ��ʃf�[�^
	AtdDbSplCommon scm;
	scm.load(dbFile);

	//�Y�ڥ�e�Y�ڥ�t�����W
	AtdDbSplFlg sfg;
	sfg.load(dbFile);

	//�Y�ڥ�e�Y�ڥ�E�F�u
	AtdDbSplWeb swb;
	swb.load(dbFile);

	//�\����S��
	AtdDbStructAll sta;
	sta.load(dbFile);

	dbFile.Close();

	//�f�[�^�x�[�X�ǂݍ���(�݌vMDB)
	dbFile.Open(sekkeiFileName,FALSE,FALSE);

	// 2018/02/26 take Add Start
	//�����݌v�
	AtdDbBaseData bd;
	bd.load(dbFile);
	// 2018/02/26 take Add End

	// 2018/02/28 take Delete Start
	//���}�̃e�[�u������擾
	//�包�����⍄�ރ��b�v�͈�
	//AtdDbGrdHstfLap ghl;
	//ghl.load(dbFile);
	// 2018/02/28 take Delete end

	// 2018/02/28 take Delete Start
	//���}�̃e�[�u������擾
	//�包���Ԑ����⍄�ޔz�u�f�[�^
	//AtdDbGrdVstfHaichi gvh;
	//gvh.load(dbFile);
	// 2018/02/28 take Delete End

	// 2018/02/28 take Delete Start
	//���}�̃e�[�u������擾
	//�包���Ԑ����⍄�ފԊu�f�[�^
	//AtdDbGrdVstfKyori gvk;
	//gvk.load(dbFile);
	// 2018/02/28 take Delete End

	// 2018/03/01 take Delete Start
	//�包�f�ʗ̓f�[�^
	//AtdDbGrdSecPower gsp;
	//gsp.load(dbFile);
	// 2018/03/01 take Delete End

	// 2018/03/01 take Add Start
	AtdDbGrdPower gp;
	gp.load(dbFile);
	// 2018/03/01 take Add End
	dbFile.Close();

	// 2018/02/26 take Add Start
	// �����`��
	/*bool brdgTypeFlg = false;
	if( bd.judgeBrdgType( brdgTypeFlg ) != JPT_OK ){
		return(1);
	}*/
	// 2018/02/26 take Add End

	//�f�[�^�o��
	std::ofstream ofOb;
	ofOb.open(csvFileName.c_str(), std::ios::out);

	//�Q�D��{�`��
	AtdDbToCsv atc;

	//(�P)�i�_���W�@2018/02/26 take �Z
	atc.dbToCsvSklKakuten(sekkeiFilePath, sgk, lgz, igm, ofOb);

	//(�Q)���f����` 2018/02/28 take �Z�i�x�_���̔��胍�W�b�N�C���j
	// 2018/02/27 take Edit Start
	//atc.dbToCsvSklOudan(sgk, lgz, ofOb);
	atc.dbToCsvSklOudan(sta, sgk, lgz, ofOb);
	// 2018/02/28 take Edit End

	//(�R)�L�����o�[ 2018/02/26 take �Z�i�������A�i�_�ԍ��͖��m�F�j
	atc.dbToCsvSklCamber(sgk, lgc, ofOb);

	//(�S)�����⍄�ވʒu 2018/02/26 take �Z�i���Ԋu�݂̂ɕύX�j
	// 2018/02/28 take Edit Start
	//atc.dbToCsvSklVstf(gvh, gvk, ofOb);
	atc.dbToCsvSklVstf(svh, lgp, ofOb);
	// 2018/02/28 take Edit End

	//(�T)�W���C���g�ʒu 2018/02/26 take �Z�i�������A�f�ʔԍ��͖��m�F�j
	atc.dbToCsvSklJoint(sgl, ofOb);

	//(�U)�包�E�F�u���[�� 2018/02/27 take �Z�i�E�F�u���ω����W�b�N�C���j
	atc.dbToCsvSklWebHeight(igm, sghc, sghv, lgp, ikl, ofOb);

	//(�V)�����⍄�ލ��� 2018/02/28 take �Z�i���W�b�N�ύX�j
	// 2018/02/28 take Edit Start
	//atc.dbToCsvSklHstf(ghl, ofOb);
	atc.dbToCsvSklHstf(sh, rh, ofOb);

	// 2018/02/28 take Edit End

	//�R�D�包

	//(�P)���ʏڍ׃f�[�^ 2018/02/27 take �Z
	atc.dbToCsvGirderCommon(agc, sss, igs, ofOb);

	//(�Q)�包�f�� 2018/02/27 take �Z
	atc.dbToCsvGirderSection(ium, sgd, sgl, ofOb);

	//(�R)���t�����W�g�����`�� 2018/02/27 take �Z
	atc.dbToCsvGirderLflgWidening(ssp, ofOb);

	//(�S)���[�}���z�[���`�� 2018/02/27 take �Z
	atc.dbToCsvGirderWebManhole(smh, scd, ofOb);

	//(�T)�����⍄��ID�o�^ 2018/03/01 take �Z�H
	// 2018/03/01 take Edit Start
	//atc.dbToCsvGirderVstfId(ium, svs, sgv, svh, isa, sta, gsp, ofOb);
	atc.dbToCsvGirderVstfId(ium, svs, sgv, svh, isa, sta, gp, ofOb);
	// 2018/03/01 take Edit End

	//(�U)�x�_�E�i�_�����⍄�ޔz�u 2018/03/01 take �Z
	atc.dbToCsvGirderVstfSetSc(sgv, ofOb);

	//(�V)���Ԑ����⍄�ޔz�u 2018/03/01 take �Z
	atc.dbToCsvGirderVstfSetV(svh, ssd, ofOb);

	//(�W)�����⍄��ID�o�^ 2018/03/01 take �Z
	atc.dbToCsvGirderHstfId(ium, shf, ofOb);

	//(�X)�����⍄�ޔz�u
	atc.dbToCsvGirderHstfSet(shh, ssd, svh, ofOb);

	//(�P�O)�t�����W�Y�ڌ`���`
	atc.dbToCsvGirderFlgSplId(ium, scm, sfg, ssf, ifo, ofOb);

	//(�P�P)�E�F�u�Y�ڌ`��
	atc.dbToCsvGirderWebSplId(ium, scm, swb, ssf, ifo, ofOb);

	//(�P�Q)�Y�ڔz�u
	atc.dbToCsvGirderSplSet(ofOb);

	//�S�D����

	//(�P)���ʏڍ׃f�[�^
	atc.dbToCsvCrossbeamCommon(acc, icb, ofOb);

	//(�Q)�x�_�㉡���f��ID�o�^
	atc.dbToCsvCrossbeamSectionId(ium, scb, scs, swb, lcb, ofOb);

	//(�R)�i�_�㉡���g�|ID�o�^
	atc.dbToCsvCrossbeamHbeamId(ium, scb, scs, swb, lcb, ofOb);

	//(�S)�x�_�㉡���t�����W�Y�ڌ`���`
	atc.dbToCsvCrossbeamFlgSplId(ium, scm, sfg, ofOb);

	//(�T)�R�l�N�V�����E�t�����W�Y�ڌ`���`
	atc.dbToCsvCrossbeamConnFsplId(ium, scm, sfg, ssf, ifo, icc, ofOb);

	//(�U)�E�F�u�Y�ڌ`���`
	atc.dbToCsvCrossbeamWsplId(ium, scm, swb, ssf, ifo, ofOb);

	//(�V)�����⍄��ID�o�^
	atc.dbToCsvCrossbeamVstfId(ium, sct, scv, ofOb);

	//(�W)�����z�u
	atc.dbToCsvCrossbeamSet(scb, lcb, lgp, ihh, ofOb);

	ofOb.close();

	return(0);
}


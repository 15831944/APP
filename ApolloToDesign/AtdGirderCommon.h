#pragma once
#include "DiafManager.h"

class AtdGirderCommon
{
public:
	AtdGirderCommon() {}
	~AtdGirderCommon() {}

	double &getUflgJc() {return _uflgJc;}
	double &getWebJc() {return _webJc;}
	double &getLflgJc() {return _lflgJc;}
	EnTypeNo &getUflgZaitanKeijo() {return _uflgZaitanKeijo;}
	EnTypeNo &getLflgZaitanKeijo() {return _lflgZaitanKeijo;}
	double &getUpdownFlgZaitanKeijoTachiageRyo() {return _updownFlgZaitanKeijoTachiageRyo;}
	double &getSolePlateKyotyokuFreeSpace() {return _solePlateKyotyokuFreeSpace;}
	double &getSolePlateKyojikuFreeSpace() {return _solePlateKyojikuFreeSpace;}
	double &getItatsugiZureRyo() {return _itatsugiZureRyo;}
	double &getLflgKakuhukubuTaper() {return _lflgKakuhukubuTaper;}
	double &getWebHoleSlopeLowerLimitGrd() {return _webHoleSlopeLowerLimitGrd;}
	string &getShitenVsCutWu() {return _shitenVsCutWu;}
	string &getShitenVsCutWd() {return _shitenVsCutWd;}
	string &getShitenVsCutFu() {return _shitenVsCutFu;}
	string &getKakutenVsCutWu() {return _kakutenVsCutWu;}
	string &getKakutenVsCutWd() {return _kakutenVsCutWd;}
	string &getKakutenVsCutFu() {return _kakutenVsCutFu;}
	string &getMiddleVsCutWu() {return _middleVsCutWu;}
	string &getMiddleVsCutWd() {return _middleVsCutWd;}
	double &getMiddleVsFreeSpace() {return _middleVsFreeSpace;}
	double &getHsFreeSpaceVs() {return _hsFreeSpaceVs;}
	double &getHsFreeSpaceSpl() {return _hsFreeSpaceSpl;}
	double &getHsFreeSpaceCbf() {return _hsFreeSpaceCbf;}
	double &getHsFreeSpaceCbfUlimit() {return _hsFreeSpaceCbfUlimit;}
	string &getHsSnipSizeVs() {return _hsSnipSizeVs;}
	string &getHsSnipSizeSpl() {return _hsSnipSizeSpl;}
	string &getHsSnipSizeCbf() {return _hsSnipSizeCbf;}
	double &getUflgSplKyojikuZaitan() {return _uflgSplKyojikuZaitan;}
	double &getUflgOutsideSplKyotyokuZaitan() {return _uflgOutsideSplKyotyokuZaitan;}
	double &getUflgInsideSplKyotyokuZaitan() {return _uflgInsideSplKyotyokuZaitan;}
	double &getWebSplKyojikuZaitan() {return _webSplKyojikuZaitan;}
	double &getWebSplHeightZaitan() {return _webSplHeightZaitan;}
	double &getLflgSplKyojikuZaitan() {return _lflgSplKyojikuZaitan;}
	double &getLflgSplKyotyokuZaitan() {return _lflgSplKyotyokuZaitan;}

	const double &getUflgJc() const {return _uflgJc;}
	const double &getWebJc() const {return _webJc;}
	const double &getLflgJc() const {return _lflgJc;}
	const EnTypeNo &getUflgZaitanKeijo() const {return _uflgZaitanKeijo;}
	const EnTypeNo &getLflgZaitanKeijo() const {return _lflgZaitanKeijo;}
	const double &getUpdownFlgZaitanKeijoTachiageRyo() const {return _updownFlgZaitanKeijoTachiageRyo;}
	const double &getSolePlateKyotyokuFreeSpace() const {return _solePlateKyotyokuFreeSpace;}
	const double &getSolePlateKyojikuFreeSpace() const {return _solePlateKyojikuFreeSpace;}
	const double &getItatsugiZureRyo() const {return _itatsugiZureRyo;}
	const double &getLflgKakuhukubuTaper() const {return _lflgKakuhukubuTaper;}
	const double &getWebHoleSlopeLowerLimitGrd() const {return _webHoleSlopeLowerLimitGrd;}
	const string &getShitenVsCutWu() const {return _shitenVsCutWu;}
	const string &getShitenVsCutWd() const {return _shitenVsCutWd;}
	const string &getShitenVsCutFu() const {return _shitenVsCutFu;}
	const string &getKakutenVsCutWu() const {return _kakutenVsCutWu;}
	const string &getKakutenVsCutWd() const {return _kakutenVsCutWd;}
	const string &getKakutenVsCutFu() const {return _kakutenVsCutFu;}
	const string &getMiddleVsCutWu() const {return _middleVsCutWu;}
	const string &getMiddleVsCutWd() const {return _middleVsCutWd;}
	const double &getMiddleVsFreeSpace() const {return _middleVsFreeSpace;}
	const double &getHsFreeSpaceVs() const {return _hsFreeSpaceVs;}
	const double &getHsFreeSpaceSpl() const {return _hsFreeSpaceSpl;}
	const double &getHsFreeSpaceCbf() const {return _hsFreeSpaceCbf;}
	const double &getHsFreeSpaceCbfUlimit() const {return _hsFreeSpaceCbfUlimit;}
	const string &getHsSnipSizeVs() const {return _hsSnipSizeVs;}
	const string &getHsSnipSizeSpl() const {return _hsSnipSizeSpl;}
	const string &getHsSnipSizeCbf() const {return _hsSnipSizeCbf;}
	const double &getUflgSplKyojikuZaitan() const {return _uflgSplKyojikuZaitan;}
	const double &getUflgOutsideSplKyotyokuZaitan() const {return _uflgOutsideSplKyotyokuZaitan;}
	const double &getUflgInsideSplKyotyokuZaitan() const {return _uflgInsideSplKyotyokuZaitan;}
	const double &getWebSplKyojikuZaitan() const {return _webSplKyojikuZaitan;}
	const double &getWebSplHeightZaitan() const {return _webSplHeightZaitan;}
	const double &getLflgSplKyojikuZaitan() const {return _lflgSplKyojikuZaitan;}
	const double &getLflgSplKyotyokuZaitan() const {return _lflgSplKyotyokuZaitan;}

	void setUflgJc(const double &val) {_uflgJc = val;}
	void setWebJc(const double &val) {_webJc = val;}
	void setLflgJc(const double &val) {_lflgJc = val;}
	void setUflgZaitanKeijo(const EnTypeNo &val) {_uflgZaitanKeijo = val;}
	void setLflgZaitanKeijo(const EnTypeNo &val) {_lflgZaitanKeijo = val;}
	void setUpdownFlgZaitanKeijoTachiageRyo(const double &val) {_updownFlgZaitanKeijoTachiageRyo = val;}
	void setSolePlateKyotyokuFreeSpace(const double &val) {_solePlateKyotyokuFreeSpace = val;}
	void setSolePlateKyojikuFreeSpace(const double &val) {_solePlateKyojikuFreeSpace = val;}
	void setItatsugiZureRyo(const double &val) {_itatsugiZureRyo = val;}
	void setLflgKakuhukubuTaper(const double &val) {_lflgKakuhukubuTaper = val;}
	void setWebHoleSlopeLowerLimitGrd(const double &val) {_webHoleSlopeLowerLimitGrd = val;}
	void setShitenVsCutWu(const string &val) {_shitenVsCutWu = val;}
	void setShitenVsCutWd(const string &val) {_shitenVsCutWd = val;}
	void setShitenVsCutFu(const string &val) {_shitenVsCutFu = val;}
	void setKakutenVsCutWu(const string &val) {_kakutenVsCutWu = val;}
	void setKakutenVsCutWd(const string &val) {_kakutenVsCutWd = val;}
	void setKakutenVsCutFu(const string &val) {_kakutenVsCutFu = val;}
	void setMiddleVsCutWu(const string &val) {_middleVsCutWu = val;}
	void setMiddleVsCutWd(const string &val) {_middleVsCutWd = val;}
	void setMiddleVsFreeSpace(const double &val) {_middleVsFreeSpace = val;}
	void setHsFreeSpaceVs(const double &val) {_hsFreeSpaceVs = val;}
	void setHsFreeSpaceSpl(const double &val) {_hsFreeSpaceSpl = val;}
	void setHsFreeSpaceCbf(const double &val) {_hsFreeSpaceCbf = val;}
	void setHsFreeSpaceCbfUlimit(const double &val) {_hsFreeSpaceCbfUlimit = val;}
	void setHsSnipSizeVs(const string &val) {_hsSnipSizeVs = val;}
	void setHsSnipSizeSpl(const string &val) {_hsSnipSizeSpl = val;}
	void setHsSnipSizeCbf(const string &val) {_hsSnipSizeCbf = val;}
	void setUflgSplKyojikuZaitan(const double &val) {_uflgSplKyojikuZaitan = val;}
	void setUflgOutsideSplKyotyokuZaitan(const double &val) {_uflgOutsideSplKyotyokuZaitan = val;}
	void setUflgInsideSplKyotyokuZaitan(const double &val) {_uflgInsideSplKyotyokuZaitan = val;}
	void setWebSplKyojikuZaitan(const double &val) {_webSplKyojikuZaitan = val;}
	void setWebSplHeightZaitan(const double &val) {_webSplHeightZaitan = val;}
	void setLflgSplKyojikuZaitan(const double &val) {_lflgSplKyojikuZaitan = val;}
	void setLflgSplKyotyokuZaitan(const double &val) {_lflgSplKyotyokuZaitan = val;}

private:
	//�W���C���g�N���A�����X
	double _uflgJc;		//��t�����W�̃W���C���g�N���A�����X
	double _webJc;		//�E�F�u�̃W���C���g�N���A�����X
	double _lflgJc;		//���t�����W�̃W���C���g�N���A�����X
	//�ޒ[�`��
	EnTypeNo _uflgZaitanKeijo;					//��t�����W�ޒ[�`��
	EnTypeNo _lflgZaitanKeijo;					//���t�����W�ޒ[�`��
	double _updownFlgZaitanKeijoTachiageRyo;	//�㉺�t�����W�ޒ[�`�󗧏グ��
	//�\�[���v���[�g
	double _solePlateKyotyokuFreeSpace;		//�\�[���v���[�g�̋��������󂫗�
	double _solePlateKyojikuFreeSpace;		//�\�[���v���[�g�̋��������󂫗�
	//���̑�
	double _itatsugiZureRyo;				//�p�Y����
	double _lflgKakuhukubuTaper;			//���t�����W�g�����̃e�[�p�[���z
	double _webHoleSlopeLowerLimitGrd;		//�E�F�u�E�̍E���z�̉����l
	//�����⍄��
	string _shitenVsCutWu;					//�x�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
	string _shitenVsCutWd;					//�x�_�㐂���⍄�ނ̗n�ڕӑ������،�
	string _shitenVsCutFu;					//�x�_�㐂���⍄�ނ̏㑤�،�
	string _kakutenVsCutWu;					//�i�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
	string _kakutenVsCutWd;					//�i�_�㐂���⍄�ނ̗n�ڕӑ������،�
	string _kakutenVsCutFu;					//�i�_�㐂���⍄�ނ̏㑤�،�
	string _middleVsCutWu;					//���Ԑ����⍄�ނ̗n�ڕӑ��㑤�،�
	string _middleVsCutWd;					//���Ԑ����⍄�ށi���k�^�C�v�j�̗n�ڕӑ������،�
	double _middleVsFreeSpace;				//���Ԑ����⍄�ށi�����^�C�v�j�̉����󂫗�
	//�����⍄��
	double _hsFreeSpaceVs;					//�����⍄�ނ̐����⍄�ޕ��A�����E�F�u���󂫗�
	double _hsFreeSpaceSpl;					//�����⍄�ނ̓Y�ڔ��󂫗�
	double _hsFreeSpaceCbf;					//�����⍄�ނ̉����t�����W���󂫗�
	double _hsFreeSpaceCbfUlimit;			//�����⍄�ނ̉����t�����W����̍������@���
	string _hsSnipSizeVs;					//�����⍄�ނ̐����⍄�ޕ��̃X�j�b�v�T�C�Y
	string _hsSnipSizeSpl;					//�����⍄�ނ̓Y�ڕ��̃X�j�b�v�T�C�Y
	string _hsSnipSizeCbf;					//�����⍄�ނ̉����t�����W���̃X�j�b�v�T�C�Y
	//�Y�ڔ�
	double _uflgSplKyojikuZaitan;			//��t�����W�Y�ڔ̋��������ޒ[
	double _uflgOutsideSplKyotyokuZaitan;	//��t�����W�O���Y�ڔ̋��������ޒ[
	double _uflgInsideSplKyotyokuZaitan;	//��t�����W�����Y�ڔ̋��������ޒ[
	double _webSplKyojikuZaitan;			//�E�F�u�Y�ڔ̋��������ޒ[
	double _webSplHeightZaitan;				//�E�F�u�Y�ڔ̍��������ޒ[
	double _lflgSplKyojikuZaitan;			//���t�����W�Y�ڔ̋��������ޒ[
	double _lflgSplKyotyokuZaitan;			//���t�����W�Y�ڔ̋��������ޒ[

};

#pragma once
#include "DiafManager.h"

class AtdCrossBeamCommon
{
public:
	AtdCrossBeamCommon() {}
	~AtdCrossBeamCommon() {}

	double &getShitenUflgJc() {return _shitenUflgJc;}
	double &getShitenWebJc() {return _shitenWebJc;}
	double &getShitenLflgJc() {return _shitenLflgJc;}
	double &getKakutenHflgJc() {return _kakutenHflgJc;}
	double &getKakutenHwebJc() {return _kakutenHwebJc;}
	string &getShitenConnCut() {return _shitenConnCut;}
	double &getShitenConnFillet() {return _shitenConnFillet;}
	double &getShitenConnTachiageryo() {return _shitenConnTachiageryo;}
	string &getKakutenConnCut() {return _kakutenConnCut;}
	double &getKakutenConnFillet() {return _kakutenConnFillet;}
	double &getKakutenConnTachiageryo() {return _kakutenConnTachiageryo;}
	string &getCvsCutWu() {return _cvsCutWu;}
	string &getCvsCutWd() {return _cvsCutWd;}
	double &getWebHoleSlopeLowerLimitCrs() {return _webHoleSlopeLowerLimitCrs;}
	EnTypeNo &getFlgSectionType() {return _flgSectionType;}
	double &getShitenUflgSplKyojikuZaitan() {return _shitenUflgSplKyojikuZaitan;}
	double &getShitenUflgSplKyotyokuZaitan() {return _shitenUflgSplKyotyokuZaitan;}
	double &getShitenWebSplKyotyokuZaitan() {return _shitenWebSplKyotyokuZaitan;}
	double &getShitenWebSplHeightZaitan() {return _shitenWebSplHeightZaitan;}
	double &getShitenLflgSplKyojikuZaitan() {return _shitenLflgSplKyojikuZaitan;}
	double &getShitenLflgSplKyotyokuZaitan() {return _shitenLflgSplKyotyokuZaitan;}
	double &getShitenConnKyojikuZaitan() {return _shitenConnKyojikuZaitan;}
	double &getShitenConnKyoutyokuZaitan() {return _shitenConnKyoutyokuZaitan;}
	double &getKakutenUflgSplKyojikuZaitan() {return _kakutenUflgSplKyojikuZaitan;}
	double &getKakutenUflgSplKyotyokuZaitan() {return _kakutenUflgSplKyotyokuZaitan;}
	double &getKakutenWebSplKyotyokuZaitan() {return _kakutenWebSplKyotyokuZaitan;}
	double &getKakutenWebSplHeightZaitan() {return _kakutenWebSplHeightZaitan;}
	double &getKakutenLflgSplKyojikuZaitan() {return _kakutenLflgSplKyojikuZaitan;}
	double &getKakutenLflgSplKyotyokuZaitan() {return _kakutenLflgSplKyotyokuZaitan;}
	double &getKakutenConnKyojikuZaitan() {return _kakutenConnKyojikuZaitan;}
	double &getKakutenConnKyoutyokuZaitan() {return _kakutenConnKyoutyokuZaitan;}

	const double &getShitenUflgJc() const {return _shitenUflgJc;}
	const double &getShitenWebJc() const {return _shitenWebJc;}
	const double &getShitenLflgJc() const {return _shitenLflgJc;}
	const double &getKakutenHflgJc() const {return _kakutenHflgJc;}
	const double &getKakutenHwebJc() const {return _kakutenHwebJc;}
	const string &getShitenConnCut() const {return _shitenConnCut;}
	const double &getShitenConnFillet() const {return _shitenConnFillet;}
	const double &getShitenConnTachiageryo() const {return _shitenConnTachiageryo;}
	const string &getKakutenConnCut() const {return _kakutenConnCut;}
	const double &getKakutenConnFillet() const {return _kakutenConnFillet;}
	const double &getKakutenConnTachiageryo() const {return _kakutenConnTachiageryo;}
	const string &getCvsCutWu() const {return _cvsCutWu;}
	const string &getCvsCutWd() const {return _cvsCutWd;}
	const double &getWebHoleSlopeLowerLimitCrs() const {return _webHoleSlopeLowerLimitCrs;}
	const EnTypeNo &getFlgSectionType() const {return _flgSectionType;}
	const double &getShitenUflgSplKyojikuZaitan() const {return _shitenUflgSplKyojikuZaitan;}
	const double &getShitenUflgSplKyotyokuZaitan() const {return _shitenUflgSplKyotyokuZaitan;}
	const double &getShitenWebSplKyotyokuZaitan() const {return _shitenWebSplKyotyokuZaitan;}
	const double &getShitenWebSplHeightZaitan() const {return _shitenWebSplHeightZaitan;}
	const double &getShitenLflgSplKyojikuZaitan() const {return _shitenLflgSplKyojikuZaitan;}
	const double &getShitenLflgSplKyotyokuZaitan() const {return _shitenLflgSplKyotyokuZaitan;}
	const double &getShitenConnKyojikuZaitan() const {return _shitenConnKyojikuZaitan;}
	const double &getShitenConnKyoutyokuZaitan() const {return _shitenConnKyoutyokuZaitan;}
	const double &getKakutenUflgSplKyojikuZaitan() const {return _kakutenUflgSplKyojikuZaitan;}
	const double &getKakutenUflgSplKyotyokuZaitan() const {return _kakutenUflgSplKyotyokuZaitan;}
	const double &getKakutenWebSplKyotyokuZaitan() const {return _kakutenWebSplKyotyokuZaitan;}
	const double &getKakutenWebSplHeightZaitan() const {return _kakutenWebSplHeightZaitan;}
	const double &getKakutenLflgSplKyojikuZaitan() const {return _kakutenLflgSplKyojikuZaitan;}
	const double &getKakutenLflgSplKyotyokuZaitan() const {return _kakutenLflgSplKyotyokuZaitan;}
	const double &getKakutenConnKyojikuZaitan() const {return _kakutenConnKyojikuZaitan;}
	const double &getKakutenConnKyoutyokuZaitan() const {return _kakutenConnKyoutyokuZaitan;}

	void setShitenUflgJc(const double &val) {_shitenUflgJc = val;}
	void setShitenWebJc(const double &val) {_shitenWebJc = val;}
	void setShitenLflgJc(const double &val) {_shitenLflgJc = val;}
	void setKakutenHflgJc(const double &val) {_kakutenHflgJc = val;}
	void setKakutenHwebJc(const double &val) {_kakutenHwebJc = val;}
	void setShitenConnCut(const string &val) {_shitenConnCut = val;}
	void setShitenConnFillet(const double &val) {_shitenConnFillet = val;}
	void setShitenConnTachiageryo(const double &val) {_shitenConnTachiageryo = val;}
	void setKakutenConnCut(const string &val) {_kakutenConnCut = val;}
	void setKakutenConnFillet(const double &val) {_kakutenConnFillet = val;}
	void setKakutenConnTachiageryo(const double &val) {_kakutenConnTachiageryo = val;}
	void setCvsCutWu(const string &val) {_cvsCutWu = val;}
	void setCvsCutWd(const string &val) {_cvsCutWd = val;}
	void setWebHoleSlopeLowerLimitCrs(const double &val) {_webHoleSlopeLowerLimitCrs = val;}
	void setFlgSectionType(const EnTypeNo &val) {_flgSectionType = val;}
	void setShitenUflgSplKyojikuZaitan(const double &val) {_shitenUflgSplKyojikuZaitan = val;}
	void setShitenUflgSplKyotyokuZaitan(const double &val) {_shitenUflgSplKyotyokuZaitan = val;}
	void setShitenWebSplKyotyokuZaitan(const double &val) {_shitenWebSplKyotyokuZaitan = val;}
	void setShitenWebSplHeightZaitan(const double &val) {_shitenWebSplHeightZaitan = val;}
	void setShitenLflgSplKyojikuZaitan(const double &val) {_shitenLflgSplKyojikuZaitan = val;}
	void setShitenLflgSplKyotyokuZaitan(const double &val) {_shitenLflgSplKyotyokuZaitan = val;}
	void setShitenConnKyojikuZaitan(const double &val) {_shitenConnKyojikuZaitan = val;}
	void setShitenConnKyoutyokuZaitan(const double &val) {_shitenConnKyoutyokuZaitan = val;}
	void setKakutenUflgSplKyojikuZaitan(const double &val) {_kakutenUflgSplKyojikuZaitan = val;}
	void setKakutenUflgSplKyotyokuZaitan(const double &val) {_kakutenUflgSplKyotyokuZaitan = val;}
	void setKakutenWebSplKyotyokuZaitan(const double &val) {_kakutenWebSplKyotyokuZaitan = val;}
	void setKakutenWebSplHeightZaitan(const double &val) {_kakutenWebSplHeightZaitan = val;}
	void setKakutenLflgSplKyojikuZaitan(const double &val) {_kakutenLflgSplKyojikuZaitan = val;}
	void setKakutenLflgSplKyotyokuZaitan(const double &val) {_kakutenLflgSplKyotyokuZaitan = val;}
	void setKakutenConnKyojikuZaitan(const double &val) {_kakutenConnKyojikuZaitan = val;}
	void setKakutenConnKyoutyokuZaitan(const double &val) {_kakutenConnKyoutyokuZaitan = val;}

private:
	//�W���C���g�N���A�����X
	double _shitenUflgJc;	//�x�_�㉡���iBH�j��t�����W�̃W���C���g�N���A�����X
	double _shitenWebJc;	//�x�_�㉡���iBH�j�E�F�u�̃W���C���g�N���A�����X
	double _shitenLflgJc;	//�x�_�㉡���iBH�j���t�����W�̃W���C���g�N���A�����X
	double _kakutenHflgJc;	//�i�_�㉡���iH�|�j�t�����W�̃W���C���g�N���A�����X
	double _kakutenHwebJc;	//�i�_�㉡���iH�|�j�E�F�u�̃W���C���g�N���A�����X
	//�R�l�N�V�����v���[�g
	string _shitenConnCut;				//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӑ��،�
	double _shitenConnFillet;			//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̃t�B���b�g��R�T�C�Y
	double _shitenConnTachiageryo;		//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӗ��グ��
	string _kakutenConnCut;				//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӑ��،����w��
	double _kakutenConnFillet;			//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̃t�B���b�g��R�T�C�Y
	double _kakutenConnTachiageryo;		//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӗ��グ��
	//�����⍄��
	string _cvsCutWu;					//�����t�����⍄�ނ̗n�ڕӑ��㑤�،�
	string _cvsCutWd;					//�����t�����⍄�ނ̗n�ڕӑ������،�
	//���̑�
	double _webHoleSlopeLowerLimitCrs;	//�E�F�u�E�̍E���z�̉����l
	EnTypeNo _flgSectionType;			//�t�����W�،��̕���
	//�ޒ[�`��
	double _shitenUflgSplKyojikuZaitan;		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	double _shitenUflgSplKyotyokuZaitan;	//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	double _shitenWebSplKyotyokuZaitan;		//�x�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
	double _shitenWebSplHeightZaitan;		//�x�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
	double _shitenLflgSplKyojikuZaitan;		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	double _shitenLflgSplKyotyokuZaitan;	//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	double _shitenConnKyojikuZaitan;		//�x�_��R�l�N�V�����̋��������ޒ[
	double _shitenConnKyoutyokuZaitan;		//�x�_��R�l�N�V�����̋��������ޒ[
	double _kakutenUflgSplKyojikuZaitan;	//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	double _kakutenUflgSplKyotyokuZaitan;	//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	double _kakutenWebSplKyotyokuZaitan;	//�i�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
	double _kakutenWebSplHeightZaitan;		//�i�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
	double _kakutenLflgSplKyojikuZaitan;	//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	double _kakutenLflgSplKyotyokuZaitan;	//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	double _kakutenConnKyojikuZaitan;		//�i�_��R�l�N�V�����̋��������ޒ[
	double _kakutenConnKyoutyokuZaitan;		//�i�_��R�l�N�V�����̋��������ޒ[

};


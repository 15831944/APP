#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//�Y�ڥ�e�Y�ڥ�t�����W
class AtdDbSplFlg;
class AtdDbSplFlgItem;

class AtdDbSplFlg
{
public:
	AtdDbSplFlg(void) {}
	~AtdDbSplFlg(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSplFlgItemList.size(); }
	void append(AtdDbSplFlgItem &atdDbSplFlgItem) { _atdDbSplFlgItemList.push_back(atdDbSplFlgItem); }
	JptErrorStatus getAt(int index, AtdDbSplFlgItem &atdDbSplFlgItem);

private:
	vector<AtdDbSplFlgItem> _atdDbSplFlgItemList;

};

class AtdDbSplFlgItem
{
public:
	AtdDbSplFlgItem(void) {}
	~AtdDbSplFlgItem(void) {}

	int &getStrcode() {return _strcode;}
	int &getPos() {return _pos;}
	int &getIupdw() {return _iupdw;}
	int &getNoj() {return _noj;}
	int &getIfmj() {return _ifmj;}
	double &getPoj() {return _poj;}
	double &getPj() {return _pj;}
	int &getNpj() {return _npj;}
	double &getP00j() {return _p00j;}
	int &getNpoj() {return _npoj;}
	double &getGoj() {return _goj;}
	double &getGj() {return _gj;}
	int &getNgj() {return _ngj;}
	int &getB1j() {return _b1j;}
	double &getT1j() {return _t1j;}
	double &getT2j() {return _t2j;}
	int &getMtj() {return _mtj;}
	double &getGjc() {return _gjc;}
	int &getNgjc() {return _ngjc;}
	double &getPjc() {return _pjc;}
	int &getNpjc() {return _npjc;}

	const int &getStrcode() const {return _strcode;}
	const int &getPos() const {return _pos;}
	const int &getIupdw() const {return _iupdw;}
	const int &getNoj() const {return _noj;}
	const int &getIfmj() const {return _ifmj;}
	const double &getPoj() const {return _poj;}
	const double &getPj() const {return _pj;}
	const int &getNpj() const {return _npj;}
	const double &getP00j() const {return _p00j;}
	const int &getNpoj() const {return _npoj;}
	const double &getGoj() const {return _goj;}
	const double &getGj() const {return _gj;}
	const int &getNgj() const {return _ngj;}
	const int &getB1j() const {return _b1j;}
	const double &getT1j() const {return _t1j;}
	const double &getT2j() const {return _t2j;}
	const int &getMtj() const {return _mtj;}
	const double &getGjc() const {return _gjc;}
	const int &getNgjc() const {return _ngjc;}
	const double &getPjc() const {return _pjc;}
	const int &getNpjc() const {return _npjc;}

	void setStrcode(const int &val) {_strcode = val;}
	void setPos(const int &val) {_pos = val;}
	void setIupdw(const int &val) {_iupdw = val;}
	void setNoj(const int &val) {_noj = val;}
	void setIfmj(const int &val) {_ifmj = val;}
	void setPoj(const double &val) {_poj = val;}
	void setPj(const double &val) {_pj = val;}
	void setNpj(const int &val) {_npj = val;}
	void setP00j(const double &val) {_p00j = val;}
	void setNpoj(const int &val) {_npoj = val;}
	void setGoj(const double &val) {_goj = val;}
	void setGj(const double &val) {_gj = val;}
	void setNgj(const int &val) {_ngj = val;}
	void setB1j(const int &val) {_b1j = val;}
	void setT1j(const double &val) {_t1j = val;}
	void setT2j(const double &val) {_t2j = val;}
	void setMtj(const int &val) {_mtj = val;}
	void setGjc(const double &val) {_gjc = val;}
	void setNgjc(const int &val) {_ngjc = val;}
	void setPjc(const double &val) {_pjc = val;}
	void setNpjc(const int &val) {_npjc = val;}

private:
	int _strcode;	//STRCODE
	int _pos;		//POS
	int _iupdw;		//IUPDW �㉺
	int _noj;		//NOJ �W���C���g�ԍ�
	int _ifmj;		//IFMJ �E�^�C�v FLG�E_�^�C�v
	double _poj;	//POJ CP
	double _pj;		//PJ P1 P FLG�E_P
	int _npj;		//NPJ P1�̐�
	double _p00j;	//P00J P2
	int _npoj;		//NPOJ P2�̐�
	double _goj;	//GOJ CG FLG�E_CG
	double _gj;		//GJ G FLG�E_G
	int _ngj;		//NGJ G�̐�
	int _b1j;		//B1J SPL�^�C�v SPL_�^�C�v
	double _t1j;	//T1J NS�� SPL_���i�O���j SPL_���i�����j
	double _t2j;	//T2J FS�� CONN_��
	int _mtj;		//MTJ NS�ގ� FS�ގ� SPL_�ގ��i�O���j SPL_�ގ��i�����j
	double _gjc;	//GJC CONN�E_���@X
	int _ngjc;		//NGJC CONN�E_���@X�̐�
	double _pjc;	//PJC CONN�E_���@Y
	int _npjc;		//NPJC CONN�E_���@Y�̐�

};


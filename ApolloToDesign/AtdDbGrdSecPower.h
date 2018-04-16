#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//�包�f�ʗ̓f�[�^
class AtdDbGrdSecPower;
class AtdDbGrdSecPowerItem;

class AtdDbGrdSecPower
{
public:
	AtdDbGrdSecPower(void) {}
	~AtdDbGrdSecPower(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbGrdSecPowerItemList.size(); }
	void append(AtdDbGrdSecPowerItem &atdDbGrdSecPowerItem) { _atdDbGrdSecPowerItemList.push_back(atdDbGrdSecPowerItem); }
	JptErrorStatus getAt(int index, AtdDbGrdSecPowerItem &atdDbGrdSecPowerItem);
	double findMageMx(const int& shosaPosNo, const int& secCaseNo);

private:
	vector<AtdDbGrdSecPowerItem> _atdDbGrdSecPowerItemList;

};

class AtdDbGrdSecPowerItem
{
public:
	AtdDbGrdSecPowerItem(void) {}
	~AtdDbGrdSecPowerItem(void) {}

	int &getBuzaiCode() {return _buzaiCode;}
	int &getSecCode() {return _secCode;}
	int &getShosaPosNo() {return _shosaPosNo;}
	int &getSecCaseNo() {return _secCaseNo;}
	double &getMageMx() {return _mageMx;}

	const int &getBuzaiCode() const {return _buzaiCode;}
	const int &getSecCode() const {return _secCode;}
	const int &getShosaPosNo() const {return _shosaPosNo;}
	const int &getSecCaseNo() const {return _secCaseNo;}
	const double &getMageMx() const {return _mageMx;}

	void setBuzaiCode(const int &val) {_buzaiCode = val;}
	void setSecCode(const int &val) {_secCode = val;}
	void ShosaPosNo(const int &val) {_shosaPosNo = val;}
	void SecCaseNo(const int &val) {_secCaseNo = val;}
	void setMageMx(const double &val) {_mageMx = val;}

private:
	int _buzaiCode;		//���ސ��R�[�h
	int _secCode;		//�f�ʃR�[�h
	int _shosaPosNo;	//�ƍ��ʒu�ԍ�
	int _secCaseNo;		//�f�ʗ̓P�[�X�ԍ�
	double _mageMx;		//�Ȃ����[�����g�i�ʓ��j�l��

};


#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//�f�ʥ�包�f��
class AtdDbSecGrd;
class AtdDbSecGrdItem;

class AtdDbSecGrd
{
public:
	AtdDbSecGrd(void) {}
	~AtdDbSecGrd(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecGrdItemList.size(); }
	void append(AtdDbSecGrdItem &atdDbSecGrdItem) { _atdDbSecGrdItemList.push_back(atdDbSecGrdItem); }
	JptErrorStatus getAt(int index, AtdDbSecGrdItem &atdDbSecGrdItem);

private:
	vector<AtdDbSecGrdItem> _atdDbSecGrdItemList;

};

class AtdDbSecGrdItem
{
public:
	AtdDbSecGrdItem(void) {}
	~AtdDbSecGrdItem(void) {}

	int &getNogrd() {return _nogrd;}
	int &getNosec() {return _nosec;}
	double &getBuf() {return _buf;}
	double &getTuf() {return _tuf;}
	int &getMuf() {return _muf;}
	double &getBlw() {return _blw;}
	int &getMlw() {return _mlw;}
	double &getBlf() {return _blf;}
	double &getTlf() {return _tlf;}
	int &getMlf() {return _mlf;}

	const int &getNogrd() const {return _nogrd;}
	const int &getNosec() const {return _nosec;}
	const double &getBuf() const {return _buf;}
	const double &getTuf() const {return _tuf;}
	const int &getMuf() const {return _muf;}
	const double &getBlw() const {return _blw;}
	const int &getMlw() const {return _mlw;}
	const double &getBlf() const {return _blf;}
	const double &getTlf() const {return _tlf;}
	const int &getMlf() const {return _mlf;}

	void setNogrd(const int &val) {_nogrd = val;}
	void setNosec(const int &val) {_nosec = val;}
	void setBuf(const double &val) {_buf = val;}
	void setTuf(const double &val) {_tuf = val;}
	void setMuf(const int &val) {_muf = val;}
	void setBlw(const double &val) {_blw = val;}
	void setMlw(const int &val) {_mlw = val;}
	void setBlf(const double &val) {_blf = val;}
	void setTlf(const double &val) {_tlf = val;}
	void setMlf(const int &val) {_mlf = val;}

private:
	int _nogrd;		//NOGRD ����
	int _nosec;		//NOSEC �f�ʔԍ�
	double _buf;	//BUF UFLG_��
	double _tuf;	//TUF UFLG_��
	int _muf;		//MUF UFLG_�ގ�
	double _blw;	//BLW WEB_��
	int _mlw;		//MLW WEB_�ގ�
	double _blf;	//BLF LFLG_��
	double _tlf;	//TLF LFLG_��
	int _mlf;		//MLF LFLG_�ގ�

};


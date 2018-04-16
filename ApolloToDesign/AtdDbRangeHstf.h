// 2018/02/28 take Add Start
#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector>

//�f�ʥ�����⍄�ނ̓���͈�(�ǉ�����)
class AtdDbRangeHstf;
class AtdDbRangeHstfItem;

class AtdDbRangeHstf
{
public:
	AtdDbRangeHstf() {}
	~AtdDbRangeHstf() {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() {return (int)_atdDbRangeHstfItemList.size();}
	void append(AtdDbRangeHstfItem &atdDbRangeHstfItem) {_atdDbRangeHstfItemList.push_back(atdDbRangeHstfItem);}
	JptErrorStatus getAt(int index, AtdDbRangeHstfItem &atdDbRangeHstfItem);

private:
	std::vector<AtdDbRangeHstfItem> _atdDbRangeHstfItemList;
};

class AtdDbRangeHstfItem
{
public:
	AtdDbRangeHstfItem() {}
	~AtdDbRangeHstfItem() {}

	int &getNoGrd() {return _noGrd;}
	int &getUplo() {return _uplo;}
	int &getNoHstf() {return _noHstf;}
	int &getNChang() {return _nChang;}
	double &getTLen1() {return _tLen1;}
	double &getTLen2() {return _tLen2;}

	const int &getNoGrd() const {return _noGrd;}
	const int &getUplo() const {return _uplo;}
	const int &getNoHstf() const {return _noHstf;}
	const int &getNChang() const {return _nChang;}
	const double &getTLen1() const {return _tLen1;}
	const double &getTLen2() const {return _tLen2;}

	void setNogrd(const int &nogrd) {_noGrd = nogrd;}
	void setUplo(const int &uplo) {_uplo = uplo;}
	void setNoHstf(const int &noHstf) {_noHstf = noHstf;}
	void setNChang(const int &nChang) {_nChang = nChang;}
	void setTLen1(const double &tLen1) {_tLen1 = tLen1;}
	void setTLen2(const double &tLen2) {_tLen2 = tLen2;}

private:
	int _noGrd;		//�包�ԍ�
	int _uplo;		//0:��i,1:���i
	int _noHstf;	//�A�ԍ�
	int _nChang;	//�i���ω����i_noHstf=-1�̂Ƃ��j
	double _tLen1;	//�ǉ�����
	double _tLen2;	//�ǉ�����
};
// 2018/02/28 take Add End
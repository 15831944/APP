// 2018/02/28 take Add Start
#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector>

//�f�ʥHSTF�̈ʒu�֌W(�i���ω�������i��)
class AtdDbStatusHstf;
class AtdDbStatusHstfItem;

class AtdDbStatusHstf
{
public:
	AtdDbStatusHstf() {}
	~AtdDbStatusHstf() {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() {return (int)_atdDbStatusHstfItemList.size();}
	void append(AtdDbStatusHstfItem &atdDbStatusHstfItem) {_atdDbStatusHstfItemList.push_back(atdDbStatusHstfItem);}
	JptErrorStatus getAt(int index, AtdDbStatusHstfItem &atdDbStatusHstfItem);

private:
	std::vector<AtdDbStatusHstfItem> _atdDbStatusHstfItemList;
};

class AtdDbStatusHstfItem
{
public:
	AtdDbStatusHstfItem() {}
	~AtdDbStatusHstfItem() {}

	int &getNoGrd() {return _noGrd;}
	int &getRlsfh() {return _rlsfh;}
	int &getNsfh() {return _nsfh;}

	const int &getNoGrd() const {return _noGrd;}
	const int &getRlsfh() const {return _rlsfh;}
	const int &getNsfh() const {return _nsfh;}

	void setNoGrd(const int &noGrd) {_noGrd = noGrd;}
	void setRlsfh(const int &rlsfh) {_rlsfh = rlsfh;}
	void setNsfh(const int &nsfh) {_nsfh = nsfh;}

private:
	int _noGrd;		//�包�ԍ�
	int _rlsfh;		//�i���ω���
	int _nsfh;		//�i��
};
// 2018/02/28 take Add End
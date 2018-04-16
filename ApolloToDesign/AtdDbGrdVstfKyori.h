#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//�包���Ԑ����⍄�ފԊu�f�[�^
class AtdDbGrdVstfKyori;
class AtdDbGrdVstfKyoriItem;

class AtdDbGrdVstfKyori
{
public:
	AtdDbGrdVstfKyori(void) {}
	~AtdDbGrdVstfKyori(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbGrdVstfKyoriItemList.size(); }
	void append(AtdDbGrdVstfKyoriItem &atdDbGrdVstfKyoriItem) { _atdDbGrdVstfKyoriItemList.push_back(atdDbGrdVstfKyoriItem); }
	JptErrorStatus getAt(int index, AtdDbGrdVstfKyoriItem &atdDbGrdVstfKyoriItem);

private:
	vector<AtdDbGrdVstfKyoriItem> _atdDbGrdVstfKyoriItemList;

};

class AtdDbGrdVstfKyoriItem
{
public:
	AtdDbGrdVstfKyoriItem(void) {}
	~AtdDbGrdVstfKyoriItem(void) {}

	int &getNostr() {return _nostr;}
	int &getNopanel() {return _nopanel;}
	int &getVstfNum() {return _vstfNum;}
	double &getVstfkyori() {return _vstfkyori;}

	const int &getNostr() const {return _nostr;}
	const int &getNopanel() const {return _nopanel;}
	const int &getVstfNum() const {return _vstfNum;}
	const double &getVstfkyori() const {return _vstfkyori;}

	void setNostr(const int &val) {_nostr = val;}
	void setNopanel(const int &val) {_nopanel = val;}
	void setVstfNum(const int &val) {_vstfNum = val;}
	void setVstfkyori(const double &val) {_vstfkyori = val;}

private:
	int _nostr;			//���ސ��R�[�h
	int _nopanel;		//�p�l���R�[�h
	int _vstfNum;		//�����⍄�ފԊu�ԍ�
	double _vstfkyori;	//�����⍄�ފԊu

};


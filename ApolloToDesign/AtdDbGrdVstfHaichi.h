#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//�包���Ԑ����⍄�ޔz�u�f�[�^
class AtdDbGrdVstfHaichi;
class AtdDbGrdVstfHaichiItem;

class AtdDbGrdVstfHaichi
{
public:
	AtdDbGrdVstfHaichi(void) {}
	~AtdDbGrdVstfHaichi(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbGrdVstfHaichiItemList.size(); }
	void append(AtdDbGrdVstfHaichiItem &atdDbGrdVstfHaichiItem) { _atdDbGrdVstfHaichiItemList.push_back(atdDbGrdVstfHaichiItem); }
	JptErrorStatus getAt(int index, AtdDbGrdVstfHaichiItem &atdDbGrdVstfHaichiItem);

private:
	vector<AtdDbGrdVstfHaichiItem> _atdDbGrdVstfHaichiItemList;

};

class AtdDbGrdVstfHaichiItem
{
public:
	AtdDbGrdVstfHaichiItem(void) {}
	~AtdDbGrdVstfHaichiItem(void) {}

	int &getNostr() {return _nostr;}
	int &getNopanel() {return _nopanel;}
	int &getNumvstf() {return _numvstf;}

	const int &getNostr() const {return _nostr;}
	const int &getNopanel() const {return _nopanel;}
	const int &getNumvstf() const {return _numvstf;}

	void setNostr(const int &val) {_nostr = val;}
	void setNopanel(const int &val) {_nopanel = val;}
	void setNumvstf(const int &val) {_numvstf = val;}

private:
	int _nostr;		//���ސ��R�[�h
	int _nopanel;	//�p�l���R�[�h
	int _numvstf;	//�����⍄�ޖ{��

};


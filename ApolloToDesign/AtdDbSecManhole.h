#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//�f�ʥ���[���̃}���z�[���̃J�b�g
class AtdDbSecManhole;
class AtdDbSecManholeItem;

class AtdDbSecManhole
{
public:
	AtdDbSecManhole(void) {}
	~AtdDbSecManhole(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbSecManholeItemList.size(); }
	void append(AtdDbSecManholeItem &atdDbSecManholeItem) { _atdDbSecManholeItemList.push_back(atdDbSecManholeItem); }
	JptErrorStatus getAt(int index, AtdDbSecManholeItem &atdDbSecManholeItem);

private:
	vector<AtdDbSecManholeItem> _atdDbSecManholeItemList;

};

class AtdDbSecManholeItem
{
public:
	AtdDbSecManholeItem(void) {}
	~AtdDbSecManholeItem(void) {}

	int &getNogrd() {return _nogrd;}
	int &getSepos() {return _sepos;}
	int &getIwcut() {return _iwcut;}

	const int &getNogrd() const {return _nogrd;}
	const int &getSepos() const {return _sepos;}
	const int &getIwcut() const {return _iwcut;}

	void setNogrd(const int &val) {_nogrd = val;}
	void setSepos(const int &val) {_sepos = val;}
	void setIwcut(const int &val) {_iwcut = val;}

private:
	int _nogrd;	//NOGRD ����
	int _sepos;	//SEPOS �z�u��
	int _iwcut;	//IWCUT
};


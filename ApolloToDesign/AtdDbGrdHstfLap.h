#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector> 
using namespace std;

//�包�����⍄�ރ��b�v�͈�
class AtdDbGrdHstfLap;
class AtdDbGrdHstfLapItem;

class AtdDbGrdHstfLap
{
public:
	AtdDbGrdHstfLap(void) {}
	~AtdDbGrdHstfLap(void) {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbGrdHstfLapItemList.size(); }
	void append(AtdDbGrdHstfLapItem& atdDbGrdHstfLapItem) { _atdDbGrdHstfLapItemList.push_back(atdDbGrdHstfLapItem); }
	JptErrorStatus getAt(int index, AtdDbGrdHstfLapItem& atdDbGrdHstfLapItem);

private:
	vector<AtdDbGrdHstfLapItem> _atdDbGrdHstfLapItemList;

};

class AtdDbGrdHstfLapItem
{
public:
	AtdDbGrdHstfLapItem(void) {}
	~AtdDbGrdHstfLapItem(void) {}

	int &getBuzaiCode() {return _buzaiCode;}
	int &getHstfPos() {return _hstfPos;}
	int &getHstfCode() {return _hstfCode;}
	int &getHstfNum() {return _hstfNum;}

	const int &getBuzaiCode() const {return _buzaiCode;}
	const int &getHstfPos() const {return _hstfPos;}
	const int &getHstfCode() const {return _hstfCode;}
	const int &getHstfNum() const {return _hstfNum;}

	void setBuzaiCode(const int &val) {_buzaiCode = val;}
	void setHstfPos(const int &val) {_hstfPos = val;}
	void setHstfCode(const int &val) {_hstfCode = val;}
	void setHstfNum(const int &val) {_hstfNum = val;}

private:
	int _buzaiCode;	//���ސ��R�[�h
	int _hstfPos;	//�����⍄�ގ�t�ʒu
	int _hstfCode;	//�����⍄�ޒf�ʃR�[�h
	int _hstfNum;	//�����⍄�ޒi��

};


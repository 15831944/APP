// 2018/02/26 take Add Start
#pragma once

#include "JptDef.h"
#include <afxdao.h>
#include <ExtCDaoDatabase.h>
#include <vector>

//�����݌v�
class AtdDbBaseData;
class AtdDbBaseDataItem;

class AtdDbBaseData
{
public:
	AtdDbBaseData() {}
	~AtdDbBaseData() {}

	JptErrorStatus load(CDaoDatabase& dbFile);
	int size() { return (int)_atdDbBaseDataItemList.size(); }
	void append(AtdDbBaseDataItem &atdDbBaseDataItem) { _atdDbBaseDataItemList.push_back(atdDbBaseDataItem); }
	JptErrorStatus getAt(int index, AtdDbBaseDataItem &atdDbBaseDataItem);
	JptErrorStatus judgeBrdgType(bool &brdgTypeFlg);

private:
	std::vector<AtdDbBaseDataItem> _atdDbBaseDataItemList;

};

class AtdDbBaseDataItem
{
public:
	AtdDbBaseDataItem() {}
	~AtdDbBaseDataItem() {}

	int &getBrdgType() {return _brdgType;}
	int &getCrossType() {return _crossType;}

	const int &getBrdgType() const {return _brdgType;}
	const int &getCrossType() const {return _crossType;}

	void setBrdgType(const int &brdgType) {_brdgType = brdgType;}
	void setCrossType(const int &crossType) {_crossType = crossType;}

private:
	int _brdgType;		//�����`���i1�FRC�包,2�FRC����,3�F�|�����包,4�F�|���Ŕ����j
	int _crossType;		//���g�\���`���i0�F�W���`��,1�F���包�`���j
};
// 2018/02/26 take Add End
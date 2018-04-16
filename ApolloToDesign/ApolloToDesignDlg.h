// ApolloToDesignDlg.h : �w�b�_�[ �t�@�C��
//

#include "MyTabCtrl.h"
#include "AtdGirderCommon.h"
#include "AtdCrossBeamCommon.h"
#include "resource.h"
#include <string>
#include <fstream>
using namespace std;

#if !defined(AFX_APOLLOTODESIGNDLG_H__5A0CA2F4_7761_40F0_8933_C33B3125ACAD__INCLUDED_)
#define AFX_APOLLOTODESIGNDLG_H__5A0CA2F4_7761_40F0_8933_C33B3125ACAD__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CApolloToDesignDlg �_�C�A���O

class CApolloToDesignDlg : public CDialog
{
// �\�z
public:
	CApolloToDesignDlg(CWnd* pParent = NULL);	// �W���̃R���X�g���N�^
// �_�C�A���O �f�[�^
	//{{AFX_DATA(CApolloToDesignDlg)
	enum { IDD = IDD_APOLLOTODESIGN_DIALOG };
	CMyTabCtrl	m_MainTab;
	CString	m_MdbFilePath;
	CString	m_CsvFilePath;
	//�包
	//�W���C���g�N���A�����X
	CString	m_UflgJc;			//��t�����W�̃W���C���g�N���A�����X
	CString	m_WebJc;			//�E�F�u�̃W���C���g�N���A�����X
	CString	m_LflgJc;			//���t�����W�̃W���C���g�N���A�����X
	//�ޒ[�`��
	int		m_UflgZaitanKeijo;					//��t�����W�ޒ[�`��
	int		m_LflgZaitanKeijo;					//���t�����W�ޒ[�`��
	CString	m_UpdownFlgZaitanKeijoTachiageRyo;	//�㉺�t�����W�ޒ[�`�󗧏グ��
	//�\�[���v���[�g
	CString	m_SolePlateKyotyokuFreeSpace;		//�\�[���v���[�g�̋��������󂫗�
	CString	m_SolePlateKyojikuFreeSpace;		//�\�[���v���[�g�̋��������󂫗�
	//���̑�
	CString	m_ItatsugiZureRyo;					//�p�Y����
	CString	m_LflgKakuhukubuTaper;				//���t�����W�g�����̃e�[�p�[���z
	CString	m_WebHoleSlopeLowerLimitGrd;		//�E�F�u�E�̍E���z�̉����l
	//�����⍄��
	CString	m_ShitenVsCutWu;					//�x�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
	CString	m_ShitenVsCutWd;					//�x�_�㐂���⍄�ނ̗n�ڕӑ������،�
	CString	m_ShitenVsCutFu;					//�x�_�㐂���⍄�ނ̏㑤�،�
	CString	m_KakutenVsCutWu;					//�i�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
	CString	m_KakutenVsCutWd;					//�i�_�㐂���⍄�ނ̗n�ڕӑ������،�
	CString	m_KakutenVsCutFu;					//�i�_�㐂���⍄�ނ̏㑤�،�
	CString	m_MiddleVsCutWu;					//���Ԑ����⍄�ނ̗n�ڕӑ��㑤�،�
	CString	m_MiddleVsCutWd;					//���Ԑ����⍄�ށi���k�^�C�v�j�̗n�ڕӑ������،�
	CString	m_MiddleVsFreeSpace;				//���Ԑ����⍄�ށi�����^�C�v�j�̉����󂫗�
	//�����⍄��
	CString	m_HsFreeSpaceVs;					//�����⍄�ނ̐����⍄�ޕ��A�����E�F�u���󂫗�
	CString	m_HsFreeSpaceSpl;					//�����⍄�ނ̓Y�ڔ��󂫗�
	CString	m_HsFreeSpaceCbf;					//�����⍄�ނ̉����t�����W���󂫗�
	CString	m_HsFreeSpaceCbfUlimit;				//�����⍄�ނ̉����t�����W����̍������@���
	CString	m_HsSnipSizeVs;						//�����⍄�ނ̐����⍄�ޕ��̃X�j�b�v�T�C�Y
	CString	m_HsSnipSizeSpl;					//�����⍄�ނ̓Y�ڕ��̃X�j�b�v�T�C�Y
	CString	m_HsSnipSizeCbf;					//�����⍄�ނ̉����t�����W���̃X�j�b�v�T�C�Y
	//�Y�ڔ�
	CString	m_UflgSplKyojikuZaitan;				//��t�����W�Y�ڔ̋��������ޒ[
	CString	m_UflgOutsideSplKyotyokuZaitan;		//��t�����W�O���Y�ڔ̋��������ޒ[
	CString	m_UflgInsideSplKyotyokuZaitan;		//��t�����W�����Y�ڔ̋��������ޒ[
	CString	m_WebSplKyojikuZaitan;				//�E�F�u�Y�ڔ̋��������ޒ[
	CString	m_WebSplHeightZaitan;				//�E�F�u�Y�ڔ̍��������ޒ[
	CString	m_LflgSplKyojikuZaitan;				//���t�����W�Y�ڔ̋��������ޒ[
	CString	m_LflgSplKyotyokuZaitan;			//���t�����W�Y�ڔ̋��������ޒ[
	//����
	//�W���C���g�N���A�����X
	CString	m_ShitenUflgJc;		//�x�_�㉡���iBH�j��t�����W�̃W���C���g�N���A�����X
	CString	m_ShitenWebJc;		//�x�_�㉡���iBH�j�E�F�u�̃W���C���g�N���A�����X
	CString	m_ShitenLflgJc;		//�x�_�㉡���iBH�j���t�����W�̃W���C���g�N���A�����X
	CString	m_KakutenHflgJc;	//�i�_�㉡���iH�|�j�t�����W�̃W���C���g�N���A�����X
	CString	m_KakutenHwebJc;	//�i�_�㉡���iH�|�j�E�F�u�̃W���C���g�N���A�����X
	//�R�l�N�V�����v���[�g
	CString	m_ShitenConnCut;				//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӑ��،�
	CString	m_ShitenConnFillet;				//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̃t�B���b�g��R�T�C�Y
	CString	m_ShitenConnTachiageryo;		//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӗ��グ��
	CString	m_KakutenConnCut;				//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӑ��،����w��
	CString	m_KakutenConnFillet;			//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̃t�B���b�g��R�T�C�Y
	CString	m_KakutenConnTachiageryo;		//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӗ��グ��
	//�����⍄��
	CString	m_CvsCutWu;						//�����t�����⍄�ނ̗n�ڕӑ��㑤�،�
	CString	m_CvsCutWd;						//�����t�����⍄�ނ̗n�ڕӑ������،�
	//���̑�
	CString	m_WebHoleSlopeLowerLimitCrs;	//�E�F�u�E�̍E���z�̉����l
	int		m_FlgSectionType;				//�t�����W�،��̕���
	//�ޒ[�`��
	CString	m_ShitenUflgSplKyojikuZaitan;		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	CString	m_ShitenUflgSplKyotyokuZaitan;		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	CString	m_ShitenWebSplKyotyokuZaitan;		//�x�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
	CString	m_ShitenWebSplHeightZaitan;			//�x�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
	CString	m_ShitenLflgSplKyojikuZaitan;		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	CString	m_ShitenLflgSplKyotyokuZaitan;		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	CString	m_ShitenConnKyojikuZaitan;			//�x�_��R�l�N�V�����̋��������ޒ[
	CString	m_ShitenConnKyoutyokuZaitan;		//�x�_��R�l�N�V�����̋��������ޒ[
	CString	m_KakutenUflgSplKyojikuZaitan;		//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	CString	m_KakutenUflgSplKyotyokuZaitan;		//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	CString	m_KakutenWebSplKyotyokuZaitan;		//�i�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
	CString	m_KakutenWebSplHeightZaitan;		//�i�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
	CString	m_KakutenLflgSplKyojikuZaitan;		//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	CString	m_KakutenLflgSplKyotyokuZaitan;		//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	CString	m_KakutenConnKyojikuZaitan;			//�i�_��R�l�N�V�����̋��������ޒ[
	CString	m_KakutenConnKyoutyokuZaitan;		//�i�_��R�l�N�V�����̋��������ޒ[
		// ����: ���̈ʒu�� ClassWizard �ɂ���ăf�[�^ �����o���ǉ�����܂��B
	//}}AFX_DATA

	// ClassWizard �͉��z�֐��̃I�[�o�[���C�h�𐶐����܂��B
	//{{AFX_VIRTUAL(CApolloToDesignDlg)
protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV �̃T�|�[�g
	//}}AFX_VIRTUAL

// �C���v�������e�[�V����
protected:
	HICON m_hIcon;

	// �������ꂽ���b�Z�[�W �}�b�v�֐�
	//{{AFX_MSG(CApolloToDesignDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnMdbFileBrowse();
	virtual void OnOK();
	virtual void OnCancel();
	afx_msg BOOL storePreviousInput(AtdGirderCommon agc, AtdCrossBeamCommon acc, string fileInputStorage);
	afx_msg BOOL readPreviousInput(const int type, string fileInputStorage);
	afx_msg void initializeGui(const int type);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	bool fileExists(LPCTSTR filename);
	void printMessage(CString& result);

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ �͑O�s�̒��O�ɒǉ��̐錾��}�����܂��B

#endif // !defined(AFX_APOLLOTODESIGNDLG_H__5A0CA2F4_7761_40F0_8933_C33B3125ACAD__INCLUDED_)

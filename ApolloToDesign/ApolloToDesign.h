// ApolloToDesign.h : APOLLOTODESIGN �A�v���P�[�V�����̃��C�� �w�b�_�[ �t�@�C���ł��B
//

#if !defined(AFX_APOLLOTODESIGN_H__37A4472C_3958_44D3_A357_61E5950F9629__INCLUDED_)
#define AFX_APOLLOTODESIGN_H__37A4472C_3958_44D3_A357_61E5950F9629__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// ���C�� �V���{��
#include "locale.h"

/////////////////////////////////////////////////////////////////////////////
// CApolloToDesignApp:
// ���̃N���X�̓���̒�`�Ɋւ��Ă� ApolloToDesign.cpp �t�@�C�����Q�Ƃ��Ă��������B
//

class CApolloToDesignApp : public CWinApp
{
public:
	CApolloToDesignApp();

// �I�[�o�[���C�h
	// ClassWizard �͉��z�֐��̃I�[�o�[���C�h�𐶐����܂��B
	//{{AFX_VIRTUAL(CApolloToDesignApp)
	public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();
	//}}AFX_VIRTUAL

// �C���v�������e�[�V����

	//{{AFX_MSG(CApolloToDesignApp)
		// ���� - ClassWizard �͂��̈ʒu�Ƀ����o�֐���ǉ��܂��͍폜���܂��B
		//        ���̈ʒu�ɐ��������R�[�h��ҏW���Ȃ��ł��������B
	//}}AFX_MSG

private:
	BOOL	m_bInitOD;
	DECLARE_MESSAGE_MAP()
};

// Output to log file
void log_MessageBox(CString strMsg);
void log_Msg(const std::string& strMsg);

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ �͑O�s�̒��O�ɒǉ��̐錾��}�����܂��B

#endif // !defined(AFX_APOLLOTODESIGN_H__37A4472C_3958_44D3_A357_61E5950F9629__INCLUDED_)

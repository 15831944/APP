// DlgCrossBeam.cpp : �����t�@�C��
//

#include "stdafx.h"
#include "ApolloToDesign_main.h"
#include "DlgCrossBeam.h"

// CDlgCrossBeam �_�C�A���O

IMPLEMENT_DYNAMIC(CDlgCrossBeam, CDialog)

CDlgCrossBeam::CDlgCrossBeam(CWnd* pParent /*=NULL*/)
	: CDialog(CDlgCrossBeam::IDD, pParent)
{
}

CDlgCrossBeam::~CDlgCrossBeam()
{
}

void CDlgCrossBeam::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgCrossBeam, CDialog)
END_MESSAGE_MAP()

BOOL CDlgCrossBeam::OnInitDialog() 
{
	CDialog::OnInitDialog();
	((CComboBox*)GetDlgItem(IDC_CBS31))->AddString(_T("�^�C�v�P"));
	((CComboBox*)GetDlgItem(IDC_CBS31))->AddString(_T("�^�C�v�Q"));
	((CComboBox*)GetDlgItem(IDC_CBS31))->AddString(_T("�^�C�v�R"));

	return TRUE;
}


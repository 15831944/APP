// DlgGirder.cpp : �����t�@�C��
//

#include "stdafx.h"
#include "ApolloToDesign_main.h"
#include "DlgGirder.h"

// CDlgGirder �_�C�A���O

IMPLEMENT_DYNAMIC(CDlgGirder, CDialog)

CDlgGirder::CDlgGirder(CWnd* pParent /*=NULL*/)
	: CDialog(CDlgGirder::IDD, pParent)
{
}

CDlgGirder::~CDlgGirder()
{
}

void CDlgGirder::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgGirder, CDialog)
END_MESSAGE_MAP()

BOOL CDlgGirder::OnInitDialog() 
{
	CDialog::OnInitDialog();
	((CComboBox*)GetDlgItem(IDC_GRD7))->AddString(_T("�^�C�v�P"));
	((CComboBox*)GetDlgItem(IDC_GRD7))->AddString(_T("�^�C�v�Q"));
	((CComboBox*)GetDlgItem(IDC_GRD7))->AddString(_T("�^�C�v�R"));
	((CComboBox*)GetDlgItem(IDC_GRD8))->AddString(_T("�^�C�v�P"));
	((CComboBox*)GetDlgItem(IDC_GRD8))->AddString(_T("�^�C�v�Q"));
	((CComboBox*)GetDlgItem(IDC_GRD8))->AddString(_T("�^�C�v�R"));

	return TRUE;
}


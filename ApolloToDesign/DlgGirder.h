#pragma once

// CDlgGirder �_�C�A���O

class CDlgGirder : public CDialog
{
	DECLARE_DYNAMIC(CDlgGirder)

public:
	CDlgGirder(CWnd* pParent = NULL);   // �W���R���X�g���N�^
	virtual ~CDlgGirder();

// �_�C�A���O �f�[�^
	enum { IDD = DLG_Girder };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �T�|�[�g
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
};

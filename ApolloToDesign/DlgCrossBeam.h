#pragma once

// CDlgCrossBeam �_�C�A���O

class CDlgCrossBeam : public CDialog
{
	DECLARE_DYNAMIC(CDlgCrossBeam)

public:
	CDlgCrossBeam(CWnd* pParent = NULL);   // �W���R���X�g���N�^
	virtual ~CDlgCrossBeam();

// �_�C�A���O �f�[�^
	enum { IDD = DLG_CrossBeam };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �T�|�[�g
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
};


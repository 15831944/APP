// ApolloToDesignDlg.cpp : �C���v�������e�[�V���� �t�@�C��
//

#include "stdafx.h"
#include "ApolloToDesign_main.h"
#include "ApolloToDesignDlg.h"
#include "StringTokenizer.h"
#include "LogPrint.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define INPUTSTORAGE "\\ApolloToDesign.gmn"

HINSTANCE hTheApp;
extern CWnd* m_pMainWnd;       // main window

/////////////////////////////////////////////////////////////////////////////
// �A�v���P�[�V�����̃o�[�W�������Ŏg���Ă��� CAboutDlg �_�C�A���O

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// �_�C�A���O �f�[�^
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard �͉��z�֐��̃I�[�o�[���C�h�𐶐����܂�
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �̃T�|�[�g
	//}}AFX_VIRTUAL

// �C���v�������e�[�V����
protected:
	//{{AFX_MSG(CAboutDlg)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// ���b�Z�[�W �n���h��������܂���B
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CApolloToDesignDlg �_�C�A���O

CApolloToDesignDlg::CApolloToDesignDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CApolloToDesignDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CApolloToDesignDlg)
		// ����: ���̈ʒu�� ClassWizard �ɂ���ă����o�̏��������ǉ�����܂��B
	m_MdbFilePath = _T("");
	m_CsvFilePath = _T("");
	//�W���C���g�N���A�����X
	m_UflgJc = _T("");			//��t�����W�̃W���C���g�N���A�����X
	m_WebJc = _T("");			//�E�F�u�̃W���C���g�N���A�����X
	m_LflgJc = _T("");			//���t�����W�̃W���C���g�N���A�����X
	//�ޒ[�`��
	m_UflgZaitanKeijo = 0;		//��t�����W�ޒ[�`��
	m_LflgZaitanKeijo = 0;		//���t�����W�ޒ[�`��
	m_UpdownFlgZaitanKeijoTachiageRyo = _T("");	//�㉺�t�����W�ޒ[�`�󗧏グ��
	//�\�[���v���[�g
	m_SolePlateKyotyokuFreeSpace = _T("");		//�\�[���v���[�g�̋��������󂫗�
	m_SolePlateKyojikuFreeSpace = _T("");		//�\�[���v���[�g�̋��������󂫗�
	//���̑�
	m_ItatsugiZureRyo = _T("");					//�p�Y����
	m_LflgKakuhukubuTaper = _T("");				//���t�����W�g�����̃e�[�p�[���z
	m_WebHoleSlopeLowerLimitGrd = _T("");		//�E�F�u�E�̍E���z�̉����l
	//�����⍄��
	m_ShitenVsCutWu = _T("");					//�x�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
	m_ShitenVsCutWd = _T("");					//�x�_�㐂���⍄�ނ̗n�ڕӑ������،�
	m_ShitenVsCutFu = _T("");					//�x�_�㐂���⍄�ނ̏㑤�،�
	m_KakutenVsCutWu = _T("");					//�i�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
	m_KakutenVsCutWd = _T("");					//�i�_�㐂���⍄�ނ̗n�ڕӑ������،�
	m_KakutenVsCutFu = _T("");					//�i�_�㐂���⍄�ނ̏㑤�،�
	m_MiddleVsCutWu = _T("");					//���Ԑ����⍄�ނ̗n�ڕӑ��㑤�،�
	m_MiddleVsCutWd = _T("");					//���Ԑ����⍄�ށi���k�^�C�v�j�̗n�ڕӑ������،�
	m_MiddleVsFreeSpace = _T("");				//���Ԑ����⍄�ށi�����^�C�v�j�̉����󂫗�
	//�����⍄��
	m_HsFreeSpaceVs = _T("");					//�����⍄�ނ̐����⍄�ޕ��A�����E�F�u���󂫗�
	m_HsFreeSpaceSpl = _T("");					//�����⍄�ނ̓Y�ڔ��󂫗�
	m_HsFreeSpaceCbf = _T("");					//�����⍄�ނ̉����t�����W���󂫗�
	m_HsFreeSpaceCbfUlimit = _T("");			//�����⍄�ނ̉����t�����W����̍������@���
	m_HsSnipSizeVs = _T("");					//�����⍄�ނ̐����⍄�ޕ��̃X�j�b�v�T�C�Y
	m_HsSnipSizeSpl = _T("");					//�����⍄�ނ̓Y�ڕ��̃X�j�b�v�T�C�Y
	m_HsSnipSizeCbf = _T("");					//�����⍄�ނ̉����t�����W���̃X�j�b�v�T�C�Y
	//�Y�ڔ�
	m_UflgSplKyojikuZaitan = _T("");			//��t�����W�Y�ڔ̋��������ޒ[
	m_UflgOutsideSplKyotyokuZaitan = _T("");	//��t�����W�O���Y�ڔ̋��������ޒ[
	m_UflgInsideSplKyotyokuZaitan = _T("");		//��t�����W�����Y�ڔ̋��������ޒ[
	m_WebSplKyojikuZaitan = _T("");				//�E�F�u�Y�ڔ̋��������ޒ[
	m_WebSplHeightZaitan = _T("");				//�E�F�u�Y�ڔ̍��������ޒ[
	m_LflgSplKyojikuZaitan = _T("");			//���t�����W�Y�ڔ̋��������ޒ[
	m_LflgSplKyotyokuZaitan = _T("");			//���t�����W�Y�ڔ̋��������ޒ[
	m_ShitenUflgJc = _T("");		//�x�_�㉡���iBH�j��t�����W�̃W���C���g�N���A�����X
	m_ShitenWebJc = _T("");			//�x�_�㉡���iBH�j�E�F�u�̃W���C���g�N���A�����X
	m_ShitenLflgJc = _T("");		//�x�_�㉡���iBH�j���t�����W�̃W���C���g�N���A�����X
	m_KakutenHflgJc = _T("");		//�i�_�㉡���iH�|�j�t�����W�̃W���C���g�N���A�����X
	m_KakutenHwebJc = _T("");		//�i�_�㉡���iH�|�j�E�F�u�̃W���C���g�N���A�����X
	//�R�l�N�V�����v���[�g
	m_ShitenConnCut = _T("");			//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӑ��،�
	m_ShitenConnFillet = _T("");		//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̃t�B���b�g��R�T�C�Y
	m_ShitenConnTachiageryo = _T("");	//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӗ��グ��
	m_KakutenConnCut = _T("");			//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӑ��،����w��
	m_KakutenConnFillet = _T("");		//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̃t�B���b�g��R�T�C�Y
	m_KakutenConnTachiageryo = _T("");	//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӗ��グ��
	//�����⍄��
	m_CvsCutWu = _T("");					//�����t�����⍄�ނ̗n�ڕӑ��㑤�،�
	m_CvsCutWd = _T("");					//�����t�����⍄�ނ̗n�ڕӑ������،�
	//���̑�
	m_WebHoleSlopeLowerLimitCrs = _T("");	//�E�F�u�E�̍E���z�̉����l
	m_FlgSectionType = 0;					//�t�����W�،��̕���
	//�ޒ[�`��
	m_ShitenUflgSplKyojikuZaitan = _T("");		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	m_ShitenUflgSplKyotyokuZaitan = _T("");		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	m_ShitenWebSplKyotyokuZaitan = _T("");		//�x�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
	m_ShitenWebSplHeightZaitan = _T("");		//�x�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
	m_ShitenLflgSplKyojikuZaitan = _T("");		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	m_ShitenLflgSplKyotyokuZaitan = _T("");		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	m_ShitenConnKyojikuZaitan = _T("");			//�x�_��R�l�N�V�����̋��������ޒ[
	m_ShitenConnKyoutyokuZaitan = _T("");		//�x�_��R�l�N�V�����̋��������ޒ[
	m_KakutenUflgSplKyojikuZaitan = _T("");		//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	m_KakutenUflgSplKyotyokuZaitan = _T("");	//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	m_KakutenWebSplKyotyokuZaitan = _T("");		//�i�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
	m_KakutenWebSplHeightZaitan = _T("");		//�i�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
	m_KakutenLflgSplKyojikuZaitan = _T("");		//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	m_KakutenLflgSplKyotyokuZaitan = _T("");	//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	m_KakutenConnKyojikuZaitan = _T("");		//�i�_��R�l�N�V�����̋��������ޒ[
	//}}AFX_DATA_INIT
	// ����: LoadIcon �� Win32 �� DestroyIcon �̃T�u�V�[�P���X��v�����܂���B
	m_hIcon = ::LoadIcon(hTheApp, MAKEINTRESOURCE(IDR_MAINFRAME));
}

void CApolloToDesignDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CApolloToDesignDlg)
	DDX_Control(pDX, IDC_MAINTAB, m_MainTab);
	DDX_Text(pDX, IDC_MDBFILEPATH, m_MdbFilePath);
	DDX_Text(pDX, IDC_CSVFILEPATH, m_CsvFilePath);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CApolloToDesignDlg, CDialog)
	//{{AFX_MSG_MAP(CApolloToDesignDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_MDBFILEBROWSE, OnMdbFileBrowse)
	ON_WM_CLOSE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CApolloToDesignDlg ���b�Z�[�W �n���h��
BOOL CApolloToDesignDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// IDM_ABOUTBOX �̓R�}���h ���j���[�͈̔͂łȂ���΂Ȃ�܂���B
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// ���̃_�C�A���O�p�̃A�C�R����ݒ肵�܂��B�t���[�����[�N�̓A�v���P�[�V�����̃��C��
	// �E�B���h�E���_�C�A���O�łȂ����͎����I�ɐݒ肵�܂���B
	SetIcon(m_hIcon, TRUE);			// �傫���A�C�R����ݒ�
	SetIcon(m_hIcon, FALSE);		// �������A�C�R����ݒ�
	
	// TODO: ���ʂȏ��������s�����͂��̏ꏊ�ɒǉ����Ă��������B
	m_MainTab.InitDialogs();
	m_MainTab.InsertItem(0, _T("�包���ʏڍ�"));
	m_MainTab.InsertItem(1, _T("�������ʏڍ�"));
	m_MainTab.SetCurSel(0);
	m_MainTab.InitTabDialog();
	//
	initializeGui(0);

	return TRUE;  // TRUE ��Ԃ��ƃR���g���[���ɐݒ肵���t�H�[�J�X�͎����܂���B
}

void CApolloToDesignDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// �����_�C�A���O�{�b�N�X�ɍŏ����{�^����ǉ�����Ȃ�΁A�A�C�R����`�悷��
// �R�[�h���ȉ��ɋL�q����K�v������܂��BMFC �A�v���P�[�V������ document/view
// ���f�����g���Ă���̂ŁA���̏����̓t���[�����[�N�ɂ�莩���I�ɏ�������܂��B

void CApolloToDesignDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // �`��p�̃f�o�C�X �R���e�L�X�g

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// �N���C�A���g�̋�`�̈���̒���
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// �A�C�R����`�悵�܂��B
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// �V�X�e���́A���[�U�[���ŏ����E�B���h�E���h���b�O���Ă���ԁA
// �J�[�\����\�����邽�߂ɂ������Ăяo���܂��B
HCURSOR CApolloToDesignDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

void CApolloToDesignDlg::OnMdbFileBrowse() 
{
	UpdateData(TRUE);

	CFileDialog fileDlg(TRUE,_T("mdb"),_T("*.mdb"),OFN_HIDEREADONLY|OFN_OVERWRITEPROMPT,_T("(*.mdb)|*.mdb||"),NULL);
	if (fileDlg.DoModal()==IDOK)
	{
		m_MdbFilePath = fileDlg.GetPathName();
		string sekkeiFilePath = m_MdbFilePath.GetBuffer();
		string dataDir = "";
		if(sekkeiFilePath != "") {
			StringTokenizer strToken(sekkeiFilePath,"\\");
			if (strToken.size2() > 1) {
				int idx = strToken.size2()-2;
				for(int i=0;i<strToken.size2()-2;i++) {
					dataDir += strToken[i];
					dataDir += "\\";
				}
			}
		}
		SetCurrentDirectory(dataDir.c_str());
		initializeGui(1);
	}

	UpdateData(FALSE);
}

void CApolloToDesignDlg::OnOK() 
{
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD3)->GetWindowText(m_UflgJc);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD4)->GetWindowText(m_WebJc);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD5)->GetWindowText(m_LflgJc);
	m_UflgZaitanKeijo = ((CComboBox*)m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD7))->GetCurSel();
	m_LflgZaitanKeijo = ((CComboBox*)m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD8))->GetCurSel();
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD9)->GetWindowText(m_UpdownFlgZaitanKeijoTachiageRyo);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD11)->GetWindowText(m_SolePlateKyotyokuFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD12)->GetWindowText(m_SolePlateKyojikuFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD6)->GetWindowText(m_ItatsugiZureRyo);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD10)->GetWindowText(m_LflgKakuhukubuTaper);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD36)->GetWindowText(m_WebHoleSlopeLowerLimitGrd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD20)->GetWindowText(m_ShitenVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD21)->GetWindowText(m_ShitenVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD22)->GetWindowText(m_ShitenVsCutFu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD23)->GetWindowText(m_KakutenVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD24)->GetWindowText(m_KakutenVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD25)->GetWindowText(m_KakutenVsCutFu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD26)->GetWindowText(m_MiddleVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD27)->GetWindowText(m_MiddleVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD28)->GetWindowText(m_MiddleVsFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD13)->GetWindowText(m_HsFreeSpaceVs);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD14)->GetWindowText(m_HsFreeSpaceSpl);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD15)->GetWindowText(m_HsFreeSpaceCbf);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD16)->GetWindowText(m_HsFreeSpaceCbfUlimit);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD17)->GetWindowText(m_HsSnipSizeVs);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD18)->GetWindowText(m_HsSnipSizeSpl);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD19)->GetWindowText(m_HsSnipSizeCbf);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD29)->GetWindowText(m_UflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD30)->GetWindowText(m_UflgOutsideSplKyotyokuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD31)->GetWindowText(m_UflgInsideSplKyotyokuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD32)->GetWindowText(m_WebSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD33)->GetWindowText(m_WebSplHeightZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD34)->GetWindowText(m_LflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD35)->GetWindowText(m_LflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS1)->GetWindowText(m_ShitenUflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS2)->GetWindowText(m_ShitenWebJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS3)->GetWindowText(m_ShitenLflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS4)->GetWindowText(m_KakutenHflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS5)->GetWindowText(m_KakutenHwebJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS6)->GetWindowText(m_ShitenConnCut);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS7)->GetWindowText(m_ShitenConnFillet);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS8)->GetWindowText(m_ShitenConnTachiageryo);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS9)->GetWindowText(m_KakutenConnCut);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS10)->GetWindowText(m_KakutenConnFillet);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS11)->GetWindowText(m_KakutenConnTachiageryo);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS12)->GetWindowText(m_CvsCutWu);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS13)->GetWindowText(m_CvsCutWd);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS30)->GetWindowText(m_WebHoleSlopeLowerLimitCrs);
	m_FlgSectionType = ((CComboBox*)m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS31))->GetCurSel();
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS14)->GetWindowText(m_ShitenUflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS15)->GetWindowText(m_ShitenUflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS16)->GetWindowText(m_ShitenWebSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS17)->GetWindowText(m_ShitenWebSplHeightZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS18)->GetWindowText(m_ShitenLflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS19)->GetWindowText(m_ShitenLflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS20)->GetWindowText(m_ShitenConnKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS21)->GetWindowText(m_ShitenConnKyoutyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS22)->GetWindowText(m_KakutenUflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS23)->GetWindowText(m_KakutenUflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS24)->GetWindowText(m_KakutenWebSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS25)->GetWindowText(m_KakutenWebSplHeightZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS26)->GetWindowText(m_KakutenLflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS27)->GetWindowText(m_KakutenLflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS28)->GetWindowText(m_KakutenConnKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS29)->GetWindowText(m_KakutenConnKyoutyokuZaitan);
	if(m_UflgJc == "") {			//��t�����W�̃W���C���g�N���A�����X
		AfxMessageBox(_T("��t�����W�̃W���C���g�N���A�����X����͂��Ă��������B"));
		return;
	}
	if(m_WebJc == "") {			//�E�F�u�̃W���C���g�N���A�����X
		AfxMessageBox(_T("�E�F�u�̃W���C���g�N���A�����X����͂��Ă��������B"));
		return;
	}
	if(m_LflgJc == "") {			//���t�����W�̃W���C���g�N���A�����X
		AfxMessageBox(_T("���t�����W�̃W���C���g�N���A�����X����͂��Ă��������B"));
		return;
	}
	if(m_UpdownFlgZaitanKeijoTachiageRyo == "") {	//�㉺�t�����W�ޒ[�`�󗧏グ��
		AfxMessageBox(_T("�㉺�t�����W�ޒ[�`�󗧏グ�ʂ���͂��Ă��������B"));
		return;
	}
	if(m_SolePlateKyotyokuFreeSpace == "") {		//�\�[���v���[�g�̋��������󂫗�
		AfxMessageBox(_T("�\�[���v���[�g�̋��������󂫗ʂ���͂��Ă��������B"));
		return;
	}
	if(m_SolePlateKyojikuFreeSpace == "") {		//�\�[���v���[�g�̋��������󂫗�
		AfxMessageBox(_T("�\�[���v���[�g�̋��������󂫗ʂ���͂��Ă��������B"));
		return;
	}
	if(m_ItatsugiZureRyo == "") {					//�p�Y����
		AfxMessageBox(_T("�p�Y���ʂ���͂��Ă��������B"));
		return;
	}
	if(m_LflgKakuhukubuTaper == "") {				//���t�����W�g�����̃e�[�p�[���z
		AfxMessageBox(_T("���t�����W�g�����̃e�[�p�[���z����͂��Ă��������B"));
		return;
	}
	if(m_WebHoleSlopeLowerLimitGrd == "") {		//�E�F�u�E�̍E���z�̉����l
		AfxMessageBox(_T("�E�F�u�E�̍E���z�̉����l����͂��Ă��������B"));
		return;
	}
	if(m_ShitenVsCutWu == "") {					//�x�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
		AfxMessageBox(_T("�x�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�����͂��Ă��������B"));
		return;
	}
	if(m_ShitenVsCutWd == "") {					//�x�_�㐂���⍄�ނ̗n�ڕӑ������،�
		AfxMessageBox(_T("�x�_�㐂���⍄�ނ̗n�ڕӑ������،�����͂��Ă��������B"));
		return;
	}
	if(m_ShitenVsCutFu == "") {					//�x�_�㐂���⍄�ނ̏㑤�،�
		AfxMessageBox(_T("�x�_�㐂���⍄�ނ̏㑤�،�����͂��Ă��������B"));
		return;
	}
	if(m_KakutenVsCutWu == "") {					//�i�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
		AfxMessageBox(_T("�i�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�����͂��Ă��������B"));
		return;
	}
	if(m_KakutenVsCutWd == "") {					//�i�_�㐂���⍄�ނ̗n�ڕӑ������،�
		AfxMessageBox(_T("�i�_�㐂���⍄�ނ̗n�ڕӑ������،�����͂��Ă��������B"));
		return;
	}
	if(m_KakutenVsCutFu == "") {					//�i�_�㐂���⍄�ނ̏㑤�،�
		AfxMessageBox(_T("�i�_�㐂���⍄�ނ̏㑤�،�����͂��Ă��������B"));
		return;
	}
	if(m_MiddleVsCutWu == "") {					//���Ԑ����⍄�ނ̗n�ڕӑ��㑤�،�
		AfxMessageBox(_T("���Ԑ����⍄�ނ̗n�ڕӑ��㑤�،�����͂��Ă��������B"));
		return;
	}
	if(m_MiddleVsCutWd == "") {					//���Ԑ����⍄�ށi���k�^�C�v�j�̗n�ڕӑ������،�
		AfxMessageBox(_T("���Ԑ����⍄�ށi���k�^�C�v�j�̗n�ڕӑ������،�����͂��Ă��������B"));
		return;
	}
	if(m_MiddleVsFreeSpace == "") {				//���Ԑ����⍄�ށi�����^�C�v�j�̉����󂫗�
		AfxMessageBox(_T("���Ԑ����⍄�ށi�����^�C�v�j�̉����󂫗ʂ���͂��Ă��������B"));
		return;
	}
	if(m_HsFreeSpaceVs == "") {					//�����⍄�ނ̐����⍄�ޕ��A�����E�F�u���󂫗�
		AfxMessageBox(_T("�����⍄�ނ̐����⍄�ޕ��A�����E�F�u���󂫗ʂ���͂��Ă��������B"));
		return;
	}
	if(m_HsFreeSpaceSpl == "") {					//�����⍄�ނ̓Y�ڔ��󂫗�
		AfxMessageBox(_T("�����⍄�ނ̓Y�ڔ��󂫗ʂ���͂��Ă��������B"));
		return;
	}
	if(m_HsFreeSpaceCbf == "") {					//�����⍄�ނ̉����t�����W���󂫗�
		AfxMessageBox(_T("�����⍄�ނ̉����t�����W���󂫗ʂ���͂��Ă��������B"));
		return;
	}
	if(m_HsFreeSpaceCbfUlimit == "") {			//�����⍄�ނ̉����t�����W����̍������@���
		AfxMessageBox(_T("�����⍄�ނ̉����t�����W����̍������@�������͂��Ă��������B"));
		return;
	}
	if(m_HsSnipSizeVs == "") {					//�����⍄�ނ̐����⍄�ޕ��̃X�j�b�v�T�C�Y
		AfxMessageBox(_T("�����⍄�ނ̐����⍄�ޕ��̃X�j�b�v�T�C�Y����͂��Ă��������B"));
		return;
	}
	if(m_HsSnipSizeSpl == "") {					//�����⍄�ނ̓Y�ڕ��̃X�j�b�v�T�C�Y
		AfxMessageBox(_T("�����⍄�ނ̓Y�ڕ��̃X�j�b�v�T�C�Y����͂��Ă��������B"));
		return;
	}
	if(m_HsSnipSizeCbf == "") {					//�����⍄�ނ̉����t�����W���̃X�j�b�v�T�C�Y
		AfxMessageBox(_T("�����⍄�ނ̉����t�����W���̃X�j�b�v�T�C�Y����͂��Ă��������B"));
		return;
	}
	if(m_UflgSplKyojikuZaitan == "") {			//��t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("��t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_UflgOutsideSplKyotyokuZaitan == "") {	//��t�����W�O���Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("��t�����W�O���Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_UflgInsideSplKyotyokuZaitan == "") {		//��t�����W�����Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("��t�����W�����Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_WebSplKyojikuZaitan == "") {				//�E�F�u�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�E�F�u�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_WebSplHeightZaitan == "") {				//�E�F�u�Y�ڔ̍��������ޒ[
		AfxMessageBox(_T("�E�F�u�Y�ڔ̍��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_LflgSplKyojikuZaitan == "") {			//���t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("���t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_LflgSplKyotyokuZaitan == "") {			//���t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("���t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_ShitenUflgJc == "") {		//�x�_�㉡���iBH�j��t�����W�̃W���C���g�N���A�����X
		AfxMessageBox(_T("�x�_�㉡���iBH�j��t�����W�̃W���C���g�N���A�����X����͂��Ă��������B"));
		return;
	}
	if(m_ShitenWebJc == "") {			//�x�_�㉡���iBH�j�E�F�u�̃W���C���g�N���A�����X
		AfxMessageBox(_T("�x�_�㉡���iBH�j�E�F�u�̃W���C���g�N���A�����X����͂��Ă��������B"));
		return;
	}
	if(m_ShitenLflgJc == "") {		//�x�_�㉡���iBH�j���t�����W�̃W���C���g�N���A�����X
		AfxMessageBox(_T("�x�_�㉡���iBH�j���t�����W�̃W���C���g�N���A�����X����͂��Ă��������B"));
		return;
	}
	if(m_KakutenHflgJc == "") {		//�i�_�㉡���iH�|�j�t�����W�̃W���C���g�N���A�����X
		AfxMessageBox(_T("�i�_�㉡���iH�|�j�t�����W�̃W���C���g�N���A�����X����͂��Ă��������B"));
		return;
	}
	if(m_KakutenHwebJc == "") {		//�i�_�㉡���iH�|�j�E�F�u�̃W���C���g�N���A�����X
		AfxMessageBox(_T("�i�_�㉡���iH�|�j�E�F�u�̃W���C���g�N���A�����X����͂��Ă��������B"));
		return;
	}
	if(m_ShitenConnCut == "") {			//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӑ��،�
		AfxMessageBox(_T("�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӑ��،�����͂��Ă��������B"));
		return;
	}
	if(m_ShitenConnFillet == "") {		//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̃t�B���b�g��R�T�C�Y
		AfxMessageBox(_T("�包�E�F�u�t���R�l�N�V�����i�x�_��j�̃t�B���b�g��R�T�C�Y����͂��Ă��������B"));
		return;
	}
	if(m_ShitenConnTachiageryo == "") {	//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӗ��グ��
		AfxMessageBox(_T("�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӗ��グ�ʂ���͂��Ă��������B"));
		return;
	}
	if(m_KakutenConnCut == "") {			//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӑ��،����w��
		AfxMessageBox(_T("�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӑ��،����w�����͂��Ă��������B"));
		return;
	}
	if(m_KakutenConnFillet == "") {		//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̃t�B���b�g��R�T�C�Y
		AfxMessageBox(_T("�包�E�F�u�t���R�l�N�V�����i�i�_��j�̃t�B���b�g��R�T�C�Y����͂��Ă��������B"));
		return;
	}
	if(m_KakutenConnTachiageryo == "") {	//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӗ��グ��
		AfxMessageBox(_T("�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӗ��グ�ʂ���͂��Ă��������B"));
		return;
	}
	if(m_CvsCutWu == "") {					//�����t�����⍄�ނ̗n�ڕӑ��㑤�،�
		AfxMessageBox(_T("�����t�����⍄�ނ̗n�ڕӑ��㑤�،�����͂��Ă��������B"));
		return;
	}
	if(m_CvsCutWd == "") {					//�����t�����⍄�ނ̗n�ڕӑ������،�
		AfxMessageBox(_T("�����t�����⍄�ނ̗n�ڕӑ������،�����͂��Ă��������B"));
		return;
	}
	if(m_WebHoleSlopeLowerLimitCrs == "") {	//�E�F�u�E�̍E���z�̉����l
		AfxMessageBox(_T("�E�F�u�E�̍E���z�̉����l����͂��Ă��������B"));
		return;
	}
	if(m_ShitenUflgSplKyojikuZaitan == "") {		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_ShitenUflgSplKyotyokuZaitan == "") {		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_ShitenWebSplKyotyokuZaitan == "") {		//�x�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�x�_�㉡���E�F�u�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_ShitenWebSplHeightZaitan == "") {		//�x�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
		AfxMessageBox(_T("�x�_�㉡���E�F�u�Y�ڔ̍��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_ShitenLflgSplKyojikuZaitan == "") {		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_ShitenLflgSplKyotyokuZaitan == "") {		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_ShitenConnKyojikuZaitan == "") {			//�x�_��R�l�N�V�����̋��������ޒ[
		AfxMessageBox(_T("�x�_��R�l�N�V�����̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_ShitenConnKyoutyokuZaitan == "") {		//�x�_��R�l�N�V�����̋��������ޒ[
		AfxMessageBox(_T("�x�_��R�l�N�V�����̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_KakutenUflgSplKyojikuZaitan == "") {		//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_KakutenUflgSplKyotyokuZaitan == "") {	//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_KakutenWebSplKyotyokuZaitan == "") {		//�i�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�i�_�㉡���E�F�u�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_KakutenWebSplHeightZaitan == "") {		//�i�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
		AfxMessageBox(_T("�i�_�㉡���E�F�u�Y�ڔ̍��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_KakutenLflgSplKyojikuZaitan == "") {		//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_KakutenLflgSplKyotyokuZaitan == "") {	//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
		AfxMessageBox(_T("�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	if(m_KakutenConnKyojikuZaitan == "") {		//�i�_��R�l�N�V�����̋��������ޒ[
		AfxMessageBox(_T("�i�_��R�l�N�V�����̋��������ޒ[����͂��Ă��������B"));
		return;
	}
	AtdGirderCommon agc;
	//�W���C���g�N���A�����X
	agc.setUflgJc(atof(m_UflgJc));	//��t�����W�̃W���C���g�N���A�����X
	agc.setWebJc(atof(m_WebJc));	//�E�F�u�̃W���C���g�N���A�����X
	agc.setLflgJc(atof(m_LflgJc));	//���t�����W�̃W���C���g�N���A�����X
	//�ޒ[�`��
	EnTypeNo enTypeNo;
	if(m_UflgZaitanKeijo == 0) {
		enTypeNo = TYPE1;	//	�^�C�v1
	} else if(m_UflgZaitanKeijo == 1) {
		enTypeNo = TYPE2;	//	�^�C�v2
	} else if(m_UflgZaitanKeijo == 2) {
		enTypeNo = TYPE3;	//	�^�C�v3
	} else {
		enTypeNo = ERR_ETN;	//	�G���[�`�F�b�N�p
	}
	agc.setUflgZaitanKeijo(enTypeNo);	//��t�����W�ޒ[�`��
	if(m_LflgZaitanKeijo == 0) {
		enTypeNo = TYPE1;	//	�^�C�v1
	} else if(m_LflgZaitanKeijo == 1) {
		enTypeNo = TYPE2;	//	�^�C�v2
	} else if(m_LflgZaitanKeijo == 2) {
		enTypeNo = TYPE3;	//	�^�C�v3
	} else {
		enTypeNo = ERR_ETN;	//	�G���[�`�F�b�N�p
	}
	agc.setLflgZaitanKeijo(enTypeNo);	//���t�����W�ޒ[�`��
	agc.setUpdownFlgZaitanKeijoTachiageRyo(atof(m_UpdownFlgZaitanKeijoTachiageRyo));	//�㉺�t�����W�ޒ[�`�󗧏グ��
	//�\�[���v���[�g
	agc.setSolePlateKyotyokuFreeSpace(atof(m_SolePlateKyotyokuFreeSpace));	//�\�[���v���[�g�̋��������󂫗�
	agc.setSolePlateKyojikuFreeSpace(atof(m_SolePlateKyojikuFreeSpace));	//�\�[���v���[�g�̋��������󂫗�
	//���̑�
	agc.setItatsugiZureRyo(atof(m_ItatsugiZureRyo));	//�p�Y����
	agc.setLflgKakuhukubuTaper(atof(m_LflgKakuhukubuTaper));	//���t�����W�g�����̃e�[�p�[���z
	agc.setWebHoleSlopeLowerLimitGrd(atof(m_WebHoleSlopeLowerLimitGrd));	//�E�F�u�E�̍E���z�̉����l
	//�����⍄��
	agc.setShitenVsCutWu(m_ShitenVsCutWu.GetBuffer());	//�x�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
	agc.setShitenVsCutWd(m_ShitenVsCutWd.GetBuffer());	//�x�_�㐂���⍄�ނ̗n�ڕӑ������،�
	agc.setShitenVsCutFu(m_ShitenVsCutFu.GetBuffer());	//�x�_�㐂���⍄�ނ̏㑤�،�
	agc.setKakutenVsCutWu(m_KakutenVsCutWu.GetBuffer());	//�i�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
	agc.setKakutenVsCutWd(m_KakutenVsCutWd.GetBuffer());	//�i�_�㐂���⍄�ނ̗n�ڕӑ������،�
	agc.setKakutenVsCutFu(m_KakutenVsCutFu.GetBuffer());	//�i�_�㐂���⍄�ނ̏㑤�،�
	agc.setMiddleVsCutWu(m_MiddleVsCutWu.GetBuffer());	//���Ԑ����⍄�ނ̗n�ڕӑ��㑤�،�
	agc.setMiddleVsCutWd(m_MiddleVsCutWd.GetBuffer());	//���Ԑ����⍄�ށi���k�^�C�v�j�̗n�ڕӑ������،�
	agc.setMiddleVsFreeSpace(atof(m_MiddleVsFreeSpace));	//���Ԑ����⍄�ށi�����^�C�v�j�̉����󂫗�
	//�����⍄��
	agc.setHsFreeSpaceVs(atof(m_HsFreeSpaceVs));	//�����⍄�ނ̐����⍄�ޕ��A�����E�F�u���󂫗�
	agc.setHsFreeSpaceSpl(atof(m_HsFreeSpaceSpl));	//�����⍄�ނ̓Y�ڔ��󂫗�
	agc.setHsFreeSpaceCbf(atof(m_HsFreeSpaceCbf));	//�����⍄�ނ̉����t�����W���󂫗�
	agc.setHsFreeSpaceCbfUlimit(atof(m_HsFreeSpaceCbfUlimit));	//�����⍄�ނ̉����t�����W����̍������@���
	agc.setHsSnipSizeVs(m_HsSnipSizeVs.GetBuffer());	//�����⍄�ނ̐����⍄�ޕ��̃X�j�b�v�T�C�Y
	agc.setHsSnipSizeSpl(m_HsSnipSizeSpl.GetBuffer());	//�����⍄�ނ̓Y�ڕ��̃X�j�b�v�T�C�Y
	agc.setHsSnipSizeCbf(m_HsSnipSizeCbf.GetBuffer());	//�����⍄�ނ̉����t�����W���̃X�j�b�v�T�C�Y
	//�Y�ڔ�
	agc.setUflgSplKyojikuZaitan(atof(m_UflgSplKyojikuZaitan));	//��t�����W�Y�ڔ̋��������ޒ[
	agc.setUflgOutsideSplKyotyokuZaitan(atof(m_UflgOutsideSplKyotyokuZaitan));	//��t�����W�O���Y�ڔ̋��������ޒ[
	agc.setUflgInsideSplKyotyokuZaitan(atof(m_UflgInsideSplKyotyokuZaitan));	//��t�����W�����Y�ڔ̋��������ޒ[
	agc.setWebSplKyojikuZaitan(atof(m_WebSplKyojikuZaitan));	//�E�F�u�Y�ڔ̋��������ޒ[
	agc.setWebSplHeightZaitan(atof(m_WebSplHeightZaitan));	//�E�F�u�Y�ڔ̍��������ޒ[
	agc.setLflgSplKyojikuZaitan(atof(m_LflgSplKyojikuZaitan));	//���t�����W�Y�ڔ̋��������ޒ[
	agc.setLflgSplKyotyokuZaitan(atof(m_LflgSplKyotyokuZaitan));	//���t�����W�Y�ڔ̋��������ޒ[
	//
	AtdCrossBeamCommon acc;
	//�W���C���g�N���A�����X
	acc.setShitenUflgJc(atof(m_ShitenUflgJc));	//�x�_�㉡���iBH�j��t�����W�̃W���C���g�N���A�����X
	acc.setShitenWebJc(atof(m_ShitenWebJc));	//�x�_�㉡���iBH�j�E�F�u�̃W���C���g�N���A�����X
	acc.setShitenLflgJc(atof(m_ShitenLflgJc));	//�x�_�㉡���iBH�j���t�����W�̃W���C���g�N���A�����X
	acc.setKakutenHflgJc(atof(m_KakutenHflgJc));	//�i�_�㉡���iH�|�j�t�����W�̃W���C���g�N���A�����X
	acc.setKakutenHwebJc(atof(m_KakutenHwebJc));	//�i�_�㉡���iH�|�j�E�F�u�̃W���C���g�N���A�����X
	//�R�l�N�V�����v���[�g
	acc.setShitenConnCut(m_ShitenConnCut.GetBuffer());	//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӑ��،�
	acc.setShitenConnFillet(atof(m_ShitenConnFillet));	//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̃t�B���b�g��R�T�C�Y
	acc.setShitenConnTachiageryo(atof(m_ShitenConnTachiageryo));	//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӗ��グ��
	acc.setKakutenConnCut(m_KakutenConnCut.GetBuffer());	//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӑ��،����w��
	acc.setKakutenConnFillet(atof(m_KakutenConnFillet));	//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̃t�B���b�g��R�T�C�Y
	acc.setKakutenConnTachiageryo(atof(m_KakutenConnTachiageryo));	//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӗ��グ��
	//�����⍄��
	acc.setCvsCutWu(m_CvsCutWu.GetBuffer());	//�����t�����⍄�ނ̗n�ڕӑ��㑤�،�
	acc.setCvsCutWd(m_CvsCutWd.GetBuffer());	//�����t�����⍄�ނ̗n�ڕӑ������،�
	//���̑�
	acc.setWebHoleSlopeLowerLimitCrs(atof(m_WebHoleSlopeLowerLimitCrs));	//�E�F�u�E�̍E���z�̉����l
	if(m_FlgSectionType == 0) {
		enTypeNo = TYPE1;	//	�^�C�v1
	} else if(m_FlgSectionType == 1) {
		enTypeNo = TYPE2;	//	�^�C�v2
	} else if(m_FlgSectionType == 2) {
		enTypeNo = TYPE3;	//	�^�C�v3
	} else {
		enTypeNo = ERR_ETN;	//	�G���[�`�F�b�N�p
	}
	acc.setFlgSectionType(enTypeNo);	//�t�����W�،��̕���
	//�ޒ[�`��
	acc.setShitenUflgSplKyojikuZaitan(atof(m_ShitenUflgSplKyojikuZaitan));	//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	acc.setShitenUflgSplKyotyokuZaitan(atof(m_ShitenUflgSplKyotyokuZaitan));	//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	acc.setShitenWebSplKyotyokuZaitan(atof(m_ShitenWebSplKyotyokuZaitan));	//�x�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
	acc.setShitenWebSplHeightZaitan(atof(m_ShitenWebSplHeightZaitan));	//�x�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
	acc.setShitenLflgSplKyojikuZaitan(atof(m_ShitenLflgSplKyojikuZaitan));	//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	acc.setShitenLflgSplKyotyokuZaitan(atof(m_ShitenLflgSplKyotyokuZaitan));	//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	acc.setShitenConnKyojikuZaitan(atof(m_ShitenConnKyojikuZaitan));	//�x�_��R�l�N�V�����̋��������ޒ[
	acc.setShitenConnKyoutyokuZaitan(atof(m_ShitenConnKyoutyokuZaitan));	//�x�_��R�l�N�V�����̋��������ޒ[
	acc.setKakutenUflgSplKyojikuZaitan(atof(m_KakutenUflgSplKyojikuZaitan));	//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	acc.setKakutenUflgSplKyotyokuZaitan(atof(m_KakutenUflgSplKyotyokuZaitan));	//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
	acc.setKakutenWebSplKyotyokuZaitan(atof(m_KakutenWebSplKyotyokuZaitan));	//�i�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
	acc.setKakutenWebSplHeightZaitan(atof(m_KakutenWebSplHeightZaitan));	//�i�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
	acc.setKakutenLflgSplKyojikuZaitan(atof(m_KakutenLflgSplKyojikuZaitan));	//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	acc.setKakutenLflgSplKyotyokuZaitan(atof(m_KakutenLflgSplKyotyokuZaitan));	//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
	acc.setKakutenConnKyojikuZaitan(atof(m_KakutenConnKyojikuZaitan));	//�i�_��R�l�N�V�����̋��������ޒ[
	acc.setKakutenConnKyoutyokuZaitan(atof(m_KakutenConnKyoutyokuZaitan));	//�i�_��R�l�N�V�����̋��������ޒ[
	//
	string sekkeiFilePath = m_MdbFilePath.GetBuffer();
	string csvFileName = m_CsvFilePath.GetBuffer();
	string dataDir = "";
	if(sekkeiFilePath == "") {
		AfxMessageBox(_T("�Q��MDB�t�@�C�������w�肵�Ă��������B"));
		return;
	} else {
		StringTokenizer strToken(sekkeiFilePath,"\\");
		if (strToken.size2() > 1) {
			int idx = strToken.size2()-2;
			for(int i=0;i<strToken.size2()-2;i++) {
				dataDir += strToken[i];
				dataDir += "\\";
			}
		}
	}
	SetCurrentDirectory(dataDir.c_str());
	string seizuFilePath = dataDir + "Draw\\DGFile.mdb";
	CString strMsg;
	if (!fileExists(sekkeiFilePath.c_str())) {
		strMsg.Format("�Q��MDB�t�@�C��(%s)�͑��݂��܂���B", sekkeiFilePath.c_str());
		AfxMessageBox(strMsg);
		return;
	}
	if (!fileExists(seizuFilePath.c_str())) {
		strMsg.Format("�݌v�t�@�C��(%s)�����݂��܂���B", seizuFilePath.c_str());
		AfxMessageBox(strMsg);
		return;
	}
	if(csvFileName == "") {	//���͂���Ă��Ȃ�
		AfxMessageBox(_T("�o�̓t�@�C�������w�肵�Ă��������B"));
		return;
	} else {
		if (fileExists(csvFileName.c_str())) {	//���ɑ��݂���ꍇ�͊m�F
			strMsg.Format("�w�肵���o�̓t�@�C��(%s)�����݂��܂��B�㏑�����Ď��s���ėǂ��ł����H",csvFileName.c_str());
			if(MessageBox(strMsg,_T("������"),MB_YESNO|MB_ICONEXCLAMATION) != IDYES) {
				return;
			}
			CFile OpenData;
			BOOL FileCheck=TRUE;
			FileCheck=OpenData.Open(csvFileName.c_str(),CFile::modeRead);
			if(FileCheck==TRUE) {
				OpenData.Close();
			} else {
				strMsg.Format("�o��DXF�t�@�C��[%s]����Ă�����s���Ă��������B", csvFileName.c_str());
				AfxMessageBox(strMsg);
				return;
			}
		}
	}

	//Set folder directory path
	TCHAR path[MAX_PATH];
	GetCurrentDirectory(MAX_PATH, path);
	string fileInputStorage = CString(path) + INPUTSTORAGE;
	storePreviousInput(agc, acc, fileInputStorage);

	//���s
	CString cstr="START";
	printMessage(cstr);
	if (ApolloToDesign_Main(sekkeiFilePath, seizuFilePath, csvFileName, agc, acc) != JPT_OK) {
		cstr="ERROR";
		printMessage(cstr);
		AfxMessageBox("�~ ����ވُ�I���FAPOLLO�f�[�^�ː݌v��񑮐��t�@�C��(ApolloToDesign)",MB_OK|MB_ICONSTOP);
		return;
	} else {
		cstr="END";
		printMessage(cstr);
		MessageBox("�� ����ގ��s�I���FAPOLLO�f�[�^�ː݌v��񑮐��t�@�C��(ApolloToDesign)",MB_OK);
		return;
	}

	CDialog::OnOK();
}

void CApolloToDesignDlg::OnCancel() 
{
	CDialog::OnCancel();
}

BOOL CApolloToDesignDlg::storePreviousInput(AtdGirderCommon agc, AtdCrossBeamCommon acc, string fileInputStorage)
{
	ofstream fStore;
	fStore.open(fileInputStorage.c_str());
	if (!fStore)
		return FALSE;
	fStore<<"MdbFilePath: "<<m_MdbFilePath<<"\n";
	fStore<<"CsvFilePath: "<<m_CsvFilePath<<"\n";
	fStore<<"UflgJc: "<<m_UflgJc<<"\n";
	fStore<<"WebJc: "<<m_WebJc<<"\n";
	fStore<<"LflgJc: "<<m_LflgJc<<"\n";
	fStore<<"UflgZaitanKeijo: "<<m_UflgZaitanKeijo<<"\n";
	fStore<<"LflgZaitanKeijo: "<<m_LflgZaitanKeijo<<"\n";
	fStore<<"UpdownFlgZaitanKeijoTachiageRyo: "<<m_UpdownFlgZaitanKeijoTachiageRyo<<"\n";
	fStore<<"SolePlateKyotyokuFreeSpace: "<<m_SolePlateKyotyokuFreeSpace<<"\n";
	fStore<<"SolePlateKyojikuFreeSpace: "<<m_SolePlateKyojikuFreeSpace<<"\n";
	fStore<<"ItatsugiZureRyo: "<<m_ItatsugiZureRyo<<"\n";
	fStore<<"LflgKakuhukubuTaper: "<<m_LflgKakuhukubuTaper<<"\n";
	fStore<<"WebHoleSlopeLowerLimitGrd: "<<m_WebHoleSlopeLowerLimitGrd<<"\n";
	fStore<<"ShitenVsCutWu: "<<m_ShitenVsCutWu<<"\n";
	fStore<<"ShitenVsCutWd: "<<m_ShitenVsCutWd<<"\n";
	fStore<<"ShitenVsCutFu: "<<m_ShitenVsCutFu<<"\n";
	fStore<<"KakutenVsCutWu: "<<m_KakutenVsCutWu<<"\n";
	fStore<<"KakutenVsCutWd: "<<m_KakutenVsCutWd<<"\n";
	fStore<<"KakutenVsCutFu: "<<m_KakutenVsCutFu<<"\n";
	fStore<<"MiddleVsCutWu: "<<m_MiddleVsCutWu<<"\n";
	fStore<<"MiddleVsCutWd: "<<m_MiddleVsCutWd<<"\n";
	fStore<<"MiddleVsFreeSpace: "<<m_MiddleVsFreeSpace<<"\n";
	fStore<<"HsFreeSpaceVs: "<<m_HsFreeSpaceVs<<"\n";
	fStore<<"HsFreeSpaceSpl: "<<m_HsFreeSpaceSpl<<"\n";
	fStore<<"HsFreeSpaceCbf: "<<m_HsFreeSpaceCbf<<"\n";
	fStore<<"HsFreeSpaceCbfUlimit: "<<m_HsFreeSpaceCbfUlimit<<"\n";
	fStore<<"HsSnipSizeVs: "<<m_HsSnipSizeVs<<"\n";
	fStore<<"HsSnipSizeSpl: "<<m_HsSnipSizeSpl<<"\n";
	fStore<<"HsSnipSizeCbf: "<<m_HsSnipSizeCbf<<"\n";
	fStore<<"UflgSplKyojikuZaitan: "<<m_UflgSplKyojikuZaitan<<"\n";
	fStore<<"UflgOutsideSplKyotyokuZaitan: "<<m_UflgOutsideSplKyotyokuZaitan<<"\n";
	fStore<<"UflgInsideSplKyotyokuZaitan: "<<m_UflgInsideSplKyotyokuZaitan<<"\n";
	fStore<<"WebSplKyojikuZaitan: "<<m_WebSplKyojikuZaitan<<"\n";
	fStore<<"WebSplHeightZaitan: "<<m_WebSplHeightZaitan<<"\n";
	fStore<<"LflgSplKyojikuZaitan: "<<m_LflgSplKyojikuZaitan<<"\n";
	fStore<<"LflgSplKyotyokuZaitan: "<<m_LflgSplKyotyokuZaitan<<"\n";
	fStore<<"ShitenUflgJc: "<<m_ShitenUflgJc<<"\n";
	fStore<<"ShitenWebJc: "<<m_ShitenWebJc<<"\n";
	fStore<<"ShitenLflgJc: "<<m_ShitenLflgJc<<"\n";
	fStore<<"KakutenHflgJc: "<<m_KakutenHflgJc<<"\n";
	fStore<<"KakutenHwebJc: "<<m_KakutenHwebJc<<"\n";
	fStore<<"ShitenUflgJc: "<<m_ShitenUflgJc<<"\n";
	fStore<<"ShitenWebJc: "<<m_ShitenWebJc<<"\n";
	fStore<<"ShitenLflgJc: "<<m_ShitenLflgJc<<"\n";
	fStore<<"KakutenHflgJc: "<<m_KakutenHflgJc<<"\n";
	fStore<<"KakutenHwebJc: "<<m_KakutenHwebJc<<"\n";
	fStore<<"ShitenConnCut: "<<m_ShitenConnCut<<"\n";
	fStore<<"ShitenConnFillet: "<<m_ShitenConnFillet<<"\n";
	fStore<<"ShitenConnTachiageryo: "<<m_ShitenConnTachiageryo<<"\n";
	fStore<<"KakutenConnCut: "<<m_KakutenConnCut<<"\n";
	fStore<<"KakutenConnFillet: "<<m_KakutenConnFillet<<"\n";
	fStore<<"KakutenConnTachiageryo: "<<m_KakutenConnTachiageryo<<"\n";
	fStore<<"CvsCutWu: "<<m_CvsCutWu<<"\n";
	fStore<<"CvsCutWd: "<<m_CvsCutWd<<"\n";
	fStore<<"WebHoleSlopeLowerLimitCrs: "<<m_WebHoleSlopeLowerLimitCrs<<"\n";
	fStore<<"FlgSectionType: "<<m_FlgSectionType<<"\n";
	fStore<<"ShitenUflgSplKyojikuZaitan: "<<m_ShitenUflgSplKyojikuZaitan<<"\n";
	fStore<<"ShitenUflgSplKyotyokuZaitan: "<<m_ShitenUflgSplKyotyokuZaitan<<"\n";
	fStore<<"ShitenWebSplKyotyokuZaitan: "<<m_ShitenWebSplKyotyokuZaitan<<"\n";
	fStore<<"ShitenWebSplHeightZaitan: "<<m_ShitenWebSplHeightZaitan<<"\n";
	fStore<<"ShitenLflgSplKyojikuZaitan: "<<m_ShitenLflgSplKyojikuZaitan<<"\n";
	fStore<<"ShitenLflgSplKyotyokuZaitan: "<<m_ShitenLflgSplKyotyokuZaitan<<"\n";
	fStore<<"ShitenConnKyojikuZaitan: "<<m_ShitenConnKyojikuZaitan<<"\n";
	fStore<<"ShitenConnKyoutyokuZaitan: "<<m_ShitenConnKyoutyokuZaitan<<"\n";
	fStore<<"KakutenUflgSplKyojikuZaitan: "<<m_KakutenUflgSplKyojikuZaitan<<"\n";
	fStore<<"KakutenUflgSplKyotyokuZaitan: "<<m_KakutenUflgSplKyotyokuZaitan<<"\n";
	fStore<<"KakutenWebSplKyotyokuZaitan: "<<m_KakutenWebSplKyotyokuZaitan<<"\n";
	fStore<<"KakutenWebSplHeightZaitan: "<<m_KakutenWebSplHeightZaitan<<"\n";
	fStore<<"KakutenLflgSplKyojikuZaitan: "<<m_KakutenLflgSplKyojikuZaitan<<"\n";
	fStore<<"KakutenLflgSplKyotyokuZaitan: "<<m_KakutenLflgSplKyotyokuZaitan<<"\n";
	fStore<<"KakutenConnKyojikuZaitan: "<<m_KakutenConnKyojikuZaitan<<"\n";
	fStore<<"KakutenConnKyoutyokuZaitan: "<<m_KakutenConnKyoutyokuZaitan<<"\n";
	fStore.close();

	return TRUE;
}

BOOL CApolloToDesignDlg::readPreviousInput(const int type, string fileInputStorage)
{	
	ifstream fStore;
	string line;
	fStore.open(fileInputStorage.c_str());
	if (!fStore)
		return FALSE;

	while ( getline(fStore,line) )
	{
		StringTokenizer strToken(line," ");
		if (strToken.size2() > 1) {
			if(type == 0) {
				if(strToken[0].compare("MdbFilePath:") == 0) 
					m_MdbFilePath = strToken[1].c_str();
			}
			if(strToken[0].compare("CsvFilePath:") == 0) 
				m_CsvFilePath = strToken[1].c_str();
			if(strToken[0].compare("UflgJc:") == 0) 
				m_UflgJc = strToken[1].c_str();
			if(strToken[0].compare("WebJc:") == 0) 
				m_WebJc = strToken[1].c_str();
			if(strToken[0].compare("LflgJc:") == 0) 
				m_LflgJc = strToken[1].c_str();
			if(strToken[0].compare("UflgZaitanKeijo:") == 0) 
				m_UflgZaitanKeijo = atoi(strToken[1].c_str());
			if(strToken[0].compare("LflgZaitanKeijo:") == 0) 
				m_LflgZaitanKeijo = atoi(strToken[1].c_str());
			if(strToken[0].compare("UpdownFlgZaitanKeijoTachiageRyo:") == 0) 
				m_UpdownFlgZaitanKeijoTachiageRyo = strToken[1].c_str();
			if(strToken[0].compare("SolePlateKyotyokuFreeSpace:") == 0) 
				m_SolePlateKyotyokuFreeSpace = strToken[1].c_str();
			if(strToken[0].compare("SolePlateKyojikuFreeSpace:") == 0) 
				m_SolePlateKyojikuFreeSpace = strToken[1].c_str();
			if(strToken[0].compare("ItatsugiZureRyo:") == 0) 
				m_ItatsugiZureRyo = strToken[1].c_str();
			if(strToken[0].compare("LflgKakuhukubuTaper:") == 0) 
				m_LflgKakuhukubuTaper = strToken[1].c_str();
			if(strToken[0].compare("WebHoleSlopeLowerLimitGrd:") == 0) 
				m_WebHoleSlopeLowerLimitGrd = strToken[1].c_str();
			if(strToken[0].compare("ShitenVsCutWu:") == 0) 
				m_ShitenVsCutWu = strToken[1].c_str();
			if(strToken[0].compare("ShitenVsCutWd:") == 0) 
				m_ShitenVsCutWd = strToken[1].c_str();
			if(strToken[0].compare("ShitenVsCutFu:") == 0) 
				m_ShitenVsCutFu = strToken[1].c_str();
			if(strToken[0].compare("KakutenVsCutWu:") == 0) 
				m_KakutenVsCutWu = strToken[1].c_str();
			if(strToken[0].compare("KakutenVsCutWd:") == 0) 
				m_KakutenVsCutWd = strToken[1].c_str();
			if(strToken[0].compare("KakutenVsCutFu:") == 0) 
				m_KakutenVsCutFu = strToken[1].c_str();
			if(strToken[0].compare("MiddleVsCutWu:") == 0) 
				m_MiddleVsCutWu = strToken[1].c_str();
			if(strToken[0].compare("MiddleVsCutWd:") == 0) 
				m_MiddleVsCutWd = strToken[1].c_str();
			if(strToken[0].compare("MiddleVsFreeSpace:") == 0) 
				m_MiddleVsFreeSpace = strToken[1].c_str();
			if(strToken[0].compare("HsFreeSpaceVs:") == 0) 
				m_HsFreeSpaceVs = strToken[1].c_str();
			if(strToken[0].compare("HsFreeSpaceSpl:") == 0) 
				m_HsFreeSpaceSpl = strToken[1].c_str();
			if(strToken[0].compare("HsFreeSpaceCbf:") == 0) 
				m_HsFreeSpaceCbf = strToken[1].c_str();
			if(strToken[0].compare("HsFreeSpaceCbfUlimit:") == 0) 
				m_HsFreeSpaceCbfUlimit = strToken[1].c_str();
			if(strToken[0].compare("HsSnipSizeVs:") == 0) 
				m_HsSnipSizeVs = strToken[1].c_str();
			if(strToken[0].compare("HsSnipSizeSpl:") == 0) 
				m_HsSnipSizeSpl = strToken[1].c_str();
			if(strToken[0].compare("HsSnipSizeCbf:") == 0) 
				m_HsSnipSizeCbf = strToken[1].c_str();
			if(strToken[0].compare("UflgSplKyojikuZaitan:") == 0) 
				m_UflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("UflgOutsideSplKyotyokuZaitan:") == 0) 
				m_UflgOutsideSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("UflgInsideSplKyotyokuZaitan:") == 0) 
				m_UflgInsideSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("WebSplKyojikuZaitan:") == 0) 
				m_WebSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("WebSplHeightZaitan:") == 0) 
				m_WebSplHeightZaitan = strToken[1].c_str();
			if(strToken[0].compare("LflgSplKyojikuZaitan:") == 0) 
				m_LflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("LflgSplKyotyokuZaitan:") == 0) 
				m_LflgSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenUflgJc:") == 0) 
				m_ShitenUflgJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenWebJc:") == 0) 
				m_ShitenWebJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenLflgJc:") == 0) 
				m_ShitenLflgJc = strToken[1].c_str();
			if(strToken[0].compare("KakutenHflgJc:") == 0) 
				m_KakutenHflgJc = strToken[1].c_str();
			if(strToken[0].compare("KakutenHwebJc:") == 0) 
				m_KakutenHwebJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenUflgJc:") == 0) 
				m_ShitenUflgJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenWebJc:") == 0) 
				m_ShitenWebJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenLflgJc:") == 0) 
				m_ShitenLflgJc = strToken[1].c_str();
			if(strToken[0].compare("KakutenHflgJc:") == 0) 
				m_KakutenHflgJc = strToken[1].c_str();
			if(strToken[0].compare("KakutenHwebJc:") == 0) 
				m_KakutenHwebJc = strToken[1].c_str();
			if(strToken[0].compare("ShitenConnCut:") == 0) 
				m_ShitenConnCut = strToken[1].c_str();
			if(strToken[0].compare("ShitenConnFillet:") == 0) 
				m_ShitenConnFillet = strToken[1].c_str();
			if(strToken[0].compare("ShitenConnTachiageryo:") == 0) 
				m_ShitenConnTachiageryo = strToken[1].c_str();
			if(strToken[0].compare("KakutenConnCut:") == 0) 
				m_KakutenConnCut = strToken[1].c_str();
			if(strToken[0].compare("KakutenConnFillet:") == 0) 
				m_KakutenConnFillet = strToken[1].c_str();
			if(strToken[0].compare("KakutenConnTachiageryo:") == 0) 
				m_KakutenConnTachiageryo = strToken[1].c_str();
			if(strToken[0].compare("CvsCutWu:") == 0) 
				m_CvsCutWu = strToken[1].c_str();
			if(strToken[0].compare("CvsCutWd:") == 0) 
				m_CvsCutWd = strToken[1].c_str();
			if(strToken[0].compare("WebHoleSlopeLowerLimitCrs:") == 0) 
				m_WebHoleSlopeLowerLimitCrs = strToken[1].c_str();
			if(strToken[0].compare("FlgSectionType:") == 0) 
				m_FlgSectionType = atoi(strToken[1].c_str());
			if(strToken[0].compare("ShitenUflgSplKyojikuZaitan:") == 0) 
				m_ShitenUflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenUflgSplKyotyokuZaitan:") == 0) 
				m_ShitenUflgSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenWebSplKyotyokuZaitan:") == 0) 
				m_ShitenWebSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenWebSplHeightZaitan:") == 0) 
				m_ShitenWebSplHeightZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenLflgSplKyojikuZaitan:") == 0) 
				m_ShitenLflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenLflgSplKyotyokuZaitan:") == 0) 
				m_ShitenLflgSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenConnKyojikuZaitan:") == 0) 
				m_ShitenConnKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("ShitenConnKyoutyokuZaitan:") == 0) 
				m_ShitenConnKyoutyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenUflgSplKyojikuZaitan:") == 0) 
				m_KakutenUflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenUflgSplKyotyokuZaitan:") == 0) 
				m_KakutenUflgSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenWebSplKyotyokuZaitan:") == 0) 
				m_KakutenWebSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenWebSplHeightZaitan:") == 0) 
				m_KakutenWebSplHeightZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenLflgSplKyojikuZaitan:") == 0) 
				m_KakutenLflgSplKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenLflgSplKyotyokuZaitan:") == 0) 
				m_KakutenLflgSplKyotyokuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenConnKyojikuZaitan:") == 0) 
				m_KakutenConnKyojikuZaitan = strToken[1].c_str();
			if(strToken[0].compare("KakutenConnKyoutyokuZaitan:") == 0) 
				m_KakutenConnKyoutyokuZaitan = strToken[1].c_str();
		}
	}

	return TRUE;
}

void CApolloToDesignDlg::initializeGui(const int type)
{
	// TODO: Add extra initialization here
	WIN32_FIND_DATA fileNameData;
	HANDLE hFile;
	
	//Set folder directory path
	TCHAR path[MAX_PATH];
	GetCurrentDirectory(MAX_PATH, path);
	string fileStorage = CString(path) + INPUTSTORAGE;
	hFile = FindFirstFile(fileStorage.c_str(), &fileNameData);
	if (hFile == INVALID_HANDLE_VALUE)
	{
		m_CsvFilePath = "DesignInfoAttr.CSV";
		//�W���C���g�N���A�����X
		m_UflgJc = "0.0";			//��t�����W�̃W���C���g�N���A�����X
		m_WebJc = "0.0";			//�E�F�u�̃W���C���g�N���A�����X
		m_LflgJc = "0.0";			//���t�����W�̃W���C���g�N���A�����X
		//�ޒ[�`��
		m_UflgZaitanKeijo = 0;		//��t�����W�ޒ[�`��
		m_LflgZaitanKeijo = 0;		//���t�����W�ޒ[�`��
		m_UpdownFlgZaitanKeijoTachiageRyo = _T("0.0");	//�㉺�t�����W�ޒ[�`�󗧏グ��
		//�\�[���v���[�g
		m_SolePlateKyotyokuFreeSpace = _T("15.0");		//�\�[���v���[�g�̋��������󂫗�
		m_SolePlateKyojikuFreeSpace = _T("20.0");		//�\�[���v���[�g�̋��������󂫗�
		//���̑�
		m_ItatsugiZureRyo = _T("100.0");				//�p�Y����
		m_LflgKakuhukubuTaper = _T("5");				//���t�����W�g�����̃e�[�p�[���z
		m_WebHoleSlopeLowerLimitGrd = _T("3.0");		//�E�F�u�E�̍E���z�̉����l
		//�����⍄��
		m_ShitenVsCutWu = _T("15C");					//�x�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
		m_ShitenVsCutWd = _T("15C");					//�x�_�㐂���⍄�ނ̗n�ڕӑ������،�
		m_ShitenVsCutFu = _T("10-45");						//�x�_�㐂���⍄�ނ̏㑤�،�
		m_KakutenVsCutWu = _T("35R");					//�i�_�㐂���⍄�ނ̗n�ڕӑ��㑤�،�
		m_KakutenVsCutWd = _T("35R");					//�i�_�㐂���⍄�ނ̗n�ڕӑ������،�
		m_KakutenVsCutFu = _T("10-45");					//�i�_�㐂���⍄�ނ̏㑤�،�
		m_MiddleVsCutWu = _T("35R");					//���Ԑ����⍄�ނ̗n�ڕӑ��㑤�،�
		m_MiddleVsCutWd = _T("35R");					//���Ԑ����⍄�ށi���k�^�C�v�j�̗n�ڕӑ������،�
		m_MiddleVsFreeSpace = _T("35.0");				//���Ԑ����⍄�ށi�����^�C�v�j�̉����󂫗�
		//�����⍄��
		m_HsFreeSpaceVs = _T("35.0");					//�����⍄�ނ̐����⍄�ޕ��A�����E�F�u���󂫗�
		m_HsFreeSpaceSpl = _T("20.0");					//�����⍄�ނ̓Y�ڔ��󂫗�
		m_HsFreeSpaceCbf = _T("20.0");					//�����⍄�ނ̉����t�����W���󂫗�
		m_HsFreeSpaceCbfUlimit = _T("100.0");			//�����⍄�ނ̉����t�����W����̍������@���
		m_HsSnipSizeVs = _T("10-45");					//�����⍄�ނ̐����⍄�ޕ��̃X�j�b�v�T�C�Y
		m_HsSnipSizeSpl = _T("10-45");					//�����⍄�ނ̓Y�ڕ��̃X�j�b�v�T�C�Y
		m_HsSnipSizeCbf = _T("10-45");					//�����⍄�ނ̉����t�����W���̃X�j�b�v�T�C�Y
		//�Y�ڔ�
		m_UflgSplKyojikuZaitan = _T("40.0");			//��t�����W�Y�ڔ̋��������ޒ[
		m_UflgOutsideSplKyotyokuZaitan = _T("40.0");	//��t�����W�O���Y�ڔ̋��������ޒ[
		m_UflgInsideSplKyotyokuZaitan = _T("40.0");		//��t�����W�����Y�ڔ̋��������ޒ[
		m_WebSplKyojikuZaitan = _T("40.0");				//�E�F�u�Y�ڔ̋��������ޒ[
		m_WebSplHeightZaitan = _T("40.0");				//�E�F�u�Y�ڔ̍��������ޒ[
		m_LflgSplKyojikuZaitan = _T("40.0");			//���t�����W�Y�ڔ̋��������ޒ[
		m_LflgSplKyotyokuZaitan = _T("40.0");			//���t�����W�Y�ڔ̋��������ޒ[
		m_ShitenUflgJc = "10.0";		//�x�_�㉡���iBH�j��t�����W�̃W���C���g�N���A�����X
		m_ShitenWebJc = "10.0";			//�x�_�㉡���iBH�j�E�F�u�̃W���C���g�N���A�����X
		m_ShitenLflgJc = "10.0";		//�x�_�㉡���iBH�j���t�����W�̃W���C���g�N���A�����X
		m_KakutenHflgJc = "10.0";		//�i�_�㉡���iH�|�j�t�����W�̃W���C���g�N���A�����X
		m_KakutenHwebJc = "10.0";		//�i�_�㉡���iH�|�j�E�F�u�̃W���C���g�N���A�����X
		//�R�l�N�V�����v���[�g
		m_ShitenConnCut = _T("15C");			//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӑ��،�
		m_ShitenConnFillet = _T("100.0");		//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̃t�B���b�g��R�T�C�Y
		m_ShitenConnTachiageryo = _T("20.0");	//�包�E�F�u�t���R�l�N�V�����i�x�_��j�̗n�ڕӗ��グ��
		m_KakutenConnCut = _T("15C");			//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӑ��،����w��
		m_KakutenConnFillet = _T("100.0");		//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̃t�B���b�g��R�T�C�Y
		m_KakutenConnTachiageryo = _T("20.0");	//�包�E�F�u�t���R�l�N�V�����i�i�_��j�̗n�ڕӗ��グ��
		//�����⍄��
		m_CvsCutWu = _T("35R");					//�����t�����⍄�ނ̗n�ڕӑ��㑤�،�
		m_CvsCutWd = _T("35R");					//�����t�����⍄�ނ̗n�ڕӑ������،�
		//���̑�
		m_WebHoleSlopeLowerLimitCrs = _T("3.0");	//�E�F�u�E�̍E���z�̉����l
		m_FlgSectionType = 0;						//�t�����W�،��̕���
		//�ޒ[�`��
		m_ShitenUflgSplKyojikuZaitan = _T("40.0");		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
		m_ShitenUflgSplKyotyokuZaitan = _T("40.0");		//�x�_�㉡����t�����W�Y�ڔ̋��������ޒ[
		m_ShitenWebSplKyotyokuZaitan = _T("40.0");		//�x�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
		m_ShitenWebSplHeightZaitan = _T("40.0");		//�x�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
		m_ShitenLflgSplKyojikuZaitan = _T("40.0");		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
		m_ShitenLflgSplKyotyokuZaitan = _T("40.0");		//�x�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
		m_ShitenConnKyojikuZaitan = _T("40.0");			//�x�_��R�l�N�V�����̋��������ޒ[
		m_ShitenConnKyoutyokuZaitan = _T("40.0");		//�x�_��R�l�N�V�����̋��������ޒ[
		m_KakutenUflgSplKyojikuZaitan = _T("40.0");		//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
		m_KakutenUflgSplKyotyokuZaitan = _T("40.0");	//�i�_�㉡����t�����W�Y�ڔ̋��������ޒ[
		m_KakutenWebSplKyotyokuZaitan = _T("40.0");		//�i�_�㉡���E�F�u�Y�ڔ̋��������ޒ[
		m_KakutenWebSplHeightZaitan = _T("40.0");		//�i�_�㉡���E�F�u�Y�ڔ̍��������ޒ[
		m_KakutenLflgSplKyojikuZaitan = _T("40.0");		//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
		m_KakutenLflgSplKyotyokuZaitan = _T("40.0");	//�i�_�㉡�����t�����W�Y�ڔ̋��������ޒ[
		m_KakutenConnKyojikuZaitan = _T("40.0");		//�i�_��R�l�N�V�����̋��������ޒ[
		m_KakutenConnKyoutyokuZaitan = _T("40.0");		//�i�_��R�l�N�V�����̋��������ޒ[
	} else {
		readPreviousInput(type, fileStorage);
	}
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD3)->SetWindowText(m_UflgJc);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD4)->SetWindowText(m_WebJc);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD5)->SetWindowText(m_LflgJc);
	((CComboBox*)m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD7))->SetCurSel(m_UflgZaitanKeijo);
	((CComboBox*)m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD8))->SetCurSel(m_LflgZaitanKeijo);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD9)->SetWindowText(m_UpdownFlgZaitanKeijoTachiageRyo);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD11)->SetWindowText(m_SolePlateKyotyokuFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD12)->SetWindowText(m_SolePlateKyojikuFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD6)->SetWindowText(m_ItatsugiZureRyo);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD10)->SetWindowText(m_LflgKakuhukubuTaper);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD36)->SetWindowText(m_WebHoleSlopeLowerLimitGrd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD20)->SetWindowText(m_ShitenVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD21)->SetWindowText(m_ShitenVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD22)->SetWindowText(m_ShitenVsCutFu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD23)->SetWindowText(m_KakutenVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD24)->SetWindowText(m_KakutenVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD25)->SetWindowText(m_KakutenVsCutFu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD26)->SetWindowText(m_MiddleVsCutWu);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD27)->SetWindowText(m_MiddleVsCutWd);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD28)->SetWindowText(m_MiddleVsFreeSpace);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD13)->SetWindowText(m_HsFreeSpaceVs);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD14)->SetWindowText(m_HsFreeSpaceSpl);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD15)->SetWindowText(m_HsFreeSpaceCbf);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD16)->SetWindowText(m_HsFreeSpaceCbfUlimit);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD17)->SetWindowText(m_HsSnipSizeVs);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD18)->SetWindowText(m_HsSnipSizeSpl);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD19)->SetWindowText(m_HsSnipSizeCbf);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD29)->SetWindowText(m_UflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD30)->SetWindowText(m_UflgOutsideSplKyotyokuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD31)->SetWindowText(m_UflgInsideSplKyotyokuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD32)->SetWindowText(m_WebSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD33)->SetWindowText(m_WebSplHeightZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD34)->SetWindowText(m_LflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[0]->GetDlgItem(IDC_GRD35)->SetWindowText(m_LflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS1)->SetWindowText(m_ShitenUflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS2)->SetWindowText(m_ShitenWebJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS3)->SetWindowText(m_ShitenLflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS4)->SetWindowText(m_KakutenHflgJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS5)->SetWindowText(m_KakutenHwebJc);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS6)->SetWindowText(m_ShitenConnCut);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS7)->SetWindowText(m_ShitenConnFillet);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS8)->SetWindowText(m_ShitenConnTachiageryo);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS9)->SetWindowText(m_KakutenConnCut);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS10)->SetWindowText(m_KakutenConnFillet);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS11)->SetWindowText(m_KakutenConnTachiageryo);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS12)->SetWindowText(m_CvsCutWu);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS13)->SetWindowText(m_CvsCutWd);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS30)->SetWindowText(m_WebHoleSlopeLowerLimitCrs);
	((CComboBox*)m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS31))->SetCurSel(m_FlgSectionType);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS14)->SetWindowText(m_ShitenUflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS15)->SetWindowText(m_ShitenUflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS16)->SetWindowText(m_ShitenWebSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS17)->SetWindowText(m_ShitenWebSplHeightZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS18)->SetWindowText(m_ShitenLflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS19)->SetWindowText(m_ShitenLflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS20)->SetWindowText(m_ShitenConnKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS21)->SetWindowText(m_ShitenConnKyoutyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS22)->SetWindowText(m_KakutenUflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS23)->SetWindowText(m_KakutenUflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS24)->SetWindowText(m_KakutenWebSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS25)->SetWindowText(m_KakutenWebSplHeightZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS26)->SetWindowText(m_KakutenLflgSplKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS27)->SetWindowText(m_KakutenLflgSplKyotyokuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS28)->SetWindowText(m_KakutenConnKyojikuZaitan);
	m_MainTab.m_Dialog[1]->GetDlgItem(IDC_CRS29)->SetWindowText(m_KakutenConnKyoutyokuZaitan);

	UpdateData(FALSE);
}

bool CApolloToDesignDlg::fileExists( LPCTSTR filename )
{
	bool result;
	WIN32_FIND_DATA wfd;
	HANDLE hFile = FindFirstFile( filename, &wfd );

	if( hFile == INVALID_HANDLE_VALUE ){
		result = false;
	}else{
		result = true;
	}

	FindClose( hFile );
	return result;
}

void CApolloToDesignDlg::printMessage(
	CString& result		//START,END,ERROR 
)
{
	time_t     ltime;
	struct tm  *ltm;

	time(&ltime);
	ltm = localtime(&ltime);
	char strdate[64],strtime[64];
	sprintf(strdate,"%u/%02u/%02u",ltm->tm_year+1900,++ltm->tm_mon,ltm->tm_mday);
	sprintf(strtime," %02u:%02u:%02u",ltm->tm_hour,ltm->tm_min,ltm->tm_sec);
	string sd = strdate;
	string st = strtime;
	
	stringstream str;
	str << "<" << sd << st << ">" << endl;

	string message;
	static char buf[128] = {""};

	if ( "START" == result ) {
		sprintf(buf, "ApolloToDesign.exe Build No.<%s %s>\n",__DATE__,__TIME__ );
		log_MsgPrintf(buf);
		message = "�� ����ގ��s�J�n�FAPOLLO�f�[�^�ː݌v��񑮐��t�@�C��(ApolloToDesign) " + str.str() + "\n";
	} else if ( "END" == result ) {
		message = "�� ����ސ���I���FAPOLLO�f�[�^�ː݌v��񑮐��t�@�C��(ApolloToDesign) " + str.str() + "\n";
	} else {
		message = "�~ ����ވُ�I���FAPOLLO�f�[�^�ː݌v��񑮐��t�@�C��(ApolloToDesign) " + str.str() + "\n";
	}
	log_MsgPrintf(message.c_str());
}


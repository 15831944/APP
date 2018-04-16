/********************************************************************
	created:	2008/12/17
	author:		p.chen
	
	purpose:	
*********************************************************************/

#include "StdAfx.h"
#include <SHARE.h>
#include "LogPrint.h"
#ifndef _EXE
#ifndef IJCAD2015
#include "sds.h"
#else
#include "adslib.h"
#endif
#else
#endif

/*:***********************************************************************
��  �O�Flog_DateTime
�@  �\�F���݂̓��t�Ǝ����̕������Ԃ�
�߂�l�F�Ȃ�
��  �l�F���t(yyyy/mm/dd),����(hh:mm:ss)
*************************************************************************/
#ifdef _UNICODE
void log_DateTimeEx(
				  wchar_t *strdate,		/*(I ) ���t */
				  wchar_t *strtime		/*(I ) ���� */
				  )
#else
void log_DateTime(
				  char	*strdate,		/*(I ) ���t */
				  char	*strtime		/*(I ) ���� */
				  )
/*:*/
#endif // _UNICODE
{
	time_t     ltime;
	struct tm  *ltm;
	
	time(&ltime);
	ltm = localtime(&ltime);
	_stprintf(strdate,_T("%u/%02u/%02u"),ltm->tm_year+1900,++ltm->tm_mon,ltm->tm_mday);
	_stprintf(strtime,_T("%02u:%02u:%02u"),ltm->tm_hour,ltm->tm_min,ltm->tm_sec);
}


/*:***********************************************************************
��  �O�Flog_Fopen
�@  �\�F۸�̧�ق��J��
�߂�l�F����I���F̧���߲��
�ُ�I���FNULL�߲��
��  �l�F̧�قɑ΂��鏑�����ݱ��������ۂ��܂��B
*************************************************************************/
FILE *log_FileOpen(void)
{
	return (_fsopen(LOG_FILENAME,"at",_SH_DENYWR));
}

/*:***********************************************************************
��  �O�Flog_Fclose
�@  �\�F۸�̧�ق����
�߂�l�F����I���F0 ��Ԃ��܂��B
�ُ�I���FEOF ��Ԃ��܂��B
��  �l�F
*************************************************************************/
int log_FileClose(
			   FILE	*stream		/*(I ) ̧���߲�� */
			   )/*:*/
{
	return (fclose(stream));
}

/*:***********************************************************************
��  �O�Flog_Printf
�@  �\�F۸�̧�قɏ����t�ŏo��
�߂�l�F����I���F�o�͂�����������Ԃ��܂��B
�ُ�I���F���̒l��Ԃ��܂��B
��  �l�F
*************************************************************************/
int log_MsgPrintf(
				  const char	*format, ...	/*(I ) �������䕶����,��߼�݂̈��� */
				  )/*:*/
{
	int			stat;
	va_list		args;
	FILE		*stream;

	/* ۸�̧�ٵ���� */
	if((stream = log_FileOpen()) == NULL) return -1;

	/* ����ؽĂ�������擾 */
	va_start(args,format);

	/* ۸�̧�قɏo�� */
#ifdef _PMFCAD
	stat = vfprintf(stream,format,args);
#else
#ifdef _UNICODE
	stat = _vftprintf(stream,format,args);
#else
	stat = vfprintf(stream,format,args);
#endif
#endif

	/* ����ؽĂ��߲��ؾ�� */
	va_end(args);

	/* ۸�̧�ٸ۰�� */
	log_FileClose(stream);

	return stat;
}


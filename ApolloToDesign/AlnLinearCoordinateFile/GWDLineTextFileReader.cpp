#pragma warning (disable:4786)

#include "stdafx.h"
#include "GWDLineTextFileReader.h"

JptErrorStatus GWDLineTextFileReader::load(std::ifstream &ifs, AlnLinearCoordinateFile *lcfPtr)
{
	if (readFileSet(ifs, lcfPtr) != JPT_OK ) {
		errorPush("***ERROR*** ���`���W�l�t�@�C��["+this->_fileName+"]�̓ǂݍ��݂Ɏ��s���܂����B\n");
		ifs.close();
		return JPT_ERROR;
	}
	ifs.close();
	return JPT_OK;
}

JptErrorStatus GWDLineTextFileReader::readFileOpen(const std::string& name, std::ifstream& ifs)
{
	// 2017/04/06 Nakagawa Add Start  VS2005���Ɠ��{��̃t�@�C�������ǂݍ��߂Ȃ��B
	locale default_locale;
	locale::global(locale("japanese"));
	// 2017/04/06 Nakagawa Add End
	ifs.open(name.c_str(), std::ios::in);
	// 2017/04/06 Nakagawa Add Start
	locale::global(locale(default_locale));
	// 2017/04/06 Nakagawa Add End
	if ( !ifs) {
		return JPT_ERROR;
	}
	this->_fileName = name;
	return JPT_OK;		
}

JptErrorStatus GWDLineTextFileReader::readFileSet(std::ifstream& ifs, AlnLinearCoordinateFile *lcfPtr)
{
	int lineNumber = 0;
	std::string line;
	std::getline(ifs, line);	//	comment
	lineNumber++; 

	for ( ;; ) {

		long initialLoc = ifs.tellg();
		std::getline(ifs, line);
		lineNumber++;

		if ( line.empty() ) {
			break;
		}

		StringTokenizer lineStr (line, ",");
		AlnLinearCoordinateData lcd;
		lcd.setLineName(lineStr[0]);
		
		ifs.seekg(initialLoc);
		if (setAlnLinearCoordinateData(lineNumber, ifs, lcd) != JPT_OK ) {

			std::stringstream ss;
			ss << "***ERROR*** " << lineNumber <<" �s�ڂœǂݍ��݂Ɏ��s���܂����B\n";
			errorPush( ss.str() ); 
			return JPT_ERROR;
		}
		lcfPtr->append(lcd);
	}
	return JPT_OK;
}

JptErrorStatus GWDLineTextFileReader::setAlnLinearCoordinateData( int& lineNumber,
																 std::ifstream& ifs, 
																 AlnLinearCoordinateData& lcd)
{
	for ( ; ; ) {
		long getLoc = ifs.tellg();
		std::string line;
		std::getline(ifs, line);
		lineNumber++;
		if ( line.empty() ) {
			break;
		}

		StringTokenizer lineStr (line, ",");
		if ( lineStr[0] != lcd.getLineName() ) {
			ifs.seekg(getLoc);
			break;
		}
		if ( lineStr.sizeStr() < 5 ) {
			continue;
		}
		if ( lineStr.sizeStr() == 5 && lineStr[2] == "" ) {
			continue;
		}
		if ( setAlnPointData (lineStr, lcd) != JPT_OK ) {
			return JPT_ERROR;
		}	
	}
	return JPT_OK;	
}

JptErrorStatus GWDLineTextFileReader::setAlnPointData( StringTokenizer& lineStr, AlnLinearCoordinateData& lcd )
{
	if ( lineStr.sizeStr() != 5 ) {
		return JPT_ERROR;
	}

	AlnPointData pointData;
	pointData.setPointName( lineStr[1] );
	pointData.setX( atof(lineStr[2].c_str()) * 1000.0);
	pointData.setY( atof(lineStr[3].c_str()) * 1000.0);
	pointData.setHeight( atof ( lineStr[4].c_str()) * 1000.0);

	if ( lcd.isAlreadyPoint( lineStr[1] ) ) {
		return JPT_ERROR;
	}
	lcd.append( pointData);
	return JPT_OK;
}

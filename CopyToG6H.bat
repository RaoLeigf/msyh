rem echo off

echo XM
set SUITECODE=GXM
set SUITE=GXM3.XM
xcopy H:\SuitCode\G6H\i8WebSite\NGWebSite\Areas\%SUITECODE%\XM\Views\ProjectMst\*.* D:\G6HMidServer\NGWebSite\Areas\%SUITECODE%\XM\Views\ProjectMst\ /s /y
xcopy H:\SuitCode\G6H\BusinessDlls\bin\%SUITE%.* D:\G6HMidServer\NGWebSite\bin\ /s /y	
xcopy H:\SuitCode\G6H\BusinessDlls\Rules\%SUITE%.* D:\G6HMidServer\NGWebSite\I6Rules\ /s /y
xcopy H:\SuitCode\G6H\i8WebSite\NGWebSite\NG3Resource\IndividualInfo\%SUITECODE%\*.* D:\G6HMidServer\NGWebSite\NG3Resource\IndividualInfo\%SUITECODE%\ /s /y

echo YS
set SUITECODE=GYS
set SUITE=GYS3.YS
xcopy H:\SuitCode\G6H\i8WebSite\NGWebSite\Areas\%SUITECODE%\*.* D:\G6HMidServer\NGWebSite\Areas\%SUITECODE%\ /s /y	
xcopy H:\SuitCode\G6H\BusinessDlls\bin\%SUITE%.* D:\G6HMidServer\NGWebSitebin\ /s /y	
xcopy H:\SuitCode\G6H\BusinessDlls\Rules\%SUITE%.* D:\G6HMidServer\NGWebSite\I6Rules\ /s /y
xcopy H:\SuitCode\G6H\i8WebSite\NGWebSite\NG3Resource\IndividualInfo\%SUITECODE%\*.* D:\G6HMidServer\NGWebSite\NG3Resource\IndividualInfo\%SUITECODE%\ /s /y

echo JX
set SUITECODE=GJX
set SUITE=GJX3.JX
xcopy H:\SuitCode\G6H\i8WebSite\NGWebSite\Areas\%SUITECODE%\*.* D:\G6HMidServer\NGWebSite\Areas\%SUITECODE%\ /s /y	
xcopy H:\SuitCode\G6H\BusinessDlls\bin\%SUITE%.* D:\G6HMidServer\NGWebSite\bin\ /s /y	
xcopy H:\SuitCode\G6H\BusinessDlls\Rules\%SUITE%.* D:\G6HMidServer\NGWebSite\I6Rules\ /s /y
xcopy H:\SuitCode\G6H\i8WebSite\NGWebSite\NG3Resource\IndividualInfo\%SUITECODE%\*.* D:\G6HMidServer\NGWebSite\NG3Resource\IndividualInfo\%SUITECODE%\ /s /y

echo QT
set SUITECODE=GQT
set SUITE=GQT3.QT
xcopy H:\SuitCode\G6H\i8WebSite\NGWebSite\Areas\%SUITECODE%\*.* D:\G6HMidServer\NGWebSite\Areas\%SUITECODE%\ /s /y	
xcopy H:\SuitCode\G6H\BusinessDlls\bin\%SUITE%.* D:\G6HMidServer\NGWebSite\bin\ /s /y	
xcopy H:\SuitCode\G6H\BusinessDlls\Rules\%SUITE%.* D:\G6HMidServer\NGWebSite\I6Rules\ /s /y
xcopy H:\SuitCode\G6H\i8WebSite\NGWebSite\NG3Resource\IndividualInfo\%SUITECODE%\*.* D:\G6HMidServer\NGWebSite\NG3Resource\IndividualInfo\%SUITECODE%\ /s /y

echo HOME
set SUITECODE=GXM
set SUITE=GXM3.XM
xcopy H:\SuitCode\G6H\i8WebSite\NGWebSite\Areas\%SUITECODE%\XM\Views\DefinedDesk\*.* D:\G6HMidServer\NGWebSite\Areas\%SUITECODE%\XM\Views\DefinedDesk\ /s /y	





rem NGXCopy close

pause
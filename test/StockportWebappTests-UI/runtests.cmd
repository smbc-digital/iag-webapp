@pushd %~dp0

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "StockportWebappTests-UI.csproj"

@if ERRORLEVEL 1 goto end

@cd ..\..\packages\SpecRun.Runner.*\tools

SpecRun.exe run Default.srprofile "/baseFolder:%~dp0\bin\Debug" /log:specrun.log /toolIntegration:TeamCity

:end

@popd

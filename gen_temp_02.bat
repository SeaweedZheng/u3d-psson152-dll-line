set WORKSPACE=.
set LUBAN_DLL=%WORKSPACE%\LuBan\Tools\Luban\Luban.dll
set CONF_ROOT=%WORKSPACE%\LuBan\DataTables

dotnet %LUBAN_DLL% ^
   -t client ^
    -c cs-simple-json ^
    -d json  ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=%WORKSPACE%\Assets\Scripts\LuBan\Gen ^
    -x outputDataDir=%WORKSPACE%\Assets\GameRes\LuBan\GenerateDatas\bytes

pause



:: LUBAN_DLL Luban.dll文件的路径。 指向 luban_examples/Tools/Luban/Luban.dll
:: CONF_ROOT 配置项目的路径。指向 luban_examples/DataTables
:: ‘-t’ 生成目标。可以为 client、server、all之类的值
:: ‘-c’ 生成的代码类型。 cs-simple-json为生成使用SimpleJSON加载json数据的c#代码
:: ‘-d’ 生成的数据类型
:: ‘outputCodeDir’ c#代码的输出目录
:: ‘outputDataDir’ json数据的输出目录

:: -x outputCodeDir=%WORKSPACE%\LubanTest\Assets\Script\Template ^
:: -x outputDataDir=%CONF_ROOT%\output

:: -x outputCodeDir=Assets\Scripts\LuBan\Gen 
:: -x outputDataDir=Assets\GameRes\LuBan\GenerateDatas\bytes 
:: -x pathValidator.rootDir=WORKSPACE
@echo off
setlocal enabledelayedexpansion

rem 获取 Unity 项目的 StreamingAssets 路径
rem 这里假设你已经在 Unity 中设置好了项目路径，并且 StreamingAssets 文件夹在正确的位置
rem 如果路径中有空格，需要用双引号括起来
set "streamingAssetsPath=%~dp0..\Assets\StreamingAssets"

rem 检查文件夹是否存在
if exist "!streamingAssetsPath!" (
    start "" "explorer.exe" "!streamingAssetsPath!"
) else (
    echo StreamingAssets 文件夹未找到:!streamingAssetsPath!
)

endlocal
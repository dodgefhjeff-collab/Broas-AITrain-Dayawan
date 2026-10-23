# 仪表时间校准逻辑测试 — 不连串口
# 用法:
#   powershell -ExecutionPolicy Bypass -File .\Run-YBTimeCalibrationTest.ps1

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

Write-Host "=== 编译并运行 YBTimeCalibrationTest ===" -ForegroundColor Cyan
dotnet run --project .\YBTimeCalibrationTest.csproj -c Release
exit $LASTEXITCODE

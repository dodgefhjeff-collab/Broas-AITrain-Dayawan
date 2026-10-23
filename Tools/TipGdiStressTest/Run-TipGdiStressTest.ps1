# UIMessageTip GDI 压测工具 — x86 Release
# 用法:
#   powershell -ExecutionPolicy Bypass -File .\Run-TipGdiStressTest.ps1

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

Write-Host "=== 编译 TipGdiStressTest (x86 / Release) ===" -ForegroundColor Cyan
dotnet build .\TipGdiStressTest.csproj -c Release -p:Platform=x86
if ($LASTEXITCODE -ne 0) {
    Write-Host "编译失败" -ForegroundColor Red
    exit $LASTEXITCODE
}

$exe = Join-Path $root "bin\x86\Release\net5.0-windows\TipGdiStressTest.exe"
if (-not (Test-Path $exe)) {
    Write-Host "未找到: $exe" -ForegroundColor Red
    exit 1
}

Write-Host "=== 启动: $exe ===" -ForegroundColor Cyan
Write-Host "建议: 点「突发压测」验证进程不退出、GDI 可回落" -ForegroundColor Yellow
Start-Process -FilePath $exe -WorkingDirectory (Split-Path $exe)

# firmar_y_publicar.ps1
Set-ExecutionPolicy Bypass -Scope Process -Force

$projectPath = "C:\Users\GAMT\Desktop\InfoRAM\InfoRAM"
$publishBase = "$projectPath\publish"
$pfxFile     = "$projectPath\InfoRAMDev.pfx"
$pfxPass     = "1234"
$timestampUrl = "http://timestamp.digicert.com"
$desc = "InfoRAM - Información detallada de la memoria RAM de la computadora"
$manifest = "$projectPath\app.manifest"

function Publicar-Y-Firmar($runtime, $outputFolder) {
    Write-Host "🚀 Publicando para $runtime..."

    dotnet publish "C:\Users\GAMT\Desktop\InfoRAM\InfoRAM\InfoRAMApp.csproj" `
        -c Release `
        -r $runtime `
        -o $outputFolder `
        /p:PublishTrimmed=false `
        /p:DebugType=none `
        /p:SelfContained=true

    $exe = Join-Path $outputFolder "InfoRAM.exe"

    if (-Not (Test-Path $exe)) {
        Write-Warning "❌ No se encontró el EXE en $exe"
        return
    }

    Write-Host "`n🔐 Firmando $exe ..."
    & "C:\Program Files (x86)\Windows Kits\10\bin\10.0.19041.0\x64\signtool.exe" sign `
        /f $pfxFile `
        /p $pfxPass `
        /fd sha256 `
        /td sha256 `
        /tr $timestampUrl `
        /d $desc `
        /du "https://inforam.local" `
        "$exe"

    Write-Host "`n🔍 Verificando firma..."
    Get-AuthenticodeSignature "$exe" | Format-Table -AutoSize
}

# Ejecutar publicación y firma para la plataforma deseada
Publicar-Y-Firmar "win-x64" "$publishBase"

Write-Host "`n✅ Proceso completado."

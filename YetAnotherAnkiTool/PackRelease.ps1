# Path to your published EXE
$exePath = "bin\Release\net8.0-windows\win-x64\publish\YetAnotherAnkiTool.exe"

# Extract version from EXE metadata
$version = (Get-Item $exePath).VersionInfo.FileVersion

# Define output zip name
$zipName = "YetAnotherAnkiTool_win-x64_v$version.zip"
$zipPath = Join-Path -Path $PSScriptRoot -ChildPath $zipName

# Delete any existing zip files that match the pattern
Get-ChildItem -Path $PSScriptRoot -Filter "YetAnotherAnkiTool_win-x64*.zip" | ForEach-Object {
    Remove-Item $_.FullName -Force
    Write-Host "🗑️ Removed old zip: $($_.Name)"
}

# Delete .pdb file from publish folder
$pdbPath = "bin\Release\net8.0-windows\win-x64\publish\YetAnotherAnkiTool.pdb"
if (Test-Path $pdbPath) {
    Remove-Item $pdbPath -Force
    Write-Host "🗑️ Removed .pdb file"
}

# Collect top-level files excluding .txt and subfolders
$publishDir = "bin\Release\net8.0-windows\win-x64\publish"
$filesToZip = Get-ChildItem -Path $publishDir -File | Where-Object {
    $_.Extension -ne ".txt"
}

# Zip the filtered files
Compress-Archive -Path $filesToZip.FullName -DestinationPath $zipPath -Force

Write-Host "✅ Packed release: $zipName"
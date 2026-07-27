# Script: Export-ClientSource.ps1
# Description: Export all essential source code files from MarziHandicrafts.Client project to a single text file

$projectPath = "D:\Projects\MarziHandicrafts.Client"
$outputFile = "D:\Projects\MarziHandicrafts.Client_Source_All.txt"

# Define which file extensions to include
$extensions = @("*.cs", "*.xaml", "*.xaml.cs", "*.razor", "*.razor.cs", "*.json", "*.csproj")

# Define folders to exclude
$excludeFolders = @("bin", "obj", "node_modules", ".git", "packages", "wwwroot/lib", "wwwroot/css", "wwwroot/js")

Write-Host "Starting export of Client source code..." -ForegroundColor Green
Write-Host "Project Path: $projectPath" -ForegroundColor Yellow
Write-Host "Output File: $outputFile" -ForegroundColor Yellow
Write-Host ""

# Clear or create the output file
"===== Client Source Code Export" | Out-File -FilePath $outputFile -Encoding UTF8
"Generated: $(Get-Date -Format 'MM/dd/yyyy HH:mm:ss')" | Out-File -FilePath $outputFile -Append -Encoding UTF8
"Project: $projectPath" | Out-File -FilePath $outputFile -Append -Encoding UTF8
"" | Out-File -FilePath $outputFile -Append -Encoding UTF8
"=" * 50 | Out-File -FilePath $outputFile -Append -Encoding UTF8
"" | Out-File -FilePath $outputFile -Append -Encoding UTF8

# Counter for files processed
$fileCount = 0
$totalSize = 0

# Get all files recursively
Get-ChildItem -Path $projectPath -Recurse -File | ForEach-Object {
    $file = $_
    $relativePath = $file.FullName.Substring($projectPath.Length + 1)
    
    # Check if file extension matches
    $includeFile = $false
    foreach ($ext in $extensions) {
        if ($file.Name -like $ext) {
            $includeFile = $true
            break
        }
    }
    
    # Check if file is in excluded folders
    $excludeFile = $false
    foreach ($folder in $excludeFolders) {
        if ($file.DirectoryName -like "*\$folder*" -or $file.DirectoryName -like "*\$folder") {
            $excludeFile = $true
            break
        }
    }
    
    # Include only if not excluded and extension matches
    if ($includeFile -and -not $excludeFile) {
        try {
            $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8
            $fileSize = [math]::Round($file.Length / 1KB, 2)
            
            "" | Out-File -FilePath $outputFile -Append -Encoding UTF8
            "=" * 50 | Out-File -FilePath $outputFile -Append -Encoding UTF8
            "FILE: $relativePath" | Out-File -FilePath $outputFile -Append -Encoding UTF8
            "Size: $fileSize KB" | Out-File -FilePath $outputFile -Append -Encoding UTF8
            "=" * 50 | Out-File -FilePath $outputFile -Append -Encoding UTF8
            $content | Out-File -FilePath $outputFile -Append -Encoding UTF8
            
            $fileCount++
            $totalSize += $file.Length
            Write-Host "Added: $relativePath ($fileSize KB)" -ForegroundColor Cyan
        }
        catch {
            Write-Host "Error reading file: $relativePath" -ForegroundColor Red
            Write-Host "Error: $_" -ForegroundColor Red
        }
    }
}

$totalSizeMB = [math]::Round($totalSize / 1MB, 2)
""
"=" * 50 | Out-File -FilePath $outputFile -Append -Encoding UTF8
"Export Complete!" | Out-File -FilePath $outputFile -Append -Encoding UTF8
"Total Files: $fileCount" | Out-File -FilePath $outputFile -Append -Encoding UTF8
"Total Size: $totalSizeMB MB" | Out-File -FilePath $outputFile -Append -Encoding UTF8
"Output: $outputFile" | Out-File -FilePath $outputFile -Append -Encoding UTF8

Write-Host ""
Write-Host "Export Complete!" -ForegroundColor Green
Write-Host "Total Files: $fileCount" -ForegroundColor Yellow
Write-Host "Total Size: $totalSizeMB MB" -ForegroundColor Yellow
Write-Host "Output File: $outputFile" -ForegroundColor Green
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
Read-Host
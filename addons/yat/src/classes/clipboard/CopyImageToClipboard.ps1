param (
    [string]$imagePath
)

# Check if the path provided is valid
if (-not (Test-Path $imagePath -PathType Leaf)) {
    Write-Host "Invalid image path. Please provide a valid path to an image file."
    exit 1
}

Add-Type -AssemblyName System.Windows.Forms

$imageBytes = [System.IO.File]::ReadAllBytes($imagePath)
$imageStream = New-Object System.IO.MemoryStream(,$imageBytes)
$image = [System.Drawing.Image]::FromStream($imageStream)
$bitmap = New-Object System.Drawing.Bitmap($image)

[System.Windows.Forms.Clipboard]::SetImage($bitmap)

$imageStream.Dispose()
$image.Dispose()
$bitmap.Dispose()

Write-Host "Image copied to clipboard successfully."

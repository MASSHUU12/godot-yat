[CmdletBinding()]
param (
    [Parameter(ValueFromRemainingArguments = $true)]
    [string]$params = ""
)

process {
    if (-not $env:GODOT) {
        Write-Host "GODOT environment variable not set"
        exit 1
    }

    if (-not (Test-Path $env:GODOT -ErrorAction SilentlyContinue)) {
        Write-Host "Godot executable not found at $env:GODOT"
        exit 1
    }

    & $env:GODOT ./ --headless -- --confirma-run --confirma-quit $params
}

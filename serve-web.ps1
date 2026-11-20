param(
    [Parameter(Mandatory = $true)]
    [string] $Root
)

$ErrorActionPreference = 'Stop'
$Root = [System.IO.Path]::GetFullPath($Root)
$RootPrefix = $Root.TrimEnd([System.IO.Path]::DirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar

if (-not (Test-Path -LiteralPath (Join-Path $Root 'index.html') -PathType Leaf)) {
    throw "Web export not found under '$Root'. Run run-web-debug.bat to export it."
}

$Listener = $null
$Url = $null
foreach ($Port in 8771..8780) {
    $Candidate = [System.Net.HttpListener]::new()
    $CandidateUrl = "http://127.0.0.1:$Port/"
    $Candidate.Prefixes.Add($CandidateUrl)
    try {
        $Candidate.Start()
        $Listener = $Candidate
        $Url = $CandidateUrl
        break
    } catch [System.Net.HttpListenerException] {
        $Candidate.Close()
    }
}

if ($null -eq $Listener) {
    throw 'Could not start the Web server on ports 8771-8780. Stop an existing server or free one of these ports.'
}

Write-Host "Serving $Root at $Url" -ForegroundColor Green
Write-Host 'Cross-Origin-Opener-Policy: same-origin'
Write-Host 'Cross-Origin-Embedder-Policy: require-corp'
Write-Host 'Press Ctrl+C to stop the server.'

$ChromeCandidates = @()
$ChromeCommand = Get-Command 'chrome.exe' -ErrorAction SilentlyContinue
if ($null -ne $ChromeCommand) {
    $ChromeCandidates += $ChromeCommand.Source
}
$ChromeCandidates += Join-Path $env:ProgramFiles 'Google\Chrome\Application\chrome.exe'
if (${env:ProgramFiles(x86)}) {
    $ChromeCandidates += Join-Path ${env:ProgramFiles(x86)} 'Google\Chrome\Application\chrome.exe'
}

$ChromePath = $ChromeCandidates |
    Where-Object { $_ -and (Test-Path -LiteralPath $_ -PathType Leaf) } |
    Select-Object -First 1

if ($ChromePath) {
    Start-Process -FilePath $ChromePath -ArgumentList @('--auto-open-devtools-for-tabs', $Url)
} else {
    Write-Host 'Chrome was not found; opening the default browser. Press F12 to view its Console.'
    Start-Process $Url
}

try {
    while ($Listener.IsListening) {
        $Context = $Listener.GetContext()
        $Response = $Context.Response
        $Response.Headers['Cross-Origin-Opener-Policy'] = 'same-origin'
        $Response.Headers['Cross-Origin-Embedder-Policy'] = 'require-corp'
        $Response.Headers['Cache-Control'] = 'no-store'

        $RelativePath = [Uri]::UnescapeDataString($Context.Request.Url.AbsolutePath.TrimStart('/'))
        if ([string]::IsNullOrWhiteSpace($RelativePath)) {
            $RelativePath = 'index.html'
        }

        $FilePath = [System.IO.Path]::GetFullPath((Join-Path $Root $RelativePath))
        if (
            -not $FilePath.StartsWith($RootPrefix, [System.StringComparison]::OrdinalIgnoreCase) -or
            -not (Test-Path -LiteralPath $FilePath -PathType Leaf)
        ) {
            $Response.StatusCode = 404
            $Body = [System.Text.Encoding]::UTF8.GetBytes('Not found')
            $Response.ContentType = 'text/plain; charset=utf-8'
            $Response.ContentLength64 = $Body.Length
            if ($Context.Request.HttpMethod -ne 'HEAD') {
                $Response.OutputStream.Write($Body, 0, $Body.Length)
            }
            Write-Host "404 $($Context.Request.Url.AbsolutePath)"
            $Response.Close()
            continue
        }

        $Response.ContentType = switch ([System.IO.Path]::GetExtension($FilePath).ToLowerInvariant()) {
            '.html' { 'text/html; charset=utf-8' }
            '.js'   { 'application/javascript' }
            '.wasm' { 'application/wasm' }
            '.pck'  { 'application/octet-stream' }
            '.png'  { 'image/png' }
            '.svg'  { 'image/svg+xml' }
            '.json' { 'application/json' }
            default { 'application/octet-stream' }
        }

        $Body = [System.IO.File]::ReadAllBytes($FilePath)
        $Response.ContentLength64 = $Body.Length
        if ($Context.Request.HttpMethod -ne 'HEAD') {
            $Response.OutputStream.Write($Body, 0, $Body.Length)
        }
        Write-Host "$($Context.Request.HttpMethod) $($Context.Request.Url.AbsolutePath) -> 200"
        $Response.Close()
    }
} finally {
    $Listener.Stop()
    $Listener.Close()
}

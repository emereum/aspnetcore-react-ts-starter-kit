$ErrorActionPreference = "Stop"; # stop on all errors
Import-module webadministration # Need for test-path against iis sites / application pools

$productName = "TemplateProductName"
$projectName = "TemplateProductName.WebApi"

$packageDir = Join-Path $PSScriptRoot "../"
$packageWebDir = Join-Path $packageDir $projectName
$packageDbDir = Join-Path $packageDir "database"

$deploymentDriveLetter = "D:"
$deploymentDir = $deploymentDriveLetter + "/deployments/$($productName)/"
$deploymentWebDir = Join-Path $deploymentDir $projectName
# Chocolatey will notify the user that the app has been installed to this path
$env:ChocolateyPackageInstallLocation = $deploymentDir

function RequireSuccess {
    if($LASTEXITCODE -ne 0) {
        throw "The previous command exited with exit code " +$LASTEXITCODE + ". The script will abort."
    }
}

# Install flyway for database migrations. We'll run migrations when the website is offline
cinst javaruntime -y
RequireSuccess
cinst flyway.commandline --version 4.2.0 -y
RequireSuccess
refreshenv
RequireSuccess

# Load serversettings.json
$serverSettings = Get-Content -Raw -Path (Join-Path $packageDir "serversettings.json") | ConvertFrom-Json

# Ensure the deployment drive exists
if(!(Test-Path $deploymentDriveLetter)) {
    throw "Can't deploy because partition $deploymentDriveLetter does not exist. Create a $deploymentDriveLetter partition and try again."
}

# Create the deployments directory if it doesn't exist
if(!(Test-Path $deploymentDir)) {
    New-Item $deploymentDir -ItemType Directory
}

Write-Host "Ensure event viewer is closed if this deployment uses nssm (it locks nssm) before deploying."

if(Test-Path "IIS:\Sites\$($projectName)") {
    # Stop previous website
    if((Get-WebsiteState -Name $projectName).Value -eq "Started") {
        Write-Host "Stopping $($projectName) website..."
        Stop-Website -Name $projectName
    }
    
    # Remove previous website
    Write-Host "Deleting $($projectName) website..."
    Remove-Website -Name $projectName
}

if(Test-Path "IIS:\AppPools\$($projectName)") {
    # Stop previous application pool
    if((Get-WebAppPoolState -Name $projectName).Value -eq "Started") {
        Write-Host "Stopping $($projectName) application pool..."
        Stop-WebAppPool -Name $projectName
    }
    
    # Remove previous application pool
    Write-Host "Deleting $($projectName) application pool..."
    Remove-WebAppPool -Name $projectName
}

Write-Host "Waiting 10 seconds for website and application pools to release file locks..."
Start-Sleep -s 10

Write-Host "Deleting current deployment..."
if(Test-Path $deploymentWebDir) {
    Remove-Item -Recurse $deploymentWebDir
}

# Perform database migration
WRite-Host "Performing database migration..."
& "flyway" "-configFile=`"$(Join-Path $packageDbDir "flyway.conf")`"" "-locations=`"filesystem:$($packageDbDir)`"" "migrate"
RequireSuccess
& "flyway" "-configFile=`"$(Join-Path $packageDbDir "flyway.conf")`"" "-locations=`"filesystem:$($packageDbDir)`"" "info"
RequireSuccess
& "flyway" "-configFile=`"$(Join-Path $packageDbDir "flyway.conf")`"" "-locations=`"filesystem:$($packageDbDir)`"" "validate"
RequireSuccess

Write-Host "Copying new deployment..."
Copy-Item -Recurse $packageWebDir $deploymentWebDir

# Install application pool and website
Write-Host "Creating new $($projectName) application pool..."
New-WebAppPool -Name $projectName
Set-ItemProperty IIS:\AppPools\$projectName managedRuntimeVersion ""

Write-Host "Creating new $($projectName) web site..."
New-Website -Name $projectName -ApplicationPool $projectName -Port $serverSettings.iisBindPort -HostHeader $serverSettings.iisBindHostname -PhysicalPath $deploymentWebDir

Write-Host "Starting $($projectName) application pool..."
Start-WebAppPool -Name $projectName

Write-Host "Starting $($projectName) website..."
Start-Website -Name $projectName

Write-Host "Done." -ForegroundColor Green
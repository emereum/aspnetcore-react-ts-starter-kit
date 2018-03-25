# Chocolatey
Set-ExecutionPolicy Bypass
iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

# Dev environment
& ".\setup-build-environment.ps1"
cinst visualstudio2017community -y
cinst visualstudiocode -y
cinst javaruntime -y
cinst flyway.commandline --version 4.2.0 -y
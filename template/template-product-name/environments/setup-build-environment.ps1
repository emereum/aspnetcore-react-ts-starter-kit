#Requires -RunAsAdministrator

# Chocolatey
Set-ExecutionPolicy Bypass
iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

# Build toolchain
cinst git ruby putty dotnetcore-sdk nuget.commandline nodejs -y
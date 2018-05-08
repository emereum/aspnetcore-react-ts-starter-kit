#Requires -RunAsAdministrator

# Chocolatey
Set-ExecutionPolicy Bypass
iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

# Dev environment
& ".\setup-build-environment.ps1"
cinst visualstudio2017community -y
cinst visualstudiocode -y
cinst javaruntime -y
cinst flyway.commandline --version 4.2.0 -y
cinst postgresql --version 9.6.8 -y

# Configure vscode to use the TypeScript compiler in the workspace rather than
# vscode's own version. To do this manually, open vscode and click the
# TypeScript version at the bottom right, then choose "Use workspace version".
mkdir ..\src\TemplateProductName.WebClient\.vscode
cp resources\settings.json ..\src\TemplateProductName.WebClient\.vscode\settings.json
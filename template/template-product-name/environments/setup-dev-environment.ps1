#Requires -RunAsAdministrator

# Chocolatey
Set-ExecutionPolicy Bypass
iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

# Dev environment
& ".\setup-build-environment.ps1"
cinst visualstudio2019community visualstudiocode javaruntime flyway.commandline postgresql -y

# Configure vscode to use the TypeScript compiler in the workspace rather than
# vscode's own version. To do this manually, open vscode and click the
# TypeScript version at the bottom right, then choose "Use workspace version".
mkdir ..\src\TemplateProductName.WebClient\.vscode
cp resources\settings.json ..\src\TemplateProductName.WebClient\.vscode\settings.json
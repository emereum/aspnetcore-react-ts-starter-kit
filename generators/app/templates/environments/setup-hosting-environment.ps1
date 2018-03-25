# TemplateProductName Server Configuration Script
# This script configures a Windows Server from a blank slate to a point where
# TemplateProductName can be deployed and run on it over ssh. It installs
# IIS, dotnet core windows hosting, OpenSSH server, and sets up a private key.

# Manual tasks
    # * Create a D partition
    # * Copy private key from Administrator\.ssh\id_rsa to local machine

$ErrorActionPreference = "Stop"

# Needed for test-path against iis sites / application pools
Import-module WebAdministration

# Infrastructure
    # Chocolatey
    Set-ExecutionPolicy Bypass
    iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
    
    # IIS and dotnet core windows hosting
    cinst IIS-WebServerRole -source WindowsFeatures -y
    cinst dotnetcore-windowshosting --version 2.0.0 -y
    
    # OpenSSH server
    cinst openssh -params '"/SSHServerFeature"' -y
    
# Configuration
    # Reload path so we have access to ssh tools
    refreshenv
    
    # Remove default website and application pools
    if(Test-Path "IIS:\Sites\Default Web Site") {
        Remove-Website -Name "Default Web Site" -ErrorAction Ignore
    }
    
    if(Test-Path "IIS:\AppPools\DefaultAppPool") {
        Remove-WebAppPool -Name "DefaultAppPool" -ErrorAction Ignore
    }
    
    # Disable password authentication, allow public key authentication over ssh.
    $sshdConf = (
        "Port 22",
        "LogLevel QUIET",
        "AuthorizedKeysFile .ssh\authorized_keys",
        "PasswordAuthentication no",
        "ChallengeResponseAuthentication no",
        "Subsystem sftp sftp-server.exe"
    ) -join "`r`n"
    
    Set-Content `
        -Path "C:\Program Files\OpenSSH-Win64\sshd_config" `
        -Value $sshdConf
        
    Restart-Service sshd
    
    # Generate a keypair
    # Open the private key on client machine in puttygen through "Conversions"
    # menu and save private key to use via putty.
    New-Item `
        ($env:USERPROFILE + "\.ssh") `
        -ItemType Directory `
        -ErrorAction Ignore
        
    & "C:\Program Files\OpenSSH-Win64\ssh-keygen" `
        -t rsa `
        -f ""$env:USERPROFILE\.ssh\id_rsa"" `
        -b 2048 `
        -C ""$env:computername"" `
        -P """"
        
    Copy-Item `
        -Path ($env:USERPROFILE  + "\.ssh\id_rsa.pub") `
        -Destination ($env:USERPROFILE + "\.ssh\authorized_keys") `
        -Force
        
    # Grant SSHD service the ability to read authorized_keys (this is hideous)
    $acl = Get-Acl ($env:USERPROFILE + "\.ssh\authorized_keys")
    $permission = "NT SERVICE\SSHD","Read","Allow"
    $accessRule = New-Object `
        System.Security.AccessControl.FileSystemAccessRule `
        $permission
    $acl.SetAccessRule($accessRule)
    $acl | Set-Acl ($env:USERPROFILE + "\.ssh\authorized_keys")

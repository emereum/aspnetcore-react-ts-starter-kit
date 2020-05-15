Write-Host @'

 Welcome to the Template Product Name database setup tool

 I am about to create and populate a template_product_name database in your
 PostgreSQL database. Please ensure PostgreSQL is installed and that
 you have your postgres DBA account password ready. I need to initially
 log in to a maintenance database (typically postgres) but I will create
 a new database for Template Product Name.

 I will log in to the database as the postgres user. When prompted for a
 password please supply the postgres password.

'@

# Ensure flyway is on the PATH
if((Get-Command "flyway.exe" -ErrorAction SilentlyContinue) -eq $null)
{
    Write-Host "Please add flyway.exe to your PATH and try again." -ForegroundColor Red
    Write-Host "If you don't have flyway, install Chocolatey then run 'chocolatey install flyway.commandline'" -ForegroundColor Red
    exit
}

# Ensure psql is on the PATH
if((Get-Command "psql.exe" -ErrorAction SilentlyContinue) -eq $null)
{
    Write-Host "Please add psql.exe to your PATH and try again." -ForegroundColor Red
    exit
}

$server = "localhost"
$database = "template_product_name"
$schema = "template_product_name"
$port = 5432
$username = "postgres"

# Get the postgres password
if($env:scriptedPgPassword -eq $null)
{
    $password = Read-Host "Enter the password for the $username user on $server" -AsSecureString
    $password = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($password))
}
else
{
    $password = $env:scriptedPgPassword
}

# Create the database
$env:pgpassword = $password
psql -h $server -U postgres -d postgres -p 5432 -v ON_ERROR_STOP=0 -c "create database $($database)"

$opts = `
    "-url=jdbc:postgresql://$($server):$($port)/$($database)", `
    "-user=$($username)", `
    "-password=$($password)", `
    "-schemas=$($schema)", `
    "-placeholders.environment=dev", `
    "-locations=filesystem:."

& "flyway" $opts "clean"

if($LASTEXITCODE -ne 0) {
    Write-Host "Something went wrong while cleaning your database." -ForegroundColor Red
    Write-Host "Your database is NOT set up." -ForegroundColor Red
    Read-Host
    exit
}

& "flyway" $opts "migrate"

if($LASTEXITCODE -ne 0) {
    Write-Host "Something went wrong." -ForegroundColor Red
    Write-Host "Your database is NOT set up." -ForegroundColor Red
    Read-Host
    exit
}

& "flyway" $opts "info"

Write-Host "Done! Press Enter to exit." -ForegroundColor Green
Read-Host

# Automated Deployments

The files in this folder are used to generate a packaged file that contains all the build artifacts and scripts necessary to deploy this product to a test or prod environment.

## How it Works

The `rakefile.rb` script at the root of the repository can be used to package and deploy this product. The `rakefile.rb` script will invoke `chocolatey` which will use the `templateproductname.nuspec` file in this directory to determine what build artifacts and deployment scripts to compress into a single deployable `templateproductname.nupkg` file. Chocolatey is a wrapper around NuGet, so this package is actually a NuGet package. The `rakefile.rb` script will copy the package onto the target server using `pscp`, then install it using `plink`. The installation process invokes the `tools\chocolateyinstall.ps1` script on the target server. This script does all of the actual deployment work: It stops services, copies build artifacts, restarts services, etc.

The `tools\chocolateyinstall.ps1` should be idempotent. It is designed so that it can be repeatedly re-run regardless of whether the package is being installed for the first time, second time, or hundredth time. The package is always force-installed on the target server so the uninstall scripts in the `tools` folder are not run.


## How to Change the Deployment Process

* To change the deployment process that is executed on the target server modify `tools\chocolateyinstall.ps1`

* To change what data are copied during the deployment process modify `templateproductname.nupkg`

## More Information

* [Chocolatey wiki](https://github.com/chocolatey/choco/wiki)
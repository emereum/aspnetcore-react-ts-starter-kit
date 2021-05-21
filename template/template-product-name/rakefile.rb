# To see a summary of the tasks in this script type `rake -D` in a terminal.

PROJECT_NAME = "TemplateProductName"
CONFIGURATION = "Release"
TARGET_FRAMEWORK_MONIKER = "net5.0"

PROJECT_ROOT_DIR = File.dirname(__FILE__)
SRC_DIR = File.join(PROJECT_ROOT_DIR, "src")
SRC_WEB_DIR = File.join(SRC_DIR, "#{PROJECT_NAME}.WebApi")
SRC_WEBCLIENT_DIR = File.join(SRC_DIR, "#{PROJECT_NAME}.WebClient")
SRC_TESTS_DIR = File.join(SRC_DIR, "#{PROJECT_NAME}.Tests")
PUBLISH_WEB_DIR = File.join(SRC_WEB_DIR, "bin", CONFIGURATION , TARGET_FRAMEWORK_MONIKER, "publish")
TEST_RESULT_FILE = File.join(PROJECT_ROOT_DIR, "TestResult.xml");

desc "Build #{PROJECT_NAME}.WebApi and #{PROJECT_NAME}.WebClient in Release mode"
task :build => [:build_webapi_thunk, :build_webclient_thunk] # Testing is coupled to building of webclient so there is no separate test task for webclient.

desc "Run xUnit tests in #{PROJECT_NAME}.Tests and save results in #{TEST_RESULT_FILE}"
task :test => [:build, :test_webapi_thunk]

desc "Create a Chocolatey package that contains all deployable artifacts"
task :pack, [:target_server_moniker] => [:clean_pack, :test, :pack_thunk]

desc "Deploy a chocolatey package containing the product to a server via SSH and SCP.\r\n" +
     "  * Should be invoked like rake deploy_to[test].\r\n" +
     "  * There must be a corresponding saved putty session like #{PROJECT_NAME.downcase}-test.\r\n" +
     "  * There must be a corresponding appsettings.json file like appsettings.test.json.\r\n" +
     "  * There is a special deployment target called deploy_to[local] which deploys to the local machine."
task :deploy_to, [:target_server_moniker] => [:pack, :deploy_to_thunk]

task :pack_thunk, [:target_server_moniker] => [:pack_webapi_thunk, :pack_webclient_thunk, :create_choco_package_thunk]

task :build_webapi_thunk do
    Dir.chdir(SRC_DIR) do
        sh "dotnet build #{PROJECT_NAME}.sln --configuration #{CONFIGURATION}"
    end
end

task :build_webclient_thunk do
    Dir.chdir(SRC_WEBCLIENT_DIR) do
        sh "npm install && npm run build"
    end
end

task :test_webapi_thunk do
    Dir.chdir(SRC_TESTS_DIR) do
        # This command hooks into the xunit.runner.visualstudio and XunitXml.TestLogger
        # references in TemplateProductName.Tests to run and log test results with xUnit.
        sh "dotnet test --test-adapter-path:. --logger:xunit;LogFilePath=#{TEST_RESULT_FILE}"
    end
end

task :pack_webapi_thunk do |task, args| 
    Dir.chdir(SRC_DIR) do
        sh "dotnet publish #{PROJECT_NAME}.WebApi --configuration #{CONFIGURATION}"
        
        # Copy appsettings.#{args[:target_server_moniker]}.json to appsettings.json and delete other environments' appsettings
        cp File.join(PUBLISH_WEB_DIR, "appsettings.#{args[:target_server_moniker]}.json"), File.join(PUBLISH_WEB_DIR, "appsettings.json")
        Dir.glob(File.join(PUBLISH_WEB_DIR, "appsettings.*.json")).each { |f| File.delete(f) }
    end
end

task :pack_webclient_thunk do
    # Copy #{PROJECT_NAME}.WebClient artefacts to the web-accessible directory in the publish folder for #{PROJECT_NAME}.Web
    cp_r File.join(SRC_WEBCLIENT_DIR, "build", "."), File.join(PUBLISH_WEB_DIR, "wwwroot")
end

task :clean_pack do
    if Dir.exist?(File.join(SRC_WEBCLIENT_DIR, "build"))
        rm_rf File.join(SRC_WEBCLIENT_DIR, "build");
    end
    
    if Dir.exist?(PUBLISH_WEB_DIR)
        rm_rf PUBLISH_WEB_DIR
    end
end

task :create_choco_package_thunk, [:target_server_moniker] do |task, args|
    # Temporarily copy environment-specific IIS config file to a spot where choco can pick it up
    cp File.join(PROJECT_ROOT_DIR, "deploy", "serversettings.#{args[:target_server_moniker]}.json"), File.join(PROJECT_ROOT_DIR, "deploy", "serversettings.json")
    
    # Make the chocolatey package
    Dir.chdir(File.join(PROJECT_ROOT_DIR, "deploy")) do
        sh "choco pack #{PROJECT_NAME.downcase}.nuspec"
    end
    
    # Remove temporary IIS configuration file
    rm File.join(PROJECT_ROOT_DIR, "deploy", "serversettings.json")
end

task :deploy_to_thunk, [:target_server_moniker] do |task, args|
    if(args[:target_server_moniker] == "local")
        # Deploy the chocolatey package to the server or workstation on which this script is executed
        sh "choco install #{PROJECT_NAME} -y -force -source deploy"
        return
    end

    # Copy chocolatey package to server
    # pscp docs: http://the.earth.li/~sgtatham/putty/0.60/htmldoc/Chapter5.html
    # plink docs: http://the.earth.li/~sgtatham/putty/0.60/htmldoc/Chapter7.html
    # setting up saved session in putty: https://www.howtoforge.com/ssh_key_based_logins_putty_p3
    putty_session_name = "#{PROJECT_NAME.downcase}-#{args[:target_server_moniker]}";
    sh "pscp deploy/#{PROJECT_NAME.downcase}.1.0.0.nupkg #{putty_session_name}:D:/packages/#{PROJECT_NAME.downcase}.1.0.0.nupkg"
    sh "plink #{putty_session_name} choco install #{PROJECT_NAME} -y -force -source D:/packages > choco.log"
    
    choco_log = File.read("choco.log")
    puts choco_log
    if !choco_log.include? "The install of #{PROJECT_NAME.downcase} was successful."
        raise "The install was not successful."
    end
end

PROJECT_NAME = "TemplateProductName"
CONFIGURATION = "Release"

PROJECT_ROOT_DIR = File.dirname(__FILE__)
SRC_DIR = File.join(PROJECT_ROOT_DIR, "src")
SRC_WEB_DIR = File.join(SRC_DIR, "#{PROJECT_NAME}.WebApi")
SRC_WEBCLIENT_DIR = File.join(SRC_DIR, "#{PROJECT_NAME}.WebClient")
PUBLISH_WEB_DIR = File.join(SRC_WEB_DIR, "bin", CONFIGURATION , "net462", "publish")

task :build => [:build_webapi_thunk, :build_webclient_thunk] # Testing is coupled to building of webclient so there is no separate test task for webclient.
task :test => [:build, :test_webapi_thunk]
task :pack, [:target_server_moniker] => [:test, :pack_thunk]
task :pack_thunk, [:target_server_moniker] => [:pack_webapi_thunk, :pack_webclient_thunk, :create_choco_package_thunk]
task :deploy_to, [:target_server_moniker] => [:pack, :deploy_to_thunk]

desc "Builds #{PROJECT_NAME}.WebApi.\r\n" +
     "Requires dotnet core 2.0 sdk.\r\n" +
     "Requires nuget 3.5 beta.\r\n" +
     "Requires npm."
task :build_webapi_thunk do
    Dir.chdir(SRC_DIR) do
        # Restore packages for non-core projects
        sh "nuget restore #{PROJECT_NAME}.sln"
        
        # Build all projects so we can run tests
        sh "dotnet build #{PROJECT_NAME}.sln --configuration #{CONFIGURATION}"
    end
end

desc "Compiles #{PROJECT_NAME}.WebClient.\r\n" +
     "Requires npm."
task :build_webclient_thunk do
    Dir.chdir(SRC_WEBCLIENT_DIR) do
        # Restore npm packages
        sh "npm install"
        
        # Publish #{PROJECT_NAME}.WebClient with create-react-app's build task
        sh "npm run build"
    end
end

desc "Runs NUnit tests in #{PROJECT_NAME}.Tests and saves results in TestResult.xml.\r\n" +
     "Requires NUnit."
task :test_webapi_thunk do
    # Run tests in #{PROJECT_NAME}.Tests
    cmd = []
    cmd << "nunit3-console"
    cmd << "\"" + File.join(SRC_DIR, "#{PROJECT_NAME}.Tests", "bin", CONFIGURATION, "#{PROJECT_NAME}.Tests.dll") + "\""
    cmd << "--result:TestResult.xml;format=nunit2"
    sh cmd.join(" ")
end

task :pack_webapi_thunk do |task, args| 
    Dir.chdir(SRC_DIR) do
        # Create publishable artifacts
        sh "dotnet publish #{PROJECT_NAME}.WebApi --configuration #{CONFIGURATION}"
    end
end

task :pack_webclient_thunk do
    # Copy #{PROJECT_NAME}.WebClient artefacts to the web-accessible directory in #{PROJECT_NAME}.Web
    cp_r File.join(SRC_WEBCLIENT_DIR, "build", "."), File.join(PUBLISH_WEB_DIR, "wwwroot")
end

desc "Generates a chocolatey package of all deployable artifacts.\r\n" +
     "Requires chocolatey to be installed and available on the system path."
task :create_choco_package_thunk, [:target_server_moniker] do
    # Temporarily copy production IIS config file to a spot where choco can pick it up
    cp File.join(PROJECT_ROOT_DIR, "deploy", "serversettings.#{args[:target_server_moniker]}.json"), File.join(PROJECT_ROOT_DIR, "deploy", "serversettings.json")
    
    # Make the chocolatey package
    Dir.chdir(File.join(PROJECT_ROOT_DIR, "deploy")) do
        sh "choco pack #{PROJECT_NAME.downcase}.nuspec"
    end
    
    # Remove temporary IIS configuration file
    rm File.join(PROJECT_ROOT_DIR, "deploy", "serversettings.json")
end

desc "Deploys the chocolatey package to a server via SSH and SCP.\r\n" +
     "Should be invoked like rake deploy_to[test].\r\n" +
     "There must be a corresponding saved putty session like #{PROJECT_NAME.downcase}-test.\r\n" +
     "There must be a corresponding appsettings.json file like appsettings.test.json"
task :deploy_to_thunk, [:target_server_moniker] do |task, args|
    # pscp docs: http://the.earth.li/~sgtatham/putty/0.60/htmldoc/Chapter5.html
    # plink docs: http://the.earth.li/~sgtatham/putty/0.60/htmldoc/Chapter7.html
    # setting up saved session in putty: https://www.howtoforge.com/ssh_key_based_logins_putty_p3

    # Copy chocolatey package to server
    putty_session_name = "#{PROJECT_NAME.downcase}-#{args[:target_server_moniker]}";
    sh "pscp deploy/#{PROJECT_NAME.downcase}.1.0.0.nupkg #{putty_session_name}:D:/packages/#{PROJECT_NAME.downcase}.1.0.0.nupkg"
    sh "plink #{putty_session_name} choco install #{PROJECT_NAME} -y -force -source D:/packages > choco.log"
    
    choco_log = File.read("choco.log")
    puts choco_log
    if !choco_log.include? "The install of #{PROJECT_NAME.downcase} was successful."
        raise "The install was not successful."
    end
end

desc "Deploys the chocolatey package to the server or workstation on which this script is executed."
task :deploy_to_local, [:target_server_moniker] => [:pack] do
    sh "choco install #{PROJECT_NAME} -y -force -source deploy"
end
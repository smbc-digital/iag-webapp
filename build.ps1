param (
  [string]$appVersion = "local-build"
)

$appName = "iag-webapp"
$projectPath = "src/StockportWebapp"
$testDir = "test/StockportWebappTests"
$publishDir = "$projectPath/publish"

function _DeleteFile($fileName) {
  If (Test-Path $fileName) {
    Write-Host "Deleting '$fileName'"
    Remove-Item $fileName
  } else {
    "'$fileName' not found. Nothing deleted"
  }
}

function _Clean() {
  echo "Cleaning content"
  _DeleteFile "$appName.zip"
}

function _DotnetRestore() {
  echo "Running dotnet restore"
  dotnet restore
}

function _DotnetTest() {
  echo "Running dotnet test"
  pushd $testDir
  dotnet test --no-build
  popd
}

function _Version() {
  echo "Versioning App"
  # TODO: Removed this line as git isn't available in powershell right now.
  # git rev-parse HEAD | Out-File "src/$projectName/sha.txt"
  echo $appVersion | Out-File "$projectPath/version.txt"
}

function _Publish() {
  echo "Publishing App"
  pushd $projectPath
  dotnet publish --configuration Release -o publish
  popd
}

function Main {
  Try {
    # clean dotnet-restore dotnet-test version publish-app package-app
    _Clean
    _DotnetRestore
    _DotnetTest
    _Version
    _Publish
  }
  Catch {
    Write-Error $_.Exception
  }
}

Main

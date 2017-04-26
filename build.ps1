function _Build($appName, $projectPath) {
  _DeleteFile "$appName.zip"
  cd $projectPath
  dotnet restore
  cd ..\..
}

function _DeleteFile($fileName) {
    If (Test-Path $fileName) {
        Write-Host "Deleting '$fileName'"
  	   Remove-Item $fileName
    } else {
      "'$fileName' not found. Nothing deleted"
    }
}

function _Publish($projectPath) {
  cd $projectPath
  dotnet publish --configuration Release -o publish
  cd ..\..
}

function _Package($name) {
  python zip.py src\StockportContentApi\publish "$name.zip"
}

function Main {
  Try {
    $appName = "webapp"
    $projectPath = "src\StockportWebapp"

    _Build $appName $projectPath
    _Publish $projectPath
  }
  Catch {
    Write-Error $_.Exception
  }
}

Main

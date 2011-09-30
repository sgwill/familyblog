properties {
  $base_dir = Resolve-Path ..
  $build_dir = "$base_dir\build"
  $test_library_dir = "$base_dir\Tests\"
  $sln = "$base_dir\WilliamsonFamily.sln"
  
  $build_debug_dir = "$build_dir\Debug"
  $test_debug_dir = "$build_debug_dir\Tests\"
  
  $build_release_dir = "$build_dir\Release" 
  $test_release_dir = "$build_release_dir\Tests\"
  
  $publish_dir = "_PublishedWebsites\$project_path\\"  
  $ftp_user = "Sadajorew"
  $ftp_password = "0417sad"
  $ftp_server = "ftp://ftp2.christianasp.net/williamsonfamily.com/"
}

$framework = '4.0'

task default -depends Instructions

task Instructions -Description "Build script instructions" {
    ""
    "This is the williamsonfamily build file"
    ""
    psake -doc
}

task Clean-Debug -Description "Clean debug build output folder" { 
    msbuild $sln "/nologo" "/t:Clean" "/p:Configuration=Debug" "/fileLogger"
    remove-item -force -recurse "$build_debug_dir\\" -ErrorAction SilentlyContinue
}

task Clean-Release -Description "Clean release build output folder" { 
    msbuild $sln "/nologo" "/t:Clean" "/p:Configuration=Release" "/fileLogger"
    remove-item -force -recurse "$build_release_dir\" -ErrorAction SilentlyContinue
}

task Clean -depends Clean-Debug,Clean-Release -Description "Clean build output folder" { 
}

task Init -depends Clean -Description "Ensure folders exist" {
    new-item $build_debug_dir -itemType directory
    new-item $build_release_dir -itemType directory
}

task Checkout-Develop -Description "Checkout develop branch" {
    git checkout develop
}

task Checkout-Release -Description "Checkout master branch" {
    git checkout master
}

task Build-Debug -Description "Build in DEBUG mode" {
    msbuild $sln "/nologo" "/t:Rebuild" "/p:Configuration=Debug" "/fileLogger"
}

task Build-Release -Description "Build in RELEASE mode" {
    msbuild $sln "/nologo" "/t:Rebuild" "/p:Configuration=Release" "/fileLogger" 
}

task Test -depends Checkout-Develop,Clean-Debug,Build-Debug -Description "Runs tests in DEBUG mode" {
	# Build
    #msbuild $sln "/nologo" "/t:Rebuild" "/p:Configuration=Debug" "/fileLogger"
    
    # Ensure Test Folder
    $pathExists = test-path $test_debug_dir
    if (-not $pathExists) {
        mkdir $test_debug_dir
    }
    
    # Copy test bins to output folder
    ls $test_library_dir -recurse | where {$_.psIsContainer -eq $true -and $_.Name -eq 'bin' } | foreach { ls $_.fullname } | where { $_.psIsContainer -and $_.Name -eq 'Debug' } | foreach { ls $_.fullname *.dll} | foreach { cp $_.fullname $test_debug_dir }     
    
    # Test
	$tests = ls $test_debug_dir *.Tests.dll | foreach { '/testcontainer:' + $test_debug_dir + $_ }
	mstest $tests
}

task Test-release -depends Clean-Release,Build-Release -Description "Runs tests in RELEASE mode" {
	# Build
    #msbuild $sln "/nologo" "/t:Rebuild" "/p:Configuration=Release" "/fileLogger"
    
    # Ensure Test Folder
    $pathExists = test-path $test_release_dir
    if (-not $pathExists) {
        mkdir $test_release_dir
    }
    
    # Copy test bins to output folder
    ls $test_library_dir -recurse | where {$_.psIsContainer -eq $true -and $_.Name -eq 'bin' } | foreach { ls $_.fullname } | where { $_.psIsContainer -and $_.Name -eq 'Release' } | foreach { ls $_.fullname *.dll} | foreach { cp $_.fullname $test_release_dir }     
    
    # Test
	$tests = ls $test_release_dir *.Tests.dll | foreach { '/testcontainer:' + $test_release_dir + $_ }
	mstest $tests
}

task list-ftp -Description "List the files of the home directory" {
    List-FtpDirectory $ftp_server $ftp_user $ftp_password
}

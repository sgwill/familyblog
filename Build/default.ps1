properties {
  $base_dir = Resolve-Path ..
  $build_dir = "$base_dir\build"
  $tests_dir = "$base_dir\Tests\"
  $sln = "$base_dir\WilliamsonFamily.sln"
  $debug_dir = "$build_dir\Debug"
  $test_dir = "$debug_dir\Tests\"
  $release_dir = "$build_dir\Release\\"
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
    remove-item -force -recurse "$debug_dir\\" -ErrorAction SilentlyContinue
}

task Clean-Release -Description "Clean release build output folder" { 
    msbuild $sln "/nologo" "/t:Clean" "/p:Configuration=Release" "/p:OutDir=""$release_dir""" "/fileLogger"
    remove-item -force -recurse "$release_dir\" -ErrorAction SilentlyContinue
}

task Clean -depends Clean-Debug,Clean-Release -Description "Clean build output folder" { 
}

task Init -depends Clean -Description "Ensure folders exist" {
    new-item $debug_dir -itemType directory
    new-item $release_dir -itemType directory
}

task Build-Debug -Description "Build in DEBUG mode" {
    msbuild $sln "/nologo" "/t:Rebuild" "/p:Configuration=Debug" "/p:OutDir=""$debug_dir""" "/fileLogger"
}

task Build-Release -Description "Build in RELEASE mode" {
    msbuild $sln "/nologo" "/t:Rebuild" "/p:Configuration=Release" "/p:OutDir=""$release_dir""" "/fileLogger" "/noconsolelogger"
}

task Test -depends Clean-Debug -Description "Runs tests" {
	# Build
    msbuild $sln "/nologo" "/t:Rebuild" "/p:Configuration=Debug" "/fileLogger"
    
    # Clean Test folder
    if (test-path $test_dir) {
        remove-item -force -recurse "$test_dir\*.*" -ErrorAction SilentlyContinue
    } else {
        mkdir $test_dir
    }
    #
    
    # Copy test bins to output folder
    ls $tests_dir -recurse | where {$_.psIsContainer -eq $true -and $_.Name -eq 'bin' } | foreach { ls $_.fullname } | where { $_.psIsContainer -and $_.Name -eq 'Debug' } | foreach { ls $_.fullname *.dll} | foreach { cp $_.fullname $test_dir }     
    
    # Test
	$tests = ls $test_dir *.Tests.dll | foreach { '/testcontainer:' + $test_dir + $_ }
	mstest $tests
}

task list-ftp -Description "List the files of the home directory" {
    List-FtpDirectory $ftp_server $ftp_user $ftp_password
}

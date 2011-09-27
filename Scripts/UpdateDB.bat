
cd ..\ThirdPartyAssemblies\SQLite

REM Uncomment this line to regenerate dbml
REM .\DbMetal.exe /namespace:WilliamsonFamily.Models.Data /provider:SQLite "/conn:Data Source=..\..\Webs\Williamsonfamily\App_Data\Provider.db" /dbml:..\..\Libraries\WilliamsonFamily.Models.Data\WilliamsonFamily\WilliamsonFamily.dbml
.\DbMetal.exe /namespace:WilliamsonFamily.Models.Data /provider:SQLite "/conn:Data Source=..\..\Webs\Williamsonfamily\App_Data\Provider.db" /dbml ..\..\Libraries\WilliamsonFamily.Models.Data\WilliamsonFamily\WilliamsonFamily.dbml /code:..\..\Libraries\WilliamsonFamily.Models.Data\WilliamsonFamily\WilliamsonFamily.dbml.cs 
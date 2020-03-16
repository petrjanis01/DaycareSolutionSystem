@echo off
java -jar .\openapi-generator-cli-3.jar generate -i swagger.yaml -l typescript-angular -o .\..\Angular\DaycareSolutionSystem.MobileApp\src\app\api\generated -t .\Ionic_Templates\ --additional-properties ngVersion=7.0.0 --additional-properties modelPropertyNaming=original
java -jar .\openapi-generator-cli-3.jar generate -i swagger.yaml -l typescript-angular -o .\..\Angular\DaycareSolutionSystem.WebApp\src\app\api\generated -t .\Angular_Templates\ --additional-properties ngVersion=7.0.0 --additional-properties modelPropertyNaming=original
pause
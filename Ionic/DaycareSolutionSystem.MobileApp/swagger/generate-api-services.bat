@echo off
java -jar .\openapi-generator-cli-3.jar generate -i swagger.yaml -l typescript-angular -o .\..\src\app\api\generated -t . --additional-properties ngVersion=7.0.0 --additional-properties modelPropertyNaming=original
pause
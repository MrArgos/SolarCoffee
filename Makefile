# Project variables
PROJECT_NAME ?= SolarCoffee
ORG_NAME ?= SolarCoffee
REPO_NAME ?= SolarCoffee

.PHONY: migrations db

migrations:
		cd ./SolarCoffee.Data && dotnet ef --startup-project ../SolarCoffee.Web/ migrations add $(name) &&cd ..

db:
		cd ./SolarCoffee.Web/ && dotnet ef database update && cd ..
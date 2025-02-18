# FROM --platform=linux/amd64 public.ecr.aws/lambda/dotnet:9 AS base
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:9.0-alpine3.21 AS base
#public.ecr.aws/lambda/dotnet:8

WORKDIR /var/task
COPY "bin/Release/lambda-publish"  .

CMD ["dotnet","$ProjectName$.dll"]
ENTRYPOINT ["./$ProjectName$"]
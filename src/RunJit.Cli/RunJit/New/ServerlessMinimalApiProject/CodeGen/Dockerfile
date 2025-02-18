# Use the official AWS Lambda .NET 9 runtime base image
FROM public.ecr.aws/lambda/dotnet:9 AS base
WORKDIR /var/task

# Copy your published application
COPY ./publish .

# Set the Lambda function entry point. The format is:
# "Assembly::Namespace.ClassName::MethodName"
# By default, APIGatewayProxyFunction provides "FunctionHandlerAsync".
ENTRYPOINT ["$ProjectName$::$ProjectName$.Aws.LambdaEntryPoint::FunctionHandlerAsync"]
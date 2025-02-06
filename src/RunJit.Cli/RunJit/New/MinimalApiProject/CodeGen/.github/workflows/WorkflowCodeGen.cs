﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.github.workflows
{
    internal static class AddWorkflowCodeGenExtension
    {
        internal static void AddWorkflowCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IGitHubCodeGen, WorkflowCodeGen>();
        }
    }

    internal sealed class WorkflowCodeGen(ConsoleService consoleService) : IGitHubCodeGen
    {
        private const string Template = """
                                        name: Deployment Pipeline
                                        
                                        on:
                                          push:
                                            branches: [ "main" ]
                                          pull_request:
                                            branches: [ "main" ]
                                        
                                        jobs:
                                          commitlint:
                                            runs-on: ubuntu-latest
                                            steps:
                                              - name: Checkout repository
                                                uses: actions/checkout@v4
                                                with:
                                                  fetch-depth: 0
                                        
                                              - name: Set up Node.js
                                                uses: actions/setup-node@v4
                                                with:
                                                  node-version: 18
                                        
                                              - name: Install commitlint
                                                run: |
                                                  npm install --save-dev @commitlint/config-conventional @commitlint/cli
                                              - name: Create commitlint config
                                                run: |
                                                  echo "module.exports = { extends: ['@commitlint/config-conventional'] };" > commitlint.config.js
                                              - name: Lint commits
                                                run: |
                                                  npx commitlint --from=$(git merge-base origin/main HEAD~10) --to=HEAD
                                          setup:
                                            runs-on: ubuntu-latest
                                            needs: commitlint
                                            outputs:
                                              workspace: ${{ steps.checkout.outputs.workspace }}
                                            steps:
                                              - name: Checkout code
                                                id: checkout
                                                uses: actions/checkout@v4
                                                with:
                                                  fetch-depth: 0
                                        
                                              - name: Upload Source
                                                uses: actions/upload-artifact@v3
                                                with:
                                                  name: source
                                                  path: ./
                                        
                                              - name: Setup .NET SDK
                                                uses: actions/setup-dotnet@v4
                                        
                                          build:
                                            runs-on: ubuntu-latest
                                            needs: setup
                                            steps:
                                              - name: Setup .NET SDK
                                                uses: actions/setup-dotnet@v4
                                                with:
                                                  dotnet-version: '9.0.100'
                                        
                                              - name: Download Source
                                                uses: actions/download-artifact@v3
                                                with:
                                                  name: source
                                        
                                              - name: Dotnet Restore
                                                run: dotnet restore
                                        
                                              - name: Build the project
                                                run: dotnet build --configuration Release --no-restore --verbosity minimal
                                        
                                              - name: Upload Source
                                                uses: actions/upload-artifact@v3
                                                with:
                                                  name: source
                                                  path: ./
                                        
                                          tests:
                                            runs-on: ubuntu-latest
                                            needs: build
                                            steps:
                                              - name: Setup .NET SDK
                                                uses: actions/setup-dotnet@v4
                                                with:
                                                  dotnet-version: '9.0.100'
                                        
                                              - name: Download Source
                                                uses: actions/download-artifact@v3
                                                with:
                                                  name: source
                                        
                                        #      - name: Run Code-Rules tests
                                        #        run: dotnet test --configuration Release --filter Coding-Rules --no-restore --no-build --verbosity minimal
                                        #
                                        #      - name: Run all other tests
                                        #        run: dotnet test --configuration Release --no-restore --no-build --verbosity minimal
                                                  
                                          approval:
                                            runs-on: ubuntu-latest
                                            needs: tests
                                            environment: prod
                                            if: github.event_name == 'push'
                                            steps:
                                              - name: Show approval comment
                                                run: echo "Please approve the deployment on the 'Environments' page."
                                                
                                          deploy:
                                            runs-on: ubuntu-latest
                                            needs: approval
                                            if: github.event_name == 'push'
                                            env:
                                              REPOSITORY_URI: ${{ secrets.AWS_ACCOUNT_ID }}.dkr.ecr.${{ secrets.AWS_REGION }}.amazonaws.com/${{ secrets.ECR_REPOSITORY }}
                                            steps:
                                              - name: Checkout Code
                                                uses: actions/checkout@v4
                                              
                                              - name: Setup dotnet
                                                uses: actions/setup-dotnet@v4
                                        
                                              - name: setup python 
                                                uses: actions/setup-python@v5
                                                with:
                                                  python-version: '3.11'
                                        
                                              - uses: actions/aws-actions_configure-aws-credentials@v4
                                                with:
                                                  aws-access-key-id: ${{ secrets.AWS_ACCESS_KEY_ID }}
                                                  aws-secret-access-key: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
                                                  aws-region: ${{ secrets.AWS_REGION }}
                                        
                                              - name: get caller identity 1
                                                run: |
                                                  pip install awscli
                                        
                                              - name: Login to Amazon ECR
                                                run: |
                                                  aws ecr get-login-password --region ${{ secrets.AWS_REGION }} | docker login --username AWS --password-stdin ${{ secrets.AWS_ACCOUNT_ID }}.dkr.ecr.${{ secrets.AWS_REGION }}.amazonaws.com
                                              
                                              - name: Build Docker Image
                                                run: |
                                                  dotnet publish -c Release -o bin/Release/lambda-publish
                                                  docker build --build-arg AWS_ACCESS_KEY_ID=${{ secrets.AWS_ACCESS_KEY_ID }} --build-arg AWS_SECRET_ACCESS_KEY=${{ secrets.AWS_SECRET_ACCESS_KEY }} -t ${{ env.REPOSITORY_URI }}:latest .
                                              
                                              - name: Push Docker Image to ECR
                                                run: |
                                                  docker tag ${{ env.REPOSITORY_URI }}:latest ${{ env.REPOSITORY_URI }}:${{ github.sha }}
                                                  docker push ${{ env.REPOSITORY_URI }}:latest
                                                  docker push ${{ env.REPOSITORY_URI }}:${{ github.sha }}
                                        """;


        public async Task GenerateAsync(SolutionFile projectFileInfo,
                                        DirectoryInfo gitHubFolder,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1. .github folder
            var workflowFolder = new DirectoryInfo(Path.Combine(gitHubFolder.FullName, "workflows"));
            if (workflowFolder.NotExists())
            {
                workflowFolder.Create();
            }

            // 2. Write all templates
            var file = Path.Combine(workflowFolder.FullName, "deployment.yml");

            var newTemplate = Template.Replace("$namespace$", minimalApiProjectInfos.ProjectName)
                                      .Replace("$dotNetToolName$", minimalApiProjectInfos.NormalizedName);


            await File.WriteAllTextAsync(file, newTemplate).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}

﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal static class AddDotSettingsCodeGenExtension
    {
        internal static void AddDotSettingsCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, DotSettingsCodeGen>();
        }
    }

    internal sealed class DotSettingsCodeGen(ConsoleService consoleService) : IMinimalApiProjectRootLevelCodeGen
    {
        private const string Template = """
                                        <wpf:ResourceDictionary xml:space="preserve" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" xmlns:s="clr-namespace:System;assembly=mscorlib" xmlns:ss="urn:shemas-jetbrains-com:settings-storage-xaml" xmlns:wpf="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
                                            <!-- Code formatting -->
                                        	<s:Boolean x:Key="/Default/CodeEditing/Intellisense/CodeCompletion/IntelliSenseCompletingCharacters/CSharpCompletingCharacters/UpgradedFromVSSettings/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_FIRST_ARG_BY_PAREN/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_LINQ_QUERY/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTILINE_ARRAY_AND_OBJECT_INITIALIZER/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTILINE_BINARY_PATTERNS/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTILINE_CALLS_CHAIN/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTILINE_EXPRESSION/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTILINE_EXTENDS_LIST/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTILINE_LIST_PATTERN/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTILINE_PARAMETER/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTILINE_PROPERTY_PATTERN/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTIPLE_DECLARATION/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTLINE_TYPE_PARAMETER_CONSTRAINS/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_MULTLINE_TYPE_PARAMETER_LIST/@EntryValue">True</s:Boolean>
                                        	<s:String x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGNMENT_TAB_FILL_STYLE/@EntryValue">OPTIMAL_FILL</s:String>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/ALIGN_TUPLE_COMPONENTS/@EntryValue">True</s:Boolean>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_AFTER_CONTROL_TRANSFER_STATEMENTS/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_AFTER_MULTILINE_STATEMENTS/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_AFTER_USING_LIST/@EntryValue">0</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_AROUND_ACCESSOR/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_AROUND_SINGLE_LINE_ACCESSOR/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_AROUND_SINGLE_LINE_AUTO_PROPERTY/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_AROUND_SINGLE_LINE_FIELD/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_AROUND_SINGLE_LINE_INVOCABLE/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_AROUND_SINGLE_LINE_LOCAL_METHOD/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_AROUND_SINGLE_LINE_PROPERTY/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_BEFORE_BLOCK_STATEMENTS/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_BEFORE_CONTROL_TRANSFER_STATEMENTS/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_BEFORE_MULTILINE_STATEMENTS/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/BLANK_LINES_BEFORE_SINGLE_LINE_COMMENT/@EntryValue">1</s:Int64>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/INDENT_ANONYMOUS_METHOD_BLOCK/@EntryValue">True</s:Boolean>
                                        	<s:String x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/INDENT_PRIMARY_CONSTRUCTOR_DECL_PARS/@EntryValue">INSIDE</s:String>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/KEEP_BLANK_LINES_IN_CODE/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/KEEP_BLANK_LINES_IN_DECLARATIONS/@EntryValue">1</s:Int64>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/KEEP_EXISTING_INVOCATION_PARENS_ARRANGEMENT/@EntryValue">False</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/KEEP_EXISTING_PRIMARY_CONSTRUCTOR_DECLARATION_PARENS_ARRANGEMENT/@EntryValue">False</s:Boolean>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/MAX_ARRAY_INITIALIZER_ELEMENTS_ON_LINE/@EntryValue">3</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/MAX_FORMAL_PARAMETERS_ON_LINE/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/MAX_INITIALIZER_ELEMENTS_ON_LINE/@EntryValue">1</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/MAX_INVOCATION_ARGUMENTS_ON_LINE/@EntryValue">3</s:Int64>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/MAX_PRIMARY_CONSTRUCTOR_PARAMETERS_ON_LINE/@EntryValue">1</s:Int64>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/OUTDENT_BINARY_PATTERN_OPS/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/OUTDENT_COMMAS/@EntryValue">True</s:Boolean>
                                        	<s:String x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/PLACE_ACCESSOR_ATTRIBUTE_ON_SAME_LINE_EX/@EntryValue">NEVER</s:String>
                                        	<s:String x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/PLACE_ACCESSORHOLDER_ATTRIBUTE_ON_SAME_LINE_EX/@EntryValue">NEVER</s:String>
                                        	<s:String x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/PLACE_FIELD_ATTRIBUTE_ON_SAME_LINE_EX/@EntryValue">NEVER</s:String>
                                        	<s:String x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/PLACE_SIMPLE_EMBEDDED_STATEMENT_ON_SAME_LINE/@EntryValue">NEVER</s:String>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/PLACE_SIMPLE_INITIALIZER_ON_SINGLE_LINE/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/STICK_COMMENT/@EntryValue">False</s:Boolean>
                                        	<s:Boolean x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/WRAP_AFTER_PRIMARY_CONSTRUCTOR_DECLARATION_LPAR/@EntryValue">False</s:Boolean>
                                        	<s:Int64 x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/WRAP_LIMIT/@EntryValue">720</s:Int64>
                                        	<s:String x:Key="/Default/CodeStyle/CodeFormatting/CSharpFormat/WRAP_OBJECT_AND_COLLECTION_INITIALIZER_STYLE/@EntryValue">CHOP_IF_LONG</s:String>
                                        	<s:String x:Key="/Default/CodeStyle/Generate/=Implementations/Options/=Mutable/@EntryIndexedValue">False</s:String>
                                        	<s:Boolean x:Key="/Default/CodeStyle/Generate/=PatternMatching/@KeyIndexDefined">True</s:Boolean>
                                        	<s:String x:Key="/Default/CodeStyle/Generate/=PatternMatching/Options/=GenerateVariable/@EntryIndexedValue">True</s:String>
                                        	<s:Boolean x:Key="/Default/CodeStyle/Naming/CSharpAutoNaming/IsNotificationDisabled/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Dpa/IsIssuesNotificationEnabled/@EntryValue">False</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Dpa/IsNoEtwHostNotificationEnabled/@EntryValue">False</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Environment/AIAssistantOptions/IsUserNotifiedAboutSmartChat/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Environment/ExternalSources/FirstTimeFormShown/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Environment/Feedback/SendActivityLogs/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Environment/Feedback/ShouldPrompt/@EntryValue">False</s:Boolean>	
                                        	<s:Boolean x:Key="/Default/Environment/JetBrainsAIOptions/HasEverLoggedIn/@EntryValue">True</s:Boolean>
                                        	<s:String x:Key="/Default/Environment/JetBrainsAIOptions/IdToken/@EntryValue">eyJhbGciOiJSUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJHcmF6aWUgQXV0aGVudGljYXRpb24iLCJ1aWQiOiJiMWQyZjc0OC1lZjRiLTQ2NWUtYjQwMC02NDAzN2Q4Y2ExY2QiLCJ1c2VyX3N0YXRlIjoiUEFJRCIsInJlZ2lzdHJhdGlvbl9kYXRlIjoxNzAxOTYyNTMxNzcyLCJsaWNlbnNlIjoiSzA0WEw3U0RMVyIsImxpY2Vuc2VfdHlwZSI6ImpldGJyYWlucy1haS5vcmdhbml6YXRpb25hbC5wcm8iLCJleHAiOjE3MjA1ODY0Nzh9.FH-M2sYGLv264L4BnAl3NT3jLNcxSV4RhiuqKBE6nhynlDv07RYoJYaH5UYcBa25M4pylcRotK-eZYx4DN2GVLv4Vk5BNmLWSN9P-baMcOKEeQ3uLFZ2Ez7iQ4vL-eKk92f97zN86SgBSkXoEr3zOVrpqXuyhdqWorHhMh8Kib9kjVR0cBRoDJwjqg8wh5yUvB8fIVmTNQqnvjSPVLB3junOL0pn_QjWs75ESAvWaIjpEReTzFGHG7a5losvUIHPkYFkUn9RkfShcAwCUg0DWSY9wdApjlyUAE8aa46b8gwsvF1KATfer5646LsTp7KPUaP6zxyZRC7fkuLV6QCyHA</s:String>
                                        	<s:String x:Key="/Default/Environment/JetBrainsAIOptions/LicenseId/@EntryValue">K04XL7SDLW</s:String>
                                        	<s:String x:Key="/Default/Environment/JetBrainsAIOptions/QuotaCurrent/@EntryValue">2424.95</s:String>
                                        	<s:String x:Key="/Default/Environment/JetBrainsAIOptions/QuotaMaximum/@EntryValue">5000000</s:String>
                                        	<s:String x:Key="/Default/Environment/JetBrainsAIOptions/QuotaRefillAt/@EntryValue">07/12/2024 10:00:08</s:String>
                                        	<s:String x:Key="/Default/Environment/JetBrainsAIOptions/QuotaUntil/@EntryValue">12/08/2024 21:00:00</s:String>
                                        	<s:Int64 x:Key="/Default/Environment/SearchAndNavigation/DefaultOccurrencesGroupingIndices/=JetBrains_002EPsiFeatures_002EUIInteractive_002ERefactorings_002EObsoletePages_002EBulkRename_002EBulkRenameChangeDescriptor/@EntryIndexedValue">12</s:Int64>
                                        	<s:Boolean x:Key="/Default/Environment/SettingsMigration/IsMigratorApplied/=JetBrains_002EReSharper_002EPsi_002ECSharp_002ECodeStyle_002ECSharpKeepExistingMigration/@EntryIndexedValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Environment/SettingsMigration/IsMigratorApplied/=JetBrains_002EReSharper_002EPsi_002ECSharp_002ECodeStyle_002ECSharpPlaceEmbeddedOnSameLineMigration/@EntryIndexedValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Environment/SettingsMigration/IsMigratorApplied/=JetBrains_002EReSharper_002EPsi_002ECSharp_002ECodeStyle_002ECSharpUseContinuousIndentInsideBracesMigration/@EntryIndexedValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Environment/SettingsMigration/IsMigratorApplied/=JetBrains_002EReSharper_002EPsi_002ECSharp_002ECodeStyle_002ESettingsUpgrade_002EMigrateBlankLinesAroundFieldToBlankLinesAroundProperty/@EntryIndexedValue">True</s:Boolean>
                                        	<s:String x:Key="/Default/Environment/UpdatesManger/LastUpdateCheck/@EntryValue">07/07/2024 15:10:53</s:String>
                                        	<s:String x:Key="/Default/Environment/UpdatesManger/LastUpdateCheckPerHost/=dotPeek/@EntryIndexedValue">03/14/2024 12:35:52</s:String>
                                        	<s:String x:Key="/Default/Environment/UpdatesManger/LastUpdateCheckPerHost/=ReSharperPlatformVs17/@EntryIndexedValue">07/07/2024 13:15:51</s:String>
                                        	<s:String x:Key="/Default/Environment/UserInterface/ShortcutSchemeName/@EntryValue">VS</s:String>
                                        	<s:String x:Key="/Default/Housekeeping/AIFeatureSuggester/LastAttemptToShowFeatureSuggesterTime/@EntryValue">06/26/2024 18:56:18</s:String>
                                        	<s:String x:Key="/Default/Housekeeping/DiskCleanup/LastRunTime/@EntryValue">07/07/2024 15:14:58</s:String>
                                        	<s:Boolean x:Key="/Default/Housekeeping/FeatureSuggestion/FeatureSuggestionManager/DisabledSuggesters/=AutoNamingFeatureSuggester/@EntryIndexedValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Housekeeping/FeatureSuggestion/FeatureSuggestionManager/DisabledSuggesters/=NullabilityAnnotationAssistSuggester/@EntryIndexedValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Housekeeping/FeatureSuggestion/FeatureSuggestionManager/DisabledSuggesters/=SwitchToGoToActionSuggester/@EntryIndexedValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Housekeeping/FeatureSuggestion/FeatureSuggestionManager/DisabledSuggesters/=TabNavigationExplainer/@EntryIndexedValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Housekeeping/GlobalSettingsUpgraded/IsUpgraded/@EntryValue">True</s:Boolean>
                                        	<s:String x:Key="/Default/Housekeeping/Layout/DialogWindows/OptionsDialogView/Position/@EntryValue">[-57.3333333333333,17.3333333333333](1551.33333333333,759.333333333333)</s:String>
                                        	<s:String x:Key="/Default/Housekeeping/Layout/DialogWindows/OwnedDialogPosition/Position/=AdvancedSearchDialog/@EntryIndexedValue">[0,66.6666666666667](500,400)</s:String>
                                        	<s:String x:Key="/Default/Housekeeping/Layout/DialogWindows/RefactoringWizardWindow/Location/@EntryValue">-183.333333333333,-323.333333333333</s:String>
                                        	<s:String x:Key="/Default/Housekeeping/OptionsDialog/SelectedPageId/@EntryValue">CSharpBlankLinesPage</s:String>
                                        	<s:Double x:Key="/Default/Housekeeping/TreeModelBrowserPanelPersistence/PreviewSplitterVerticalProportion/=UnitTestSessionDescriptor/@EntryIndexedValue">1.7533718689788049</s:Double>
                                        	<s:Boolean x:Key="/Default/Housekeeping/UpgradeFromExceptionReport/DefaultReporterBehaviorChanged/@EntryValue">True</s:Boolean>
                                        	<s:Boolean x:Key="/Default/Housekeeping/UpgradeFromExceptionReport/UpgradePerformed/@EntryValue">True</s:Boolean>
                                        	<s:String x:Key="/Default/Housekeeping/WhatsNewShown/WhatsNewShownIndexedEntry/=RS0/@EntryIndexedValue">2024.1</s:String>
                                        
                                            <!-- Patterns & Templates -->
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Shortcut/@EntryValue">request-validator</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Text/@EntryValue">public static class Add$validator$Extension&#xD;
                                        {&#xD;
                                            public static void Add$validator$(this IServiceCollection services)&#xD;
                                            {&#xD;
                                                services.AddSingletonIfNotExists&lt;IRequestValidator&lt;$request$&gt;, $validator$&gt;();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        internal class $validator$ : IRequestValidator&lt;$request$&gt;&#xD;
                                        {&#xD;
                                            public Task ValidateAsync($request$ request)&#xD;
                                            {&#xD;
                                                throw new System.NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Field/=request/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Field/=request/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Field/=validator/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=440A76404BCB9C41A73564348CE78AEF/Field/=validator/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Shortcut/@EntryValue">#if</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Text/@EntryValue">#if $expression$&#xD;
                                             $SELECTION$$END$ &#xD;
                                          #endif</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Scope/=74A278E9BF386142B53D57114609A033/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Scope/=74A278E9BF386142B53D57114609A033/Type/@EntryValue">InCSharpExceptStringLiterals</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Scope/=74A278E9BF386142B53D57114609A033/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Scope/=4B847A5ED8F7574F925C66209222DF71/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Scope/=4B847A5ED8F7574F925C66209222DF71/Type/@EntryValue">AtLineStart</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Field/=expression/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Field/=expression/Expression/@EntryValue">constant("true")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B20F39F722283242A1DCC3A37FDE093B/Field/=expression/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Shortcut/@EntryValue">#region</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Text/@EntryValue">#region $name$&#xD;
                                             $SELECTION$$END$&#xD;
                                          #endregion</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Scope/=74A278E9BF386142B53D57114609A033/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Scope/=74A278E9BF386142B53D57114609A033/Type/@EntryValue">InCSharpExceptStringLiterals</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Scope/=74A278E9BF386142B53D57114609A033/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Scope/=4B847A5ED8F7574F925C66209222DF71/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Scope/=4B847A5ED8F7574F925C66209222DF71/Type/@EntryValue">AtLineStart</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Field/=name/Expression/@EntryValue">constant("MyRegion")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=114CAE40334756458DF914559FA65BBF/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/Text/@EntryValue">[NUnit.Framework.Test]&#xD;
                                        public Task Should_Not_Be_Able_To_Get_$entity$_By_Id_Without_Authentication()&#xD;
                                        {&#xD;
                                        	return Client.AssertGetAsUnauthorizedAsync("$url$");&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/Field/=entity/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/Field/=entity/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/Field/=url/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8CD67D12BAA48A4D859329BE238D2425/Field/=url/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C8C0B3D1BF8B44D9A11DBBB1D51540A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C8C0B3D1BF8B44D9A11DBBB1D51540A/Text/@EntryValue">[Route("$route$")]</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C8C0B3D1BF8B44D9A11DBBB1D51540A/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C8C0B3D1BF8B44D9A11DBBB1D51540A/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C8C0B3D1BF8B44D9A11DBBB1D51540A/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C8C0B3D1BF8B44D9A11DBBB1D51540A/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C8C0B3D1BF8B44D9A11DBBB1D51540A/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C8C0B3D1BF8B44D9A11DBBB1D51540A/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C8C0B3D1BF8B44D9A11DBBB1D51540A/Field/=route/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C8C0B3D1BF8B44D9A11DBBB1D51540A/Field/=route/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A9CD96273781A7498704638741B82AB4/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A9CD96273781A7498704638741B82AB4/Text/@EntryValue">internal static class $request$ValidatorExtension&#xD;
                                        {&#xD;
                                            public static void $request$Validator(this IServiceCollection services)&#xD;
                                            {&#xD;
                                                services.AddSingletonIfNotExists&lt;IRequestValidator&lt;$request$&gt;, $request$Validator&gt;();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        internal class $request$Validator : IRequestValidator&lt;$request$&gt;&#xD;
                                        {&#xD;
                                            public Task ValidateAsync(AddOpportunity request)&#xD;
                                            {&#xD;
                                                return Task.CompletedTask;       &#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A9CD96273781A7498704638741B82AB4/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A9CD96273781A7498704638741B82AB4/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A9CD96273781A7498704638741B82AB4/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A9CD96273781A7498704638741B82AB4/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A9CD96273781A7498704638741B82AB4/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A9CD96273781A7498704638741B82AB4/Field/=request/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A9CD96273781A7498704638741B82AB4/Field/=request/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C6F9E140AE36FA4A9EBA091DC70ACEA1/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C6F9E140AE36FA4A9EBA091DC70ACEA1/Text/@EntryValue">[Route("$route$")]</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C6F9E140AE36FA4A9EBA091DC70ACEA1/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C6F9E140AE36FA4A9EBA091DC70ACEA1/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C6F9E140AE36FA4A9EBA091DC70ACEA1/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C6F9E140AE36FA4A9EBA091DC70ACEA1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C6F9E140AE36FA4A9EBA091DC70ACEA1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C6F9E140AE36FA4A9EBA091DC70ACEA1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C6F9E140AE36FA4A9EBA091DC70ACEA1/Field/=route/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C6F9E140AE36FA4A9EBA091DC70ACEA1/Field/=route/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7947D2F3CC4542BF8EC7DDEB5CF70F/@KeyIndexDefined">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7947D2F3CC4542BF8EC7DDEB5CF70F/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7947D2F3CC4542BF8EC7DDEB5CF70F/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7947D2F3CC4542BF8EC7DDEB5CF70F/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7947D2F3CC4542BF8EC7DDEB5CF70F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7947D2F3CC4542BF8EC7DDEB5CF70F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7947D2F3CC4542BF8EC7DDEB5CF70F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/Text/@EntryValue">[TestClass]&#xD;
                                        [TestCategory("$category$")]&#xD;
                                        public class $testName$&#xD;
                                        {&#xD;
                                        	&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/Field/=category/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/Field/=category/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/Field/=testName/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=458D185AC455504D83D145782B5F6038/Field/=testName/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/Shortcut/@EntryValue">addconfigurationextension</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/Text/@EntryValue">public static class Add$name$Extension&#xD;
                                        {&#xD;
                                            public static void Add$name$(this IServiceCollection services, IConfiguration configuration)&#xD;
                                            {&#xD;
                                        		services.AddSingletonOption&lt;$name$&gt;(configuration);&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFF6886F70E2C345A7636A9CABE01DD1/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/Shortcut/@EntryValue">addserviceextension</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/Text/@EntryValue">public static class Add$Name$Extension&#xD;
                                        {&#xD;
                                            public static void Add$Name$(this IServiceCollection services)&#xD;
                                            {&#xD;
                                        		services.AddSingletonIfNotExists&lt;I$Name$, $Name$&gt;();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/Field/=Name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=89498DE7E7B6F245BB5446935389B1C5/Field/=Name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Shortcut/@EntryValue">addswaggergen</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Text/@EntryValue">services.AddSwaggerGen(c =&gt;&#xD;
                                        {&#xD;
                                            c.SwaggerDoc("$version$", new OpenApiInfo&#xD;
                                            {&#xD;
                                                Version = "$version$",&#xD;
                                                Title = "$title$",&#xD;
                                                Description = "A simple example ASP.NET Core Web API",&#xD;
                                                TermsOfService = new Uri("$termsOfUse$"),&#xD;
                                                Contact = new OpenApiContact&#xD;
                                                {&#xD;
                                                    Name = "$name$",&#xD;
                                                    Email = $email$,&#xD;
                                                    Url = new Uri("$contactUri$"),&#xD;
                                                },&#xD;
                                                License = new OpenApiLicense&#xD;
                                                {&#xD;
                                                    Name = "$license$",&#xD;
                                                    Url = new Uri("$licenseuri$"),&#xD;
                                                }&#xD;
                                            });&#xD;
                                        &#xD;
                                        	c.OperationFilter&lt;RemoveVersionParameterFilter&gt;();&#xD;
                                        	c.DocumentFilter&lt;ReplaceVersionWithExactValueInPathFilter&gt;();&#xD;
                                        	c.EnableAnnotations();&#xD;
                                        });</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=version/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=version/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=title/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=title/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=termsOfUse/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=termsOfUse/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=name/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=email/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=email/Order/@EntryValue">4</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=contactUri/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=contactUri/Order/@EntryValue">5</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=license/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=license/Order/@EntryValue">6</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=licenseuri/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AF41F15AFBA0434ABDD2AB558C3A80AA/Field/=licenseuri/Order/@EntryValue">7</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B865B888C91316488434F9C3963AD86C/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B865B888C91316488434F9C3963AD86C/Shortcut/@EntryValue">apiversion</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B865B888C91316488434F9C3963AD86C/Text/@EntryValue">[ApiVersion("1.0")]</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B865B888C91316488434F9C3963AD86C/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B865B888C91316488434F9C3963AD86C/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B865B888C91316488434F9C3963AD86C/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B865B888C91316488434F9C3963AD86C/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B865B888C91316488434F9C3963AD86C/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B865B888C91316488434F9C3963AD86C/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=85692C03602BD9419D835140425B22D4/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=85692C03602BD9419D835140425B22D4/Shortcut/@EntryValue">appbuilder</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=85692C03602BD9419D835140425B22D4/Text/@EntryValue">internal class AppBuilder&#xD;
                                        {&#xD;
                                            internal App Build()&#xD;
                                            {&#xD;
                                                var services = new ServiceCollection();&#xD;
                                                var startup = new Startup();&#xD;
                                        &#xD;
                                                startup.ConfigureServices(services);&#xD;
                                                var serviceProvider = services.BuildServiceProvider();&#xD;
                                        &#xD;
                                                return serviceProvider.GetService&lt;App&gt;();        &#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=85692C03602BD9419D835140425B22D4/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=85692C03602BD9419D835140425B22D4/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=85692C03602BD9419D835140425B22D4/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=85692C03602BD9419D835140425B22D4/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=85692C03602BD9419D835140425B22D4/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=85692C03602BD9419D835140425B22D4/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=85692C03602BD9419D835140425B22D4/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDF8642FAD237B42B3C9848EFEB4E545/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDF8642FAD237B42B3C9848EFEB4E545/Shortcut/@EntryValue">appwithservicecollection</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDF8642FAD237B42B3C9848EFEB4E545/Text/@EntryValue">internal class App&#xD;
                                        {&#xD;
                                            private readonly IServiceProvider _serviceProvider;&#xD;
                                        &#xD;
                                            public App(IServiceProvider serviceProvider)&#xD;
                                            {&#xD;
                                                _serviceProvider = serviceProvider;&#xD;
                                            }&#xD;
                                        &#xD;
                                            public Task&lt;int&gt; RunAsync(string[] args)&#xD;
                                            {&#xD;
                                                return Task.FromResult(0);&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDF8642FAD237B42B3C9848EFEB4E545/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDF8642FAD237B42B3C9848EFEB4E545/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDF8642FAD237B42B3C9848EFEB4E545/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDF8642FAD237B42B3C9848EFEB4E545/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDF8642FAD237B42B3C9848EFEB4E545/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDF8642FAD237B42B3C9848EFEB4E545/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDF8642FAD237B42B3C9848EFEB4E545/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/Shortcut/@EntryValue">asrt</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/Description/@EntryValue">Make an assertion</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/Text/@EntryValue">System.Diagnostics.Debug.Assert($END$);</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=354AE64A27D05A4683CE963435035574/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Shortcut/@EntryValue">asrtn</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Description/@EntryValue">Assert expression not null</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Text/@EntryValue">System.Diagnostics.Debug.Assert($EXPR$ != null, "$MESSAGE$");</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Field/=EXPR/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Field/=EXPR/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Field/=EXPR/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Field/=MESSAGE/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=05F019C782D58747A1D3BF462546F906/Field/=MESSAGE/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Shortcut/@EntryValue">assert-add-get-mstest</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Text/@EntryValue">[TestMethod]&#xD;
                                        public async Task Should_Return_That_$Domain$_Which_Was_Added()&#xD;
                                        {&#xD;
                                            // 1. We have to add a $Domain$ first before we can fetch one !&#xD;
                                            var added$Domain$Response = await Client.AssertPostAsync&lt;Add$Domain$Response&gt;("v1/$domainPath$", "Add$Domain$Payload.json", "Added$Domain$Response.json");&#xD;
                                        &#xD;
                                            // 2. We should be able to fetch a $Domain$ which we added before&#xD;
                                            await Client.AssertGetAsync&lt;Get$Domain$ByIdResponse&gt;($"v1/$domainPath$/{added$Domain$Response.User.Id}", "$Domain$GetByIdResponse.json");&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Field/=Domain/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Field/=Domain/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Field/=domainPath/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E76BE2AE3CF21A4D90024CD0394AB881/Field/=domainPath/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA89427ABC7CE34FA887D53B8B2C353D/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA89427ABC7CE34FA887D53B8B2C353D/Shortcut/@EntryValue">assert-get-all</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA89427ABC7CE34FA887D53B8B2C353D/Text/@EntryValue">[NUnit.Framework.Test]&#xD;
                                        public Task Should_Return_All_Expected_$domain$s()&#xD;
                                        {&#xD;
                                            return Client.AssertGetAsync&lt;GetAll$domain$Response&gt;($"api/",&#xD;
                                                                                                "GetAll$domain$.json");&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA89427ABC7CE34FA887D53B8B2C353D/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA89427ABC7CE34FA887D53B8B2C353D/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA89427ABC7CE34FA887D53B8B2C353D/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA89427ABC7CE34FA887D53B8B2C353D/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA89427ABC7CE34FA887D53B8B2C353D/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA89427ABC7CE34FA887D53B8B2C353D/Field/=domain/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA89427ABC7CE34FA887D53B8B2C353D/Field/=domain/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/Shortcut/@EntryValue">assert-get-emtpy-result-mstest</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/Text/@EntryValue">[TestMethod]&#xD;
                                        public Task Should_Return_No_$domain$_If_No_One_Was_Added()&#xD;
                                        {&#xD;
                                            return Client.AssertGetAsync&lt;GetAll$domain$Response&gt;("v1/$domain$", "{}");&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/Field/=domain/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7105FA64DB263842B65B934250A09058/Field/=domain/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/Shortcut/@EntryValue">assert-get-no-auth-mstest</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/Text/@EntryValue">[TestMethod]&#xD;
                                        public Task Should_Not_Return_All_$domain$_Without_Authentication()&#xD;
                                        {&#xD;
                                            return Client.AssertGetAsUnauthorizedAsync("$url$");&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/Field/=domain/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/Field/=domain/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/Field/=url/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1392E7BFE698AE42B886AA0B06F10E70/Field/=url/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Shortcut/@EntryValue">assertdeletebyidnoauth</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Text/@EntryValue">[NUnit.Framework.Test]&#xD;
                                        public Task Should_Not_Be_Able_To_Delete_$entity$_By_Id_Without_Authentication()&#xD;
                                        {&#xD;
                                            return Client.AssertDeleteAsUnauthorizedAsync("$url$");&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Field/=entity/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Field/=entity/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Field/=url/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E7F5A3E1A31854C86F099DBF4FBB650/Field/=url/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Shortcut/@EntryValue">assertdeletenoauth</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Text/@EntryValue">[NUnit.Framework.Test]&#xD;
                                        public Task Should_Not_Be_Able_To_Delete_All_$entity$_Without_Authentication()&#xD;
                                        {&#xD;
                                            return Client.AssertDeleteAsUnauthorizedAsync("$url$");&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Field/=entity/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Field/=entity/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Field/=url/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=48855A09C0C9D044A49E0FA049DA6D56/Field/=url/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Shortcut/@EntryValue">assertgetempty</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Text/@EntryValue">[TestMethod]&#xD;
                                        public Task Should_Return_No_$entity$_If_No_One_Was_Added()&#xD;
                                        {&#xD;
                                            return Client.AssertGetAsync&lt;$response$&gt;("$url$", "{}");&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Field/=response/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Field/=response/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Field/=url/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Field/=url/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Field/=entity/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7D704F5D72E44A45AA9718C953DB7EC7/Field/=entity/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Shortcut/@EntryValue">assertgetnoauth</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Text/@EntryValue">[NUnit.Framework.Test]&#xD;
                                        public Task Should_Not_Return_All_$entity$_Without_Authentication()&#xD;
                                        {&#xD;
                                        	return Client.AssertGetAsUnauthorizedAsync("$url$");&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Field/=entity/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Field/=entity/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Field/=url/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D4CD4522BEACE9459BF41A012CF07183/Field/=url/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Shortcut/@EntryValue">assertpostnoauth</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Text/@EntryValue">[NUnit.Framework.Test]&#xD;
                                        public Task Should_Not_Be_Possible_To_Add_$entity$_Without_Authentication()&#xD;
                                        {&#xD;
                                        	return Client.AssertPostAsUnauthorizedAsync("$url$", new $entity$());&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Field/=entity/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Field/=entity/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Field/=url/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D1394926A334C84AA0ABAAA8A6EE5F10/Field/=url/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Shortcut/@EntryValue">attachedProperty</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Description/@EntryValue">Attached property</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Text/@EntryValue">public static readonly $dependencyProperty$ $propertyName$Property = $dependencyProperty$.RegisterAttached(&#xD;
                                          "$propertyName$", typeof($propertyType$), typeof($containingType$), new PropertyMetadata(default($propertyType$)));&#xD;
                                        &#xD;
                                        public static void Set$propertyName$(DependencyObject $element$, $propertyType$ value)&#xD;
                                        {&#xD;
                                          $element$.SetValue($propertyName$Property, value);&#xD;
                                        }&#xD;
                                        &#xD;
                                        public static $propertyType$ Get$propertyName$(DependencyObject $element$)&#xD;
                                        {&#xD;
                                          return ($propertyType$) $element$.GetValue($propertyName$Property);&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=propertyType/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=propertyType/InitialRange/@EntryValue">2</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=propertyType/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=propertyName/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=propertyName/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=propertyName/InitialRange/@EntryValue">2</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=propertyName/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=containingType/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=containingType/Expression/@EntryValue">typeName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=containingType/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=containingType/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=element/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=element/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=element/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=dependencyProperty/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=dependencyProperty/Expression/@EntryValue">dependencyPropertyType()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=dependencyProperty/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1977368ACEFFAE4E9DCB17D517539A3B/Field/=dependencyProperty/Order/@EntryValue">4</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Shortcut/@EntryValue">Attribute</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Description/@EntryValue">Attribute using recommended pattern</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Text/@EntryValue">[System.AttributeUsage(System.AttributeTargets.$target$, Inherited = $inherited$, AllowMultiple = $allowmultiple$)]&#xD;
                                        sealed class $name$Attribute : System.Attribute&#xD;
                                        {&#xD;
                                            // See the attribute guidelines at &#xD;
                                            //  http://go.microsoft.com/fwlink/?LinkId=85236&#xD;
                                           public $name$Attribute () &#xD;
                                           { &#xD;
                                               $SELSTART$// TODO: Implement code here&#xD;
                                               throw new System.NotImplementedException();$SELEND$&#xD;
                                           }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Scope/=558F05AA0DE96347816FF785232CFB2A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Scope/=558F05AA0DE96347816FF785232CFB2A/Type/@EntryValue">InCSharpTypeAndNamespace</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Scope/=558F05AA0DE96347816FF785232CFB2A/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=name/Expression/@EntryValue">constant("My")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=target/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=target/Expression/@EntryValue">constant("All")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=target/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=inherited/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=inherited/Expression/@EntryValue">constant("false")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=inherited/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=allowmultiple/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=allowmultiple/Expression/@EntryValue">constant("true")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0F35E18376564D4EAF74C84462E8E019/Field/=allowmultiple/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Shortcut/@EntryValue">auth0</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Text/@EntryValue">{&#xD;
                                          "Auth0": {&#xD;
                                            "Authority": "$authority$",&#xD;
                                            "TokenEndpoint": "$tokenendpoint$",&#xD;
                                            "ClientId": "$clientid$",&#xD;
                                            "ClientSecret": "$clientsecret$",&#xD;
                                            "Audience": "$audience$",&#xD;
                                            "TokenCacheTime": "23:55:00"&#xD;
                                          }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Field/=audience/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Field/=audience/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Field/=clientid/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Field/=clientid/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Field/=clientsecret/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Field/=clientsecret/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Field/=tokenendpoint/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Field/=tokenendpoint/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Field/=authority/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7D060CC871D5479034CFC20984EBC4/Field/=authority/Order/@EntryValue">4</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/Shortcut/@EntryValue">checked</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/Description/@EntryValue">checked block</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/Text/@EntryValue">checked&#xD;
                                          {&#xD;
                                             $END$&#xD;
                                          }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=9BE634097493074F976CAF52B8358C0E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Shortcut/@EntryValue">class</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Text/@EntryValue">class $name$&#xD;
                                          {&#xD;
                                            $END$&#xD;
                                          }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Scope/=558F05AA0DE96347816FF785232CFB2A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Scope/=558F05AA0DE96347816FF785232CFB2A/Type/@EntryValue">InCSharpTypeAndNamespace</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Scope/=558F05AA0DE96347816FF785232CFB2A/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Field/=name/Expression/@EntryValue">constant("MyClass")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=79EC7A10F8C5424AB1F620BFF9D9A55F/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BB8C7B36E5352E4D909780D078AE2616/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BB8C7B36E5352E4D909780D078AE2616/Shortcut/@EntryValue">clisystemtestbase</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BB8C7B36E5352E4D909780D078AE2616/Text/@EntryValue">public abstract class SystemTestBase&#xD;
                                        {&#xD;
                                            protected string Output { get; private set; }&#xD;
                                        &#xD;
                                            protected int ExitCode { get; private set; }&#xD;
                                        &#xD;
                                            [TestInitialize]&#xD;
                                            public async Task InitAsync()&#xD;
                                            {&#xD;
                                                BeforeExecution();&#xD;
                                        &#xD;
                                                await using var sw = new StringWriter();&#xD;
                                                Console.SetOut(sw);&#xD;
                                        &#xD;
                                                string[] strings = CollectConsoleParameters().ToArray();&#xD;
                                                ExitCode = await Program.Main(strings);&#xD;
                                                Output = sw.ToString();&#xD;
                                        &#xD;
                                                AfterExecution();&#xD;
                                            }&#xD;
                                        &#xD;
                                            protected virtual void BeforeExecution()&#xD;
                                            {&#xD;
                                            }&#xD;
                                        &#xD;
                                            protected abstract IEnumerable&lt;string&gt; CollectConsoleParameters();&#xD;
                                        &#xD;
                                            protected virtual void AfterExecution()&#xD;
                                            {&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BB8C7B36E5352E4D909780D078AE2616/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BB8C7B36E5352E4D909780D078AE2616/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BB8C7B36E5352E4D909780D078AE2616/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BB8C7B36E5352E4D909780D078AE2616/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BB8C7B36E5352E4D909780D078AE2616/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BB8C7B36E5352E4D909780D078AE2616/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BB8C7B36E5352E4D909780D078AE2616/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/Shortcut/@EntryValue">collecterrros</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/Text/@EntryValue">private static IEnumerable&lt;string&gt; CollectErrors($type$ type)&#xD;
                                        {&#xD;
                                        	yield break;&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/Field/=type/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=496C565B4C01584DBA16970AC97F9D4F/Field/=type/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=33B67CE9FE69324BB5C4918CC730B06F/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=33B67CE9FE69324BB5C4918CC730B06F/Shortcut/@EntryValue">command-no-response</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=33B67CE9FE69324BB5C4918CC730B06F/Text/@EntryValue">internal sealed record $command$ : ICommand;&#xD;
                                        &#xD;
                                        internal sealed class $command$Handler : ICommandHandler&lt;$command$&gt;&#xD;
                                        {&#xD;
                                            public Task Handle($command$ request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=33B67CE9FE69324BB5C4918CC730B06F/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=33B67CE9FE69324BB5C4918CC730B06F/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=33B67CE9FE69324BB5C4918CC730B06F/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=33B67CE9FE69324BB5C4918CC730B06F/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=33B67CE9FE69324BB5C4918CC730B06F/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=33B67CE9FE69324BB5C4918CC730B06F/Field/=command/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=33B67CE9FE69324BB5C4918CC730B06F/Field/=command/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/Shortcut/@EntryValue">command-with-response</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/Text/@EntryValue">internal sealed record $command$ : ICommand&lt;$response$&gt;;&#xD;
                                        &#xD;
                                        public sealed record $response$;&#xD;
                                        &#xD;
                                        internal sealed class $command$Handler : ICommandHandler&lt;$command$, $response$&gt;&#xD;
                                        {&#xD;
                                            public Task&lt;$response$&gt; Handle($command$ request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/Field/=command/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/Field/=command/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/Field/=response/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=70EE0E6CE47B244098C3D801806588D7/Field/=response/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B534B8320B945B43B5B070BF4CF23571/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B534B8320B945B43B5B070BF4CF23571/Shortcut/@EntryValue">consumesjson</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B534B8320B945B43B5B070BF4CF23571/Text/@EntryValue">[Consumes(MediaTypeNames.Application.Json)]</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B534B8320B945B43B5B070BF4CF23571/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B534B8320B945B43B5B070BF4CF23571/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B534B8320B945B43B5B070BF4CF23571/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B534B8320B945B43B5B070BF4CF23571/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B534B8320B945B43B5B070BF4CF23571/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B534B8320B945B43B5B070BF4CF23571/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Shortcut/@EntryValue">controller-simple-get</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Text/@EntryValue">[Authorize]&#xD;
                                        [ApiController]&#xD;
                                        [ApiVersion("1.0")]&#xD;
                                        [Route("$baseurl$")]&#xD;
                                        public class $entityUpperCase$sController : ControllerBase&#xD;
                                        {&#xD;
                                        	[HttpGet]&#xD;
                                        	[ProducesResponseType(typeof(GetAll$entityUpperCase$Response), StatusCodes.Status200OK)]&#xD;
                                        	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                        	[SwaggerOperation(OperationId = "get-all-$entitylowercase$", Tags = new[] { "$entityUpperCase$s" })]&#xD;
                                        	public Task&lt;GetAll$entityUpperCase$Response&gt; GetAll$entityUpperCase$Async()&#xD;
                                        	{&#xD;
                                        		throw new NotImplementedException();&#xD;
                                        	}&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Field/=baseurl/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Field/=baseurl/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Field/=entityUpperCase/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Field/=entityUpperCase/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Field/=entitylowercase/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F6E97C774C0EBE458B9BF56D1BEAA9CE/Field/=entitylowercase/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E63EA076A7E75048B60C38407CC1C2AC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E63EA076A7E75048B60C38407CC1C2AC/Shortcut/@EntryValue">cors-settings</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E63EA076A7E75048B60C38407CC1C2AC/Text/@EntryValue">  "CorsSettings": {&#xD;
                                            "Origins": "*",&#xD;
                                            "Headers": "Origin, X-Requested-With, Content-Type, Accept",&#xD;
                                            "AllowCredentials": true&#xD;
                                          }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E63EA076A7E75048B60C38407CC1C2AC/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E63EA076A7E75048B60C38407CC1C2AC/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E63EA076A7E75048B60C38407CC1C2AC/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E63EA076A7E75048B60C38407CC1C2AC/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E63EA076A7E75048B60C38407CC1C2AC/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Shortcut/@EntryValue">ctor</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Description/@EntryValue">Constructor</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Text/@EntryValue">public $classname$ ()&#xD;
                                          {&#xD;
                                            $END$&#xD;
                                          }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Field/=classname/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Field/=classname/Expression/@EntryValue">typeName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Field/=classname/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0E1FAF5241FA8A40AEB03CA2AE065648/Field/=classname/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Shortcut/@EntryValue">ctx</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Description/@EntryValue">Current file context</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Text/@EntryValue">$CTX$</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Field/=CTX/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Field/=CTX/Expression/@EntryValue">context()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A334C89806C7EA41A9842AEA755A9DB7/Field/=CTX/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/Shortcut/@EntryValue">cw</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/Description/@EntryValue">Console.WriteLine</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/Text/@EntryValue">System.Console.WriteLine($END$);</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FD9118DAEBFF4A90C6EED9337279C9/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Shortcut/@EntryValue">dependencyProperty</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Description/@EntryValue">Dependency property</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Text/@EntryValue">public static readonly $dependencyProperty$ $propertyName$Property = $dependencyProperty$.Register(&#xD;
                                          $nameofProperty$, typeof($propertyType$), typeof($containingType$), new PropertyMetadata(default($propertyType$)));&#xD;
                                        &#xD;
                                        public $propertyType$ $propertyName$&#xD;
                                        {&#xD;
                                          get { return ($propertyType$) GetValue($propertyName$Property); }&#xD;
                                          set { SetValue($propertyName$Property, value); }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=propertyType/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=propertyType/InitialRange/@EntryValue">2</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=propertyType/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=propertyName/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=propertyName/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=propertyName/InitialRange/@EntryValue">2</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=propertyName/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=containingType/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=containingType/Expression/@EntryValue">typeName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=containingType/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=containingType/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=dependencyProperty/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=dependencyProperty/Expression/@EntryValue">dependencyPropertyType()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=dependencyProperty/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=dependencyProperty/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=nameofProperty/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=nameofProperty/Expression/@EntryValue">nameOfEntity(propertyName)</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=nameofProperty/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=61E52A32A8000749A65D71522F7675B8/Field/=nameofProperty/Order/@EntryValue">4</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Shortcut/@EntryValue">do</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Description/@EntryValue">do...while loop</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Text/@EntryValue">do&#xD;
                                        {&#xD;
                                          $SELECTION$$END$&#xD;
                                        } while ($expression$);</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Mnemonic/@EntryValue">4</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Field/=expression/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Field/=expression/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4F61B30618E42D489E7A607F7A81459A/Field/=expression/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Shortcut/@EntryValue">ear</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Description/@EntryValue">Create an empty array</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Text/@EntryValue">$TYPE$[] $NAME$ = new $TYPE$[] {};</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Field/=TYPE/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Field/=TYPE/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Field/=NAME/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Field/=NAME/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8C1E613588C720469EB878D54125C9F2/Field/=NAME/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/Shortcut/@EntryValue">else</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/Description/@EntryValue">else statement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/Text/@EntryValue">else&#xD;
                                        {&#xD;
                                          $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/IsKeywordRequired/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DFFE5706C2C1CC4FA7DE44A8581809E9/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Shortcut/@EntryValue">enum</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Text/@EntryValue">enum $name$&#xD;
                                        {&#xD;
                                          $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Scope/=558F05AA0DE96347816FF785232CFB2A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Scope/=558F05AA0DE96347816FF785232CFB2A/Type/@EntryValue">InCSharpTypeAndNamespace</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Scope/=558F05AA0DE96347816FF785232CFB2A/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Field/=name/Expression/@EntryValue">constant("MyEnum")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5864DF8CF3941B46824BA7075B496234/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1EB75AABA8EA594FAF6B2E1B7A0717CD/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1EB75AABA8EA594FAF6B2E1B7A0717CD/Shortcut/@EntryValue">errorhandlingmiddleware</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1EB75AABA8EA594FAF6B2E1B7A0717CD/Text/@EntryValue">public class ErrorHandlingMiddleware : IMiddleware&#xD;
                                        {&#xD;
                                            public async Task InvokeAsync(HttpContext context, RequestDelegate next)&#xD;
                                            {&#xD;
                                                try&#xD;
                                                {&#xD;
                                                    await next(context);&#xD;
                                                }&#xD;
                                                catch (ProblemDetailsException exception)&#xD;
                                                {&#xD;
                                                    context.Response.Clear();&#xD;
                                                    context.Response.ContentType = MediaTypeNames.Application.Json;&#xD;
                                        &#xD;
                                                    if (exception.ProblemDetails.Status != null)&#xD;
                                                    {&#xD;
                                                        context.Response.StatusCode = exception.ProblemDetails.Status.Value;&#xD;
                                                    }&#xD;
                                        &#xD;
                                                    await context.Response.WriteAsync(JsonSerializer.Serialize(exception.ProblemDetails));&#xD;
                                                }&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        public class ProblemDetailsException : Exception&#xD;
                                        {&#xD;
                                            public ProblemDetailsException(int statusCode, string title, string details,&#xD;
                                                params (string key, object value)[] extensions)&#xD;
                                            {&#xD;
                                                ProblemDetails = new ProblemDetails&#xD;
                                                {&#xD;
                                                    Title = title,&#xD;
                                                    Detail = details,&#xD;
                                                    Status = statusCode&#xD;
                                                };&#xD;
                                        &#xD;
                                                foreach (var extension in extensions.Select(tuple =&gt;&#xD;
                                                    new KeyValuePair&lt;string, object&gt;(tuple.key, tuple.value)))&#xD;
                                                {&#xD;
                                                    ProblemDetails.Extensions.Add(extension);&#xD;
                                                }&#xD;
                                            }&#xD;
                                        &#xD;
                                            public ProblemDetails ProblemDetails { get; }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1EB75AABA8EA594FAF6B2E1B7A0717CD/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1EB75AABA8EA594FAF6B2E1B7A0717CD/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1EB75AABA8EA594FAF6B2E1B7A0717CD/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1EB75AABA8EA594FAF6B2E1B7A0717CD/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1EB75AABA8EA594FAF6B2E1B7A0717CD/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1EB75AABA8EA594FAF6B2E1B7A0717CD/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Shortcut/@EntryValue">Exception</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Text/@EntryValue">public class $newException$Exception : System.Exception&#xD;
                                        {&#xD;
                                          public $newException$Exception() { }&#xD;
                                          public $newException$Exception( string message ) : base( message ) { }&#xD;
                                          public $newException$Exception( string message, System.Exception inner ) : base( message, inner ) { }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Scope/=558F05AA0DE96347816FF785232CFB2A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Scope/=558F05AA0DE96347816FF785232CFB2A/Type/@EntryValue">InCSharpTypeAndNamespace</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Scope/=558F05AA0DE96347816FF785232CFB2A/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Field/=newException/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Field/=newException/Expression/@EntryValue">constant("My")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B9383001B58E7F4399544EC6C6D4D82F/Field/=newException/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Shortcut/@EntryValue">extensionclass</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Text/@EntryValue">public static class $Name$Extensions&#xD;
                                        {&#xD;
                                        	$method$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Field/=Name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Field/=Name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Field/=method/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4844543B88F8814EAF0499F226CEAEA1/Field/=method/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Shortcut/@EntryValue">extensionmethod</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Text/@EntryValue">public static void $name$(this $type$ source)&#xD;
                                        {&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Field/=type/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DACC672DB86C8D408E2B8DFBEBE56C98/Field/=type/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Shortcut/@EntryValue">for</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Description/@EntryValue">Simple "for" loop</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Text/@EntryValue">for (int $INDEX$ = 0; $INDEX$ &lt; $UPPER$; $INDEX$++)
                                        {
                                          $SELECTION$$END$
                                        }</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Mnemonic/@EntryValue">3</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Categories/=Iteration/@EntryIndexedValue">Iteration</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Field/=INDEX/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Field/=INDEX/Expression/@EntryValue">suggestIndexVariable()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Field/=INDEX/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Field/=UPPER/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=68FA74F8D4330A42BEFB3E08129F1F35/Field/=UPPER/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Shortcut/@EntryValue">foreach</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Description/@EntryValue">foreach block</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Text/@EntryValue">foreach ($TYPE$ $VARIABLE$ in $COLLECTION$)
                                        {
                                          $SELECTION$$END$
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Categories/=Iteration/@EntryIndexedValue">Iteration</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Field/=COLLECTION/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Field/=COLLECTION/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Field/=COLLECTION/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Field/=TYPE/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Field/=TYPE/Expression/@EntryValue">suggestVariableType()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Field/=TYPE/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Field/=VARIABLE/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Field/=VARIABLE/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D0E71BB59F55A449A6E127810A8802E/Field/=VARIABLE/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Shortcut/@EntryValue">forr</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Description/@EntryValue">Reverse 'for' loop</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Text/@EntryValue">for (int $index$ = $max$ - 1; $index$ &gt;= 0 ; $index$--)&#xD;
                                              {&#xD;
                                               $END$&#xD;
                                              }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Field/=index/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Field/=index/Expression/@EntryValue">constant("i")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Field/=index/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Field/=max/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Field/=max/Expression/@EntryValue">constant("length")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D8FBD7D188BBCE4ABB40D540E00A273B/Field/=max/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Shortcut/@EntryValue">from</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Description/@EntryValue">Language-Integrated Query</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Text/@EntryValue">from $VAR$ in $COLLECTION$ $END$</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/Type/@EntryValue">InCSharpExpression</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">3.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Scope/=CE6825B6B50BCB44A4991BEC7FBA3363/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Scope/=CE6825B6B50BCB44A4991BEC7FBA3363/Type/@EntryValue">InCSharpQuery</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Scope/=CE6825B6B50BCB44A4991BEC7FBA3363/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">3.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Field/=COLLECTION/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Field/=COLLECTION/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Field/=COLLECTION/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Field/=VAR/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Field/=VAR/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FFBBE406877ED540AA54BDEEC062C26B/Field/=VAR/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=26B7E4A6CF98EE4E866ECE48CBAC24F8/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=26B7E4A6CF98EE4E866ECE48CBAC24F8/Shortcut/@EntryValue">getresponsetypes</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=26B7E4A6CF98EE4E866ECE48CBAC24F8/Text/@EntryValue">[ProducesResponseType(StatusCodes.Status200OK)]&#xD;
                                        [ProducesResponseType(StatusCodes.Status404NotFound)]</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=26B7E4A6CF98EE4E866ECE48CBAC24F8/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=26B7E4A6CF98EE4E866ECE48CBAC24F8/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=26B7E4A6CF98EE4E866ECE48CBAC24F8/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=26B7E4A6CF98EE4E866ECE48CBAC24F8/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=26B7E4A6CF98EE4E866ECE48CBAC24F8/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=26B7E4A6CF98EE4E866ECE48CBAC24F8/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=311056B6864A9A4097CACBCBD829A018/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=311056B6864A9A4097CACBCBD829A018/Shortcut/@EntryValue">globalproduceconsumes</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=311056B6864A9A4097CACBCBD829A018/Text/@EntryValue">services.Configure&lt;Microsoft.AspNetCore.Mvc.MvcOptions&gt;(options =&gt;&#xD;
                                        {&#xD;
                                        	options.Filters.Add(new Microsoft.AspNetCore.Mvc.ProducesAttribute(System.Net.Mime.MediaTypeNames.Application.Json));&#xD;
                                        	options.Filters.Add(new Microsoft.AspNetCore.Mvc.ConsumesAttribute(System.Net.Mime.MediaTypeNames.Application.Json));&#xD;
                                        });</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=311056B6864A9A4097CACBCBD829A018/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=311056B6864A9A4097CACBCBD829A018/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=311056B6864A9A4097CACBCBD829A018/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=311056B6864A9A4097CACBCBD829A018/Scope/=74A278E9BF386142B53D57114609A033/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=311056B6864A9A4097CACBCBD829A018/Scope/=74A278E9BF386142B53D57114609A033/Type/@EntryValue">InCSharpExceptStringLiterals</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=311056B6864A9A4097CACBCBD829A018/Scope/=74A278E9BF386142B53D57114609A033/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Shortcut/@EntryValue">hal</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Description/@EntryValue">ASP.NET MVC Html.ActionLink</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Text/@EntryValue">Html.ActionLink("$TEXT$", "$ACTION$", "$CONTROLLER$")</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/Type/@EntryValue">InCSharpExpression</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Field/=CONTROLLER/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Field/=CONTROLLER/Expression/@EntryValue">AspMvcController()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Field/=CONTROLLER/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Field/=ACTION/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Field/=ACTION/Expression/@EntryValue">AspMvcAction()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Field/=ACTION/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Field/=TEXT/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=77EFEAA375D05C438E4FAA109D414F0A/Field/=TEXT/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Shortcut/@EntryValue">if</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Description/@EntryValue">if statement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Text/@EntryValue">if ($expr$)&#xD;
                                        {  &#xD;
                                          $SELECTION$$END$&#xD;
                                        }</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Mnemonic/@EntryValue">1</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Field/=expr/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Field/=expr/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2309729714BBB04EB920AA915E5D0401/Field/=expr/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Shortcut/@EntryValue">indexer</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Text/@EntryValue">$access$ $type$ this[$indextype$ index]&#xD;
                                        {&#xD;
                                          get {$END$ /* return the specified index here */ }&#xD;
                                          set { /* set the specified index to value here */ }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Field/=access/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Field/=access/Expression/@EntryValue">constant("public")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Field/=access/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Field/=type/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Field/=type/Expression/@EntryValue">constant("object")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Field/=type/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Field/=indextype/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Field/=indextype/Expression/@EntryValue">constant("int")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DA7C27CCE7F18F4CB8285120D4B30C1A/Field/=indextype/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Shortcut/@EntryValue">interface</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Text/@EntryValue">interface I$name$&#xD;
                                        {&#xD;
                                          $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Scope/=558F05AA0DE96347816FF785232CFB2A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Scope/=558F05AA0DE96347816FF785232CFB2A/Type/@EntryValue">InCSharpTypeAndNamespace</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Scope/=558F05AA0DE96347816FF785232CFB2A/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Field/=name/Expression/@EntryValue">constant("Interface")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F911B959B4259749AC984F9935FCD690/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Shortcut/@EntryValue">itar</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Description/@EntryValue">Iterate an array</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Text/@EntryValue">for (int $INDEX$ = 0; $INDEX$ &lt; $ARRAY$.Length; $INDEX$++)
                                        {
                                          $TYPE$ $VAR$ = $ARRAY$[$INDEX$];
                                          $END$
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Categories/=Iteration/@EntryIndexedValue">Iteration</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=INDEX/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=INDEX/Expression/@EntryValue">suggestIndexVariable()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=INDEX/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=ARRAY/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=ARRAY/Expression/@EntryValue">arrayVariable()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=ARRAY/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=TYPE/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=TYPE/Expression/@EntryValue">suggestVariableType()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=TYPE/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=VAR/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=VAR/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=81FCBFCF7D874C4B91CFA2C35B1C6FED/Field/=VAR/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Shortcut/@EntryValue">iterator</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Description/@EntryValue">simple iterator</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Text/@EntryValue">public $SystemCollectionsGenericIEnumeratorG$&lt;$type$&gt; GetEnumerator()&#xD;
                                        {&#xD;
                                            $SELSTART$throw new System.NotImplementedException();&#xD;
                                            yield return default($type$);&#xD;
                                            $SELEND$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Field/=type/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Field/=type/Expression/@EntryValue">constant("ElementType")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Field/=type/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Field/=SystemCollectionsGenericIEnumeratorG/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Field/=SystemCollectionsGenericIEnumeratorG/Expression/@EntryValue">constant("System.Collections.Generic.IEnumerator")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CBB4F856BADB43438E70875916BD8696/Field/=SystemCollectionsGenericIEnumeratorG/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Shortcut/@EntryValue">itli</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Description/@EntryValue">Iterate a IList&lt;T&gt;</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Text/@EntryValue">for (int $INDEX$ = 0; $INDEX$ &lt; $LIST$.Count; $INDEX$++)&#xD;
                                        {&#xD;
                                          $TYPE$ $ITEM$ = $LIST$[$INDEX$];&#xD;
                                          $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Categories/=Iteration/@EntryIndexedValue">Iteration</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=INDEX/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=INDEX/Expression/@EntryValue">suggestIndexVariable()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=INDEX/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=LIST/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=LIST/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=LIST/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=TYPE/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=TYPE/Expression/@EntryValue">suggestVariableType()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=TYPE/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=ITEM/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=ITEM/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8F0E54793F0A314D8075BC893388FA05/Field/=ITEM/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Shortcut/@EntryValue">join</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Description/@EntryValue">Join clause in language integrated query</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Text/@EntryValue">join $NAME$ in $COL$ on $EXPR1$ equals $EXPR2$ $END$</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Scope/=CE6825B6B50BCB44A4991BEC7FBA3363/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Scope/=CE6825B6B50BCB44A4991BEC7FBA3363/Type/@EntryValue">InCSharpQuery</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Scope/=CE6825B6B50BCB44A4991BEC7FBA3363/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Field/=COL/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Field/=COL/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Field/=COL/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Field/=NAME/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Field/=NAME/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Field/=NAME/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Field/=EXPR1/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Field/=EXPR1/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Field/=EXPR2/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC7C16F31F9EE84F904D0A3402F0AE64/Field/=EXPR2/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Shortcut/@EntryValue">lock</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Description/@EntryValue">lock statement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Text/@EntryValue">lock ($expression$)&#xD;
                                        {&#xD;
                                           $SELECTION$$END$&#xD;
                                        }</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Mnemonic/@EntryValue">5</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Field/=expression/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Field/=expression/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EBB23BAB9366E444BA5150799D6BC766/Field/=expression/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Shortcut/@EntryValue">ltcExtensionMethods</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Description/@EntryValue">This is the snippet for an class which represents extension methods for an specifc type</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Text/@EntryValue">// &lt;copyright file="$Type$.cs" company="TRUMPF GmbH + Co. KG"&gt;&#xD;
                                        //   All rights reserved to TRUMPF GmbH + Co. KG, Germany.&#xD;
                                        // &lt;/copyright&gt;&#xD;
                                        &#xD;
                                        namespace ExtensionMethods&#xD;
                                        {&#xD;
                                            using System;&#xD;
                                        &#xD;
                                        	/// &lt;summary&gt;&#xD;
                                        	/// Extension methods for the &lt;see cref="$Type$"/&gt;.&#xD;
                                        	/// &lt;/summary&gt;&#xD;
                                        	public static class $Type$Extensions&#xD;
                                        	{	&#xD;
                                        	}&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Field/=Type/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Field/=Type/InitialRange/@EntryValue">1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5E4ADF066AD53D4499268487A4B53B72/Field/=Type/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Shortcut/@EntryValue">ltcIDisposeableBase</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Description/@EntryValue">This template represents a base class for IDisposable</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Text/@EntryValue">/// &lt;summary&gt;&#xD;
                                        /// Represents a base class for &lt;see cref="IDisposable"/&gt; implementation.&#xD;
                                        /// &lt;/summary&gt;&#xD;
                                        /// &lt;seealso cref="System.IDisposable" /&gt;&#xD;
                                        public abstract class $Name$ : IDisposable&#xD;
                                        {&#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// The m disposed&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            private bool mDisposed;&#xD;
                                        &#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            public void Dispose()&#xD;
                                            {&#xD;
                                                Dispose(true);&#xD;
                                                GC.SuppressFinalize(this);&#xD;
                                            }&#xD;
                                        &#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Releases unmanaged and - optionally - managed resources.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            /// &lt;param name="disposing"&gt;&lt;c&gt;true&lt;/c&gt; to release both managed and unmanaged resources; &lt;c&gt;false&lt;/c&gt; to release only unmanaged resources.&lt;/param&gt;&#xD;
                                            protected void Dispose(bool disposing)&#xD;
                                            {&#xD;
                                                if (!mDisposed)&#xD;
                                                {&#xD;
                                                    if (disposing)&#xD;
                                                    {&#xD;
                                                        DisposeManagedResources();&#xD;
                                                    }&#xD;
                                        &#xD;
                                                    DisposeUnManagedResources();&#xD;
                                                    mDisposed = true;&#xD;
                                                }&#xD;
                                            }&#xD;
                                        &#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Disposes the managed resources.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            protected abstract void DisposeManagedResources();    &#xD;
                                        &#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Disposes the unmanaged resources.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            protected abstract void DisposeUnManagedResources();&#xD;
                                        &#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Finalizes an instance of the &lt;see cref="~$Name$"/&gt; class.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            ~$Name$()&#xD;
                                            {&#xD;
                                                Dispose(false);&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Field/=Name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E630FBC80FC38F479F8F4BF65136CCEF/Field/=Name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Shortcut/@EntryValue">ltcMarkupExtensionConverterOneWay</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Description/@EntryValue">test</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Text/@EntryValue">[ValueConversion(typeof(object), typeof(object))]&#xD;
                                        public class $classname$ : TcMarkupExtensionConverterOneWay&lt;$classname$&gt;&#xD;
                                        {&#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Converts a value.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            /// &lt;param name="value"&gt;The value produced by the binding source.&lt;/param&gt;&#xD;
                                            /// &lt;param name="targetType"&gt;The type of the binding target property.&lt;/param&gt;&#xD;
                                            /// &lt;param name="parameter"&gt;The converter parameter to use.&lt;/param&gt;&#xD;
                                            /// &lt;param name="culture"&gt;The culture to use in the converter.&lt;/param&gt;&#xD;
                                            /// &lt;returns&gt;&#xD;
                                            /// The converted value. If the method returns &lt;c&gt;null&lt;/c&gt;, the valid &lt;c&gt;null&lt;/c&gt; value is used.&#xD;
                                            /// &lt;/returns&gt;&#xD;
                                            public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)&#xD;
                                            {&#xD;
                                                if (!ValidateValue(value))&#xD;
                                                {&#xD;
                                                    return null;&#xD;
                                                }&#xD;
                                        &#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Field/=classname/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=78495D577D35A34B9E360EEA9D5747D2/Field/=classname/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/Shortcut/@EntryValue">ltcMarkupExtensionMultiValueConverter</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/Text/@EntryValue">[ValueConversion(typeof(object), typeof(object))]&#xD;
                                        public class  $classname$ : TcMarkupExtensionMultiValueConverter&lt; $classname$&gt;&#xD;
                                        {&#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            /// &lt;param name="values"&gt;The array of values that the source bindings in the &lt;see cref="T:System.Windows.Data.MultiBinding" /&gt; produces. The value &lt;see cref="F:System.Windows.DependencyProperty.UnsetValue" /&gt; indicates that the source binding has no value to provide for conversion.&lt;/param&gt;&#xD;
                                            /// &lt;param name="targetType"&gt;The type of the binding target property.&lt;/param&gt;&#xD;
                                            /// &lt;param name="parameter"&gt;The converter parameter to use.&lt;/param&gt;&#xD;
                                            /// &lt;param name="culture"&gt;The culture to use in the converter.&lt;/param&gt;&#xD;
                                            /// &lt;returns&gt;&#xD;
                                            /// A converted value.If the method returns null, the valid null value is used.A return value of &lt;see cref="T:System.Windows.DependencyProperty" /&gt;.&lt;see cref="F:System.Windows.DependencyProperty.UnsetValue" /&gt; indicates that the converter did not produce a value, and that the binding will use the &lt;see cref="P:System.Windows.Data.BindingBase.FallbackValue" /&gt; if it is available, or else will use the default value.A return value of &lt;see cref="T:System.Windows.Data.Binding" /&gt;.&lt;see cref="F:System.Windows.Data.Binding.DoNothing" /&gt; indicates that the binding does not transfer the value or use the &lt;see cref="P:System.Windows.Data.BindingBase.FallbackValue" /&gt; or the default value.&#xD;
                                            /// &lt;/returns&gt;&#xD;
                                            /// &lt;exception cref="System.NotImplementedException"&gt;&lt;/exception&gt;&#xD;
                                            public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)&#xD;
                                            {&#xD;
                                                if (!ValidateValue(values))&#xD;
                                                {&#xD;
                                        &#xD;
                                                    return null;&#xD;
                                        &#xD;
                                                }&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        &#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Converts a binding target value to the source binding values.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            /// &lt;param name="value"&gt;The value that the binding target produces.&lt;/param&gt;&#xD;
                                            /// &lt;param name="targetTypes"&gt;The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.&lt;/param&gt;&#xD;
                                            /// &lt;param name="parameter"&gt;The converter parameter to use.&lt;/param&gt;&#xD;
                                            /// &lt;param name="culture"&gt;The culture to use in the converter.&lt;/param&gt;&#xD;
                                            /// &lt;returns&gt;&#xD;
                                            /// An array of values that have been converted from the target value back to the source values.&#xD;
                                            /// &lt;/returns&gt;&#xD;
                                            /// &lt;exception cref="System.NotImplementedException"&gt;&lt;/exception&gt;&#xD;
                                            public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/Field/=classname/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AFA58C46B3CEC4FB18605F36155D807/Field/=classname/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/Shortcut/@EntryValue">ltcMarkupExtensionMultiValueConverterOneWay</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/Text/@EntryValue">[ValueConversion(typeof(object), typeof(object))]&#xD;
                                        public class  $classname$ : TcMarkupExtensionMultiValueConverterOneWay&lt; $classname$&gt;&#xD;
                                        {&#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            /// &lt;param name="values"&gt;The array of values that the source bindings in the &lt;see cref="T:System.Windows.Data.MultiBinding" /&gt; produces. The value &lt;see cref="F:System.Windows.DependencyProperty.UnsetValue" /&gt; indicates that the source binding has no value to provide for conversion.&lt;/param&gt;&#xD;
                                            /// &lt;param name="targetType"&gt;The type of the binding target property.&lt;/param&gt;&#xD;
                                            /// &lt;param name="parameter"&gt;The converter parameter to use.&lt;/param&gt;&#xD;
                                            /// &lt;param name="culture"&gt;The culture to use in the converter.&lt;/param&gt;&#xD;
                                            /// &lt;returns&gt;&#xD;
                                            /// A converted value.If the method returns null, the valid null value is used.A return value of &lt;see cref="T:System.Windows.DependencyProperty" /&gt;.&lt;see cref="F:System.Windows.DependencyProperty.UnsetValue" /&gt; indicates that the converter did not produce a value, and that the binding will use the &lt;see cref="P:System.Windows.Data.BindingBase.FallbackValue" /&gt; if it is available, or else will use the default value.A return value of &lt;see cref="T:System.Windows.Data.Binding" /&gt;.&lt;see cref="F:System.Windows.Data.Binding.DoNothing" /&gt; indicates that the binding does not transfer the value or use the &lt;see cref="P:System.Windows.Data.BindingBase.FallbackValue" /&gt; or the default value.&#xD;
                                            /// &lt;/returns&gt;&#xD;
                                            /// &lt;exception cref="System.NotImplementedException"&gt;&lt;/exception&gt;&#xD;
                                            public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)&#xD;
                                            {&#xD;
                                                if (!ValidateValue(values))&#xD;
                                                {&#xD;
                                        &#xD;
                                                    return null;&#xD;
                                        &#xD;
                                                }&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/Field/=classname/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DF225FFCA4724A47A19779EF8646C1BE/Field/=classname/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Shortcut/@EntryValue">ltcPublicReturnsTask</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Description/@EntryValue">Creates a method with no parameters, which returns a class.</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Text/@EntryValue">public Task&lt;$returnType$&gt; $name$()&#xD;
                                        {&#xD;
                                        	$END$&#xD;
                                        	return null;&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Field/=returnType/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Field/=returnType/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2E766DAF0A68FE4AAC86186085612459/Field/=name/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Shortcut/@EntryValue">ltmExtension</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Description/@EntryValue">Represents an extension method</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Text/@EntryValue">public static void $Name$(this $Type$ $argument$)&#xD;
                                        {&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Field/=Name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Field/=Name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Field/=Type/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Field/=Type/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Field/=argument/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=73D7E5D505C60F43BFFE979F74B533B9/Field/=argument/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Shortcut/@EntryValue">ltmPrivateVoid</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Description/@EntryValue">Creates a private void method.</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Text/@EntryValue">private void $name$()&#xD;
                                        {&#xD;
                                        	$END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F3217F4CAB441D40964CA33B888EF1C8/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Shortcut/@EntryValue">ltmPublicVoid</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Description/@EntryValue">Creates a simple public void method.</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Text/@EntryValue">public void $name$()&#xD;
                                        {&#xD;
                                            $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=59CAC0A42753C948B74B481440173FE0/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Shortcut/@EntryValue">ltmYieldReturn</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Text/@EntryValue">public IEnumerable&lt;$Type$&gt; $Name$()&#xD;
                                        {&#xD;
                                            yield return null;&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Field/=Type/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Field/=Type/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Field/=Name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=649F74BC7BBAC644B5EDD9A6B0C7EA29/Field/=Name/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Shortcut/@EntryValue">ltpArgumentNull</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Description/@EntryValue">Checks if a value is null and throws an argument null exception</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Text/@EntryValue">Throw.IfNull(() =&gt; $var$);</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Field/=var/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF16AB460F70BA4991317B3100822E10/Field/=var/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/Shortcut/@EntryValue">ltpCheckAccess</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/Description/@EntryValue">Creates "TcInvoke.CheckAccess(this);".</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/Text/@EntryValue">TcInvoke.CheckAccess(this);</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A703FC64A347694FBEF4851090384535/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Shortcut/@EntryValue">ltpIDisposable</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Description/@EntryValue">The implementation of the IDisposable pattern without base class</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Text/@EntryValue">private bool disposed = false;&#xD;
                                        &#xD;
                                        /// &lt;summary&gt;&#xD;
                                        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.&#xD;
                                        /// &lt;/summary&gt;&#xD;
                                        public void Dispose()&#xD;
                                        {&#xD;
                                            Dispose(true);&#xD;
                                            GC.SuppressFinalize(this);&#xD;
                                        }&#xD;
                                        &#xD;
                                        /// &lt;summary&gt;&#xD;
                                        /// Releases unmanaged and - optionally - managed resources.&#xD;
                                        /// &lt;/summary&gt;&#xD;
                                        /// &lt;param name="disposing"&gt;&lt;c&gt;true&lt;/c&gt; to release both managed and unmanaged resources; &lt;c&gt;false&lt;/c&gt; to release only unmanaged resources.&lt;/param&gt;&#xD;
                                        protected virtual void Dispose(bool disposing)&#xD;
                                        {&#xD;
                                            if(!this.disposed)&#xD;
                                            {&#xD;
                                                if(disposing)&#xD;
                                                {&#xD;
                                                    // Dispose managed resources.&#xD;
                                                }&#xD;
                                        &#xD;
                                        		// Dispose unmanaged resources.&#xD;
                                                // Note disposing has been done.&#xD;
                                                disposed = true;&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        /// &lt;summary&gt;&#xD;
                                        /// Finalizes an instance of the &lt;see cref="$Name$"/&gt; class.&#xD;
                                        /// &lt;/summary&gt;&#xD;
                                        ~$Name$()&#xD;
                                        {&#xD;
                                            Dispose(false);&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Field/=Name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EC05DAC64ADE514AAB23BFD39BF3A45A/Field/=Name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Shortcut/@EntryValue">ltpTcProperty</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Text/@EntryValue">private $Type$ m$Name$;&#xD;
                                        public $Type$ $Name$&#xD;
                                        {&#xD;
                                            get&#xD;
                                            {&#xD;
                                                TcInvoke.CheckAccess(this);&#xD;
                                                return m$Name$;&#xD;
                                            }&#xD;
                                        &#xD;
                                            set&#xD;
                                            {&#xD;
                                                TcInvoke.CheckAccess(this);&#xD;
                                                if (m$Name$.NotEqualsTo(value))&#xD;
                                                {&#xD;
                                                    m$Name$ = value;            &#xD;
                                                    RaisePropertyChanged(() =&gt; $Name$);&#xD;
                                                }&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Field/=Type/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Field/=Type/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Field/=Name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C709908B0FEE5D41B22548853DE80FFF/Field/=Name/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/Shortcut/@EntryValue">lttcConstructorTest</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/Text/@EntryValue">/// &lt;summary&gt;&#xD;
                                        /// Represents the constructor unit tests for the type &lt;see cref="$type$"/&gt;.&#xD;
                                        /// &lt;/summary&gt;&#xD;
                                        [TestClass]&#xD;
                                        public class ConstructorTest&#xD;
                                        {&#xD;
                                        	[TestInitialize]&#xD;
                                            public void TestInitialize()&#xD;
                                            {&#xD;
                                                $END$&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/Field/=type/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F32E54477A75E6419F7CA0843995E644/Field/=type/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/Shortcut/@EntryValue">lttcMarkupExtensionConverter</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/Text/@EntryValue">[ValueConversion(typeof(object), typeof(object))]&#xD;
                                        public class  $classname$ : TcMarkupExtensionConverter&lt; $classname$&gt;&#xD;
                                        {&#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Converts a value.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            /// &lt;param name="value"&gt;The value produced by the binding source.&lt;/param&gt;&#xD;
                                            /// &lt;param name="targetType"&gt;The type of the binding target property.&lt;/param&gt;&#xD;
                                            /// &lt;param name="parameter"&gt;The converter parameter to use.&lt;/param&gt;&#xD;
                                            /// &lt;param name="culture"&gt;The culture to use in the converter.&lt;/param&gt;&#xD;
                                            /// &lt;returns&gt;&#xD;
                                            /// The converted value. If the method returns &lt;c&gt;null&lt;/c&gt;, the valid &lt;c&gt;null&lt;/c&gt; value is used.&#xD;
                                            /// &lt;/returns&gt;&#xD;
                                            public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)&#xD;
                                            {&#xD;
                                                if (!ValidateValue(value))&#xD;
                                                {&#xD;
                                                    return null;&#xD;
                                                }&#xD;
                                        &#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        &#xD;
                                            /// &lt;summary&gt;&#xD;
                                            /// Converts the value back.&#xD;
                                            /// &lt;/summary&gt;&#xD;
                                            /// &lt;param name="value"&gt;The value produced by changes on the view.&lt;/param&gt;&#xD;
                                            /// &lt;param name="targetType"&gt;The type of the binding target property.&lt;/param&gt;&#xD;
                                            /// &lt;param name="parameter"&gt;The converter parameter to use.&lt;/param&gt;&#xD;
                                            /// &lt;param name="culture"&gt;The culture to use in the converter.&lt;/param&gt;&#xD;
                                            /// &lt;returns&gt;The back converted value..&lt;/returns&gt;&#xD;
                                            public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/Field/=classname/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=29D6506C85934E43937FAB6D5C5E2E05/Field/=classname/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/Shortcut/@EntryValue">lttcSimple</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/Text/@EntryValue">/// &lt;summary&gt;&#xD;
                                        /// Represents the unit test for the &lt;see cref="$ClassToTest$"/&gt;&#xD;
                                        /// &lt;/summary&gt;&#xD;
                                        [TestClass]&#xD;
                                        public class $ClassToTest$Test&#xD;
                                        {&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/Field/=ClassToTest/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3037EC7D823A9E4EA837AC6CAEA27995/Field/=ClassToTest/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Shortcut/@EntryValue">lttcSimpleMethodOrProperty</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Text/@EntryValue">/// &lt;summary&gt;&#xD;
                                        /// Represents the unit test for the &lt;see cref="$type$.$methodOrProperty$"/&gt;&#xD;
                                        /// &lt;/summary&gt;&#xD;
                                        [TestClass]&#xD;
                                        public class Tc$methodOrProperty$Test&#xD;
                                        {&#xD;
                                        	$END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Field/=type/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Field/=type/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Field/=methodOrProperty/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=CF6B7B5845BBE348BA2B5F6779EF3876/Field/=methodOrProperty/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Shortcut/@EntryValue">lttcSingleThreadedInvokable</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Description/@EntryValue">Creates a test class with TcInvokeTestClassBase as base class and uses TcSingleThread to run everything synchroniously.</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Text/@EntryValue">/// &lt;summary&gt;&#xD;
                                        /// Represents the unit test for the &lt;see cref="$class$"/&gt;&#xD;
                                        /// &lt;/summary&gt;&#xD;
                                        [TestClass]&#xD;
                                        public class $class$Test : TcInvokeTestClassSingleThreadedBase&#xD;
                                        {&#xD;
                                            private $type$ mTestObject;&#xD;
                                        &#xD;
                                            public override void OnTestInitialize()&#xD;
                                            {&#xD;
                                                base.OnTestInitialize();&#xD;
                                        		&#xD;
                                                mTestObject = new $type$($END$);&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Field/=class/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Field/=class/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Field/=type/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FCC76572B2173B43A0330207029B2C30/Field/=type/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Shortcut/@EntryValue">lttcSingleThreadedMethodOrPropertyInvokable</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Description/@EntryValue">Creates a test class with TcInvokeTestClassBase as super class</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Text/@EntryValue">/// &lt;summary&gt;&#xD;
                                        /// Represents the unit test for the &lt;see cref="$class$.$methodOrProperty$"/&gt;&#xD;
                                        /// &lt;/summary&gt;&#xD;
                                        [TestClass]&#xD;
                                        public class Tc$methodOrProperty$Test : TcInvokeTestClassSingleThreadedBase&#xD;
                                        {&#xD;
                                            private $class$ mTestObject;&#xD;
                                        &#xD;
                                            public override void OnTestInitialize()&#xD;
                                            {&#xD;
                                                base.OnTestInitialize();&#xD;
                                        &#xD;
                                                mTestObject = new $class$($END$);&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Field/=class/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Field/=class/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Field/=methodOrProperty/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F23ED90C98DFE246A012442C0886897E/Field/=methodOrProperty/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/Shortcut/@EntryValue">lttmConstructor</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/Description/@EntryValue">Template for argument null check of constructor</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/Text/@EntryValue">[TestMethod]&#xD;
                                        [TestCategory(TcTestCategory.UNIT_TEST)]&#xD;
                                        public void WhenConstructed_ThenNullParameterShouldThrowAnException()&#xD;
                                        {&#xD;
                                            TcAssert&lt;$ClassToTest$&gt;.IfNull();&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/Field/=ClassToTest/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=422919F8B7BEC24CADB06E7EF87CDD76/Field/=ClassToTest/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Shortcut/@EntryValue">lttmConstructorPropertyCheck</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Description/@EntryValue">Checks if a property is correctly set, when it is injected via the constructor.</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Text/@EntryValue">[TestMethod]&#xD;
                                        [TestCategory(TcTestCategory.UNIT_TEST)]&#xD;
                                        public void WhenConstructed_Then$PropertyName$ShouldBeSetCorrectly()&#xD;
                                        {&#xD;
                                            Assert.AreEqual(m$PropertyName$, mTestObject.$PropertyName$);&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Field/=PropertyName/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Field/=PropertyName/Expression/@EntryValue">completeSmart()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Field/=PropertyName/InitialRange/@EntryValue">2</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=18EB2395B4D858458DE716CB0F2442F6/Field/=PropertyName/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Shortcut/@EntryValue">lttmMethodArgumentChecking</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Description/@EntryValue">Creates a method to check one argument for an ArgumentNullException</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Text/@EntryValue">[TestMethod]&#xD;
                                        [TestCategory(TcTestCategory.UNIT_TEST)]&#xD;
                                        public void When$method$CalledWith$argumentNameUpper$Null_ThenArgumentNullExceptionShouldBeThrown()&#xD;
                                        {&#xD;
                                            TcAssert.ThrowsArg&lt;ArgumentNullException&gt;(() =&gt; mTestObject.$method$($END$), "$argumentName$");&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Field/=method/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Field/=method/Expression/@EntryValue">completeSmart()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Field/=method/InitialRange/@EntryValue">1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Field/=method/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Field/=argumentName/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Field/=argumentName/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Field/=argumentNameUpper/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Field/=argumentNameUpper/Expression/@EntryValue">capitalize(argumentName)</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Field/=argumentNameUpper/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5C8386DDFFB8428911FFA8BCF2B0B5/Field/=argumentNameUpper/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/Shortcut/@EntryValue">lttmProjectTest</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/Text/@EntryValue">[TestMethod]&#xD;
                                        [TestCategory(TcTestCategory.NON_FUNCTIONAL_TEST)]&#xD;
                                        public void $Name$()&#xD;
                                        {    &#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/Field/=Name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=22AB3E01C15BC54DBDE766795E9469E3/Field/=Name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/Shortcut/@EntryValue">lttmPropertyBinderAllProperties</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/Description/@EntryValue">Tests all bindable properties of the test object for raise property changed, value changing, and not multiple raise property changes if value was set twice with same value.</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/Text/@EntryValue">[TestMethod]&#xD;
                                        [TestCategory(TcTestCategory.UNIT_TEST)]&#xD;
                                        public void WhenClassImplementNotifyPropertyChanged_ThenAllPropertiesHaveToFollowTheContract()&#xD;
                                        {&#xD;
                                            mTestObject.AssertAllBindableProperties();&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2AAF3E26A1E39E43BCB6847022672504/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Shortcut/@EntryValue">lttmSimpleBlackBox</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Description/@EntryValue">Creates a simple black box test</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Text/@EntryValue">[TestMethod]&#xD;
                                        [TestCategory(TcTestCategory.UNIT_TEST)]&#xD;
                                        public void When$This$_Then$That$()&#xD;
                                        {&#xD;
                                            $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Field/=This/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Field/=This/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Field/=That/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DDAFDA622F177B41B7F8DC4E85B4A49C/Field/=That/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/Shortcut/@EntryValue">lttmTestCleanup</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/Description/@EntryValue">Creates a method for cleaning up the tests</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/Text/@EntryValue">[TestCleanup]&#xD;
                                        public void TestCleanup()&#xD;
                                        {&#xD;
                                        	$END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8B7777A92FB6224D9F5D285D8F2E0297/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/Shortcut/@EntryValue">lttmTestInitialize</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/Description/@EntryValue">Creates a method to initialize a test.</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/Text/@EntryValue">[TestInitialize]&#xD;
                                        public void TestInitialize()&#xD;
                                        {&#xD;
                                        	$END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B68587E1C9BD64293BD589FE393DFD4/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Shortcut/@EntryValue">lttmTestInitializeConnector</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Description/@EntryValue">Create the initialize method for a connector test with the needed dependencies as mocks</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Text/@EntryValue">private $TestClass$ mTestObject;&#xD;
                                        private TiRemoteObjectWrapper&lt;$RemoteObject$&gt; mRemoteObjectWrapper;&#xD;
                                        private TiGlobalFactoryService mGlobalFactoryService;&#xD;
                                        private TiArgumentService mArgumentService;&#xD;
                                        private TiLogService mLogService;&#xD;
                                        &#xD;
                                        [TestInitialize]&#xD;
                                        public void TestInitialize()&#xD;
                                        {&#xD;
                                            mRemoteObjectWrapper = TcObjectCreator.Create&lt;TiRemoteObjectWrapper&lt;$RemoteObject$&gt;&gt;();&#xD;
                                            mGlobalFactoryService = TcObjectCreator.Create&lt;TiGlobalFactoryService&gt;();&#xD;
                                            mArgumentService = TcObjectCreator.Create&lt;TiArgumentService&gt;();&#xD;
                                            mLogService = TcObjectCreator.Create&lt;TiLogService&gt;();&#xD;
                                        &#xD;
                                            mTestObject = new $TestClass$(mRemoteObjectWrapper, mGlobalFactoryService, mArgumentService, mLogService);&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Field/=TestClass/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Field/=TestClass/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Field/=RemoteObject/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1044CC8FEC3A82478EB44FB3E218B818/Field/=RemoteObject/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/Shortcut/@EntryValue">lttpObjectCreateor</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/Text/@EntryValue">TcObjectCreator.Create&lt;$Type$&gt;()$END$</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/Field/=Type/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4649E931A5F3CE44BDB5F4CA841FC570/Field/=Type/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Shortcut/@EntryValue">lttpSubstitute</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Description/@EntryValue">Creates a new Substitute</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Text/@EntryValue">Substitute.For&lt;Ti$interface$&gt;()$END$</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Field/=interface/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=0AED3F61BFEE324CA986BA71E9DFCDD3/Field/=interface/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/Shortcut/@EntryValue">lttpSuppressionNSubstituteAssert</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/Description/@EntryValue">Crates a suppression for an assert made with NSubstitute.</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/Text/@EntryValue">[SuppressMessage("Trumpf.Usage", "TCA2205:TcTCA2205", Justification = "NSubstitue assert")]</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/Categories/=Trumpf_0020Snippets/@EntryIndexedValue">Trumpf Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C463BD00A677A24280C869ED94D1F354/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Shortcut/@EntryValue">mbox</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Description/@EntryValue">MessageBox.Show</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Text/@EntryValue">System.Windows.Forms.MessageBox.Show("$string$");</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Field/=string/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Field/=string/Expression/@EntryValue">constant("Test")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=11077A8664583047B96B3E6DAEFC3728/Field/=string/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/Shortcut/@EntryValue">middleware</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/Text/@EntryValue">public static class Add$name$Extension&#xD;
                                        {&#xD;
                                            public static void Add$name$(this IServiceCollection services)&#xD;
                                            {       &#xD;
                                                services.AddSingletonIfNotExists&lt;$name$&gt;();&#xD;
                                            }&#xD;
                                        	&#xD;
                                        	public static void Use$name$(this IApplicationBuilder applicationBuilder)&#xD;
                                        	{&#xD;
                                        		applicationBuilder.UseMiddleware&lt;$name$&gt;();&#xD;
                                        	}&#xD;
                                        }&#xD;
                                        &#xD;
                                        internal sealed class $name$ : IMiddleware&#xD;
                                        {    &#xD;
                                            public async Task InvokeAsync(HttpContext context, RequestDelegate next)&#xD;
                                            {      &#xD;
                                        		await next(context).ConfigureAwait(false);       &#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DEE0B1B74081024485601BADDE06E5C6/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=64979CDD891063408DE818D69D0DF83D/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=64979CDD891063408DE818D69D0DF83D/Shortcut/@EntryValue">mstestbase-aspnet</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=64979CDD891063408DE818D69D0DF83D/Text/@EntryValue">[TestClass]&#xD;
                                        public abstract class MsTestBase : IntegrationTestBase&lt;Startup&gt;&#xD;
                                        {&#xD;
                                            [AssemblyInitialize]&#xD;
                                            public static Task Init(TestContext testContext)&#xD;
                                            {&#xD;
                                                return AssemblyInitializeAsync(testContext, (services, configuration) =&gt;&#xD;
                                                {&#xD;
                                                    services.AddMediator();&#xD;
                                                });&#xD;
                                            }&#xD;
                                        &#xD;
                                            [AssemblyCleanup]&#xD;
                                            public static void Cleanup() =&gt; AssemblyCleanup();&#xD;
                                        }&#xD;
                                        &#xD;
                                        public abstract class IntegrationTestBase&lt;TStartup&gt; where TStartup : class&#xD;
                                        {&#xD;
                                            // We use assembly initialize to get better performance. But it is only possible if you change not states on the base&#xD;
                                            // properties, otherwise we have to change to to clean up every test.&#xD;
                                            //&#xD;
                                            // Hint: With this 'AssemblyInitialize' the API is just started once.&#xD;
                                            public static Task AssemblyInitializeAsync(TestContext testContext)&#xD;
                                            {&#xD;
                                                return AssemblyInitializeAsync(testContext, (_, _) =&gt; { });&#xD;
                                            }&#xD;
                                        &#xD;
                                            public static Task AssemblyInitializeAsync(TestContext testContext, &#xD;
                                                                                        Action&lt;IServiceCollection, IConfiguration&gt; registerServices)&#xD;
                                            {&#xD;
                                                // Create this with new, is not a fault, the reason is to keep the test class more cleaner.&#xD;
                                                WebApplicationFactory = new CustomWebApplicationFactory&lt;TStartup&gt;(registerServices);&#xD;
                                                ServiceProvider = WebApplicationFactory.Services;&#xD;
                                                Client = WebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });&#xD;
                                        &#xD;
                                                JsonSerializer = WebApplicationFactory.Services.GetService&lt;IJsonSerializer&gt;()!;&#xD;
                                        &#xD;
                                                Mediator = ServiceProvider.GetOrThrowMissingException&lt;IMediator&gt;();&#xD;
                                        &#xD;
                                                UniqueRunnerName = System.Environment.MachineName;&#xD;
                                        &#xD;
                                                return Task.CompletedTask;&#xD;
                                            }&#xD;
                                        &#xD;
                                            public static CustomWebApplicationFactory&lt;TStartup&gt; WebApplicationFactory { get; set; } = null!;&#xD;
                                        &#xD;
                                            public static IServiceProvider ServiceProvider { get; private set; } = null!;&#xD;
                                        &#xD;
                                            public static IJsonSerializer JsonSerializer { get; private set; } = null!;&#xD;
                                        &#xD;
                                            protected static string UniqueRunnerName { get; private set; } = string.Empty;&#xD;
                                        &#xD;
                                            protected static IMediator Mediator { get; private set; } = null!;&#xD;
                                        &#xD;
                                            protected static HttpClient Client { get; private set; } = null!;&#xD;
                                        &#xD;
                                            [AssemblyCleanup]&#xD;
                                            public static void AssemblyCleanup()&#xD;
                                            {&#xD;
                                                WebApplicationFactory?.Dispose();&#xD;
                                                Client?.Dispose();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        public class CustomWebApplicationFactory&lt;TStartup&gt; : WebApplicationFactory&lt;TStartup&gt; where TStartup : class&#xD;
                                        {&#xD;
                                            private readonly Action&lt;IServiceCollection, IConfiguration&gt; _registerServices;&#xD;
                                        &#xD;
                                            public CustomWebApplicationFactory() : this((_, _) =&gt; { })&#xD;
                                            {&#xD;
                                            }&#xD;
                                        &#xD;
                                            public CustomWebApplicationFactory(Action&lt;IServiceCollection, IConfiguration&gt; registerServices)&#xD;
                                            {&#xD;
                                                _registerServices = registerServices;&#xD;
                                            }&#xD;
                                        &#xD;
                                            protected override void ConfigureWebHost(IWebHostBuilder builder)&#xD;
                                            {&#xD;
                                                var testSettingsFileInfo = new FileInfo(Path.Combine(System.Environment.CurrentDirectory, "appsettings.test.json"));&#xD;
                                                IConfiguration configuration = null!;&#xD;
                                        &#xD;
                                                builder.ConfigureAppConfiguration((_, configurationBuilder) =&gt;&#xD;
                                                {&#xD;
                                                    if (testSettingsFileInfo.Exists)&#xD;
                                                    {&#xD;
                                                        configurationBuilder.AddJsonFile(testSettingsFileInfo.FullName);&#xD;
                                                    }&#xD;
                                        &#xD;
                                                    configuration = configurationBuilder.Build();&#xD;
                                                });&#xD;
                                        &#xD;
                                        &#xD;
                                                builder.ConfigureServices(services =&gt;&#xD;
                                                {&#xD;
                                                    _registerServices(services, configuration);&#xD;
                                                    // if we need to switch between services we have to do it here&#xD;
                                                });&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=64979CDD891063408DE818D69D0DF83D/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=64979CDD891063408DE818D69D0DF83D/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=64979CDD891063408DE818D69D0DF83D/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=64979CDD891063408DE818D69D0DF83D/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=64979CDD891063408DE818D69D0DF83D/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=64979CDD891063408DE818D69D0DF83D/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Shortcut/@EntryValue">namespace</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Text/@EntryValue">namespace $name$&#xD;
                                          {&#xD;
                                             $END$$SELECTION$&#xD;
                                          }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Scope/=558F05AA0DE96347816FF785232CFB2A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Scope/=558F05AA0DE96347816FF785232CFB2A/Type/@EntryValue">InCSharpTypeAndNamespace</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Scope/=558F05AA0DE96347816FF785232CFB2A/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Field/=name/Expression/@EntryValue">constant("MyNamespace")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=D44BEB0055F8C24EBBAC00647EAD4244/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Shortcut/@EntryValue">newtestassertpost</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Text/@EntryValue">[TestClass]&#xD;
                                        public class $TestName$ : MsTestBase&#xD;
                                        {&#xD;
                                        	[TestMethod]&#xD;
                                        	public Task $TestMethod$()&#xD;
                                        	{&#xD;
                                        		return Client.AssertPostAsync&lt;$TResponse$&gt;("$url$",&#xD;
                                        												  "$payload$",&#xD;
                                        												  "$result$");&#xD;
                                        	}&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=TestName/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=TestName/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=TestMethod/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=TestMethod/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=url/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=url/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=payload/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=payload/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=result/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=result/Order/@EntryValue">4</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=TResponse/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=143441FE0A2A3048B3F0413E905034BB/Field/=TResponse/Order/@EntryValue">5</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/Shortcut/@EntryValue">nguid</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/Description/@EntryValue">Insert new GUID</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/Text/@EntryValue">$GUID$</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/Field/=GUID/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/Field/=GUID/Expression/@EntryValue">guid()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2B42B0BD9B6C0845B914AC7EF31DDCE1/Field/=GUID/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=49EFEFF1E87A4342AD31493835937C8B/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=49EFEFF1E87A4342AD31493835937C8B/Shortcut/@EntryValue">notification</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=49EFEFF1E87A4342AD31493835937C8B/Text/@EntryValue">public record $notification$() : INotification;&#xD;
                                        &#xD;
                                        internal class $notification$Handler : INotificationHandler&lt;$notification$&gt;&#xD;
                                        {&#xD;
                                            public Task Handle($notification$ notification, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=49EFEFF1E87A4342AD31493835937C8B/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=49EFEFF1E87A4342AD31493835937C8B/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=49EFEFF1E87A4342AD31493835937C8B/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=49EFEFF1E87A4342AD31493835937C8B/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=49EFEFF1E87A4342AD31493835937C8B/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=49EFEFF1E87A4342AD31493835937C8B/Field/=notification/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=49EFEFF1E87A4342AD31493835937C8B/Field/=notification/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Shortcut/@EntryValue">oauth0getall</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Text/@EntryValue">[HttpGet]&#xD;
                                        [OAuth2Scope("$service$.$entitiylowercase$.write")]&#xD;
                                        [ProducesResponseType(typeof(Add$entityuppercase$Response), StatusCodes.Status200OK)]&#xD;
                                        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                        [SwaggerOperation(OperationId = "get-all-$entitiylowercase$", Tags = new[] {"$entityuppercase$"})]&#xD;
                                        public Task&lt;GetAll$entityuppercase$Response&gt; GetOpportunityAsync($entityuppercase$ $entitiylowercase$)&#xD;
                                        {&#xD;
                                            return _mediator.SendAsync(new GetAll$entityuppercase$($entitiylowercase$));&#xD;
                                        }&#xD;
                                        &#xD;
                                        internal record GetAll$entityuppercase$($entityuppercase$ $entityuppercase$) : IQuery&lt;GetAll$entityuppercase$Response&gt;;&#xD;
                                        &#xD;
                                        internal class GetAll$entityuppercase$Handler : IQueryHandler&lt;GetAll$entityuppercase$, GetAll$entityuppercase$Response&gt;&#xD;
                                        {&#xD;
                                        	public Task&lt;GetAll$entityuppercase$Response&gt; Handle(Get$entityuppercase$ request, CancellationToken cancellationToken)&#xD;
                                        	{&#xD;
                                        		throw new NotImplementedException();&#xD;
                                        	}&#xD;
                                        }&#xD;
                                        </s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Field/=service/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Field/=service/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Field/=entitiylowercase/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Field/=entitiylowercase/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Field/=entityuppercase/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B7510D9F0A07614F89194D45AD5D0497/Field/=entityuppercase/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Shortcut/@EntryValue">oauth0post</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Text/@EntryValue">[HttpPost]&#xD;
                                        [OAuth2Scope("$service$.$entitiylowercase$.write")]&#xD;
                                        [ProducesResponseType(typeof(Add$entityuppercase$Response), StatusCodes.Status200OK)]&#xD;
                                        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                        [SwaggerOperation(OperationId = "add-$entitiylowercase$", Tags = new[] {"$entityuppercase$"})]&#xD;
                                        public Task&lt;Add$entityuppercase$Response&gt; AddOpportunityAsync($entityuppercase$ $entitiylowercase$)&#xD;
                                        {&#xD;
                                            return _mediator.SendAsync(new Add$entityuppercase$($entitiylowercase$));&#xD;
                                        }&#xD;
                                        &#xD;
                                        internal record Add$entityuppercase$($entityuppercase$ $entityuppercase$) : ICommand&lt;Add$entityuppercase$Response&gt;;&#xD;
                                        &#xD;
                                        internal class Add$entityuppercase$Handler : ICommandHandler&lt;Add$entityuppercase$, Add$entityuppercase$Response&gt;&#xD;
                                        {&#xD;
                                        	public Task&lt;Add$entityuppercase$Response&gt; Handle(Add$entityuppercase$ request, CancellationToken cancellationToken)&#xD;
                                        	{&#xD;
                                        		throw new NotImplementedException();&#xD;
                                        	}&#xD;
                                        }&#xD;
                                        </s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Field/=service/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Field/=service/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Field/=entitiylowercase/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Field/=entitiylowercase/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Field/=entityuppercase/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=53C669725D8E854AB7F53198B16EFDC7/Field/=entityuppercase/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Shortcut/@EntryValue">oauthrestcontroller</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Text/@EntryValue">[Authorize]&#xD;
                                        [ApiController]&#xD;
                                        [ApiVersion("1.0")]&#xD;
                                        [Route("$baseurl$")]&#xD;
                                        public class OpportunitiesController : ControllerBase&#xD;
                                        {&#xD;
                                        	private readonly IMediator _mediator;&#xD;
                                        &#xD;
                                        	public OpportunitiesController(IMediator mediator)&#xD;
                                        	{&#xD;
                                        		_mediator = mediator;&#xD;
                                        	}&#xD;
                                        &#xD;
                                        	[HttpPost]&#xD;
                                        	[OAuth2Scope("$service$.$entitylower$.write")]&#xD;
                                        	[ProducesResponseType(typeof(Add$entity$Response), StatusCodes.Status200OK)]&#xD;
                                        	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                        	[SwaggerOperation(OperationId = "add-$entity$", Tags = new[] {"$entity$"})]&#xD;
                                        	public Task&lt;Add$entity$Response&gt; Add$entity$Async($entitylower$ $entitylower$)&#xD;
                                        	{&#xD;
                                        		return _mediator.SendAsync(new Add$entity$($entitylower$));&#xD;
                                        	}&#xD;
                                        &#xD;
                                        &#xD;
                                        	[HttpGet]&#xD;
                                        	[OAuth2Scope("$service$.$entity$.read")]&#xD;
                                        	[ProducesResponseType(typeof(GetAll$entity$Response), StatusCodes.Status200OK)]&#xD;
                                        	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                        	[SwaggerOperation(OperationId = "get-all-opportunities", Tags = new[] {"$entity$"})]&#xD;
                                        	public Task&lt;GetAll$entity$Response&gt; GetAll$entity$sAsync()&#xD;
                                        	{&#xD;
                                        		return _mediator.SendAsync(new GetAll$entity$s($entitylower$));&#xD;
                                        	}&#xD;
                                        &#xD;
                                        	[HttpGet("{idOrNumber}")]&#xD;
                                        	[OAuth2Scope("$service$.$entity$.read")]&#xD;
                                        	[ProducesResponseType(typeof(Get$entity$ByIdOrNumberResponse), StatusCodes.Status200OK)]&#xD;
                                        	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                        	[SwaggerOperation(OperationId = "get-opportunity-line-detail-by-id", Tags = new[] {"$entity$"})]&#xD;
                                        	public Task&lt;Get$entity$ByIdOrNumberResponse&gt; GetOpportunityByIdOrNumberAsync(string id)&#xD;
                                        	{&#xD;
                                        		return _mediator.SendAsync(new Get$entity$ByIdOrNumber(partner, idOrNumber));&#xD;
                                        	}&#xD;
                                        &#xD;
                                        	[HttpDelete("{idOrNumber}")]&#xD;
                                        	[OAuth2Scope("$service$.$entity$.delete")]&#xD;
                                        	[ProducesResponseType(StatusCodes.Status204NoContent)]&#xD;
                                        	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                        	[SwaggerOperation(OperationId = "delete-$entity$-by-id", Tags = new[] {"$entity$"})]&#xD;
                                        	public async Task&lt;ActionResult&gt; DeleteOpportunityByIdOrNumberAsync(string id)&#xD;
                                        	{&#xD;
                                        		await _mediator.SendAsync(new Delete$entity$ById(partner, idOrNumber)).ConfigureAwait(false);&#xD;
                                        		return NoContent();&#xD;
                                        	}&#xD;
                                        &#xD;
                                        	[HttpDelete]&#xD;
                                        	[OAuth2Scope("$service$.$entity$.delete")]&#xD;
                                        	[ProducesResponseType(typeof(DeleteAllOpportunitiesResponse), StatusCodes.Status200OK)]&#xD;
                                        	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                        	[SwaggerOperation(OperationId = "delete-all-$entity$", Tags = new[] {"$entity$"})]&#xD;
                                        	public Task&lt;DeleteAllOpportunitiesResponse&gt; DeleteAllOpportunitiesAsync(string partner,&#xD;
                                        																			[FromQuery] string? idOrNumber = null)&#xD;
                                        	{&#xD;
                                        		return _mediator.SendAsync(new DeleteAll$entity$(partner, idOrNumber));&#xD;
                                        	}&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Field/=baseurl/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Field/=baseurl/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Field/=entity/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Field/=entity/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Field/=service/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Field/=service/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Field/=entitylower/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6A93F98DB9A422438F749BE6D10EE6EA/Field/=entitylower/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/Shortcut/@EntryValue">out</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/Description/@EntryValue">Print a string</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/Text/@EntryValue">System.Console.Out.WriteLine("$END$");</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FB8272F0E1523D40A10C8F64D194AD27/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Shortcut/@EntryValue">outv</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Description/@EntryValue">Print value of a variable</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Text/@EntryValue">System.Console.Out.WriteLine("$EXPR$ = {0}", $EXPR$);</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Field/=EXPR/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Field/=EXPR/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Field/=EXPR/InitialRange/@EntryValue">1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=EB0F6A6BA9A1C043B6B7EE5650B0B5AF/Field/=EXPR/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/Shortcut/@EntryValue">pci</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/Description/@EntryValue">public const int</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/Text/@EntryValue">public const int </s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4E153B2F4FF57A4E879BAA0AD7590820/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/Shortcut/@EntryValue">pcs</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/Description/@EntryValue">public const string</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/Text/@EntryValue">public const string </s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4AC3A7B66C008649B9CBF5C3048DDE4D/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/Shortcut/@EntryValue">pipelinebehavior</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/Text/@EntryValue">public static class Add$name$BehaviorExtension&#xD;
                                        {&#xD;
                                            public static void Add$name$Behavior(this IServiceCollection services)&#xD;
                                            {&#xD;
                                                services.AddTransient(typeof(IPipelineBehavior&lt;,&gt;), typeof($name$Behavior&lt;,&gt;));&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        &#xD;
                                        public class $name$Behavior&lt;TRequest, TResponse&gt; : IPipelineBehavior&lt;TRequest, TResponse&gt;  where TRequest : notnull&#xD;
                                        {&#xD;
                                            public Task&lt;TResponse&gt; Handle(TRequest request, RequestHandlerDelegate&lt;TResponse&gt; next, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                return next();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B5ED4A77E881D043AEDC3FF3CE40B3A0/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5211A76AB7DD48A0068C88F19E1D1F/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5211A76AB7DD48A0068C88F19E1D1F/Shortcut/@EntryValue">postresponsetypes</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5211A76AB7DD48A0068C88F19E1D1F/Text/@EntryValue">[ProducesResponseType(StatusCodes.Status201Created)]&#xD;
                                        [ProducesResponseType(StatusCodes.Status400BadRequest)]</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5211A76AB7DD48A0068C88F19E1D1F/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5211A76AB7DD48A0068C88F19E1D1F/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5211A76AB7DD48A0068C88F19E1D1F/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5211A76AB7DD48A0068C88F19E1D1F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5211A76AB7DD48A0068C88F19E1D1F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BE5211A76AB7DD48A0068C88F19E1D1F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=16861334B761764088F33B50F63783EF/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=16861334B761764088F33B50F63783EF/Shortcut/@EntryValue">producejson</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=16861334B761764088F33B50F63783EF/Text/@EntryValue">[Produces(MediaTypeNames.Application.Json)]</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=16861334B761764088F33B50F63783EF/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=16861334B761764088F33B50F63783EF/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=16861334B761764088F33B50F63783EF/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=16861334B761764088F33B50F63783EF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=16861334B761764088F33B50F63783EF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=16861334B761764088F33B50F63783EF/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F130E381C42AC447963CD2C0CD9FE431/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F130E381C42AC447963CD2C0CD9FE431/Shortcut/@EntryValue">program.cs</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F130E381C42AC447963CD2C0CD9FE431/Text/@EntryValue">namespace $namespace$&#xD;
                                        {&#xD;
                                            public static class Program&#xD;
                                            {&#xD;
                                                public static void Main(string[] args)&#xD;
                                                {&#xD;
                                                    CreateHostBuilder(args).Build().Run();&#xD;
                                                }&#xD;
                                        &#xD;
                                                public static IHostBuilder CreateHostBuilder(string[] args)&#xD;
                                                {&#xD;
                                                    return Host.CreateDefaultBuilder(args)&#xD;
                                                               .ConfigureWebHostDefaults(webBuilder =&gt; { webBuilder.UseStartup&lt;Startup&gt;(); });&#xD;
                                                }&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F130E381C42AC447963CD2C0CD9FE431/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F130E381C42AC447963CD2C0CD9FE431/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F130E381C42AC447963CD2C0CD9FE431/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F130E381C42AC447963CD2C0CD9FE431/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F130E381C42AC447963CD2C0CD9FE431/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F130E381C42AC447963CD2C0CD9FE431/Field/=namespace/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F130E381C42AC447963CD2C0CD9FE431/Field/=namespace/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Shortcut/@EntryValue">prop</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Description/@EntryValue">Property</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Text/@EntryValue">public $TYPE$ $NAME$ { get; set; }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Field/=TYPE/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Field/=TYPE/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Field/=NAME/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Field/=NAME/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=50C1B3A73748EC4A9E8A9C79858FCCF4/Field/=NAME/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Shortcut/@EntryValue">propg</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Description/@EntryValue">Property with a 'get' accessor and a private 'set' accessor</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Text/@EntryValue">public $type$ $property$ { get; private set; }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Field/=type/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Field/=type/Expression/@EntryValue">constant("int")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Field/=type/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Field/=property/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Field/=property/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=93F98A2471C8E14B981B66D087BF011A/Field/=property/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/Shortcut/@EntryValue">psr</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/Description/@EntryValue">public static readonly</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/Text/@EntryValue">public static readonly </s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=8A131857A9840044859C2CF990D19AC5/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/Shortcut/@EntryValue">psvm</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/Description/@EntryValue">The "Main" method declaration</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/Text/@EntryValue">public static void Main( string[] args )
                                        {
                                          $END$
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F43AC10E6088D842B8077AD12A18E35C/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Shortcut/@EntryValue">pulse-rest-controller</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Text/@EntryValue">[Authorize]&#xD;
                                        [ApiController]&#xD;
                                        [ApiVersion("1.0")]&#xD;
                                        [Route("v{version:apiVersion}/$path$")]&#xD;
                                        [ApiExplorerSettings(GroupName = "$groupName$")]&#xD;
                                        public class $ResourceName$Controller : ControllerBase&#xD;
                                        {&#xD;
                                            private readonly IMediator _mediator;&#xD;
                                        &#xD;
                                            public $ResourceName$Controller(IMediator mediator)&#xD;
                                            {&#xD;
                                                _mediator = mediator;&#xD;
                                            }&#xD;
                                        &#xD;
                                            [HttpPost]&#xD;
                                            [SwaggerOperation(OperationId = "add$ResourceName$")]&#xD;
                                            [ProducesResponseType(typeof(Add$ResourceName$Response), StatusCodes.Status200OK)]&#xD;
                                            [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                            [AuthorizeUser(UserAuthorizationType.AdminPrivilege)]&#xD;
                                            public Task&lt;Add$ResourceName$Response&gt; Add$ResourceName$Async($ResourceName$ $resourceName$,&#xD;
                                        																  CancellationToken cancellationToken = default)&#xD;
                                            {&#xD;
                                                return _mediator.SendAsync(new Add$ResourceName$($resourceName$), cancellationToken);&#xD;
                                            }&#xD;
                                        &#xD;
                                            [HttpGet]&#xD;
                                            [SwaggerOperation(OperationId = "getAll$ResourceName$s")]&#xD;
                                            [ProducesResponseType(typeof(GetAll$ResourceName$sResponse), StatusCodes.Status200OK)]&#xD;
                                            [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                            [AuthorizeUser(UserAuthorizationType.AdminPrivilege)]&#xD;
                                            public Task&lt;GetAll$ResourceName$sResponse&gt; GetAll$ResourceName$sAsync(CancellationToken cancellationToken = default)&#xD;
                                            {&#xD;
                                                return _mediator.SendAsync(new GetAll$ResourceName$s(), cancellationToken);&#xD;
                                            }&#xD;
                                        &#xD;
                                            [HttpGet("{id}")]&#xD;
                                            [SwaggerOperation(OperationId = "get$ResourceName$ById")]&#xD;
                                            [ProducesResponseType(typeof(Get$ResourceName$ByIdResponse), StatusCodes.Status200OK)]&#xD;
                                            [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                            [AuthorizeUser(UserAuthorizationType.AdminPrivilege)]&#xD;
                                            public Task&lt;Get$ResourceName$ByIdResponse&gt; Get$ResourceName$ByIdOrNumber(long id,&#xD;
                                        																			 CancellationToken cancellationToken = default)&#xD;
                                            {&#xD;
                                                return _mediator.SendAsync(new Get$ResourceName$ById(id), cancellationToken);&#xD;
                                            }&#xD;
                                        &#xD;
                                            [HttpDelete]&#xD;
                                            [SwaggerOperation(OperationId = "deleteAll$ResourceName$s")]&#xD;
                                            [ProducesResponseType(StatusCodes.Status204NoContent)]&#xD;
                                            [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                            [AuthorizeUser(UserAuthorizationType.AdminPrivilege)]&#xD;
                                            public async Task&lt;IActionResult&gt; DeleteAll$ResourceName$sAsync(CancellationToken cancellationToken = default)&#xD;
                                            {&#xD;
                                                await _mediator.SendAsync(new DeleteAll$ResourceName$s(), cancellationToken).ConfigureAwait(false);&#xD;
                                                return NoContent();&#xD;
                                            }&#xD;
                                        &#xD;
                                            [HttpDelete("{id}")]&#xD;
                                            [SwaggerOperation(OperationId = "delete$ResourceName$ById")]&#xD;
                                            [ProducesResponseType(StatusCodes.Status204NoContent)]&#xD;
                                            [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                            [AuthorizeUser(UserAuthorizationType.AdminPrivilege)]&#xD;
                                            public async Task&lt;IActionResult&gt; Delete$ResourceName$ByIdAsync(long id,&#xD;
                                        																   CancellationToken cancellationToken = default)&#xD;
                                            {&#xD;
                                        &#xD;
                                                await _mediator.SendAsync(new Delete$ResourceName$ById(id), cancellationToken).ConfigureAwait(false);&#xD;
                                                return NoContent();&#xD;
                                        &#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        public record $ResourceName$;&#xD;
                                        &#xD;
                                        &#xD;
                                        internal record Add$ResourceName$($ResourceName$ $ResourceName$) : ICommand&lt;Add$ResourceName$Response&gt;;&#xD;
                                        &#xD;
                                        public record Add$ResourceName$Response($ResourceName$ $ResourceName$);&#xD;
                                        &#xD;
                                        internal class Add$ResourceName$Handler : ICommandHandler&lt;Add$ResourceName$, Add$ResourceName$Response&gt;&#xD;
                                        {&#xD;
                                            public Task&lt;Add$ResourceName$Response&gt; Handle(Add$ResourceName$ request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        &#xD;
                                        &#xD;
                                        internal record GetAll$ResourceName$s : IQuery&lt;GetAll$ResourceName$sResponse&gt;;&#xD;
                                        &#xD;
                                        public record GetAll$ResourceName$sResponse(IImmutableList&lt;$ResourceName$&gt; $ResourceName$s);&#xD;
                                        &#xD;
                                        internal class GetAll$ResourceName$sHandler : IQueryHandler&lt;GetAll$ResourceName$s, GetAll$ResourceName$sResponse&gt;&#xD;
                                        {&#xD;
                                            public Task&lt;GetAll$ResourceName$sResponse&gt; Handle(GetAll$ResourceName$s request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        &#xD;
                                        &#xD;
                                        internal record Get$ResourceName$ById(long Id) : IQuery&lt;Get$ResourceName$ByIdResponse&gt;;&#xD;
                                        &#xD;
                                        public record Get$ResourceName$ByIdResponse($ResourceName$ $ResourceName$);&#xD;
                                        &#xD;
                                        internal class Get$ResourceName$ByIdHandler : IQueryHandler&lt;Get$ResourceName$ById, Get$ResourceName$ByIdResponse&gt;&#xD;
                                        {&#xD;
                                            public Task&lt;Get$ResourceName$ByIdResponse&gt; Handle(Get$ResourceName$ById request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        &#xD;
                                        internal record Delete$ResourceName$ById(long Id) : ICommand;&#xD;
                                        &#xD;
                                        internal class Delete$ResourceName$ByIdHandler : ICommandHandler&lt;Delete$ResourceName$ById&gt;&#xD;
                                        {&#xD;
                                            public Task Handle(Delete$ResourceName$ById request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        &#xD;
                                        &#xD;
                                        internal record DeleteAll$ResourceName$s : ICommand;&#xD;
                                        &#xD;
                                        internal class DeleteAll$ResourceName$sHandler : ICommandHandler&lt;DeleteAll$ResourceName$s&gt;&#xD;
                                        {&#xD;
                                            public Task Handle(DeleteAll$ResourceName$s request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Field/=path/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Field/=path/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Field/=groupName/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Field/=groupName/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Field/=ResourceName/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Field/=ResourceName/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Field/=resourceName/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F1CDFE209B34E4408F9303EE14D8F000/Field/=resourceName/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/Shortcut/@EntryValue">query</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/Text/@EntryValue">internal sealed record $query$ : IQuery&lt;$response$&gt;;&#xD;
                                        &#xD;
                                        internal sealed class $query$Handler : IQueryHandler&lt;$query$, $response$&gt;&#xD;
                                        {&#xD;
                                            public Task&lt;$response$&gt; Handle($query$ request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/Field/=query/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/Field/=query/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/Field/=response/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=1023E2094A3A2D4EAA83234EBBAAEED9/Field/=response/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Shortcut/@EntryValue">query-async-enumerable</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Text/@EntryValue">internal sealed record $query$ : IStreamRequest&lt;$response$&gt;;&#xD;
                                        &#xD;
                                        internal sealed class $query$Handler : IStreamRequestHandler&lt;$query$, $response$&gt;&#xD;
                                        {&#xD;
                                            public IAsyncEnumerable&lt;$response$&gt; Handle($query$ request, [EnumeratorCancellation] CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Field/=query/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Field/=query/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Field/=response/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=37DCA16E02BA4E489B24B6E6B9224CD2/Field/=response/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Shortcut/@EntryValue">query-responsetype</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Text/@EntryValue">internal sealed record $query$ : IQuery&lt;$response$&gt;;&#xD;
                                        &#xD;
                                        public sealed record $response$;&#xD;
                                        &#xD;
                                        internal sealed class $query$Handler : IQueryHandler&lt;$query$, $response$&gt;&#xD;
                                        {&#xD;
                                            public Task&lt;$response$&gt; Handle($query$ request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Field/=query/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Field/=query/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Field/=response/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=96A76B766D1DCF4998E8DB74ADC514B4/Field/=response/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D92D2969132854AAF0BAD54B678F95E/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D92D2969132854AAF0BAD54B678F95E/Shortcut/@EntryValue">requestvalidator</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D92D2969132854AAF0BAD54B678F95E/Text/@EntryValue">internal static class $request$ValidatorExtension&#xD;
                                        {&#xD;
                                            public static void $request$Validator(this IServiceCollection services)&#xD;
                                            {&#xD;
                                                services.AddSingletonIfNotExists&lt;IRequestValidator&lt;$request$&gt;, $request$Validator&gt;();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        internal class $request$Validator : IRequestValidator&lt;$request$&gt;&#xD;
                                        {&#xD;
                                            public Task ValidateAsync($request$ request)&#xD;
                                            {&#xD;
                                                return Task.CompletedTask;      &#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D92D2969132854AAF0BAD54B678F95E/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D92D2969132854AAF0BAD54B678F95E/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D92D2969132854AAF0BAD54B678F95E/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D92D2969132854AAF0BAD54B678F95E/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D92D2969132854AAF0BAD54B678F95E/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D92D2969132854AAF0BAD54B678F95E/Field/=request/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=6D92D2969132854AAF0BAD54B678F95E/Field/=request/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Shortcut/@EntryValue">rest-controller-no-authorization</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Text/@EntryValue">[Authorize]&#xD;
                                        [ApiController]&#xD;
                                        [ApiVersion("1.0")]&#xD;
                                        [Route("v{version:apiVersion}/$path$")]&#xD;
                                        [ApiExplorerSettings(GroupName = "$groupName$")]&#xD;
                                        public class $ResourceName$Controller : ControllerBase&#xD;
                                        {&#xD;
                                            private readonly IMediator _mediator;&#xD;
                                        &#xD;
                                            public $ResourceName$Controller(IMediator mediator)&#xD;
                                            {&#xD;
                                                _mediator = mediator;&#xD;
                                            }&#xD;
                                        &#xD;
                                            [HttpPost]&#xD;
                                            [ProducesResponseType(typeof(Add$ResourceName$Response), StatusCodes.Status200OK)]&#xD;
                                            [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]   &#xD;
                                            public Task&lt;Add$ResourceName$Response&gt; Add$ResourceName$Async($ResourceName$ $resourceName$,&#xD;
                                        																  CancellationToken cancellationToken = default)&#xD;
                                            {&#xD;
                                                return _mediator.SendAsync(new Add$ResourceName$($resourceName$), cancellationToken);&#xD;
                                            }&#xD;
                                        &#xD;
                                            [HttpGet]&#xD;
                                            [ProducesResponseType(typeof(GetAll$ResourceName$sResponse), StatusCodes.Status200OK)]&#xD;
                                            [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]&#xD;
                                            public Task&lt;GetAll$ResourceName$sResponse&gt; GetAll$ResourceName$sAsync(CancellationToken cancellationToken = default)&#xD;
                                            {&#xD;
                                                return _mediator.SendAsync(new GetAll$ResourceName$s(), cancellationToken);&#xD;
                                            }&#xD;
                                        &#xD;
                                            [HttpGet("{id}")]&#xD;
                                            [ProducesResponseType(typeof(Get$ResourceName$ByIdResponse), StatusCodes.Status200OK)]&#xD;
                                            [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]   &#xD;
                                            public Task&lt;Get$ResourceName$ByIdResponse&gt; Get$ResourceName$ByIdOrNumber(long id,&#xD;
                                        																			 CancellationToken cancellationToken = default)&#xD;
                                            {&#xD;
                                                return _mediator.SendAsync(new Get$ResourceName$ById(id), cancellationToken);&#xD;
                                            }&#xD;
                                        &#xD;
                                            [HttpDelete]&#xD;
                                            [ProducesResponseType(StatusCodes.Status204NoContent)]&#xD;
                                            [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]  &#xD;
                                            public async Task&lt;IActionResult&gt; DeleteAll$ResourceName$sAsync(CancellationToken cancellationToken = default)&#xD;
                                            {&#xD;
                                                await _mediator.SendAsync(new DeleteAll$ResourceName$s(), cancellationToken).ConfigureAwait(false);&#xD;
                                                return NoContent();&#xD;
                                            }&#xD;
                                        &#xD;
                                            [HttpDelete("{id}")]&#xD;
                                            [ProducesResponseType(StatusCodes.Status204NoContent)]&#xD;
                                            [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]   &#xD;
                                            public async Task&lt;IActionResult&gt; Delete$ResourceName$ByIdAsync(long id,&#xD;
                                        																   CancellationToken cancellationToken = default)&#xD;
                                            {&#xD;
                                        &#xD;
                                                await _mediator.SendAsync(new Delete$ResourceName$ById(id), cancellationToken).ConfigureAwait(false);&#xD;
                                                return NoContent();&#xD;
                                        &#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        public record $ResourceName$;&#xD;
                                        &#xD;
                                        &#xD;
                                        internal record Add$ResourceName$($ResourceName$ $ResourceName$) : ICommand&lt;Add$ResourceName$Response&gt;;&#xD;
                                        &#xD;
                                        public record Add$ResourceName$Response($ResourceName$ $ResourceName$);&#xD;
                                        &#xD;
                                        internal class Add$ResourceName$Handler : ICommandHandler&lt;Add$ResourceName$, Add$ResourceName$Response&gt;&#xD;
                                        {&#xD;
                                            public Task&lt;Add$ResourceName$Response&gt; Handle(Add$ResourceName$ request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        &#xD;
                                        &#xD;
                                        internal record GetAll$ResourceName$s : IQuery&lt;GetAll$ResourceName$sResponse&gt;;&#xD;
                                        &#xD;
                                        public record GetAll$ResourceName$sResponse(IImmutableList&lt;$ResourceName$&gt; $ResourceName$s);&#xD;
                                        &#xD;
                                        internal class GetAll$ResourceName$sHandler : IQueryHandler&lt;GetAll$ResourceName$s, GetAll$ResourceName$sResponse&gt;&#xD;
                                        {&#xD;
                                            public Task&lt;GetAll$ResourceName$sResponse&gt; Handle(GetAll$ResourceName$s request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        &#xD;
                                        &#xD;
                                        internal record Get$ResourceName$ById(long Id) : IQuery&lt;Get$ResourceName$ByIdResponse&gt;;&#xD;
                                        &#xD;
                                        public record Get$ResourceName$ByIdResponse($ResourceName$ $ResourceName$);&#xD;
                                        &#xD;
                                        internal class Get$ResourceName$ByIdHandler : IQueryHandler&lt;Get$ResourceName$ById, Get$ResourceName$ByIdResponse&gt;&#xD;
                                        {&#xD;
                                            public Task&lt;Get$ResourceName$ByIdResponse&gt; Handle(Get$ResourceName$ById request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        &#xD;
                                        internal record Delete$ResourceName$ById(long Id) : ICommand;&#xD;
                                        &#xD;
                                        internal class Delete$ResourceName$ByIdHandler : ICommandHandler&lt;Delete$ResourceName$ById&gt;&#xD;
                                        {&#xD;
                                            public Task Handle(Delete$ResourceName$ById request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        &#xD;
                                        &#xD;
                                        internal record DeleteAll$ResourceName$s : ICommand;&#xD;
                                        &#xD;
                                        internal class DeleteAll$ResourceName$sHandler : ICommandHandler&lt;DeleteAll$ResourceName$s&gt;&#xD;
                                        {&#xD;
                                            public Task Handle(DeleteAll$ResourceName$s request, CancellationToken cancellationToken)&#xD;
                                            {&#xD;
                                                throw new NotImplementedException();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Field/=path/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Field/=path/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Field/=groupName/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Field/=groupName/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Field/=ResourceName/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Field/=ResourceName/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Field/=resourceName/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C5BFA5B24818A47B519209EEA99EBD5/Field/=resourceName/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Shortcut/@EntryValue">ritar</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Description/@EntryValue">Iterate an array in inverse order</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Text/@EntryValue">for (int $INDEX$ = $ARRAY$.Length - 1; $INDEX$ &gt;= 0; $INDEX$--)
                                        {
                                          $TYPE$ $VAR$ = $ARRAY$[$INDEX$];
                                          $END$
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Categories/=Iteration/@EntryIndexedValue">Iteration</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=INDEX/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=INDEX/Expression/@EntryValue">suggestIndexVariable()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=INDEX/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=ARRAY/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=ARRAY/Expression/@EntryValue">arrayVariable()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=ARRAY/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=TYPE/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=TYPE/Expression/@EntryValue">suggestVariableType()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=TYPE/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=VAR/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=VAR/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=815EF179192A0746820F3C7DDB8E7BED/Field/=VAR/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/Shortcut/@EntryValue">routeattribute</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/Text/@EntryValue">[Route("$route$")]</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/Field/=route/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=A2A6AE0461B68C4AAB2EE4416D74C7BE/Field/=route/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=90D260BBCA85E7469121F19B0F503E09/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=90D260BBCA85E7469121F19B0F503E09/Shortcut/@EntryValue">routeversion</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=90D260BBCA85E7469121F19B0F503E09/Text/@EntryValue">[Route("v{version:apiVersion}/")]</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=90D260BBCA85E7469121F19B0F503E09/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=90D260BBCA85E7469121F19B0F503E09/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=90D260BBCA85E7469121F19B0F503E09/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=90D260BBCA85E7469121F19B0F503E09/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=90D260BBCA85E7469121F19B0F503E09/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Shortcut/@EntryValue">rta</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Description/@EntryValue">ASP.NET Controller RedirectToAction</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Text/@EntryValue">RedirectToAction("$ACTION$", "$CONTROLLER$")</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/Type/@EntryValue">InCSharpExpression</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Field/=CONTROLLER/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Field/=CONTROLLER/Expression/@EntryValue">AspMvcController()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Field/=CONTROLLER/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Field/=ACTION/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Field/=ACTION/Expression/@EntryValue">AspMvcAction()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=672AA4AB48660D4A9F2EC36FB4CE8C3B/Field/=ACTION/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Shortcut/@EntryValue">sfc</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Description/@EntryValue">Safely cast variable</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Text/@EntryValue">$VARTYPE$ $VAR$ = $VAR1$ as $TYPE$;
                                        
                                        if ($VAR$ != null)
                                        {
                                          $END$
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=VAR1/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=VAR1/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=VAR1/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=TYPE/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=TYPE/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=VARTYPE/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=VARTYPE/Expression/@EntryValue">suggestVariableType()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=VARTYPE/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=VARTYPE/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=VAR/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=VAR/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=AB3327059BA5FD42B453685D835444A5/Field/=VAR/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/Shortcut/@EntryValue">sim</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/Description/@EntryValue">'int Main' method </s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/Text/@EntryValue">static int Main(string[] args)&#xD;
                                        {&#xD;
                                          $END$&#xD;
                                          return 0;&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=E8E860B5857B4540A15F3F6359802842/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/Shortcut/@EntryValue">simple-in-memory-db-context</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/Text/@EntryValue">public static class Add$Domain$DbContextExtension&#xD;
                                        {&#xD;
                                            public static void Add$Domain$DbContext(this IServiceCollection services)&#xD;
                                            {&#xD;
                                                services.AddSingletonIfNotExists&lt;$Domain$DbContext&gt;();&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        public class $Domain$DbContext&#xD;
                                        {&#xD;
                                            private readonly List&lt;$Domain$&gt; _$domain$s = new();&#xD;
                                        &#xD;
                                            internal IImmutableList&lt;$Domain$&gt; GetAll$Domain$s()&#xD;
                                            {&#xD;
                                                return _$domain$s.ToImmutableList();&#xD;
                                            }&#xD;
                                        &#xD;
                                            public $Domain$ Add$Domain$($Domain$ request$Domain$)&#xD;
                                            {&#xD;
                                                _$domain$s.Add(request$Domain$);&#xD;
                                                return request$Domain$;&#xD;
                                            }&#xD;
                                        &#xD;
                                            public void DeleteById(long requestId)&#xD;
                                            {&#xD;
                                                var $domain$ToDelete = _$domain$s.FirstOrDefault($domain$ =&gt; $domain$.Id == requestId);&#xD;
                                                if ($domain$ToDelete.IsNull())&#xD;
                                                {&#xD;
                                                    throw new ProblemDetailsException("Expected $domain$ does not exist",&#xD;
                                                                                        $"The $domain$ wiht the id: {requestId} was not found or does not exists",&#xD;
                                                                                        ("$Domain$ Id", requestId.ToInvariantString()));&#xD;
                                        &#xD;
                                                }&#xD;
                                        &#xD;
                                                _$domain$s.Remove($domain$ToDelete);&#xD;
                                            }&#xD;
                                        &#xD;
                                            public void DeleteBy(Func&lt;$Domain$, bool&gt; $domain$ToDelete)&#xD;
                                            {&#xD;
                                                var $domain$s = _$domain$s.Where($domain$ToDelete).ToList();&#xD;
                                                _$domain$s.RemoveRange($domain$s);&#xD;
                                            }&#xD;
                                        &#xD;
                                            public void DeleteAll()&#xD;
                                            {&#xD;
                                                _$domain$s.Clear();&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/Field/=Domain/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/Field/=Domain/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/Field/=domain/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=14D96BDE901FCA45A705B5014F666794/Field/=domain/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C3C7D0974891B4A811BEEA9B5F167E3/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C3C7D0974891B4A811BEEA9B5F167E3/Shortcut/@EntryValue">so-get-all-test-base</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C3C7D0974891B4A811BEEA9B5F167E3/Text/@EntryValue">    [Dev]&#xD;
                                            [Environments.Test]&#xD;
                                            [Qa]&#xD;
                                            public class GetAll$domain$Test : ApiTestBase&#xD;
                                            {&#xD;
                                                public GetAll$domain$Test(Stage stage) : base(stage) { }&#xD;
                                            }&#xD;
                                        </s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C3C7D0974891B4A811BEEA9B5F167E3/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C3C7D0974891B4A811BEEA9B5F167E3/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C3C7D0974891B4A811BEEA9B5F167E3/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C3C7D0974891B4A811BEEA9B5F167E3/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C3C7D0974891B4A811BEEA9B5F167E3/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C3C7D0974891B4A811BEEA9B5F167E3/Field/=domain/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4C3C7D0974891B4A811BEEA9B5F167E3/Field/=domain/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=510BBF6BB4CE5541B92C01F47414688E/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=510BBF6BB4CE5541B92C01F47414688E/Shortcut/@EntryValue">so-setup-cleanup</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=510BBF6BB4CE5541B92C01F47414688E/Text/@EntryValue">[SetUp]&#xD;
                                        public Task SetupAsync()&#xD;
                                        {&#xD;
                                            // 1. Cleanup, to go sure all is clean !!&#xD;
                                            return CleanupAsync();&#xD;
                                        }&#xD;
                                        &#xD;
                                        [TearDown]&#xD;
                                        public Task CleanupAsync()&#xD;
                                        {&#xD;
                                            return Task.CompletedTask;&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=510BBF6BB4CE5541B92C01F47414688E/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=510BBF6BB4CE5541B92C01F47414688E/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=510BBF6BB4CE5541B92C01F47414688E/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=510BBF6BB4CE5541B92C01F47414688E/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=510BBF6BB4CE5541B92C01F47414688E/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=843B6EA6F37B6B44AACECA8B1E7149D0/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=843B6EA6F37B6B44AACECA8B1E7149D0/Shortcut/@EntryValue">startup</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=843B6EA6F37B6B44AACECA8B1E7149D0/Text/@EntryValue">internal class Startup&#xD;
                                        {&#xD;
                                            internal void ConfigureServices(IServiceCollection services)&#xD;
                                            {&#xD;
                                        &#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=843B6EA6F37B6B44AACECA8B1E7149D0/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=843B6EA6F37B6B44AACECA8B1E7149D0/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=843B6EA6F37B6B44AACECA8B1E7149D0/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=843B6EA6F37B6B44AACECA8B1E7149D0/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=843B6EA6F37B6B44AACECA8B1E7149D0/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=843B6EA6F37B6B44AACECA8B1E7149D0/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=843B6EA6F37B6B44AACECA8B1E7149D0/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/Shortcut/@EntryValue">startup.cs</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/Text/@EntryValue">namespace $namespace$&#xD;
                                        {&#xD;
                                            public sealed class Startup : SimpleStartup&#xD;
                                            {&#xD;
                                                public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) :&#xD;
                                                    base(configuration, webHostEnvironment, new PathString("/api/$domain$"))&#xD;
                                                {&#xD;
                                                }&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        </s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/Field/=domain/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/Field/=domain/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/Field/=namespace/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5F4F384E6E4EC644B465882F6E40ED88/Field/=namespace/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Shortcut/@EntryValue">struct</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Text/@EntryValue">struct $name$&#xD;
                                        {&#xD;
                                          $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Scope/=558F05AA0DE96347816FF785232CFB2A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Scope/=558F05AA0DE96347816FF785232CFB2A/Type/@EntryValue">InCSharpTypeAndNamespace</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Scope/=558F05AA0DE96347816FF785232CFB2A/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Field/=name/Expression/@EntryValue">constant("MyStruct")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=520ED668623B004894103358E0628DE4/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Shortcut/@EntryValue">surveycontroller</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Text/@EntryValue">[ApiVersion("2.0")]&#xD;
                                        [Route("v{version:apiVersion}/$route$")]&#xD;
                                        [ApiController]&#xD;
                                        public class $Controller$ : ControllerBase&#xD;
                                        {      &#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Field/=Controller/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Field/=Controller/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Field/=route/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C24E0C258BD329449CDFF84D7431A39F/Field/=route/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/Shortcut/@EntryValue">svm</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/Description/@EntryValue">'void Main' method</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/Text/@EntryValue">static void Main(string[] args)&#xD;
                                        {&#xD;
                                          $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=7E9BAF3D7CE45140A3FECBB7FE4E05FD/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Shortcut/@EntryValue">swaggerinfo</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Text/@EntryValue">{&#xD;
                                          "SwaggerInfo": {&#xD;
                                            "Id": "$id$",&#xD;
                                            "Title": "$title$",&#xD;
                                            "Description": "$description$",&#xD;
                                            "ContactName": "$contactname$",&#xD;
                                            "ContactEmail": "$contactemail$",&#xD;
                                            "ContactUrl": "$contacturl$",&#xD;
                                            "Audience": "$audience$"&#xD;
                                          }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=id/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=id/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=title/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=title/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=description/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=description/Order/@EntryValue">2</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=contactname/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=contactname/Order/@EntryValue">3</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=contactemail/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=contactemail/Order/@EntryValue">4</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=contacturl/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=contacturl/Order/@EntryValue">5</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=audience/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BDD48BD8114FB24F85A2D84E696DAEC4/Field/=audience/Order/@EntryValue">6</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B8325EB2B52F4E409AE22D6F9C657A0B/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B8325EB2B52F4E409AE22D6F9C657A0B/Shortcut/@EntryValue">swaggerversionfilter</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B8325EB2B52F4E409AE22D6F9C657A0B/Text/@EntryValue">public class RemoveVersionParameterFilter : IOperationFilter&#xD;
                                        {&#xD;
                                            public void Apply(OpenApiOperation operation, OperationFilterContext context)&#xD;
                                            {&#xD;
                                                var versionParameter = operation.Parameters.Single(p =&gt; p.Name == "version");&#xD;
                                                operation.Parameters.Remove(versionParameter);&#xD;
                                            }&#xD;
                                        }&#xD;
                                        &#xD;
                                        public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter&#xD;
                                        {&#xD;
                                            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)&#xD;
                                            {&#xD;
                                                swaggerDoc.Paths = GetOpenApiPath(swaggerDoc, context);&#xD;
                                            }&#xD;
                                        &#xD;
                                            private OpenApiPaths GetOpenApiPath(OpenApiDocument swaggerDoc, DocumentFilterContext context)&#xD;
                                            {&#xD;
                                                var paths = new OpenApiPaths();&#xD;
                                                foreach (var path in swaggerDoc.Paths)&#xD;
                                                {&#xD;
                                                    var matchingApi = context.ApiDescriptions.FirstOrDefault(api =&gt; api.RelativePath.EqualsTo(path.Key.TrimStart('/')));&#xD;
                                                    var versionInfo = matchingApi.ActionDescriptor.EndpointMetadata.FirstOfType&lt;ApiVersionAttribute&gt;();&#xD;
                                                    if (versionInfo.IsNull())&#xD;
                                                    {&#xD;
                                                        paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);&#xD;
                                                        continue;&#xD;
                                                    }&#xD;
                                        &#xD;
                                                    var apiVersion = Convert(swaggerDoc.Info);&#xD;
                                                    if (versionInfo.Versions.Any(v =&gt; v.EqualsTo(apiVersion)))&#xD;
                                                    {&#xD;
                                                        paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);&#xD;
                                                    }&#xD;
                                                }&#xD;
                                        &#xD;
                                                return paths;&#xD;
                                            }&#xD;
                                        &#xD;
                                            private ApiVersion Convert(OpenApiInfo openApiInfo)&#xD;
                                            {&#xD;
                                                var values = openApiInfo.Version.ToLower().Replace("v", string.Empty).Split(".").Select(number =&gt; number.ToInt()).ToArray();&#xD;
                                                if (values.Length == 1)&#xD;
                                                {&#xD;
                                                    return new ApiVersion(values.First(), 0);&#xD;
                                                }&#xD;
                                        &#xD;
                                                if (values.Length == 2)&#xD;
                                                {&#xD;
                                                    return new ApiVersion(values.First(), values.ElementAt(1));&#xD;
                                                }&#xD;
                                        &#xD;
                                                throw new ProblemDetailsException(500, "Unknown version string", $"Could not convert swagger version info: '{openApiInfo.Version}' into AP-Version");&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B8325EB2B52F4E409AE22D6F9C657A0B/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B8325EB2B52F4E409AE22D6F9C657A0B/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B8325EB2B52F4E409AE22D6F9C657A0B/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B8325EB2B52F4E409AE22D6F9C657A0B/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B8325EB2B52F4E409AE22D6F9C657A0B/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B8325EB2B52F4E409AE22D6F9C657A0B/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=B8325EB2B52F4E409AE22D6F9C657A0B/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Shortcut/@EntryValue">switch</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Description/@EntryValue">switch statement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Text/@EntryValue">switch ($expression$)&#xD;
                                        {&#xD;
                                          $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Field/=expression/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Field/=expression/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=55E5BFB1C5AD354DB43DBBACA2B1914E/Field/=expression/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=639B53E36EE21B46A158D962A055FA5B/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=639B53E36EE21B46A158D962A055FA5B/Shortcut/@EntryValue">swo-test-class</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=639B53E36EE21B46A158D962A055FA5B/Text/@EntryValue">[Dev]&#xD;
                                        [Test]&#xD;
                                        [Qa]&#xD;
                                        [Prod]&#xD;
                                        internal class $name$ : NoSyncProcessTestBase&#xD;
                                        {&#xD;
                                            public $name$(Stage stage) : base(stage)&#xD;
                                            {&#xD;
                                            }&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=639B53E36EE21B46A158D962A055FA5B/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=639B53E36EE21B46A158D962A055FA5B/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=639B53E36EE21B46A158D962A055FA5B/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=639B53E36EE21B46A158D962A055FA5B/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=639B53E36EE21B46A158D962A055FA5B/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=639B53E36EE21B46A158D962A055FA5B/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=639B53E36EE21B46A158D962A055FA5B/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Shortcut/@EntryValue">test</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Text/@EntryValue">[Xunit.Fact]&#xD;
                                        public void $METHOD$()&#xD;
                                        {$END$}</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Categories/=xUnit/@EntryIndexedValue">xUnit</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Categories/=Unit_0020Testing/@EntryIndexedValue">Unit Testing</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Scope/=C41E13C6A9E49B48A3CA89E735234942/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Scope/=C41E13C6A9E49B48A3CA89E735234942/Type/@EntryValue">InTestProject</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Scope/=C41E13C6A9E49B48A3CA89E735234942/CustomProperties/=Provider/@EntryIndexedValue">xUnit</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Field/=METHOD/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=BA213BDD6696244FBBD08EAE54F32F4B/Field/=METHOD/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Shortcut/@EntryValue">test</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Text/@EntryValue">[NUnit.Framework.Test]&#xD;
                                        public void $METHOD$()&#xD;
                                        {$END$}</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Categories/=NUnit3x/@EntryIndexedValue">NUnit3x</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Categories/=Unit_0020Testing/@EntryIndexedValue">Unit Testing</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Scope/=C41E13C6A9E49B48A3CA89E735234942/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Scope/=C41E13C6A9E49B48A3CA89E735234942/Type/@EntryValue">InTestProject</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Scope/=C41E13C6A9E49B48A3CA89E735234942/CustomProperties/=Provider/@EntryIndexedValue">NUnit3x</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Field/=METHOD/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=5FCCA601C0052D41816EB829C585A41C/Field/=METHOD/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Shortcut/@EntryValue">test</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Text/@EntryValue">[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]&#xD;
                                        public void $METHOD$()&#xD;
                                        {$END$}</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Categories/=MSTest/@EntryIndexedValue">MSTest</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Categories/=Unit_0020Testing/@EntryIndexedValue">Unit Testing</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Scope/=C41E13C6A9E49B48A3CA89E735234942/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Scope/=C41E13C6A9E49B48A3CA89E735234942/Type/@EntryValue">InTestProject</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Scope/=C41E13C6A9E49B48A3CA89E735234942/CustomProperties/=Provider/@EntryIndexedValue">MSTest</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Field/=METHOD/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C4F9AA5F6669A2438DAEE2694E029A99/Field/=METHOD/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Shortcut/@EntryValue">test</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Text/@EntryValue">[NUnit.Framework.Test]&#xD;
                                        public void $METHOD$()&#xD;
                                        {$END$}</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Categories/=NUnit2x/@EntryIndexedValue">NUnit2x</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Categories/=Unit_0020Testing/@EntryIndexedValue">Unit Testing</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Scope/=C41E13C6A9E49B48A3CA89E735234942/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Scope/=C41E13C6A9E49B48A3CA89E735234942/Type/@EntryValue">InTestProject</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Scope/=C41E13C6A9E49B48A3CA89E735234942/CustomProperties/=Provider/@EntryIndexedValue">NUnit2x</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Field/=METHOD/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FC8D7941811E074BA41BC7195D0E7D5A/Field/=METHOD/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Shortcut/@EntryValue">testclass</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Text/@EntryValue">[TestClass]&#xD;
                                        [TestCategory("$category$")]&#xD;
                                        public class $testName$ : MsTestBase&#xD;
                                        {&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Field/=category/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Field/=category/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Field/=testName/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FAC9B62478B294468A2D409B417135A1/Field/=testName/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F48CE3838D03444EA69B8C23CB1A07B8/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F48CE3838D03444EA69B8C23CB1A07B8/Shortcut/@EntryValue">testmethod</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F48CE3838D03444EA69B8C23CB1A07B8/Text/@EntryValue">[TestMethod]&#xD;
                                        public void $name$()&#xD;
                                        {&#xD;
                                        &#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F48CE3838D03444EA69B8C23CB1A07B8/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F48CE3838D03444EA69B8C23CB1A07B8/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F48CE3838D03444EA69B8C23CB1A07B8/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F48CE3838D03444EA69B8C23CB1A07B8/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F48CE3838D03444EA69B8C23CB1A07B8/Scope/=139FF4CE89E7094686FDA7BF5FFBBE92/Type/@EntryValue">Everywhere</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F48CE3838D03444EA69B8C23CB1A07B8/Field/=name/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F48CE3838D03444EA69B8C23CB1A07B8/Field/=name/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F5543C3B8BC9204385224EB47C2AAE7E/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F5543C3B8BC9204385224EB47C2AAE7E/Shortcut/@EntryValue">thr</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F5543C3B8BC9204385224EB47C2AAE7E/Description/@EntryValue">throw new</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F5543C3B8BC9204385224EB47C2AAE7E/Text/@EntryValue">throw new </s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F5543C3B8BC9204385224EB47C2AAE7E/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F5543C3B8BC9204385224EB47C2AAE7E/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F5543C3B8BC9204385224EB47C2AAE7E/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F5543C3B8BC9204385224EB47C2AAE7E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F5543C3B8BC9204385224EB47C2AAE7E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=F5543C3B8BC9204385224EB47C2AAE7E/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Shortcut/@EntryValue">try</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Description/@EntryValue">try catch</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Text/@EntryValue">try &#xD;
                                        {          &#xD;
                                          $SELECTION$&#xD;
                                        }&#xD;
                                        catch ($EXCEPTION$ $EX_NAME$)&#xD;
                                        {&#xD;
                                          $SELSTART$System.Console.WriteLine($EX_NAME$);&#xD;
                                          throw;$SELEND$&#xD;
                                        }</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Mnemonic/@EntryValue">8</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Field/=EXCEPTION/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Field/=EXCEPTION/Expression/@EntryValue">constant("Exception")</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Field/=EXCEPTION/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Field/=EX_005FNAME/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Field/=EX_005FNAME/Expression/@EntryValue">suggestVariableName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DED7798D82D02045ABA68FC339C76DFA/Field/=EX_005FNAME/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Shortcut/@EntryValue">tryf</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Description/@EntryValue">try finally</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Text/@EntryValue">try &#xD;
                                        {&#xD;
                                          $SELECTION$&#xD;
                                        }&#xD;
                                        finally&#xD;
                                        {&#xD;
                                          $END$&#xD;
                                        }</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Mnemonic/@EntryValue">9</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=FA62801AE9ACA34089E6E38B1F425B1C/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Shortcut/@EntryValue">ua</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Description/@EntryValue">ASP.NET MVC Url.Action</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Text/@EntryValue">Url.Action("$ACTION$", "$CONTROLLER$")</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/Type/@EntryValue">InCSharpExpression</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Scope/=E6E678D4B937A84D8C4585DDD2F27DB0/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Field/=CONTROLLER/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Field/=CONTROLLER/Expression/@EntryValue">AspMvcController()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Field/=CONTROLLER/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Field/=ACTION/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Field/=ACTION/Expression/@EntryValue">AspMvcAction()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=DCDFBDFC419CEB4099216C8D52DF43D9/Field/=ACTION/Order/@EntryValue">1</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/Shortcut/@EntryValue">unchecked</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/Description/@EntryValue">unchecked block</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/Text/@EntryValue">unchecked&#xD;
                                        {&#xD;
                                          $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=C912A7E8A815554C972CE14283AABCAF/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/Shortcut/@EntryValue">unsafe</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/Description/@EntryValue">unsafe statement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/Text/@EntryValue">unsafe&#xD;
                                        {&#xD;
                                          $END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=92DDB1A6C1214147A64B168C8EC58B8B/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/Shortcut/@EntryValue">useswaggerui</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/Text/@EntryValue">app.UseSwagger();&#xD;
                                        app.UseSwaggerUI(c =&gt;&#xD;
                                        {&#xD;
                                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "$api$");&#xD;
                                        });</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/Type/@EntryValue">InCSharpFile</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/Scope/=C3001E7C0DA78E4487072B7E050D86C5/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/Field/=api/@KeyIndexDefined">True</s:Boolean>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=2478CE4F188D9D4FBC1AC4657B030EF1/Field/=api/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Shortcut/@EntryValue">using</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Description/@EntryValue">using statement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Text/@EntryValue">using($resource$)&#xD;
                                        {&#xD;
                                          $SELECTION$$END$&#xD;
                                        }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Field/=resource/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Field/=resource/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=3029EBA05131A140933D5EBBA69ED48C/Field/=resource/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Shortcut/@EntryValue">while</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Description/@EntryValue">while loop</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Text/@EntryValue">while ($expression$)&#xD;
                                        {&#xD;
                                          $SELECTION$$END$&#xD;
                                        }</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Mnemonic/@EntryValue">2</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/IsBlessed/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Applicability/=Surround/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/Type/@EntryValue">InCSharpStatement</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Scope/=2C285F182AC98D44B0B4F29D4D2149EC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Field/=expression/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Field/=expression/Expression/@EntryValue">complete()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=69AA0FB53ED5F246B1F1D65DB6A708B8/Field/=expression/Order/@EntryValue">0</s:Int64>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Shortcut/@EntryValue">~</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Description/@EntryValue">Destructor</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Text/@EntryValue">~$classname$()&#xD;
                                          {&#xD;
                                            $END$&#xD;
                                          }</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Reformat/@EntryValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/ShortenQualifiedReferences/@EntryValue">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Categories/=Imported_00209_002F3_002F2020/@EntryIndexedValue">Imported 9/3/2020</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Categories/=Imported_0020Visual_0020C_0023_0020Snippets/@EntryIndexedValue">Imported Visual C# Snippets</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Applicability/=Live/@EntryIndexedValue">True</s:Boolean>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/Type/@EntryValue">InCSharpTypeMember</s:String>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Scope/=B68999B9D6B43E47A02B22C12A54C3CC/CustomProperties/=minimumLanguageVersion/@EntryIndexedValue">2.0</s:String>
                                            <s:Boolean x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Field/=classname/@KeyIndexDefined">True</s:Boolean>
                                            <s:String x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Field/=classname/Expression/@EntryValue">typeName()</s:String>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Field/=classname/InitialRange/@EntryValue">-1</s:Int64>
                                            <s:Int64 x:Key="/Default/PatternsAndTemplates/LiveTemplates/Template/=4B9F55AAF68AAD48871D232092C882A9/Field/=classname/Order/@EntryValue">0</s:Int64>
                                        </wpf:ResourceDictionary>
                                        """;


        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1 Setup file name
            var file = $"{solutionFile.SolutionFileInfo.Value.FullName}.DotSettings";

            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}

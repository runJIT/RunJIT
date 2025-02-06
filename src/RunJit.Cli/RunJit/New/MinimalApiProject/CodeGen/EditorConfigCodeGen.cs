﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal static class AddEditorConfigCodeGenExtension
    {
        internal static void AddEditorConfigCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, EditorConfigCodeGen>();
        }
    }

    internal sealed class EditorConfigCodeGen(ConsoleService consoleService) : IMinimalApiProjectRootLevelCodeGen
    {
        private const string Template = """
                                        # To learn more about .editorconfig see https://aka.ms/editorconfigdocs
                                        ###############################
                                        # Core EditorConfig Options   #
                                        ###############################
                                        ###############################
                                        # .NET Diagnostic Analyzers    #
                                        ###############################
                                        [*.cs]
                                        dotnet_analyzer_diagnostic.severity = warning
                                        dotnet_diagnostic.CA1032.severity = silent
                                        dotnet_diagnostic.CS1591.severity = none
                                        dotnet_diagnostic.CS1998.severity = error
                                        dotnet_diagnostic.CA1701.severity = silent
                                        dotnet_diagnostic.CA1702.severity = silent
                                        dotnet_diagnostic.CA1707.severity = silent
                                        dotnet_diagnostic.CA1711.severity = silent
                                        dotnet_diagnostic.CA1716.severity = silent
                                        dotnet_diagnostic.CA1724.severity = silent
                                        dotnet_diagnostic.CA1819.severity = silent
                                        dotnet_diagnostic.CA1822.severity = silent
                                        dotnet_diagnostic.CA2007.severity = warning
                                        dotnet_diagnostic.IDE0008.severity = silent
                                        dotnet_diagnostic.IDE0003.severity = silent
                                        dotnet_diagnostic.IDE0010.severity = silent
                                        dotnet_diagnostic.IDE0017.severity = silent
                                        dotnet_diagnostic.IDE0022.severity = none
                                        dotnet_diagnostic.IDE0028.severity = none
                                        dotnet_diagnostic.IDE0046.severity = silent
                                        dotnet_diagnostic.IDE0050.severity = silent
                                        dotnet_diagnostic.IDE0058.severity = silent
                                        dotnet_diagnostic.IDE0072.severity = silent
                                        dotnet_diagnostic.IDE0130.severity = silent
                                        dotnet_diagnostic.IDE0005.severity = silent
                                        dotnet_diagnostic.IDE0270.severity = error
                                        dotnet_diagnostic.IDE0007.severity = error
                                        # All files
                                        [*]
                                        indent_style = space
                                        # Code files
                                        [*.{cs,csx,vb,vbx}]
                                        indent_size = 4
                                        insert_final_newline = true
                                        charset = utf-8-bom
                                        ###############################
                                        # .NET Coding Conventions     #
                                        ###############################
                                        [*.{cs,vb}]
                                        # Organize usings
                                        dotnet_sort_system_directives_first = true
                                        # this. preferences
                                        dotnet_style_qualification_for_field = false:silent
                                        dotnet_style_qualification_for_property = false:silent
                                        dotnet_style_qualification_for_method = false:silent
                                        dotnet_style_qualification_for_event = false:silent
                                        # Language keywords vs BCL types preferences
                                        dotnet_style_predefined_type_for_locals_parameters_members = true:silent
                                        dotnet_style_predefined_type_for_member_access = true:silent
                                        # Parentheses preferences
                                        dotnet_style_parentheses_in_arithmetic_binary_operators = always_for_clarity:silent
                                        dotnet_style_parentheses_in_relational_binary_operators = always_for_clarity:silent
                                        dotnet_style_parentheses_in_other_binary_operators = always_for_clarity:silent
                                        dotnet_style_parentheses_in_other_operators = never_if_unnecessary:silent
                                        # Modifier preferences
                                        dotnet_style_require_accessibility_modifiers = for_non_interface_members:silent
                                        dotnet_style_readonly_field = true:suggestion
                                        # Expression-level preferences
                                        dotnet_style_object_initializer = true:suggestion
                                        dotnet_style_collection_initializer = true:suggestion
                                        dotnet_style_explicit_tuple_names = true:suggestion
                                        dotnet_style_null_propagation = true:suggestion
                                        dotnet_style_coalesce_expression = true:suggestion
                                        dotnet_style_prefer_is_null_check_over_reference_equality_method = true:silent
                                        dotnet_style_prefer_inferred_tuple_names = true:suggestion
                                        dotnet_style_prefer_inferred_anonymous_type_member_names = true:suggestion
                                        dotnet_style_prefer_auto_properties = true:silent
                                        dotnet_style_prefer_conditional_expression_over_assignment = true:silent
                                        dotnet_style_prefer_conditional_expression_over_return = true:silent
                                        ###############################
                                        # Naming Conventions          #
                                        ###############################
                                        # Style Definitions
                                        dotnet_naming_style.pascal_case_style.capitalization             = pascal_case
                                        # Use PascalCase for constant fields  
                                        dotnet_naming_rule.constant_fields_should_be_pascal_case.severity = suggestion
                                        dotnet_naming_rule.constant_fields_should_be_pascal_case.symbols  = constant_fields
                                        dotnet_naming_rule.constant_fields_should_be_pascal_case.style = pascal_case_style
                                        dotnet_naming_symbols.constant_fields.applicable_kinds            = field
                                        dotnet_naming_symbols.constant_fields.applicable_accessibilities  = *
                                        dotnet_naming_symbols.constant_fields.required_modifiers          = const
                                        dotnet_style_operator_placement_when_wrapping = beginning_of_line
                                        tab_width = 4
                                        end_of_line = crlf
                                        dotnet_diagnostic.CA1305.severity = suggestion
                                        dotnet_diagnostic.CA1860.severity = suggestion
                                        dotnet_diagnostic.CA1859.severity = suggestion
                                        dotnet_diagnostic.CA1848.severity = suggestion
                                        dotnet_diagnostic.CA1826.severity = suggestion
                                        dotnet_diagnostic.CA2254.severity = suggestion
                                        dotnet_diagnostic.CA1861.severity = suggestion
                                        dotnet_style_prefer_simplified_boolean_expressions = true:suggestion
                                        ###############################
                                        # C# Coding Conventions       #
                                        ###############################
                                        [*.cs]
                                        # var preferences
                                        csharp_style_var_for_built_in_types = true:silent
                                        csharp_style_var_when_type_is_apparent = true:silent
                                        csharp_style_var_elsewhere = true:silent
                                        # Expression-bodied members
                                        csharp_style_expression_bodied_methods = false:silent
                                        csharp_style_expression_bodied_constructors = false:silent
                                        csharp_style_expression_bodied_operators = false:silent
                                        csharp_style_expression_bodied_properties = true:silent
                                        csharp_style_expression_bodied_indexers = true:silent
                                        csharp_style_expression_bodied_accessors = true:silent
                                        # Pattern matching preferences
                                        csharp_style_pattern_matching_over_is_with_cast_check = true:suggestion
                                        csharp_style_pattern_matching_over_as_with_null_check = true:suggestion
                                        # Null-checking preferences
                                        csharp_style_throw_expression = true:suggestion
                                        csharp_style_conditional_delegate_call = true:suggestion
                                        # Modifier preferences
                                        csharp_preferred_modifier_order = public,private,protected,internal,static,extern,new,virtual,abstract,sealed,override,readonly,unsafe,volatile,async:suggestion
                                        # Expression-level preferences
                                        csharp_prefer_braces = true:silent
                                        csharp_style_deconstructed_variable_declaration = true:suggestion
                                        csharp_prefer_simple_default_expression = true:suggestion
                                        csharp_style_pattern_local_over_anonymous_function = true:suggestion
                                        csharp_style_inlined_variable_declaration = true:suggestion
                                        
                                        ###############################
                                        # C# Formatting Rules         #
                                        ###############################
                                        # New line preferences
                                        csharp_new_line_before_open_brace = all
                                        csharp_new_line_before_else = true
                                        csharp_new_line_before_catch = true
                                        csharp_new_line_before_finally = true
                                        csharp_new_line_before_members_in_object_initializers = true
                                        csharp_new_line_before_members_in_anonymous_types = true
                                        csharp_new_line_between_query_expression_clauses = true
                                        # Indentation preferences
                                        csharp_indent_case_contents = true
                                        csharp_indent_switch_labels = true
                                        csharp_indent_labels = flush_left
                                        # Space preferences
                                        csharp_space_after_cast = false
                                        csharp_space_after_keywords_in_control_flow_statements = true
                                        csharp_space_between_method_call_parameter_list_parentheses = false
                                        csharp_space_between_method_declaration_parameter_list_parentheses = false
                                        csharp_space_between_parentheses = false
                                        csharp_space_before_colon_in_inheritance_clause = true
                                        csharp_space_after_colon_in_inheritance_clause = true
                                        csharp_space_around_binary_operators = before_and_after
                                        csharp_space_between_method_declaration_empty_parameter_list_parentheses = false
                                        csharp_space_between_method_call_name_and_opening_parenthesis = false
                                        csharp_space_between_method_call_empty_parameter_list_parentheses = false
                                        # Wrapping preferences
                                        csharp_preserve_single_line_statements = true
                                        csharp_preserve_single_line_blocks = true
                                        csharp_using_directive_placement = outside_namespace:silent
                                        csharp_prefer_simple_using_statement = true:suggestion
                                        csharp_style_namespace_declarations = block_scoped:silent
                                        csharp_style_prefer_method_group_conversion = true:silent
                                        csharp_style_prefer_top_level_statements = true:silent
                                        csharp_style_expression_bodied_lambdas = true:silent
                                        csharp_style_expression_bodied_local_functions = false:silent
                                        dotnet_diagnostic.IDE0200.severity = suggestion
                                        dotnet_diagnostic.IDE0045.severity = suggestion
                                        csharp_style_prefer_primary_constructors = true:suggestion
                                        dotnet_diagnostic.IDE0300.severity = suggestion
                                        dotnet_diagnostic.IDE0301.severity = suggestion
                                        dotnet_diagnostic.IDE0302.severity = suggestion
                                        dotnet_diagnostic.IDE0303.severity = suggestion
                                        dotnet_diagnostic.IDE0304.severity = suggestion
                                        dotnet_diagnostic.IDE0305.severity = suggestion
                                        dotnet_diagnostic.IDE0290.severity = error
                                        dotnet_diagnostic.IDE0090.severity = suggestion
                                        dotnet_diagnostic.IDE0042.severity = suggestion
                                        dotnet_diagnostic.IDE0055.severity = suggestion
                                        dotnet_diagnostic.IDE0037.severity = suggestion
                                        csharp_style_unused_value_expression_statement_preference = discard_variable:silent
                                        csharp_style_allow_embedded_statements_on_same_line_experimental = true:silent
                                        ###############################
                                        # VB Coding Conventions       #
                                        ###############################
                                        [*.vb]
                                        # Modifier preferences
                                        visual_basic_preferred_modifier_order = Partial,Default,Private,Protected,Public,Friend,NotOverridable,Overridable,MustOverride,Overloads,Overrides,MustInherit,NotInheritable,Static,Shared,Shadows,ReadOnly,WriteOnly,Dim,Const,WithEvents,Widening,Narrowing,Custom,Async:suggestion
                                        
                                        """;


        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1 Setup file name
            var file = Path.Combine(solutionFile.SolutionFileInfo.Value.Directory!.FullName, ".editorconfig");

            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}

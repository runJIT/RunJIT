﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Generate.Client
{
    internal static class AddJsonSerializerBuilderExtension
    {
        internal static void AddJsonSerializerBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<JsonSerializerBuilder>();
        }
    }

    internal sealed class JsonSerializerBuilder
    {
        private readonly string _clientTemplate = """
                                                  using System.Reflection;
                                                  using System.Text.Json.Serialization;
                                                  using System.Text.Json;
                                                  
                                                  namespace $namespace$
                                                  {
                                                      internal static class AddJsonSerializerExtension
                                                      {
                                                          internal static void AddJsonSerializer(this IServiceCollection services,
                                                                                                 IConfiguration configuration)
                                                          {
                                                              if (services.IsAlreadyRegistered<IJsonSerializer>())
                                                              {
                                                                  return;
                                                              }
                                                  
                                                              using var provider = services.BuildServiceProvider();
                                                  
                                                              // Now we check if a serializer options was already registered for global use
                                                              var jsonSerializerOptions = provider.GetService<JsonSerializerOptions>();
                                                  
                                                              if (jsonSerializerOptions.IsNull())
                                                              {
                                                                  // Fallback setup
                                                                  jsonSerializerOptions = new JsonSerializerOptions()
                                                                                          {
                                                                                              PropertyNameCaseInsensitive = true,
                                                                                              PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                                                                              NumberHandling = JsonNumberHandling.AllowReadingFromString,
                                                                                              Converters = { new JsonStringEnumConverter() }
                                                                                          };
                                                  
                                                                  services.AddSingletonIfNotExists(jsonSerializerOptions);
                                                              }
                                                  
                                                              // ToDo: Need to think how to share the API serializer settings with the
                                                              //       client side here
                                                              services.AddSingletonIfNotExists<IJsonSerializer, JsonSerializer>();
                                                          }
                                                      }
                                                  
                                                      [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
                                                      public class ShowJsonOnErrorAttribute : Attribute
                                                      {
                                                      }
                                                  
                                                      internal interface IJsonSerializer
                                                      {
                                                          /// <summary>
                                                          /// Converts the provided value into a <see cref="string"/>.
                                                          /// </summary>
                                                          /// <typeparam name="T">The type of the value to serialize.</typeparam>
                                                          /// <returns>A <see cref="string"/> representation of the value.</returns>
                                                          /// <param name="source">The source to convert.</param>
                                                          /// <exception cref="NotSupportedException">
                                                          /// There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter"/>
                                                          /// for <typeparamref name="T"/> or its serializable members.
                                                          /// </exception>
                                                          string Serialize<T>(T source);
                                                  
                                                          /// <summary>
                                                          /// Converts the provided value into a <see cref="string"/>.
                                                          /// </summary>
                                                          /// <typeparam name="T">The type of the value to serialize.</typeparam>
                                                          /// <returns>A <see cref="string"/> representation of the value.</returns>
                                                          /// <param name="source">The source to convert.</param>
                                                          /// <param name="defaultValue">A optional default value if result will be <see langword="null"/> you can define alternative return value..</param>
                                                          /// <exception cref="NotSupportedException">
                                                          /// There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter"/>
                                                          /// for <typeparamref name="T"/> or its serializable members.
                                                          /// </exception>
                                                          string? SerializeOrDefault<T>(T source,
                                                                                        string? defaultValue = default);
                                                  
                                                          /// <summary>
                                                          /// Parses the text representing a single JSON value into a <typeparamref name="T"/>.
                                                          /// </summary>
                                                          /// <typeparam name="T">The type to deserialize the JSON value into.</typeparam>
                                                          /// <returns>A <typeparamref name="T"/> representation of the JSON value.</returns>
                                                          /// <param name="json">JSON text to parse.</param>
                                                          /// <exception cref="ArgumentNullException">
                                                          /// <paramref name="json"/> is <see langword="null"/>.
                                                          /// </exception>
                                                          /// <exception cref="JsonException">
                                                          /// The JSON is invalid.
                                                          ///
                                                          /// -or-
                                                          ///
                                                          /// <typeparamref name="T" /> is not compatible with the JSON.
                                                          ///
                                                          /// -or-
                                                          ///
                                                          /// There is remaining data in the string beyond a single JSON value.</exception>
                                                          /// <exception cref="NotSupportedException">
                                                          /// There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter"/>
                                                          /// for <typeparamref name="T"/> or its serializable members.
                                                          /// </exception>
                                                          T Deserialize<T>(string json);
                                                  
                                                          /// <summary>
                                                          /// Parses the text representing a single JSON value into a <typeparamref name="T"/>.
                                                          /// </summary>
                                                          /// <typeparam name="T">The type to deserialize the JSON value into.</typeparam>
                                                          /// <returns>A <typeparamref name="T"/> representation of the JSON value.</returns>
                                                          /// <param name="json">JSON text to parse.</param>
                                                          /// <param name="defaultValue">A optional default value if result will be <see langword="null"/> you can define alternative return value..</param>
                                                          /// <exception cref="ArgumentNullException">
                                                          /// <paramref name="json"/> is <see langword="null"/>.
                                                          /// </exception>
                                                          /// <exception cref="JsonException">
                                                          /// The JSON is invalid.
                                                          ///
                                                          /// -or-
                                                          ///
                                                          /// <typeparamref name="T" /> is not compatible with the JSON.
                                                          ///
                                                          /// -or-
                                                          ///
                                                          /// There is remaining data in the string beyond a single JSON value.</exception>
                                                          /// <exception cref="NotSupportedException">
                                                          /// There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter"/>
                                                          /// for <typeparamref name="T"/> or its serializable members.
                                                          /// </exception>
                                                          T? DeserializeOrDefault<T>(string json,
                                                                                     T? defaultValue = default);
                                                  
                                                          /// <summary>
                                                          /// Parses the text representing a single JSON value into a <paramref name="returnType"/>.
                                                          /// </summary>
                                                          /// <returns>A <paramref name="returnType"/> representation of the JSON value.</returns>
                                                          /// <param name="json">JSON text to parse.</param>
                                                          /// <param name="returnType">The type of the object to convert to and return.</param>
                                                          /// <exception cref="ArgumentNullException">
                                                          /// <paramref name="json"/> or <paramref name="returnType"/> is <see langword="null"/>.
                                                          /// </exception>
                                                          /// <exception cref="JsonException">
                                                          /// The JSON is invalid.
                                                          ///
                                                          /// -or-
                                                          ///
                                                          /// <paramref name="returnType"/> is not compatible with the JSON.
                                                          ///
                                                          /// -or-
                                                          ///
                                                          /// There is remaining data in the string beyond a single JSON value.</exception>
                                                          /// <exception cref="NotSupportedException">
                                                          /// There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter"/>
                                                          /// for <paramref name="returnType"/> or its serializable members.
                                                          /// </exception>
                                                          object Deserialize<T>(string json,
                                                                                Type returnType);
                                                  
                                                          /// <summary>
                                                          /// Parses the text representing a single JSON value into a <paramref name="returnType"/>.
                                                          /// </summary>
                                                          /// <returns>A <paramref name="returnType"/> representation of the JSON value.</returns>
                                                          /// <param name="json">JSON text to parse.</param>
                                                          /// <param name="returnType">The type of the object to convert to and return.</param>
                                                          /// <param name="defaultValue">A optional default value if result will be <see langword="null"/> you can define alternative return value..</param>
                                                          /// <exception cref="ArgumentNullException">
                                                          /// <paramref name="json"/> or <paramref name="returnType"/> is <see langword="null"/>.
                                                          /// </exception>
                                                          /// <exception cref="JsonException">
                                                          /// The JSON is invalid.
                                                          ///
                                                          /// -or-
                                                          ///
                                                          /// <paramref name="returnType"/> is not compatible with the JSON.
                                                          ///
                                                          /// -or-
                                                          ///
                                                          /// There is remaining data in the string beyond a single JSON value.</exception>
                                                          /// <exception cref="NotSupportedException">
                                                          /// There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter"/>
                                                          /// for <paramref name="returnType"/> or its serializable members.
                                                          /// </exception>
                                                          object? DeserializeOrDefault(string json,
                                                                                       Type returnType,
                                                                                       object? defaultValue = default);
                                                      }
                                                  
                                                      internal class JsonSerializer : IJsonSerializer
                                                      {
                                                          private readonly ILogger<JsonSerializer> _logger;
                                                  
                                                          private readonly JsonSerializerOptions _serializeOptions;
                                                  
                                                          public JsonSerializer(ILogger<JsonSerializer> logger,
                                                                                JsonSerializerOptions jsonSerializerOptions)
                                                          {
                                                              _logger = logger;
                                                  
                                                              _serializeOptions = new JsonSerializerOptions
                                                                                  {
                                                                                      PropertyNameCaseInsensitive = true,
                                                                                      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                                                                      NumberHandling = JsonNumberHandling.AllowReadingFromString,
                                                                                      Converters = { new JsonStringEnumConverter() }
                                                                                  };
                                                          }
                                                  
                                                          public string Serialize<T>(T source)
                                                          {
                                                              return System.Text.Json.JsonSerializer.Serialize(source, _serializeOptions);
                                                          }
                                                  
                                                          public string? SerializeOrDefault<T>(T source,
                                                                                               string? defaultValue = default)
                                                          {
                                                              try
                                                              {
                                                                  return System.Text.Json.JsonSerializer.Serialize(source, _serializeOptions);
                                                              }
                                                              catch (Exception e)
                                                              {
                                                                  _logger.LogError(e, e.Message);
                                                  
                                                                  return defaultValue;
                                                              }
                                                          }
                                                  
                                                          public T Deserialize<T>(string json)
                                                          {
                                                              T? deserializeResult = default;
                                                              var errorMessage = string.Empty;
                                                  
                                                              try
                                                              {
                                                                  deserializeResult = System.Text.Json.JsonSerializer.Deserialize<T>(json, _serializeOptions);
                                                              }
                                                              catch (Exception e)
                                                              {
                                                                  errorMessage = e.Message;
                                                              }
                                                  
                                                              if (deserializeResult.IsNull())
                                                              {
                                                                  var securityCritical = typeof(T).GetCustomAttribute<ShowJsonOnErrorAttribute>();
                                                                  var jsonString = securityCritical.IsNotNull() ? json : "Hidden cause of security critical infos";
                                                  
                                                                  // Not cool :(
                                                                  throw new Siemens.AspNet.ErrorHandling.Contracts.InternalServerErrorDetailsException("Could not deserialize your json string into expected type",
                                                                                                                                                       $"Could not deserialize your json string into expected type: {typeof(T).Name}",
                                                                                                                                                       ("Exception", errorMessage),
                                                                                                                                                       ("JsonString", jsonString),
                                                                                                                                                       ("Type", typeof(T).Name),
                                                                                                                                                       ("TypeFullName", typeof(T).FullName ?? string.Empty),
                                                                                                                                                       ("Info", $"Add [{nameof(ShowJsonOnErrorAttribute)}] to your type to see json. But be careful of security critical infos"));
                                                              }
                                                  
                                                              return deserializeResult;
                                                          }
                                                  
                                                          public T? DeserializeOrDefault<T>(string json,
                                                                                            T? defaultValue = default)
                                                          {
                                                              try
                                                              {
                                                                  var deserializeResult = System.Text.Json.JsonSerializer.Deserialize<T>(json, _serializeOptions);
                                                  
                                                                  return deserializeResult;
                                                              }
                                                              catch (Exception)
                                                              {
                                                                  return defaultValue;
                                                              }
                                                          }
                                                  
                                                          public object Deserialize<T>(string json,
                                                                                       Type returnType)
                                                          {
                                                              object? deserializeResult = default(T);
                                                              var errorMessage = string.Empty;
                                                  
                                                              try
                                                              {
                                                                  deserializeResult = System.Text.Json.JsonSerializer.Deserialize(json, returnType, _serializeOptions);
                                                              }
                                                              catch (Exception e)
                                                              {
                                                                  errorMessage = e.Message;
                                                              }
                                                  
                                                              if (deserializeResult.IsNull())
                                                              {
                                                                  var securityCritical = typeof(T).GetCustomAttribute<ShowJsonOnErrorAttribute>();
                                                                  var jsonString = securityCritical.IsNotNull() ? json : "Hidden cause of security critical infos";
                                                  
                                                                  throw new Siemens.AspNet.ErrorHandling.Contracts.InternalServerErrorDetailsException("Could not deserialize your json string into expected type",
                                                                                                                                                       $"Could not deserialize your json string into expected type: {typeof(T).Name}",
                                                                                                                                                       ("Exception", errorMessage),
                                                                                                                                                       ("JsonString", jsonString),
                                                                                                                                                       ("Type", typeof(T).Name),
                                                                                                                                                       ("TypeFullName", typeof(T).FullName ?? string.Empty),
                                                                                                                                                       ("Info", $"Add [{nameof(ShowJsonOnErrorAttribute)}] to your type to see json. But be careful of security critical infos"));
                                                              }
                                                  
                                                              return deserializeResult;
                                                          }
                                                  
                                                          public object? DeserializeOrDefault(string json,
                                                                                              Type returnType,
                                                                                              object? defaultValue = default)
                                                          {
                                                              try
                                                              {
                                                                  var deserializeResult = System.Text.Json.JsonSerializer.Deserialize(json, returnType, _serializeOptions);
                                                  
                                                                  return deserializeResult;
                                                              }
                                                              catch (Exception)
                                                              {
                                                                  return defaultValue;
                                                              }
                                                          }
                                                      }
                                                  }
                                                  
                                                  """;

        public string BuildFor(string projectName,
                               string clientName)
        {
            var appsettings = _clientTemplate.Replace("$moduleName$", clientName)
                                             .Replace("$namespace$", projectName);

            return appsettings;
        }
    }
}

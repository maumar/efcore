// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Cosmos.Metadata.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

/// <summary>
///     Property extension methods for Cosmos metadata.
/// </summary>
/// <remarks>
///     See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see>, and
///     <see href="https://aka.ms/efcore-docs-cosmos">Accessing Azure Cosmos DB with EF Core</see> for more information and examples.
/// </remarks>
public static class CosmosPropertyExtensions
{
    /// <summary>
    ///     Returns the property name that the property is mapped to when targeting Cosmos.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>Returns the property name that the property is mapped to when targeting Cosmos.</returns>
    public static string GetJsonPropertyName(this IReadOnlyProperty property)
        => (string?)property[CosmosAnnotationNames.PropertyName]
            ?? GetDefaultJsonPropertyName(property);

    private static string GetDefaultJsonPropertyName(IReadOnlyProperty property)
    {
        var entityType = property.DeclaringType as IEntityType;
        var ownership = entityType?.FindOwnership();

        if (ownership != null
            && !entityType!.IsDocumentRoot())
        {
            var pk = property.FindContainingPrimaryKey();
            if (pk != null
                && ((property.ClrType == typeof(int) && property.IsShadowProperty())
                    || ownership.Properties.Contains(property))
                && pk.Properties.Count == ownership.Properties.Count + (ownership.IsUnique ? 0 : 1)
                && ownership.Properties.All(fkProperty => pk.Properties.Contains(fkProperty)))
            {
                return "";
            }
        }

        return property.Name;
    }

    /// <summary>
    ///     Sets the property name that the property is mapped to when targeting Cosmos.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="name">The name to set.</param>
    public static void SetJsonPropertyName(this IMutableProperty property, string? name)
        => property.SetOrRemoveAnnotation(
            CosmosAnnotationNames.PropertyName,
            name);

    /// <summary>
    ///     Sets the property name that the property is mapped to when targeting Cosmos.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="name">The name to set.</param>
    /// <param name="fromDataAnnotation">Indicates whether the configuration was specified using a data annotation.</param>
    /// <returns>The configured value.</returns>
    public static string? SetJsonPropertyName(
        this IConventionProperty property,
        string? name,
        bool fromDataAnnotation = false)
        => (string?)property.SetOrRemoveAnnotation(
            CosmosAnnotationNames.PropertyName,
            name,
            fromDataAnnotation)?.Value;

    /// <summary>
    ///     Gets the <see cref="ConfigurationSource" /> for the property name that the property is mapped to when targeting Cosmos.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>
    ///     The <see cref="ConfigurationSource" /> for the property name that the property is mapped to when targeting Cosmos.
    /// </returns>
    public static ConfigurationSource? GetJsonPropertyNameConfigurationSource(this IConventionProperty property)
        => property.FindAnnotation(CosmosAnnotationNames.PropertyName)?.GetConfigurationSource();

    /// <summary>
    ///     Returns the definition of the vector stored in this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>Returns the definition of the vector stored in this property.</returns>
    [Experimental(EFDiagnostics.CosmosVectorSearchExperimental)]
    public static CosmosVectorType? GetVectorType(this IReadOnlyProperty property)
        => (CosmosVectorType?)property[CosmosAnnotationNames.VectorType];

    /// <summary>
    ///     Sets the definition of the vector stored in this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="vectorType">The type of vector stored in the property.</param>
    [Experimental(EFDiagnostics.CosmosVectorSearchExperimental)]
    public static void SetVectorType(this IMutableProperty property, CosmosVectorType? vectorType)
        => property.SetOrRemoveAnnotation(CosmosAnnotationNames.VectorType, vectorType);

    /// <summary>
    ///     Sets the definition of the vector stored in this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="vectorType">The type of vector stored in the property.</param>
    /// <param name="fromDataAnnotation">Indicates whether the configuration was specified using a data annotation.</param>
    /// <returns>The configured value.</returns>
    [Experimental(EFDiagnostics.CosmosVectorSearchExperimental)]
    public static CosmosVectorType? SetVectorType(
        this IConventionProperty property,
        CosmosVectorType? vectorType,
        bool fromDataAnnotation = false)
        => (CosmosVectorType?)property.SetOrRemoveAnnotation(
            CosmosAnnotationNames.VectorType,
            vectorType,
            fromDataAnnotation)?.Value;

    /// <summary>
    ///     Gets the <see cref="ConfigurationSource" /> for the definition of the vector stored in this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>
    ///     The <see cref="ConfigurationSource" /> for the definition of the vector stored in this property.
    /// </returns>
    [Experimental(EFDiagnostics.CosmosVectorSearchExperimental)]
    public static ConfigurationSource? GetVectorTypeConfigurationSource(this IConventionProperty property)
        => property.FindAnnotation(CosmosAnnotationNames.VectorType)?.GetConfigurationSource();

    /// <summary>
    ///     Returns the value indicating whether full-text search is enabled for this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns><see langword="true" /> if full-text search is enabled for this property, <see langword="false" /> otherwise.</returns>
    public static bool? GetIsFullTextSearchEnabled(this IReadOnlyProperty property)
        => (bool?)property[CosmosAnnotationNames.IsFullTextSearchEnabled];

    /// <summary>
    ///     Enables full-text search for this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="enabled">Indicates whether full-text search is enabled for the property.</param>
    public static void SetIsFullTextSearchEnabled(this IMutableProperty property, bool? enabled)
        => property.SetAnnotation(CosmosAnnotationNames.IsFullTextSearchEnabled, enabled);

    /// <summary>
    ///     Enables full-text search for this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="enabled">Indicates whether full-text search is enabled for the property.</param>
    /// <param name="fromDataAnnotation">Indicates whether the configuration was specified using a data annotation.</param>
    /// <returns>The configured value.</returns>
    public static bool? SetIsFullTextSearchEnabled(
        this IConventionProperty property,
        bool? enabled,
        bool fromDataAnnotation = false)
        => (bool?)property.SetAnnotation(
            CosmosAnnotationNames.IsFullTextSearchEnabled,
            enabled,
            fromDataAnnotation)?.Value;

    /// <summary>
    ///     Gets the <see cref="ConfigurationSource" /> for enabling full-text search for this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>
    ///     The <see cref="ConfigurationSource" /> for enabling full-text search for this property.
    /// </returns>
    public static ConfigurationSource? GetIsFullTextSearchEnabledConfigurationSource(this IConventionProperty property)
        => property.FindAnnotation(CosmosAnnotationNames.IsFullTextSearchEnabled)?.GetConfigurationSource();

    /// <summary>
    ///     Returns the full-text search language defined for this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>The full-text search language for this property.</returns>
    public static string? GetFullTextSearchLanguage(this IReadOnlyProperty property)
        => (string?)property[CosmosAnnotationNames.FullTextSearchLanguage];

    /// <summary>
    ///     Sets the full-text search language defined for this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="language">The full-text search language for this property.</param>
    public static void SetFullTextSearchLanguage(this IMutableProperty property, string? language)
        => property.SetAnnotation(CosmosAnnotationNames.FullTextSearchLanguage, language);

    /// <summary>
    ///     Sets the full-text search language defined for this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="language">The full-text search language for the property.</param>
    /// <param name="fromDataAnnotation">Indicates whether the configuration was specified using a data annotation.</param>
    /// <returns>The configured value.</returns>
    public static string? SetFullTextSearchLanguage(
        this IConventionProperty property,
        string? language,
        bool fromDataAnnotation = false)
        => (string?)property.SetAnnotation(
            CosmosAnnotationNames.FullTextSearchLanguage,
            language,
            fromDataAnnotation)?.Value;

    /// <summary>
    ///     Gets the <see cref="ConfigurationSource" /> for the definition of the full-text search language for this property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>
    ///     The <see cref="ConfigurationSource" /> for the definition of full-text-search language for this property.
    /// </returns>
    public static ConfigurationSource? GetFullTextSearchLanguageConfigurationSource(this IConventionProperty property)
        => property.FindAnnotation(CosmosAnnotationNames.FullTextSearchLanguage)?.GetConfigurationSource();
}

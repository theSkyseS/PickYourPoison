using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PickYourPoison
{
    public record Ingredients
    {
        [JsonProperty] public List<Ingredient> IngredientsList = new();
    }

    public record Ingredient
    {
        [JsonProperty("name")]public string Name { get; set; } = string.Empty;
        [JsonProperty("effect 1")]public string EffectOne { get; set; } = string.Empty;
        [JsonProperty("effect 2")]public string EffectTwo { get; set; } = string.Empty;
        [JsonProperty("effect 3")]public string EffectThree { get; set; } = string.Empty;
        [JsonProperty("effect 4")]public string EffectFour { get; set; } = string.Empty;
        [JsonProperty("effect 1 magnitude")]public float EffectOneMagnitude { get; set; } = 1f;
        [JsonProperty("effect 2 magnitude")]public float EffectTwoMagnitude { get; set; } = 1f;
        [JsonProperty("effect 3 magnitude")]public float EffectThreeMagnitude { get; set; } = 1f;
        [JsonProperty("effect 4 magnitude")]public float EffectFourMagnitude { get; set; } = 1f;
        [JsonProperty("rarity")]public string Rarity { get; set; } = string.Empty;
        [JsonProperty("type")] public string Type { get; set; } = string.Empty;
        [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    }

    public record Effects
    {
        [JsonProperty] public List<Effect> EffectsList = new();
    }

    public record Effect
    {
        [JsonProperty("name")]public string Name { get; set; } = string.Empty;
        [JsonProperty("type")] public string Type { get; set; } = string.Empty;
    }
}
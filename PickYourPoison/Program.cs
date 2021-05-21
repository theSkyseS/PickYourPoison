using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Newtonsoft.Json;
using PickYourPoison.Settings;

namespace PickYourPoison
{
    public class Program
    {
        private static Lazy<Settings.Settings> _settings = null!;

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetAutogeneratedSettings("settings", "settings.json", out _settings)
                .SetTypicalOpen(GameRelease.SkyrimSE, "YourPatcher.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            const int titleSize = 30;
            const int subtitleSize = 20;
            const int textSize = 20;
            const int decsriptionSize = 16;

            var ingredientsFile = File.ReadAllText(state.RetrieveConfigFile("ingredients.json"));
            var effectsFile = File.ReadAllText(state.RetrieveConfigFile("effects.json"));
            Ingredients ingredients = JsonConvert.DeserializeObject<Ingredients>(ingredientsFile)!;
            Effects effects = JsonConvert.DeserializeObject<Effects>(effectsFile)!;
            
            if (_settings.Value.RunMode == Mode.AA)
            {
                effects.EffectsList.Add(new Effect
                {
                    Name = "Cure Poison",
                    Type = "Healing and Resistance"
                });
                effects.EffectsList.Add(new Effect
                {
                    Name = "Waterwalking",
                    Type = "Healing and Resistance"
                });
                ingredients.IngredientsList.ForEach(x =>
                {
                    switch (x.Name)
                    {
                        case "Slaughterfish Scales":
                            x.EffectOne = "Cure Poison";
                            break;
                        case "Charred Skeever Hide":
                            x.EffectThree = "Cure Poison";
                            break;
                        case "Swamp Fungal Pod":
                            x.EffectOne = "Cure Poison";
                            break;
                        case "Glowing Mushroom":
                            x.EffectFour = "Waterwalking";
                            break;
                        case "River Betty":
                            x.EffectFour = "Waterwalking";
                            break;
                    }
                });
            }
            else if (_settings.Value.RunMode == Mode.CACO)
            {
                var ingredientsCACOFile = File.ReadAllText(state.RetrieveConfigFile("ingredientsCACO.json"));
                var effectsCACOFile = File.ReadAllText(state.RetrieveConfigFile("effectsCACO.json"));
                var ingredientsCACO = JsonConvert.DeserializeObject<Ingredients>(ingredientsCACOFile)!;
                var effectsCACO = JsonConvert.DeserializeObject<Effects>(effectsCACOFile)!;
                ingredients.IngredientsList.AddRange(ingredientsCACO.IngredientsList);
                effects.EffectsList.AddRange(effectsCACO.EffectsList);
                foreach (Ingredient ingredient in ingredients.IngredientsList)
                    switch (ingredient.Name)
                    {
                        case "Abecean Longfin":
                            ingredient.EffectOne = "Weakness to Frost";
                            ingredient.EffectTwo = "Fortify Light Armor";
                            ingredient.EffectThree = "Fortify Restoration";
                            ingredient.EffectFour = "Weakness to Poison";
                            break;
                        case "Ancestor Moth":
                            ingredient.EffectOne = "Damage Magicka Regen";
                            ingredient.EffectTwo = "Detect Life";
                            ingredient.EffectThree = "Fortify Conjuration";
                            ingredient.EffectFour = "Fortify Enchanting";
                            break;
                        case "Ash Hopper Jelly":
                            ingredient.EffectOne = "Restore Health";
                            ingredient.EffectTwo = "Weakness to Frost";
                            ingredient.EffectThree = "Fortify Illusion";
                            ingredient.EffectFour = "Fortify Light Armor";
                            break;
                        case "Bear Claws":
                            ingredient.EffectOne = "Fortify Health";
                            ingredient.EffectTwo = "Restore Stamina";
                            ingredient.EffectThree = "Fortify One-handed";
                            ingredient.EffectFour = "Fortify Block";
                            break;
                        case "Beehive Husk":
                            ingredient.EffectOne = "Fortify Light Armor";
                            ingredient.EffectTwo = "Cure Poison";
                            ingredient.EffectThree = "Fortify Sneak";
                            ingredient.EffectFour = "Fortify Destruction";
                            break;
                        case "Bleeding Crown":
                            ingredient.EffectOne = "Weakness to Fire";
                            ingredient.EffectTwo = "Weakness to Poison";
                            ingredient.EffectThree = "Fortify Light Armor";
                            ingredient.EffectFour = "Fortify Block";
                            break;
                        case "Blue Mountain Flower":
                            ingredient.EffectOne = "Fortify Conjuration";
                            ingredient.EffectTwo = "Fortify Magicka";
                            ingredient.EffectThree = "Damage Health";
                            ingredient.EffectFour = "Restore Magicka";
                            break;
                        case "Bone Meal":
                            ingredient.EffectOne = "Damage Stamina";
                            ingredient.EffectTwo = "Fortify Conjuration";
                            ingredient.EffectThree = "Resist Fire";
                            ingredient.EffectFour = "Night Eye";
                            break;
                        case "Briar Heart":
                            ingredient.EffectOne = "Fortify Health";
                            ingredient.EffectTwo = "Fortify Destruction";
                            ingredient.EffectThree = "Fortify Magicka";
                            ingredient.EffectFour = "Paralysis";
                            break;

                        case "Canis Root":
                            ingredient.EffectOne = "Damage Stamina";
                            ingredient.EffectTwo = "Fortify Marksman";
                            ingredient.EffectThree = "Silence";
                            ingredient.EffectFour = "Fortify Light Armor";
                            break;

                        case "Chaurus Eggs":
                            ingredient.EffectOne = "Weakness to Poison";
                            ingredient.EffectTwo = "Night Eye";
                            ingredient.EffectThree = "Invisibility";
                            ingredient.EffectFour = "Damage Magicka";
                            break;

                        case "Chicken Egg":
                            ingredient.EffectOne = "Fortify Alteration";
                            ingredient.EffectTwo = "Resist Paralysis";
                            ingredient.EffectThree = "Resist Poison";
                            ingredient.EffectFour = "Damage Magicka Regen";
                            break;
                        case "Crimson Nirnroot":
                            ingredient.EffectOne = "Damage Health";
                            ingredient.EffectTwo = "Invisibility";
                            ingredient.EffectThree = "Resist Magic";
                            ingredient.EffectFour = "Drain Strength";
                            break;
                        case "Daedra Heart":
                            ingredient.EffectOne = "Restore Health";
                            ingredient.EffectTwo = "Fear";
                            ingredient.EffectThree = "Silence";
                            ingredient.EffectFour = "Damage Magicka";
                            break;
                        case "Dwarven Oil":
                            ingredient.EffectOne = "Weakness to Magic";
                            ingredient.EffectTwo = "Fortify Sneak";
                            ingredient.EffectThree = "Fortify Lockpicking";
                            ingredient.EffectFour = "Fortify Illusion";
                            break;
                        case "Ectoplasm":
                            ingredient.EffectOne = "Etherealize";
                            ingredient.EffectTwo = "Fortify Illusion";
                            ingredient.EffectThree = "Fortify Magicka";
                            ingredient.EffectFour = "Damage Health";
                            break;
                        case "Elves Ear":
                            ingredient.EffectOne = "Restore Magicka";
                            ingredient.EffectTwo = "Restore Health";
                            ingredient.EffectThree = "Weakness to Frost";
                            ingredient.EffectFour = "Resist Fire";
                            break;

                        case "Emperor Parasol Moss":
                            ingredient.EffectOne = "Regenerate Health";
                            ingredient.EffectTwo = "Fortify Two-handed";
                            ingredient.EffectThree = "Damage Health";
                            ingredient.EffectFour = "Fortify Heavy Armor";
                            break;

                        case "Fire Salts":
                            ingredient.EffectOne = "Weakness to Fire";
                            ingredient.EffectTwo = "Resist Frost";
                            ingredient.EffectThree = "Regenerate Magicka";
                            ingredient.EffectFour = "Restore Magicka";
                            break;

                        case "Frost Mirriam":
                            ingredient.EffectOne = "Resist Frost";
                            ingredient.EffectTwo = "Fortify Stamina";
                            ingredient.EffectThree = "Fortify Magicka";
                            ingredient.EffectFour = "Weakness to Fire";
                            break;

                        case "Frost Salts":
                            ingredient.EffectOne = "Weakness to Frost";
                            ingredient.EffectTwo = "Resist Fire";
                            ingredient.EffectThree = "Damage Health";
                            ingredient.EffectFour = "Restore Magicka";
                            break;

                        case "Garlic":
                            ingredient.EffectOne = "Resist Disease";
                            ingredient.EffectTwo = "Regenerate Stamina";
                            ingredient.EffectThree = "Regenerate Health";
                            ingredient.EffectFour = "Regenerate Magicka";
                            break;
                        case "Giant's Toe":
                            ingredient.EffectOne = "Fortify Health";
                            ingredient.EffectTwo = "Feather";
                            ingredient.EffectThree = "Damage Stamina Regen";
                            ingredient.EffectFour = "Drain Strength";
                            break;
                        case "Gleamblossom":
                            ingredient.EffectOne = "Etherealize";
                            ingredient.EffectTwo = "Paralysis";
                            ingredient.EffectThree = "Regenerate Health";
                            ingredient.EffectFour = "Fear";
                            break;
                        case "Glowing Mushroom":
                            ingredient.EffectOne = "Etherealize";
                            ingredient.EffectTwo = "Resist Shock";
                            ingredient.EffectThree = "Fortify Destruction";
                            ingredient.EffectFour = "Fortify Health";
                            break;
                        case "Grass Pod":
                            ingredient.EffectOne = "Drain Intelligence";
                            ingredient.EffectTwo = "Cure Poison";
                            ingredient.EffectThree = "Fortify Alteration";
                            ingredient.EffectFour = "Restore Magicka";
                            break;

                        case "Hagraven Claw":
                            ingredient.EffectOne = "Fortify Enchanting";
                            ingredient.EffectTwo = "Fortify Speech";
                            ingredient.EffectThree = "Drain Strength";
                            ingredient.EffectFour = "Resist Magic";
                            break;

                        case "Hanging Moss":
                            ingredient.EffectOne = "Damage Magicka Regen";
                            ingredient.EffectTwo = "Fortify Stamina";
                            ingredient.EffectThree = "Fortify One-handed";
                            ingredient.EffectFour = "Damage Magicka";
                            break;

                        case "Hawk Beak":
                            ingredient.EffectOne = "Feather";
                            ingredient.EffectTwo = "Resist Shock";
                            ingredient.EffectThree = "Fortify Marksman";
                            ingredient.EffectFour = "Regenerate Stamina";
                            break;

                        case "Hawk Egg":
                            ingredient.EffectOne = "Damage Undead";
                            ingredient.EffectTwo = "Night Eye";
                            ingredient.EffectThree = "Fortify Smithing";
                            ingredient.EffectFour = "Lingering Damage Undead";
                            break;

                        case "Hawk Feathers":
                            ingredient.EffectOne = "Fortify Speed";
                            ingredient.EffectTwo = "Cure Disease";
                            ingredient.EffectThree = "Fortify Light Armor";
                            ingredient.EffectFour = "Fortify One-handed";
                            break;

                        case "Histcarp":
                            ingredient.EffectOne = "Damage Stamina Regen";
                            ingredient.EffectTwo = "Restore Stamina";
                            ingredient.EffectThree = "Waterbreathing";
                            ingredient.EffectFour = "Resist Disease";
                            break;

                        case "Honeycomb":
                            ingredient.EffectOne = "Regenerate Magicka";
                            ingredient.EffectTwo = "Fortify One-handed";
                            ingredient.EffectThree = "Fatigue";
                            ingredient.EffectFour = "Fortify Illusion";
                            break;

                        case "Human Heart":
                            ingredient.EffectOne = "Regenerate Stamina";
                            ingredient.EffectTwo = "Detect Life";
                            ingredient.EffectThree = "Damage Magicka";
                            ingredient.EffectFour = "Damage Magicka Regen";
                            break;

                        case "Ice Wraith Teeth":
                            ingredient.EffectOne = "Weakness to Frost";
                            ingredient.EffectTwo = "Regenerate Magicka";
                            ingredient.EffectThree = "Etherealize";
                            ingredient.EffectFour = "Fortify Heavy Armor";
                            break;

                        case "Imp Stool":
                            ingredient.EffectOne = "Lingering Damage Health";
                            ingredient.EffectTwo = "Regenerate Health";
                            ingredient.EffectThree = "Restore Health";
                            ingredient.EffectFour = "Damage Health";
                            break;

                        case "Jarrin Root":
                            ingredient.EffectOne = "Damage Health";
                            ingredient.EffectTwo = "Damage Stamina";
                            ingredient.EffectThree = "Damage Magicka";
                            ingredient.EffectFour = "Drain Strength";
                            break;

                        case "Lavender":
                            ingredient.EffectOne = "Fortify Speech";
                            ingredient.EffectTwo = "Fortify Stamina";
                            ingredient.EffectThree = "Drain Intelligence";
                            ingredient.EffectFour = "Fortify Conjuration";
                            break;

                        case "Mora Tapinella":
                            ingredient.EffectOne = "Fortify Alteration";
                            ingredient.EffectTwo = "Fortify Illusion";
                            ingredient.EffectThree = "Fortify Magicka";
                            ingredient.EffectFour = "Lingering Damage Health";
                            break;

                        case "Mudcrab Chitin":
                            ingredient.EffectOne = "Resist Disease";
                            ingredient.EffectTwo = "Cure Disease";
                            ingredient.EffectThree = "Restore Stamina";
                            ingredient.EffectFour = "Resist Fire";
                            break;

                        case "Netch Jelly":
                            ingredient.EffectOne = "Restore Stamina";
                            ingredient.EffectTwo = "Resist Paralysis";
                            ingredient.EffectThree = "Feather";
                            ingredient.EffectFour = "Paralysis";
                            break;

                        case "Nirnroot":
                            ingredient.EffectOne = "Damage Health";
                            ingredient.EffectTwo = "Invisibility";
                            ingredient.EffectThree = "Resist Magic";
                            ingredient.EffectFour = "Drain Strength";
                            break;

                        case "Orange Dartwing":
                            ingredient.EffectOne = "Fortify Pickpocket";
                            ingredient.EffectTwo = "Lingering Damage Health";
                            ingredient.EffectThree = "Silence";
                            ingredient.EffectFour = "Fortify Speed";
                            break;

                        case "Powdered Mammoth Tusk":
                            ingredient.EffectOne = "Fortify Block";
                            ingredient.EffectTwo = "Weakness to Fire";
                            ingredient.EffectThree = "Fortify Unarmed";
                            ingredient.EffectFour = "Fear";
                            break;

                        case "Purple Mountain Flower":
                            ingredient.EffectOne = "Fortify Sneak";
                            ingredient.EffectTwo = "Fortify Marksman";
                            ingredient.EffectThree = "Lingering Damage Undead";
                            ingredient.EffectFour = "Restore Stamina";
                            break;

                        case "Red Mountain Flower":
                            ingredient.EffectOne = "Drain Intelligence";
                            ingredient.EffectTwo = "Fortify Health";
                            ingredient.EffectThree = "Damage Magicka Regen";
                            ingredient.EffectFour = "Restore Health";
                            break;

                        case "Rock Warbler Egg":
                            ingredient.EffectOne = "Restore Health";
                            ingredient.EffectTwo = "Cure Poison";
                            ingredient.EffectThree = "Weakness to Magic";
                            ingredient.EffectFour = "Damage Stamina";
                            break;

                        case "Sabre Cat Eye":
                            ingredient.EffectOne = "Night Eye";
                            ingredient.EffectTwo = "Restore Stamina";
                            ingredient.EffectThree = "Ravage Health";
                            ingredient.EffectFour = "Damage Magicka";
                            break;

                        case "Sabre Cat Tooth":
                            ingredient.EffectOne = "Fortify Stamina";
                            ingredient.EffectTwo = "Weakness to Poison";
                            ingredient.EffectThree = "Fortify One-handed";
                            ingredient.EffectFour = "Fortify Heavy Armor";
                            break;

                        case "Salt Pile":
                            ingredient.EffectOne = "Fortify Unarmed";
                            ingredient.EffectTwo = "Regenerate Stamina";
                            ingredient.EffectThree = "Weakness to Magic";
                            ingredient.EffectFour = "Damage Stamina Regen";
                            break;

                        case "Scaly Pholiota":
                            ingredient.EffectOne = "Feather";
                            ingredient.EffectTwo = "Regenerate Stamina";
                            ingredient.EffectThree = "Fortify Speed";
                            ingredient.EffectFour = "Weakness to Magic";
                            break;

                        case "Skeever Tail":
                            ingredient.EffectOne = "Damage Stamina Regen";
                            ingredient.EffectTwo = "Fortify Sneak";
                            ingredient.EffectThree = "Ravage Health";
                            ingredient.EffectFour = "Damage Health";
                            break;

                        case "Slaughterfish Egg":
                            ingredient.EffectOne = "Waterbreathing";
                            ingredient.EffectTwo = "Fortify Stamina";
                            ingredient.EffectThree = "Lingering Damage Health";
                            ingredient.EffectFour = "Fortify Pickpocket";
                            break;

                        case "Slaughterfish Scales":
                            ingredient.EffectOne = "Fortify Block";
                            ingredient.EffectTwo = "Lingering Damage Health";
                            ingredient.EffectThree = "Fortify Heavy Armor";
                            ingredient.EffectFour = "Waterwalking";
                            break;

                        case "Small Antlers":
                            ingredient.EffectOne = "Fortify Restoration";
                            ingredient.EffectTwo = "Damage Undead";
                            ingredient.EffectThree = "Weakness to Poison";
                            ingredient.EffectFour = "Fortify Speed";
                            break;

                        case "Spider Egg":
                            ingredient.EffectOne = "Fortify Marksman";
                            ingredient.EffectTwo = "Damage Stamina";
                            ingredient.EffectThree = "Damage Magicka Regen";
                            ingredient.EffectFour = "Detect Life";
                            break;

                        case "Swamp Fungal Pod":
                            ingredient.EffectOne = "Lingering Damage Undead";
                            ingredient.EffectTwo = "Resist Shock";
                            ingredient.EffectThree = "Damage Magicka Regen";
                            ingredient.EffectFour = "Paralysis";
                            break;

                        case "Torchbug Abdomen":
                            ingredient.EffectOne = "Lingering Damage Undead";
                            ingredient.EffectTwo = "Fortify Stamina";
                            ingredient.EffectThree = "Weakness to Magic";
                            ingredient.EffectFour = "Detect Life";
                            break;

                        case "Trama Root":
                            ingredient.EffectOne = "Weakness to Shock";
                            ingredient.EffectTwo = "Damage Magicka";
                            ingredient.EffectThree = "Drain Strength";
                            ingredient.EffectFour = "Feather";
                            break;

                        case "Troll Fat":
                            ingredient.EffectOne = "Resist Disease";
                            ingredient.EffectTwo = "Frenzy";
                            ingredient.EffectThree = "Fortify Two-handed";
                            ingredient.EffectFour = "Lingering Damage Health";
                            break;

                        case "Tundra Cotton":
                            ingredient.EffectOne = "Fortify Unarmed";
                            ingredient.EffectTwo = "Resist Paralysis";
                            ingredient.EffectThree = "Fortify Block";
                            ingredient.EffectFour = "Fortify Speech";
                            break;

                        case "Vampire Dust":
                            ingredient.EffectOne = "Regenerate Health";
                            ingredient.EffectTwo = "Silence";
                            ingredient.EffectThree = "Invisibility";
                            ingredient.EffectFour = "Cure Disease";
                            break;

                        case "Void Salts":
                            ingredient.EffectOne = "Weakness to Shock";
                            ingredient.EffectTwo = "Resist Shock";
                            ingredient.EffectThree = "Silence";
                            ingredient.EffectFour = "Restore Magicka";
                            break;

                        case "White Cap":
                            ingredient.EffectOne = "Weakness to Frost";
                            ingredient.EffectTwo = "Drain Intelligence";
                            ingredient.EffectThree = "Fortify Restoration";
                            ingredient.EffectFour = "Fortify Heavy Armor";
                            break;

                        case "Wisp Wrappings":
                            ingredient.EffectOne = "Etherealize";
                            ingredient.EffectTwo = "Detect Life";
                            ingredient.EffectThree = "Fortify Destruction";
                            ingredient.EffectFour = "Resist Magic";
                            break;
                    }

                effects.EffectsList.ForEach(effect =>
                {
                    switch (effect.Name)
                    {
                        case "Ravage Magicka":
                            effect.Name = "Silence";
                            effect.Type = "Weaknesses and Manipulation";
                            break;
                        case "Ravage Stamina":
                            effect.Name = "Fatigue";
                            effect.Type = "Weaknesses and Manipulation";
                            break;
                        case "Fortify Barter":
                            effect.Name = "Fortify Speech";
                            effect.Type = "Fortification: Mages & Thieves";
                            break;
                        case "Lingering Damage Magicka":
                            effect.Name = "Damage Undead";
                            effect.Type = "Poisons";
                            break;
                        case "Lingering Damage Stamina":
                            effect.Name = "Lingering Damage Undead";
                            effect.Type = "Poisons";
                            break;
                    }
                });
            }

            ingredients.IngredientsList = ingredients.IngredientsList.OrderBy(x => x.Name).ToList();
            effects.EffectsList = effects.EffectsList.OrderBy(x => x.Name).ToList();
            SortedSet<string> ingredientTypes = new();
            SortedSet<string> effectTypes = new();
            ingredients.IngredientsList.ForEach(x => ingredientTypes.Add(x.Type));
            effects.EffectsList.ForEach(x => effectTypes.Add(x.Type));
            string GenerateTitle(string type, int volume)
            {
                const string title = "Pick Your Poison";
                string subtitle = $"Vol {volume}, {type}";
                return
                    $"\n\n<p align='center'><font size='{titleSize}'>{title}</font>\n<font size='{subtitleSize}'>{subtitle}</font></p>\n[pagebreak]\n";
            }

            string WriteIngredientBook(string ingredientType, int volume)
            {
                string text = GenerateTitle(ingredientType, volume);
                text += "<p align='left'>";
                text = ingredients.IngredientsList
                    .Where(ingredient => ingredient.Type == ingredientType)
                    .Aggregate(text,
                        (current, ingredient) =>
                            current +
                            $"<font size='{textSize}'>{ingredient.Name}</font>\n<font size='{decsriptionSize}'>{ingredient.Description}\n\n</font>");
                text += "</p>";
                return text;
            }

            string WriteEffectBook(string effectType, int volume)
            {
                string text = GenerateTitle(effectType, volume);
                text += "<p align='left'>";
                foreach (Effect effect in effects.EffectsList)
                {
                    if (effect.Type != effectType) continue;
                    string effectText =
                        $"<font size='{textSize}'>{effect.Name}</font>\n<font size='{decsriptionSize}'>";
                    foreach (string ingredientType in ingredientTypes)
                    {
                        string effectTextRow = $"{ingredientType}: ";
                        var added = false;
                        foreach (var ingredient in ingredients.IngredientsList.Where(ingredient =>
                            ingredient.Type.Equals(ingredientType) && (ingredient.EffectOne == effect.Name ||
                                                                       ingredient.EffectTwo == effect.Name ||
                                                                       ingredient.EffectThree == effect.Name ||
                                                                       ingredient.EffectFour == effect.Name)))
                        {
                            effectTextRow += $"{ingredient.Name}, ";
                            added = true;
                        }

                        if (added)
                        {
                            effectText += effectTextRow + "\n";
                        }
                    }
                    effectText += "\n</font>";
                    text += effectText;
                }
                text += "</p>";
                return text;
            }

            List<string> books = new()
            {
                WriteIngredientBook("Fauna", 1),
                WriteIngredientBook("Fishes", 2),
                WriteIngredientBook("Flora", 3),
                WriteIngredientBook("Flowers", 4),
                WriteIngredientBook("Fungi", 5),
                WriteIngredientBook("Insects", 6),
                WriteIngredientBook("Miscellanea", 7),
                WriteIngredientBook("Supernatural", 8),
                WriteEffectBook("Fortification: Mages & Thieves", 9),
                WriteEffectBook("Fortification: Warriors & Misc.", 10),
                WriteEffectBook("Healing and Resistance", 11),
                WriteEffectBook("Poisons", 12),
                WriteEffectBook("Weaknesses and Manipulation", 13),
            };

            List<(string, string)> bookNames = new()
            {
                ("Pick Your Poison, Vol. 01: Fauna", "PyP, v.01: Fauna"),
                ("Pick Your Poison, Vol. 02: Fishes", "PyP, v.02: Fishes"),
                ("Pick Your Poison, Vol. 03: Flora", "PyP, v.03: Flora"),
                ("Pick Your Poison, Vol. 04: Flowers", "PyP, v.04: Flowers"),
                ("Pick Your Poison, Vol. 05: Fungi", "PyP, v.05: Fungi"),
                ("Pick Your Poison, Vol. 06: Insects", "PyP, v.06: Insects"),
                ("Pick Your Poison, Vol. 07: Miscellanea", "PyP, v.07: Misc."),
                ("Pick Your Poison, Vol. 08: Supernatural", "PyP, v.08: Supernatural"),
                ("Pick Your Poison, Vol. 09: Fortification: Mages & Thieves", "PyP, v.09: Fortification A"),
                ("Pick Your Poison, Vol. 10: Fortification: Warriors & Misc", "PyP, v.10: Fortification B"),
                ("Pick Your Poison, Vol. 11: Healing and Resistance", "PyP, v.11: Healing"),
                ("Pick Your Poison, Vol. 12: Poisons", "PyP, v.12: Poisons"),
                ("Pick Your Poison, Vol. 13: Weaknesses & Manipulation", "PyP, v.13: Manipulation")

            };

            for (var i = 0; i < FormKeys.PickYourPoison.Book.Books.Count; i++)
            {
                var book = (FormLink<IBookGetter>) FormKeys.PickYourPoison.Book.Books[i];
                var bookCopy = state.PatchMod.Books.GetOrAddAsOverride(book, state.LinkCache);
                bookCopy.BookText = books[i];
                bookCopy.Name = _settings.Value.LongDescriptions ? bookNames[i].Item1 : bookNames[i].Item2;
            }
        }
    }
}
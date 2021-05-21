using System.Collections.Generic;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;

namespace PickYourPoison.FormKeys
{
    public class PickYourPoison
    {
        public static readonly ModKey ModKey = ModKey.FromNameAndExtension("pick_your_poison.esp");

        public static class Book
        {
            private static FormLink<IBookGetter> Construct(uint id) => ModKey.MakeFormKey(id);

            public static readonly List<IFormLink<IBookGetter>> Books = new()
            {
                Construct(0x005901),
                Construct(0x005902),
                Construct(0x005904),
                Construct(0x005905),
                Construct(0x005906),
                Construct(0x005907),
                Construct(0x005908),
                Construct(0x005909),
                Construct(0x00590A),
                Construct(0x00590B),
                Construct(0x00590C),
                Construct(0x00590D),
                Construct(0x00590E)
            };
        }
    }
}
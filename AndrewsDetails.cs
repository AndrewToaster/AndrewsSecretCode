using Smod2;
using Smod2.Attributes;
using System;

namespace AndrewsSecretCode
{
    public sealed class AndrewsDetails : PluginDetails
    {
        public const string ID_PREFIX = "andrewsmods.";

        public AndrewsDetails(string pluginId)
        {
            author = "AndrewToasterr";
            name = "No Name";
            description = "No Description";
            id = ID_PREFIX + pluginId;
            SmodMajor = PluginManager.SMOD_MAJOR;
            SmodMinor = PluginManager.SMOD_MINOR;
            SmodRevision = PluginManager.SMOD_REVISION;
        }
    }
}

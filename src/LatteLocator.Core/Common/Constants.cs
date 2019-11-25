using System;

namespace LatteLocator.Core.Common
{
    public static class Constants
    {
        private const string MapServiceToken = "";

        public static string GetMapServiceToken()
        {
            if(string.IsNullOrEmpty(MapServiceToken))
            {
                throw new Exception("Missing MapControl MapService Key. Visit https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/ to learn how to get yours.");
            }

            return MapServiceToken;
        }

        public static string UpdatedStarbucksEndpoint = "https://www.starbucks.com/bff/locations";
        // No longer needed with new API
        //public static string StarbucksEndpoint = "http://www.starbucks.com/api/location.ashx";
        //public static string AccessToken = "no longer user, but leaving it here just in case";
        //public static string ApiKey = "no longer user, but leaving it here just in case";

        //Segoe NOTE MDL2 Assets
        public static string MapGlyph = "";
        public static string HomeGlyph = "";
        public static string GlobeGlyph = "";
        public static string AboutGlyph = "";
        public static string WarningGlyph = "";
        public static string QuestionGlyph = "";
        public static string HelpGlyph = "";
        public static string SettingsGlyph = "";
        public static string CalendarThinGlyph = "";
        public static string ChartGlyph = "";
        public static string VideoProtectionGlyph = "";
        public static string KeyGlyph = "";
        public static string VideoTrimGlyph = "";
        public static string VideoPlaybackGlyph = "";
        public static string FilmStripGlyph = "";
        public static string VideoRecordingGlyph = "";
        public static string StoreGlyph = "";
        public static string WebcamGlyph = "";
        public static string SwitchCameraGlyph = "";
        public static string ImportVideoGlyph = "";
        public static string ScissorsGlyph = "";
        public static string MusicGlyph = "";
        public static string UnlockGlyph = "";
        public static string LockGlyph = "";

        public const string UnlockIcon = "🔓";
        public const string LockIcon = "🔒";
    }
}

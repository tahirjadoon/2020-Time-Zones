using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZ.TimeZonesTest
{
    //Run the app with CTRL+F5 and it will remin open 
    //When pressing F5 the app will close autometically. We can either do Console.ReadLine(); or Console.ReadKey(); and wait. 
    class Program
    {
        private static readonly TimeZoneInfo _cutsomTimeZone = TimeZoneInfo.FindSystemTimeZoneById(CustomTimeZone);

        //Windows System Time Zone Ids
        //CustomTimeZone is the basic unit in this example
        private const string CustomTimeZone = "Eastern Standard Time";
        private const string OtherTimeZone = "Central Standard Time";

        static void Main(string[] args)
        {
            CustomTimeZone.WriteNewLine();
            var easternOffset = GetOffsetMinutes(CustomTimeZone);
            $"{CustomTimeZone} Offset (UTC): {easternOffset}".WriteNewLine(WriteLine.ColorCyan);
            "".RevertColor();

            "".EmptyLine();
            OtherTimeZone.WriteNewLine();
            var otherOffset = GetOffsetMinutes(OtherTimeZone);
            $"{OtherTimeZone} Offset (UTC): {otherOffset}".WriteNewLine(WriteLine.ColorBlue);
            "".RevertColor();

            "".EmptyLine();
            $"Convert the '{CustomTimeZone}' to '{OtherTimeZone}'".WriteNewLine();
            var customZoneDate = new DateTime(2020, 05, 06, 2, 57, 32);
            var otherZoneDateConverted = ConvertToOffset(customZoneDate, OtherTimeZone, true);
            $"{CustomTimeZone} Date: {customZoneDate.ToString("yyyy-MM-dd HH:mm:ss")}".WriteNewLine(WriteLine.ColorCyan);
            $"{OtherTimeZone} Date: {otherZoneDateConverted.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss")}".WriteNewLine(WriteLine.ColorBlue);
            "".RevertColor();

            "".EmptyLine();
            $"Converting above '{CustomTimeZone}' and '{OtherTimeZone}' to UTC".WriteNewLine();
            var customZoneDateToUtc = ConvertToUtc(customZoneDate, CustomTimeZone);
            var otherZoneDateConvertedToUtc = ConvertToUtc(otherZoneDateConverted, OtherTimeZone);
            $"{CustomTimeZone} Date to UTC: {customZoneDateToUtc.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss")}".WriteNewLine(WriteLine.ColorCyan);
            $"{OtherTimeZone} Date to UTC: {otherZoneDateConvertedToUtc.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss")}".WriteNewLine(WriteLine.ColorBlue);
            "".RevertColor();

            "".EmptyLine();
            "Displaying all the system time zone ids".WriteNewLine();
            "ID | DisplayName | StandardName | DaylightName | BaseUtcOffset".WriteNewLine(WriteLine.ColorCyan);
            "".RevertColor();
            foreach (var zone in TimeZoneInfo.GetSystemTimeZones())
            {
                $"{zone.Id} | {zone.DisplayName} | {zone.StandardName} | {zone.DaylightName} | {zone.BaseUtcOffset}".WriteNewLine();
            }

            "".EmptyLine();
            "".EmptyLine();
            "Press any key to exit >>".WriteSameLine(WriteLine.ColorRed);
            Console.ReadKey();
        }

        /// <summary>
        /// Offset minutes to GMT. Takes the daylight into account
        /// </summary>
        /// <param name="timeZoneId">The system time zone id</param>
        /// <returns>Double minutes</returns>
        private static double GetOffsetMinutes(string timeZoneId)
        {
            //take day light into account
            var utc = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId ?? CustomTimeZone);
            var minutes = (TimeZoneInfo.ConvertTimeFromUtc(utc, zone) - utc).TotalMinutes;
            return minutes;
        }


        /// <summary>
        /// converts the time from one standard time to another
        /// </summary>
        /// <param name="date">The Date. If convertToUtc=false then the date will be treated as GMT</param>
        /// <param name="timeZoneId">The convert to time zone id</param>
        /// <param name="convertToUtc">When true the date will be first converted to UTC using the CustomTimeZone</param>
        /// <returns></returns>
        private static DateTime? ConvertToOffset(DateTime? date, string timeZoneId, bool convertToUtc = true)
        {
            if (date == null)
                return null;

            var utcTime = convertToUtc ? TimeZoneInfo.ConvertTimeToUtc(date ?? DateTime.MinValue, _cutsomTimeZone) : date ?? DateTime.MinValue;

            return DateTime.Parse(TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)).ToString());
        }

        private static DateTime? ConvertToUtc(DateTime? date, string timeZoneId)
        {
            if (date == null)
                return null;
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(date ?? DateTime.MinValue, _cutsomTimeZone);
            return utcTime;
        }
    }

    public static class WriteLine
    {
        public static ConsoleColor ColorBlue = ConsoleColor.Blue;
        public static ConsoleColor ColorCyan = ConsoleColor.Cyan;
        public static ConsoleColor ColorGreen = ConsoleColor.Green;
        public static ConsoleColor ColorRed = ConsoleColor.Red;
        public static ConsoleColor ColorWhite = ConsoleColor.White;
        public static ConsoleColor ColorYellow = ConsoleColor.Yellow;

        public static void WriteNewLine(this string message, ConsoleColor? color = null)
        {
            if (color != null)
                Console.ForegroundColor = color.GetValueOrDefault();
            if (!string.IsNullOrWhiteSpace(message))
                Console.WriteLine(message);
        }

        public static void WriteSameLine(this string message, ConsoleColor? color = null)
        {
            if (color != null)
                Console.ForegroundColor = color.GetValueOrDefault();
            if (!string.IsNullOrWhiteSpace(message))
                Console.Write(message);
        }

        public static void RevertColor(this string message)
        {
            Console.ForegroundColor = ColorWhite;
        }

        public static void EmptyLine(this string message)
        {
            Console.WriteLine(string.Empty);
        }

        public static void EmptySameLine(this string message)
        {
            Console.Write(string.Empty);
        }
    }
}

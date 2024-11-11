namespace CarSellerCore.Util;

public class DataUtil
{
    public static string FormatDate(string dateString)
    {
        if (DateTime.TryParse(dateString, out DateTime date))
        {
            int day = date.Day;
            string suffix = GetDaySuffix(day);
            string formattedDate = $"{day}{suffix} {date:MMMM yyyy, h:mm tt}";
            return formattedDate;
        }

        return string.Empty;
    }

    private static string GetDaySuffix(int day)
    {
        if (day is >= 11 and <= 13)
        {
            return "th";
        }

        int reminder = day % 10;
        return reminder switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th"
        };
    }
}
using System;
using System.IO;

namespace StudentAssistant.Tests.Helpers
{
    public static class TestValueProvider
    {
        public static string GetValueStringByFlag(int flag)
        {
            switch (flag)
            {
                case 1:
                    return Path.Combine("TestFiles", "examScheduleFileTest.xls");
                case 2:
                    return @"https://www.mirea.ru/upload/medialibrary/39e/KBiSP-4-kurs-2-sem.xlsx";
                case 3:
                    return Guid.NewGuid().ToString();
                case 4:
                    return Path.Combine("TestFiles");
                case 5:
                    return Path.Combine("TestFiles", "scheduleFileTest.xlsx");

                default:
                    return string.Empty;
            } 
        }
    }
}
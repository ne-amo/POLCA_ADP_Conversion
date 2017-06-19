using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace POLCA_Export_ADP_Converter
{
    class Program
    {
        private const string YFB = "YFB";
        private const string OVERTIME = "WOT";
        private const string DOUBLETIME = "WDT";

        static void Main(string[] args)
        {
           if(args == null)
               return;
           if (args.Length == 0)
               return;

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            var calendarWeek = 0;
                
            var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            var tempPath = appPath + "\\temp.csv";
            using (var writer = new CsvWriter(File.CreateText(tempPath)))
            {
                using (var parser = new CsvParser(File.OpenText(args[0])))
                {
                    bool header = true;
                    while (true)
                    {
                        var row = parser.Read();
                        if (row == null)
                        {
                            break;
                        }
                        if (header)
                        {
                            writer.WriteField(row[0]);
                            writer.WriteField(row[1]);
                            writer.WriteField(row[2]);
                            writer.WriteField(row[3]);
                            writer.WriteField(row[4]);
                            writer.WriteField(row[5]);
                            writer.WriteField(row[6]);
                            writer.WriteField(row[7]);
                            writer.WriteField(row[8]);
                            writer.WriteField(row[9]);
                            writer.WriteField(row[10]);
                            writer.WriteField(row[11]);
                            writer.WriteField(row[12]);
                            writer.WriteField(row[13]);
                            writer.WriteField(row[14]);
                            header = false;
                            continue;
                        }
                        if (calendarWeek == 0)
                        {
                            //https://msdn.microsoft.com/en-us/library/system.globalization.calendar.getweekofyear(v=vs.110).aspx

                            var weekEnding = DateTime.Parse(row[12]);
                            calendarWeek = cal.GetWeekOfYear(weekEnding, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                        }
                        // Column 0 = YFB
                        writer.WriteField(YFB);
                        // Column 1 = Week Number
                        writer.WriteField(calendarWeek.ToString());
                        // Column 2 = Employee ID
                        writer.WriteField(row[2]);
                        // Column 3 = Department
                        writer.WriteField(row[3]);
                        // Column 4 = payrate
                        writer.WriteField(row[4]);
                        // Column 5 = prod hours
                        writer.WriteField(row[5]);

                        if (!string.IsNullOrEmpty(row[6]) && row[6] != "NULL")
                        {
                            // Column 6 = overtime hours
                            writer.WriteField("NULL");
                            // Column 7 = other hours code
                            writer.WriteField(OVERTIME);
                            // Column 8 = other hours hours
                            var overTimeHours = row[6];
                            writer.WriteField(overTimeHours);
                        }
                        else
                        {
                            // Column 6 = overtime hours
                            writer.WriteField(row[6]);
                            // Column 7 = other hours code
                            writer.WriteField(row[7]);
                            // Column 8 = other hours hours
                            writer.WriteField(row[8]);
                        }

                        // Column 9 = other hours2  code
                        writer.WriteField(row[9]);
                        // Column 10 = other hours 2 hours 
                        writer.WriteField(row[10]);
                        // Column 11 = business unit
                        writer.WriteField(row[11]);
                        // Column 12 = week ending dt
                        writer.WriteField(row[12]);
                        // Column 13 = pay type
                        writer.WriteField(row[13]);
                        // Column 14 = prepay
                        writer.WriteField(row[14]);

                        // Go to next row
                        writer.NextRecord();
                    }
                }

            }

            File.Delete(args[0]);
            File.Move(tempPath, args[0]);

        }
    }
}

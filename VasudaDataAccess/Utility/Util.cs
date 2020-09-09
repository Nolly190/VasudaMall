using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VasudaDataAccess.Data_Access;
using VasudaDataAccess.Data_Access.Implementation;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Utility
{
    public class Util
    {
      
        public static bool ValidateUrl(string urlName)
        {
            return Uri.TryCreate(urlName, UriKind.Absolute, out Uri uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static string FormatDate(DateTime date)
        {
            return $"{date.Day} {GetMonthName(date.Month)}, {date.Year}";
        }
        
        public static string FormatNullableDate(DateTime? date)
        {
            return $"{date.Value.Day} {GetMonthName(date.Value.Month)}, {date.Value.Year}";
        }

        public static string ReduceDescriptionLength(string description)
        {
            string des;
            if (string.IsNullOrEmpty(description))
            {
                des = "N/A";
            }
            else
            {
                if (description.Length < 50)
                {
                    des = description ;
                }
                else
                {
                    des = $"{description.Substring(0, 50)} ....";
                }
            }
            return des;
        }

        public static string GetMonthName(int index)
        {
            string[] months = new string[]
            {
                "January","February","March","April","May","June","July","August","September","October","November","December"
            };

            var name = months[index - 1];
            return name;
        }
     

    }
}

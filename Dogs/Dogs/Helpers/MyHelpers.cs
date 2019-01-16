using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dogs.Helpers
{
    public static class MyHelpers
    {
        public static IHtmlContent DogHTMLString(this IHtmlHelper htmlHelper)
            => new HtmlString("<strong>Dog html string</strong>");

        public static IHtmlContent EventsHTMLString(this IHtmlHelper htmlHelper)
            => new HtmlString("<strong color=\"blue\">Takes part in no events. </strong>");
        public static IHtmlContent EventsHTMLString(this IHtmlHelper htmlHelper, int a) { 
            return new HtmlString("<strong style=\"color: orange;\"> Takes part in " +a+" events.</strong>");
        }
        public static IHtmlContent YearsOldHTMLString(this IHtmlHelper htmlHelper, DateTime a)
        {
            return new HtmlString("<strong style=\"color: orange;\"> Is " + a.Year + " years old.</strong>");
        }
        public static IHtmlContent YearsOldHTMLString(this IHtmlHelper htmlHelper, double a)
        {
            a = a/365;
            return new HtmlString(" is " + (int)a + " years old.");
        }

        public static String DogString(this IHtmlHelper htmlHelper)
            => "<strong>dog string</strong>";
    }
}

namespace SysLibraryWeb.HtmlHelpers
{
    using System;
    using System.IO;
    using System.Text.Encodings.Web;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SysLibraryWeb.Models;

    public static class PagingHelper
    {
        public static HtmlString PageLinks(this IHtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringWriter writer=new StringWriter(); //TagBuilder每页tostring方法，只能通过writeto方法将内容写入一个textwriter对象中以取出其值。
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag=new TagBuilder("a");
                tag.MergeAttribute("herf",pageUrl(i));
                tag.InnerHtml.AppendHtml(i.ToString());
                if (i==pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                tag.WriteTo(writer,HtmlEncoder.Default);
            }
            return new HtmlString(writer.ToString());
        }
    }
}
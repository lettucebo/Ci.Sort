using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Web.Routing;
using Ci.Sort.Enums;
using Ci.Sort.Models;

namespace Ci.Mvc.Sort
{
    public static class SortHelper
    {
        public static MvcHtmlString SortLabel(this HtmlHelper helper, string name, string key, object routes, SortOrder sort)
        {
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var iTagBuilder = new TagBuilder("i");
            var actionName = helper.ViewContext.RouteData.GetRequiredString("action");

            var routeValues = (RouteValueDictionary)HtmlHelper.ObjectToDictionary(routes);
            routeValues.Add(nameof(SortOrder.Key), key);

            MvcHtmlString url = MvcHtmlString.Empty;
            if (sort.Key == key)
            {
                if (sort.Order == Order.Ascending)
                {
                    routeValues.Add(nameof(SortOrder.Order), Order.Descending);
                    url = MvcHtmlString.Create(urlHelper.Action(actionName, routeValues));
                    iTagBuilder.MergeAttribute("class", "fa fa-arrow-up");
                    iTagBuilder.MergeAttribute("aria-hidden", "true");
                }
                else
                {
                    routeValues.Add(nameof(SortOrder.Order), Order.Ascending);
                    url = MvcHtmlString.Create(urlHelper.Action(actionName, routeValues));
                    iTagBuilder.MergeAttribute("class", "fa fa-arrow-down");
                    iTagBuilder.MergeAttribute("aria-hidden", "true");
                }
            }
            else
            {
                routeValues.Add(nameof(SortOrder.Order), Order.Descending);
                url = MvcHtmlString.Create(urlHelper.Action(actionName, routeValues));
            }

            var builder = new TagBuilder("a");
            builder.GenerateId(name);
            builder.SetInnerText(name);
            builder.MergeAttribute("href", url.ToString());

            var result = builder.ToString(TagRenderMode.Normal) + iTagBuilder.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(result);
        }
    }
}

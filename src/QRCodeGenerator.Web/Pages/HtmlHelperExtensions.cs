﻿namespace QRCodeGenerator.Web.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using HtmlTags;
    using HtmlTags.Conventions;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.DependencyInjection;

    using QRCodeGenerator.Web.Infrastructure.Tags;

    public static class HtmlHelperExtensions
    {
        public static HtmlTag DisplayLabel<T>(this IHtmlHelper<T> helper, Expression<Func<T, object>> expression)
            where T : class
        {
            return helper.Tag(expression, nameof(TagConventions.DisplayLabels));
        }

        public static HtmlTag DisplayLabel<T>(this IHtmlHelper<List<T>> helper, Expression<Func<T, object>> expression)
            where T : class
        {
            var library = helper.ViewContext.HttpContext.RequestServices.GetService<HtmlConventionLibrary>();
            var generator = ElementGenerator<T>.For(library, t => helper.ViewContext.HttpContext.RequestServices.GetService(t));
            return generator.TagFor(expression, nameof(TagConventions.DisplayLabels));
        }

        public static HtmlTag ValidationDiv(this IHtmlHelper helper)
        {
            return new HtmlTag("div")
                .Id("validationSummary")
                .AddClass("alert")
                .AddClass("alert-danger")
                .AddClass("hidden");
        }

     

        public static HtmlTag FormBlock<T>(this IHtmlHelper<T> helper,
            Expression<Func<T, object>> expression,
            Action<HtmlTag> labelModifier = null,
            Action<HtmlTag> inputModifier = null) where T : class
        {
            labelModifier = labelModifier ?? (_ => { });
            inputModifier = inputModifier ?? (_ => { });

            var divTag = new HtmlTag("div");
            divTag.AddClass("form-group");

            var labelTag = helper.Label(expression);
            labelModifier(labelTag);

            var inputTag = helper.Input(expression);
            inputModifier(inputTag);

            var validationTag = helper.ValidationMessage(expression);



            divTag.Append(labelTag);
            divTag.Append(inputTag);
            divTag.Append(validationTag);

            return divTag;
        }
    }
}
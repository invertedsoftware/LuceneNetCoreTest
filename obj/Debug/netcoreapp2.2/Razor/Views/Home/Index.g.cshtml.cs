#pragma checksum "C:\Users\gal.ratner\source\repos\LuceneNetCoreTest\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "09b5cc114c9cb404d9c79d5c6e51275a6cbe0478"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\gal.ratner\source\repos\LuceneNetCoreTest\Views\_ViewImports.cshtml"
using LuceneNetCoreTest;

#line default
#line hidden
#line 2 "C:\Users\gal.ratner\source\repos\LuceneNetCoreTest\Views\_ViewImports.cshtml"
using LuceneNetCoreTest.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09b5cc114c9cb404d9c79d5c6e51275a6cbe0478", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b0e40dc9bf0067244afe70f3a55325ba428e1cab", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<LuceneNetCoreTest.Models.Resort>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\gal.ratner\source\repos\LuceneNetCoreTest\Views\Home\Index.cshtml"
  
	ViewData["Title"] = "Home Page";

#line default
#line hidden
            BeginContext(95, 137, true);
            WriteLiteral("\r\n\t<div class=\"text-center\">\r\n\t\t<h1 class=\"display-4\">Search for a Resort using Lucene.NET on .NET Core</h1>\r\n\t\t<div class=\"card-deck\">\r\n");
            EndContext();
#line 9 "C:\Users\gal.ratner\source\repos\LuceneNetCoreTest\Views\Home\Index.cshtml"
             foreach (var item in Model)
			{

#line default
#line hidden
            BeginContext(271, 88, true);
            WriteLiteral("\t\t\t\t<div class=\"card mb-4\">\r\n\t\t\t\t\t<div class=\"card-body\">\r\n\t\t\t\t\t\t<h5 class=\"card-title\">");
            EndContext();
            BeginContext(360, 9, false);
#line 13 "C:\Users\gal.ratner\source\repos\LuceneNetCoreTest\Views\Home\Index.cshtml"
                                          Write(item.Name);

#line default
#line hidden
            EndContext();
            BeginContext(369, 34, true);
            WriteLiteral("</h5>\r\n\t\t\t\t\t\t<p class=\"card-text\">");
            EndContext();
            BeginContext(404, 26, false);
#line 14 "C:\Users\gal.ratner\source\repos\LuceneNetCoreTest\Views\Home\Index.cshtml"
                                        Write(item.Resort_Description__c);

#line default
#line hidden
            EndContext();
            BeginContext(430, 31, true);
            WriteLiteral("</p>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t</div>\r\n");
            EndContext();
#line 17 "C:\Users\gal.ratner\source\repos\LuceneNetCoreTest\Views\Home\Index.cshtml"
			}

#line default
#line hidden
            BeginContext(467, 19, true);
            WriteLiteral("\t\t</div>\r\n\t</div>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<LuceneNetCoreTest.Models.Resort>> Html { get; private set; }
    }
}
#pragma warning restore 1591

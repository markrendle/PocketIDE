﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PocketIDE.Web.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PocketIDE.Web.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to div.libCScode{width:98.9%;padding-bottom:20px;border-top:#fff 5px solid;display:block}
        ///div.libCScode table{border:0;font-size:95%;margin-bottom:5px;width:100%}
        ///div.libCScode table th{background:#ddd;border-bottom-color:#c8cdde;border-bottom-style:solid;border-bottom-width:1px;color:#006;font-weight:bold}
        ///div.libCScode table td,pre.libCScode{background:#ddd;border-top-color:#c8cdde;border-top-style:solid;border-top-width:1px;padding-left:5px;padding-right:5px;padding-top:5px;margin:0 0 10px 0;display:bloc [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CustomCss {
            get {
                return ResourceManager.GetString("CustomCss", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to $(function () {
        ///    $(&quot;.region-link&quot;).click(function () {
        ///        if ($(this).next().length == 0) {
        ///            contentUrl = $(this).attr(&quot;data-content-url&quot;);
        ///            $(this).parent().append(&apos;&lt;div class=&quot;region-content&quot;&gt;&lt;/div&gt;&apos;);
        ///			$(this).siblings().first().load(contentUrl);
        ///        } else {
        ///            $(this).next().toggle(&quot;hidden&quot;);
        ///        }
        ///        return false;
        ///    });
        ///});
        ///.
        /// </summary>
        internal static string MsdnJQueryCode {
            get {
                return ResourceManager.GetString("MsdnJQueryCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///&lt;title&gt;{TITLE}&lt;/title&gt;
        ///&lt;link href=&quot;http://alexgorbatchev.com/pub/sh/current/styles/shThemeDefault.css&quot; rel=&quot;stylesheet&quot; type=&quot;text/css&quot; /&gt;
        ///&lt;style type=&quot;text/css&quot;&gt;
        ///    td.code { padding-left:8px }
        ///    div.line { font-family:Courier New }
        ///&lt;/style&gt;
        ///&lt;script src=&quot;http://alexgorbatchev.com/pub/sh/current/scripts/shCore.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
        ///&lt;script src=&quot;http://alexgorbatchev.com/pub/sh/current/scripts/shBrushCSharp.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///&lt;div s [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PublishTemplate {
            get {
                return ResourceManager.GetString("PublishTemplate", resourceCulture);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ZLangRT.Attributes;
using Z标准包.文件系统;
using System.IO;
using System.Xml;

namespace ZExcel开发包
{
    [ZInstance(typeof(OfficeProperties))]
    public abstract class Office属性集
    { 
        // 摘要: 
        //     Gets/Set the Application property of the document (extended property)
        public string Application { get; set; }
        //
        // 摘要: 
        //     Gets/Set the AppVersion property of the document (extended property)
        public string AppVersion { get; set; }
        
        [ZCode("作者")]
        public string Author { get; set; }
        //
        // 摘要: 
        //     Gets/sets the category property of the document (core property)
        public string Category { get; set; }

        [ZCode("注释")]
        public string Comments { get; set; }

        [ZCode("公司")]
        public string Company { get; set; }
        //
        // 摘要: 
        //     Provides access to the XML document that holds all the code document properties.
        public abstract XmlDocument CorePropertiesXml { get; }
        //
        // 摘要: 
        //     Gets/sets the created property of the document (core property)
        public DateTime Created { get; set; }
        //
        // 摘要: 
        //     Provides access to the XML document which holds the document's custom properties
        public abstract XmlDocument CustomPropertiesXml { get; }
        //
        // 摘要: 
        //     Provides access to the XML document that holds the extended properties of
        //     the document (app.xml)
        public abstract XmlDocument ExtendedPropertiesXml { get; }
        //
        // 摘要: 
        //     Gets/sets the HyperlinkBase property of the document (extended property)
        public Uri HyperlinkBase { get; set; }
        //
        // 摘要: 
        //     Hyperlinks need update
        public bool HyperlinksChanged { get; set; }
        //
        // 摘要: 
        //     Gets/sets the keywords property of the document (core property)
        public string Keywords { get; set; }
        //
        // 摘要: 
        //     Gets/sets the lastModifiedBy property of the document (core property)
        public string LastModifiedBy { get; set; }
        //
        // 摘要: 
        //     Gets/sets the lastPrinted property of the document (core property)
        public string LastPrinted { get; set; }
        //
        // 摘要: 
        //     Indicates whether hyperlinks in a document are up-to-date
        public bool LinksUpToDate { get; set; }
        //
        // 摘要: 
        //     Gets/sets the Manager property of the document (extended property)
        public string Manager { get; set; }
        //
        // 摘要: 
        //     Gets/sets the modified property of the document (core property)
        public DateTime Modified { get; set; }
        //
        // 摘要: 
        //     Display mode of the document thumbnail. True to enable scaling. False to
        //     enable cropping.
        public bool ScaleCrop { get; set; }
        //
        // 摘要: 
        //     If true, document is shared between multiple producers.
        public bool SharedDoc { get; set; }
        //
        // 摘要: 
        //     Gets/sets the status property of the document (core property)
        public string Status { get; set; }
        //
        // 摘要: 
        //     Gets/sets the subject property of the document (core property)
        public string Subject { get; set; }
        
        [ZCode("标题")]
        public string Title { get; set; }

        // 摘要: 
        //     Gets the value of a custom property
        //
        // 参数: 
        //   propertyName:
        //     The name of the property
        //
        // 返回结果: 
        //     The current value of the property
        public abstract object GetCustomPropertyValue(string propertyName);
        //
        // 摘要: 
        //     Get the value of an extended property
        //
        // 参数: 
        //   propertyName:
        //     The name of the property
        //
        // 返回结果: 
        //     The value
        public abstract string GetExtendedPropertyValue(string propertyName);
        //
        // 摘要: 
        //     Allows you to set the value of a current custom property or create your own
        //     custom property.
        //
        // 参数: 
        //   propertyName:
        //     The name of the property
        //
        //   value:
        //     The value of the property
        public abstract void SetCustomPropertyValue(string propertyName, object value);
        //
        // 摘要: 
        //     Set the value for an extended property
        //
        // 参数: 
        //   propertyName:
        //     The name of the property
        //
        //   value:
        //     The value
        public abstract void SetExtendedPropertyValue(string propertyName, string value);
    }
}

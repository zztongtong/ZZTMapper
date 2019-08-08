using System;
using System.Collections.Generic;
using System.Text;

namespace ZZT.Mapper.Attr
{
    public class MapToOrFromPropertyAttribute:Attribute
    {
        /// <summary>
        /// 源类型
        /// </summary>
        public Type Source { get; set; }

        /// <summary>
        /// 属性名名
        /// </summary>
        public string PropName { get; set; }

        public MapToOrFromPropertyAttribute(Type _source,string _propName)
        {
            Source = _source;
            PropName = _propName;
        }
    }
}

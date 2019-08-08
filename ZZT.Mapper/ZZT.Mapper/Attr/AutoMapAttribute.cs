using System;
using System.Collections.Generic;
using System.Text;

namespace ZZT.Mapper.Attr
{
    public class AutoMapAttribute:Attribute
    {
        /// <summary>
        /// 源类型
        /// </summary>
        public Type Source { get; set; }

        public AutoMapAttribute(Type _source)
        {
            Source = _source;
        }
    }
}

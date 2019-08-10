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

        /// <summary>
        /// 是否要反向映射
        /// </summary>
        public bool Reverse { get; set; }
        public AutoMapAttribute(Type _source,bool _reverse=false)
        {
            Source = _source;
            Reverse = _reverse;
        }
    }
}

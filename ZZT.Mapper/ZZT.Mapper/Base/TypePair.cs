using System;
using System.Collections.Generic;
using System.Text;

namespace ZZT.Mapper.Base
{
    /// <summary>
    /// 类型映射对
    /// </summary>
    public class TypePair
    {
        /// <summary>
        /// 目标类型
        /// </summary>
        public Type TDestion;

        /// <summary>
        /// 源类型
        /// </summary>
        public Type TSource;

        public TypePair(Type _des,Type _sour)
        {
            TDestion = _des;
            TSource = _sour;
        }

        public override bool Equals(object obj)
        {
            var typePair = (TypePair)obj;
            return typePair.TDestion == this.TDestion && typePair.TSource == this.TSource;
        }

        public override int GetHashCode()
        {
            int desHashcode = TDestion.GetHashCode();
            int sourHashcode = TSource.GetHashCode();
            return desHashcode + sourHashcode;
        }
    }

    /// <summary>
    /// 属性映射对
    /// </summary>
    public class PropPair
    {
        /// <summary>
        /// 目标属性名
        /// </summary>
        public string DesPropName { get; set; }

        /// <summary>
        /// 源属性名
        /// </summary>
        public string SourPropName { get; set; }
    }
}

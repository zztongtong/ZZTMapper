using System;
using ZZT.Mapper.Attr;
using System.Linq;

namespace ZZT.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoMapper
    {
        /// <summary>
        /// 对象映射
        /// </summary>
        /// <typeparam name="TDestion">目标对象</typeparam>
        /// <typeparam name="TSource">源对象</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestion Map<TDestion, TSource>(TSource source)
        {
            var desType = typeof(TDestion);
            var sourceType = typeof(TSource);

            TDestion desIns = default(TDestion);
            var automapList= desType.GetCustomAttributes(false).Where(e => e.GetType() == typeof(AutoMapAttribute)).Where(x=>((AutoMapAttribute)x).Source==typeof(TSource)).ToList();
            if (automapList == null || automapList.Count() == 0)
            {
                //TDestion没有指定来源于TSource的映射
                return desIns;
            }
            
           
            //获取TSource的成员
            var sourceProps = sourceType.GetProperties();
            var destionProps = desType.GetProperties();
            foreach(var desProp in destionProps)
            {
                var mapToOrFromSource = desProp.GetCustomAttributes(false).Where(e => e.GetType() == typeof(MapToOrFromPropertyAttribute)).Where(x => ((MapToOrFromPropertyAttribute)x).Source == sourceType).FirstOrDefault();
                if (mapToOrFromSource!=null)
                {
                    var mapToOrFromSourceIns = (MapToOrFromPropertyAttribute)mapToOrFromSource;
                    var propVal = sourceType.GetProperties(mapToOrFromSourceIns.PropName).GetValue(source, null);
                    desProp.SetValue(desIns, propVal, null);
                    //此属性有
                }
            }
            foreach (var prop in sourceProps)
            {
                //获取目标对象此名称的字段
                var desProp = desType.GetProperty(prop.Name);
                //获取源对象此字段的值
                var propVal = prop.GetValue(source, null);
                //将源对象此字段的值赋给目标对象对应字段
                desProp.SetValue(desIns, propVal, null);
            }
            return desIns;
        }

    }
}

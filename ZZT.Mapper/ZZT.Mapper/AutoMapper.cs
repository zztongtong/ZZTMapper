using System;

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
            TDestion desIns = (TDestion)Activator.CreateInstance(typeof(TDestion));
            //获取TSource的成员
            var sourceProps = sourceType.GetProperties();
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

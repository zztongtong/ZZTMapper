using System;
using ZZT.Mapper.Attr;
using System.Linq;

namespace ZZT.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class ZZTMapper
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
            var sourceAutomapList= desType.GetCustomAttributes(false).Where(e => e.GetType() == typeof(AutoMapAttribute)).Where(x=>((AutoMapAttribute)x).Source==typeof(TSource)).ToList();
            var desAutomapList = sourceType.GetCustomAttributes(false).Where(e => e.GetType() == typeof(AutoMapAttribute)).Where(x => ((AutoMapAttribute)x).Source == typeof(TDestion)).ToList();
            if ((sourceAutomapList == null&&desAutomapList==null) || (sourceAutomapList.Count() == 0&&desAutomapList.Count()==0))
            {
                //TDestion没有指定来源于TSource的映射且TSource没有指定映射到TDestion
                return desIns;
            }
            
            //获取TSource的成员
            var sourceProps = sourceType.GetProperties();
            var destionProps = desType.GetProperties();
            if (sourceAutomapList != null && sourceAutomapList.Count() > 0)
            {
                //TDestion指定源映射TSource
                foreach (var desProp in destionProps)
                {
                    var mapToOrFromSource = desProp.GetCustomAttributes(false).Where(e => e.GetType() == typeof(MapToOrFromPropertyAttribute)).Where(x => ((MapToOrFromPropertyAttribute)x).Source == sourceType).FirstOrDefault();
                    if (mapToOrFromSource != null)
                    {
                        var mapToOrFromSourceIns = (MapToOrFromPropertyAttribute)mapToOrFromSource;
                        var propVal = sourceProps.Where(e => e.Name == mapToOrFromSourceIns.PropName).FirstOrDefault().GetValue(source, null);
                        desProp.SetValue(desIns, propVal, null);
                    }
                    else
                    {
                        var propVal = sourceProps.Where(e => e.Name == desProp.Name).FirstOrDefault().GetValue(source, null);
                        desProp.SetValue(desIns, propVal, null);
                    }
                }
            }
            else
            {
                //TSource指定源映射TDestion
                foreach (var sourceProp in sourceProps)
                {
                    var mapToOrFromSource = sourceProp.GetCustomAttributes(false).Where(e => e.GetType() == typeof(MapToOrFromPropertyAttribute)).Where(x => ((MapToOrFromPropertyAttribute)x).Source == desType).FirstOrDefault();
                    if (mapToOrFromSource != null)
                    {
                        var mapToOrFromSourceIns = (MapToOrFromPropertyAttribute)mapToOrFromSource;
                        var propVal = sourceProp.GetValue(source, null);
                        destionProps.Where(e=>e.Name== mapToOrFromSourceIns.PropName).FirstOrDefault().SetValue(desIns, propVal, null);
                    }
                    else
                    {
                        var propVal = sourceProp.GetValue(source, null);
                        destionProps.Where(e => e.Name == sourceProp.Name).FirstOrDefault().SetValue(desIns, propVal, null);
                    }
                }
            }
            
            return desIns;
        }

    }
}

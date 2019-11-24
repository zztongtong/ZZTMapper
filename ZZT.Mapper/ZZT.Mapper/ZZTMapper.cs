using System;
using System.Collections;
using System.Collections.Generic;
using ZZT.Mapper.Attr;
using System.Linq;
using System.Collections.Concurrent;
using ZZT.Mapper.Base;
using System.Reflection;
using System.Threading.Tasks;

namespace ZZT.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class ZZTMapper
    {
        /// <summary>
        /// 用于存放所有配置信息
        /// </summary>
        public static ConcurrentDictionary<TypePair, List<PropPair>> LockDictionar = new ConcurrentDictionary<TypePair, List<PropPair>>();

        public static TypePair CreateTypePair<TDestion, TSource>()
        {
            return new TypePair(typeof(TDestion), typeof(TSource));
        }

        /// <summary>
        /// 创建映射配置
        /// </summary>
        /// <typeparam name="TDestion">目标对象</typeparam>
        /// <typeparam name="TSource">源对象</typeparam>
        /// <param name="des"></param>
        /// <param name="sour"></param>
        public static void CreateMap<TDestion,TSource>()
        {
            var sourceAutomap = (AutoMapAttribute)typeof(TDestion).GetCustomAttributes(false).Where(e => e.GetType() == typeof(AutoMapAttribute)).Where(x => ((AutoMapAttribute)x).Source == typeof(TSource)).FirstOrDefault();
            
            TypePair tp = CreateTypePair<TDestion, TSource>();
            List<PropPair> pp = new List<PropPair>(); 
            var desProps = tp.TDestion.GetProperties();
            var sourProps = tp.TSource.GetProperties();

            if (sourceAutomap == null)
            {
                //如果TDestion没有指定TSource
                foreach (var prop in desProps)
                {
                    if (tp.TSource.GetProperty(prop.Name) != null)
                    {
                        var pPair = new PropPair { DesPropName = prop.Name, SourPropName = prop.Name };
                        pp.Add(pPair);
                    }
                }
                LockDictionar.GetOrAdd(tp, pp);
                CreateMap<TSource, TDestion>();
                return;
            }

            foreach (var prop in desProps)
            {
                var mapToOrFromSource = (MapToOrFromPropertyAttribute)prop.GetCustomAttributes(false).Where(e => e.GetType() == typeof(MapToOrFromPropertyAttribute)).Where(x => ((MapToOrFromPropertyAttribute)x).Source == tp.TSource).FirstOrDefault();
                if (mapToOrFromSource != null)
                {
                    var sourProp = tp.TSource.GetProperty(mapToOrFromSource.PropName);
                    if (sourProp!=null&&prop.PropertyType==sourProp.PropertyType)
                    {
                        var pPair = new PropPair { DesPropName = prop.Name, SourPropName = mapToOrFromSource.PropName };
                        pp.Add(pPair);
                    }
                }
                else
                {
                    if (tp.TSource.GetProperty(prop.Name) != null)
                    {
                        var pPair = new PropPair { DesPropName = prop.Name, SourPropName = prop.Name };
                        pp.Add(pPair);
                    }

                }
            }

            LockDictionar.GetOrAdd(tp, pp);
            
            if (sourceAutomap.Reverse)
            {
                TypePair reverTP = CreateTypePair<TSource, TDestion>();
                List<PropPair> reverPP = new List<PropPair>();
                foreach(var item in pp)
                {
                    reverPP.Add(new PropPair { DesPropName = item.SourPropName, SourPropName = item.DesPropName });
                }
                LockDictionar.GetOrAdd(reverTP, reverPP);
            }
        }
        /// <summary>
        /// 创建tSource与tDestion之间的映射关系
        /// </summary>
        /// <param name="tDestion"></param>
        /// <param name="tSource"></param>
        public static void CreateMap(Type tDestion,Type tSource)
        {
            var sourceAutomap=(AutoMapAttribute) tDestion.GetCustomAttributes().Where(e => e.GetType() == typeof(AutoMapAttribute)).Where(x => ((AutoMapAttribute)x).Source == tSource).FirstOrDefault();
            
            TypePair tp = new TypePair(tDestion, tSource);
            List<PropPair> pp = new List<PropPair>();
            var desProps = tp.TDestion.GetProperties();
            var sourProps = tp.TSource.GetProperties();

            if (sourceAutomap == null)
            {
                //tDestion 与tSource之间不存在映射关系
                foreach(var prop in desProps)
                {
                    if (tp.TSource.GetProperty(prop.Name) != null)
                    {
                        var pPair = new PropPair { DesPropName = prop.Name, SourPropName = prop.Name };
                        pp.Add(pPair);
                    }
                }
                LockDictionar.GetOrAdd(tp, pp);
                CreateMap(tSource, tDestion);
                return;
            }

            foreach (var prop in desProps)
            {
                if (prop.PropertyType == typeof(IEnumerable<>))
                {
                    //暂不转换TDestion中的集合类型对象
                    continue;
                }

                var mapToOrFromSource = (MapToOrFromPropertyAttribute)prop.GetCustomAttributes(false).Where(e => e.GetType() == typeof(MapToOrFromPropertyAttribute)).Where(x => ((MapToOrFromPropertyAttribute)x).Source == tp.TSource).FirstOrDefault();
                if (mapToOrFromSource != null)
                {
                    var sourProp = tp.TSource.GetProperty(mapToOrFromSource.PropName);
                    if (sourProp != null && prop.PropertyType == sourProp.PropertyType)
                    {
                        var pPair = new PropPair { DesPropName = prop.Name, SourPropName = mapToOrFromSource.PropName };
                        pp.Add(pPair);
                    }
                }
                else
                {
                    if (tp.TSource.GetProperty(prop.Name) != null)
                    {
                        var pPair = new PropPair { DesPropName = prop.Name, SourPropName = prop.Name };
                        pp.Add(pPair);
                    }
                }
            }

            LockDictionar.GetOrAdd(tp, pp);

            if (sourceAutomap.Reverse)
            {
                TypePair reverTP = new TypePair(tSource, tDestion);
                List<PropPair> reverPP = new List<PropPair>();
                foreach (var item in pp)
                {
                    reverPP.Add(new PropPair { DesPropName = item.SourPropName, SourPropName = item.DesPropName });
                }
                LockDictionar.GetOrAdd(reverTP, reverPP);
            }
        }

        /// <summary>
        /// 初始化ZZTMapper
        /// </summary>
        /// <param name="assemblies"></param>
        public static void Init(IEnumerable<Assembly> assemblies)
        {
            foreach(var item in assemblies)
            {
                //获取带有AutoMap属性的类型
                var types = item.GetTypes().Where(e=>e.GetCustomAttributes().Where(x=>x.GetType()==typeof(AutoMapAttribute)).Count()>0);
                Parallel.ForEach(types, (type) => {
                    var automaps = type.GetCustomAttributes().Where(e => e.GetType() == typeof(AutoMapAttribute));
                    Parallel.ForEach(automaps, (automap) => {
                        CreateMap(type, ((AutoMapAttribute)automap).Source);
                    });
                });
                
            }
        }


        /// <summary>
        /// 对象转换-两者有映射关系
        /// </summary>
        /// <typeparam name="TDestion"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TDestion Map<TDestion>(object obj)
        {
            var desType = typeof(TDestion);
            var sourceType = obj.GetType();
            TDestion dins = (TDestion)Activator.CreateInstance(desType);
            TypePair tp = new TypePair(desType, sourceType);
            try
            {
                
                if (!LockDictionar.ContainsKey(tp))
                {
                    //两者没有配置映射关系，则只映射类型且名称相同的字段
                    PropertyInfo[] props= desType.GetProperties();
                    Parallel.ForEach(props, (item) => {
                        if(desType.GetProperty(item.Name).GetType()== sourceType.GetProperty(item.Name).GetType())
                        {
                            desType.GetProperty(item.Name).SetValue(dins, sourceType.GetProperty(item.Name).GetValue(obj));
                        }
                    });
                }
                else
                {
                    List<PropPair> pps = LockDictionar[tp];
                    Parallel.ForEach(pps, (item) => {
                        desType.GetProperty(item.DesPropName).SetValue(dins, sourceType.GetProperty(item.SourPropName).GetValue(obj));
                    });
                }
            }
            catch(Exception ex)
            {
                throw new Exception(desType.Name + "与" + sourceType.Name + "映射失败", ex);
            }
            
            return dins;
        }
    }
}

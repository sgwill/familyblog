using System;

[assembly: WebActivator.PreApplicationStartMethod(typeof(WilliamsonFamily.Web.App_Start.MiniProfiler_Start), "Start")]

namespace WilliamsonFamily.Web.App_Start
{
    public static class MiniProfiler_Start
    {
        public static void Start()
        {
            InitProfilerSettings();
        }

        /// <summary>
        /// Customize aspects of the MiniProfiler.
        /// </summary>
        private static void InitProfilerSettings()
        {
            // a powerful feature of the MiniProfiler is the ability to share links to results with other developers.
            // by default, however, long-term result caching is done in HttpRuntime.Cache, which is very volatile.
            // 
            // let's rig up methods to binary serialize our profiler results to a database, so they survive app restarts.
            // (note: this method is more to test that the MiniProfiler can be serialized by protobuf-net - a real database storage
            // scheme would put each property into its own column, so they could be queried independently of the MiniProfiler's UI)

            // a setter will take the current profiler and should save it somewhere by its guid Id
            //MiniProfiler.Settings.LongTermCacheSetter = (profiler) =>
            //{
            //    using (var ms = new MemoryStream())
            //    {
            //        ProtoBuf.Serializer.Serialize(ms, profiler);

            //        using (var conn = BaseController.GetOpenConnection())
            //        {
            //            // we use the insert to ignore syntax here, because MiniProfiler will
            //            conn.Execute("insert or ignore into MiniProfilerResults (Id, Results) values (@id, @results)", new { id = profiler.Id, results = ms.GetBuffer() });
            //        }
            //    }
            //};

            //// the getter will be passed a guid and should return the saved MiniProfiler
            //MiniProfiler.Settings.LongTermCacheGetter = (id) =>
            //{
            //    byte[] results = null;
            //    using (var conn = BaseController.GetOpenConnection())
            //    {
            //        dynamic buffer = conn.Query("select Results from MiniProfilerResults where Id = @id", new { id = id }).SingleOrDefault();

            //        if (buffer == null)
            //            return null;

            //        results = (byte[])buffer.Results;
            //    }

            //    using (var ms = new MemoryStream(results))
            //    {
            //        return ProtoBuf.Serializer.Deserialize<MiniProfiler>(ms);
            //    }
            //};

        }
    }
}
using Dapper;
using Simplic.Cache;
using Simplic.Data.Sql;
using Simplic.Sql;
using System;

namespace Simplic.Change.Tracking.Data.DB
{
    public class RequestChangeRepository : SqlRepositoryBase<Guid, RequestChange>, IRequestChangeRepository
    {
        private ISqlService sqlService;
        public RequestChangeRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService)
            : base(sqlService, sqlColumnService, cacheService)
        {
            this.sqlService = sqlService;
        }
        public override string TableName => "ChangeTracking";

        public override string PrimaryKeyColumn => "Guid";

        public override Guid GetId(RequestChange obj) => obj.Guid;
    }
}

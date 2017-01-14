//
// https://github.com/ServiceStack/ServiceStack.Redis
// ServiceStack.Redis: ECMA CLI Binding to the Redis key-value storage system
//
// Authors:
//   Demis Bellot (demis.bellot@gmail.com)
//
// Copyright 2013 Service Stack LLC. All Rights Reserved.
//
// Licensed under the same terms of ServiceStack.
//

using ServiceStack.Caching;

namespace ServiceStack.Redis
{
	/// <summary>
    /// For interoperabilty GetCacheClient() and GetReadOnlyCacheClient()
	/// return an ICacheClient wrapper around the redis manager which has the affect of calling 
    /// GetClient() for all write operations and GetReadOnlyClient() for the read ones.
	/// 
	/// This works well for master-slave replication scenarios where you have 
	/// 1 master that replicates to multiple read slaves.
	/// </summary>
	public partial class PooledRedisClientManager
		: IRedisClientCacheManager
	{
		public ICacheClient GetCacheClient()
		{
            return new RedisClientManagerCacheClient(this);
        }

		public ICacheClient GetReadOnlyCacheClient()
		{
			return new RedisClientManagerCacheClient(this) { ReadOnly = true };
		}
	}
}
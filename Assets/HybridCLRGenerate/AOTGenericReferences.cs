using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"CSRedisCore.dll",
		"DOTween.dll",
		"Newtonsoft.Json.dll",
		"SelfAOT.dll",
		"System.Core.dll",
		"System.Memory.dll",
		"System.Runtime.CompilerServices.Unsafe.dll",
		"System.dll",
		"UnityEngine.AndroidJNIModule.dll",
		"UnityEngine.AssetBundleModule.dll",
		"UnityEngine.CoreModule.dll",
		"UnityEngine.JSONSerializeModule.dll",
		"mscorlib.dll",
		"zxing.unity.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// CSRedis.CSRedisClient.<>c__151<object>
	// CSRedis.CSRedisClient.<>c__154<object>
	// CSRedis.CSRedisClient.<>c__158<object>
	// CSRedis.CSRedisClient.<>c__161<object>
	// CSRedis.CSRedisClient.<>c__165<object>
	// CSRedis.CSRedisClient.<>c__170<object>
	// CSRedis.CSRedisClient.<>c__190<object>
	// CSRedis.CSRedisClient.<>c__199<object>
	// CSRedis.CSRedisClient.<>c__201<object>
	// CSRedis.CSRedisClient.<>c__210<object>
	// CSRedis.CSRedisClient.<>c__221<object>
	// CSRedis.CSRedisClient.<>c__229<object>
	// CSRedis.CSRedisClient.<>c__239<object>
	// CSRedis.CSRedisClient.<>c__293<object>
	// CSRedis.CSRedisClient.<>c__29<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass101_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass103_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass111_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass113_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass115_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass117_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass119_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass121_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass123_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass128_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass130_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass132_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass134_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass136_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass138_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass143_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass145_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass148_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass163_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass167_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass168_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass173_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass175_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass177_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass179_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass181_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass183_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass185_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass191_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass194_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass202_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass208_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass216_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass223_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass233_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass235_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass278_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass285_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass287_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass291_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass293_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass295_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass299_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass43_0<System.UIntPtr>
	// CSRedis.CSRedisClient.<>c__DisplayClass43_0<int>
	// CSRedis.CSRedisClient.<>c__DisplayClass43_0<long>
	// CSRedis.CSRedisClient.<>c__DisplayClass43_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass47_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass53_0<long>
	// CSRedis.CSRedisClient.<>c__DisplayClass53_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass54_0<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass54_1<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass54_2<object>
	// CSRedis.CSRedisClient.<>c__DisplayClass54_3<object>
	// CSRedis.Internal.ObjectPool.IObjectPool<object>
	// CSRedis.Internal.ObjectPool.IPolicy<object>
	// CSRedis.Internal.ObjectPool.Object<object>
	// CSRedis.Internal.ObjectPool.ObjectPool.<>c<object>
	// CSRedis.Internal.ObjectPool.ObjectPool.<>c__DisplayClass27_0<object>
	// CSRedis.Internal.ObjectPool.ObjectPool.<GetAsync>d__39<object>
	// CSRedis.Internal.ObjectPool.ObjectPool.GetSyncQueueInfo<object>
	// CSRedis.Internal.ObjectPool.ObjectPool<object>
	// CSRedis.RedisScan<System.ValueTuple<object,System.Decimal>>
	// CSRedis.RedisScan<System.ValueTuple<object,object>>
	// CSRedis.RedisScan<object>
	// RedisHelper.<>c<object>
	// RedisHelper<object>
	// ScriptableObjectSingleton<object>
	// System.Action<CryPrinter.Pixel>
	// System.Action<Loom.DelayedQueueItem>
	// System.Action<Loom.NoDelayedQueueItem>
	// System.Action<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Action<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Action<System.DateTime>
	// System.Action<System.Guid>
	// System.Action<System.Nullable<int>>
	// System.Action<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Action<System.ValueTuple<object,int>>
	// System.Action<UnityEngine.Vector3>
	// System.Action<byte>
	// System.Action<float>
	// System.Action<int,object>
	// System.Action<int>
	// System.Action<long>
	// System.Action<object,object>
	// System.Action<object>
	// System.Action<ulong>
	// System.Action<ushort>
	// System.ArraySegment.ArraySegmentEnumerator<byte>
	// System.ArraySegment.ArraySegmentEnumerator<int>
	// System.ArraySegment<byte>
	// System.ArraySegment<int>
	// System.Collections.Concurrent.ConcurrentDictionary.<GetEnumerator>d__32<System.Guid,object>
	// System.Collections.Concurrent.ConcurrentDictionary.<GetEnumerator>d__32<object,int>
	// System.Collections.Concurrent.ConcurrentDictionary.<GetEnumerator>d__32<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary.<GetEnumerator>d__32<ushort,ushort>
	// System.Collections.Concurrent.ConcurrentDictionary.DictionaryEnumerator<System.Guid,object>
	// System.Collections.Concurrent.ConcurrentDictionary.DictionaryEnumerator<object,int>
	// System.Collections.Concurrent.ConcurrentDictionary.DictionaryEnumerator<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary.DictionaryEnumerator<ushort,ushort>
	// System.Collections.Concurrent.ConcurrentDictionary.Node<System.Guid,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Node<object,int>
	// System.Collections.Concurrent.ConcurrentDictionary.Node<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Node<ushort,ushort>
	// System.Collections.Concurrent.ConcurrentDictionary.Tables<System.Guid,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Tables<object,int>
	// System.Collections.Concurrent.ConcurrentDictionary.Tables<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Tables<ushort,ushort>
	// System.Collections.Concurrent.ConcurrentDictionary<System.Guid,object>
	// System.Collections.Concurrent.ConcurrentDictionary<object,int>
	// System.Collections.Concurrent.ConcurrentDictionary<object,object>
	// System.Collections.Concurrent.ConcurrentDictionary<ushort,ushort>
	// System.Collections.Concurrent.ConcurrentQueue.<Enumerate>d__27<byte>
	// System.Collections.Concurrent.ConcurrentQueue.<Enumerate>d__27<object>
	// System.Collections.Concurrent.ConcurrentQueue.Segment<byte>
	// System.Collections.Concurrent.ConcurrentQueue.Segment<object>
	// System.Collections.Concurrent.ConcurrentQueue<byte>
	// System.Collections.Concurrent.ConcurrentQueue<object>
	// System.Collections.Concurrent.ConcurrentStack.<GetEnumerator>d__35<object>
	// System.Collections.Concurrent.ConcurrentStack.Node<object>
	// System.Collections.Concurrent.ConcurrentStack<object>
	// System.Collections.Generic.ArraySortHelper<CryPrinter.Pixel>
	// System.Collections.Generic.ArraySortHelper<Loom.DelayedQueueItem>
	// System.Collections.Generic.ArraySortHelper<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.ArraySortHelper<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ArraySortHelper<System.DateTime>
	// System.Collections.Generic.ArraySortHelper<System.Guid>
	// System.Collections.Generic.ArraySortHelper<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.ArraySortHelper<System.ValueTuple<object,int>>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<byte>
	// System.Collections.Generic.ArraySortHelper<float>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<long>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.ArraySortHelper<ulong>
	// System.Collections.Generic.ArraySortHelper<ushort>
	// System.Collections.Generic.Comparer<CryPrinter.Pixel>
	// System.Collections.Generic.Comparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.Comparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.Comparer<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Comparer<System.DateTime>
	// System.Collections.Generic.Comparer<System.Decimal>
	// System.Collections.Generic.Comparer<System.Guid>
	// System.Collections.Generic.Comparer<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.Comparer<System.ValueTuple<object,int>>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<long>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Comparer<ulong>
	// System.Collections.Generic.Comparer<ushort>
	// System.Collections.Generic.Dictionary.Enumerator<int,byte>
	// System.Collections.Generic.Dictionary.Enumerator<int,float>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<ulong,int>
	// System.Collections.Generic.Dictionary.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ulong,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<int,float>
	// System.Collections.Generic.Dictionary.KeyCollection<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<ulong,int>
	// System.Collections.Generic.Dictionary.KeyCollection<ulong,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ulong,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<int,float>
	// System.Collections.Generic.Dictionary.ValueCollection<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<ulong,int>
	// System.Collections.Generic.Dictionary.ValueCollection<ulong,object>
	// System.Collections.Generic.Dictionary<int,byte>
	// System.Collections.Generic.Dictionary<int,float>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary<int,long>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,byte>
	// System.Collections.Generic.Dictionary<object,float>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<ulong,int>
	// System.Collections.Generic.Dictionary<ulong,object>
	// System.Collections.Generic.EqualityComparer<CryPrinter.Pixel>
	// System.Collections.Generic.EqualityComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.EqualityComparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.EqualityComparer<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.EqualityComparer<System.DateTime>
	// System.Collections.Generic.EqualityComparer<System.Decimal>
	// System.Collections.Generic.EqualityComparer<System.Guid>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<object,int>>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Vector3>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<ulong>
	// System.Collections.Generic.EqualityComparer<ushort>
	// System.Collections.Generic.HashSet.Enumerator<ushort>
	// System.Collections.Generic.HashSet<ushort>
	// System.Collections.Generic.HashSetEqualityComparer<ushort>
	// System.Collections.Generic.ICollection<CryPrinter.Pixel>
	// System.Collections.Generic.ICollection<Loom.DelayedQueueItem>
	// System.Collections.Generic.ICollection<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.ICollection<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ulong,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.ICollection<System.DateTime>
	// System.Collections.Generic.ICollection<System.Guid>
	// System.Collections.Generic.ICollection<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.ICollection<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>
	// System.Collections.Generic.ICollection<System.ValueTuple<object,System.Decimal>>
	// System.Collections.Generic.ICollection<System.ValueTuple<object,int>>
	// System.Collections.Generic.ICollection<System.ValueTuple<object,object>>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3>
	// System.Collections.Generic.ICollection<byte>
	// System.Collections.Generic.ICollection<float>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<long>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<ulong>
	// System.Collections.Generic.ICollection<ushort>
	// System.Collections.Generic.IComparer<CryPrinter.Pixel>
	// System.Collections.Generic.IComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.IComparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.IComparer<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IComparer<System.DateTime>
	// System.Collections.Generic.IComparer<System.Guid>
	// System.Collections.Generic.IComparer<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.IComparer<System.ValueTuple<object,int>>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3>
	// System.Collections.Generic.IComparer<byte>
	// System.Collections.Generic.IComparer<float>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<long>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IComparer<ulong>
	// System.Collections.Generic.IComparer<ushort>
	// System.Collections.Generic.IDictionary<System.Guid,object>
	// System.Collections.Generic.IDictionary<int,object>
	// System.Collections.Generic.IDictionary<object,int>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IDictionary<ushort,ushort>
	// System.Collections.Generic.IEnumerable<CryPrinter.Pixel>
	// System.Collections.Generic.IEnumerable<Loom.DelayedQueueItem>
	// System.Collections.Generic.IEnumerable<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.IEnumerable<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Guid,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ushort,ushort>>
	// System.Collections.Generic.IEnumerable<System.DateTime>
	// System.Collections.Generic.IEnumerable<System.Guid>
	// System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>
	// System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal>>
	// System.Collections.Generic.IEnumerable<System.ValueTuple<object,int>>
	// System.Collections.Generic.IEnumerable<System.ValueTuple<object,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<float>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<long>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ulong>
	// System.Collections.Generic.IEnumerable<ushort>
	// System.Collections.Generic.IEnumerator<CryPrinter.Pixel>
	// System.Collections.Generic.IEnumerator<Loom.DelayedQueueItem>
	// System.Collections.Generic.IEnumerator<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.IEnumerator<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Guid,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ulong,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ushort,ushort>>
	// System.Collections.Generic.IEnumerator<System.DateTime>
	// System.Collections.Generic.IEnumerator<System.Guid>
	// System.Collections.Generic.IEnumerator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.IEnumerator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>
	// System.Collections.Generic.IEnumerator<System.ValueTuple<object,System.Decimal>>
	// System.Collections.Generic.IEnumerator<System.ValueTuple<object,int>>
	// System.Collections.Generic.IEnumerator<System.ValueTuple<object,object>>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<float>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<long>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ulong>
	// System.Collections.Generic.IEnumerator<ushort>
	// System.Collections.Generic.IEqualityComparer<System.Guid>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<ulong>
	// System.Collections.Generic.IEqualityComparer<ushort>
	// System.Collections.Generic.IList<CryPrinter.Pixel>
	// System.Collections.Generic.IList<Loom.DelayedQueueItem>
	// System.Collections.Generic.IList<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.IList<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<ulong,int>>
	// System.Collections.Generic.IList<System.DateTime>
	// System.Collections.Generic.IList<System.Guid>
	// System.Collections.Generic.IList<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.IList<System.ValueTuple<object,int>>
	// System.Collections.Generic.IList<UnityEngine.Vector3>
	// System.Collections.Generic.IList<byte>
	// System.Collections.Generic.IList<float>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<long>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.IList<ulong>
	// System.Collections.Generic.IList<ushort>
	// System.Collections.Generic.KeyValuePair<System.Guid,object>
	// System.Collections.Generic.KeyValuePair<int,byte>
	// System.Collections.Generic.KeyValuePair<int,float>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,long>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,byte>
	// System.Collections.Generic.KeyValuePair<object,float>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<ulong,int>
	// System.Collections.Generic.KeyValuePair<ulong,object>
	// System.Collections.Generic.KeyValuePair<ushort,ushort>
	// System.Collections.Generic.LinkedList.Enumerator<object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List.Enumerator<CryPrinter.Pixel>
	// System.Collections.Generic.List.Enumerator<Loom.DelayedQueueItem>
	// System.Collections.Generic.List.Enumerator<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.List.Enumerator<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List.Enumerator<System.DateTime>
	// System.Collections.Generic.List.Enumerator<System.Guid>
	// System.Collections.Generic.List.Enumerator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.List.Enumerator<System.ValueTuple<object,int>>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.List.Enumerator<byte>
	// System.Collections.Generic.List.Enumerator<float>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<long>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List.Enumerator<ulong>
	// System.Collections.Generic.List.Enumerator<ushort>
	// System.Collections.Generic.List.SynchronizedList<CryPrinter.Pixel>
	// System.Collections.Generic.List.SynchronizedList<Loom.DelayedQueueItem>
	// System.Collections.Generic.List.SynchronizedList<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.List.SynchronizedList<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.List.SynchronizedList<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List.SynchronizedList<System.DateTime>
	// System.Collections.Generic.List.SynchronizedList<System.Guid>
	// System.Collections.Generic.List.SynchronizedList<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.List.SynchronizedList<System.ValueTuple<object,int>>
	// System.Collections.Generic.List.SynchronizedList<UnityEngine.Vector3>
	// System.Collections.Generic.List.SynchronizedList<byte>
	// System.Collections.Generic.List.SynchronizedList<float>
	// System.Collections.Generic.List.SynchronizedList<int>
	// System.Collections.Generic.List.SynchronizedList<long>
	// System.Collections.Generic.List.SynchronizedList<object>
	// System.Collections.Generic.List.SynchronizedList<ulong>
	// System.Collections.Generic.List.SynchronizedList<ushort>
	// System.Collections.Generic.List<CryPrinter.Pixel>
	// System.Collections.Generic.List<Loom.DelayedQueueItem>
	// System.Collections.Generic.List<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.List<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List<System.DateTime>
	// System.Collections.Generic.List<System.Guid>
	// System.Collections.Generic.List<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.List<System.ValueTuple<object,int>>
	// System.Collections.Generic.List<UnityEngine.Vector3>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<float>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<long>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.List<ulong>
	// System.Collections.Generic.List<ushort>
	// System.Collections.Generic.ObjectComparer<CryPrinter.Pixel>
	// System.Collections.Generic.ObjectComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.ObjectComparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.ObjectComparer<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ObjectComparer<System.DateTime>
	// System.Collections.Generic.ObjectComparer<System.Decimal>
	// System.Collections.Generic.ObjectComparer<System.Guid>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<object,int>>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<long>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectComparer<ulong>
	// System.Collections.Generic.ObjectComparer<ushort>
	// System.Collections.Generic.ObjectEqualityComparer<CryPrinter.Pixel>
	// System.Collections.Generic.ObjectEqualityComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.ObjectEqualityComparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.ObjectEqualityComparer<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ObjectEqualityComparer<System.DateTime>
	// System.Collections.Generic.ObjectEqualityComparer<System.Decimal>
	// System.Collections.Generic.ObjectEqualityComparer<System.Guid>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<object,int>>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<ulong>
	// System.Collections.Generic.ObjectEqualityComparer<ushort>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<CryPrinter.Pixel>
	// System.Collections.ObjectModel.ReadOnlyCollection<Loom.DelayedQueueItem>
	// System.Collections.ObjectModel.ReadOnlyCollection<Loom.NoDelayedQueueItem>
	// System.Collections.ObjectModel.ReadOnlyCollection<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.DateTime>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Guid>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.ValueTuple<object,int>>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3>
	// System.Collections.ObjectModel.ReadOnlyCollection<byte>
	// System.Collections.ObjectModel.ReadOnlyCollection<float>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<long>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<ulong>
	// System.Collections.ObjectModel.ReadOnlyCollection<ushort>
	// System.Comparison<CryPrinter.Pixel>
	// System.Comparison<Loom.DelayedQueueItem>
	// System.Comparison<Loom.NoDelayedQueueItem>
	// System.Comparison<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Comparison<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Comparison<System.DateTime>
	// System.Comparison<System.Guid>
	// System.Comparison<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Comparison<System.ValueTuple<object,int>>
	// System.Comparison<UnityEngine.Vector3>
	// System.Comparison<byte>
	// System.Comparison<float>
	// System.Comparison<int>
	// System.Comparison<long>
	// System.Comparison<object>
	// System.Comparison<ulong>
	// System.Comparison<ushort>
	// System.EventHandler<object>
	// System.Func<Loom.DelayedQueueItem,byte>
	// System.Func<System.Guid,object,object>
	// System.Func<System.Guid,object>
	// System.Func<System.Threading.Tasks.VoidTaskResult>
	// System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>
	// System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal>>
	// System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,byte>
	// System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,object>
	// System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>,byte>
	// System.Func<System.ValueTuple<object,System.Decimal>,byte>
	// System.Func<System.ValueTuple<object,int>,byte>
	// System.Func<System.ValueTuple<object,int>,object>
	// System.Func<System.ValueTuple<object,object>,byte>
	// System.Func<UnityEngine.Color,UnityEngine.Color,float,UnityEngine.Color>
	// System.Func<UnityEngine.Vector3,UnityEngine.Vector3,float,UnityEngine.Vector3>
	// System.Func<byte,byte>
	// System.Func<byte>
	// System.Func<float,float>
	// System.Func<object,System.Threading.Tasks.VoidTaskResult>
	// System.Func<object,System.UIntPtr>
	// System.Func<object,System.ValueTuple<object,object>>
	// System.Func<object,byte>
	// System.Func<object,int,int>
	// System.Func<object,int>
	// System.Func<object,long>
	// System.Func<object,object,System.UIntPtr>
	// System.Func<object,object,long>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Func<ushort,byte>
	// System.Func<ushort,ushort,ushort>
	// System.Func<ushort,ushort>
	// System.Linq.Buffer<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>
	// System.Linq.Buffer<System.ValueTuple<object,System.Decimal>>
	// System.Linq.Buffer<System.ValueTuple<object,object>>
	// System.Linq.Buffer<byte>
	// System.Linq.Buffer<int>
	// System.Linq.Buffer<object>
	// System.Linq.Buffer<ushort>
	// System.Linq.Enumerable.<CastIterator>d__99<int>
	// System.Linq.Enumerable.<CastIterator>d__99<object>
	// System.Linq.Enumerable.<DistinctIterator>d__68<object>
	// System.Linq.Enumerable.<RepeatIterator>d__117<object>
	// System.Linq.Enumerable.<ReverseIterator>d__79<int>
	// System.Linq.Enumerable.<ReverseIterator>d__79<object>
	// System.Linq.Enumerable.<SelectManyIterator>d__17<object,byte>
	// System.Linq.Enumerable.<SkipIterator>d__31<byte>
	// System.Linq.Enumerable.<TakeIterator>d__25<byte>
	// System.Linq.Enumerable.<TakeIterator>d__25<int>
	// System.Linq.Enumerable.<TakeIterator>d__25<object>
	// System.Linq.Enumerable.<TakeIterator>d__25<ushort>
	// System.Linq.Enumerable.Iterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.Iterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Linq.Enumerable.Iterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>
	// System.Linq.Enumerable.Iterator<System.ValueTuple<object,System.Decimal>>
	// System.Linq.Enumerable.Iterator<System.ValueTuple<object,int>>
	// System.Linq.Enumerable.Iterator<System.ValueTuple<object,object>>
	// System.Linq.Enumerable.Iterator<byte>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereArrayIterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.WhereArrayIterator<object>
	// System.Linq.Enumerable.WhereEnumerableIterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.WhereEnumerableIterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>
	// System.Linq.Enumerable.WhereEnumerableIterator<System.ValueTuple<object,System.Decimal>>
	// System.Linq.Enumerable.WhereEnumerableIterator<System.ValueTuple<object,object>>
	// System.Linq.Enumerable.WhereEnumerableIterator<byte>
	// System.Linq.Enumerable.WhereEnumerableIterator<object>
	// System.Linq.Enumerable.WhereListIterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.WhereListIterator<object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>
	// System.Linq.Enumerable.WhereSelectArrayIterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal>>
	// System.Linq.Enumerable.WhereSelectArrayIterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<System.ValueTuple<object,int>,object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<byte,byte>
	// System.Linq.Enumerable.WhereSelectArrayIterator<object,System.ValueTuple<object,object>>
	// System.Linq.Enumerable.WhereSelectArrayIterator<object,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal>>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<System.ValueTuple<object,int>,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<byte,byte>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<object,System.ValueTuple<object,object>>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<object,object>
	// System.Linq.Enumerable.WhereSelectListIterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>
	// System.Linq.Enumerable.WhereSelectListIterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal>>
	// System.Linq.Enumerable.WhereSelectListIterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,object>
	// System.Linq.Enumerable.WhereSelectListIterator<System.ValueTuple<object,int>,object>
	// System.Linq.Enumerable.WhereSelectListIterator<byte,byte>
	// System.Linq.Enumerable.WhereSelectListIterator<object,System.ValueTuple<object,object>>
	// System.Linq.Enumerable.WhereSelectListIterator<object,object>
	// System.Linq.EnumerableSorter<ushort,ushort>
	// System.Linq.EnumerableSorter<ushort>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<ushort>
	// System.Linq.OrderedEnumerable<ushort,ushort>
	// System.Linq.OrderedEnumerable<ushort>
	// System.Linq.Set<object>
	// System.Nullable<System.DateTime>
	// System.Nullable<System.TimeSpan>
	// System.Nullable<System.ValueTuple<byte,byte,ushort,object>>
	// System.Nullable<System.ValueTuple<object,object>>
	// System.Nullable<UnityEngine.Color>
	// System.Nullable<byte>
	// System.Nullable<float>
	// System.Nullable<int>
	// System.Nullable<long>
	// System.Predicate<CryPrinter.Pixel>
	// System.Predicate<Loom.DelayedQueueItem>
	// System.Predicate<Loom.NoDelayedQueueItem>
	// System.Predicate<SlotMaker.ReelSettingBlackboard.STReelSetting>
	// System.Predicate<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Predicate<System.DateTime>
	// System.Predicate<System.Guid>
	// System.Predicate<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>
	// System.Predicate<System.ValueTuple<object,int>>
	// System.Predicate<UnityEngine.Vector3>
	// System.Predicate<byte>
	// System.Predicate<float>
	// System.Predicate<int>
	// System.Predicate<long>
	// System.Predicate<object>
	// System.Predicate<ulong>
	// System.Predicate<ushort>
	// System.ReadOnlySpan<byte>
	// System.ReadOnlySpan<int>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Span.Enumerator<byte>
	// System.Span.Enumerator<int>
	// System.Span<byte>
	// System.Span<int>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task.<>c<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.Task.<>c<object>
	// System.Threading.Tasks.Task<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskCompletionSource<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_1<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_1<object>
	// System.Threading.Tasks.TaskFactory<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.TaskFactory<object>
	// System.Threading.ThreadLocal.FinalizationHelper<object>
	// System.Threading.ThreadLocal.IdManager<object>
	// System.Threading.ThreadLocal.LinkedSlot<object>
	// System.Threading.ThreadLocal<object>
	// System.Tuple<object,System.Decimal>
	// System.Tuple<object,object>
	// System.ValueTuple<byte,byte,ushort,object>
	// System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>
	// System.ValueTuple<object,System.Decimal>
	// System.ValueTuple<object,int>
	// System.ValueTuple<object,object>
	// UnityEngine.Events.InvokableCall<System.DateTime>
	// UnityEngine.Events.InvokableCall<UnityEngine.Rect>
	// UnityEngine.Events.InvokableCall<UnityEngine.Vector2>
	// UnityEngine.Events.InvokableCall<UnityEngine.Vector3>
	// UnityEngine.Events.InvokableCall<UnityEngine.Vector4>
	// UnityEngine.Events.InvokableCall<byte>
	// UnityEngine.Events.InvokableCall<double>
	// UnityEngine.Events.InvokableCall<float>
	// UnityEngine.Events.InvokableCall<int>
	// UnityEngine.Events.InvokableCall<long>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<System.DateTime>
	// UnityEngine.Events.UnityAction<UnityEngine.Rect>
	// UnityEngine.Events.UnityAction<UnityEngine.Vector2>
	// UnityEngine.Events.UnityAction<UnityEngine.Vector3>
	// UnityEngine.Events.UnityAction<UnityEngine.Vector4>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<double>
	// UnityEngine.Events.UnityAction<float>
	// UnityEngine.Events.UnityAction<int>
	// UnityEngine.Events.UnityAction<long>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityAction<ulong>
	// UnityEngine.Events.UnityEvent<System.DateTime>
	// UnityEngine.Events.UnityEvent<UnityEngine.Rect>
	// UnityEngine.Events.UnityEvent<UnityEngine.Vector2>
	// UnityEngine.Events.UnityEvent<UnityEngine.Vector3>
	// UnityEngine.Events.UnityEvent<UnityEngine.Vector4>
	// UnityEngine.Events.UnityEvent<byte>
	// UnityEngine.Events.UnityEvent<double>
	// UnityEngine.Events.UnityEvent<float>
	// UnityEngine.Events.UnityEvent<int>
	// UnityEngine.Events.UnityEvent<long>
	// UnityEngine.Events.UnityEvent<object>
	// ZXing.BarcodeWriter<object>
	// ZXing.Rendering.IBarcodeRenderer<object>
	// }}

	public void RefMethods()
	{
		// object CSRedis.CSRedisClient.BLPop<object>(int,string[])
		// System.Nullable<System.ValueTuple<string,object>> CSRedis.CSRedisClient.BLPopWithKey<object>(int,string[])
		// object CSRedis.CSRedisClient.BRPop<object>(int,string[])
		// object CSRedis.CSRedisClient.BRPopLPush<object>(string,string,int)
		// System.Nullable<System.ValueTuple<string,object>> CSRedis.CSRedisClient.BRPopWithKey<object>(int,string[])
		// object CSRedis.CSRedisClient.DeserializeObject<object>(string)
		// object[] CSRedis.CSRedisClient.DeserializeRedisValueArrayInternal<object>(byte[][])
		// System.Collections.Generic.Dictionary<object,object> CSRedis.CSRedisClient.DeserializeRedisValueDictionaryInternal<object,object>(System.Collections.Generic.Dictionary<object,byte[]>)
		// object CSRedis.CSRedisClient.DeserializeRedisValueInternal<object>(byte[])
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.DeserializeRedisValueTuple1Internal<object,System.Decimal>(System.Tuple<byte[],System.Decimal>[])
		// object[] CSRedis.CSRedisClient.ExecuteArray<object>(string[],System.Func<CSRedis.Internal.ObjectPool.Object<CSRedis.RedisClient>,string[],object[]>)
		// long CSRedis.CSRedisClient.ExecuteScalar<long>(string,System.Func<CSRedis.Internal.ObjectPool.Object<CSRedis.RedisClient>,string,long>)
		// object CSRedis.CSRedisClient.ExecuteScalar<object>(string,System.Func<CSRedis.Internal.ObjectPool.Object<CSRedis.RedisClient>,string,object>)
		// object[] CSRedis.CSRedisClient.GeoRadius<object>(string,System.Decimal,System.Decimal,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// object[] CSRedis.CSRedisClient.GeoRadiusByMember<object>(string,object,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.GeoRadiusByMemberWithDist<object>(string,object,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>[] CSRedis.CSRedisClient.GeoRadiusByMemberWithDistAndCoord<object>(string,object,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.GeoRadiusWithDist<object>(string,System.Decimal,System.Decimal,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>[] CSRedis.CSRedisClient.GeoRadiusWithDistAndCoord<object>(string,System.Decimal,System.Decimal,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// object CSRedis.CSRedisClient.Get<object>(string)
		// System.UIntPtr CSRedis.CSRedisClient.GetAndExecute<System.UIntPtr>(CSRedis.RedisClientPool,System.Func<CSRedis.Internal.ObjectPool.Object<CSRedis.RedisClient>,System.UIntPtr>,int,int)
		// int CSRedis.CSRedisClient.GetAndExecute<int>(CSRedis.RedisClientPool,System.Func<CSRedis.Internal.ObjectPool.Object<CSRedis.RedisClient>,int>,int,int)
		// long CSRedis.CSRedisClient.GetAndExecute<long>(CSRedis.RedisClientPool,System.Func<CSRedis.Internal.ObjectPool.Object<CSRedis.RedisClient>,long>,int,int)
		// object CSRedis.CSRedisClient.GetAndExecute<object>(CSRedis.RedisClientPool,System.Func<CSRedis.Internal.ObjectPool.Object<CSRedis.RedisClient>,object>,int,int)
		// object CSRedis.CSRedisClient.GetRange<object>(string,long,long)
		// object CSRedis.CSRedisClient.GetSet<object>(string,object)
		// object CSRedis.CSRedisClient.HGet<object>(string,string)
		// System.Collections.Generic.Dictionary<string,object> CSRedis.CSRedisClient.HGetAll<object>(string)
		// object[] CSRedis.CSRedisClient.HMGet<object>(string,string[])
		// CSRedis.RedisScan<System.ValueTuple<string,object>> CSRedis.CSRedisClient.HScan<object>(string,long,string,System.Nullable<long>)
		// object[] CSRedis.CSRedisClient.HVals<object>(string)
		// object CSRedis.CSRedisClient.LIndex<object>(string,long)
		// object CSRedis.CSRedisClient.LPop<object>(string)
		// long CSRedis.CSRedisClient.LPush<object>(string,object[])
		// object[] CSRedis.CSRedisClient.LRange<object>(string,long,long)
		// object[] CSRedis.CSRedisClient.MGet<object>(string[])
		// object CSRedis.CSRedisClient.NodesNotSupport<object>(string,System.Func<CSRedis.Internal.ObjectPool.Object<CSRedis.RedisClient>,string,object>)
		// object CSRedis.CSRedisClient.NodesNotSupport<object>(string[],object,System.Func<CSRedis.Internal.ObjectPool.Object<CSRedis.RedisClient>,string[],object>)
		// object CSRedis.CSRedisClient.RPop<object>(string)
		// object CSRedis.CSRedisClient.RPopLPush<object>(string,string)
		// long CSRedis.CSRedisClient.RPush<object>(string,object[])
		// long CSRedis.CSRedisClient.SAdd<object>(string,object[])
		// object[] CSRedis.CSRedisClient.SDiff<object>(string[])
		// object[] CSRedis.CSRedisClient.SInter<object>(string[])
		// object[] CSRedis.CSRedisClient.SMembers<object>(string)
		// object CSRedis.CSRedisClient.SPop<object>(string)
		// object[] CSRedis.CSRedisClient.SPop<object>(string,long)
		// object CSRedis.CSRedisClient.SRandMember<object>(string)
		// object[] CSRedis.CSRedisClient.SRandMembers<object>(string,int)
		// long CSRedis.CSRedisClient.SRem<object>(string,object[])
		// CSRedis.RedisScan<object> CSRedis.CSRedisClient.SScan<object>(string,long,string,System.Nullable<long>)
		// object[] CSRedis.CSRedisClient.SUnion<object>(string[])
		// CSRedis.RedisScan<object> CSRedis.CSRedisClient.Scan<object>(long,string,System.Nullable<long>)
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.ZPopMax<object>(string,long)
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.ZPopMin<object>(string,long)
		// object[] CSRedis.CSRedisClient.ZRange<object>(string,long,long)
		// object[] CSRedis.CSRedisClient.ZRangeByLex<object>(string,string,string,System.Nullable<long>,long)
		// object[] CSRedis.CSRedisClient.ZRangeByScore<object>(string,System.Decimal,System.Decimal,System.Nullable<long>,long)
		// object[] CSRedis.CSRedisClient.ZRangeByScore<object>(string,string,string,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.ZRangeByScoreWithScores<object>(string,System.Decimal,System.Decimal,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.ZRangeByScoreWithScores<object>(string,string,string,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.ZRangeWithScores<object>(string,long,long)
		// long CSRedis.CSRedisClient.ZRem<object>(string,object[])
		// object[] CSRedis.CSRedisClient.ZRevRange<object>(string,long,long)
		// object[] CSRedis.CSRedisClient.ZRevRangeByScore<object>(string,System.Decimal,System.Decimal,System.Nullable<long>,long)
		// object[] CSRedis.CSRedisClient.ZRevRangeByScore<object>(string,string,string,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.ZRevRangeByScoreWithScores<object>(string,System.Decimal,System.Decimal,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.ZRevRangeByScoreWithScores<object>(string,string,string,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] CSRedis.CSRedisClient.ZRevRangeWithScores<object>(string,long,long)
		// CSRedis.RedisScan<System.ValueTuple<object,System.Decimal>> CSRedis.CSRedisClient.ZScan<object>(string,long,string,System.Nullable<long>)
		// object DG.Tweening.TweenExtensions.Pause<object>(object)
		// object DG.Tweening.TweenExtensions.Play<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.OnComplete<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,DG.Tweening.Ease)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,DG.Tweening.EaseFunction)
		// SlotDllAlgorithmG152.JackpotData Newtonsoft.Json.JsonConvert.DeserializeObject<SlotDllAlgorithmG152.JackpotData>(string)
		// SlotDllAlgorithmG152.JackpotData Newtonsoft.Json.JsonConvert.DeserializeObject<SlotDllAlgorithmG152.JackpotData>(string,Newtonsoft.Json.JsonSerializerSettings)
		// SlotDllAlgorithmG152.LinkInfo Newtonsoft.Json.JsonConvert.DeserializeObject<SlotDllAlgorithmG152.LinkInfo>(string)
		// SlotDllAlgorithmG152.LinkInfo Newtonsoft.Json.JsonConvert.DeserializeObject<SlotDllAlgorithmG152.LinkInfo>(string,Newtonsoft.Json.JsonSerializerSettings)
		// SlotDllAlgorithmG152.PullData Newtonsoft.Json.JsonConvert.DeserializeObject<SlotDllAlgorithmG152.PullData>(string)
		// SlotDllAlgorithmG152.PullData Newtonsoft.Json.JsonConvert.DeserializeObject<SlotDllAlgorithmG152.PullData>(string,Newtonsoft.Json.JsonSerializerSettings)
		// SlotDllAlgorithmG152.SlotData Newtonsoft.Json.JsonConvert.DeserializeObject<SlotDllAlgorithmG152.SlotData>(string)
		// SlotDllAlgorithmG152.SlotData Newtonsoft.Json.JsonConvert.DeserializeObject<SlotDllAlgorithmG152.SlotData>(string,Newtonsoft.Json.JsonSerializerSettings)
		// SlotDllAlgorithmG152.Summary Newtonsoft.Json.JsonConvert.DeserializeObject<SlotDllAlgorithmG152.Summary>(string)
		// SlotDllAlgorithmG152.Summary Newtonsoft.Json.JsonConvert.DeserializeObject<SlotDllAlgorithmG152.Summary>(string,Newtonsoft.Json.JsonSerializerSettings)
		// object Newtonsoft.Json.JsonConvert.DeserializeObject<object>(string)
		// object Newtonsoft.Json.JsonConvert.DeserializeObject<object>(string,Newtonsoft.Json.JsonSerializerSettings)
		// byte Newtonsoft.Json.Linq.JToken.ToObject<byte>()
		// int Newtonsoft.Json.Linq.JToken.ToObject<int>()
		// object Newtonsoft.Json.Linq.JToken.ToObject<object>()
		// object RedisHelper<object>.BLPop<object>(int,string[])
		// System.Nullable<System.ValueTuple<string,object>> RedisHelper<object>.BLPopWithKey<object>(int,string[])
		// object RedisHelper<object>.BRPop<object>(int,string[])
		// object RedisHelper<object>.BRPopLPush<object>(string,string,int)
		// System.Nullable<System.ValueTuple<string,object>> RedisHelper<object>.BRPopWithKey<object>(int,string[])
		// object[] RedisHelper<object>.GeoRadius<object>(string,System.Decimal,System.Decimal,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// object[] RedisHelper<object>.GeoRadiusByMember<object>(string,object,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// System.ValueTuple<object,System.Decimal>[] RedisHelper<object>.GeoRadiusByMemberWithDist<object>(string,object,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>[] RedisHelper<object>.GeoRadiusByMemberWithDistAndCoord<object>(string,object,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// System.ValueTuple<object,System.Decimal>[] RedisHelper<object>.GeoRadiusWithDist<object>(string,System.Decimal,System.Decimal,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>[] RedisHelper<object>.GeoRadiusWithDistAndCoord<object>(string,System.Decimal,System.Decimal,System.Decimal,CSRedis.GeoUnit,System.Nullable<long>,System.Nullable<CSRedis.GeoOrderBy>)
		// object RedisHelper<object>.Get<object>(string)
		// object RedisHelper<object>.GetRange<object>(string,long,long)
		// object RedisHelper<object>.GetSet<object>(string,object)
		// object RedisHelper<object>.HGet<object>(string,string)
		// System.Collections.Generic.Dictionary<string,object> RedisHelper<object>.HGetAll<object>(string)
		// object[] RedisHelper<object>.HMGet<object>(string,string[])
		// CSRedis.RedisScan<System.ValueTuple<string,object>> RedisHelper<object>.HScan<object>(string,long,string,System.Nullable<long>)
		// object[] RedisHelper<object>.HVals<object>(string)
		// object RedisHelper<object>.LIndex<object>(string,long)
		// object RedisHelper<object>.LPop<object>(string)
		// long RedisHelper<object>.LPush<object>(string,object[])
		// object[] RedisHelper<object>.LRange<object>(string,long,long)
		// object[] RedisHelper<object>.MGet<object>(string[])
		// object RedisHelper<object>.RPop<object>(string)
		// object RedisHelper<object>.RPopLPush<object>(string,string)
		// long RedisHelper<object>.RPush<object>(string,object[])
		// long RedisHelper<object>.SAdd<object>(string,object[])
		// object[] RedisHelper<object>.SDiff<object>(string[])
		// object[] RedisHelper<object>.SInter<object>(string[])
		// object[] RedisHelper<object>.SMembers<object>(string)
		// object RedisHelper<object>.SPop<object>(string)
		// object[] RedisHelper<object>.SPop<object>(string,long)
		// object RedisHelper<object>.SRandMember<object>(string)
		// object[] RedisHelper<object>.SRandMembers<object>(string,int)
		// long RedisHelper<object>.SRem<object>(string,object[])
		// CSRedis.RedisScan<object> RedisHelper<object>.SScan<object>(string,long,string,System.Nullable<long>)
		// object[] RedisHelper<object>.SUnion<object>(string[])
		// CSRedis.RedisScan<object> RedisHelper<object>.Scan<object>(string,long,string,System.Nullable<long>)
		// System.ValueTuple<object,System.Decimal>[] RedisHelper<object>.ZPopMax<object>(string,long)
		// System.ValueTuple<object,System.Decimal>[] RedisHelper<object>.ZPopMin<object>(string,long)
		// object[] RedisHelper<object>.ZRange<object>(string,long,long)
		// object[] RedisHelper<object>.ZRangeByLex<object>(string,string,string,System.Nullable<long>,long)
		// object[] RedisHelper<object>.ZRangeByScore<object>(string,System.Decimal,System.Decimal,System.Nullable<long>,long)
		// object[] RedisHelper<object>.ZRangeByScore<object>(string,string,string,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] RedisHelper<object>.ZRangeByScoreWithScores<object>(string,System.Decimal,System.Decimal,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] RedisHelper<object>.ZRangeByScoreWithScores<object>(string,string,string,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] RedisHelper<object>.ZRangeWithScores<object>(string,long,long)
		// long RedisHelper<object>.ZRem<object>(string,object[])
		// object[] RedisHelper<object>.ZRevRange<object>(string,long,long)
		// object[] RedisHelper<object>.ZRevRangeByScore<object>(string,System.Decimal,System.Decimal,System.Nullable<long>,long)
		// object[] RedisHelper<object>.ZRevRangeByScore<object>(string,string,string,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] RedisHelper<object>.ZRevRangeByScoreWithScores<object>(string,System.Decimal,System.Decimal,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] RedisHelper<object>.ZRevRangeByScoreWithScores<object>(string,string,string,System.Nullable<long>,long)
		// System.ValueTuple<object,System.Decimal>[] RedisHelper<object>.ZRevRangeWithScores<object>(string,long,long)
		// CSRedis.RedisScan<System.ValueTuple<object,System.Decimal>> RedisHelper<object>.ZScan<object>(string,long,string,System.Nullable<long>)
		// object System.Activator.CreateInstance<object>()
		// byte[] System.Array.Empty<byte>()
		// object[] System.Array.Empty<object>()
		// ushort[] System.Array.Empty<ushort>()
		// int System.Array.IndexOf<byte>(byte[],byte)
		// int System.Array.IndexOfImpl<byte>(byte[],byte,int,int)
		// bool System.Linq.Enumerable.Any<object>(System.Collections.Generic.IEnumerable<object>)
		// bool System.Linq.Enumerable.Any<ushort>(System.Collections.Generic.IEnumerable<ushort>,System.Func<ushort,bool>)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Cast<int>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Cast<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.CastIterator<int>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.CastIterator<object>(System.Collections.IEnumerable)
		// bool System.Linq.Enumerable.Contains<object>(System.Collections.Generic.IEnumerable<object>,object)
		// bool System.Linq.Enumerable.Contains<object>(System.Collections.Generic.IEnumerable<object>,object,System.Collections.Generic.IEqualityComparer<object>)
		// int System.Linq.Enumerable.Count<object>(System.Collections.Generic.IEnumerable<object>)
		// int System.Linq.Enumerable.Count<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// int System.Linq.Enumerable.Count<ushort>(System.Collections.Generic.IEnumerable<ushort>,System.Func<ushort,bool>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Distinct<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.DistinctIterator<object>(System.Collections.Generic.IEnumerable<object>,System.Collections.Generic.IEqualityComparer<object>)
		// System.Collections.Generic.KeyValuePair<object,object> System.Linq.Enumerable.ElementAt<System.Collections.Generic.KeyValuePair<object,object>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>,int)
		// System.Collections.Generic.KeyValuePair<ulong,int> System.Linq.Enumerable.ElementAt<System.Collections.Generic.KeyValuePair<ulong,int>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,int>>,int)
		// object System.Linq.Enumerable.ElementAt<object>(System.Collections.Generic.IEnumerable<object>,int)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Empty<int>()
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Empty<object>()
		// System.Collections.Generic.KeyValuePair<object,object> System.Linq.Enumerable.First<System.Collections.Generic.KeyValuePair<object,object>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>)
		// object System.Linq.Enumerable.First<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.KeyValuePair<object,object> System.Linq.Enumerable.FirstOrDefault<System.Collections.Generic.KeyValuePair<object,object>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>)
		// System.Linq.IOrderedEnumerable<ushort> System.Linq.Enumerable.OrderBy<ushort,ushort>(System.Collections.Generic.IEnumerable<ushort>,System.Func<ushort,ushort>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Repeat<object>(object,int)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.RepeatIterator<object>(object,int)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Reverse<int>(System.Collections.Generic.IEnumerable<int>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Reverse<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.ReverseIterator<int>(System.Collections.Generic.IEnumerable<int>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.ReverseIterator<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>> System.Linq.Enumerable.Select<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>(System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>,System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>)
		// System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal>> System.Linq.Enumerable.Select<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal>>(System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>,System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal>>)
		// System.Collections.Generic.IEnumerable<System.ValueTuple<object,object>> System.Linq.Enumerable.Select<object,System.ValueTuple<object,object>>(System.Collections.Generic.IEnumerable<object>,System.Func<object,System.ValueTuple<object,object>>)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.Select<byte,byte>(System.Collections.Generic.IEnumerable<byte>,System.Func<byte,byte>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,object>(System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>,System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<System.ValueTuple<object,int>,object>(System.Collections.Generic.IEnumerable<System.ValueTuple<object,int>>,System.Func<System.ValueTuple<object,int>,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.SelectMany<object,byte>(System.Collections.Generic.IEnumerable<object>,System.Func<object,System.Collections.Generic.IEnumerable<byte>>)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.SelectManyIterator<object,byte>(System.Collections.Generic.IEnumerable<object>,System.Func<object,System.Collections.Generic.IEnumerable<byte>>)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.Skip<byte>(System.Collections.Generic.IEnumerable<byte>,int)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.SkipIterator<byte>(System.Collections.Generic.IEnumerable<byte>,int)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.Take<byte>(System.Collections.Generic.IEnumerable<byte>,int)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Take<int>(System.Collections.Generic.IEnumerable<int>,int)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Take<object>(System.Collections.Generic.IEnumerable<object>,int)
		// System.Collections.Generic.IEnumerable<ushort> System.Linq.Enumerable.Take<ushort>(System.Collections.Generic.IEnumerable<ushort>,int)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.TakeIterator<byte>(System.Collections.Generic.IEnumerable<byte>,int)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.TakeIterator<int>(System.Collections.Generic.IEnumerable<int>,int)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.TakeIterator<object>(System.Collections.Generic.IEnumerable<object>,int)
		// System.Collections.Generic.IEnumerable<ushort> System.Linq.Enumerable.TakeIterator<ushort>(System.Collections.Generic.IEnumerable<ushort>,int)
		// System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>[] System.Linq.Enumerable.ToArray<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>(System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>)
		// System.ValueTuple<object,System.Decimal>[] System.Linq.Enumerable.ToArray<System.ValueTuple<object,System.Decimal>>(System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal>>)
		// System.ValueTuple<object,object>[] System.Linq.Enumerable.ToArray<System.ValueTuple<object,object>>(System.Collections.Generic.IEnumerable<System.ValueTuple<object,object>>)
		// byte[] System.Linq.Enumerable.ToArray<byte>(System.Collections.Generic.IEnumerable<byte>)
		// int[] System.Linq.Enumerable.ToArray<int>(System.Collections.Generic.IEnumerable<int>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// ushort[] System.Linq.Enumerable.ToArray<ushort>(System.Collections.Generic.IEnumerable<ushort>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<Loom.DelayedQueueItem> System.Linq.Enumerable.Where<Loom.DelayedQueueItem>(System.Collections.Generic.IEnumerable<Loom.DelayedQueueItem>,System.Func<Loom.DelayedQueueItem,bool>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Where<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>> System.Linq.Enumerable.Iterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>.Select<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>(System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal>>)
		// System.Collections.Generic.IEnumerable<System.ValueTuple<object,System.Decimal>> System.Linq.Enumerable.Iterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>.Select<System.ValueTuple<object,System.Decimal>>(System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,System.ValueTuple<object,System.Decimal>>)
		// System.Collections.Generic.IEnumerable<System.ValueTuple<object,object>> System.Linq.Enumerable.Iterator<object>.Select<System.ValueTuple<object,object>>(System.Func<object,System.ValueTuple<object,object>>)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.Iterator<byte>.Select<byte>(System.Func<byte,byte>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>>.Select<object>(System.Func<System.ValueTuple<object,System.Decimal,System.Decimal,System.Decimal,long>,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<System.ValueTuple<object,int>>.Select<object>(System.Func<System.ValueTuple<object,int>,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<object>.Select<object>(System.Func<object,object>)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.PageBase.<AsyncOpen01>d__24>(System.Runtime.CompilerServices.TaskAwaiter&,Game.PageBase.<AsyncOpen01>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.PageBase.<WaitUntil>d__23>(System.Runtime.CompilerServices.TaskAwaiter&,Game.PageBase.<WaitUntil>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityWebSocket.WebSocket.<ConnectTask>d__36>(System.Runtime.CompilerServices.TaskAwaiter&,UnityWebSocket.WebSocket.<ConnectTask>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityWebSocket.WebSocket.<SendTask>d__42>(System.Runtime.CompilerServices.TaskAwaiter&,UnityWebSocket.WebSocket.<SendTask>d__42&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityWebSocket.WebSocket.<ReceiveTask>d__43>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityWebSocket.WebSocket.<ReceiveTask>d__43&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.PageBase.<AsyncOpen01>d__24>(System.Runtime.CompilerServices.TaskAwaiter&,Game.PageBase.<AsyncOpen01>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.PageBase.<WaitUntil>d__23>(System.Runtime.CompilerServices.TaskAwaiter&,Game.PageBase.<WaitUntil>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityWebSocket.WebSocket.<ConnectTask>d__36>(System.Runtime.CompilerServices.TaskAwaiter&,UnityWebSocket.WebSocket.<ConnectTask>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityWebSocket.WebSocket.<SendTask>d__42>(System.Runtime.CompilerServices.TaskAwaiter&,UnityWebSocket.WebSocket.<SendTask>d__42&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityWebSocket.WebSocket.<ReceiveTask>d__43>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityWebSocket.WebSocket.<ReceiveTask>d__43&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.PageBase.<AsyncOpen02>d__25>(System.Runtime.CompilerServices.TaskAwaiter&,Game.PageBase.<AsyncOpen02>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.PageBase.<OnOpenAsync>d__19>(System.Runtime.CompilerServices.TaskAwaiter&,Game.PageBase.<OnOpenAsync>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.PageManager.<OpenPageAsync>d__24>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.PageManager.<OpenPageAsync>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,SQLitePlayerPrefs03.<GetData>d__28>(System.Runtime.CompilerServices.TaskAwaiter<object>&,SQLitePlayerPrefs03.<GetData>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.PageBase.<AsyncOpen01>d__24>(Game.PageBase.<AsyncOpen01>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.PageBase.<WaitUntil>d__23>(Game.PageBase.<WaitUntil>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityWebSocket.WebSocket.<ConnectTask>d__36>(UnityWebSocket.WebSocket.<ConnectTask>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityWebSocket.WebSocket.<ReceiveTask>d__43>(UnityWebSocket.WebSocket.<ReceiveTask>d__43&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityWebSocket.WebSocket.<SendTask>d__42>(UnityWebSocket.WebSocket.<SendTask>d__42&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<Game.PageBase.<AsyncOpen02>d__25>(Game.PageBase.<AsyncOpen02>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<Game.PageBase.<OnOpenAsync>d__19>(Game.PageBase.<OnOpenAsync>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<Game.PageManager.<OpenPageAsync>d__24>(Game.PageManager.<OpenPageAsync>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<SQLitePlayerPrefs03.<GetData>d__28>(SQLitePlayerPrefs03.<GetData>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,MoneyBox.WebSocketClient.<ConnectWS>d__14>(System.Runtime.CompilerServices.TaskAwaiter&,MoneyBox.WebSocketClient.<ConnectWS>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,MoneyBox.WebSocketClient.<Reconnect>d__35>(System.Runtime.CompilerServices.TaskAwaiter&,MoneyBox.WebSocketClient.<Reconnect>d__35&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<<OnClickSetPasswordAdmin>b__115_0>d>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<<OnClickSetPasswordAdmin>b__115_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<<OnClickSetPasswordManager>b__117_0>d>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<<OnClickSetPasswordManager>b__117_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<<OnClickSetPasswordShift>b__119_0>d>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<<OnClickSetPasswordShift>b__119_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickAgentIDMachineID>d__133>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickAgentIDMachineID>d__133&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickBetSet>d__82>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickBetSet>d__82&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickBillValidatorModel>d__127>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickBillValidatorModel>d__127&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickBonusReportSetting>d__131>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickBonusReportSetting>d__131&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickCoinInScale>d__107>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickCoinInScale>d__107&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickCoinOutScale>d__106>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickCoinOutScale>d__106&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickIOTAccessMethods>d__132>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickIOTAccessMethods>d__132&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickJackpotGamePercent>d__134>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickJackpotGamePercent>d__134&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickJackpotPercent>d__112>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickJackpotPercent>d__112&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickLevel>d__135>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickLevel>d__135&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickMaxBusinessDayRecord>d__126>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickMaxBusinessDayRecord>d__126&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickMaxCoinInOutRecord>d__122>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickMaxCoinInOutRecord>d__122&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickMaxErrorRecord>d__125>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickMaxErrorRecord>d__125&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickMaxEventRecord>d__124>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickMaxEventRecord>d__124&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickMaxGameRecord>d__123>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickMaxGameRecord>d__123&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickPrinterModel>d__128>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickPrinterModel>d__128&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickRemoteControlAccount>d__130>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickRemoteControlAccount>d__130&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickRemoteControlSetting>d__129>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickRemoteControlSetting>d__129&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickScoreScale>d__108>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickScoreScale>d__108&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickScoreUpLongClickScale>d__109>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickScoreUpLongClickScale>d__109&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickSetPasswordAdmin001>d__116>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickSetPasswordAdmin001>d__116&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickSetPasswordAdmin>d__115>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickSetPasswordAdmin>d__115&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickSetPasswordManager001>d__118>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickSetPasswordManager001>d__118&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickSetPasswordManager>d__117>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickSetPasswordManager>d__117&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickSetPasswordShift001>d__120>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickSetPasswordShift001>d__120&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickSetPasswordShift>d__119>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickSetPasswordShift>d__119&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMachineSettings.<OnClickWaveScore>d__136>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMachineSettings.<OnClickWaveScore>d__136&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMain.<OnChenkUser>d__24>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMain.<OnChenkUser>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMain.<OnClickLanguage>d__33>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMain.<OnClickLanguage>d__33&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMain.<OnClickTimeAndDate>d__30>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMain.<OnClickTimeAndDate>d__30&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Console001.PageConsoleMain.<OnClickVolumeSetting>d__26>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Console001.PageConsoleMain.<OnClickVolumeSetting>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,DeviceCoder.<OnResponseShowCoder>d__8>(System.Runtime.CompilerServices.TaskAwaiter<object>&,DeviceCoder.<OnResponseShowCoder>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.PageBase.<TestAsyncFunc>d__26>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.PageBase.<TestAsyncFunc>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.PageManager.<OpenPageAsync>d__25>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.PageManager.<OpenPageAsync>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,MoneyBox.WebSocketClient.<ConnectWS>d__14>(System.Runtime.CompilerServices.TaskAwaiter<object>&,MoneyBox.WebSocketClient.<ConnectWS>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,PageConsoleAdmin.<OnClickAgentID>d__46>(System.Runtime.CompilerServices.TaskAwaiter<object>&,PageConsoleAdmin.<OnClickAgentID>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,PageConsoleAdmin.<OnClickMachineID>d__47>(System.Runtime.CompilerServices.TaskAwaiter<object>&,PageConsoleAdmin.<OnClickMachineID>d__47&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,PageConsoleAdmin.<OnClickSasAccount>d__52>(System.Runtime.CompilerServices.TaskAwaiter<object>&,PageConsoleAdmin.<OnClickSasAccount>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,PageConsoleAdmin.<OnClickSasConnection>d__50>(System.Runtime.CompilerServices.TaskAwaiter<object>&,PageConsoleAdmin.<OnClickSasConnection>d__50&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,PageConsoleAdmin.<OnClickSasInOutScale>d__54>(System.Runtime.CompilerServices.TaskAwaiter<object>&,PageConsoleAdmin.<OnClickSasInOutScale>d__54&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,PopupConsoleMoneyBoxRedeem.<OnclickInputMoney>d__16>(System.Runtime.CompilerServices.TaskAwaiter<object>&,PopupConsoleMoneyBoxRedeem.<OnclickInputMoney>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,PopupConsoleSlideSetting001.<OnClickKeyboard>d__21>(System.Runtime.CompilerServices.TaskAwaiter<object>&,PopupConsoleSlideSetting001.<OnClickKeyboard>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,PssOn00152.PopupGameLoading.<CheckUpdateInfo>d__11>(System.Runtime.CompilerServices.TaskAwaiter<object>&,PssOn00152.PopupGameLoading.<CheckUpdateInfo>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<<OnClickSetPasswordAdmin>b__115_0>d>(Console001.PageConsoleMachineSettings.<<OnClickSetPasswordAdmin>b__115_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<<OnClickSetPasswordManager>b__117_0>d>(Console001.PageConsoleMachineSettings.<<OnClickSetPasswordManager>b__117_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<<OnClickSetPasswordShift>b__119_0>d>(Console001.PageConsoleMachineSettings.<<OnClickSetPasswordShift>b__119_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickAgentIDMachineID>d__133>(Console001.PageConsoleMachineSettings.<OnClickAgentIDMachineID>d__133&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickBetSet>d__82>(Console001.PageConsoleMachineSettings.<OnClickBetSet>d__82&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickBillInScale>d__111>(Console001.PageConsoleMachineSettings.<OnClickBillInScale>d__111&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickBillValidatorModel>d__127>(Console001.PageConsoleMachineSettings.<OnClickBillValidatorModel>d__127&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickBonusReportSetting>d__131>(Console001.PageConsoleMachineSettings.<OnClickBonusReportSetting>d__131&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickCoinInScale>d__107>(Console001.PageConsoleMachineSettings.<OnClickCoinInScale>d__107&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickCoinOutScale>d__106>(Console001.PageConsoleMachineSettings.<OnClickCoinOutScale>d__106&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickIOTAccessMethods>d__132>(Console001.PageConsoleMachineSettings.<OnClickIOTAccessMethods>d__132&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickJackpotGamePercent>d__134>(Console001.PageConsoleMachineSettings.<OnClickJackpotGamePercent>d__134&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickJackpotPercent>d__112>(Console001.PageConsoleMachineSettings.<OnClickJackpotPercent>d__112&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickLevel>d__135>(Console001.PageConsoleMachineSettings.<OnClickLevel>d__135&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickMaxBusinessDayRecord>d__126>(Console001.PageConsoleMachineSettings.<OnClickMaxBusinessDayRecord>d__126&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickMaxCoinInOutRecord>d__122>(Console001.PageConsoleMachineSettings.<OnClickMaxCoinInOutRecord>d__122&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickMaxErrorRecord>d__125>(Console001.PageConsoleMachineSettings.<OnClickMaxErrorRecord>d__125&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickMaxEventRecord>d__124>(Console001.PageConsoleMachineSettings.<OnClickMaxEventRecord>d__124&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickMaxGameRecord>d__123>(Console001.PageConsoleMachineSettings.<OnClickMaxGameRecord>d__123&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickPrintOutScale>d__110>(Console001.PageConsoleMachineSettings.<OnClickPrintOutScale>d__110&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickPrinterModel>d__128>(Console001.PageConsoleMachineSettings.<OnClickPrinterModel>d__128&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickRemoteControlAccount>d__130>(Console001.PageConsoleMachineSettings.<OnClickRemoteControlAccount>d__130&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickRemoteControlSetting>d__129>(Console001.PageConsoleMachineSettings.<OnClickRemoteControlSetting>d__129&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickScoreScale>d__108>(Console001.PageConsoleMachineSettings.<OnClickScoreScale>d__108&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickScoreUpLongClickScale>d__109>(Console001.PageConsoleMachineSettings.<OnClickScoreUpLongClickScale>d__109&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickSetPasswordAdmin001>d__116>(Console001.PageConsoleMachineSettings.<OnClickSetPasswordAdmin001>d__116&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickSetPasswordAdmin>d__115>(Console001.PageConsoleMachineSettings.<OnClickSetPasswordAdmin>d__115&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickSetPasswordManager001>d__118>(Console001.PageConsoleMachineSettings.<OnClickSetPasswordManager001>d__118&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickSetPasswordManager>d__117>(Console001.PageConsoleMachineSettings.<OnClickSetPasswordManager>d__117&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickSetPasswordShift001>d__120>(Console001.PageConsoleMachineSettings.<OnClickSetPasswordShift001>d__120&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickSetPasswordShift>d__119>(Console001.PageConsoleMachineSettings.<OnClickSetPasswordShift>d__119&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMachineSettings.<OnClickWaveScore>d__136>(Console001.PageConsoleMachineSettings.<OnClickWaveScore>d__136&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMain.<OnChenkUser>d__24>(Console001.PageConsoleMain.<OnChenkUser>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMain.<OnClickLanguage>d__33>(Console001.PageConsoleMain.<OnClickLanguage>d__33&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMain.<OnClickTimeAndDate>d__30>(Console001.PageConsoleMain.<OnClickTimeAndDate>d__30&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Console001.PageConsoleMain.<OnClickVolumeSetting>d__26>(Console001.PageConsoleMain.<OnClickVolumeSetting>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<DeviceCoder.<OnResponseShowCoder>d__8>(DeviceCoder.<OnResponseShowCoder>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Game.PageBase.<TestAsyncFunc>d__26>(Game.PageBase.<TestAsyncFunc>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Game.PageManager.<OpenPageAsync>d__25>(Game.PageManager.<OpenPageAsync>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<MoneyBox.WebSocketClient.<ConnectWS>d__14>(MoneyBox.WebSocketClient.<ConnectWS>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<MoneyBox.WebSocketClient.<Reconnect>d__35>(MoneyBox.WebSocketClient.<Reconnect>d__35&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PageConsoleAdmin.<OnClickAgentID>d__46>(PageConsoleAdmin.<OnClickAgentID>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PageConsoleAdmin.<OnClickMachineID>d__47>(PageConsoleAdmin.<OnClickMachineID>d__47&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PageConsoleAdmin.<OnClickSasAccount>d__52>(PageConsoleAdmin.<OnClickSasAccount>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PageConsoleAdmin.<OnClickSasConnection>d__50>(PageConsoleAdmin.<OnClickSasConnection>d__50&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PageConsoleAdmin.<OnClickSasInOutScale>d__54>(PageConsoleAdmin.<OnClickSasInOutScale>d__54&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PopupConsoleMoneyBoxRedeem.<OnclickInputMoney>d__16>(PopupConsoleMoneyBoxRedeem.<OnclickInputMoney>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PopupConsoleSlideSetting001.<OnClickKeyboard>d__21>(PopupConsoleSlideSetting001.<OnClickKeyboard>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PssOn00152.PopupGameLoading.<CheckUpdateInfo>d__11>(PssOn00152.PopupGameLoading.<CheckUpdateInfo>d__11&)
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<byte>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<int>()
		// object System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<object>(System.IntPtr)
		// int System.Runtime.InteropServices.Marshal.SizeOf<object>(object)
		// System.Void System.Runtime.InteropServices.Marshal.StructureToPtr<object>(object,System.IntPtr,bool)
		// System.Span<int> System.Runtime.InteropServices.MemoryMarshal.Cast<byte,int>(System.Span<byte>)
		// bool System.SpanHelpers.IsReferenceOrContainsReferences<byte>()
		// bool System.SpanHelpers.IsReferenceOrContainsReferences<int>()
		// byte UnityEngine.AndroidJNIHelper.ConvertFromJNIArray<byte>(System.IntPtr)
		// object UnityEngine.AndroidJNIHelper.ConvertFromJNIArray<object>(System.IntPtr)
		// System.IntPtr UnityEngine.AndroidJNIHelper.GetFieldID<object>(System.IntPtr,string,bool)
		// System.IntPtr UnityEngine.AndroidJNIHelper.GetMethodID<byte>(System.IntPtr,string,object[],bool)
		// System.IntPtr UnityEngine.AndroidJNIHelper.GetMethodID<object>(System.IntPtr,string,object[],bool)
		// byte UnityEngine.AndroidJavaObject.Call<byte>(string,object[])
		// object UnityEngine.AndroidJavaObject.Call<object>(string,object[])
		// object UnityEngine.AndroidJavaObject.GetStatic<object>(string)
		// byte UnityEngine.AndroidJavaObject._Call<byte>(string,object[])
		// object UnityEngine.AndroidJavaObject._Call<object>(string,object[])
		// object UnityEngine.AndroidJavaObject._GetStatic<object>(string)
		// object UnityEngine.AssetBundle.LoadAsset<object>(string)
		// UnityEngine.AssetBundleRequest UnityEngine.AssetBundle.LoadAssetAsync<object>(string)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// bool UnityEngine.Component.TryGetComponent<object>(object&)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// bool UnityEngine.GameObject.TryGetComponent<object>(object&)
		// object UnityEngine.JsonUtility.FromJson<object>(string)
		// object UnityEngine.Object.FindObjectOfType<object>()
		// object[] UnityEngine.Object.FindObjectsOfType<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion)
		// object[] UnityEngine.Resources.ConvertObjects<object>(UnityEngine.Object[])
		// object[] UnityEngine.Resources.FindObjectsOfTypeAll<object>()
		// object UnityEngine.Resources.Load<object>(string)
		// object[] UnityEngine.Resources.LoadAll<object>(string)
		// UnityEngine.ResourceRequest UnityEngine.Resources.LoadAsync<object>(string)
		// byte UnityEngine._AndroidJNIHelper.ConvertFromJNIArray<byte>(System.IntPtr)
		// object UnityEngine._AndroidJNIHelper.ConvertFromJNIArray<object>(System.IntPtr)
		// System.IntPtr UnityEngine._AndroidJNIHelper.GetFieldID<object>(System.IntPtr,string,bool)
		// System.IntPtr UnityEngine._AndroidJNIHelper.GetMethodID<byte>(System.IntPtr,string,object[],bool)
		// System.IntPtr UnityEngine._AndroidJNIHelper.GetMethodID<object>(System.IntPtr,string,object[],bool)
		// string UnityEngine._AndroidJNIHelper.GetSignature<byte>(object[])
		// string UnityEngine._AndroidJNIHelper.GetSignature<object>(object[])
		// string string.Concat<ushort>(System.Collections.Generic.IEnumerable<ushort>)
		// string string.Join<int>(string,System.Collections.Generic.IEnumerable<int>)
	}
}
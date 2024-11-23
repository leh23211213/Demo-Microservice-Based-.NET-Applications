// using App.Services.ProductAPI.Controllers.v1;
// using App.Services.ProductAPI.Data;
// using BenchmarkDotNet.Attributes;
// using App.Services.ProductAPI.Data;
// using BenchmarkDotNet.Engines;

// [SimpleJob(runStrategy: RunStrategy.Throughput)]
// [MemoryDiagnoser]
// public class ProductControllerBenchmark
// {
//     private readonly ReadProductAPIController _controller;
//     private readonly ApplicationDbContext _dbContext;
//     public ProductControllerBenchmark(
//                                     ApplicationDbContext dbContext
//                                     )
//     {
//         _dbContext = dbContext;
//         _controller = new ReadProductAPIController(dbContext);
//     }

//     [Benchmark]
//     public async Task BenchmarkGetProducts()
//     {
//         var result = await _controller.Get();
//         // Optional: kiểm tra kết quả nếu cần
//     }
// }


/*开发目标：
   这个模块定义了用于操作Office的接口，但是它们只是一个标准，具体实现由其他模块负责，
   这样设计目的在于使外部调用保持一致，同时在底层可以切换不同的Excel引擎

   目标平台：
   Office标准是跨平台的，但是它的实现不一定跨平台

   重要说明：
   #本模块及其实现不会为性能过多优化，原因在于：
   1.Word和Excel是为处理小型数据集设计的，
   如果你需要处理大量数据，就应该使用更加专业的工具
   2.办公自动化对性能需求不高，因为它不像前端开发一样，需要程序立即对命令作出响应，
   你可以挂着让程序慢慢跑，反正不需要你手动操作*/
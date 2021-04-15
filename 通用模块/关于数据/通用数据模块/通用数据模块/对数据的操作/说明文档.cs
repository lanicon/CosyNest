﻿
/*问：IDataPipeQuery只能查询数据，IDataPipeAdd只能添加数据，
  为什么要区分这两种不同的管道？

  答：因为数据的来源和去向有可能不同，举个例子，
  有时候需要把从Excel中提取出来的数据导入数据库，
  另外有些数据源并不支持完整的增删改查功能，例如，
  如果一个数据是通过爬虫获取的，那么它不支持添加修改和删除
   
  问：通过这两个接口获取到的数据如果支持绑定，应该怎么实现？

  答：在从数据源读取数据或者将数据添加到数据源的时候，设置IData的Binding属性，
  使数据或数据源发生修改时能够向对方发出通知
   
  问：关于上一个问题，为什么要这样设计？有什么好处？

  答：好处是当数据被修改的时候，同步操作能够自动完成，不需要手动干预，
  并且这种设计也降低了实现接口的难度，因为它保证了在绑定数据的时候，
  程序一定知道这个数据位于数据源的什么位置

  问：为什么IDataPipeQuery.Query没有异步版本？
 
  答：因为IDirectView<IData>本身是延迟迭代的，不会阻塞当前线程，
  因此作者认为增加一个返回Task<IDirectView<IData>>或IAsyncEnumerable<IData>的方法意义不大，
  如果你需要异步遍历数据，可以调用ToAsyncEnumerable扩展方法将其转换为异步迭代器

  问：实现IDataPipeQuery和IDataPipeAdd需要遵循哪些规范？

  答：请遵循以下规范：
  第一，这些接口被设计为具备自我描述能力，也就是说，当它们被实例化的时候，
  就已经知道数据应该从哪里来，或者应该被添加到什么地方，不需要任何额外的参数或说明，
  举例来说，如果你想要查询数据库中某个表的数据，那么你就应该通过DB对象返回一个Table对象，
  并将Table对象实现IDataPipeQuery接口，以后所有通过这个对象进行的查询，都是在查询数据库中的这张表，
  而不是传入一个参数告诉数据位于数据库的什么位置，然后再进行查询
  这样设计带来的好处有三点，能够让数据模型变得直观，能够把对任何数据源的查询都变成LinqToObject的形式，
  以及提供更强大的可扩展性，因为这种设计着重于返回数据接口，而不是直接返回数据
   
  第二，应该把数据源设计为不可变的，因为这种模型非常依赖正确的数据源，如果在这方面出现问题，会很难排查
   
  第三，在添加或查询数据的时候，除非数据来源于数据库等结构化数据源，
  否则应该检查数据的架构是否一致，如果不一致，应该及时引发异常，这样可以使一些问题被尽早发现*/
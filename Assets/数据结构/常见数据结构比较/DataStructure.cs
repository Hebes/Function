namespace 常见数据结构比较
{
    /// <summary>
    /// https://zhuanlan.zhihu.com/p/638240446
    /// </summary>
    public class DataStructure
    {
        public void Init()
        {
            //摘要：在C#中，有多种常见的数据结构可用于存储和操作数据。
            //本文将详细介绍C#中的一些常见数据结构，
            //包括List<T>、Dictionary<TKey, TValue>、HashSet<T>、Queue<T>、Stack<T>和Tuple<T1, T2, ...>。
            //我们将探讨它们的特点、用法和适用场景，帮助您了解如何选择适合您需求的数据结构。
            //             1. List<T>
            // List<T>是C#中常用的动态数组，可以存储同一类型的元素。它提供了许多方法来添加、删除和访问列表中的元素。List<T>的优点是灵活性和可变性，适用于需要频繁插入和删除元素的场景。它可以按索引访问元素，并且提供了排序、搜索和过滤等功能。
            //
            // List<T> list = new List<T>();
            // list.Add(item);
            // list.Insert(index, item);
            // list.Remove(item);
            // list.RemoveAt(index);
            // T item = list[index];
            // int count = list.Count;
            // 2. Dictionary<TKey, TValue>
            // Dictionary<TKey, TValue>是C#中的键值对集合，用于存储具有唯一键的元素。它提供了快速的键值查找和访问能力。Dictionary<TKey, TValue>适用于需要根据键快速查找元素的场景，如字典、映射等。它可以使用键来添加、删除和修改元素，并提供了枚举和遍历功能。
            //
            // Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            // dictionary.Add(key, value);
            // dictionary[key] = value;
            // dictionary.Remove(key);
            // bool containsKey = dictionary.ContainsKey(key);
            // bool containsValue = dictionary.ContainsValue(value);
            // TValue value;
            // bool found = dictionary.TryGetValue(key, out value);
            // int count = dictionary.Count;
            // 3. HashSet<T>
            // HashSet<T>是C#中的集合，它存储唯一元素，并提供高效的查找、添加和删除操作。HashSet<T>适用于需要存储无重复项的场景，如去重、查找唯一元素等。它具有快速的查找性能，可以判断元素是否存在于集合中。
            //
            // HashSet<T> hashSet = new HashSet<T>();
            // hashSet.Add(item);
            // hashSet.Remove(item);
            // bool contains = hashSet.Contains(item);
            // int count = hashSet.Count;
            // 4. Queue<T>
            // Queue<T>是C#中的先进先出（FIFO）队列，用于按顺序处理元素。它提供了入队（Enqueue）和出队（Dequeue）操作，适用于需要按顺序处理元素的场景，如任务队列、消息队列等。Queue<T>还提供了其他常用方法，如Peek（查看队列头部元素）和Contains（判断元素是否存在于队列中）。
            //
            // Queue<T> queue = new Queue<T>();
            // queue.Enqueue(item);
            // T item = queue.Dequeue();
            // T item = queue.Peek();
            // bool isEmpty = queue.Count == 0;
            // 5. Stack<T>
            // Stack<T>是C#中的后进先出（LIFO）堆栈，用于反向顺序处理元素。它提供了入栈（Push）和出栈（Pop）操作，适用于需要反向处理元素的场景，如函数调用栈、撤销操作等。Stack<T>还提供了其他方法，如Peek（查看堆栈顶部元素）和Contains（判断元素是否存在于堆栈中）。
            //
            // Stack<T> stack = new Stack<T>();
            // stack.Push(item);
            // T item = stack.Pop();
            // T item = stack.Peek();
            // bool isEmpty = stack.Count == 0;
            // 6. Tuple<T1, T2, ...>
            // Tuple<T1, T2, ...>是C#中用于组合多个不同类型值的结构体。它允许将多个值作为单个对象进行处理，而无需创建新的自定义类。Tuple<T1, T2, ...>的长度可以根据需要自由扩展，每个元素可以具有不同的类型。Tuple在需要临时组合多个值时非常方便，如方法返回多个值、传递多个参数等。
            //
            // Tuple<T1, T2, ...> tuple = new Tuple<T1, T2, ...>(value1, value2, ...);
            // T1 value1 = tuple.Item1;
            // T2 value2 = tuple.Item2;
            // ...
            // 7. 比较与选择适当的数据结构
            // 在选择适当的数据结构时，需考虑以下几个因素：
            //
            // 数据类型：确定要存储的数据类型和数据结构的兼容性。List<T>、Dictionary<TKey, TValue>、HashSet<T>适合存储同一类型的元素，而Tuple<T1, T2, ...>则适用于不同类型的元素组合。
            // 数据访问方式：根据需要进行元素的查找、添加和删除操作。如果需要快速根据键进行查找，可以选择Dictionary<TKey, TValue>。如果需要按顺序处理元素，可以选择Queue<T>或Stack<T>。如果需要保持唯一性，可以选择HashSet<T>。
            // 数据操作的频率：根据数据的操作频率选择合适的数据结构。如果需要频繁地插入和删除元素，List<T>可能更适合。如果需要频繁地进行元素查找，Dictionary<TKey, TValue>或HashSet<T>可能更合适。
            // 内存占用和性能：根据应用程序的要求，评估不同数据结构的内存占用和性能特点。某些数据结构可能需要更多的内存或具有更高的性能开销。
            // 以下是一些常见的使用场景，对于每种数据结构都列举了适合的情况：
            // List<T>:
            // 存储和管理动态大小的元素集合。
            // 需要频繁地添加和删除元素。
            // 需要按索引访问元素。
            // 需要对元素进行排序、搜索和过滤。
            // Dictionary<TKey, TValue>:
            // 快速查找和访问键值对。
            // 存储具有唯一键的数据。
            // 构建字典、映射和缓存等应用。
            // 需要高效的键值查找操作。
            // HashSet<T>:
            // 存储唯一元素，去重。
            // 快速判断元素是否存在于集合中。
            // 集合操作，如交集、并集和差集。
            // 需要高效的查找和去重操作。
            // Queue<T>:
            // 先进先出（FIFO）的数据处理。
            // 任务队列和消息队列的实现。
            // 广度优先搜索（BFS）算法。
            // 需要按顺序处理元素。
            // Stack<T>:
            // 后进先出（LIFO）的数据处理。
            // 函数调用栈的实现。
            // 撤销和回退操作。
            // 需要反向处理元素。
            // Tuple<T1, T2, ...>:
            // 临时组合多个不同类型的值。
            // 作为方法的返回类型，返回多个值。
            // 传递多个参数。
            //
            //
            // 当考虑数据结构的内存占用情况时，以下是对C#中常见数据结构的更详细分析：
            // List<T>：List<T>的内存占用受到两个主要因素的影响：列表中存储的元素数量和元素类型的大小。每个元素占用的内存取决于其数据类型的大小。列表会根据需要自动调整大小，因此它可能会分配比实际使用的内存更多的空间。如果列表的容量超过实际元素数量，它可能会占用更多的内存。当元素数量变化较大时，动态调整大小可能导致内存碎片化。
            // Dictionary<TKey, TValue>：Dictionary<TKey, TValue>的内存占用取决于存储的键值对数量和键值对中键和值的类型大小。每个键值对占用一定的内存空间，而键和值的数据类型大小也会影响内存占用。类似于List<T>，Dictionary<TKey, TValue>也会分配比实际使用的内存更多的空间以容纳未来的键值对。在大型字典中，内存占用可能会相对较高。
            // HashSet<T>：HashSet<T>的内存占用取决于存储的唯一元素数量和元素的数据类型大小。与Dictionary<TKey, TValue>类似，每个元素占用一定的内存空间，并且HashSet<T>也会分配比实际使用的内存更多的空间。在存储大量唯一元素时，内存占用可能较高。
            // Queue<T>和Stack<T>：Queue<T>和Stack<T>的内存占用取决于存储的元素数量和元素的数据类型大小。这两个数据结构的内部实现使用了数组或链表，因此内存占用与元素数量成正比。与List<T>相比，它们通常具有较少的内存占用，因为它们不需要为每个元素存储额外的容量。
            // Tuple<T1, T2, ...>：Tuple<T1, T2, ...>的内存占用取决于组合的元素数量和每个元素的数据类型大小。Tuple本质上是一个包含多个元素的对象，因此它的内存占用将受到元素数量和类型大小的影响。Tuple的内存占用相对较小，因为它仅存储了元素的值，而不需要额外的容量。
            // 需要注意的是，数据结构的内存占用也会受到CLR（Common Language Runtime）的优化和垃圾回收机制的影响。CLR会进行内存管理和优化，以最大程度地减少内存占用和提高性能。
            //
            // 在选择数据结构时，除了内存占用，还需要考虑以下因素：
            //
            // 性能：除了内存占用，不同的数据结构在插入、删除、查找和遍历等操作上可能具有不同的性能特点。某些数据结构在特定操作上可能更加高效，因此需要根据应用程序的需求选择适当的数据结构。
            // 功能和用法：不同的数据结构提供不同的功能和用法。例如，List<T>适用于动态添加和删除元素，而Dictionary<TKey, TValue>适用于快速查找元素。根据需要的功能和使用方式，选择合适的数据结构能够更好地满足需求。
            // 数据唯一性：如果需要存储唯一的元素，HashSet<T>是一个很好的选择，因为它会自动确保不包含重复项。而List<T>和Dictionary<TKey, TValue>则可以包含重复的元素。
            // 数据排序：如果需要对元素进行排序操作，List<T>和Dictionary<TKey, TValue>提供了相应的排序方法。HashSet<T>、Queue<T>和Stack<T>通常不保持元素的特定顺序。
            // 可读性和维护性：选择易于理解和维护的数据结构有助于代码的可读性和可维护性。考虑到代码的可读性，选择能够直观表达意图的数据结构。
            // 综上所述，除了内存占用，选择适当的数据结构应综合考虑性能、功能、用法、数据唯一性、排序需求、可读性和维护性等因素。根据具体的应用需求，选择最适合的数据结构能够提高代码的效率和可维护性。
        }
    }
}
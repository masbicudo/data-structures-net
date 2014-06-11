namespace DataStructures.Tests.TestModels
{
    internal class TreeItemData<T> : TreeItemData
    {
        public TreeItemData(int id, int? parentId, T value)
            : base(id, parentId, value)
        {
            this.Value = value;
        }

        public new T Value { get; private set; }
    }

    internal abstract class TreeItemData
    {
        public int Id { get; private set; }

        public int? ParentId { get; private set; }

        public object Value { get; private set; }

        protected TreeItemData(int id, int? parentId, object value)
        {
            this.Value = value;
            this.Id = id;
            this.ParentId = parentId;
        }

        public static TreeItemData<T> Create<T>(int id, int? parentId, T value)
        {
            return new TreeItemData<T>(id, parentId, value);
        }
    }
}
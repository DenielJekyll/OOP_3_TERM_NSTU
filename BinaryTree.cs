namespace Tree
{

    public enum BinSide
    {
        Left,
        Right
    }
    // Бинарное дерево поиска
    public class BinaryTree
    {

        public dynamic Data { get; private set; }
        public BinaryTree Left { get; set; }
        public BinaryTree Right { get; set; }
        public BinaryTree Parent { get; set; }
        private long countElements = 0;
        // Вставляет целочисленное значение в дерево
        public void Insert(dynamic data)
        {
            countElements += 1;
            if (Data == null)
            {
                Data = data;
                return;
            }
            if (Data.R <= data.R)
            {
                if (Right == null) Right = new BinaryTree();
                Insert(data, Right, this);
                return;
            }
            else
            {
                if (Left == null) Left = new BinaryTree();
                Insert(data, Left, this);
            }
        }

        // Вставляет значение в определённый узел дерева
        private void Insert(dynamic data, BinaryTree node, BinaryTree parent)
        {
            if (node.Data == null)
            {
                node.Data = data;
                node.Parent = parent;
                return;
            }
            if (node.Data.R <= data.R)
            {
                if (node.Right == null) node.Right = new BinaryTree();
                Insert(data, node.Right, node);
            }
            else
            {
                if (node.Left == null) node.Left = new BinaryTree();
                Insert(data, node.Left, node);
            }
        }

        // Уставляет узел в определённый узел дерева
        private void Insert(BinaryTree data, BinaryTree node, BinaryTree parent)
        {
            if (node.Data == null)
            {
                node.Data = data.Data;
                node.Left = data.Left;
                node.Right = data.Right;
                node.Parent = parent;
                return;
            }
            if (node.Data.R <= data.Data.R)
            {
                if (node.Right == null) node.Right = new BinaryTree();
                Insert(data, node.Right, node);
            }
            else
            {
                if (node.Left == null) node.Left = new BinaryTree();
                Insert(data, node.Left, node);
            }
        }
       
        /// Определяет, в какой ветви для родительского лежит данный узел
        private BinSide? MeForParent(BinaryTree node)
        {
            if (node.Parent == null) return null;
            if (node.Parent.Left == node) return BinSide.Left;
            if (node.Parent.Right == node) return BinSide.Right;
            return null;
        }
        
        private void Remove(BinaryTree node)
        {
            countElements -= 1;
            if (node == null) return;
            var me = MeForParent(node);

            if (me == null)
            {
                BinaryTree bufRightLeft = null;
                BinaryTree bufRightRight = null;
                node.Data = null;
                if (node.Right != null)
                {
                    bufRightLeft = node.Right.Left;
                    bufRightRight = node.Right.Right;
                    node.Data = node.Right.Data;
                }
                var bufLeft = node.Left;
                node.Right = bufRightRight;
                node.Left = bufRightLeft;
                if (bufLeft != null)
                    Insert(bufLeft, node, node);
                return;
            }
            //Если у узла нет дочерних элементов, его можно смело удалять
            if (node.Left == null && node.Right == null)
            {
                if (me == BinSide.Left)
                    node.Parent.Left = null;
                else
                    node.Parent.Right = null;
                return;
            }
            //Если нет левого дочернего, то правый дочерний становится на место удаляемого
            if (node.Left == null)
            {
                if (me == BinSide.Left)
                    node.Parent.Left = node.Right;
                else
                    node.Parent.Right = node.Right;
                node.Right.Parent = node.Parent;
                return;
            }
            //Если нет правого дочернего, то левый дочерний становится на место удаляемого
            if (node.Right == null)
            {
                if (me == BinSide.Left)
                    node.Parent.Left = node.Left;
                else
                    node.Parent.Right = node.Left;
                node.Left.Parent = node.Parent;
                return;
            }

            //Если присутствуют оба дочерних узла, то правый ставим на место удаляемого, а левый вставляем в правый
            if (me == BinSide.Left)
                node.Parent.Left = node.Right;
            if (me == BinSide.Right)
                node.Parent.Right = node.Right;
            if (me == null)
            {
                var bufLeft = node.Left;
                var bufRightLeft = node.Right.Left;
                var bufRightRight = node.Right.Right;
                node.Data = node.Right.Data;
                node.Right = bufRightRight;
                node.Left = bufRightLeft;
                Insert(bufLeft, node, node);
            }
            else
            {
                node.Right.Parent = node.Parent;
                Insert(node.Left, node.Right, node.Right);
            }
        }
        // Удаляет значение из дерева
        public void Remove(double R, uint index)
        {
            var removeNode = Find(R, index);
            if (removeNode != null) Remove(removeNode);
        }
        // Ищет узел с заданным значением
        public BinaryTree Find(double R, uint index)
        {
            if (Data.R == R && Data.index == index) return this;
            if (Data.R > R) return Find(R, Left, index);
            return Find(R, Right, index);
        }
        // Ищет значение в определённом узле
        private BinaryTree Find(double R, BinaryTree node, uint index)
        {
            if (node == null) return null;
            if (node.Data.R == R && node.Data.index == index) return node;
            if (node.Data.R > R) return Find(R, node.Left, index);
            return Find(R, node.Right, index);
        }
        
        /// Количество элементов в дереве
        public long CountElements()
        {
            return countElements;
        }
    }
}

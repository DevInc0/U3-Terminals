using System.Collections.Generic;

namespace Terminals
{
    public struct Storage
    {
        public List<StoredItem> storedItems;

        public List<PlayerShoppingBasket> baskets;

        public Storage(List<StoredItem> _storedItems = null, List<PlayerShoppingBasket> _baskets = null)
        {
            storedItems = _storedItems;
            baskets = _baskets;
        }
    }
}

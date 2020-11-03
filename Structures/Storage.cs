using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

import { useState, useEffect } from "react";
import AppBar from "../components/AppBar";
import Sidebar from "../components/Sidebar";
import Footer from "../components/Footer";
import { Navigate, useNavigate } from "react-router-dom";

async function updateCartDetail(cartDetail) {
  const { product, ...rest } = cartDetail;
  rest.product = null;
  try {
    const response = await fetch('http://localhost:5124/api/cart/update', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('token')}`
      },
      body: JSON.stringify(rest)
    });

    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
  } catch (error) {
    console.error('Error updating cart detail:', error);
    throw error;
  }
}

export default function CartPage({ removeFromCart, isLoggedIn }) {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);
  const [selections, setSelections] = useState({});
  const [totalPrice, setTotalPrice] = useState(0);
  const [cartItems, setCartItems] = useState([]);
  const customer = JSON.parse(sessionStorage.getItem("customer"));
  const navigate = useNavigate();

  const toggleSidebar = () => setIsSidebarOpen(!isSidebarOpen);
  const closeSidebar = () => setIsSidebarOpen(false);

  if (!isLoggedIn) return <Navigate to="/login" />;

  const handleSelectionChange = async (productId, isRented) => {
    const updatedItem = cartItems.find(item => item.productId === productId);
    const product = updatedItem.product;
    const rentNoOfDays = isRented ? (selections[productId]?.rentNoOfDays || product.minRentDays) : 0;

    setSelections((prev) => ({
      ...prev,
      [productId]: { ...prev[productId], isRented, rentNoOfDays },
    }));

    const updatedCartItem = { ...updatedItem, isRented, rentNoOfDays };

    setCartItems((prevItems) => 
      prevItems.map(item => 
        item.productId === productId ? updatedCartItem : item
      )
    );

    try {
      await updateCartDetail(updatedCartItem);
    } catch (error) {
      console.error('Error updating cart item:', error);
    }
  };

  const handleDaysChange = async (productId, rentNoOfDays) => {
    const updatedItem = cartItems.find(item => item.productId === productId);
    const product = updatedItem.product;
    const minDays = product.minRentDays;
    const updatedDays = Math.max(parseInt(rentNoOfDays, 10) || minDays, minDays);

    setSelections((prev) => ({
      ...prev,
      [productId]: { ...prev[productId], rentNoOfDays: updatedDays },
    }));

    updatedItem.rentNoOfDays = updatedDays;

    setCartItems((prevItems) => 
      prevItems.map(item => 
        item.productId === productId ? updatedItem : item
      )
    );

    try {
      await updateCartDetail(updatedItem);
    } catch (error) {
      console.error('Error updating cart item:', error);
    }
  };

  const isProductRentable = (product) => {
    return product.product.isRentable;
  };

  useEffect(() => {
    let total = 0;
    cartItems.forEach((item) => {
      const selection = selections[item.product.productId] || {};
      if (selection.isRented) {
        total += (item.product.productBasePrice / 10) * (selection.rentNoOfDays || 1);
      } else {
        total += item.product.productBasePrice;
      }
    });
    setTotalPrice(total);
  }, [cartItems, selections]);

  useEffect(() => {
    const fetchCartItems = async () => {
      try {
        const response = await fetch(`http://localhost:5124/api/cart/${customer.customerId}`,
          {
            method:"GET",
            headers:{
              'Authorization': `Bearer ${sessionStorage.getItem('token')}`
            },
          }
        );
        const data = await response.json();
        setCartItems(data);

        // Initialize selections based on fetched cart items
        const initialSelections = {};
        data.forEach(item => {
          initialSelections[item.productId] = {
            isRented: item.isRented,
            rentNoOfDays: item.rentNoOfDays || item.product.minRentDays
          };
        });
        setSelections(initialSelections);
      } catch (error) {
        console.error('Error fetching cart items:', error);
      }
    };

    fetchCartItems();
  }, []);

  const handleRemoveFromCart = async (productId) => {
    try {
      await removeFromCart(productId);
      setCartItems((prevItems) => prevItems.filter(item => item.productId !== productId));
    } catch (error) {
      console.error('Error removing item from cart:', error);
    }
  };

  const handleProceedToPayment = () => {
    fetch(`http://localhost:5124/api/invoice/generate-invoice/${customer.customerId}`, {
      method: 'GET',
      headers: {
      'Authorization': `Bearer ${sessionStorage.getItem('token')}`
      }
    })
    .then(response => {
      if (!response.ok) {
      throw new Error('Network response was not ok');
      }
      return response.json();
    })
    .then(data => {
      console.log('Checkout successful:', data);
      // navigate("/myshelf"); // Redirect to My Shelf
    })
    .catch(error => {
      console.error('Error during checkout:', error);
    });
  };

  return (
    <div className="flex flex-col min-h-screen">
      <AppBar toggleSidebar={toggleSidebar} isLoggedIn={isLoggedIn} />
      <Sidebar isOpen={isSidebarOpen} closeSidebar={closeSidebar} isLoggedIn={isLoggedIn} />
      <main className="flex-grow p-4 mt-16">
        <div className="container mx-auto">
          <h1 className="text-3xl font-bold text-gray-800 mb-8">Your Cart</h1>
          {cartItems.length === 0 ? (
            <p className="text-gray-600">Your cart is empty.</p>
          ) : (
            <div className="space-y-4">
              {cartItems.map((item) => (
                <div key={item.productId} className="bg-white p-4 rounded-lg shadow-md">
                  <div className="flex items-center justify-between">
                    <div>
                      <h3 className="text-xl font-bold text-gray-800">{item.product.productEnglishName}</h3>
                      <p className="text-gray-600">Price: ${item.product.productBasePrice}</p>
                      <p className="text-gray-600">{item.product.productDescriptionShort}</p>
                    </div>
                    <div className="flex flex-col items-end space-y-2">
                      {/* Conditional Dropdown */}
                      {isProductRentable(item) && (
                        <select
                          className="border rounded-lg px-2 py-1"
                          onChange={(e) => handleSelectionChange(item.productId, e.target.value === "rent")}
                          value={selections[item.productId]?.isRented ? "rent" : "purchase"}
                        >
                          <option value="purchase">Purchase</option>
                          <option value="rent">Rent</option>
                        </select>
                      )}
                      {/* Rental Days Input */}
                      {selections[item.productId]?.isRented && (
                        <input
                          type="number"
                          min={item.product.minRentDays}
                          className="border rounded-lg px-2 py-1 w-20"
                          value={selections[item.productId]?.rentNoOfDays || item.product.minRentDays}
                          onChange={(e) => handleDaysChange(item.productId, e.target.value)}
                        />
                      )}
                      {/* Remove Button */}
                      <button
                        onClick={() => handleRemoveFromCart(item.productId)}
                        className="text-red-500 hover:text-red-700 font-medium px-3 py-1 border border-red-500 rounded-lg"
                      >
                        Remove
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
          {/* Total Price & Payment Button */}
          {cartItems.length > 0 && (
            <div className="mt-8 p-4 bg-gray-100 rounded-lg shadow-md flex justify-between items-center">
              <h2 className="text-xl font-bold text-gray-800">Total Price: ${totalPrice.toFixed(2)}</h2>
              <button 
                onClick={handleProceedToPayment}
                className="bg-blue-500 text-white font-semibold px-4 py-2 rounded-lg hover:bg-blue-600"
              >
                Proceed to Payment
              </button>
            </div>
          )}
        </div>
      </main>
      <Footer />
    </div>
  );
}

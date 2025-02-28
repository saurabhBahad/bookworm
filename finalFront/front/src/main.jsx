import React, { useState, useCallback, useMemo, useEffect } from "react";
import ReactDOM from "react-dom/client";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
  useNavigate,
} from "react-router-dom";
import App from "./App";
import LoginPage from "./pages/LoginPage";
import SignupPage from "./pages/SignupPage";
import HomePage from "./pages/HomePage";
import EBookPage from "./pages/EBookPage";
import AudioPage from "./pages/AudioPage";
import CartPage from "./pages/CartPage";
import ProfilePage from "./pages/ProfilePage";
import AboutUs from "./pages/AboutUs";
import Myshelf from "./pages/Myshelf";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./index.css";

function Main() {
  const [isLoggedIn, setIsLoggedIn] = useState(
    !!sessionStorage.getItem("token")
  );
  const [cartItems, setCartItems] = useState([]);
  const [myshelfItems, setMyshelfItems] = useState([]);

  useEffect(() => {
    const token = sessionStorage.getItem("token");
    if (token) {
      setIsLoggedIn(true);
    }
  }, []);

  const handleLogin = useCallback((token) => {
    sessionStorage.setItem("token", token);
    setIsLoggedIn(true);
    console.log(isLoggedIn);

    console.log("logged in");
  }, []);

  const handleLogout = useCallback(() => {
    setIsLoggedIn(false);
    sessionStorage.removeItem("token");
    sessionStorage.removeItem("customer");
  }, []);

  const addToCart = useCallback((item , isRented , noOfDays) => {
    const customer = JSON.parse(sessionStorage.getItem("customer"));
    const cartHelper = {
      "custId":customer.customerId,
      "prodId":item.productId,
      "isRented":isRented ?? false,
      "noOfDays":noOfDays ?? 0
    }
    // console.log(cartHelper);
    setCartItems((prevItems) => {
      fetch(`http://localhost:5124/api/cart/cart`, { 
        method: "POST",
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${sessionStorage.getItem("token")}`,
        },
        body:JSON.stringify(cartHelper),
      })
      .then(res => {
        if (res.ok){
          toast.success(`${item.productEnglishName} added to cart!`, {
            position: "bottom-right",
            autoClose: 3000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
          });
        }else{
          throw new Error("Unable to add the product")
        }
      })
      .catch((error) => {
        // console.error('Error:', error);
        toast.error(`${item.productEnglishName} not added to cart!`, {
          position: "bottom-right",
          autoClose: 3000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
        });
        return;
      });
      return [...prevItems, item]; 
    });
  }, []);

  const removeFromCart = useCallback((id) => {
    const customer = JSON.parse(sessionStorage.getItem("customer"));
    fetch(`http://localhost:5124/api/cart/${customer.customerId}/product/${id}`, {
      method: "DELETE",
      headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${sessionStorage.getItem("token")}`,
      },
    })
    .then(response => {
      if (response.ok) {
      setCartItems((prevItems) => prevItems.filter((item) => item?.product?.productId !== id));
      toast.success("Item removed from cart!", {
        position: "bottom-right",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
      });
      } else {
      throw new Error('Failed to remove item');
      }
    })
    .catch((error) => {
      toast.error("Failed to remove item from cart!", {
      position: "bottom-right",
      autoClose: 3000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      });
    });
  }, []);

  const proceedToPayment = useCallback(() => {
    setMyshelfItems((prev) =>{
       const updatedMyshelf = [...prev, ...cartItems];
    return updatedMyshelf;
  });
    setCartItems([]);
    toast.success("Payment successful! Books added to My Shelf.", {
      position: "bottom-right",
      autoClose: 3000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
    });
  }, []);

  useEffect(() => {
    const savedMyshelf = JSON.parse(localStorage.getItem("myshelfItems")) || [];
    setMyshelfItems(savedMyshelf);
  }, []);

  const routes = useMemo(
    () => (
      <Routes>
        <Route
          path="/"
          element={
            isLoggedIn ? (
              <HomePage onLogout={handleLogout} isLoggedIn={isLoggedIn} />
            ) : (
              <App isLoggedIn={isLoggedIn} />
            )
          }
        />
        <Route path="/login" element={<LoginPage onLogin={handleLogin} />} />
        <Route path="/register" element={<SignupPage />} />
        <Route path="/profile" element={<ProfilePage isLoggedIn = {isLoggedIn}/>} />
        <Route path="/aboutus" element={<AboutUs />} />
        <Route path="/myshelf" element={<Myshelf myshelfItems={myshelfItems} isLoggedIn={isLoggedIn} onLogout={handleLogout}/>}/>
        <Route
          path="/ebook"
          element={<EBookPage addToCart={addToCart} isLoggedIn={isLoggedIn} onLogout={handleLogout}/>}
        />
        <Route
          path="/audio"
          element={<AudioPage addToCart={addToCart} isLoggedIn={isLoggedIn} onLogout={handleLogout}/>}
        />
        <Route
          path="/cart"
          element={
            isLoggedIn ? (
              <CartPage
                cartItems={cartItems}
                removeFromCart={removeFromCart}
                proceedToPayment={proceedToPayment}
                isLoggedIn={isLoggedIn}
              />
            ) : (
              <Navigate to="/login" />
            )
          }
        />
      </Routes>
    ),
    [
      isLoggedIn,
      handleLogout,
      handleLogin,
      addToCart,
      removeFromCart,
      proceedToPayment,
      cartItems,
      myshelfItems,
    ]
  );

  return (
    <>
      <ToastContainer />
      <Router>{routes}</Router>
    </>
  );
}

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <Main />
  </React.StrictMode>
);

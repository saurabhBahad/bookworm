import React, { useState } from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import App from "./App";
import LoginPage from "./pages/LoginPage";
import SignupPage from "./pages/SignupPage";
import HomePage from "./pages/HomePage";
import EBookPage from "./pages/EBookPage";
import AudioPage from "./pages/AudioPage";
import CartPage from "./pages/CartPage";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./index.css";

function Main() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [cartItems, setCartItems] = useState([]);

  const handleLogin = () => {
    setIsLoggedIn(true);
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
  };

  const addToCart = (item) => {
    setCartItems([...cartItems, item]);
    toast.success(`${item.title} added to cart!`, {
      position: "bottom-right",
      autoClose: 3000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
    });
  };

  const removeFromCart = (id) => {
    setCartItems(cartItems.filter((item) => item.id !== id));
  };

  <ToastContainer />
  return (
    <Router>
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
        <Route
          path="/login"
          element={<LoginPage onLogin={handleLogin} />}
        />
        <Route path="/register" element={<SignupPage />} />
        <Route
          path="/ebook"
          element={<EBookPage addToCart={addToCart} isLoggedIn={isLoggedIn} />}
        />
        <Route
          path="/audio"
          element={<AudioPage addToCart={addToCart} isLoggedIn={isLoggedIn} />}
        />
        <Route
          path="/cart"
          element={
            isLoggedIn ? (
              <CartPage cartItems={cartItems} removeFromCart={removeFromCart} />
            ) : (
              <Navigate to="/login" />
            )
          }
        />
      </Routes>
    </Router>
  );
}

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <Main />
  </React.StrictMode>
);
import { useState, useEffect } from "react";
import AppBar from "../components/AppBar";
import Sidebar from "../components/Sidebar";
import Footer from "../components/Footer";
import { useNavigate } from "react-router-dom";

export default function Myshelf({ onLogout, isLoggedIn }) {
  const navigate = useNavigate();
  const [myshelfItems, setMyshelfItems] = useState([]);
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  useEffect(() => {
    if (!isLoggedIn) {
      navigate("/login");
    } else {
      const shelfId = JSON.parse(sessionStorage.getItem("customer")).shelfId;
      if (shelfId) {
        fetch(`http://localhost:5124/api/MyShelf/shelf-detail/${shelfId}`, {
          headers: {
            Authorization: `Bearer ${sessionStorage.getItem("token")}`
          }
        })
          .then((response) => {
            if (!response.ok) {
              throw new Error(`Error: ${response.status} ${response.statusText}`);
            }
            return response.json();
          })
          .then((data) => {
            console.log(data);
            if (Array.isArray(data)) {
              setMyshelfItems(data);
            } else {
              throw new Error("Invalid data format");
            }
          })
          .catch((error) => {
            console.error(error.message);
          });
      }
    }
  }, [isLoggedIn, navigate]);

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  const closeSidebar = () => {
    setIsSidebarOpen(false);
  };

  return (
    <div className="flex flex-col min-h-screen">
      <AppBar toggleSidebar={toggleSidebar} isLoggedIn={isLoggedIn} onLogout={onLogout} />
      <Sidebar isOpen={isSidebarOpen} closeSidebar={closeSidebar} isLoggedIn={isLoggedIn} />
      <main className="flex-grow p-4 mt-16">
        <div className="container mx-auto">
          <h1 className="text-3xl font-bold text-gray-800 mb-8">My Shelf</h1>
          {myshelfItems.length === 0 ? (
            <p className="text-gray-600">Your shelf is empty.</p>
          ) : (
            <div className="space-y-4">
              {myshelfItems.map((item) => (
                <div key={item.shelfDtlId} className="bg-white p-4 rounded-lg shadow-md">
                  <h3 className="text-xl font-bold text-gray-800">{item.product.productEnglishName}</h3>
                  <p className="text-gray-600">Price: ${item.basePrice}</p>
                  <p className="text-gray-600">{item.product.productDescriptionShort}</p>
                  <p className="text-gray-600">Transaction Type: {item.tranType}</p>
                </div>
              ))}
            </div>
          )}
        </div>
      </main>
      <Footer />
    </div>
  );
}

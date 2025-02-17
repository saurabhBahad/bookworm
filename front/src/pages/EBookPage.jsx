import { useState } from "react";
import AppBar from "../components/AppBar";
import Sidebar from "../components/Sidebar";
import Footer from "../components/Footer";
import Card from "../components/Card";

export default function EBookPage({ addToCart, isLoggedIn }) {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  const closeSidebar = () => {
    setIsSidebarOpen(false);
  };

  // Sample data for e-books
  const ebooks = [
    {
      id: 1,
      image: "https://via.placeholder.com/300x200",
      title: "The Great E-Book",
      author: "John Doe",
      description: "A fascinating e-book that takes you on a journey through the world of literature.",
    },
    {
      id: 2,
      image: "https://via.placeholder.com/300x200",
      title: "Another Awesome E-Book",
      author: "Jane Smith",
      description: "An inspiring story of courage and adventure.",
    },
    {
      id: 3,
      image: "https://via.placeholder.com/300x200",
      title: "Learn React in 30 Days",
      author: "Alice Johnson",
      description: "A step-by-step guide to mastering React.",
    },
  ];

  return (
    <div className="flex flex-col min-h-screen">
      <AppBar toggleSidebar={toggleSidebar} isLoggedIn={isLoggedIn} />
      <Sidebar isOpen={isSidebarOpen} closeSidebar={closeSidebar} isLoggedIn={isLoggedIn} />
      <main className="flex-grow p-4 mt-16">
        <div className="container mx-auto">
          <h1 className="text-3xl font-bold text-gray-800 mb-8">E-Books</h1>

          {/* Grid of E-Books */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {ebooks.map((ebook) => (
              <div key={ebook.id} className="w-full">
                <Card
                  image={ebook.image}
                  title={ebook.title}
                  author={ebook.author}
                  description={ebook.description}
                  onAddToCart={() => addToCart(ebook)}
                />
              </div>
            ))}
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
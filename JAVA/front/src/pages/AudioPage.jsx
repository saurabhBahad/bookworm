import { useState } from "react";
import AppBar from "../components/AppBar";
import Sidebar from "../components/Sidebar";
import Footer from "../components/Footer";
import Card from "../components/Card";

export default function AudioPage({ addToCart, isLoggedIn }) {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  const closeSidebar = () => {
    setIsSidebarOpen(false);
  };

  // Sample data for audio items
  const audioItems = [
    {
      id: 1,
      image: "https://via.placeholder.com/300x200",
      title: "Amazing Audio Book",
      author: "Jane Smith",
      description: "A masterpiece of storytelling. Perfect for long drives or relaxing evenings.",
    },
    {
      id: 2,
      image: "https://via.placeholder.com/300x200",
      title: "Relaxing Music",
      author: "John Doe",
      description: "Soothing melodies to help you unwind and relax.",
    },
    {
      id: 3,
      image: "https://via.placeholder.com/300x200",
      title: "Podcast on Technology",
      author: "Alice Johnson",
      description: "Explore the latest trends in technology and innovation.",
    },
  ];

  return (
    <div className="flex flex-col min-h-screen">
      <AppBar toggleSidebar={toggleSidebar} isLoggedIn={isLoggedIn} />
      <Sidebar isOpen={isSidebarOpen} closeSidebar={closeSidebar} isLoggedIn={isLoggedIn} />
      <main className="flex-grow p-4 mt-16">
        <div className="container mx-auto">
          <h1 className="text-3xl font-bold text-gray-800 mb-8">Audio</h1>

          {/* Grid of Audio Items */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {audioItems.map((audio) => (
              <div key={audio.id} className="w-full">
                <Card
                  image={audio.image}
                  title={audio.title}
                  author={audio.author}
                  description={audio.description}
                  onAddToCart={() => addToCart(audio)}
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
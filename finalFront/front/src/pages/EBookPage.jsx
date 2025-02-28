import { useState, useEffect } from "react";
import AppBar from "../components/AppBar";
import Sidebar from "../components/Sidebar";
import Footer from "../components/Footer";
import Card from "../components/Card";
import {useNavigate } from "react-router-dom";

const LANGUAGES_API_URL = "http://localhost:5124/api/languages/";
const GENRES_API_URL = "http://localhost:5124/api/genre/";
const PRODUCTS_API_URL = "http://localhost:5124/api/productmaster/search";

export default function EBookPage({ addToCart, isLoggedIn ,onLogout}) {
  const navigate = useNavigate();
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);
  const [ebooks, setEbooks] = useState([]);
  const [filteredEbooks, setFilteredEbooks] = useState([]);
  const [languages, setLanguages] = useState([]);
  const [genres, setGenres] = useState([]);
  const [languageFilter, setLanguageFilter] = useState("");
  const [genreFilter, setGenreFilter] = useState("");
  const [nameFilter, setNameFilter] = useState("");
  const [authorFilter, setAuthorFilter] = useState("");

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  const closeSidebar = () => {
    setIsSidebarOpen(false);
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (!isLoggedIn) {
          navigate("/login");
          return;
        }

        const [languagesResponse, genresResponse] = await Promise.all([
          fetch(LANGUAGES_API_URL,{
            headers:{
              Authorization: `Bearer ${sessionStorage.getItem("token")}`
            }
          }),
          fetch(GENRES_API_URL,{
            headers:{
              Authorization: `Bearer ${sessionStorage.getItem("token")}`
            }
          }),
        ]);

        if (!languagesResponse.ok || !genresResponse.ok) {
          throw new Error("Failed to fetch languages or genres");
        }

        const languagesData = await languagesResponse.json();
        const genresData = await genresResponse.json();

        setLanguages(languagesData ?? []);
        setGenres(genresData ?? []);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  useEffect(() => {
    const fetchEbooks = async () => {
      try {
        const filter = {
          langId: languageFilter || 0,
          genreId: genreFilter || 0,
          typeId: 1,
          productName: nameFilter || "",
          authorName: authorFilter || "",
        };

        const response = await fetch(PRODUCTS_API_URL, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${sessionStorage.getItem("token")}`
          },
          body: JSON.stringify(filter),
        });

        const data = await response.json();
        setEbooks(data);
        setFilteredEbooks(data); // Set filteredEbooks directly
      } catch (error) {
        console.error("Error fetching e-books:", error);
      }
    };

    fetchEbooks();
  }, [languageFilter, genreFilter, nameFilter, authorFilter]);

  return (
    <div className="flex flex-col min-h-screen">
      <AppBar toggleSidebar={toggleSidebar} isLoggedIn={isLoggedIn} onLogout={onLogout}/>
      <Sidebar
        isOpen={isSidebarOpen}
        closeSidebar={closeSidebar}
        isLoggedIn={isLoggedIn}
      />
      <main className="flex-grow p-4 mt-16">
        <div className="container mx-auto">
          <h1 className="text-3xl font-bold text-gray-800 mb-8">E-Books</h1>

          {/* Filters */}
          <div className="mb-8">
            <input
              type="text"
              placeholder="Filter by product name"
              value={nameFilter}
              onChange={(e) => setNameFilter(e.target.value)}
              className="border p-2 mr-4"
            />
            <select
              value={languageFilter}
              onChange={(e) => setLanguageFilter(e.target.value)}
              className="border p-2 mr-4"
            >
              <option value="">All Languages</option>
              {languages.map((language) => (
                <option key={language.languageId} value={language.languageId}>
                  {language.languageDesc}
                </option>
              ))}
            </select>
            <select
              value={genreFilter}
              onChange={(e) => setGenreFilter(e.target.value)}
              className="border p-2 mr-4"
            >
              <option value="">All Genres</option>
              {genres.map((genre) => (
                <option key={genre.genreId} value={genre.genreId}>
                  {genre.genreDesc}
                </option>
              ))}
            </select>
            <input
              type="text"
              placeholder="Filter by author"
              value={authorFilter}
              onChange={(e) => setAuthorFilter(e.target.value)}
              className="border p-2"
            />
          </div>

          {/* Grid of E-Books */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredEbooks.map((ebook) => (
              <div key={ebook.productId} className="w-full">
                {/* {console.log(ebook.productId)} */}
                <Card
                  image={ebook.path} 
                  title={ebook.productEnglishName}
                  description={ebook.productDescriptionShort}
                  price={ebook.productBasePrice}
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

import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import LandingPage from "./pages/LandingPage";
import Login from "./pages/Login";
import SignUp from "./pages/SignUp";
import PrivateRoute from "./components/PrivateRoute";
import Profile from "./pages/Profile";
import ArtworkPage from "./pages/ArtworkPage";
import NotFound from "./pages/NotFound";
import GalleryPage from "./pages/GalleryPage";
import CityPage from "./pages/CityPage";
import DiscoverPage from "./pages/DiscoverPage";
import FollowingPage from "./pages/FollowingPage";
import ChatPage from "./pages/ChatPage";
import MapPage from "./pages/MapPage";
import { Bounce, ToastContainer } from "react-toastify";

function App() {
  return (
    <BrowserRouter>
      <div className="App">
        <ToastContainer
          position="bottom-right"
          autoClose={5000}
          hideProgressBar
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="light"
          transition={Bounce}
        />

        <Routes>
          <Route path="/" element={<LandingPage />} />
          <Route path="/login" element={<Login />} />
          <Route path="/sign-up" element={<SignUp />} />
          <Route
            path="/artwork/new"
            element={
              <PrivateRoute>
                <ArtworkPage isNew />
              </PrivateRoute>
            }
          />
          <Route
            path="/artwork/:artworkId"
            element={
              <PrivateRoute>
                <ArtworkPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/gallery/:galleryId"
            element={
              <PrivateRoute>
                <GalleryPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/city/:cityId"
            element={
              <PrivateRoute>
                <CityPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/discover"
            element={
              <PrivateRoute>
                <DiscoverPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/following"
            element={
              <PrivateRoute>
                <FollowingPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/chat"
            element={
              <PrivateRoute>
                <ChatPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/map"
            element={
              <PrivateRoute>
                <MapPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/:username"
            element={
              <PrivateRoute>
                <Profile />
              </PrivateRoute>
            }
          />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;

import { BrowserRouter, Route, Routes } from "react-router-dom";
import LoginLayout from "./layouts/LoginLayout";
import MainLayout from "./layouts/MainLayout";
function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<LoginLayout />} />
        {/* <Route path="/" element={<MainLayout />} /> */}
      </Routes>
    </BrowserRouter>
  );
}

export default App;

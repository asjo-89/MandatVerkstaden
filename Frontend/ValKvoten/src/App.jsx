import { BrowserRouter, Route, Routes } from "react-router-dom";
import LoginLayout from "./layouts/LoginLayout";
import MainLayout from "./layouts/MainLayout";
import LoginForm from "./components/forms/LoginForm";
import ProtectedRoute from "./components/helpers/ProtectedRoute";
import Dashboard from "./pages/Dashboard";
import { RootRedirect } from "./components/helpers/RootRedirect";


function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<RootRedirect />} />
        
        <Route element={<LoginLayout />}>
          <Route path="/login" element={<LoginForm />} />
        </Route>

        <Route element={<ProtectedRoute />}>
          <Route element={<MainLayout />}>
            <Route path="/dashboard" element={<Dashboard />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;

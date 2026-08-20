import { BrowserRouter, Route, Routes } from "react-router-dom";
import LoginLayout from "./layouts/LoginLayout";
import RegisterForm from "./components/forms/RegisterForm";
import MainLayout from "./layouts/MainLayout";
import LoginForm from "./components/forms/LoginForm";
import ProtectedRoute from "./components/helpers/ProtectedRoute";
import Dashboard from "./pages/Dashboard";
import { RootRedirect } from "./components/helpers/RootRedirect";
import Scenario from "./pages/Scenario";


function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<RootRedirect />} />
        
        <Route element={<LoginLayout />}>
          <Route path="/login" element={<LoginForm />} />
          <Route path="/register" element={<RegisterForm />} />
        </Route>

        <Route element={<ProtectedRoute />}>
          <Route element={<MainLayout />}>
            <Route path="/dashboard" element={<Dashboard />} />
            <Route path="/scenario" element={<Scenario />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;

import { BrowserRouter, Route, Routes } from "react-router-dom";
import { RootRedirect } from "./components/helpers/RootRedirect";

import LoginLayout from "./layouts/LoginLayout";
import RegisterForm from "./components/forms/RegisterForm";
import MainLayout from "./layouts/MainLayout";
import LoginForm from "./components/forms/LoginForm";
import ProtectedRoute from "./components/helpers/ProtectedRoute";
import Dashboard from "./pages/Dashboard";
import ElectionResults from "./pages/ElectionResults";


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
            <Route path="/electionResults" element={<ElectionResults />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
